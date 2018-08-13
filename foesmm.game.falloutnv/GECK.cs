using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using foesmm.common.game;

namespace foesmm.game.falloutnv
{
    internal class GECK : ToolKit
    {
        public GECK(string installPath) : base(installPath)
        {
        }

        public override string Title => "G.E.C.K.";
        public override string Executable => "Geck.exe";
    }
}
