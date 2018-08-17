using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using CommandLine;
using foesmm.common;
using foesmm.common.game;

namespace foesmm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : IFoESMM
    {
        public IGame CurrentGame { get; set; }

        public string CrashTrace =>
            $"FoESMM v{Assembly.GetExecutingAssembly().GetName().Version} crashed at {DateTime.UtcNow:R}\n";

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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            Parser.Default.ParseArguments<Options>(e.Args)
                .WithParsed(opts => RunOptionsAndReturnExitCode(this, opts))
                .WithNotParsed(errs => HandleParseError(this, errs));
        }

        private void RunOptionsAndReturnExitCode(IFoESMM app, Options opts)
        {
            Log.InitializeLogger(opts.Logging, opts.Trace);
            Log.I("foesmm started");

            string gameCodeBase;
            if (opts.Game != null && File.Exists(gameCodeBase = Path.Combine(CurrentDirectory, $"foesmm.game.{opts.Game}.dll")))
            {
                var gameAssembly = new AssemblyName()
                {
                    CodeBase = gameCodeBase
                };
                var assembly = Assembly.Load(gameAssembly);
                var gameType = assembly.GetTypes().First(type => typeof(IGame).IsAssignableFrom(type));
                CurrentGame = (IGame) Activator.CreateInstance(gameType);

                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                var chooser = new GameChooserWindow(PreloadGames());
                if (chooser.ShowDialog() != true || CurrentGame == null) return;
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        private void HandleParseError(IFoESMM app, IEnumerable<Error> errors)
        {
            Console.WriteLine("Exit");
            app.Shutdown();
        }

        private static IEnumerable<IGame> PreloadGames()
        {
            var games = Directory.EnumerateFiles(CurrentDirectory, "foesmm.game.*.dll");

            var preloadDomain = AppDomain.CreateDomain("Preload Domain");
            var gameList = (from gameAssembly in games select new AssemblyName {CodeBase = gameAssembly} into assemblyName select preloadDomain.Load(assemblyName) into assembly select assembly.GetTypes().First(type => typeof(IGame).IsAssignableFrom(type)) into gameType select (IGame) Activator.CreateInstance(gameType)).OrderBy(game => game.ReleaseYear).ToList();
            AppDomain.Unload(preloadDomain);

            return gameList;
        }

        private void UnhandledExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.ProcessException(this, e.Exception);
        }
    }
}
