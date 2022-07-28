using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamOverlay.Classes.Map;
using StreamOverlay.Classes.Settings;

namespace StreamOverlay
{
    public class Setting : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        private int appVersion;
        public int AppVersion
        {
            get
            {
                return appVersion;
            }
            set
            {
                appVersion = value;
                NotifyPropertyChanged("AppVersion");
            }
        }
        private int appLanguage;
        public int AppLanguage
        {
            get
            {
                return appLanguage;
            }
            set
            {
                appLanguage = value;
                NotifyPropertyChanged("AppLanguage");
            }
        }

        private string selectedOverlay;
        public string SelectedOverlay
        {
            get
            {
                return selectedOverlay;
            }
            set
            {
                selectedOverlay = value;
                NotifyPropertyChanged("SelectedOverlay");
            }
        }
        private int elementsStyle;
        public int ElementsStyle
        {
            get
            {
                return elementsStyle;
            }
            set
            {
                elementsStyle = value;
                NotifyPropertyChanged("ElementStyle");
            }
        }
        private double hudScale = 100;
        public double HUDScale
        {
            get { return hudScale; }
            set
            {
                hudScale = value;
                NotifyPropertyChanged("HUDScale");
            }
        }
        private bool chromakey;
        public bool Chromakey
        {
            get
            {
                return chromakey;
            }
            set
            {
                chromakey = value;
                NotifyPropertyChanged("Chromakey");
            }
        }
        private bool autoplaySound;
        public bool AutoplaySound
        {
            get { return autoplaySound; }
            set
            {
                autoplaySound = value;
                NotifyPropertyChanged("AutoplaySound");
            }
        }
        private bool loopBackgrounds;
        public bool LoopBackgrounds
        {
            get { return loopBackgrounds; }
            set
            {
                loopBackgrounds = value;
                NotifyPropertyChanged("LoopBackgrounds");
            }
        }

        public string Team1Persons { get; set; } = "<NOT SET>";
        public string Team2Persons { get; set; } = "<NOT SET>";


        public List<string> MapPool { get; set; } = new List<string>();
        public Element Countdown { get; set; } = new Element();
        public Element Schedule { get; set; } = new Element();
        
        public Element EventLogo { get; set; } = new Element();
        public Element BrandLogo { get; set; } = new Element();
        public Element TwitchPanel { get; set; } = new Element();
        public Element ScoreInputPanel { get; set; } = new Element();
        public Element PlayersPanel { get; set; } = new Element();
        public Element Map { get; set; } = new Element();

    }
}
