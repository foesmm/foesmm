using System;

namespace foesmm
{
    public class GameSelectedEventArgs : EventArgs
    {
        public IGame Game { get; }
        
        public GameSelectedEventArgs(IGame game)
        {
            Game = game;
        }
    }
}