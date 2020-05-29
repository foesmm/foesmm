using System.IO;

namespace foesmm
{
    internal class ScriptExtenderPlugin : IScriptExtenderPlugin
    {
        private ScriptExtenderPlugin(FileInfo file)
        {
            File = file;
        }

        public static IScriptExtenderPlugin Load(FileInfo file)
        {
            return new ScriptExtenderPlugin(file);
        }

        public FileInfo File { get; set; }
    }
}