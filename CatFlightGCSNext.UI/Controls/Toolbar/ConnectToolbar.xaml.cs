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

namespace CatFlightGCSNext.UI.Controls.Toolbar
{
    /// <summary>
    /// Interaction logic for ConnectToolbar.xaml
    /// </summary>
    public partial class ConnectToolbar : UserControl
    {
        public ConnectToolbar()
        {
            InitializeComponent();
        }

        private void cbComport_DropDownOpened(object sender, EventArgs e)
        {
            cbComport.Items.Clear();

            foreach (string port in FlightData.connection.GetPortList())
            {
                cbComport.Items.Add(port);
            }

            cbComport.SelectedIndex = 0;
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (FlightData.connection != null)
            {
                if (FlightData.connection.IsOpened())
                {
                    // TODO: Complete Disconnect Event
                    btnConnect.Dispatcher.Invoke(delegate { btnConnect.Content = "連線"; });
                }
            }
            else
            {
                // TODO: Complete Connect Event
                btnConnect.Dispatcher.Invoke(delegate { btnConnect.Content = "斷線"; });
            }
        }
    }
}
