using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using foesmm.common.game;

namespace foesmm.Resources
{
    public class ResourcesHelper
    {
        public static BitmapImage GetCoverOverlay(ReleaseState state)
        {
            if (state == ReleaseState.Release) return null;
            return new BitmapImage(new Uri($"pack://application:,,,/{typeof(ResourcesHelper).Assembly.GetName().Name};component/Resources/{state.ToString()}.png"));
        }
    }
}
