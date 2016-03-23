using System;
using System.Windows.Media.Imaging;
using FoESMM.Common;
using libespmsharp;

namespace FoESMM.Games.Fallout3
{
    public class Fallout3Game : Game
    {
        public override string Name => "Fallout 3";
        public override string ExecutableName => "Fallout3.exe";
        public override ScriptExtender ScriptExtender => new Fallout3ScriptExtender();
        public override ESPMVersion PluginVersion => ESPMVersion.Fo3;

        public Fallout3Game()
        {
            InstallPath = GameDetector.CheckSteamInstallation("22370", ref ReplacementDirectory);
        }
    }
}