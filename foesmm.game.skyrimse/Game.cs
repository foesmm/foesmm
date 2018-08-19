using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using foesmm.common.game;

namespace foesmm.game.skyrimse
{
    public class Game : AbstractGame
    {
        public override World World => World.ElderScrolls;
        public override ReleaseState ReleaseState => ReleaseState.Soon;
        public override string Title => "The Elder Scrolls: Skyrim Special Edition";
        public override string ShortTitle => "Skyrim SE";
        public override int ReleaseYear => 2016;
        public override string Executable => null;
        public override IToolKit ToolKit => null;
        public override IScriptExtender ScriptExtender => null;
        public override ISaveManager SaveManager => null;
        protected override string[] GOGKeys => null;
        protected override string[] SteamKeys => null;
        protected override Dictionary<string, string> RetailKeys => null;
    }
}
