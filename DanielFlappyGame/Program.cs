using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;

namespace DanielFlappyGame
{
    class Program
    {
        public static Game world;
        static void Main(string[] args)
        {
            world = new MenuWorld();
        }
    }
}
