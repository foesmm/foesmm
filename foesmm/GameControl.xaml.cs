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
using System.Windows.Navigation;
using System.Windows.Shapes;
using foesmm.common.game;

namespace foesmm
{
    /// <summary>
    /// Interaction logic for GameControl.xaml
    /// </summary>
    public partial class GameControl : UserControl
    {
        public static DependencyProperty GameProperty = DependencyProperty.Register("Game", typeof(IGame), typeof(GameControl), new PropertyMetadata(null, new PropertyChangedCallback(OnGameChanged)));

        public IGame Game
        {
            get => (IGame) GetValue(GameProperty);
            set => SetValue(GameProperty, value);
        }

        private static void OnGameChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var control = (GameControl) dependencyObject;
            var game = (IGame) e.NewValue;

            control.Cover.Source = game.Cover;

            switch (game.World)
            {
                case World.ElderScrolls:
                    control.Title.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "/Fonts/#Planewalker");
                    break;
                case World.Fallout:
                    control.Title.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "/Fonts/#Overseer");
                    break;
            }
            control.Title.Content = game.ShortTitle;

            Console.WriteLine(game.ToString());
        }

        public GameControl()
        {
            InitializeComponent();
        }
    }
}
