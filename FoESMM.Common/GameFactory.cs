using System;
using System.Linq;
using System.Reflection;

namespace FoESMM.Common
{
    public class GameFactory
    {
        public static Game InitializeGame(string sGame)
        {
            Game game = null;
            var gameType = Assembly.Load($"FoESMM.Games.{sGame}").GetTypes().First(t => t.Name == $"{sGame}Game");

            if (gameType != null)
            {
                game = (Game) Activator.CreateInstance(gameType);
                game.InitializeGame();
            }

            return game;
        }
    }
}