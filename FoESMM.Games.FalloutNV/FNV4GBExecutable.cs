using FoESMM.Common;

namespace FoESMM.Games.FalloutNV
{
    public class FNV4GBExecutable : Executable
    {
        public override string Name => "FNV4GB";
        public override string ExecutableName => "fnv4gb.exe";
        public override string Arguments => "-SteamAppId %SteamAppId%";
    }
}