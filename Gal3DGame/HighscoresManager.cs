using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gal3DGame
{
	/// <summary>
	/// An helper class responsble for highscores managment.
	/// </summary>
	public static class HighscoresManager
	{

		private const int MaxScoresCount = 5;

		/// <summary>
		/// Player scores holder.
		/// </summary>
		public class Highscores
		{
			public List<PlayerNamePoints> PlayersPoints;
		}


		/// <summary>
		/// A player name and points holder.
		/// </summary>
		public class PlayerNamePoints
		{
			/// <summary>
			/// The name of the player.
			/// </summary>
			public string PlayerName;
			/// <summary>
			/// The amount of points that the player collected.
			/// </summary>
			public int Points;

			public PlayerNamePoints()
			{

			}

			public PlayerNamePoints(string playerName, int points)
			{
				this.PlayerName = playerName;
				this.Points = points;
			}
		}

		/// <summary>
		/// Add a new highscore.
		/// </summary>
		/// <param name="playerName">The name of the player.</param>
		/// <param name="points">The amount of points collected by the player.</param>
		public static void AddScore(string playerName, int points)
		{
			Highscores highscores = RetriveHighScores();

			AddScoreToHighscores(highscores, playerName, points);

			OrderHighscores(highscores);

			TrimHighscores(highscores);

			SaveHighScores(highscores);
		}

		private static Highscores RetriveHighScores()
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Highscores));
			using (FileStream reader = File.OpenRead("highscores.xml"))
			{
				return (Highscores) serializer.Deserialize(reader);
			}
		}

		private static void AddScoreToHighscores(Highscores highscores, string playerName, int points)
		{
			highscores.PlayersPoints.Add(new PlayerNamePoints(playerName, points));
		}

		private static void OrderHighscores(Highscores highscores)
		{
			highscores.PlayersPoints = highscores.PlayersPoints.OrderByDescending(s => s.Points).ToList();
		}

		private static void TrimHighscores(Highscores highscores)
		{
			while(highscores.PlayersPoints.Count > MaxScoresCount)
			{
				highscores.PlayersPoints.RemoveAt(MaxScoresCount);
			}
		}

		private static void SaveHighScores(Highscores highscores)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Highscores));
			using (FileStream writer = File.OpenWrite("highscores.xml"))
			{
				serializer.Serialize(writer, highscores);
			}
		}

		/// <summary>
		/// Retrives a string of the top 5 players and their scores.
		/// </summary>
		/// <returns>A string of the top players scores.</returns>
		public static string GetHighscoresText()
		{
			var highscores = RetriveHighScores();

			string txt = "";
			int i = 1;
			foreach(var score in highscores.PlayersPoints)
			{
				if(!string.IsNullOrEmpty(txt))
					txt += "\r\n";
				txt += i + ". " + score.PlayerName + " - " + score.Points;
				i++;
			}

			return txt;
		}

	}
}
