using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace foesmm
{
    [Serializable]
    [XmlRoot("Directories")]
    public class GameDirectories
    {
        public string Game { get; set; }
        public string Data { get; set; }
        public string ScriptExtender { get; set; }
        public string ApplicationData { get; set; }
        public string Configuration { get; set; }
        public string Saves { get; set; }

        public GameDirectories()
        {
        }

        private static string ResolvePath(string input, string game, WineHelper.Environment environment)
        {
            var sb = new StringBuilder(input);
            sb.Replace("%Game%", game);
            sb.Replace("%AppData%", environment.ApplicationDataPath ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            sb.Replace("%Documents%", environment.MyDocumentsPath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            return sb.ToString();
        }

        public GameDirectories Resolve(DirectoryInfo gameDirectory, WineHelper.Environment environment)
        {
            var game = gameDirectory.FullName;
            return new GameDirectories
            {
                Game = gameDirectory.FullName,
                Data = ResolvePath(Data, game, environment),
                ScriptExtender = ResolvePath(ScriptExtender, game, environment),
                ApplicationData = ResolvePath(ApplicationData, game, environment),
                Configuration = ResolvePath(Configuration, game, environment),
                Saves = ResolvePath(Saves, game, environment),
            };
        }
    }
}