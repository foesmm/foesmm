using System;
using Eto.Forms;
using Eto.Drawing;
using foesmm.forms;

namespace foesmm
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            new App(Eto.Platform.Detect).Run(new GameSelectForm());
        }
    }
}