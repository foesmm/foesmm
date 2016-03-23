using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FoESMM.Common.Commands
{
    public class RunDefaultCommand : ICommand
    {
        private Window _window;
        private Game _game;

        public RunDefaultCommand(Window oParent, Game oGame)
        {
            _window = oParent;
            _game = oGame;
        }

        public bool CanExecute(object parameter)
        {
            Console.WriteLine("RunDefaultCommand CanExecute " + _game);
            return true;
        }

        public void Execute(object parameter)
        {
            var processStartInfo = new ProcessStartInfo();

            if (_game.CommonExecutables != null && _game.CommonExecutables.Count > 0)
            {
                foreach (var executable in _game.CommonExecutables.Where(executable => executable.IsInstalled))
                {
                    processStartInfo.FileName = executable.GetExecutablePath();
                    processStartInfo.Arguments = _game.ReplaceArguments(executable.Arguments);
                    break;
                }
            }
            else if (_game.ScriptExtender.IsInstalled)
            {
                processStartInfo.FileName = _game.ScriptExtender.GetExecutablePath();
                processStartInfo.Arguments = _game.ReplaceArguments(_game.ScriptExtender.Arguments);
            }
            else
            {
                processStartInfo.FileName = _game.GetMainExecutablePath();
                processStartInfo.Arguments = _game.ReplaceArguments(_game.Arguments);
            }

            if (processStartInfo.FileName == null) return;

            Console.WriteLine(processStartInfo.FileName + " " + processStartInfo.Arguments);
            Process.Start(processStartInfo);
            _window.Close();
        }

        public event EventHandler CanExecuteChanged;
    }
}