using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using foesmm.common.game;

namespace foesmm.game.fallout4
{
    public class Game : AbstractGame
    {
        public override World World => World.Fallout;
        public override ReleaseState ReleaseState => ReleaseState.Soon;
        public override string Title => "Fallout 4";
        public override string ShortTitle => Title;
        public override int ReleaseYear => 2015;
        public override string Executable => null;
        public override IToolKit ToolKit => null;
        public override IScriptExtender ScriptExtender => null;
        public override ISaveManager SaveManager => null;
    }
}
