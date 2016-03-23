using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using FoESMM.Common;
using libespmsharp;

namespace FoESMM.Games.FalloutNV
{
    public class FalloutNVGame : Game
    {
        public override string Name => "Fallout: New Vegas";
        public override string ExecutableName => "FalloutNV.exe";
        public override ScriptExtender ScriptExtender => new FalloutNVScriptExtender();
        public override List<Executable> CommonExecutables => new List<Executable>
        {
            new FNV4GBExecutable()
        };

        public override ESPMVersion PluginVersion => ESPMVersion.FNV;

        public FalloutNVGame()
        {
            InstallPath = GameDetector.CheckSteamInstallation("22380", ref ReplacementDirectory) ?? GameDetector.CheckSteamInstallation("22490", ref ReplacementDirectory);
        }
    }
}