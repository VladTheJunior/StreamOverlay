using LibVLCSharp.Shared;
using StreamOverlay.Classes.Map;
using StreamOverlay.Classes.Overlays;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace StreamOverlay
{
    public class EmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return Visibility.Hidden;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ContentToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return Visibility.Visible;
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool)
            {
                if ((bool)value == false)
                {
                    return Visibility.Collapsed;
                }
                else
                { return Visibility.Visible; }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }




    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string)
            {
                return new BrushConverter().ConvertFromString((string)value) as SolidColorBrush;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class PathToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string)
            {
                return new BitmapImage(new Uri((string)value));
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class PathToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string)
            {
                var ib = new ImageBrush()
                {
                    ImageSource = new BitmapImage(new Uri((string)value)),

                    Stretch = Stretch.Uniform
                };
                return ib;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    /// <summary>
    /// Логика взаимодействия для SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        }
        #region Methods
        private static ObservableCollection<Map> BuildMaps()
        {
            ObservableCollection<Map> Maps = new ObservableCollection<Map>();



            Maps.Add(new Map() { title = "Alaska", icon = "/data/maps/alaska_mini.png" });
            Maps.Add(new Map() { title = "Amazonia", icon = "/data/maps/amazon_mini.png" });
            Maps.Add(new Map() { title = "Andes", icon = "/data/maps/andes_mini.png" });
            Maps.Add(new Map() { title = "Andes - Upper", icon = "/data/maps/andes_upper_mini.png" });
            Maps.Add(new Map() { title = "Araucania", icon = "/data/maps/auraucania_mini.png" });
            Maps.Add(new Map() { title = "Arctic Territories", icon = "/data/maps/arctic_territories_mini.png" });
            Maps.Add(new Map() { title = "Atlas", icon = "/data/maps/af_atlas_mini.png" });
            Maps.Add(new Map() { title = "Baja California", icon = "/data/maps/baja_california_mini.png" });
            Maps.Add(new Map() { title = "Bengal", icon = "/data/maps/bengal_mini.png" });
            Maps.Add(new Map() { title = "Borneo", icon = "/data/maps/borneo_mini.png" });
            Maps.Add(new Map() { title = "California", icon = "/data/maps/california_mini.png" });
            Maps.Add(new Map() { title = "Caribbean", icon = "/data/maps/caribbean_mini.png" });
            Maps.Add(new Map() { title = "Cascade Range", icon = "/data/maps/cascade_range_mini.png" });
            Maps.Add(new Map() { title = "Central Plain", icon = "/data/maps/central_plain_mini.png" });
            Maps.Add(new Map() { title = "Ceylon", icon = "/data/maps/ceylon_mini.png" });
            Maps.Add(new Map() { title = "Colorado", icon = "/data/maps/colorado_mini.png" });
            Maps.Add(new Map() { title = "Dakota", icon = "/data/maps/dakota_mini.png" });
            Maps.Add(new Map() { title = "Darfur", icon = "/data/maps/af_darfur_mini.png" });
            Maps.Add(new Map() { title = "Fertile Crescent", icon = "/data/maps/fertile_crescent_mini.png" });
            Maps.Add(new Map() { title = "Florida", icon = "/data/maps/florida_mini.png" });
            Maps.Add(new Map() { title = "Gold Coast", icon = "/data/maps/af_goldcoast_mini.png" });
            Maps.Add(new Map() { title = "Gran Chaco", icon = "/data/maps/gran_chaco_mini.png" });
            Maps.Add(new Map() { title = "Great Lakes", icon = "/data/maps/great_lakes_mini.png" });
            Maps.Add(new Map() { title = "Great Rift", icon = "/data/maps/af_greatrift_mini.png" });
            Maps.Add(new Map() { title = "Highlands", icon = "/data/maps/af_highlands_mini.png" });
            Maps.Add(new Map() { title = "Himalayas", icon = "/data/maps/himalayas_mini.png" });
            Maps.Add(new Map() { title = "Himalayas - Upper", icon = "/data/maps/himalayas_upper_mini.png" });
            Maps.Add(new Map() { title = "Hispaniola", icon = "/data/maps/hispaniola_mini.png" });
            Maps.Add(new Map() { title = "Hokkaido", icon = "/data/maps/hokkaido_mini.png" });
            Maps.Add(new Map() { title = "Honshu", icon = "/data/maps/honshu_mini.png" });
            Maps.Add(new Map() { title = "Horn", icon = "/data/maps/af_horn_mini.png" });
            Maps.Add(new Map() { title = "Indochina", icon = "/data/maps/indochina_mini.png" });
            Maps.Add(new Map() { title = "Indonesia", icon = "/data/maps/indonesia_mini.png" });
            Maps.Add(new Map() { title = "Kamchatka", icon = "/data/maps/kamchatka_mini.png" });
            Maps.Add(new Map() { title = "Korea", icon = "/data/maps/korea_mini.png" });
            Maps.Add(new Map() { title = "Lake Chad", icon = "/data/maps/af_lakechad_mini.png" });
            Maps.Add(new Map() { title = "Malaysia", icon = "/data/maps/malaysia_mini.png" });
            Maps.Add(new Map() { title = "Manchuria", icon = "/data/maps/manchuria_mini.png" });
            Maps.Add(new Map() { title = "Mexico", icon = "/data/maps/mexico_mini.png" });
            Maps.Add(new Map() { title = "Mongolia", icon = "/data/maps/mongolia_mini.png" });
            Maps.Add(new Map() { title = "New England", icon = "/data/maps/new_england_mini.png" });
            Maps.Add(new Map() { title = "Niger Delta", icon = "/data/maps/af_nigerdelta_mini.png" });
            Maps.Add(new Map() { title = "Niger River", icon = "/data/maps/af_nigerriver_mini.png" });
            Maps.Add(new Map() { title = "Nile Valley", icon = "/data/maps/af_nilevalley_mini.png" });
            Maps.Add(new Map() { title = "Northwest Territory", icon = "/data/maps/northwest_territory_mini.png" });
            Maps.Add(new Map() { title = "Orinoco", icon = "/data/maps/orinoco_mini.png" });
            Maps.Add(new Map() { title = "Ozarks", icon = "/data/maps/ozarks_mini.png" });
            Maps.Add(new Map() { title = "Painted Desert", icon = "/data/maps/painted_desert_mini.png" });
            Maps.Add(new Map() { title = "Pampas", icon = "/data/maps/pampas_mini.png" });
            Maps.Add(new Map() { title = "Pampas Sierras", icon = "/data/maps/pampas_sierras_mini.png" });
            Maps.Add(new Map() { title = "Parallel Rivers", icon = "/data/maps/parallel_rivers_mini.png" });
            Maps.Add(new Map() { title = "Patagonia", icon = "/data/maps/patagonia_mini.png" });
            Maps.Add(new Map() { title = "Pepper Coast", icon = "/data/maps/af_peppercoast_mini.png" });
            Maps.Add(new Map() { title = "Plymouth", icon = "/data/maps/plymouth_mini.png" });
            Maps.Add(new Map() { title = "Punjab", icon = "/data/maps/punjab_mini.png" });
            Maps.Add(new Map() { title = "Honshu - Regicide", icon = "/data/maps/regicide_mini.png" });
            Maps.Add(new Map() { title = "Rockies", icon = "/data/maps/rockies_mini.png" });
            Maps.Add(new Map() { title = "Savanna", icon = "/data/maps/af_savanna_mini.png" });
            Maps.Add(new Map() { title = "Sudd", icon = "/data/maps/af_sudd_mini.png" });
            Maps.Add(new Map() { title = "Swahili Coast", icon = "/data/maps/swahili_mini.png" });
            Maps.Add(new Map() { title = "Tassili", icon = "/data/maps/af_tassili_mini.png" });
            Maps.Add(new Map() { title = "Texas", icon = "/data/maps/texas_mini.png" });
            Maps.Add(new Map() { title = "Tripolitania", icon = "/data/maps/af_tripolitania_mini.png" });
            Maps.Add(new Map() { title = "Unknown", icon = "/data/maps/unknown_mini.png" });
            Maps.Add(new Map() { title = "Yellow River", icon = "/data/maps/yellow_river_mini.png" });
            Maps.Add(new Map() { title = "Yucatan", icon = "/data/maps/yucatan_mini.png" });
            Maps.Add(new Map() { title = "Yukon", icon = "/data/maps/yukon_mini.png" });
            Maps.Add(new Map() { title = "Siwa Oasis", icon = "/data/maps/siwa_oasis_mini.png" });
            Maps.Add(new Map() { title = "Ivory Coast", icon = "/data/maps/ivory_coast_mini.png" });
            Maps.Add(new Map() { title = "Lake Victoria", icon = "/data/maps/af_lake_victoria_mini.png" });
            Maps.Add(new Map() { title = "Saharan Routes", icon = "/data/maps/saharan_routes_mini.png" });
            Maps.Add(new Map() { title = "Saharan Routes -LOST", icon = "/data/maps/saharan_routes_lost_mini.png" });
            Maps.Add(new Map() { title = "Sahel", icon = "/data/maps/sahel_mini.png" });
            Maps.Add(new Map() { title = "Dunes", icon = "/data/maps/dunes_mini.png" });

            Maps.Add(new Map() { title = "Sonora", icon = "/data/maps/sonora_mini.png" });
            Maps.Add(new Map() { title = "Bahia", icon = "/data/maps/bahia_mini.png" });
            Maps.Add(new Map() { title = "Carolina", icon = "/data/maps/carolina_mini.png" });
            Maps.Add(new Map() { title = "Deccan", icon = "/data/maps/deccan_mini.png" });
            Maps.Add(new Map() { title = "Silk Road", icon = "/data/maps/silk_road_mini.png" });
            Maps.Add(new Map() { title = "Everglades", icon = "/data/maps/everglades_mini.png" });
            Maps.Add(new Map() { title = "Great Plains", icon = "/data/maps/great_plains_mini.png" });
            Maps.Add(new Map() { title = "Guianas", icon = "/data/maps/guianas_mini.png" });
            Maps.Add(new Map() { title = "Siberia", icon = "/data/maps/siberia_mini.png" });
            Maps.Add(new Map() { title = "Minas Gerais", icon = "/data/maps/minas_gerais_mini.png" });
            Maps.Add(new Map() { title = "Panama", icon = "/data/maps/panama_mini.png" });
            Maps.Add(new Map() { title = "Saguenay", icon = "/data/maps/saguenay_mini.png" });


            return Maps;
        }

        #endregion

        #region Properties and Variables

        private int selectedOverlayIndex;
        public int SelectedOverlayIndex
        {
            get
            {
                return selectedOverlayIndex;
            }
            set
            {
                selectedOverlayIndex = value;
                NotifyPropertyChanged("SelectedOverlayIndex");
                NotifyPropertyChanged("SelectedOverlay");
                NotifyPropertyChanged("isMapPoolEnabled");
                NotifyPropertyChanged("isRoundEnabled");
                NotifyPropertyChanged("isPlayersEnabled");
                NotifyPropertyChanged("isTeamsEnabled");
                NotifyPropertyChanged("isCivsEnabled");
            }
        }
        public Overlay SelectedOverlay
        {
            get
            {
                return Overlays[selectedOverlayIndex];
            }
        }

        private Cursor FAoE { get; set; }
        public Cursor AoE
        {
            get { return FAoE; }
            set
            {
                FAoE = value;
                NotifyPropertyChanged("AoE");
            }
        }

        private string timerAlign { get; set; }
        public string TimerAlign
        {
            get { return timerAlign; }
            set
            {
                timerAlign = value;
                NotifyPropertyChanged("TimerAlign");
            }
        }

        private string scheduleAlign { get; set; }
        public string ScheduleAlign
        {
            get { return scheduleAlign; }
            set
            {
                scheduleAlign = value;
                NotifyPropertyChanged("ScheduleAlign");
            }
        }

        private string mapAlign { get; set; }
        public string MapAlign
        {
            get { return mapAlign; }
            set
            {
                mapAlign = value;
                NotifyPropertyChanged("MapAlign");
            }
        }

        private string brandAlign { get; set; }
        public string BrandAlign
        {
            get { return brandAlign; }
            set
            {
                brandAlign = value;
                NotifyPropertyChanged("BrandAlign");
            }
        }

        private string eventAlign { get; set; }
        public string EventAlign
        {
            get { return eventAlign; }
            set
            {
                eventAlign = value;
                NotifyPropertyChanged("EventAlign");
            }
        }

        public ObservableCollection<Map> MapPool = new ObservableCollection<Map>(BuildMaps());

        ObservableCollection<Overlay> Overlays = new ObservableCollection<Overlay>();
        #endregion



        int Version = 19;

        public SettingsDialog()
        {
            TimerAlign = "TopRight";
            ScheduleAlign = "BottomRight";
            MapAlign = "BottomLeft";
            BrandAlign = "TopLeft";
            EventAlign = "TopLeft";
            SelectedOverlayIndex = 0;


            Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata { DefaultValue = 15 });
            InitializeComponent();
            DataContext = this;

            AoE = new Cursor(Application.GetResourceStream(new Uri("pack://application:,,,/resources/Cursor.cur")).Stream);

            lvMapPool.ItemsSource = MapPool;

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(lvMapPool.ItemsSource);
            collectionView.SortDescriptions.Add(new SortDescription("title", ListSortDirection.Ascending));
            var view = (ICollectionViewLiveShaping)CollectionViewSource.GetDefaultView(lvMapPool.ItemsSource);
            view.IsLiveSorting = true;

            List<Logo> brandLogos =
    Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "data", "brand")).Where(x => Path.GetExtension(x).ToLower() == ".png").Select(x => new Logo { Name = Path.GetFileNameWithoutExtension(x), Path = x }).ToList();
            brandLogos.Insert(0, new Logo() { Name = "<NOT SET>", Path = "" });

            ICollectionView brand_view = CollectionViewSource.GetDefaultView(brandLogos);
            brand_view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            BrandLogos.ItemsSource = brand_view;


            var animations = Directory.GetDirectories(Path.Combine(Environment.CurrentDirectory, "data", "animations"));

            foreach (var anim in animations)
            {
                if (File.Exists(Path.Combine(anim, "video.mp4")) && File.Exists(Path.Combine(anim, "icon.png")) && File.Exists(Path.Combine(anim, "preview.png")))
                {
                    Overlays.Add(new Overlay() { title = Path.GetFileName(anim), preview = Path.Combine(anim, "preview.png"), icon = Path.Combine(anim, "icon.png"), video = Path.Combine(anim, "video.mp4") });
                }

            }

            List<Logo> eventLogos = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "data", "events")).Where(x => Path.GetExtension(x).ToLower() == ".png").Select(x => new Logo { Name = Path.GetFileNameWithoutExtension(x), Path = x }).ToList();
            eventLogos.Insert(0, new Logo() { Name = "<NOT SET>", Path = "" });
            ICollectionView event_view = CollectionViewSource.GetDefaultView(eventLogos);
            brand_view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            EventLogos.ItemsSource = event_view;

            if (File.Exists("UpdateCounter.txt"))
            {
                int counter = Convert.ToInt32(File.ReadAllText("UpdateCounter.txt"));
                if (counter < Version)
                {
                    gReleaseNotes.Visibility = Visibility.Visible;
                    BlurControl.Visibility = Visibility.Visible;
                    File.WriteAllText("UpdateCounter.txt", Version.ToString());
                }
            }
            else
            {
                gReleaseNotes.Visibility = Visibility.Visible;
                BlurControl.Visibility = Visibility.Visible;
                File.WriteAllText("UpdateCounter.txt", Version.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            ThumbnailGenerator thumbnailGenerator = new ThumbnailGenerator();

            thumbnailGenerator.Background.Source = new BitmapImage(new Uri(SelectedOverlay.preview));
            //thumbnailGenerator.Overlay.Source = new BitmapImage(new Uri(thumbnail.background.overlay));

            if (BrandLogos.SelectedIndex == 0)
            {
                thumbnailGenerator.iBrandLogo.Visibility = Visibility.Collapsed;
            }
            else
            {
                thumbnailGenerator.iBrandLogo.Source = new BitmapImage(new Uri((BrandLogos.SelectedItem as Logo).Path));
            }

            if (EventLogos.SelectedIndex == 0)
            {
                thumbnailGenerator.iEventLogo.Visibility = Visibility.Collapsed;
            }
            else
            {
                thumbnailGenerator.iEventLogo.Source = new BitmapImage(new Uri((EventLogos.SelectedItem as Logo).Path));
            }

            thumbnailGenerator.BorderSlider.Value = 10;
            thumbnailGenerator.BlurSlider.Value = 3;
            thumbnailGenerator.FontColorBorder.SelectedColor = (Color)ColorConverter.ConvertFromString("#ff000001");

            thumbnailGenerator.FontTitle.Text = "/Fonts/#Trajan Pro 3";
            thumbnailGenerator.ShadowColorTitle.SelectedColor = (Color)ColorConverter.ConvertFromString("#ff000000");
            thumbnailGenerator.ShadowTitleSlider.Value = 30;
            thumbnailGenerator.FontColorTitle.SelectedColor = (Color)ColorConverter.ConvertFromString("#ffd1372f");
            thumbnailGenerator.FontSizeSliderTitle.Value = 160;
            thumbnailGenerator.TitleThumbnail.SetValue(Canvas.LeftProperty, 120.0);
            thumbnailGenerator.TitleThumbnail.SetValue(Canvas.TopProperty, 45.0);


            thumbnailGenerator.FontRound.Text = "/Fonts/#Trajan Pro 3";
            thumbnailGenerator.ShadowColorRound.SelectedColor = (Color)ColorConverter.ConvertFromString("#ff000000");
            thumbnailGenerator.ShadowRoundSlider.Value = 20;
            thumbnailGenerator.FontColorRound.SelectedColor = (Color)ColorConverter.ConvertFromString("#ffFFFFFF");
            thumbnailGenerator.FontSizeSliderRound.Value = 120;
            thumbnailGenerator.RoundThumbnail.SetValue(Canvas.LeftProperty, 0.0);
            thumbnailGenerator.RoundThumbnail.SetValue(Canvas.TopProperty, 0.0);

            thumbnailGenerator.FontPlayer.Text = "/Fonts/#Trajan Pro 3";
            thumbnailGenerator.ShadowColorPlayer.SelectedColor = (Color)ColorConverter.ConvertFromString("#ff000000");
            thumbnailGenerator.ShadowPlayerSlider.Value = 20;
            thumbnailGenerator.FontColorPlayer1.SelectedColor = (Color)ColorConverter.ConvertFromString("#ffFFFFFF");
            thumbnailGenerator.FontSizeSliderPlayer.Value = 110;



            thumbnailGenerator.FontPlayer.Text = "/Fonts/#Trajan Pro 3";
            thumbnailGenerator.ShadowColorPlayer.SelectedColor = (Color)ColorConverter.ConvertFromString("#ff000000");
            thumbnailGenerator.ShadowPlayerSlider.Value = 20;
            thumbnailGenerator.FontColorPlayer2.SelectedColor = (Color)ColorConverter.ConvertFromString("#ffFFFFFF");
            thumbnailGenerator.FontSizeSliderPlayer.Value = 110;


            thumbnailGenerator.FontVS.Text = "/Fonts/#Trajan Pro 3";
            thumbnailGenerator.ShadowColorVS.SelectedColor = (Color)ColorConverter.ConvertFromString("#ff000000");
            thumbnailGenerator.ShadowVSSlider.Value = 20;
            thumbnailGenerator.FontColorVS.SelectedColor = (Color)ColorConverter.ConvertFromString("#ffffd700");
            thumbnailGenerator.FontSizeSliderVS.Value = 120;

            thumbnailGenerator.VsThumbnail.SetValue(Canvas.LeftProperty, 650.0);
            thumbnailGenerator.VsThumbnail.SetValue(Canvas.TopProperty, 180.0);

            thumbnailGenerator.Player2Thumbnail.SetValue(Canvas.TopProperty, 310.0);
            thumbnailGenerator.Player1Thumbnail.SetValue(Canvas.TopProperty, 50.0);

            thumbnailGenerator.Player2Thumbnail.SetValue(Canvas.LeftProperty, 500.0);
            thumbnailGenerator.Player1Thumbnail.SetValue(Canvas.LeftProperty, 500.0);

            thumbnailGenerator.ShowDialog();
        }



        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.Topmost = false;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            this.Topmost = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window is MainWindow || window is ThumbnailGenerator)
                {
                    if (!window.IsVisible)
                    {
                        window.Close();
                    }
                }
            }
        }


        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            if (SelectedOverlayIndex == 0)
            {
                SelectedOverlayIndex = Overlays.Count - 1;
            }
            else
            {
                SelectedOverlayIndex--;
            }
        }

        private void btn_act_forward_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOverlayIndex == Overlays.Count - 1)
            {
                SelectedOverlayIndex = 0;
            }
            else
            {
                SelectedOverlayIndex++;
            }
        }

        private Point AlignPosition(string align, double width, double height)
        {
            switch (align)
            {
                case "TopRight":
                    return new Point(1920 - 40 - width, 20);
                case "BottomRight":
                    return new Point(1920 - 40 - width, 1080 - 40 - height);
                case "BottomLeft":
                    return new Point(20, 1080 - 40 - height);
                case "TopLeft":
                    return new Point(20, 20);
            }

            return new Point(0, 0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SelectedOverlay == null)
            {
                (sender as Button).IsEnabled = true;
                return;
            }

            (sender as Button).IsEnabled = false;


            MainWindow mainWindow = new MainWindow();


            

            if (tbAnimation.IsChecked == true)
            {
                mainWindow.PreviewImage.Source = new BitmapImage(new Uri(SelectedOverlay.preview));

                mainWindow._libVLC = new LibVLC();
                mainWindow._mediaPlayer = new MediaPlayer(mainWindow._libVLC);

                mainWindow.Unloaded += mainWindow.Player_Unloaded;

                mainWindow.Animation.Loaded += (sender, e) =>
                {
                    mainWindow.Animation.MediaPlayer = mainWindow._mediaPlayer;
                    using (var media = new Media(mainWindow._libVLC, new Uri(Path.Combine(Environment.CurrentDirectory, SelectedOverlay.video)), new string[] { ":input-repeat=65535" }))
                        mainWindow.Animation.MediaPlayer.Play(media);
                    mainWindow.PreviewImage.Visibility = Visibility.Collapsed;
                };
            }
            else
            {
                mainWindow.PreviewImage.Source = new BitmapImage(new Uri(SelectedOverlay.preview));
            }

            if (BrandLogos.SelectedIndex == 0)
            {
                mainWindow.iBrandLogo.Visibility = Visibility.Collapsed;
            }
            else
            {
                mainWindow.iBrandLogo.Source = new BitmapImage(new Uri((BrandLogos.SelectedItem as Logo).Path));
            }

            if (EventLogos.SelectedIndex == 0)
            {
                mainWindow.iEventLogo.Visibility = Visibility.Collapsed;
            }
            else
            {
                mainWindow.iEventLogo.Source = new BitmapImage(new Uri((EventLogos.SelectedItem as Logo).Path));
            }

            if (cbCountdown.IsChecked == false)
            {
                mainWindow.gCountdown.Visibility = Visibility.Hidden;
            }

            if (cbSchedule.IsChecked == true)
            {
                var myCur = Application.GetResourceStream(new Uri("pack://application:,,,/resources/Cursor.cur")).Stream;
                mainWindow.Schedule.PreviewKeyDown += mainWindow.TextBox_PreviewKeyDown;
                mainWindow.Schedule.Cursor = new Cursor(myCur);
                mainWindow.Schedule.Focus();
            }
            else
            {
                mainWindow.gSchedule.Visibility = Visibility.Hidden;
            }



            List<Map> maps = lvMapPool.SelectedItems.Cast<Map>().Where(x => x.isSelected == true).ToList();

            DoubleAnimation hideMap = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0));
            mainWindow.ibMapIcon.BeginAnimation(Image.OpacityProperty, hideMap);

            if (maps.Count > 0)
            {
                DoubleAnimation center = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.8));
                CircleEase ease1 = new CircleEase();
                ease1.EasingMode = EasingMode.EaseInOut;
                center.EasingFunction = ease1;

                DoubleAnimation center2 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
                center2.EasingFunction = ease1;

                DoubleAnimation center5 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.8));
                center5.EasingFunction = ease1;


                DoubleAnimation center7 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
                center7.EasingFunction = ease1;
                center7.BeginTime = TimeSpan.FromSeconds(9);


                DoubleAnimation center3 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.8));
                center3.BeginTime = TimeSpan.FromSeconds(9);
                ease1.EasingMode = EasingMode.EaseInOut;
                center3.EasingFunction = ease1;

                DoubleAnimation center4 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
                center4.BeginTime = TimeSpan.FromSeconds(8.4);
                center5.EasingFunction = ease1;


                mainWindow.AnimateMapPool.Interval = new TimeSpan(0, 0, 0);

                mainWindow.AnimateMapPool.Tick += async (object s, EventArgs eventArgs) =>
                {
                    mainWindow.AnimateMapPool.Stop();

                    int MapIndex = 1;
                    foreach (Map map in maps)
                    {

                        mainWindow.tbMapPool.Text = "#" + MapIndex.ToString() + " " + map.title;
                        mainWindow.ibMapIcon.Source = new BitmapImage(new Uri("pack://application:,,," + map.icon));

                        mainWindow.stMap.BeginAnimation(ScaleTransform.ScaleXProperty, center5);
                        mainWindow.stMap.BeginAnimation(ScaleTransform.ScaleYProperty, center5);

                        mainWindow.MapTransparentStop.BeginAnimation(GradientStop.OffsetProperty, center);
                        mainWindow.MapBlackStop.BeginAnimation(GradientStop.OffsetProperty, center2);

                        mainWindow.ibMapIcon.BeginAnimation(Image.OpacityProperty, center5);

                        await Task.Delay(1000);


                        if (maps.Count > 1)
                        {
                            mainWindow.stMap.BeginAnimation(ScaleTransform.ScaleXProperty, center7, HandoffBehavior.Compose);
                            mainWindow.stMap.BeginAnimation(ScaleTransform.ScaleYProperty, center7, HandoffBehavior.Compose);
                            mainWindow.MapTransparentStop.BeginAnimation(GradientStop.OffsetProperty, center3);
                            mainWindow.MapBlackStop.BeginAnimation(GradientStop.OffsetProperty, center4);
                        }

                        MapIndex++;
                        await Task.Delay(10000);
                    }
                    if (maps.Count > 1)
                        mainWindow.AnimateMapPool.Start();
                };
                mainWindow.AnimateMapPool.Start();
            }
            else
            {
                mainWindow.gMapPool.Visibility = Visibility.Hidden;
            }

            mainWindow.InputBindings.Add(new InputBinding(mainWindow.TimeUp, new KeyGesture(Key.Up, ModifierKeys.Control)));
            mainWindow.InputBindings.Add(new InputBinding(mainWindow.TimeDown, new KeyGesture(Key.Down, ModifierKeys.Control)));
            mainWindow.InputBindings.Add(new InputBinding(mainWindow.CloseOverlay, new KeyGesture(Key.Escape)));
            mainWindow.InputBindings.Add(new InputBinding(mainWindow.MinimizeOverlay, new KeyGesture(Key.Space, ModifierKeys.Control)));

            mainWindow.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.Arrange(new Rect(0, 0, mainWindow.DesiredSize.Width, mainWindow.DesiredSize.Height));


            mainWindow.gCountdown.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.gCountdown.Arrange(new Rect(0, 0, mainWindow.gCountdown.DesiredSize.Width, mainWindow.gCountdown.DesiredSize.Height));

            mainWindow.gSchedule.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.gSchedule.Arrange(new Rect(0, 0, mainWindow.gSchedule.DesiredSize.Width, mainWindow.gSchedule.DesiredSize.Height));

            mainWindow.gMapPool.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.gMapPool.Arrange(new Rect(0, 0, mainWindow.gMapPool.DesiredSize.Width, mainWindow.gMapPool.DesiredSize.Height));


            mainWindow.iBrandLogo.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.iBrandLogo.Arrange(new Rect(0, 0, mainWindow.iBrandLogo.DesiredSize.Width, mainWindow.iBrandLogo.DesiredSize.Height));

            mainWindow.iEventLogo.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.iEventLogo.Arrange(new Rect(0, 0, mainWindow.iEventLogo.DesiredSize.Width, mainWindow.iEventLogo.DesiredSize.Height));



            Point pCountdown = AlignPosition(TimerAlign, mainWindow.gCountdown.ActualWidth, mainWindow.gCountdown.ActualHeight);
            mainWindow.gCountdown.SetValue(Canvas.LeftProperty, pCountdown.X);
            mainWindow.gCountdown.SetValue(Canvas.TopProperty, pCountdown.Y);

            Point pSchedule = AlignPosition(ScheduleAlign, mainWindow.gSchedule.ActualWidth, mainWindow.gSchedule.ActualHeight);
            mainWindow.gSchedule.SetValue(Canvas.LeftProperty, pSchedule.X);
            mainWindow.gSchedule.SetValue(Canvas.TopProperty, pSchedule.Y);

            Point pMap = AlignPosition(MapAlign, mainWindow.gMapPool.ActualWidth, mainWindow.gMapPool.ActualHeight);
            mainWindow.gMapPool.SetValue(Canvas.LeftProperty, pMap.X);
            mainWindow.gMapPool.SetValue(Canvas.TopProperty, pMap.Y);

            Point pBrand = AlignPosition(BrandAlign, mainWindow.iBrandLogo.ActualWidth, mainWindow.iBrandLogo.ActualHeight);
            mainWindow.iBrandLogo.SetValue(Canvas.LeftProperty, pBrand.X);
            mainWindow.iBrandLogo.SetValue(Canvas.TopProperty, pBrand.Y);

            Point pEvent = AlignPosition(EventAlign, mainWindow.iEventLogo.ActualWidth, mainWindow.iEventLogo.ActualHeight);
            mainWindow.iEventLogo.SetValue(Canvas.LeftProperty, pEvent.X);
            mainWindow.iEventLogo.SetValue(Canvas.TopProperty, pEvent.Y);

            mainWindow.Cursor = AoE;
            Mouse.OverrideCursor = AoE;
            mainWindow.Show();
            this.Hide();


            (sender as Button).IsEnabled = true;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            BlurControl.Visibility = Visibility.Collapsed;
            gMessageBox.Visibility = Visibility.Collapsed;
        }



        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            gReleaseNotes.Visibility = Visibility.Collapsed;
            BlurControl.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            gReleaseNotes.Visibility = Visibility.Visible;
            BlurControl.Visibility = Visibility.Visible;
        }

        private void bTimerAlign_Click(object sender, RoutedEventArgs e)
        {
            switch (TimerAlign)
            {
                case "TopRight":
                    TimerAlign = "BottomRight";
                    break;
                case "BottomRight":
                    TimerAlign = "BottomLeft";
                    break;
                case "BottomLeft":
                    TimerAlign = "TopLeft";
                    break;
                case "TopLeft":
                    TimerAlign = "TopRight";
                    break;
            }
        }

        private void bScheduleAlign_Click(object sender, RoutedEventArgs e)
        {
            switch (ScheduleAlign)
            {
                case "TopRight":
                    ScheduleAlign = "BottomRight";
                    break;
                case "BottomRight":
                    ScheduleAlign = "BottomLeft";
                    break;
                case "BottomLeft":
                    ScheduleAlign = "TopLeft";
                    break;
                case "TopLeft":
                    ScheduleAlign = "TopRight";
                    break;
            }
        }

        private void bBrandAlign_Click(object sender, RoutedEventArgs e)
        {
            switch (BrandAlign)
            {
                case "TopRight":
                    BrandAlign = "BottomRight";
                    break;
                case "BottomRight":
                    BrandAlign = "BottomLeft";
                    break;
                case "BottomLeft":
                    BrandAlign = "TopLeft";
                    break;
                case "TopLeft":
                    BrandAlign = "TopRight";
                    break;
            }
        }

        private void bEventAlign_Click(object sender, RoutedEventArgs e)
        {
            switch (EventAlign)
            {
                case "TopRight":
                    EventAlign = "BottomRight";
                    break;
                case "BottomRight":
                    EventAlign = "BottomLeft";
                    break;
                case "BottomLeft":
                    EventAlign = "TopLeft";
                    break;
                case "TopLeft":
                    EventAlign = "TopRight";
                    break;
            }
        }

        private void bMapAlign_Click(object sender, RoutedEventArgs e)
        {
            switch (MapAlign)
            {
                case "TopRight":
                    MapAlign = "BottomRight";
                    break;
                case "BottomRight":
                    MapAlign = "BottomLeft";
                    break;
                case "BottomLeft":
                    MapAlign = "TopLeft";
                    break;
                case "TopLeft":
                    MapAlign = "TopRight";
                    break;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }

}
