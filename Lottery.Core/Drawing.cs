using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Core
{
    public class Drawing
    {
        public DateTime DrawDate { get; set; }

        public int[] WhiteBall { get; set; }

        public int[] WhiteBallDrawing { get; set; }

        public int PowerBall { get; set; }

        public int PowerBallDrawing { get; set; }

        public int CorrectNumbers { get; set; }

        public bool CorrectPb { get; set; }
    }
}
