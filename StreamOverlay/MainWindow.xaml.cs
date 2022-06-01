using System;
using System.Collections.Generic;
using System.Linq;
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

using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;
using LibVLCSharp.Shared;
using System.Windows.Threading;
using System.ComponentModel;
using System.Diagnostics;
using StreamOverlay.Classes.Civ;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Windows.Media.Animation;

namespace StreamOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool needClose = true;

        public ObservableCollection<Civ> Team1Player1CivPool = new ObservableCollection<Civ>();
        public ObservableCollection<Civ> Team1Player2CivPool = new ObservableCollection<Civ>();
        public ObservableCollection<Civ> Team1Player3CivPool = new ObservableCollection<Civ>();

        public ObservableCollection<Civ> Team2Player1CivPool = new ObservableCollection<Civ>();
        public ObservableCollection<Civ> Team2Player2CivPool = new ObservableCollection<Civ>();
        public ObservableCollection<Civ> Team2Player3CivPool = new ObservableCollection<Civ>();



        private string _wating = "waiting";
        public string Time
        {
            get
            {
                if (_time.TotalSeconds == 0)
                {
                    if (_wating == "waiting..")
                    {
                        _wating = "waiting...";
                        return "waiting...";
                    }
                    if (_wating == "waiting...")
                    {
                        _wating = "waiting";
                        return "waiting";
                    }
                    if (_wating == "waiting")
                    {
                        _wating = "waiting.";
                        return "waiting.";
                    }
                    else
                    {
                        _wating = "waiting..";
                        return "waiting..";
                    }
                }
                else
                {
                    _wating = "waiting";
                    return _time.ToString("c");
                }

            }
        }

        public class ActionCommand : ICommand
        {
            private readonly Action _action;

            public ActionCommand(Action action)
            {
                _action = action;
            }

            public void Execute(object parameter)
            {
                _action();
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;
        }

        DispatcherTimer _timer;
        TimeSpan _time;

        private ICommand timeUp;
        public ICommand TimeUp
        {
            get
            {
                return timeUp
                    ?? (timeUp = new ActionCommand(() =>
                    {
                        _time = _time.Add(TimeSpan.FromMinutes(1));
                        NotifyPropertyChanged("Time");
                    }));
            }
        }

        private ICommand timeDown;
        public ICommand TimeDown
        {
            get
            {
                return timeDown
                    ?? (timeDown = new ActionCommand(() =>
                    {
                        if (_time.TotalSeconds > 60)
                        {
                            _time = _time.Add(TimeSpan.FromMinutes(-1));
                            NotifyPropertyChanged("Time");
                        }
                        else if (_time.TotalSeconds > 0)
                        {
                            _time = _time.Add(TimeSpan.FromSeconds(-1));
                            NotifyPropertyChanged("Time");
                        }
                    }));
            }
        }


        private ICommand closeOverlay;
        public ICommand CloseOverlay
        {
            get
            {
                return closeOverlay
                    ?? (closeOverlay = new ActionCommand(() =>
                    {
                        needClose = false;
                        this.Hide();
                        Application.Current.MainWindow.Show();
                    }));
            }
        }

        private ICommand mininizeOverlay;
        public ICommand MinimizeOverlay
        {
            get
            {
                return mininizeOverlay
                    ?? (mininizeOverlay = new ActionCommand(() =>
                    {
                        this.WindowState = WindowState.Minimized;
                    }));
            }
        }

        private ICommand countdownVisibility;
        public ICommand CountdownVisibility
        {
            get
            {
                return countdownVisibility
                    ?? (countdownVisibility = new ActionCommand(() =>
                    {
                        if (gCountdown.Visibility == Visibility.Visible)
                        {
                            gCountdown.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            gCountdown.Visibility = Visibility.Visible;
                        }
                        
                    }));
            }
        }

        private ICommand scheduleVisibility;
        public ICommand ScheduleVisibility
        {
            get
            {
                return scheduleVisibility
                    ?? (scheduleVisibility = new ActionCommand(() =>
                    {
                        if (gSchedule.Visibility == Visibility.Visible)
                        {
                            gSchedule.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            gSchedule.Visibility = Visibility.Visible;
                        }

                    }));
            }
        }

        private ICommand mapPoolVisibility;
        public ICommand MapPoolVisibility
        {
            get
            {
                return mapPoolVisibility
                    ?? (mapPoolVisibility = new ActionCommand(() =>
                    {
                        if (gMapPool.Visibility == Visibility.Visible)
                        {
                            gMapPool.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            gMapPool.Visibility = Visibility.Visible;
                        }

                    }));
            }
        }

        private ICommand eventLogoVisibility;
        public ICommand EventLogoVisibility
        {
            get
            {
                return eventLogoVisibility
                    ?? (eventLogoVisibility = new ActionCommand(() =>
                    {
                        if (gEventLogo.Visibility == Visibility.Visible)
                        {
                            gEventLogo.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            gEventLogo.Visibility = Visibility.Visible;
                        }

                    }));
            }
        }

        private ICommand brandLogoVisibility;
        public ICommand BrandLogoVisibility
        {
            get
            {
                return brandLogoVisibility
                    ?? (brandLogoVisibility = new ActionCommand(() =>
                    {
                        if (gBrandLogo.Visibility == Visibility.Visible)
                        {
                            gBrandLogo.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            gBrandLogo.Visibility = Visibility.Visible;
                        }

                    }));
            }
        }

        private ICommand playersPanelVisibility;
        public ICommand PlayerPanelVisibility
        {
            get
            {
                return playersPanelVisibility
                    ?? (playersPanelVisibility = new ActionCommand(() =>
                    {
                        if (gPlayersPanel.Visibility == Visibility.Visible)
                        {
                            gPlayersPanel.Visibility = Visibility.Hidden;
                            gPlayers.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            gPlayersPanel.Visibility = Visibility.Visible;
                            gPlayers.Visibility = Visibility.Visible;
                        }

                    }));
            }
        }


        private ICommand twitchPanelVisibility;
        public ICommand TwitchPanelVisibility
        {
            get
            {
                return twitchPanelVisibility
                    ?? (twitchPanelVisibility = new ActionCommand(() =>
                    {
                        if (gTwitchInfo.Visibility == Visibility.Visible)
                        {
                            gTwitchInfo.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            gTwitchInfo.Visibility = Visibility.Visible;
                        }

                    }));
            }
        }

        private ICommand chromakeyVisibility;
        public ICommand ChromakeyVisibility
        {
            get
            {
                return chromakeyVisibility
                    ?? (chromakeyVisibility = new ActionCommand(() =>
                    {
                        if (((SolidColorBrush)OverlayCanvas.Background).Color == ((SolidColorBrush)(new BrushConverter().ConvertFromString("#ff00b140"))).Color)
                        {
                            PreviewImage.Opacity = 1;
                            OverlayCanvas.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#01000000");
                        }
                        else
                        {
                            PreviewImage.Opacity = 0;
                            OverlayCanvas.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#ff00b140");
                        }

                    }));
            }
        }

        private ICommand scorePanelVisibility;
        public ICommand ScorePanelVisibility
        {
            get
            {
                return scorePanelVisibility
                    ?? (scorePanelVisibility = new ActionCommand(() =>
                    {
                        if (gScorePanel.Visibility == Visibility.Visible)
                        {
                            gScorePanel.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            gScorePanel.Visibility = Visibility.Visible;
                        }


                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        }
        public string TwitchIcon { get; set; }
        public DispatcherTimer AnimateMapPool = new DispatcherTimer();
        public DispatcherTimer TwitchInfo = new DispatcherTimer();
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }

        public LibVLC _libVLC;
        public MediaPlayer _mediaPlayer;

        List<string> Playlist = new List<string>();
        int currentAudioIndex = 0;
        public HttpClient client = new HttpClient();

        public MainWindow()
        {
            client.DefaultRequestHeaders.Add("Client-Id", "kimne78kx3ncx6brgo4mv6wki5h1ko");
            InitializeComponent();
            DataContext = this;
            var myCur = Application.GetResourceStream(new Uri("pack://application:,,,/resources/Cursor.cur")).Stream;
            Cursor = new Cursor(myCur);
            _time = TimeSpan.FromSeconds(0);
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                NotifyPropertyChanged("Time");
                if (_time != TimeSpan.Zero)
                    _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);


            _timer.Start();
            Playlist = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "data", "audio", "Age of Empires")).Where(x => Path.GetExtension(x).ToLower() == ".mp3").ToList();
            Playlist.AddRange(Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "data", "audio", "Age of Empires II")).Where(x => Path.GetExtension(x).ToLower() == ".mp3"));
            Playlist.AddRange(Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "data", "audio", "Age of Empires III")).Where(x => Path.GetExtension(x).ToLower() == ".mp3"));
            Playlist.AddRange(Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "data", "audio", "Age of Empires IV")).Where(x => Path.GetExtension(x).ToLower() == ".mp3"));
            AudioPlayer.Source = new Uri(Playlist[0]);
            tbAudioName.Text = Path.GetFileName(Playlist[currentAudioIndex]);
            if (Settings1.Default.Audio)
            {
                AudioPlayer.Play();
                bPlay.Visibility = Visibility.Collapsed;
            }
            else
            {
                bPause.Visibility = Visibility.Collapsed;
            }
                
        }

        public void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                switch (e.Key)
                {
                    case Key.Down:
                        TimeDown.Execute(null);
                        e.Handled = true;
                        return;

                    case Key.Up:
                        TimeUp.Execute(null);
                        e.Handled = true;
                        return;
                }
        }

        public void Player_Unloaded(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Dispose();
            _libVLC.Dispose();
            AudioPlayer.Stop();
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            if (needClose)
                Environment.Exit(0);
        }


        double FirstXPos, FirstYPos;
        object MovingObject;

        private void ObjectMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Slider)
            {
                FirstXPos = e.GetPosition(sender as FrameworkElement).X;
                FirstYPos = e.GetPosition(sender as FrameworkElement).Y;
                MovingObject = sender;
            }
                
        }

        private void ObjectMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Slider)
            {
                MovingObject = null;
            }
        }

        private void OverlayCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if  (sender is not TextBox)
                this.Focus();
        }

        private void Team1_MouseDown(object sender, MouseButtonEventArgs e)
        {

           int Tag = Convert.ToInt32((sender as Grid).Tag);

            var civ1 = Team1Player1CivPool.FirstOrDefault(x => x.Tag == Tag);
            var civ2 = Team1Player2CivPool.FirstOrDefault(x => x.Tag == Tag);
            var civ3 = Team1Player3CivPool.FirstOrDefault(x => x.Tag == Tag);

            if (civ1 != null)
            {
                civ1.NextStatus();
            }
            if (civ2 != null)
            {
                civ2.NextStatus();
            }
            if (civ3 != null)
            {
                civ3.NextStatus();
            }
            Team1Score = Team1Player1CivPool.Where(x => x.Status == 1).Count();
            
            NotifyPropertyChanged("Team1Score");

        }

        private void Team2_MouseDown(object sender, MouseButtonEventArgs e)
        {

            int Tag = Convert.ToInt32((sender as Grid).Tag);

            var civ1 = Team2Player1CivPool.FirstOrDefault(x => x.Tag == Tag);
            var civ2 = Team2Player2CivPool.FirstOrDefault(x => x.Tag == Tag);
            var civ3 = Team2Player3CivPool.FirstOrDefault(x => x.Tag == Tag);

            if (civ1 != null)
            {
                civ1.NextStatus();
            }
            if (civ2 != null)
            {
                civ2.NextStatus();
            }
            if (civ3 != null)
            {
                civ3.NextStatus();
            }
            Team2Score = Team2Player1CivPool.Where(x => x.Status == 1).Count();
            NotifyPropertyChanged("Team2Score");
        }

        private void Team1_Player1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int Tag = Convert.ToInt32((sender as Grid).Tag);

            var civ = Team1Player1CivPool.FirstOrDefault(x => x.Tag == Tag);

            if (civ != null)
            {
                civ.NextId();
            }
        }

        private void Team1_Player2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int Tag = Convert.ToInt32((sender as Grid).Tag);

            var civ = Team1Player2CivPool.FirstOrDefault(x => x.Tag == Tag);

            if (civ != null)
            {
                civ.NextId();
            }
        }

        private void Team1_Player3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int Tag = Convert.ToInt32((sender as Grid).Tag);

            var civ = Team1Player3CivPool.FirstOrDefault(x => x.Tag == Tag);

            if (civ != null)
            {
                civ.NextId();
            }
        }

        private void Team2_Player1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int Tag = Convert.ToInt32((sender as Grid).Tag);

            var civ = Team2Player1CivPool.FirstOrDefault(x => x.Tag == Tag);

            if (civ != null)
            {
                civ.NextId();
            }
        }

        private void Team2_Player2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int Tag = Convert.ToInt32((sender as Grid).Tag);

            var civ = Team2Player2CivPool.FirstOrDefault(x => x.Tag == Tag);

            if (civ != null)
            {
                civ.NextId();
            }
        }

        private void Team2_Player3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int Tag = Convert.ToInt32((sender as Grid).Tag);

            var civ = Team2Player3CivPool.FirstOrDefault(x => x.Tag == Tag);

            if (civ != null)
            {
                civ.NextId();
            }
        }

        private void Slider_ValueChanged(System.Object sender, RoutedPropertyChangedEventArgs<System.Double> e)
        {
         
            ScaleTransform scale = new ScaleTransform(1+ e.NewValue/100, 1 + e.NewValue / 100);
            iBrandLogo.LayoutTransform = scale;
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ScaleTransform scale = new ScaleTransform(1 + e.NewValue / 100, 1 + e.NewValue / 100);
            iEventLogo.LayoutTransform = scale;
        }

        private void AudioPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (cbLoop.IsChecked == false)
            {
                if (currentAudioIndex >= Playlist.Count - 1)
                    currentAudioIndex = 0;
                else
                    currentAudioIndex++;
            }
            AudioPlayer.Source = new Uri(Playlist[currentAudioIndex]);
            AudioPlayer.Play();
            tbAudioName.Text = Path.GetFileName(Playlist[currentAudioIndex]);
            bPlay.Visibility = Visibility.Collapsed;
            bPause.Visibility = Visibility.Visible;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (currentAudioIndex == 0)
                currentAudioIndex = Playlist.Count - 1;
            else
                currentAudioIndex--;
            AudioPlayer.Source = new Uri(Playlist[currentAudioIndex]);
            AudioPlayer.Play();
            tbAudioName.Text = Path.GetFileName(Playlist[currentAudioIndex]);
            bPlay.Visibility = Visibility.Collapsed;
            bPause.Visibility = Visibility.Visible;
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            AudioPlayer.Play();
            bPlay.Visibility = Visibility.Collapsed;
            bPause.Visibility = Visibility.Visible;
        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            AudioPlayer.Pause();
            bPlay.Visibility = Visibility.Visible;
            bPause.Visibility = Visibility.Collapsed;
        }

        private void Image_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            if (currentAudioIndex >= Playlist.Count -1)
                currentAudioIndex = 0;
            else
                currentAudioIndex++;
            AudioPlayer.Source = new Uri(Playlist[currentAudioIndex]);
            AudioPlayer.Play();
            tbAudioName.Text = Path.GetFileName(Playlist[currentAudioIndex]);
            bPlay.Visibility = Visibility.Collapsed;
            bPause.Visibility = Visibility.Visible;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

            currentAudioIndex = Playlist.IndexOf(Playlist.FirstOrDefault(x=> Path.GetFileName(x).StartsWith("001 Age of Empires - ")));
            AudioPlayer.Source = new Uri(Playlist[currentAudioIndex]);
            AudioPlayer.Play();
            tbAudioName.Text = Path.GetFileName(Playlist[currentAudioIndex]);
            bPlay.Visibility = Visibility.Collapsed;
            bPause.Visibility = Visibility.Visible;
        }

        private void TextBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            currentAudioIndex = Playlist.IndexOf(Playlist.FirstOrDefault(x => Path.GetFileName(x).StartsWith("001 Age of Empires II - ")));
            AudioPlayer.Source = new Uri(Playlist[currentAudioIndex]);
            AudioPlayer.Play();
            tbAudioName.Text = Path.GetFileName(Playlist[currentAudioIndex]);
            bPlay.Visibility = Visibility.Collapsed;
            bPause.Visibility = Visibility.Visible;
        }

        private void TextBlock_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            currentAudioIndex = Playlist.IndexOf(Playlist.FirstOrDefault(x => Path.GetFileName(x).StartsWith("001 Age of Empires III - ")));
            AudioPlayer.Source = new Uri(Playlist[currentAudioIndex]);
            AudioPlayer.Play();
            tbAudioName.Text = Path.GetFileName(Playlist[currentAudioIndex]);
            bPlay.Visibility = Visibility.Collapsed;
            bPause.Visibility = Visibility.Visible;
        }

        private void TextBlock_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            currentAudioIndex = Playlist.IndexOf(Playlist.FirstOrDefault(x => Path.GetFileName(x).StartsWith("001 Age of Empires IV - ")));
            AudioPlayer.Source = new Uri(Playlist[currentAudioIndex]);
            AudioPlayer.Play();
            tbAudioName.Text = Path.GetFileName(Playlist[currentAudioIndex]);
            bPlay.Visibility = Visibility.Collapsed;
            bPause.Visibility = Visibility.Visible;
        }

        bool CasterShowed = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!CasterShowed)
            {

            
            DoubleAnimation center = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.8));
            CircleEase ease1 = new CircleEase();
            ease1.EasingMode = EasingMode.EaseInOut;
            center.EasingFunction = ease1;

            DoubleAnimation center2 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
            center2.EasingFunction = ease1;
            CasterTransparentStop.BeginAnimation(GradientStop.OffsetProperty, center);
            CasterBlackStop.BeginAnimation(GradientStop.OffsetProperty, center2);
                CasterShowed = true;
            }
            else {
                CircleEase ease1 = new CircleEase();
                DoubleAnimation center3 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.8));
                center3.BeginTime = TimeSpan.FromSeconds(0.4);
                ease1.EasingMode = EasingMode.EaseInOut;
            center3.EasingFunction = ease1;

            DoubleAnimation center4 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
            center4.EasingFunction = ease1;

            CasterTransparentStop.BeginAnimation(GradientStop.OffsetProperty, center3);
            CasterBlackStop.BeginAnimation(GradientStop.OffsetProperty, center4);
                CasterShowed = false;
            }

        }

        private void ObjectMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (MovingObject != null)
                {
                    (MovingObject as FrameworkElement).SetValue(Canvas.LeftProperty,
                         e.GetPosition((MovingObject as FrameworkElement).Parent as FrameworkElement).X - FirstXPos);

                    (MovingObject as FrameworkElement).SetValue(Canvas.TopProperty,
                         e.GetPosition((MovingObject as FrameworkElement).Parent as FrameworkElement).Y - FirstYPos);
                }
            }

        }
    }
}
