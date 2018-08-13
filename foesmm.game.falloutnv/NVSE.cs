using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using foesmm.common.game;

namespace foesmm.game.falloutnv
{
    internal class NVSE : ScriptExtender
    {
        public NVSE(string installPath) : base(installPath)
        {
        }

        public override string Title => "NVSE";
        public override string Executable => "nvse_loader.exe";
        public override Uri Uri => new Uri("http://nvse.silverlock.org/");
        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
