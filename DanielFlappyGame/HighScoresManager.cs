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
            
        }

        private void LoadHighScores()
        {
            doc = XDocument.Load("HighScores.xml");
            
        }

        public List<Score> GetScores()
        {
            return null;
        }

        public void AddScore(Score score)
        {
            
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

        public string MakeScoreElement()
        {           
           return  "<Score>" + "<Points>" + points + "</Points>" + "<Name>" + points + "</Name>" + "<Date>" + points + "</Date> </Scores>";
        }
    }
}
