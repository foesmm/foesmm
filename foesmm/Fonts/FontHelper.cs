using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace foesmm.Fonts
{
    public static class FontHelper
    {
        public static FontFamily OverseerFontFamily => new FontFamily(new Uri("pack://application:,,,/"), "/Fonts/#Overseer");
        public static FontFamily PlanewalkerFontFamily => new FontFamily(new Uri("pack://application:,,,/"), "/Fonts/#Planewalker");
    }
}
