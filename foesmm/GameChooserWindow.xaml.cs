using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using foesmm.Resources;

namespace foesmm
{
    /// <summary>
    /// Interaction logic for GameChooserWindow.xaml
    /// </summary>
    public partial class GameChooserWindow : Window
    {
        public GameChooserWindow(IEnumerable<IGame> availableGames)
        {
            InitializeComponent();

            foreach (var game in availableGames)
            {
                Games.Items.Add(game);
            }

            MaxWidth = Math.Ceiling(Games.Items.Count / 2f + 1) * 140;
        }
    }
}
