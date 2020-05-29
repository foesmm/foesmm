using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Eto;
using Eto.Forms;

namespace foesmm
{
    [Serializable]
    [XmlRoot("Game")]
    public class Game : IGame
    {
        public static void TryLoad(Uri uri, ICollection<IGameInfo> supportedGames, Action<IGame> onGameFound)
        {
            if (supportedGames == null) throw new ArgumentNullException(nameof(supportedGames));
            if (!uri.Scheme.Equals("file")) return;
            var targets = supportedGames.ToDictionary(x => x.Executables[ExecutableType.Main].File, x => x);

            if ((File.GetAttributes(uri.LocalPath) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                var directoryInfo = new DirectoryInfo(uri.LocalPath);

                if (Application.Instance.Platform.IsMac && directoryInfo.Name.EndsWith(".app"))
                {
                    WineHelper.DetectFromMacWrapper(directoryInfo.FullName);
                }
                else
                {
                    var found = directoryInfo.GetFiles("*.exe", SearchOption.AllDirectories)
                        .Where(file => targets.ContainsKey(file.Name)).ToDictionary(x => x, x => targets[x.Name]);

                    foreach (var gameInfo in found)
                    {
                        var environment = new WineHelper.Environment();
                        if (!Application.Instance.Platform.IsWpf)
                        {
                            environment = WineHelper.ResolveEnvironment(gameInfo.Key.Directory);
                        }

                        onGameFound(new Game(gameInfo.Value, gameInfo.Key.Directory, environment));
                    }
                }
            }
            else
            {
                var fileInfo = new FileInfo(uri.LocalPath);
                var gameInfo = targets[fileInfo.Name];
                if (gameInfo != null)
                {
                    var environment = new WineHelper.Environment();
                    if (!Application.Instance.Platform.IsWpf)
                    {
                        environment = WineHelper.ResolveEnvironment(fileInfo.Directory);
                    }

                    onGameFound(new Game(gameInfo, fileInfo.Directory, environment));
                }
            }
        }

        public static IGameInfo LoadInfo(FileInfo gameManifest)
        {
            IGameInfo gameInfo = null;
            using (var xmlReader = new StreamReader(gameManifest.FullName))
            {
                var serializer = new XmlSerializer(typeof(GameInfo));
                try
                {
                    serializer.UnknownAttribute += (sender, args) =>
                    {
                        System.Xml.XmlAttribute attr = args.Attr;
                        Console.WriteLine($"Unknown attribute {attr.Name}=\'{attr.Value}\'");
                    };
                    serializer.UnknownNode += (sender, args) =>
                    {
                        Console.WriteLine($"Unknown Node:{args.Name}\t{args.Text}");
                    };
                    serializer.UnknownElement +=
                        (sender, args) =>
                            Console.WriteLine("Unknown Element:"
                                              + args.Element.Name + "\t" + args.Element.InnerXml);
                    serializer.UnreferencedObject +=
                        (sender, args) =>
                            Console.WriteLine("Unreferenced Object:"
                                              + args.UnreferencedId + "\t" + args.UnreferencedObject.ToString());
                    gameInfo = (GameInfo) serializer.Deserialize(xmlReader);
                    Debug.WriteLine("Break");
                }
                catch (InvalidOperationException ioe)
                {
                    Debug.WriteLine("Error");
                }
            }

            return gameInfo;
        }

        public Game()
        {
        }

        private Game(IGameInfo gameInfo, DirectoryInfo gameDirectory, WineHelper.Environment environment)
        {
            Guid = Guid.NewGuid();
            Info = gameInfo;
            WineEngine = environment.WineEngine;
            Directories = gameInfo.DirectoryInfo.Resolve(gameDirectory, environment);

            Initialize();
        }

        [XmlAttribute("Guid")] public Guid Guid { get; set; }

        private string _title;

        [XmlAttribute("Title")]
        public string Title
        {
            get => _title ?? Info.ShortTitle;
            set => _title = value;
        }

        [XmlAttribute("Game")]
        public Guid GameGuid
        {
            get => Info.Guid;
            set { Info = GameLibrary.Instance.SupportedGames.First(x => x.Guid.Equals(value)); }
        }

        [XmlIgnore] public IGameInfo Info { get; set; }
        [XmlAttribute("WineEngine")] public Engine WineEngine { get; set; }
        [XmlElement("Directories")] public GameDirectories Directories { get; set; }

        private bool _initialized = false;

        private LoadOrder _loadOrder;

        [XmlIgnore]
        public LoadOrder LoadOrder
        {
            get
            {
                Initialize();
                return _loadOrder;
            }
        }

        public override string ToString()
        {
            return $"{Info} [{Directories.Game}]";
        }

        private void Initialize()
        {
            if (_initialized) return;

            _loadOrder = new LoadOrder(this);

            _initialized = true;
            Debug.WriteLine("Break");
        }
    }
}