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
    /// Interaction logic for FlightMessageView.xaml
    /// </summary>
    public partial class FlightLogView : UserControl
    {
        public FlightLogView()
        {
            InitializeComponent();
        }

        private void OpenFlightLog_Click(object sender, RoutedEventArgs e)
        {
            if (tbFlightLog.Visibility == Visibility.Collapsed)
                ChangeFlightLogView(Visibility.Visible);
            else
                ChangeFlightLogView(Visibility.Collapsed);
        }

        public void ChangeFlightLogView(Visibility visibility)
        {
            tbFlightLog.Visibility = visibility;
        }
    }
}
