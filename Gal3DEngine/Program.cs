using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run(60, 60);
        }
    }
}
