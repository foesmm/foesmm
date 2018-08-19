using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace foesmm.common
{
    public static class RegistryHelper
    {
        public static string CheckInstallInfo(string path, string[] possibleKeys, string valueKey)
        {
            var registryViews = new[] { RegistryView.Registry32, RegistryView.Registry64 };

            foreach (var view in registryViews)
            {
                foreach (var key in possibleKeys)
                {
                    try
                    {
                        var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view);
                        var registryPath = baseKey.OpenSubKey(Path.Combine(path, key));
                        object value;
                        if (registryPath != null && (value = registryPath.GetValue(valueKey)) != null)
                        {
                            return value.ToString();
                        }
                    }
                    catch (Exception e)
                    {
                        Log.E(e.Message);
                    }
                }
            }

            return null;
        }
    }
}
