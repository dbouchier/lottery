using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Core
{
    public class NumberResult
    {
        public int Number { get; set; }

        public int Frequency { get; set; }

        public int DrawingsAvailable { get; set; }

        public double Probability { get; set; }

        public double FrequencyAverage
        {
            get
            {
                if (DrawingsAvailable == 0)
                {
                    return 0;
                }
                return Math.Round((double)Frequency / (double)DrawingsAvailable, 5);
            }
        }

        public double PointsAverage
        {
            get
            {
                if (DrawingsAvailable == 0)
                {
                    return 0;
                }
                return Math.Round((double)Points / (double)DrawingsAvailable, 5);
            }
        }

        public double ProbabilityAverage
        {
            get
            {
                if (Frequency > 0)
                {
                    return (double) Frequency / Probability;
                }
                return 0;
            }
        }

        public double Points { get; set; }
    }
}
