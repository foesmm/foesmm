using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using foesmm.common.game;

namespace foesmm.common
{
    // ReSharper disable once InconsistentNaming
    public interface IFoESMM
    {
        IGame CurrentGame { get; }
        string CrashTrace { get; }

        void ManageGame(IGame game);
        void RunGame(IGame game, string profile = null);

        void Shutdown();
    }
}
