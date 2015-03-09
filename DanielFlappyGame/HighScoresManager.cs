using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DanielFlappyGame
{
    public class HighScoresManager
    {
        XDocument doc;
        List<Score> scores;
        public HighScoresManager()
        {
            scores = new List<Score>();
            LoadHighScores();
        }

        private void LoadHighScores()
        {
            doc = XDocument.Load("HighScores.xml");
            scores = (from n in doc.Root.Elements("Score")
                            select new Score
                            {
                                points =int.Parse(n.Element("Points").Value),
                                name = (string)n.Element("Name"),
                                date = DateTime.Parse((string)n.Element("Date"))

                            }).ToList();
        }

        public List<Score> GetScores()
        {
            return scores;
        }

        public void AddScore(Score score)
        {
            scores.Add(score);
            doc.Element("Scores").Add(new XElement(score.MakeScoreElement()));
            doc.Save("HighScores.xml");
        }

        public Score GetTopScore()
        {
            List<Score> scores = GetScores();
            int max = 0;
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

    public class Score
    {
        public int points;
        public string name;
        public DateTime date;

        public XElement MakeScoreElement()
        {
            XElement element = new XElement("Score");
            element.Add(new XElement("Points", points));
            element.Add(new XElement("Name", name));
            element.Add(new XElement("Date", date.ToShortDateString()));
            return element;
        }   
    }
}
