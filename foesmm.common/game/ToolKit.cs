using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace foesmm.common.game
{
    public abstract class ToolKit : IToolKit
    {
        private string InstallPath { get; }
        public abstract string Title { get; }
        public abstract string Executable { get; }
        public string Version => InstallPath != null ? FileVersionInfo.GetVersionInfo(Path.Combine(InstallPath, Executable)).FileVersion : null;

        protected ToolKit(string installPath)
        {
            InstallPath = installPath;
        }

    }
}
