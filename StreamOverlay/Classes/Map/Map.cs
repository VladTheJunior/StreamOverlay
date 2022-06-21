using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamOverlay.Classes.Map
{
    public class Map : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        }
        public string title { get; set; }
        public string icon { get; set; }

        

        private int order = 0;
        public int Order
        {
            get { return order; }
            set
            {
                order = value;
                NotifyPropertyChanged("Order");
            }
        }
    }
}
