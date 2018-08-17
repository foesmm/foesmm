using System;
using Xunit;
using SharpCompress;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace foesmm.common.Tests
{
    public class PackageManager
    {
        [Fact]
        public void TestSolid7Zip()
        {
            var archive = ArchiveFactory.Open("extracted.7z");

            var reader = archive.ExtractAllEntries();
            reader.EntryExtractionProgress += (sender, args) =>
            {
                Console.WriteLine(args.ReaderProgress.PercentageReadExact);
            };
            while (reader.MoveToNextEntry())
            {
                if (reader.Entry.IsDirectory) continue;
                reader.WriteEntryToDirectory("test", new ExtractionOptions() {ExtractFullPath = true, Overwrite = true});
                Console.WriteLine("Break");
            }

            Console.WriteLine("Break");
        }
    }
}
