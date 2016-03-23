using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FoESMM.Common.Controls;
using Microsoft.Win32;

namespace FoESMM.Common.Windows
{
    /// <summary>
    /// Interaction logic for GameChooserWindow.xaml
    /// </summary>
    public partial class GameChooserWindow : Window
    {
        private readonly List<GameCell> _availableGames = new List<GameCell>();
        private GameCell _selectedGameCell;
        
        public GameChooserWindow()
        {
            InitializeComponent();

            _availableGames.Add(new GameCell(GameFactory.InitializeGame("Fallout3")));
            _availableGames.Add(new GameCell(GameFactory.InitializeGame("FalloutNV")));
            _availableGames.Add(new GameCell(GameFactory.InitializeGame("Fallout4")));

            for (var i = 0; i < _availableGames.Count; i++)
            {
                var row = i/3;
                var col = i%3;

                Grid.SetRow(_availableGames[i], row);
                Grid.SetColumn(_availableGames[i], col);
                _availableGames[i].MouseLeftButtonUp += GameChosen;
                GamesGrid.Children.Add(_availableGames[i]);
            }

            Start.Click += StartFoESMM;
        }

        private void StartFoESMM(object sender, RoutedEventArgs e)
        {
            var FoESMM = new MainWindow(_selectedGameCell.Game);
            FoESMM.Show();
            Close();
        }

        private void GameChosen(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var cell = (GameCell) sender;

            if (cell.Equals(_selectedGameCell)) return;
            var game = cell.Game;
            if (!game.IsFound())
            {
                var browseDialog = new OpenFileDialog
                {
                    Filter = $"{game.Name} Executable|{game.ExecutableName}",
                };

                if (browseDialog.ShowDialog() != true) return;
                game.SetCustomInstallPath(Path.GetDirectoryName(browseDialog.FileName));
                cell.Game = game;
            }

            _selectedGameCell?.SetSelected(false);
            _selectedGameCell = cell;
            _selectedGameCell.SetSelected(true);
            Start.IsEnabled = _selectedGameCell != null;
        }
    }
}
