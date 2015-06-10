using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;

namespace DanielFlappyGame
{
    /// <summary>
    /// Starts the Game.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The current world of the game.
        /// </summary>
        public static Game world;
        /// <summary>
        /// Start the Game.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            world = new MenuWorld();
        }
    }
}
