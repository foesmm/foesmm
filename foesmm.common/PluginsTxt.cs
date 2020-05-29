using System.Collections.Generic;
using System.IO;

namespace foesmm
{
    public class PluginsTxt
    {
        private readonly string _file;
        public IList<string> Plugins { get; }
        public PluginsTxt(string filename)
        {
            _file = filename;
            Plugins = new List<string>();
            var lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                if (line.StartsWith("#")) continue;
                Plugins.Add(line.ToLower());
            }
        }
    }
}