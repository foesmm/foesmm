using System;
using System.Collections.Generic;
using System.Drawing;
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
using foesmm.common;
using foesmm.common.game;
using foesmm.Fonts;
using foesmm.Resources;
using foesmm.Shaders;

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
            control.Overlay.Source = ResourcesHelper.GetCoverOverlay(game.ReleaseState);
            if (game.ReleaseState != ReleaseState.Soon)
            {
                var effect = new GrayscaleEffect();
                control.Cover.Effect = effect;

                if (!game.DetectInstallation())
                {
                    effect.DesaturationFactor = 0;
                    control.Manage.SetValue(Grid.ColumnSpanProperty, 2);
                    control.Manage.Margin = new Thickness(5, 0, 5, 5);
                    control.Manage.Content = "Configure";
                    control.Run.Visibility = Visibility.Hidden;
                }
                else
                {
                    effect.DesaturationFactor = 1;
                }
            }
            else
            {
                control.Manage.Visibility = Visibility.Hidden;
                control.Run.Visibility = Visibility.Hidden;
            }

            switch (game.World)
            {
                case World.ElderScrolls:
                    control.Title.FontFamily = FontHelper.PlanewalkerFontFamily;
                    break;
                case World.Fallout:
                    control.Title.FontFamily = FontHelper.OverseerFontFamily;
                    break;
            }

            control.Title.Content = game.ShortTitle;

            Console.WriteLine(game.ToString());
        }

        public GameControl()
        {
            InitializeComponent();
        }

        private void Manage_Click(object sender, RoutedEventArgs e)
        {
            IFoESMM app;
            GameChooserWindow parentWindow;
            if (Game.ReleaseState == ReleaseState.Soon ||
                (parentWindow = Window.GetWindow(this) as GameChooserWindow) == null ||
                (app = Application.Current as IFoESMM) == null) return;

            app.ManageGame(Game.GetType().Assembly.CodeBase, Game.InstallPath);
            parentWindow.Close();
        }

        private void Run_OnClick(object sender, RoutedEventArgs e)
        {
            IFoESMM app;
            GameChooserWindow parentWindow;
            if (Game.ReleaseState == ReleaseState.Soon ||
                (parentWindow = Window.GetWindow(this) as GameChooserWindow) == null ||
                (app = Application.Current as IFoESMM) == null) return;

            app.RunGame(Game.GetType().Assembly.CodeBase);
            parentWindow.Close();
        }
    }
}
