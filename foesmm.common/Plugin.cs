using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.HashFunction;
using System.Data.HashFunction.xxHash;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Dispatch;

namespace foesmm
{
    public class Plugin : IPlugin, INotifyPropertyChanged
    {
        public static Plugin Load(FileInfo file, PluginInfo info)
        {
            return new Plugin(file, info);
        }

        private Plugin(FileInfo file, PluginInfo info)
        {
            File = file;
            Info = info;

            var archivePattern = File.Name.Substring(0, File.Name.Length - File.Extension.Length) + "*.bsa";
            _archives = new List<IArchive>(
                File.Directory?.GetFiles(archivePattern).OrderBy(x => x.Name).Select(x => Archive.Load(x, this)) ??
                new List<IArchive>());
            if (Info != null)
            {
                _archives.AddRange(Info.Archives.Select(x =>
                    Archive.Load(
                        new FileInfo(Path.Combine(File.DirectoryName ?? throw new ArgumentException(), x.Name)),
                        this)));
            }
        }

        public async Task<IHashValue> CalculateChecksum()
        {
            App.Log.Info($"Calculating checksum: {this}");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            using (var stream = File.OpenRead(512 * 1024))
            {
                Checksum = await App.Shared.ChecksumCalculator.ComputeHashAsync(stream);
            }

            stopWatch.Stop();
            App.Log.Info($"{this} checksum: {Checksum.AsHexString()} in {stopWatch.Elapsed}");
            return Checksum;
        }

        public PluginInfo Info { get; }
        public FileInfo File { get; }
        private bool _active;

        public bool Active
        {
            get => _active;
            set
            {
                if (_active == value) return;
                _active = value;
                NotifyPropertyChanged();
            }
        }

        private readonly List<IArchive> _archives;

        private string _index = null;

        public string Index
        {
            get => _index;
            private set
            {
                if (_index == value) return;
                _index = value;
                NotifyPropertyChanged();
            }
        }

        public void SetIndex(int index)
        {
            Index = index == -1 ? null : index.ToString("X2");
        }

        public IReadOnlyList<IArchive> Archives => _archives;
        public IHashValue Checksum { get; private set; }

        public override string ToString()
        {
            return $"{File.Name}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}