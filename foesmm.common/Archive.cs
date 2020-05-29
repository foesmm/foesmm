using System.Data.HashFunction;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Dispatch;

namespace foesmm
{
    public class Archive : IArchive
    {
        public static IArchive Load(FileInfo file, IPlugin plugin)
        {
            return new Archive(file, plugin);
        }

        private Archive(FileInfo file, IPlugin plugin)
        {
            File = file;
            Plugin = plugin;
        }

        public async Task<IHashValue> CalculateChecksum()
        {
            App.Log.Info($"Calculating checksum: {this}");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            using (var stream = File.OpenRead(16 * 1024 * 1024))
            {
                Checksum = await App.Shared.ChecksumCalculator.ComputeHashAsync(stream);
            }

            stopWatch.Stop();
            App.Log.Info($"{this} checksum: {Checksum.AsHexString()} in {stopWatch.Elapsed}");
            return Checksum;
        }

        public FileInfo File { get; }
        public IPlugin Plugin { get; }

        public bool Active => Plugin?.Active ?? true;
        public IHashValue Checksum { get; private set; }

        public override string ToString()
        {
            return File.Name;
        }
    }
}