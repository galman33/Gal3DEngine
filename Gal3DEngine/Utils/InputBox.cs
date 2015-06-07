using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine.Utils
{
    /// <summary>
    /// Helper class providing an input box for the user.
    /// </summary>
	public static class InputBox
	{
        /// <summary>
        /// Show an input box with a given - default text, title , position.
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="title">The Title of the input box.</param>
        /// <param name="defaultResponse">The default response of the input box.</param>
        /// <param name="xPos">The X cordinate of the input box.</param>
        /// <param name="yPos">The Y cordinate of the input box.</param>
        /// <returns></returns>
		public static string Show(string prompt, string title = "", string defaultResponse = "", int xPos = -1, int yPos = -1)
		{
			return Interaction.InputBox(prompt, title, defaultResponse, xPos, yPos);
		}

	}
}
