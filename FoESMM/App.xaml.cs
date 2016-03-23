using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using FoESMM.Common;

namespace FoESMM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Window startWindow;
            if (false)
            {
                var gameStr = "FalloutNV";

                var game = GameFactory.InitializeGame(gameStr);

                startWindow = new Common.Windows.MainWindow(game);
            }
            else
            {
                startWindow = new Common.Windows.GameChooserWindow();
            }
            startWindow.Show();
        }
    }
}
