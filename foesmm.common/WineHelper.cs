using System;
using System.Diagnostics;
using System.IO;
using PListNet;
using PListNet.Nodes;

namespace foesmm
{
    public static class WineHelper
    {
        public struct Environment
        {
            public Engine WineEngine;
            public string MyDocumentsPath;
            public string ApplicationDataPath;
        }
        public static void DetectFromMacWrapper(string wrapperPath)
        {
            using (var infoPlist = new FileStream(Path.Combine(wrapperPath, "Contents", "Info.plist"), FileMode.Open))
            {
                var meta = (DictionaryNode) PList.Load(infoPlist);
                var bundleIdentifier = (StringNode) meta["CFBundleIdentifier"];
                if (bundleIdentifier.Value.StartsWith("com.codeweavers"))
                {
                    var shellScript = (meta["CrossOverHelperCommand"] as StringNode)?.Value.Trim('"');
                }
            }
        }

        public static Environment ResolveEnvironment(DirectoryInfo gameDirectory)
        {
            const string marker = "drive_c";
            var index = gameDirectory.FullName.IndexOf(marker, StringComparison.InvariantCultureIgnoreCase);
            var bottleRoot = new DirectoryInfo(gameDirectory.FullName.Substring(0, index));

            var environment = new Environment
            {
                WineEngine = Engine.Wine
            };
            if (File.Exists(Path.Combine(bottleRoot.FullName, "cxbottle.conf")))
            {
                var userDirectory = Path.Combine(bottleRoot.FullName, "drive_c", "users", "crossover");
                environment.WineEngine = Engine.CrossOver;
                environment.MyDocumentsPath = Path.Combine(userDirectory, "My Documents");
                environment.ApplicationDataPath = Path.Combine(userDirectory, "Local Settings", "Application Data");
                
            }
            
            return environment;
        }
    }
}