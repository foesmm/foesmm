using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace foesmm.common.game
{
    public interface IGame
    {
        ReleaseState ReleaseState { get; }
        string Title { get; }
        int ReleaseYear { get; }
        BitmapImage Cover { get; }

        IScriptExtender ScriptExtender { get; }
        ISaveManager SaveManager { get; }

        string CrashTrace { get; } 
    }
}
