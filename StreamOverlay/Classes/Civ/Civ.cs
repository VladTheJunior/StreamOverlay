using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamOverlay.Classes.Civ
{
    public class Civ: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

    public void NotifyPropertyChanged(string propName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

    }
        public int Tag { get; set; }
    public int Id { get; set; }
        public int Status { get; set; }


        public string Icon
        {
            get
            {
                switch (Id)
                {
                    case 0: return "/data/civs/flag_hc_random.png";
                    case 1: return "/data/civs/flag_hc_spanish.png";
                    case 2: return "/data/civs/flag_hc_british.png";
                    case 3: return "/data/civs/flag_hc_french.png";
                    case 4: return "/data/civs/flag_hc_portuguese.png";
                    case 5: return "/data/civs/flag_hc_dutch.png";
                    case 6: return "/data/civs/flag_hc_russian.png";
                    case 7: return "/data/civs/flag_hc_german.png";
                    case 8: return "/data/civs/flag_hc_ottoman.png";
                    case 9: return "/data/civs/flag_hc_iroquois.png";
                    case 10: return "/data/civs/flag_hc_sioux.png";
                    case 11: return "/data/civs/flag_hc_aztec.png";
                    case 12: return "/data/civs/flag_hc_japanese.png";
                    case 13: return "/data/civs/flag_hc_chinese.png";
                    case 14: return "/data/civs/flag_hc_indian.png";
                    case 15: return "/data/civs/flag_hc_inca.png";
                    case 16: return "/data/civs/flag_hc_swedish.png";
                    case 17: return "/data/civs/flag_hc_american.png";
                    case 18: return "/data/civs/flag_hc_ethiopian.png";
                    case 19: return "/data/civs/flag_hc_hausa.png";
                    case 20: return "/data/civs/flag_hc_mexican.png";
                    
                }
                return "/data/civs/flag_hc_random.png";
            }
        }

        public string StatusIcon
        {
            get
            {
                switch (Status)
                {
                    case 0: return "";
                    case 1: return "/resources/win.png";
                    case 2: return "/resources/loss.png";
                }
                return "";
            }
        }


        public void NextStatus()
        {
            if (Status == 2)
            {
                Status = 0;
            }
            else
            {
                Status += 1;
            }
            NotifyPropertyChanged("Status");
            NotifyPropertyChanged("StatusIcon");
        }

        public void NextId()
        {
            if (Id == 20)
            {
                Id = 0;
            }
            else
            {
                Id += 1;
            }
            NotifyPropertyChanged("Id");
            NotifyPropertyChanged("Icon");
        }
    }
}
