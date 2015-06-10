using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DGame
{
	/// <summary>
	/// An helper class holding a reference to a Random object.
	/// </summary>
    class RandomHelper
    {

        private static Random random = new Random();

		/// <summary>
		/// An instance of a Random object.
		/// </summary>
		public static Random Random
		{
			get
			{
				return random;
			}
		}

    }
}
