using LibVLCSharp.Shared;
using System.Windows;

namespace StreamOverlay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Core.Initialize();
        }
    }

}
