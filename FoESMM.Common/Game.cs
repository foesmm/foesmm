using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using libespmsharp;

namespace FoESMM.Common
{
    public abstract class Game : Executable
    {
        protected BitmapImage IconInternal;
        public virtual BitmapImage Icon => IconInternal;
        public virtual ScriptExtender ScriptExtender => null;
        public virtual List<Executable> CommonExecutables => null;
        public override Version Version => Version.Parse(FileVersionInfo.GetVersionInfo(GetMainExecutablePath()).FileVersion.Replace(',','.'));
        protected string NormalizedInstallPath;
        public string InstallPath
        {
            get { return NormalizedInstallPath; }
            protected set
            {
                NormalizedInstallPath = string.IsNullOrEmpty(value) ? null : Path.GetFullPath(value)
                    .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
        }

        protected Dictionary<string, string> ReplacementDirectory = new Dictionary<string, string>();
        public virtual string DataDirectory => InstallPath + "\\Data";
        public virtual string MasterFileExtension => "*.esm";
        public virtual string PluginFileExtension => "*.esp";
        public virtual PluginFactory PluginFactory => new PluginFactory();
        public virtual ESPMVersion PluginVersion => ESPMVersion.Unknown;

        protected Game()
        {
            SetImage(GetType().Namespace);
        }

        public void InitializeGame()
        {
        }

        public string GetMainExecutablePath()
        {
            if (InstallPath == null || ExecutableName == null) return null;
            return InstallPath + "\\" + ExecutableName;
        }

        public bool IsFound()
        {
            return GetMainExecutablePath() != null && File.Exists(GetMainExecutablePath());
        }

        public void SetCustomInstallPath(string sCustomPath)
        {
            InstallPath = sCustomPath;
            InitializeGame();
        }

        private static Uri GenerateUri(string sAssemby)
        {
            return new Uri($"pack://application:,,,/{sAssemby};component/Resources/icon.png");
        }

        protected void SetImage(string sAssembly)
        {
            try
            {
                IconInternal = new BitmapImage(GenerateUri(sAssembly));
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public string ReplaceArguments(string sArguments)
        {
            return ReplacementDirectory.Aggregate(sArguments, (current, entry) => current.Replace(entry.Key, entry.Value));
        }
    }
}