using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.HashFunction;
using System.Data.HashFunction.xxHash;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Dispatch;
using Eto;
using Eto.Forms;
using foesmm.forms;
using LibGit2Sharp;
using NLog;
using NLog.Config;
using NLog.Targets;
using LogLevel = NLog.LogLevel;

namespace foesmm
{
    public class App : Application
    {
        public static App Shared => (App) Instance;
        public bool TraceLog { get; set; }
        public static Logger Log => LogManager.GetLogger("foesmm");
        public static string DataPath => Path.Combine(Helper.LocalApplicationDataDirectory.FullName, "Data");

        private static string SettingsPath =>
            Path.Combine(Helper.LocalApplicationDataDirectory.FullName, "Settings.xml");

        private static Identity _signature;
        public static Commit DataVersion { get; private set; }
        public Settings Settings { get; private set; }
        public SerialQueue SerialQueue { get; private set; }
        public IxxHash ChecksumCalculator { get; private set; }

        public App(string platformType) : base(platformType)
        {
            Init();
        }

        public App(Platform platformType) : base(platformType)
        {
            Init();
        }

        private void Init()
        {
            ChecksumCalculator = xxHashFactory.Instance.Create(new xxHashConfig {HashSizeInBits = 64});
            SerialQueue = new SerialQueue();
            Settings = new Settings();
            _signature = new Identity("foesmm", "bot@foesmm.org");

            InitializeLogger();
            InitializeGameData();
        }

        private void InitializeLogger()
        {
            var logDirectory = Path.Combine(Helper.LocalApplicationDataDirectory.FullName, "Logs");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            var config = new LoggingConfiguration();

            var logLevel = TraceLog ? LogLevel.Trace : LogLevel.Info;
            const string logLayout = "${date} [${level:uppercase=true}] ${message}";
            var logConsole = new ConsoleTarget
            {
                Layout = logLayout
            };
            var logFile = new FileTarget
            {
                FileName = Path.Combine(logDirectory, "foesmm-${shortdate}.log"),
                Layout = logLayout,
                MaxArchiveFiles = 10,
                MaxArchiveDays = 7
            };
            config.AddTarget("console", logConsole);
            config.AddTarget("file", logFile);

            config.AddRule(logLevel, LogLevel.Fatal, logConsole);
            config.AddRule(logLevel, LogLevel.Fatal, logFile);

            LogManager.Configuration = config;
            Log.Info("Initialized");
        }

        private void InitializeGameData()
        {
            var appDirectory = Helper.LocalApplicationDataDirectory;
            if (!appDirectory.Exists) appDirectory.Create();

            Repository repo;
            try
            {
                repo = new Repository(DataPath);
                Log.Debug("Updating Game Data");
                Commands.Pull(repo, new Signature(_signature, DateTimeOffset.Now), null);
            }
            catch (RepositoryNotFoundException e)
            {
                Log.Debug("Getting Game Data");
                repo = new Repository(Repository.Clone("https://github.com/foesmm/data", DataPath));
            }

            DataVersion = repo.Head.Tip;
            Log.Info($"Game Data version: {DataVersion.Sha}");

            LoadSettings();
        }

        private void LoadSettings()
        {
            if (!File.Exists(SettingsPath))
            {
                SaveSettings();
            }
            else
            {
                using (var xmlReader = new StreamReader(SettingsPath))
                {
                    var serializer = new XmlSerializer(typeof(Settings));
                    Settings = (Settings) serializer.Deserialize(xmlReader);
                }
            }

            foreach (var game in Settings.Games)
            {
                GameLibrary.Instance.Games.Add(game);
            }

            GameLibrary.Instance.Games.CollectionChanged += GamesOnCollectionChanged;
        }

        private void GamesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var game in e.NewItems)
                {
                    Settings.Games.Add((Game) game);
                }
            }

            SaveSettings();
            Debug.WriteLine("Break");
        }

        public void SaveSettings()
        {
            var writerSettings = new XmlWriterSettings
            {
                Indent = true,
                NewLineHandling = NewLineHandling.Entitize,
                CloseOutput = true
            };

            using (var writer = XmlWriter.Create(new StreamWriter(SettingsPath), writerSettings))
            {
                var serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(writer, Settings);
            }
        }
    }
}