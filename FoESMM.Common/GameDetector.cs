using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace FoESMM.Common
{
    public class GameDetector
    {
        public static string CheckSteamInstallation(string sSteamAppId, ref Dictionary<string, string> dReplacements)
        {
            var foundPath = CheckCustomUninstallKey("Steam App " + sSteamAppId);
            if (foundPath != null)
            {
                dReplacements.Add("%SteamAppId%", sSteamAppId);
            }
            return foundPath;
        }

        public static string CheckCustomUninstallKey(string sUninstallId)
        {
            var steamUninstallKey =
                Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" +
                                                 sUninstallId, false);

            return steamUninstallKey?.GetValue("InstallLocation").ToString();
        }
    }
}
