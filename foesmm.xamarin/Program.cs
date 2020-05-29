using Eto.Forms;
using foesmm.forms;
using Foundation;

namespace foesmm
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            new App(Eto.Platforms.XamMac2).Run(new GameSelectForm());
        }
    }
}
