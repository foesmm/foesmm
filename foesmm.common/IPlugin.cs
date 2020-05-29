using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.HashFunction;
using System.IO;
using System.Threading.Tasks;

namespace foesmm
{
    public interface IPlugin : INotifyPropertyChanged
    {
        PluginInfo Info { get; }
        FileInfo File { get; }
        bool Active { get; set; }
        string Index { get; }
        IReadOnlyList<IArchive> Archives { get; }
        IHashValue Checksum { get; }
        Task<IHashValue> CalculateChecksum();
    }
}