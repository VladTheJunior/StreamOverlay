using ColorPicker;
using ColorPicker.Models;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace StreamOverlay
{
    /// <summary>
    /// Логика взаимодействия для ThumbnailGenerator.xaml
    /// </summary>
    public partial class ThumbnailGenerator : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        }
        double FirstXPos, FirstYPos;
        object MovingObject;

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



        private void SaveControlImage(FrameworkElement control, string filename)
        {
            // Get the size of the Visual and its descendants.
            Rect rect = VisualTreeHelper.GetDescendantBounds(control);

            // Make a DrawingVisual to make a screen
            // representation of the control.
            DrawingVisual dv = new DrawingVisual();

            // Fill a rectangle the same size as the control
            // with a brush containing images of the control.
            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(control);
                ctx.DrawRectangle(brush, null, new Rect(rect.Size));
            }

            // Make a bitmap and draw on it.
            int width = (int)control.ActualWidth;
            int height = (int)control.ActualHeight;
            RenderTargetBitmap rtb = new RenderTargetBitmap(
                width, height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);

            // Make a PNG encoder.
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            // Save the file.
            using (FileStream fs = new FileStream(Path.Combine(Environment.CurrentDirectory, filename),
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                encoder.Save(fs);
            }


            Process.Start(new ProcessStartInfo()
            {
                FileName = "explorer",
                Arguments = "/e, /select, \"" + Path.Combine(Environment.CurrentDirectory, filename) + "\""
            });
            Process.Start(new ProcessStartInfo()
            {
                FileName = Path.Combine(Environment.CurrentDirectory, filename),
                UseShellExecute = true,
                Verb = "open"
            });
        }

        public ObservableCollection<FontFamily> fonts = new ObservableCollection<FontFamily>(Fonts.SystemFontFamilies);
        

        public ThumbnailGenerator()
        {
            DataContext = this;
            InitializeComponent();
            AoE = new Cursor(Application.GetResourceStream(new Uri("pack://application:,,,/resources/Cursor.cur")).Stream);

            fonts.Add(new FontFamily("/Fonts/#Formal436 BT"));
            fonts.Add(new FontFamily("Fonts/#Segoe Print"));
            fonts.Add(new FontFamily("/Fonts/#Monotype Corsiva"));
            fonts.Add(new FontFamily("/Fonts/#Trajan Pro 3"));
            ICollectionView view = CollectionViewSource.GetDefaultView(fonts);
            view.SortDescriptions.Add(new SortDescription("Source", ListSortDirection.Ascending));
            FontTitle.ItemsSource = view;
            FontVS.ItemsSource = view;
            FontRound.ItemsSource = view;
            FontPlayer.ItemsSource = view;

        }

        private void MovedPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //In this event, we get current mouse position on the control to use it in the MouseMove event.
            FirstXPos = e.GetPosition(sender as FrameworkElement).X;
            FirstYPos = e.GetPosition(sender as FrameworkElement).Y;
            MovingObject = sender;
        }


        void MovedPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MovingObject = null;
        }

        private void MovedMouseMove(object sender, MouseEventArgs e)
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Thumbnails"));
            SaveControlImage(Thumbnail, Path.Combine("Thumbnails", "[" + Title.Text + " - " + Round.Text + "] " + Player1.Text + " vs. " + Player2.Text + ".png"));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            // Get the size of the Visual and its descendants.
            Rect rect = VisualTreeHelper.GetDescendantBounds(Thumbnail);

            // Make a DrawingVisual to make a screen
            // representation of the control.
            DrawingVisual dv = new DrawingVisual();

            // Fill a rectangle the same size as the control
            // with a brush containing images of the control.
            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(Thumbnail);
                ctx.DrawRectangle(brush, null, new Rect(rect.Size));
            }

            // Make a bitmap and draw on it.
            int width = (int)Thumbnail.ActualWidth;
            int height = (int)Thumbnail.ActualHeight;
            RenderTargetBitmap rtb = new RenderTargetBitmap(
                width, height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);

            // Make a PNG encoder.
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            var bitmapImage = new BitmapImage();
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                YoutubePreview.Source = bitmapImage;
            }
        }

       

        public void updatePositionV()
        {
            Player2Thumbnail.SetValue(Canvas.LeftProperty, (double)VsThumbnail.GetValue(Canvas.LeftProperty) + VsThumbnail.ActualWidth / 2 - Player2Thumbnail.ActualWidth / 2);
            Player1Thumbnail.SetValue(Canvas.LeftProperty, (double)VsThumbnail.GetValue(Canvas.LeftProperty) + VsThumbnail.ActualWidth / 2 - Player1Thumbnail.ActualWidth / 2);
        }

        public void updatePositionH()
        {
            Player2Thumbnail.SetValue(Canvas.LeftProperty, (double)VsThumbnail.GetValue(Canvas.LeftProperty) - VsThumbnail.ActualWidth / 2 - Player2Thumbnail.ActualWidth);
            Player1Thumbnail.SetValue(Canvas.LeftProperty, (double)VsThumbnail.GetValue(Canvas.LeftProperty) + VsThumbnail.ActualWidth / 2 + VsThumbnail.ActualWidth);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            updatePositionV();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            updatePositionH();
        }

        private void FontColorTitle_ColorChanged(object sender, RoutedEventArgs e)
        {
            TitleThumbnail.Foreground = new SolidColorBrush(FontColorTitle.SelectedColor);
        }

        private void FontColorBorder_ColorChanged(object sender, RoutedEventArgs e)
        {
            bThumbnailBorder.BorderBrush = new SolidColorBrush(FontColorBorder.SelectedColor); 
        }

        private void ShadowColorTitle_ColorChanged(object sender, RoutedEventArgs e)
        {
            DropShadowTitle.Color = ShadowColorTitle.SelectedColor;
        }

        private void FontColorRound_ColorChanged(object sender, RoutedEventArgs e)
        {
            RoundThumbnail.Foreground = new SolidColorBrush(FontColorRound.SelectedColor);
        }

        private void ShadowColorRound_ColorChanged(object sender, RoutedEventArgs e)
        {
            DropShadowRound.Color = ShadowColorRound.SelectedColor;
        }

        private void FontColorPlayer1_ColorChanged(object sender, RoutedEventArgs e)
        {
            Player1Thumbnail.Foreground = new SolidColorBrush(FontColorPlayer1.SelectedColor);
        }

        private void FontColorPlayer2_ColorChanged(object sender, RoutedEventArgs e)
        {
            Player2Thumbnail.Foreground = new SolidColorBrush(FontColorPlayer2.SelectedColor);
        }

        private void ShadowColorPlayer_ColorChanged(object sender, RoutedEventArgs e)
        {
            DropShadowPlayer.Color = ShadowColorPlayer.SelectedColor;
        }

        private void FontColorVS_ColorChanged(object sender, RoutedEventArgs e)
        {
            VsThumbnail.Foreground = new SolidColorBrush(FontColorVS.SelectedColor);
        }

        private void ShadowColorVS_ColorChanged(object sender, RoutedEventArgs e)
        {
            DropShadowVs.Color = ShadowColorVS.SelectedColor;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }


    public class BorderThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            return new Thickness((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToUpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string)
            {
                return ((string)value).ToUpper();
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BrushColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return ((SolidColorBrush)value).Color;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
