using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace foesmm
{
    [Serializable]
    [XmlRoot("Settings")]
    public class Settings
    {
        [XmlArray("Games")]
        public List<Game> Games { get; set; }

        public Settings()
        {
            Games = new List<Game>();
        }
    }
}