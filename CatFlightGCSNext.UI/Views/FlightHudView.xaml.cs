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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatFlightGCSNext.UI.Views
{
    /// <summary>
    /// Interaction logic for HudControl.xaml
    /// </summary>
    public partial class FlightHudView : UserControl
    {
        // 使用DependencyProperty註冊，讓頂層UI可以修改參數
        public static DependencyProperty hudImageProperty = 
            DependencyProperty.Register("_hudImage", typeof(string), typeof(Image));    // Image

        public static DependencyProperty hudTypeProperty =
            DependencyProperty.Register("_hudType", typeof(string), typeof(string));    // Type

        // HUD Image Variable
        public string _hudImage
        {
            get => (string)GetValue(hudImageProperty);
            set
            {
                SetValue(hudImageProperty, value);
                hudImage.Source = (ImageSource)FindResource(value);     // Change Hud Image From Resource
            }
        }

        // HUD Type Variable
        public string _hudType
        {
            get => (string)GetValue(hudTypeProperty);
            set
            {
                SetValue(hudTypeProperty, value);
                hudType.Text = value;                                   // Change Hud Type From Resource
            }
        }

        public FlightHudView()
        {
            InitializeComponent();
            GenerateRandomColor(hudType, 0x5F);
        }

        public void GenerateRandomColor(object control, byte opacity)
        {
            byte r = (byte)new Random().Next(0, 256);
            byte g = (byte)new Random().Next(0, 256);
            byte b = (byte)new Random().Next(0, 256);
            ((TextBlock)control).Background = new SolidColorBrush(Color.FromArgb(opacity, r, g, b));
        }
    }
}
