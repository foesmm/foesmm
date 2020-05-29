using System.Data.HashFunction;
using System.IO;
using System.Threading.Tasks;

namespace foesmm
{
    public interface IArchive
    {
        FileInfo File { get; }
        IPlugin Plugin { get; }
        bool Active { get; }
        IHashValue Checksum { get; }
        Task<IHashValue> CalculateChecksum();
    }
}