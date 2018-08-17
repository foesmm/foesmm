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
    }
}
