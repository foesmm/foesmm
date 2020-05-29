using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;
using foesmm.controls;

namespace foesmm.forms
{
    public class GameSelectForm : Form
    {
        public GameSelectForm()
        {
            Title = T._("Fallout & Elder Scrolls Mod Manager");
            ClientSize = new Size(640, 480);
            var gameLibrary = new GameLibraryControl();
            gameLibrary.GameSelected += OnGameSelected;

            var label = T._("Or drop game folder, link or executable on this window");

            Content = gameLibrary;
        }

        private void OnGameSelected(object sender, GameSelectedEventArgs e)
        {
            App.Shared.MainForm = new ManageGameForm(e.Game);
            App.Shared.MainForm.Show();
            Close();
        }
    }
}