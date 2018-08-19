using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        public IGame CurrentGame { get; private set; }

        public string Title => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute), false))?.Title ?? "Unknown Title";
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public string CrashTrace =>
            $"{Title} v{Version} crashed at {DateTime.UtcNow:R}\n";

        public void ManageGame(string assemblyCodeBase, string installDir)
        {
            var game = LoadGame(assemblyCodeBase);
            game.DetectInstallation(installDir);
            ManageGame(game);
        }

        public void ManageGame(IGame game)
        {
            CurrentGame = game;
            var mainWindow = new MainWindow(game);
            mainWindow.Show();
        }

        public void RunGame(string assemblyCodeBase, string profile = null)
        {
            RunGame(LoadGame(assemblyCodeBase), profile);
        }

        public void RunGame(IGame game, string profile = null)
        {
            CurrentGame = game;
            throw new NotImplementedException();
            Shutdown();
        }

        private static IGame LoadGame(string assemblyCodePage, AppDomain domain = null)
        {
            var gameAssembly = new AssemblyName()
            {
                CodeBase = assemblyCodePage
            };
            var assembly = domain == null ? Assembly.Load(gameAssembly) : domain.Load(gameAssembly);
            var gameType = assembly.GetTypes().First(type => typeof(IGame).IsAssignableFrom(type));
            return (IGame)Activator.CreateInstance(gameType);
        }

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
                var game = LoadGame(gameCodeBase);

                if (opts.StartGame)
                {
                    RunGame(game, opts.Profile);
                }
                else
                {
                    ManageGame(game);
                }
            }
            else
            {
                var chooser = new GameChooserWindow(PreloadGames());
                chooser.Show();
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
            var gameList = games.Select(gameAssembly => LoadGame(gameAssembly, preloadDomain)).OrderBy(game => game.ReleaseYear).ToList();
            AppDomain.Unload(preloadDomain);

            return gameList;
        }

        private void UnhandledExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.ProcessException(this, e.Exception);
        }
    }
}
