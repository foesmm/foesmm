using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace foesmm.common.game
{
    public interface IGame
    {
        World World { get; }
        ReleaseState ReleaseState { get; }
        string Title { get; }
        string ShortTitle { get; }
        int ReleaseYear { get; }
        BitmapImage Cover { get; }

        IToolKit ToolKit { get; }
        IScriptExtender ScriptExtender { get; }
        ISaveManager SaveManager { get; }

        string CrashTrace { get; } 
    }
}
