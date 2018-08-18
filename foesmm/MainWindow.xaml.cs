using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using foesmm.common;
using foesmm.common.game;

namespace foesmm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected IFoESMM App { get; set; }
        protected IGame Game { get; set; }

        public MainWindow(IGame game)
        {
            InitializeComponent();

            App = Application.Current as IFoESMM;
            Game = game;

            Title = $"{App?.Title} v{App?.Version} - {Game.Title}";
        }
    }
}
