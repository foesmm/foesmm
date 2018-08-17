using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Microsoft.VisualBasic.Devices;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace foesmm.common
{
    public static class Log
    {
        private static bool Initialized { get; set; }

        public static void InitializeLogger(bool enableLog, bool trace)
        {
            if (Initialized) return;

            var config = new LoggingConfiguration();

#if DEBUG
            var consoleTarget = new ColoredConsoleTarget("Debug")
            {
                Layout = @"${date:format=HH\:mm\:ss} foesmm/${level:format=FirstCharacter}> ${message} ${exception}"
            };
            config.AddTarget(consoleTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget);
#endif

            if (enableLog)
            {
                var fileTarget = new FileTarget("Logging")
                {
                    FileName = "${basedir}/foesmm.log",
                    Layout = @"${date:format=HH\:mm\:ss} foesmm/${level:format=FirstCharacter}> ${message} ${exception}"
                };
                config.AddTarget(fileTarget);
                config.AddRule(trace ? LogLevel.Trace : LogLevel.Info, LogLevel.Fatal, fileTarget);
            }

            LogManager.Configuration = config;

            Initialized = true;
        }

        public static void F(string message, object context = null)
        {
            LogMessage(LogLevel.Fatal, message, context);
        }

        public static void E(string message, object context = null)
        {
            LogMessage(LogLevel.Error, message, context);
        }

        public static void W(string message, object context = null)
        {
            LogMessage(LogLevel.Warn, message, context);
        }

        public static void I(string message, object context = null)
        {
            LogMessage(LogLevel.Info, message, context);
        }

        public static void D(string message, object context = null)
        {
            LogMessage(LogLevel.Debug, message, context);
        }

        public static void T(string message, object context = null)
        {
            LogMessage(LogLevel.Trace, message, context);
        }

        private static void LogMessage(LogLevel level, string message, object context = null)
        {
            var logger = LogManager.GetLogger("foesmm");
            if (context != null)
            {
                message = $"[CTX: {context}] {message}";
            }
            logger.Log(level, message);
        }

        public static void ProcessException(IFoESMM app, Exception exception)
        {
            var appdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Path.Combine("foesmm","crushdumps"));
            if (!Directory.Exists(appdata))
            {
                Directory.CreateDirectory(appdata);
            }
            var filename = $"dump-{DateTime.UtcNow:yyyyMMdd-HHmmss}.txt";
            var crashdump = Path.Combine(appdata, filename);

            Log.D(crashdump);

            var computerInfo = new ComputerInfo();

            var dump = string.Concat(
                app.CrashTrace,
                app.CurrentGame?.CrashTrace,
                $"OS: {computerInfo.OSFullName} v{computerInfo.OSVersion}\n",
                Wine.IsWine ? $"Wine: v{Wine.GetVersion()}, Host: {Wine.GetHost()}\n" : null,
                $"Exception Signature: {exception.Guid()}\n",
                $"Source: {Path.GetFileName(exception.FirstFrame().GetFileName())}:{exception.FirstFrame().GetFileLineNumber()}\n",
                exception.StackTrace
            );

            var innerException = exception.InnerException;
            while (innerException != null)
            {
                dump += innerException.ToString();
                innerException = innerException.InnerException;
            };

            Log.F(dump);
            
            File.WriteAllText(crashdump, dump);

            var reporter = new CrashReporter(app, exception.Message, crashdump);
            reporter.ShowDialog();

            Console.WriteLine();
        }
    }
}
