using System;
using System.Windows;
using System.Windows.Input;

namespace FoESMM.Common.Commands
{
    public class SwitchGameCommand : ICommand
    {
        private readonly Window _window;

        public SwitchGameCommand(Window oParent)
        {
            _window = oParent;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var chooser = new Windows.GameChooserWindow();
            chooser.Show();
            _window.Close();
        }

        public event EventHandler CanExecuteChanged;
    }
}