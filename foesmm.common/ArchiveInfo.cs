using System;
using System.Xml.Serialization;

namespace foesmm
{
    [Serializable]
    [XmlRoot("Archive")]
    public class ArchiveInfo
    {
        [XmlAttribute("Name")] public string Name { get; set; }

        [XmlAttribute("Checksum", DataType = "hexBinary")]
        public byte[] Checksum { get; set; }
    }
}