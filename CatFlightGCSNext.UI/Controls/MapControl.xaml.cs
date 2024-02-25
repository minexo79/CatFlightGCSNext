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
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using CatFlightGCSNext.UI.Controls.Markers;
using CatFlightGCSNext.UI.Utils;
using CatFlightGCSNext.Core.Utils;
using System.ComponentModel;

namespace CatFlightGCSNext.UI.Controls
{
    /// <summary>
    /// Interaction logic for MapControl.xaml
    /// </summary>
    public partial class MapControl : UserControl
    {
        public int zoomLevel { get => (int)map.Zoom; set => map.Zoom = value; }
        public bool isRanging       = false;
        public bool isLinePlanning  = false;

        public MapControl()
        {
            InitializeComponent();

            ChangeMapMenu(0);
        }

        // Initialize Map Control
        public void InitializeMap(int zoom)
        {
            map.Manager.Mode = AccessMode.ServerAndCache;               // Using Server And Cache Mode

            map.MapProvider = GMapProviders.BingSatelliteMap;           // Using Bing Map
            map.ShowCenter = false;                                     // Do not show center cross
            map.DragButton = MouseButton.Left;                          // Left Button To Drag Map
            map.MinZoom = 5;
            map.MaxZoom = 20;
            map.Zoom = zoom;                                            // Set Zoom Level
            map.Position = new PointLatLng(23.9745699, 120.9254906);    // Locate Nantou As Initial Position
        }

        // Change Map Providers
        public void ChangeMapProviders(string providerName)
        {
            switch(providerName)
            {
                case "Google_Default":
                    map.MapProvider = GMapProviders.GoogleMap;
                    break;
                case "Google_Satellite":
                    map.MapProvider = GMapProviders.GoogleSatelliteMap;
                    break;
                case "Google_Terrain":
                    map.MapProvider = GMapProviders.GoogleTerrainMap;
                    break;
                case "Google_Hybird":
                    map.MapProvider = GMapProviders.GoogleHybridMap;
                    break;
                case "Bing_Default":
                    map.MapProvider = GMapProviders.BingMap;
                    break;
                case "Bing_OS":
                    map.MapProvider = GMapProviders.BingOSMap;
                    break;
                case "Bing_Satellite":
                    map.MapProvider = GMapProviders.BingSatelliteMap;
                    break;
                case "Bing_Hybrid":
                    map.MapProvider = GMapProviders.BingHybridMap;
                    break;
                case "OSM_Default":
                    map.MapProvider = GMapProviders.OpenStreetMap;
                    break;
            }
            map.UpdateLayout();
        }

        // Close GMap Completely
        // https://stackoverflow.com/questions/38096482/gmap-net-process-takes-a-long-time-to-close-after-window-closes
        public void CloseMapCache()
        {
            map.Manager.CancelTileCaching();                            // Close Map Caching
        }

        public void ChangeMapMenu(int index)
        {
            mapMenu.Items.Clear();

            switch(index)
            {
                case 0:
                    mapMenu.Items.Add(new MenuItem() { Header = "飛到這裡" });
                    mapMenu.Items.Add(new MenuItem() { Header = "起飛" });
                    mapMenu.Items.Add(new MenuItem() { Header = "降落" });
                    mapMenu.Items.Add(new MenuItem() { Header = "返航" });
                    mapMenu.Items.Add(new MenuItem() { Header = "設定Home點" });
                    break;
                case 1:
                    mapMenu.Items.Add(new MenuItem() { Header = "刪除航點" });
                    mapMenu.Items.Add(new MenuItem() { Header = "插入航點" });
                    mapMenu.Items.Add(new MenuItem() { Header = "計算SRTM" });
                    break;
            }
        }

        /// Measurement Area
        /// ============================================================================================================
        List<GMapMarker> measMarker = new List<GMapMarker>();
        GMapRoute measRoute;

        /// <summary>
        /// Measure The Distane Between A And B Position
        /// </summary>
        public void Measure_Distance()
        {
            // Clear All Markers On Map
            map.Markers.Clear();
            measMarker.Clear();
            measRoute?.Clear();

            if (!isRanging)
                // Binding A Event That Set A Start Position To Measure The Distance 
                map.MouseDown += Map_Measure_MouseDown;
            else
                // Debinding Measurement Event
                map.MouseDown -= Map_Measure_MouseDown;

            isRanging = !isRanging;
        }


        private void Map_Measure_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (map.Markers.Count > 2)      // > 2 Point: Clear All Markers On Map
                                                // In GMap.NET.WindowsPresentation, All The Draws Is Point
                {
                    map.Markers.Clear();
                    measMarker.Clear();
                    measRoute?.Clear();
                }

                if (map.Markers.Count == 0)     // 0 Point: Create First Marker
                {
                    // Get Mouse Position
                    var position = e.GetPosition(this);
                    
                    PointLatLng mapPos = map.FromLocalToLatLng((int)position.X, (int)position.Y);

                    string _lat = "";
                    string _lng = "";

                    LatLngConvert.ConvertToDecimal6(mapPos, out _lat, out _lng);

                    // Marker First Point
                    // In WPF, We Don't Use The GMap Overlay To Create The Markers
                    // https://stackoverflow.com/a/42733736
                    GMapMarker marker1 = new GMapMarker(mapPos);
                    {
                        marker1.Shape = new MeasureMarker(this, marker1, false, $"經度：{_lng}\n緯度：{_lat}");
                        marker1.ZIndex = 99;
                        marker1.Offset = new Point(-20, -40);
                        marker1.Position = mapPos;
                    }

                    // Add Markers To Map
                    map.Markers.Add(marker1);
                    measMarker.Add(marker1);
                }
                else                            // 1 Point: Create Second Marker And Measurement Distance
                {
                    // Get Mouse Position
                    var position = e.GetPosition(this);
                    PointLatLng mapPos = map.FromLocalToLatLng((int)position.X, (int)position.Y);

                    string _lat = "";
                    string _lng = "";

                    LatLngConvert.ConvertToDecimal6(mapPos, out _lat, out _lng);

                    // Calculate Distance Between Marker 1 & Marker 2
                    double distance = PointExtension.GetDistance(measMarker[0].Position, mapPos);

                    // Marker Second Point
                    GMapMarker marker2 = new GMapMarker(mapPos);
                    {
                        marker2.Shape = new MeasureMarker(this, marker2, true, 
                            $"經度：{_lng}\n" +
                            $"緯度：{_lat}\n" +
                            $"距離：{distance:F2} M / {(distance/1000):F2} KM");
                        marker2.ZIndex = 99;
                        marker2.Offset = new Point(-20, -40);
                        marker2.Position = mapPos;
                    }

                    // Add Markers To Map
                    map.Markers.Add(marker2);
                    measMarker.Add(marker2);

                    // Generate Two Point Routes
                    List<PointLatLng> pointLatLngs = measMarker.ConvertAll(x => x.Position);

                    measRoute = new GMapRoute(pointLatLngs);
                    measRoute.Shape = new Path() 
                    { 
                        Stroke = new SolidColorBrush(Colors.LightGreen),
                        StrokeThickness = 3 
                    };
                    measRoute.ZIndex = -1;
                    map.Markers.Add(measRoute);         // In GMap.NET.WindowsPresentation, All The Draws Is Point
                }
            }
        }

        // Planning Area
        // ============================================================================================================
        
        /// <summary>
        /// Flight Line Plan
        /// </summary>
        void FlightLine_Planning()
        {
            // Clear All Markers On Map
            map.Markers.Clear();
            measMarker.Clear();
            measRoute?.Clear();

            if (!isLinePlanning)
                // Binding A Event That Set A Start Position To Measure The Distance 
                map.MouseDown += Map_LinePlanning_MouseDown;
            else
                // Debinding Measurement Event
                map.MouseDown -= Map_LinePlanning_MouseDown;

            isRanging = !isRanging;
        }

        private void Map_LinePlanning_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {

            }
    }
}
