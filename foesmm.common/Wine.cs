using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace foesmm.common
{
    public class Wine
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string moduleName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr hModule);

        // ReSharper disable InconsistentNaming
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr wine_get_version();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr wine_get_host_version(out IntPtr host, out IntPtr release);
        // ReSharper restore InconsistentNaming

        public static bool IsWine => GetVersion() != null;

        public static string GetVersion()
        {
            var hModule = LoadLibrary("ntdll.dll");
            var hProc = GetProcAddress(hModule, "wine_get_version");
            if (hProc != (IntPtr) 0)
            {
                var getVersion = (wine_get_version) Marshal.GetDelegateForFunctionPointer(hProc, typeof(wine_get_version));
                var result = Marshal.PtrToStringAnsi(getVersion());
                return result;
            }

            FreeLibrary(hModule);

            return null;
        }

        public static string GetHost()
        {
            var hModule = LoadLibrary("ntdll.dll");
            var hProc = GetProcAddress(hModule, "wine_get_host_version");
            if (hProc != (IntPtr)0)
            {
                var getHost = (wine_get_host_version)Marshal.GetDelegateForFunctionPointer(hProc, typeof(wine_get_host_version));
                getHost(out var host, out var version);
                return $"{Marshal.PtrToStringAnsi(host)} v{Marshal.PtrToStringAnsi(version)}";
            }

            FreeLibrary(hModule);

            return null;
        }
    }
}
