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
using FoESMM.Common.Shaders;

namespace FoESMM.Common.Controls
{
    /// <summary>
    /// Interaction logic for GameCell.xaml
    /// </summary>
    public partial class GameCell : UserControl
    {
        private Game _game;

        public Game Game
        {
            get { return _game; }
            set
            {
                _game = value;
                ToolTip = Game.InstallPath;

                if (Game.IsFound())
                {
                    Status.Content = $"Found, {Game.Version}";
                    Status.Foreground = Brushes.Green;
                }
                else
                {
                    Status.Content = "Not found";
                    Status.Foreground = Brushes.Red;
                }
            }
        }

        public void SetSelected(bool bSelected)
        {
            ((GrayscaleEffect) Icon.Effect).DesaturationFactor = bSelected ? 1 : 0;
        }

        public GameCell()
        {
            InitializeComponent();
            RenderOptions.SetBitmapScalingMode(Icon, BitmapScalingMode.HighQuality);
            Icon.Effect = new GrayscaleEffect();
        }

        public GameCell(Game game) : this()
        {
            Game = game;

            Icon.Source = Game.Icon;
            Title.Content = Game.Name;
        }
    }
}
