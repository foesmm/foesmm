using System;
using System.Diagnostics;
using System.IO;

namespace FoESMM.Common
{
    public abstract class Executable
    {
        public virtual string Name => null;
        public virtual string ExecutableName => null;
        public virtual string Arguments => null;

        public bool IsInstalled => File.Exists(GetExecutablePath());
        public virtual Version Version => Version.Parse(FileVersionInfo.GetVersionInfo(GetExecutablePath()).FileVersion.Replace(',', '.'));

        public string GetExecutablePath()
        {
            return Directory.GetCurrentDirectory() + "\\" + ExecutableName;
        }
    }
}