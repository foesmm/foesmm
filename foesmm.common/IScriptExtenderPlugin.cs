using System.IO;

namespace foesmm
{
    public interface IScriptExtenderPlugin
    {
        FileInfo File { get; }
    }
}