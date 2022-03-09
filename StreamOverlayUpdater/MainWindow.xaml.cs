using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace StreamOverlayUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class File
        {
            public string name { get; set; }
            public string url { get; set; }
            public string md5 { get; set; }
            public string install_path { get; set; }
        }

        public class Updates
        {
            public List<File> files { get; set; }
        }

        static async Task<string> CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                var data = await System.IO.File.ReadAllBytesAsync(filename);
                using (MemoryStream stream = new MemoryStream(data))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
                }
            }
        }

        private void DownloadFile(Queue<File> urls)
        {
            if (urls.Any())
            {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFileCompleted += client_DownloadFileCompleted;

                var url = urls.Dequeue();
                Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Environment.CurrentDirectory, url.install_path)));
                client.DownloadFileAsync(new Uri((url.url)), Path.Combine(Environment.CurrentDirectory, url.install_path));
                tbFileName.Text = "Downloading: " + url.name;
                return;
            }

            tbProgress.Text = "Download Complete";
            tbFileName.Text = "";
            tbDownloaded.Text = "";
            using (var batFile = new StreamWriter(System.IO.File.Create("Update.bat")))
            {
                batFile.WriteLine("@ECHO OFF");
                batFile.WriteLine("TIMEOUT /t 1 /nobreak > NUL");
                batFile.WriteLine("TASKKILL /F /IM \"{0}\" > NUL", Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName));
                batFile.WriteLine("IF EXIST \"{0}\" MOVE \"{0}\" \"{1}\"", Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName) + ".upd", Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName));
                batFile.WriteLine("DEL \"%~f0\" & START \"\" /B \"{0}\"", "StreamOverlay.exe");
            }
            ProcessStartInfo startInfo = new ProcessStartInfo("Update.bat");
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            Process.Start(startInfo);
            Environment.Exit(0);
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // handle error scenario
                throw e.Error;
            }
            if (e.Cancelled)
            {
                // handle cancelled scenario
            }
            DownloadFile(files);
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            pbUpdates.Value = (double)e.BytesReceived / (double)e.TotalBytesToReceive;
            tbDownloaded.Text = "Downloaded: " + (e.BytesReceived / 1024).ToString("N0") + " KB from " + (e.TotalBytesToReceive / 1024).ToString("N0") + " KB";
        }

        async Task<string> HttpGetAsync(string URI)
        {
            try
            {
                HttpClient hc = new HttpClient();
                Task<System.IO.Stream> result = hc.GetStreamAsync(URI);

                System.IO.Stream vs = await result;
                using (StreamReader am = new StreamReader(vs, Encoding.UTF8))
                {
                    return await am.ReadToEndAsync();
                }
            }
            catch
            {
                return "error";
            }
        }

        async Task<Updates> CheckUpdates()
        {
            string json = await HttpGetAsync("https://raw.githubusercontent.com/VladTheJunior/StreamOverlayUpdates/master/Updates.json");
            Updates res = new Updates();
            
            res.files = new List<File>();
            try
            {
                res = JsonConvert.DeserializeObject<Updates>(json);
            }
            catch
            {
            }
            return res;
        }

        Queue<File> files = new Queue<File>();
        public MainWindow()
        {
            InitializeComponent();
            Cursor = new Cursor(Application.GetResourceStream(new Uri("pack://application:,,,/resources/Cursor.cur")).Stream);

        }



        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Updates client = new Updates();
            client.files = new List<File>();
            foreach (string path in Directory.GetFiles(Environment.CurrentDirectory, "*.*", SearchOption.AllDirectories))
            {
                if (!path.Contains(".git")  && Path.GetFileName(Path.GetDirectoryName(path)) != "Thumbnails" && Path.GetFileName(path) != "UpdateCounter.txt" && Path.GetFileName(path) != ".gitignore" && Path.GetFileName(Path.GetDirectoryName(path)) != "Output")
                    client.files.Add(new File { name = Path.GetFileName(path), install_path = path.Remove(0, Environment.CurrentDirectory.Length + 1), md5 = await CalculateMD5(path), url = new Uri(new Uri("https://raw.githubusercontent.com/VladTheJunior/StreamOverlayUpdates/master/"), path.Remove(0, Environment.CurrentDirectory.Length + 1)).ToString() });
            }
            System.IO.File.WriteAllText("Updates.json", JsonConvert.SerializeObject(client));

            //files.Enqueue(new File { url = "http://xakops.pythonanywhere.com/static/StreamOverlay/Overlays/Autumn Championship/Layout.png", install_path = "1.png", name="112" });

            tbProgress.Text = "Checking for updates...";
            Updates server = await CheckUpdates();
            if (server.files.Count > 0)
            {
                
                var differences = server.files.Where(s => !client.files.Any(c => c.install_path == s.install_path && c.md5 == s.md5));
                foreach (File f in differences)
                {
                    if (f.name == "StreamOverlayUpdater.exe")
                        f.install_path += ".upd";
                    files.Enqueue(f);
                }
            }

            if (!files.Any())
                tbProgress.Text = "No new updates.";
            else
            {
                tbProgress.Text = "Getting updates...";
            }
            DownloadFile(files);
        }
    }
}
