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
using System.Windows.Shapes;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;
using LibVLCSharp.Shared;
using System.Windows.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace StreamOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool needClose = true;


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
                        if (iEventLogo.Visibility == Visibility.Visible)
                        {
                            iEventLogo.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            iEventLogo.Visibility = Visibility.Visible;
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
                        if (iBrandLogo.Visibility == Visibility.Visible)
                        {
                            iBrandLogo.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            iBrandLogo.Visibility = Visibility.Visible;
                        }

                    }));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        }

        public DispatcherTimer AnimateMapPool = new DispatcherTimer();


        public LibVLC _libVLC;
        public MediaPlayer _mediaPlayer;

        public MainWindow()
        {
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
            //In this event, we get current mouse position on the control to use it in the MouseMove event.
            FirstXPos = e.GetPosition(sender as FrameworkElement).X;
            FirstYPos = e.GetPosition(sender as FrameworkElement).Y;
            MovingObject = sender;
        }

        private void ObjectMouseUp(object sender, MouseButtonEventArgs e)
        {
            MovingObject = null;
        }

        private void OverlayCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if  (sender is not TextBox)
                this.Focus();
        }

        private void Schedule_MouseEnter(object sender, MouseEventArgs e)
        {
            Schedule.CaretBrush = new SolidColorBrush(Colors.WhiteSmoke);
        }

        private void Schedule_MouseLeave(object sender, MouseEventArgs e)
        {
            Schedule.CaretBrush = new SolidColorBrush(Colors.Transparent);
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
