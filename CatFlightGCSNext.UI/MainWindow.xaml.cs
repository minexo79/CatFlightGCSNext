using CatFlightGCSNext.UI.Controls;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CatFlightGCSNext.UI.Controls.HudControl;

namespace CatFlightGCSNext.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public string droneLocation { get => "飛機位置：N/A N/A N/A"; }
        public HudControl.HudVisible hudVisible { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;            // Set Data Binding To This Class

            hudVisible = hudControl.hudVisible; // Binding Exist Hud On XAML

            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mapControl.InitializeMap(9);
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            mapControl.CloseMapCache();
        }

        private void mapChange_Click(object sender, RoutedEventArgs e)
        {
            string mapName = ((MenuItem)sender).Name.Replace("map_", "");

            mapControl.ChangeMapProviders(mapName);
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow help = new HelpWindow();
            help.Show();
        }
    }
}