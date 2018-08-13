using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using foesmm.common.game;

namespace foesmm.common
{
    // ReSharper disable once InconsistentNaming
    public interface FoESMM
    {
        IGame CurrentGame { get; }

        string CrashTrace { get; }
    }
}
