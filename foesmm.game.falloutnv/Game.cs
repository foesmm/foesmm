using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using foesmm.common.game;

namespace foesmm.game.falloutnv
{
    public class Game : AbstractGame
    {
        public override World World => World.Fallout;
        public override ReleaseState ReleaseState => ReleaseState.Alpha;
        public override string Title => "Fallout: New Vegas";
        public override string ShortTitle => "New Vegas";
        public override int ReleaseYear => 2010;
        public override string Executable => "FalloutNV.exe";
        public override IScriptExtender ScriptExtender => InstallPath != null ? new NVSE(InstallPath) : null;
        public override ISaveManager SaveManager => null;
        public override IToolKit ToolKit => InstallPath != null ? new GECK(InstallPath) : null;

        protected override string[] GOGKeys => new[] {"1312824873", "1454587428"};
        protected override string[] SteamKeys => new[] {"Steam App 22380", "Steam App 22490"};
        protected override Dictionary<string, string> RetailKeys => new Dictionary<string, string>
        {
            {@"Software\Bethesda Softworks\falloutnv", @"installed path"}
        };
    }
}
