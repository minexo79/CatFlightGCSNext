using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CatFlightGCSNext.UI.Utils
{
    public static class ColorConvert
    {
        public static SolidColorBrush? ToBrush(this string HexColorString)
        {
            return new BrushConverter().ConvertFrom(HexColorString) as SolidColorBrush;
        }
    }
}
