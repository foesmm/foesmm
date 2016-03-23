using System.IO;

namespace FoESMM.Common
{
    public class Plugin
    {
        public Plugin(string sFile)
        {
            Name = Path.GetFileName(sFile);
            File = sFile;
        }

        public string Name { get; private set; }
        public string File { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}