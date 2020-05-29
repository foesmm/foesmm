using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace foesmm
{
    [Serializable]
    [XmlRoot("Game")]
    public class GameInfo : IGameInfo
    {
        [XmlAttribute("GUID")] public Guid Guid { get; set; }
        [XmlAttribute("Title")] public string Title { get; set; }
        [XmlAttribute("Description")] public string Description { get; set; }
        [XmlAttribute("ShortTitle")] public string ShortTitle { get; set; }
        [XmlAttribute("ReleaseYear")] public int ReleaseYear { get; set; }
        [XmlAttribute("ReleaseState")] public ReleaseState ReleaseState { get; set; }
        [XmlElement("Executable")] public List<Executable> PossibleExecutables { get; set; }
        [XmlElement("Directories")] public GameDirectories DirectoryInfo { get; set; }

        [XmlArray("Addons")]
        [XmlArrayItem("Addon")]
        public List<Addon> OfficialAddons { get; set; }
        
        [XmlArray("Plugins")]
        [XmlArrayItem("Plugin")]
        public List<PluginInfo> OfficialPlugins { get; set; }

        public IReadOnlyDictionary<ExecutableType, IExecutable> Executables =>
            PossibleExecutables.ToDictionary(x => x.Type, x => (IExecutable) x);

        public IReadOnlyCollection<IModInfo> Addons => OfficialAddons;
        public IReadOnlyCollection<PluginInfo> Plugins => OfficialPlugins;

        public override string ToString()
        {
            return $"{Title} {{{Guid}}}";
        }
    }
}