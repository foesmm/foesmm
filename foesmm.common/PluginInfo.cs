using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace foesmm
{
    [Serializable]
    [XmlRoot("Plugin")]
    public class PluginInfo
    {
        [XmlAttribute("Guid")] public Guid Guid { get; set; }
        [XmlAttribute("Name")] public string Name { get; set; }
        [XmlAttribute("Type")] public PluginType Type { get; set; }

        [XmlAttribute("Checksum", DataType = "hexBinary")]
        public byte[] Checksum { get; set; }
        
        [XmlElement("Archive")] public List<ArchiveInfo> Archives { get; set; }

        public override string ToString()
        {
            return $"{Type}: {Name}";
        }
    }
}