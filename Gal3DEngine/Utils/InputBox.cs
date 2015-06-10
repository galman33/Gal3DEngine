using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine.Utils
{

	/// <summary>
	/// Popups an input box to ask the user a question and receive his input.
	/// </summary>
	public static class InputBox
	{

		/// <summary>
		/// Show an input box.
		/// </summary>
		/// <param name="prompt">The question to ask the user.</param>
		/// <param name="title">The title of the popup.</param>
		/// <param name="defaultResponse">The default user input.</param>
		/// <param name="xPos"></param>
		/// <param name="yPos"></param>
		/// <returns></returns>
		public static string Show(string prompt, string title = "", string defaultResponse = "", int xPos = -1, int yPos = -1)
		{
			return Interaction.InputBox(prompt, title, defaultResponse, xPos, yPos);
		}

	}
}
