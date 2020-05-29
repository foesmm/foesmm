using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using LibGit2Sharp;

namespace foesmm
{
    public class GameLibrary
    {
        private static readonly Lazy<GameLibrary> Library = new Lazy<GameLibrary>(() => new GameLibrary());
        public static GameLibrary Instance => Library.Value;

        private GameLibrary()
        {
            Games = new ObservableCollection<IGame>();
            SupportedGames = new List<IGameInfo>();

            var currentDirectory = new DirectoryInfo(Path.Combine(App.DataPath, "Games"));
            foreach (var gameManifest in currentDirectory.GetFiles("*.manifest"))
            {
                SupportedGames.Add(Game.LoadInfo(gameManifest));
            }
        }

        public ObservableCollection<IGame> Games { get; }
        public ICollection<IGameInfo> SupportedGames { get; }


        public void TryLoad(Uri uri)
        {
            Game.TryLoad(uri, SupportedGames, game => Games.Add(game));
        }
    }
}