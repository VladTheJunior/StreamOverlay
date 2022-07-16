using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamOverlay.Classes.Settings
{
    public class Element: INotifyPropertyChanged
    {
                public event PropertyChangedEventHandler PropertyChanged;

    public void NotifyPropertyChanged(string propName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

    }
    private Position position = new Position();
        public Position Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                NotifyPropertyChanged("Position");
            }
        }
        private double zoom = 0;
        public double Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                zoom = value;
                NotifyPropertyChanged("Zoom");
            }
        }
        private object source;
        public object Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                NotifyPropertyChanged("Source");
            }
        }
        private bool isVisible = false;
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                NotifyPropertyChanged("IsVisible");
            }
        }
    }
}
