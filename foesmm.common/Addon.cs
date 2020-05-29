using System;
using System.Xml.Serialization;

namespace foesmm
{
    [Serializable]
    [XmlRoot("Addon")]
    public class Addon : IModInfo
    {
        [XmlAttribute("Guid")] public Guid Guid { get; set; }
        [XmlAttribute("Title")] public string Title { get; set; }
        [XmlAttribute("Description")] public string Description { get; set; }

        public override string ToString()
        {
            return $"{Title} {{{Guid}}}";
        }
    }
}