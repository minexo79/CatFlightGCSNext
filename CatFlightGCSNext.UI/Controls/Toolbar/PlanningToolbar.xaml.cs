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

namespace CatFlightGCSNext.UI.Controls.Toolbar
{
    /// <summary>
    /// Interaction logic for PlanningToolbar.xaml
    /// </summary>
    public partial class PlanningToolbar : UserControl
    {
        public event EventHandler<EventArgs> OnRangeButtonClick;

        public PlanningToolbar()
        {
            InitializeComponent();
        }

        private void btnRanging_Click(object sender, RoutedEventArgs e)
        {
            OnRangeButtonClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
