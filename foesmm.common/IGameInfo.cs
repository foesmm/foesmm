using System;
using System.Collections.Generic;

namespace foesmm
{
    public interface IGameInfo : IModInfo
    {
        string ShortTitle { get; }
        int ReleaseYear { get; }
        ReleaseState ReleaseState { get; }
        IReadOnlyDictionary<ExecutableType, IExecutable> Executables { get; }
        IReadOnlyCollection<IModInfo> Addons { get; }
        IReadOnlyCollection<PluginInfo> Plugins { get; }
        GameDirectories DirectoryInfo { get; }
    }
}