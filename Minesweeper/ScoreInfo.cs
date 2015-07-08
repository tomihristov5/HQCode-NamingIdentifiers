using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class ScoreInfo
    {
        public string PlayerName { get; set; }

        public int PlayerPoints { get; set; }

        public ScoreInfo()
        {

        }

        public ScoreInfo(string name, int points)
        {
            this.PlayerName = name;
            this.PlayerPoints = points;
        }
    }
}
