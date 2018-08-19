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
        string Title { get; }
        string Version { get; }

        IGame CurrentGame { get; }
        string CrashTrace { get; }

        void ManageGame(string assemblyCodeBase, string installPath);
        void ManageGame(IGame game);
        void RunGame(string assemblyCodeBase, string profile = null);
        void RunGame(IGame game, string profile = null);

        void Shutdown();
    }
}
