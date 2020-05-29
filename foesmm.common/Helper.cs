using System;
using System.IO;
using System.Reflection;
using Eto.Forms;

namespace foesmm
{
    public static class Helper
    {
        public static string CurrentDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static DirectoryInfo LocalApplicationDataDirectory => Application.Instance.Platform.IsMac
            ? new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Library", "Application Support", "foesmm"))
            : new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "foesmm"));

        public static FileStream OpenRead(this FileInfo fileInfo, int bufferSize)
        {
            return new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, true);
        }
    }
}