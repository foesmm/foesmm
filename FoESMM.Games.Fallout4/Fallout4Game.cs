using System.Windows.Media.Imaging;
using FoESMM.Common;
using libespmsharp;

namespace FoESMM.Games.Fallout4
{
    public class Fallout4Game : Game
    {
        public override string Name => "Fallout 4";
        public override string ExecutableName => "Fallout4.exe";
        public override ScriptExtender ScriptExtender => new Fallout4ScriptExtender();
        public override ESPMVersion PluginVersion => ESPMVersion.Fo4;

        public Fallout4Game()
        {
            InstallPath = GameDetector.CheckSteamInstallation("377160", ref ReplacementDirectory) ??
                          GameDetector.CheckCustomUninstallKey("Fallout 4_is1");
        }
    }
}