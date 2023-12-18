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
using CatFlightGCSNext.Core;

namespace CatFlightGCSNext.UI.Views
{
    /// <summary>
    /// Interaction logic for FlightModeView.xaml
    /// </summary>
    public partial class FlightModeView : UserControl
    {
        private int _flightMode = -1;
        public int FlightMode
        {
            get => _flightMode;
            set
            {
                // Change Text When Flight Mode Changed
                _flightMode = value;

                txtFlightMode.Dispatcher.Invoke(delegate
                {
                    if (_flightMode >= 0 && _flightMode < FlightParameters.FlightMode.Length)
                        txtFlightMode.Text = FlightParameters.FlightMode[_flightMode];
                    else 
                        txtFlightMode.Text = "未知";
                });
            }
        }

        public FlightModeView()
        {
            InitializeComponent();
        }
    }
}
