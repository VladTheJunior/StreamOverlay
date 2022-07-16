using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StreamOverlay
{
    public enum PositionType
    {
        TopRight,
        BottomLeft,
        BottomRight,
        TopLeft,
        Custom,
    }
    public class Position : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        private double x;

        public double X
        {
            get { return x; }
            set
            {
                x = value;
                NotifyPropertyChanged("X");
            }
        }

        private double y;

        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                NotifyPropertyChanged("Y");
            }
        }

        private PositionType type;
        public PositionType Type
        {
            get { return type; }
            set
            {
                type = value;
                NotifyPropertyChanged("Type");
            }
        }

        public void GetPositionFromType(double width, double height)
        {
            switch (Type)
            {
                case PositionType.TopRight:
                    X = 1920 - 40 - width;
                        Y = 20;
                    break;
                case PositionType.BottomLeft:
                    X = 20;
                    Y=1080 - 40 - height;
                    break;
                case PositionType.BottomRight:
                    X = 1920 - 40 - width;
                        Y=1080 - 40 - height;
                    break;
                case PositionType.TopLeft:
                    X = 20;
                    Y= 20;
                    break;
            }
        }

        public static PositionType GetTypeFromPosition(Point point, double width, double height)
        {
            if (point.X == 1920 - 40 - width && point.Y == 20)
                return PositionType.TopRight;

            if (point.X == 20 && point.Y == 1080 - 40 - height)
                return PositionType.BottomLeft;

            if (point.X == 1920 - 40 - width && point.Y == 1080 - 40 - height)
                return PositionType.BottomRight;


            if (point.X == 20 && point.Y == 20)
                return PositionType.TopLeft;

            return PositionType.Custom;
        }

    }
}
