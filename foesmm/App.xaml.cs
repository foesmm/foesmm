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
using System.Windows.Threading;
using CommandLine;
using foesmm.common;
using foesmm.common.game;

namespace foesmm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, FoESMM
    {
        protected IEnumerable<IGame> AvailableGames;
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
            
            AvailableGames = PreloadGames();

            Parser.Default.ParseArguments<Options>(e.Args)
                .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(this, opts))
                .WithNotParsed<Options>((errs) => HandleParseError(this, errs));
        }

        private void RunOptionsAndReturnExitCode(App app, Options opts)
        {
            Log.InitializeLogger(opts.Logging, opts.Trace);
            Log.I("foesmm started");

            throw new Exception("Test exception");

            app.Shutdown();
        }

        private void HandleParseError(App app, IEnumerable<Error> errors)
        {
            Console.WriteLine("Exit");
            app.Shutdown();
        }


        private IEnumerable<IGame> PreloadGames()
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
