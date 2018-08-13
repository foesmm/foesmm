using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using foesmm.common.game;

namespace foesmm.game.falloutnv
{
    public class Game : AbstractGame
    {
        public override ReleaseState ReleaseState => ReleaseState.Alpha;
        public override string Title => "Fallout: New Vegas";
        public override int ReleaseYear => 2010;
        public override string Executable => "FalloutNV.exe";
        public override IScriptExtender ScriptExtender => InstallPath != null ? new NVSE(InstallPath) : null;
        public override IToolKit ToolKit => InstallPath != null ? new GECK(InstallPath) : null;
    }
}
