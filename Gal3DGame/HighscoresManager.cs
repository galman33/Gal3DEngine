using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Gal3DGame
{
	public class HighscoresManager
	{

		private const int MaxScoresCount = 5;

		public class Highscores
		{
			public List<PlayerNamePoints> PlayersPoints;
		}

		public class PlayerNamePoints
		{
			public string PlayerName;
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

	}
}
