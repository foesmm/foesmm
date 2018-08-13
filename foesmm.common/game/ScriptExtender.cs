using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace foesmm.common.game
{
    public abstract class ScriptExtender : IScriptExtender
    {
        protected string InstallPath { get; }
        public abstract string Title { get; }
        public abstract string Executable { get; }
        public string Version => InstallPath != null ? FileVersionInfo.GetVersionInfo(Path.Combine(InstallPath, Executable)).FileVersion : null;
        public abstract Uri Uri { get; }

        protected ScriptExtender(string installPath)
        {
            InstallPath = installPath;
        }

        public abstract void Update();
    }
}
