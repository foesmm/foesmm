using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using foesmm.common.game;

namespace foesmm.game.oblivion
{
    public class Game : AbstractGame
    {
        public override World World => World.ElderScrolls;
        public override ReleaseState ReleaseState => ReleaseState.Soon;
        public override string Title => "The Elder Scrolls: Oblivion";
        public override string ShortTitle => "Oblivion";
        public override int ReleaseYear => 2006;
        public override string Executable => null;
        public override IToolKit ToolKit => null;
        public override IScriptExtender ScriptExtender => null;
        public override ISaveManager SaveManager => null;
    }
}
