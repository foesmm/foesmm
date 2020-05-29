using System;
using System.Xml.Serialization;

namespace foesmm
{
    [Serializable]
    [XmlRoot("Download")]
    public class Download
    {
        [XmlAttribute("Uri")]
        public string Uri { get; set; }
        [XmlAttribute("Checksum", DataType = "hexBinary")]
        public byte[] Checksum { get; set; } 
        [XmlAttribute("Provider")]
        public string Provider { get; set; }
    }
}