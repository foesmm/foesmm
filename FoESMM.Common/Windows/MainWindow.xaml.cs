using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FoESMM.Common.Commands;

namespace FoESMM.Common.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Game Game { get; }
        public ICommand RunDefaultCommand { get; private set; }
        public ICommand SwitchGameCommand { get; private set; }

        public MainWindow(Game game)
        {
            Game = game;
            RunDefaultCommand = new RunDefaultCommand(this, Game);
            SwitchGameCommand = new SwitchGameCommand(this);

            InitializeComponent();

            Directory.SetCurrentDirectory(Game.InstallPath);
            GameIcon.Source = Game.Icon;

            UpdateLaunchButton();
            UpdateStatusBar();

            var plugins = new List<Plugin>();

            var fileList = Directory.GetFiles(Game.DataDirectory, Game.MasterFileExtension).ToList();
            foreach (var file in fileList)
            {
                plugins.Add(Game.PluginFactory.ReadFile(file, true));
            }
            fileList = Directory.GetFiles(Game.DataDirectory, Game.PluginFileExtension).ToList();
            foreach (var file in fileList)
            {
                plugins.Add(Game.PluginFactory.ReadFile(file));
            }

            LoadOrderGrid.ItemsSource = plugins;
        }

        protected void UpdateLaunchButton()
        {
            if (false) // Custom Launch option
            {
                
            }
            else if (Game.CommonExecutables != null && Game.CommonExecutables.Count > 0)
            {
                foreach (var executable in Game.CommonExecutables.Where(executable => executable.IsInstalled))
                {
                    LaunchButton.Content = $"Launch {executable.Name}";
                    break;
                }
            }
            else if (Game.ScriptExtender != null && Game.ScriptExtender.IsInstalled)
            {
                LaunchButton.Content = $"Launch {Game.ScriptExtender.Name}";
            }
            else
            {
                LaunchButton.Content = $"Launch {Game.Name}";
            }
        }

        protected void UpdateStatusBar()
        {
            GameBox.Text = $"{Game.Name} v{Game.Version}";
            ScriptExtenderBox.Text = Game.ScriptExtender.IsInstalled ? $"{Game.ScriptExtender.Name} v{Game.ScriptExtender.Version}" : $"{Game.ScriptExtender.Name} not installed";
        }
    }
}
