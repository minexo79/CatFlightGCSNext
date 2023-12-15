using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static CatFlightGCSNext.UI.Controls.HudControl;

namespace CatFlightGCSNext.UI.Controls
{
    /// <summary>
    /// Interaction logic for hudControl.xaml
    /// </summary>
    public partial class HudControl : UserControl
    {
        /// <summary>
        /// 控制HUD顯示與否的類別
        /// </summary>
        public class HudVisible : INotifyPropertyChanged
        {
            private bool _pilotHudVisible = true;
            /// <summary>
            /// Pilot Hud Visible
            /// </summary>
            public bool pilotHudVisible
            {
                get => _pilotHudVisible;
                set
                {
                    _pilotHudVisible = value;
                    OnPropertyChanged(nameof(pilotHudVisible));
                }
            }

            private bool _compassVisible = true;
            /// <summary>
            /// Compass Visible
            /// </summary>
            public bool compassVisible
            {
                get => _compassVisible;
                set
                {
                    _compassVisible = value;
                    OnPropertyChanged(nameof(compassVisible));
                }
            }

            private bool _satelliteVisible = true;
            /// <summary>
            /// GPS Satellite & HDOP Visible
            /// </summary>
            public bool satelliteVisible 
            { 
                get => _satelliteVisible; 
                set 
                {
                    _satelliteVisible = value;
                    OnPropertyChanged(nameof(satelliteVisible));
                } 
            }

            private bool _energyVisible = true;
            /// <summary>
            /// Energy Volt & Amp Visible
            /// </summary>
            public bool energyVisible
            {
                get => _energyVisible;
                set
                {
                    _energyVisible = value;
                    OnPropertyChanged(nameof(energyVisible));
                }
            }

            private bool _flightSpeedVisible = true;
            /// <summary>
            /// Horizontal Speed & Vertical Speed Visible
            /// </summary>
            public bool flightSpeedVisible
            {
                get => _flightSpeedVisible;
                set
                {
                    _flightSpeedVisible = value;
                    OnPropertyChanged(nameof(flightSpeedVisible));
                }
            }

            private bool _flightInfoVisible = true;
            /// <summary>
            /// Flight Time & Flight Distance & Home Dist / Alt Visible
            /// </summary>
            public bool flightInfoVisible
            {
                get => _flightInfoVisible;
                set
                {
                    _flightInfoVisible = value;
                    OnPropertyChanged(nameof(flightInfoVisible));
                }
            }

            /// <summary>
            /// Event Handler On Property Changed
            /// </summary>
            public event PropertyChangedEventHandler? PropertyChanged;
            public void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            }
        }

        public HudVisible hudVisible { get; set; } = new HudVisible();

        public HudControl()
        {
            InitializeComponent();

            hudVisible.PropertyChanged += HudVisible_PropertyChanged;
        }

        private void HudVisible_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "pilotHudVisible")
            {
                pilotHudView.Visibility     = hudVisible.pilotHudVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (e.PropertyName == "compassVisible")
            {
                compassView.Visibility      = hudVisible.compassVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (e.PropertyName == "satelliteVisible")
            {
                gpsCountView.Visibility     = hudVisible.satelliteVisible ? Visibility.Visible : Visibility.Collapsed;
                gpsHdopsView.Visibility     = hudVisible.satelliteVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (e.PropertyName == "energyVisible")
            {
                energyVoltView.Visibility   = hudVisible.energyVisible ? Visibility.Visible : Visibility.Collapsed;
                energyAmpView.Visibility    = hudVisible.energyVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (e.PropertyName == "flightSpeedVisible")
            {
                hSpeedView.Visibility       = hudVisible.flightSpeedVisible ? Visibility.Visible : Visibility.Collapsed;
                vSpeedView.Visibility       = hudVisible.flightSpeedVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (e.PropertyName == "flightInfoVisible")
            {
                flightDistView.Visibility   = hudVisible.flightInfoVisible ? Visibility.Visible : Visibility.Collapsed;
                homeDistView.Visibility     = hudVisible.flightInfoVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                // Do Nothing
            }
        }
    }
}
