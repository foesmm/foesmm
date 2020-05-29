using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace foesmm
{
    [Serializable]
    [XmlRoot("Executable")]
    public class Executable : IExecutable
    {
        [XmlAttribute("File")] public string File { get; set; }
        [XmlAttribute("Type")] public ExecutableType Type { get; set; }

        [XmlElement("Checksum", DataType = "hexBinary")]
        public List<byte[]> Checksums { get; set; }
        
        [XmlElement("Download")]
        public List<Download> Downloads { get; set; }

        public override string ToString()
        {
            return File;
        }
    }
}