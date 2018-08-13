using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using CommandLine;

namespace foesmm
{
    internal class Options
    {
        [Option('g', "game", HelpText = "Game to manage or run.")]
        public string Game { get; set; }

        [Option('p', "profile", HelpText = "Profile of game to run.")]
        public string Profile { get; set; }

        [Option('r', "run", HelpText = "Start game.")]
        public bool StartGame { get; set; }

        [Option("log", HelpText = "Enable file logging.")]
        public bool Logging { get; set; }

        [Option("trace", HelpText = "Enable trace logging.")]
        public bool Trace { get; set; }
    }
}
