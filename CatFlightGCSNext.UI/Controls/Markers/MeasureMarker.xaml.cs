using GMap.NET.WindowsPresentation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows;
using CatFlightGCSNext.UI.Utils;

namespace CatFlightGCSNext.UI.Controls.Markers
{
    /// <summary>
    /// Interaction logic for MeasureMarker.xaml
    /// </summary>
    public partial class MeasureMarker
    {
        MapControl control;
        GMapMarker marker;
        Popup popup;
        Label label;

        /// <summary>
        /// Marker For Measure Two Point Distance
        /// </summary>
        /// <param name="_control"></param>
        /// <param name="_marker"></param>
        /// <param name="isTwoPoint"></param>
        /// <param name="dist"></param>
        public MeasureMarker(MapControl _control, GMapMarker _marker, bool isTwoPoint = false, string content = "")
        {
            InitializeComponent();

            this.control    = _control;
            this.marker     = _marker;
            this.popup      = new Popup();
            this.label      = new Label()
            {
                Background = ColorConvert.ToBrush("#303030"),
                Foreground = Brushes.White,
                Padding = new Thickness(5),
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                BorderThickness = new Thickness(0)
            };

            this.MouseEnter += MeasureMarker_MouseEnter;
            this.MouseLeave += MeasureMarker_MouseLeave;

            label.Content = content;

            popup.Placement = PlacementMode.MousePoint;
            popup.Child = label;
        }

        // Mouse Leave: Do Not Show Popup.
        private void MeasureMarker_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.popup.IsOpen = false;
        }

        // Mouse Enter: Show Popup.
        private void MeasureMarker_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.popup.IsOpen = true;
        }
    }
}
