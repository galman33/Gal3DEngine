using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine.Utils
{
	public static class InputBox
	{

		public static string InputBox(string prompt, string title = "", string defaultResponse = "", int xPos = -1, int yPos = -1)
		{
			return Interaction.InputBox(prompt, title, defaultResponse, xPos, yPos);
		}

	}
}
