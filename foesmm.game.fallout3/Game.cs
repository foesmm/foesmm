using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using foesmm.common.game;

namespace foesmm.game.fallout3
{
    public class Game : AbstractGame
    {
        public override World World => World.Fallout;
        public override ReleaseState ReleaseState => ReleaseState.Soon;
        public override string Title => "Fallout 3";
        public override string ShortTitle => Title;
        public override int ReleaseYear => 2008;
        public override string Executable => "Fallout3.exe";
        public override IScriptExtender ScriptExtender => null;
        public override ISaveManager SaveManager => null;
        public override IToolKit ToolKit => null;

        protected override string[] GOGKeys => new[] {"1248282609", "1454315831"};
        protected override string[] SteamKeys => new[] {"Steam App 22300", "Steam App 22370"};
        protected override Dictionary<string, string> RetailKeys => new Dictionary<string, string>
        {
            {
                @"Software\Bethesda Softworks\Fallout3", @"installed path"
            }
        };
    }
}
