using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace foesmm
{
    public interface IGame
    {
        Guid Guid { get; }
        string Title { get; }
        IGameInfo Info { get; }
        Engine WineEngine { get; }
        GameDirectories Directories { get; }
        LoadOrder LoadOrder { get; }
    }
}