using System.Configuration;
using System.Data;
using System.Reflection;
using System.Windows;

namespace CatFlightGCSNext.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string author = "blackcat";
    }
}
