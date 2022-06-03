using LibVLCSharp.Shared;
using StreamOverlay.Classes.Civ;
using StreamOverlay.Classes.Map;
using StreamOverlay.Classes.Overlays;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using static StreamOverlay.Classes.Twitch.Twitch;
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
            Maps.Add(new Map() { title = "Arabia", icon = "/data/maps/arabia_mini.png" });
            Maps.Add(new Map() { title = "Araucania", icon = "/data/maps/auraucania_mini.png" });
            Maps.Add(new Map() { title = "Arctic Territories", icon = "/data/maps/arctic_territories_mini.png" });
            Maps.Add(new Map() { title = "Atlas", icon = "/data/maps/af_atlas_mini.png" });
            Maps.Add(new Map() { title = "Baja California", icon = "/data/maps/baja_california_mini.png" });
            Maps.Add(new Map() { title = "Bengal", icon = "/data/maps/bengal_mini.png" });
            Maps.Add(new Map() { title = "Borneo", icon = "/data/maps/borneo_mini.png" });
            Maps.Add(new Map() { title = "California", icon = "/data/maps/california_mini.png" });
            Maps.Add(new Map() { title = "Caribbean", icon = "/data/maps/caribbean_mini.png" });
            Maps.Add(new Map() { title = "Cascade Range", icon = "/data/maps/cascade_range_mini.png" });
            Maps.Add(new Map() { title = "Congo Basin", icon = "/data/maps/congo_basin_mini.png" });
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

            Maps.Add(new Map() { title = "Alps", icon = "/data/maps/eu_alps_mini.png" });
            Maps.Add(new Map() { title = "Anatolia", icon = "/data/maps/eu_anatolia_mini.png" });
            Maps.Add(new Map() { title = "Archipelago", icon = "/data/maps/eu_archipelago_mini.png" });
            Maps.Add(new Map() { title = "Balkan Mountains", icon = "/data/maps/eu_balkan_mountains_mini.png" });
            Maps.Add(new Map() { title = "Baltic Sea", icon = "/data/maps/eu_baltic_mini.png" });
            Maps.Add(new Map() { title = "Black Forest", icon = "/data/maps/eu_black_forest_mini.png" });
            Maps.Add(new Map() { title = "Bohemia", icon = "/data/maps/eu_bohemia_mini.png" });
            Maps.Add(new Map() { title = "Budapest", icon = "/data/maps/eu_budapest_mini.png" });
            Maps.Add(new Map() { title = "Carpathians", icon = "/data/maps/eu_carpathians_mini.png" });
            Maps.Add(new Map() { title = "Corsica and Sardinia", icon = "/data/maps/eu_corsica_sardinia_mini.png" });

            Maps.Add(new Map() { title = "Courland", icon = "/data/maps/eu_courland_mini.png" });
            Maps.Add(new Map() { title = "Danish Straits", icon = "/data/maps/eu_danish_strait_mini.png" });
            Maps.Add(new Map() { title = "Dnieper Basin", icon = "/data/maps/eu_dnieper_basin_mini.png" });
            Maps.Add(new Map() { title = "England", icon = "/data/maps/eu_england_mini.png" });
            Maps.Add(new Map() { title = "Eurasian Steppe", icon = "/data/maps/eu_eurasian_steppe_mini.png" });
            Maps.Add(new Map() { title = "Finland", icon = "/data/maps/eu_finland_mini.png" });
            Maps.Add(new Map() { title = "Forest Nothing", icon = "/data/maps/eu_forest_nothing_mini.png" });
            Maps.Add(new Map() { title = "France", icon = "/data/maps/eu_france_mini.png" });
            Maps.Add(new Map() { title = "Hungarian Plains", icon = "/data/maps/eu_hungarian_plains_mini.png" });
            Maps.Add(new Map() { title = "Ireland", icon = "/data/maps/eu_ireland_mini.png" });

            Maps.Add(new Map() { title = "Italy", icon = "/data/maps/eu_italy_mini.png" });
            Maps.Add(new Map() { title = "Lithuania", icon = "/data/maps/eu_lithuania_mini.png" });
            Maps.Add(new Map() { title = "Low Countries", icon = "/data/maps/eu_low_countries_mini.png" });
            Maps.Add(new Map() { title = "Pripet Marshes", icon = "/data/maps/eu_pripet_marshes_mini.png" });
            Maps.Add(new Map() { title = "Pyrenees", icon = "/data/maps/eu_pyrenees_mini.png" });
            Maps.Add(new Map() { title = "Saxony", icon = "/data/maps/eu_saxony_mini.png" });
            Maps.Add(new Map() { title = "Scandinavia", icon = "/data/maps/eu_scandinavia_mini.png" });
            Maps.Add(new Map() { title = "Scotland", icon = "/data/maps/eu_scotland_mini.png" });
            Maps.Add(new Map() { title = "Spain", icon = "/data/maps/eu_spain_mini.png" });
            Maps.Add(new Map() { title = "Vistula Basin", icon = "/data/maps/eu_vistula_basin_mini.png" });

            Maps.Add(new Map() { title = "Wallachia", icon = "/data/maps/eu_wallachia_mini.png" });


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

        private string twitchAlign { get; set; }
        public string TwitchAlign
        {
            get { return twitchAlign; }
            set
            {
                twitchAlign = value;
                NotifyPropertyChanged("TwitchAlign");
            }
        }

        public ObservableCollection<Map> MapPool = new ObservableCollection<Map>(BuildMaps());


        ObservableCollection<Overlay> Overlays = new ObservableCollection<Overlay>();
        #endregion

        int CurrentPlayingOverlayIndex = 0;
        int CurrentPlayingOverlayCount = 0;

        int Version = 27;

        public SettingsDialog()
        {
            TimerAlign = "TopRight";
            ScheduleAlign = "BottomRight";
            MapAlign = "BottomLeft";
            BrandAlign = "TopLeft";
            EventAlign = "TopLeft";
            TwitchAlign = "TopLeft";



            Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata { DefaultValue = 15 });
            InitializeComponent();
            DataContext = this;

            AoE = new Cursor(Application.GetResourceStream(new Uri("pack://application:,,,/resources/Cursor.cur")).Stream);


            foreach (var mapTitle in Settings1.Default.MapPool)
            {
                var map = MapPool.FirstOrDefault(x => x.title == mapTitle);
                if (map != null)
                {
                    map.isSelected = true;
                }
            }
            cbScorePanel.IsChecked = Settings1.Default.ScorePanel;
            cbChromakey.IsChecked = Settings1.Default.Chromakey;
            tbTwitchChannel.Text = Settings1.Default.TwitchChannel;
            cbTwitch.IsChecked = Settings1.Default.TwitchPanel;
            cbOverlayLoop.IsChecked = Settings1.Default.OverlayLoop;
            cbAudio.IsChecked = Settings1.Default.Audio;
            cbCountdown.IsChecked = Settings1.Default.Countdown;
            cbSchedule.IsChecked = Settings1.Default.Schedule;
            cbPlayersPanel.IsChecked = Settings1.Default.PlayersPanelEnabled;

            TeamPanel.SelectedIndex = Settings1.Default.PlayersPanelIndex;

            lvMapPool.ItemsSource = MapPool;



            ICollectionView collectionView = CollectionViewSource.GetDefaultView(lvMapPool.ItemsSource);
            collectionView.SortDescriptions.Add(new SortDescription("title", ListSortDirection.Ascending));
            var view = (ICollectionViewLiveShaping)CollectionViewSource.GetDefaultView(lvMapPool.ItemsSource);
            view.IsLiveSorting = true;

            List<Logo> brandLogos =
    Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "data", "logo")).Where(x => Path.GetExtension(x).ToLower() == ".png").Select(x => new Logo { Name = Path.GetFileNameWithoutExtension(x), Path = x }).ToList();
            brandLogos.Insert(0, new Logo() { Name = "<NOT SET>", Path = "" });




            ICollectionView brand_view = CollectionViewSource.GetDefaultView(brandLogos);
            brand_view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            BrandLogos.ItemsSource = brand_view;
            var brand = brandLogos.FirstOrDefault(x => x.Name == Settings1.Default.BrandLogo);
            if (brand != null)
            {
                BrandLogos.SelectedItem = brand;
            }

            var animations = Directory.GetDirectories(Path.Combine(AppContext.BaseDirectory, "data", "animations"));
            SelectedOverlayIndex = 0;
            int i = 0;
            foreach (var anim in animations)
            {
                if (File.Exists(Path.Combine(anim, "video.mp4")) && File.Exists(Path.Combine(anim, "icon.png")) && File.Exists(Path.Combine(anim, "preview.png")))
                {
                    Overlays.Add(new Overlay() { isLooped = File.Exists(Path.Combine(anim, "looped")), title = Path.GetFileName(anim), preview = Path.Combine(anim, "preview.png"), icon = Path.Combine(anim, "icon.png"), video = Path.Combine(anim, "video.mp4") });
                    if (Path.GetFileName(anim) == Settings1.Default.SelectedOverlay)
                    {
                        SelectedOverlayIndex = i;
                    }
                    i++;
                }

            }



            List<Logo> eventLogos = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "data", "logo")).Where(x => Path.GetExtension(x).ToLower() == ".png").Select(x => new Logo { Name = Path.GetFileNameWithoutExtension(x), Path = x }).ToList();
            eventLogos.Insert(0, new Logo() { Name = "<NOT SET>", Path = "" });
            ICollectionView event_view = CollectionViewSource.GetDefaultView(eventLogos);
            brand_view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            EventLogos.ItemsSource = event_view;

            var eve = eventLogos.FirstOrDefault(x => x.Name == Settings1.Default.EventLogo);
            if (eve != null)
            {
                EventLogos.SelectedItem = eve;
            }

            if (File.Exists(Path.Combine(AppContext.BaseDirectory, "UpdateCounter.txt")))
            {
                int counter = Convert.ToInt32(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "UpdateCounter.txt")));
                if (counter < Version)
                {
                    gReleaseNotes.Visibility = Visibility.Visible;
                    BlurControl.Visibility = Visibility.Visible;
                    File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "UpdateCounter.txt"), Version.ToString());
                }
            }
            else
            {
                gReleaseNotes.Visibility = Visibility.Visible;
                BlurControl.Visibility = Visibility.Visible;
                File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "UpdateCounter.txt"), Version.ToString());
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



        private async void Window_Initialized(object sender, EventArgs e)
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

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SelectedOverlay == null)
            {
                (sender as Button).IsEnabled = true;
                return;
            }

            (sender as Button).IsEnabled = false;

            Settings1.Default.Audio = (bool)cbAudio.IsChecked;
            Settings1.Default.Save();


            if (cbTwitch.IsChecked == true)
            {
                try
                {
                    var data = new StringContent(
                        "[{ \"operationName\":\"ChannelShell\",\"variables\":{ \"login\":\"" + tbTwitchChannel.Text + "\"},\"extensions\":{ \"persistedQuery\":{ \"version\":1,\"sha256Hash\":\"580ab410bcd0c1ad194224957ae2241e5d252b2c5173d8e0cce9d32d5bb14efe\"} } }, {\"operationName\":\"ChannelAvatar\",\"variables\":{\"channelLogin\":\"" + tbTwitchChannel.Text + "\"},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\": \"84ed918aaa9aaf930e58ac81733f552abeef8ac26c0117746865428a7e5c8ab0\"}}},{\"operationName\": \"UseLive\",\"variables\": {\"channelLogin\": \"" + tbTwitchChannel.Text + "\"},\"extensions\": {\"persistedQuery\": {\"version\": 1,\"sha256Hash\": \"639d5f11bfb8bf3053b424d9ef650d04c4ebb7d94711d644afb08fe9a0fad5d9\"}}}]", Encoding.UTF8, "application/json");

                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Client-Id", "kimne78kx3ncx6brgo4mv6wki5h1ko");
                    HttpResponseMessage response = await client.PostAsync("https://gql.twitch.tv/gql", data);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var twitch = JsonSerializer.Deserialize<List<Root>>(responseBody);
                    twitch[1].data.user.followers.totalCount.ToString();
                }
                catch
                {

                    mbText.Text = "The entered Twitch channel does not exist.";
                    BlurControl.Visibility = Visibility.Visible;
                    gMessageBox.Visibility = Visibility.Visible;
                    (sender as Button).IsEnabled = true;
                    return;

                }

            }

            MainWindow mainWindow = new MainWindow();




            if (tbAnimation.IsChecked == true)
            {
                mainWindow.PreviewImage.Source = new BitmapImage(new Uri(SelectedOverlay.preview));

                mainWindow._libVLC = new LibVLC(); // "--reset-plugins-cache"
                mainWindow._mediaPlayer = new MediaPlayer(mainWindow._libVLC);

                mainWindow.Unloaded += mainWindow.Player_Unloaded;

                List<Overlay> loopedOverlays = Overlays.Where(x => x.isLooped).ToList();

                mainWindow.Animation.Loaded += (sender, e) =>
                {
                    mainWindow.Animation.MediaPlayer = mainWindow._mediaPlayer;
                    if (cbOverlayLoop.IsChecked == false)
                        mainWindow.Animation.MediaPlayer.Play(new Media(mainWindow._libVLC, new Uri(Path.Combine(AppContext.BaseDirectory, SelectedOverlay.video)), new string[] { ":input-repeat=65535" }));
                    else
                        mainWindow.Animation.MediaPlayer.Play(new Media(mainWindow._libVLC, new Uri(Path.Combine(AppContext.BaseDirectory, loopedOverlays[0].video))));
                    mainWindow.PreviewImage.Visibility = Visibility.Collapsed;
                };


                if (cbOverlayLoop.IsChecked == true)
                {


                    mainWindow._mediaPlayer.EndReached += (sender, e) =>
                {

                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        CurrentPlayingOverlayCount++;
                        if (CurrentPlayingOverlayCount >= 5)
                        {


                            if (CurrentPlayingOverlayIndex >= loopedOverlays.Count - 1)
                            {
                                CurrentPlayingOverlayIndex = 0;
                            }
                            else
                            {
                                CurrentPlayingOverlayIndex++;
                            }
                            CurrentPlayingOverlayCount = 0;
                        }
                        mainWindow._mediaPlayer.Play(new Media(mainWindow._libVLC, new Uri(Path.Combine(AppContext.BaseDirectory, loopedOverlays[CurrentPlayingOverlayIndex].video))));
                    });
                };
                }
            }
            else
            {
                mainWindow.PreviewImage.Source = new BitmapImage(new Uri(SelectedOverlay.preview));
            }

            if (BrandLogos.SelectedIndex == 0)
            {
                mainWindow.gBrandLogo.Visibility = Visibility.Collapsed;
            }
            else
            {
                mainWindow.iBrandLogo.Source = new BitmapImage(new Uri((BrandLogos.SelectedItem as Logo).Path));
            }

            if (EventLogos.SelectedIndex == 0)
            {
                mainWindow.gEventLogo.Visibility = Visibility.Collapsed;
            }
            else
            {
                mainWindow.iEventLogo.Source = new BitmapImage(new Uri((EventLogos.SelectedItem as Logo).Path));
            }

            if (cbCountdown.IsChecked == false)
            {
                mainWindow.gCountdown.Visibility = Visibility.Hidden;
            }


            var myCur = Application.GetResourceStream(new Uri("pack://application:,,,/resources/Cursor.cur")).Stream;
            mainWindow.Schedule.PreviewKeyDown += mainWindow.TextBox_PreviewKeyDown;
            mainWindow.Schedule.Cursor = new Cursor(myCur);
            myCur = Application.GetResourceStream(new Uri("pack://application:,,,/resources/Cursor.cur")).Stream;
            mainWindow.tbScoreText.Cursor = new Cursor(myCur);
            mainWindow.tbScoreText.PreviewKeyDown += mainWindow.TextBox_PreviewKeyDown;
            if (cbSchedule.IsChecked == true)
            {
                mainWindow.Schedule.Focus();
            }
            else
            {
                mainWindow.gSchedule.Visibility = Visibility.Hidden;
            }

            if (cbScorePanel.IsChecked == false)
            {
                mainWindow.gScorePanel.Visibility = Visibility.Hidden;
            }

            if (cbTwitch.IsChecked == false)
            {
                mainWindow.gTwitchInfo.Visibility = Visibility.Hidden;
            }

            if (cbChromakey.IsChecked == true)
            {
                mainWindow.PreviewImage.Opacity = 0;
                mainWindow.OverlayCanvas.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#ff00b140");
            }

            mainWindow.TwitchInfo.Interval = new TimeSpan(0, 0, 0);
            mainWindow.TwitchInfo.Tick += async (object s, EventArgs eventArgs) =>
            {
                mainWindow.TwitchInfo.Stop();
                mainWindow.TwitchInfo.Interval = new TimeSpan(0, 0, 30);
                try
                {
                    var data = new StringContent(
                        "[{ \"operationName\":\"ChannelShell\",\"variables\":{ \"login\":\"" + tbTwitchChannel.Text + "\"},\"extensions\":{ \"persistedQuery\":{ \"version\":1,\"sha256Hash\":\"580ab410bcd0c1ad194224957ae2241e5d252b2c5173d8e0cce9d32d5bb14efe\"} } }, {\"operationName\":\"ChannelAvatar\",\"variables\":{\"channelLogin\":\"" + tbTwitchChannel.Text + "\"},\"extensions\":{\"persistedQuery\":{\"version\":1,\"sha256Hash\": \"84ed918aaa9aaf930e58ac81733f552abeef8ac26c0117746865428a7e5c8ab0\"}}},{\"operationName\": \"UseLive\",\"variables\": {\"channelLogin\": \"" + tbTwitchChannel.Text + "\"},\"extensions\": {\"persistedQuery\": {\"version\": 1,\"sha256Hash\": \"639d5f11bfb8bf3053b424d9ef650d04c4ebb7d94711d644afb08fe9a0fad5d9\"}}}]", Encoding.UTF8, "application/json");


                    HttpResponseMessage response = await mainWindow.client.PostAsync("https://gql.twitch.tv/gql", data);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    //Debug.WriteLine(responseBody);
                    var twitch = JsonSerializer.Deserialize<List<Root>>(responseBody);
                    mainWindow.TwitchChannel.Text = twitch[0].data.userOrError.displayName;
                    mainWindow.twitchFollowers.Text = twitch[1].data.user.followers.totalCount.ToString();
                    if (twitch[0].data.userOrError.stream != null)
                        mainWindow.twitchViewers.Text = twitch[0].data.userOrError.stream.viewersCount.ToString();
                    if (twitch[2].data.user.stream != null)
                    {
                        var streamLength = DateTime.UtcNow - twitch[2].data.user.stream.createdAt;
                        mainWindow.twitchStreamLength.Text = $"{Math.Truncate(streamLength.TotalHours)} hr {streamLength.Minutes} min";
                    }

                    mainWindow.TwitchIcon = twitch[0].data.userOrError.profileImageURL;
                    mainWindow.NotifyPropertyChanged("TwitchIcon");

                }
                catch { }
                mainWindow.TwitchInfo.Start();
            };
            mainWindow.TwitchInfo.Start();

            List<Map> maps = lvMapPool.SelectedItems.Cast<Map>().Where(x => x.isSelected == true).ToList();


            if (maps.Count == 0)
            {
                mainWindow.tbTeam1Score.Visibility = Visibility.Hidden;
                mainWindow.tbTeam2Score.Visibility = Visibility.Hidden;
            }
            for (int i = 0; i < maps.Count; i++)
            {
                mainWindow.Team1Player1CivPool.Add(new Civ() { Tag = i });
                mainWindow.Team2Player1CivPool.Add(new Civ() { Tag = i });
                if (TeamPanel.SelectedIndex >= 1)
                {
                    mainWindow.Team1Player2CivPool.Add(new Civ() { Tag = i });
                    mainWindow.Team2Player2CivPool.Add(new Civ() { Tag = i });
                }
                if (TeamPanel.SelectedIndex >= 2)
                {
                    mainWindow.Team1Player3CivPool.Add(new Civ() { Tag = i });
                    mainWindow.Team2Player3CivPool.Add(new Civ() { Tag = i });
                }
            }
            mainWindow.lwTeam1Player1CivPool.ItemsSource = mainWindow.Team1Player1CivPool;
            mainWindow.lwTeam2Player1CivPool.ItemsSource = mainWindow.Team2Player1CivPool;

            if (TeamPanel.SelectedIndex >= 1)
            {
                mainWindow.lwTeam1Player2CivPool.ItemsSource = mainWindow.Team1Player2CivPool;
                mainWindow.lwTeam2Player2CivPool.ItemsSource = mainWindow.Team2Player2CivPool;
            }
            if (TeamPanel.SelectedIndex >= 2)
            {
                mainWindow.lwTeam1Player3CivPool.ItemsSource = mainWindow.Team1Player3CivPool;
                mainWindow.lwTeam2Player3CivPool.ItemsSource = mainWindow.Team2Player3CivPool;
            }

            if (TeamPanel.SelectedIndex == 0)
            {
                mainWindow.iTeam1.Source = new BitmapImage(new Uri("pack://application:,,,/resources/Team1_1v1.png"));
                mainWindow.iTeam2.Source = new BitmapImage(new Uri("pack://application:,,,/resources/Team2_1v1.png"));
            }
            else if (TeamPanel.SelectedIndex == 1)
            {
                mainWindow.iTeam1.Source = new BitmapImage(new Uri("pack://application:,,,/resources/Team1_2v2.png"));
                mainWindow.iTeam2.Source = new BitmapImage(new Uri("pack://application:,,,/resources/Team2_2v2.png"));
            }
            else
            {
                mainWindow.iTeam1.Source = new BitmapImage(new Uri("pack://application:,,,/resources/Team1_3v3.png"));
                mainWindow.iTeam2.Source = new BitmapImage(new Uri("pack://application:,,,/resources/Team2_3v3.png"));
            }


            if (cbPlayersPanel.IsChecked == false)
            {
                mainWindow.gPlayers.Visibility = Visibility.Hidden;
                mainWindow.gPlayersPanel.Visibility = Visibility.Hidden;
            }

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

                DoubleAnimation center11 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));

                center11.EasingFunction = ease1;

                DoubleAnimation center12 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.8));

                center12.EasingFunction = ease1;


                DoubleAnimation center7 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
                center7.EasingFunction = ease1;
                center7.BeginTime = TimeSpan.FromSeconds(6);


                DoubleAnimation center3 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.8));
                center3.BeginTime = TimeSpan.FromSeconds(6.4);
                ease1.EasingMode = EasingMode.EaseInOut;
                center3.EasingFunction = ease1;

                DoubleAnimation center4 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
                center4.BeginTime = TimeSpan.FromSeconds(6);
                center4.EasingFunction = ease1;

                DoubleAnimation center13 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.8));
                center13.BeginTime = TimeSpan.FromSeconds(6);
                ease1.EasingMode = EasingMode.EaseInOut;
                center13.EasingFunction = ease1;

                DoubleAnimation center14 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
                center14.BeginTime = TimeSpan.FromSeconds(6.4);
                center14.EasingFunction = ease1;


                mainWindow.tbBO.Text = $"Best of {maps.Count}";

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

                        mainWindow.BOTransparentStop.BeginAnimation(GradientStop.OffsetProperty, center11);
                        mainWindow.BOBlackStop.BeginAnimation(GradientStop.OffsetProperty, center12);

                        mainWindow.ibMapIcon.BeginAnimation(Image.OpacityProperty, center5);

                        await Task.Delay(1000);


                        if (maps.Count > 1)
                        {
                            mainWindow.stMap.BeginAnimation(ScaleTransform.ScaleXProperty, center7, HandoffBehavior.Compose);
                            mainWindow.stMap.BeginAnimation(ScaleTransform.ScaleYProperty, center7, HandoffBehavior.Compose);
                            mainWindow.MapTransparentStop.BeginAnimation(GradientStop.OffsetProperty, center3);
                            mainWindow.MapBlackStop.BeginAnimation(GradientStop.OffsetProperty, center4);
                            mainWindow.BOTransparentStop.BeginAnimation(GradientStop.OffsetProperty, center13);
                            mainWindow.BOBlackStop.BeginAnimation(GradientStop.OffsetProperty, center14);
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

            mainWindow.InputBindings.Add(new InputBinding(mainWindow.CountdownVisibility, new KeyGesture(Key.D1, ModifierKeys.Control)));
            mainWindow.InputBindings.Add(new InputBinding(mainWindow.ScheduleVisibility, new KeyGesture(Key.D2, ModifierKeys.Control)));
            mainWindow.InputBindings.Add(new InputBinding(mainWindow.MapPoolVisibility, new KeyGesture(Key.D3, ModifierKeys.Control)));
            mainWindow.InputBindings.Add(new InputBinding(mainWindow.BrandLogoVisibility, new KeyGesture(Key.D4, ModifierKeys.Control)));
            mainWindow.InputBindings.Add(new InputBinding(mainWindow.EventLogoVisibility, new KeyGesture(Key.D5, ModifierKeys.Control)));
            mainWindow.InputBindings.Add(new InputBinding(mainWindow.PlayerPanelVisibility, new KeyGesture(Key.D6, ModifierKeys.Control)));
            mainWindow.InputBindings.Add(new InputBinding(mainWindow.TwitchPanelVisibility, new KeyGesture(Key.D7, ModifierKeys.Control)));
            mainWindow.InputBindings.Add(new InputBinding(mainWindow.ScorePanelVisibility, new KeyGesture(Key.D8, ModifierKeys.Control)));

            mainWindow.InputBindings.Add(new InputBinding(mainWindow.ChromakeyVisibility, new KeyGesture(Key.D0, ModifierKeys.Control)));


            PresentationSource presentationsource = PresentationSource.FromVisual(this);
            Matrix m = presentationsource.CompositionTarget.TransformToDevice;

            double DpiWidthFactor = m.M11;
            double DpiHeightFactor = m.M22;

            mainWindow.VideoBox.Width = 1920 / DpiWidthFactor;
            mainWindow.VideoBox.Height = 1080 / DpiHeightFactor;


            mainWindow.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.Arrange(new Rect(0, 0, mainWindow.DesiredSize.Width, mainWindow.DesiredSize.Height));


            mainWindow.gCountdown.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.gCountdown.Arrange(new Rect(0, 0, mainWindow.gCountdown.DesiredSize.Width, mainWindow.gCountdown.DesiredSize.Height));

            mainWindow.gSchedule.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.gSchedule.Arrange(new Rect(0, 0, mainWindow.gSchedule.DesiredSize.Width, mainWindow.gSchedule.DesiredSize.Height));

            mainWindow.gMapPool.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.gMapPool.Arrange(new Rect(0, 0, mainWindow.gMapPool.DesiredSize.Width, mainWindow.gMapPool.DesiredSize.Height));


            mainWindow.gBrandLogo.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.gBrandLogo.Arrange(new Rect(0, 0, mainWindow.gBrandLogo.DesiredSize.Width, mainWindow.gBrandLogo.DesiredSize.Height));

            mainWindow.gEventLogo.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.gEventLogo.Arrange(new Rect(0, 0, mainWindow.gEventLogo.DesiredSize.Width, mainWindow.gEventLogo.DesiredSize.Height));

            mainWindow.gTwitchInfo.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.gTwitchInfo.Arrange(new Rect(0, 0, mainWindow.gTwitchInfo.DesiredSize.Width, mainWindow.gTwitchInfo.DesiredSize.Height));

            mainWindow.gScorePanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.gScorePanel.Arrange(new Rect(0, 0, mainWindow.gScorePanel.DesiredSize.Width, mainWindow.gScorePanel.DesiredSize.Height));

            mainWindow.gPlayersPanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.gPlayersPanel.Arrange(new Rect(0, 0, mainWindow.gPlayersPanel.DesiredSize.Width, mainWindow.gPlayersPanel.DesiredSize.Height));

            mainWindow.gPlayers.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.gPlayers.Arrange(new Rect(0, 0, mainWindow.gPlayers.DesiredSize.Width, mainWindow.gPlayers.DesiredSize.Height));

            mainWindow.gPlaybackControl.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            mainWindow.gPlaybackControl.Arrange(new Rect(0, 0, mainWindow.gPlaybackControl.DesiredSize.Width, mainWindow.gPlaybackControl.DesiredSize.Height));



            Point pCountdown = AlignPosition(TimerAlign, mainWindow.gCountdown.ActualWidth, mainWindow.gCountdown.ActualHeight);
            mainWindow.gCountdown.SetValue(Canvas.LeftProperty, pCountdown.X);
            mainWindow.gCountdown.SetValue(Canvas.TopProperty, pCountdown.Y);

            Point pSchedule = AlignPosition(ScheduleAlign, mainWindow.gSchedule.ActualWidth, mainWindow.gSchedule.ActualHeight);
            mainWindow.gSchedule.SetValue(Canvas.LeftProperty, pSchedule.X);
            mainWindow.gSchedule.SetValue(Canvas.TopProperty, pSchedule.Y);

            Point pMap = AlignPosition(MapAlign, mainWindow.gMapPool.ActualWidth, mainWindow.gMapPool.ActualHeight);
            mainWindow.gMapPool.SetValue(Canvas.LeftProperty, pMap.X);
            mainWindow.gMapPool.SetValue(Canvas.TopProperty, pMap.Y);

            Point pBrand = AlignPosition(BrandAlign, mainWindow.gBrandLogo.ActualWidth, mainWindow.gBrandLogo.ActualHeight);
            mainWindow.gBrandLogo.SetValue(Canvas.LeftProperty, pBrand.X);
            mainWindow.gBrandLogo.SetValue(Canvas.TopProperty, pBrand.Y);

            Point pEvent = AlignPosition(EventAlign, mainWindow.gEventLogo.ActualWidth, mainWindow.gEventLogo.ActualHeight);
            mainWindow.gEventLogo.SetValue(Canvas.LeftProperty, pEvent.X);
            mainWindow.gEventLogo.SetValue(Canvas.TopProperty, pEvent.Y);

            Point pTwitch = AlignPosition(TwitchAlign, mainWindow.gTwitchInfo.ActualWidth, mainWindow.gTwitchInfo.ActualHeight);
            mainWindow.gTwitchInfo.SetValue(Canvas.LeftProperty, 0.0);
            mainWindow.gTwitchInfo.SetValue(Canvas.TopProperty, 0.0);

            mainWindow.gScorePanel.SetValue(Canvas.LeftProperty, 1315.0);
            mainWindow.gScorePanel.SetValue(Canvas.TopProperty, 65.0);

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

        private void Window_Closed(object sender, EventArgs e)
        {
            Settings1.Default.ScorePanel = (bool)cbScorePanel.IsChecked;
            Settings1.Default.Chromakey = (bool)cbChromakey.IsChecked;
            Settings1.Default.TwitchChannel = tbTwitchChannel.Text;
            Settings1.Default.TwitchPanel = (bool)cbTwitch.IsChecked;
            Settings1.Default.OverlayLoop = (bool)cbOverlayLoop.IsChecked;
            Settings1.Default.Audio = (bool)cbAudio.IsChecked;
            Settings1.Default.Schedule = (bool)cbSchedule.IsChecked;
            Settings1.Default.Countdown = (bool)cbCountdown.IsChecked;
            Settings1.Default.PlayersPanelEnabled = (bool)cbPlayersPanel.IsChecked;

            Settings1.Default.PlayersPanelIndex = TeamPanel.SelectedIndex;
            Settings1.Default.SelectedOverlay = Overlays[SelectedOverlayIndex].title;
            var maps = new StringCollection();
            maps.AddRange(MapPool.Where(x => x.isSelected == true).Select(x => x.title).ToArray());
            Settings1.Default.MapPool = maps;
            Settings1.Default.BrandLogo = (BrandLogos.SelectedItem as Logo).Name;
            Settings1.Default.EventLogo = (EventLogos.SelectedItem as Logo).Name;
            Settings1.Default.Save();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string targetURL = "https://docs.google.com/forms/d/e/1FAIpQLScJUrydEvq3TmhGmA2dMmK1mk8tvWGqtDH_9DvmUXK-LTGD9Q/viewform?usp=sf_link";
            var psi = new ProcessStartInfo
            {
                FileName = targetURL,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void bTwitchAlign_Click(object sender, RoutedEventArgs e)
        {
            switch (TwitchAlign)
            {
                case "TopRight":
                    TwitchAlign = "BottomRight";
                    break;
                case "BottomRight":
                    TwitchAlign = "BottomLeft";
                    break;
                case "BottomLeft":
                    TwitchAlign = "TopLeft";
                    break;
                case "TopLeft":
                    TwitchAlign = "TopRight";
                    break;
            }
        }
    }

}
