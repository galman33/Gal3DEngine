using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DanielFlappyGame
{

    /// <summary>
    /// Manage the highscores of the game.
    /// </summary>
    public class HighScoresManager
    {
        XDocument doc;
        /// <summary>
        /// The highscores of the computer.
        /// </summary>
        List<Score> scores;

        /// <summary>
        /// Initiallize the highscores manager.
        /// </summary>
        public HighScoresManager()
        {
            scores = new List<Score>();
            LoadHighScores();
        }
        /// <summary>
        /// Loads the highscores data from the XML file.
        /// </summary>
        private void LoadHighScores()
        {
            doc = XDocument.Load("HighScores.xml");
            scores = (from n in doc.Root.Elements("Score")
                            select new Score
                            {
                                points =int.Parse((string)(n.Element("Points"))),
                                name = (string)n.Element("Name"),
                                date = DateTime.Parse((string)n.Element("Date"))

                            }).ToList();
        }
        /// <summary>
        /// Returns the highscores of the current computer.
        /// </summary>
        /// <returns></returns>
        public List<Score> GetScores()
        {
            return scores;
        }
        /// <summary>
        /// Add new highscore to the highscore list.
        /// </summary>
        /// <param name="score">The new highscore.</param>
        public void AddScore(Score score)
        {
            scores.Add(score);
            doc.Element("Scores").Add(new XElement(score.MakeScoreElement()));
            doc.Save("HighScores.xml");
        }
        /// <summary>
        /// Returns the best score achieved by the current computer.
        /// </summary>
        /// <returns></returns>
        public Score GetTopScore()
        {
            List<Score> scores = GetScores();
            int max = -1;
            Score maxScore = null;

            foreach(Score score in scores)
            {
                if(score.points > max)
                {
                    max = score.points;
                    maxScore = score;
                }
            }
            return maxScore;
        }

    }
    /// <summary>
    /// Handles the data about a Score
    /// </summary>
    public class Score
    {
        /// <summary>
        /// The highscore.
        /// </summary>
        public int points;
        /// <summary>
        /// The player name.
        /// </summary>
        public string name;
        /// <summary>
        /// the date of the highscore.
        /// </summary>
        public DateTime date;

        /// <summary>
        /// Converts the data into an XML element.
        /// </summary>
        /// <returns>Xml element</returns>
        public XElement MakeScoreElement()
        {
            XElement element = new XElement("Score");
            element.Add(new XElement("Points", points));
            element.Add(new XElement("Name", name));
            element.Add(new XElement("Date", date.ToShortDateString()));
            return element;
        }
        /// <summary>
        /// Converts the data into a String.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return name + " : " + points + " Points in " + date.ToShortDateString();
        }
    }
}
