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

namespace CatFlightGCSNext.UI.Views
{
    /// <summary>
    /// Interaction logic for FlightModeView.xaml
    /// </summary>
    public partial class FlightArmedView : UserControl
    {
        private bool _armed = false;
        public bool armed
        {
            get => _armed;
            set
            {
                _armed = value;

                txtArmed.Dispatcher.Invoke(delegate 
                {
                    txtArmed.Text = value ? "ARMED" : "DISARMED";
                    txtArmed.Background = value ? Brushes.Red : Brushes.Transparent;
                });
            }
        }

        public FlightArmedView()
        {
            InitializeComponent();
        }
    }
}
