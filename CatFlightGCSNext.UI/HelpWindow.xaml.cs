using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CatFlightGCSNext.UI
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public string version { get => "軟體版本：" + App.version; }
        public string author { get => "作者：" + App.author; }
        public HelpWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
