using System;
using System.Collections.Generic;
using System.Linq;
using Lottery.Core;

namespace Lottery.Services
{
    public class AnalysisServices
    {
        private List<Drawing> _drawings;
        private List<NumberResult> _numberResults;
        private List<NumberResult> _pbResults;
        private List<Drawing> _drawingResults;

        public AnalysisServices(List<Drawing> drawings)
        {
            _drawings = drawings;
            //BuildNumberResults(null, null);
        }

        #region BuildNumberResults

        public void BuildNumberResults(DateTime? startDate, DateTime? endDate, DateTime drawDate, bool appendOnly, bool resetCountersOnRealign)
        {
            if (!appendOnly)
            {
                _numberResults = new List<NumberResult>();
                _pbResults = new List<NumberResult>();
            }

            DateTime drawingStartDate = startDate ?? DateTime.MinValue;
            DateTime drawingEndDate = endDate ?? DateTime.MaxValue;
            bool resetWb = false;
            bool resetPb = false;

            if (resetCountersOnRealign)
            {
                if (drawDate == DateTime.Parse("11/5/1997"))
                {
                    //Reset Powerball
                    _pbResults = new List<NumberResult>();
                    resetPb = true;
                }
                else if (drawDate == DateTime.Parse("10/9/2002"))
                {
                    //Reset WhiteBalls
                    _numberResults = new List<NumberResult>();
                    resetWb = true;
                }
                else if (drawDate == DateTime.Parse("8/28/2005"))
                {
                    //Reset WhiteBalls
                    _numberResults = new List<NumberResult>();
                    resetWb = true;
                }
                else if (drawDate == DateTime.Parse("1/07/2009"))
                {
                    //Reset WhiteBalls
                    //Reset Powerball
                    _numberResults = new List<NumberResult>();
                    _pbResults = new List<NumberResult>();
                    resetPb = true;
                    resetWb = true;
                }
                else if (drawDate == DateTime.Parse("1/15/2012"))
                {
                    //Reset Powerball
                    _pbResults = new List<NumberResult>();
                    resetPb = true;
                }
                else if (drawDate == DateTime.Parse("10/07/2015"))
                {
                    //Reset WhiteBalls
                    //Reset Powerball
                    _numberResults = new List<NumberResult>();
                    _pbResults = new List<NumberResult>();
                    resetPb = true;
                    resetWb = true;
                }
            }
            //if (appendOnly)
            //{
            //    drawingStartDate = drawingEndDate;
            //}

            if (!appendOnly || resetWb)
            {
                for (short numberCounter = 0; numberCounter < 69; numberCounter++)
                {
                    NumberResult numberResult = new NumberResult();
                    numberResult.Number = numberCounter + 1;
                    numberResult.Frequency = 0;
                    _numberResults.Add(numberResult);
                }
            }
            if (!appendOnly || resetPb)
            {
                for (short numberCounter = 0; numberCounter < 45; numberCounter++)
                {
                    NumberResult pbResult = new NumberResult();
                    pbResult.Number = numberCounter + 1;
                    pbResult.Frequency = 0;
                    _pbResults.Add(pbResult);
                }
            }

            TimeSpan ts;

            foreach (Drawing drawing in _drawings.Where(d => d.DrawDate <= drawingEndDate && d.DrawDate >= drawingStartDate))
            {
                ts = drawingEndDate - _drawings[0].DrawDate;
                for (int wbCounter = 0; wbCounter < 5; wbCounter++)
                {
                    int wbIndex = drawing.WhiteBall[wbCounter] - 1;
                    _numberResults[wbIndex].Frequency += 1;
                    //if (!appendOnly)
                    //{
                    //    _numberResults[wbIndex].Points = _numberResults[wbIndex].Frequency + CalculateWbPoints(_numberResults[wbIndex].Number, drawing.DrawDate);
                    //}
                }



                _pbResults[drawing.PowerBall - 1].Frequency += 1;
                _pbResults[drawing.PowerBall - 1].DrawingsAvailable += 1;
                //if (!appendOnly)
                //{
                //    _pbResults[drawing.PowerBall - 1].Points = _pbResults[drawing.PowerBall - 1].Frequency + CalculateWbPoints(_pbResults[drawing.PowerBall - 1].Number, drawing.DrawDate);
                //}

                #region Adjustments

                double adj = 0;

                //Increment number of drawings per number
                for (int n = 0; n < 69; n++)
                {
                    //Recalculate points

                    if (appendOnly)
                    {
                        _numberResults[n].Points = _numberResults[n].ProbabilityAverage +
                                                   CalculateWbPoints(_numberResults[n].Number, drawingEndDate);
                        if (n < 45)
                        {
                            _pbResults[n].Points = _pbResults[n].ProbabilityAverage +
                                                   CalculatePbPoints(_pbResults[n].Number, drawingEndDate);
                        }
                    }

                    //White ball
                    if (_numberResults[n].Number <= 49)
                    {
                        _numberResults[n].DrawingsAvailable += 1;
                    }

                    else if (_numberResults[n].Number <= 53 && drawing.DrawDate >= DateTime.Parse("10/9/2002"))
                    {
                        _numberResults[n].DrawingsAvailable += 1;
                    }
                    else if (_numberResults[n].Number <= 55 && drawing.DrawDate >= DateTime.Parse("8/28/2005"))
                    {
                        _numberResults[n].DrawingsAvailable += 1;
                    }
                    else if (_numberResults[n].Number <= 59 && drawing.DrawDate >= DateTime.Parse("1/07/2009"))
                    {
                        _numberResults[n].DrawingsAvailable += 1;
                    }
                    else if (_numberResults[n].Number <= 69 && drawing.DrawDate >= DateTime.Parse("10/07/2015"))
                    {
                        _numberResults[n].DrawingsAvailable += 1;
                    }

                    int maxWb = GetMaxWhiteBall(drawing.DrawDate);
                    if (maxWb >= _numberResults[n].Number)
                    {
                        _numberResults[n].Probability += (double) 5 / maxWb;
                    }

                    //Powerball
                    if (n < 45)
                    {
                        if (_pbResults[n].Number <= 26)
                        {
                            _pbResults[n].DrawingsAvailable++;

                        }
                        else if (_pbResults[n].Number <= 45 && drawing.DrawDate <= DateTime.Parse("11/5/1997"))
                        {
                            _pbResults[n].DrawingsAvailable++;
                        }
                        else if (_pbResults[n].Number <= 42 && drawing.DrawDate < DateTime.Parse("1/1/2009"))
                        {
                            _pbResults[n].DrawingsAvailable++;
                        }
                        else if (_pbResults[n].Number <= 39 && drawing.DrawDate < DateTime.Parse("1/15/2012"))
                        {
                            _pbResults[n].DrawingsAvailable++;
                        }
                        else if (_pbResults[n].Number <= 35 && drawing.DrawDate < DateTime.Parse("10/7/2015"))
                        {
                            _pbResults[n].DrawingsAvailable++;
                        }

                        int maxPb = GetMaxPowerBall(drawing.DrawDate);
                        _pbResults[n].Probability += (double)1 / maxPb;
                    }

                }

                #endregion
            }

            if (!appendOnly)
            {
                for (int n = 0; n < 69; n++)
                {
                    //Recalculate points
                    _numberResults[n].Points = _numberResults[n].ProbabilityAverage +
                                                CalculateWbPoints(_numberResults[n].Number, drawingEndDate);
                    if (n < 45)
                    {
                        _pbResults[n].Points = _pbResults[n].ProbabilityAverage +
                                                CalculatePbPoints(_pbResults[n].Number, drawingEndDate);
                    }
                }
            }
        }

        #endregion

        private int GetMaxWhiteBall(DateTime drawingDate)
        {
            if (drawingDate < DateTime.Parse("10/9/2002"))
            {
                return 49;
            }
            if (drawingDate < DateTime.Parse("8/28/2005"))
            {
                return 53;
            }
            if (drawingDate < DateTime.Parse("1/07/2009"))
            {
                return 55;
            }
            if (drawingDate < DateTime.Parse("10/07/2015"))
            {
                return 59;
            }
            
            return 69;
        }

        private int GetMaxPowerBall(DateTime drawingDate)
        {
            if (drawingDate < DateTime.Parse("11/5/1997"))
            {
                return 45;
            }
            if (drawingDate < DateTime.Parse("1/07/2009"))
            {
                return 42;
            }
            if (drawingDate < DateTime.Parse("1/15/2012"))
            {
                return 39;
            }
            if (drawingDate < DateTime.Parse("10/07/2015"))
            {
                return 35;
            }

            return 26;
        }


        public Drawing LeastCommon(DateTime drawingDate)
        {

            Drawing results = new Drawing();
            List<NumberResult> sortedList = _numberResults.Where(d => d.DrawingsAvailable > 0).OrderBy(d => d.ProbabilityAverage).ToList();
            List<NumberResult> sortedPbList = _pbResults.Where(d => d.DrawingsAvailable > 0 && d.Number <= GetMaxPowerBall(drawingDate)).OrderBy(d => d.ProbabilityAverage).ToList();
            results.WhiteBall = new int[5];
            if (sortedList.Count >= 5)
            {
                for (int n = 0; n < 5; n++)
                {
                    results.WhiteBall[n] = sortedList[n].Number;
                }
            }
            if (sortedPbList.Count >= 1)
            {
                results.PowerBall = sortedPbList[0].Number;
            }
            results.DrawDate = drawingDate;

            return results;
        }

        public Drawing LeastCommonDateFactor(DateTime drawingDate)
        {

            Drawing results = new Drawing();
            List<NumberResult> sortedList = _numberResults.Where(d => d.DrawingsAvailable > 0).OrderBy(d => d.Points).ToList();
            List<NumberResult> sortedPbList = _pbResults.Where(d => d.DrawingsAvailable > 0 && d.Number <= GetMaxPowerBall(drawingDate)).OrderBy(d => d.Points).ToList();


            results.WhiteBall = new int[5];
            if (sortedList.Count >= 5)
            {
                for (int n = 0; n < 5; n++)
                {
                    results.WhiteBall[n] = sortedList[n].Number;
                }
            }
            if (sortedPbList.Count >= 1)
            {
                results.PowerBall = sortedPbList[0].Number;
            }
            results.DrawDate = drawingDate;

            return results;
        }

        public Drawing MostCommon(DateTime drawingDate)
        {

            Drawing results = new Drawing();
            List<NumberResult> sortedList = _numberResults.Where(d => d.DrawingsAvailable > 0).OrderByDescending(d => d.FrequencyAverage).ToList();
            List<NumberResult> sortedPbList = _pbResults.Where(d => d.DrawingsAvailable > 0 && d.Number <= GetMaxPowerBall(drawingDate)).OrderByDescending(d => d.FrequencyAverage).ToList();
            results.WhiteBall = new int[5];
            if (sortedList.Count >= 5)
            {
                for (int n = 0; n < 5; n++)
                {
                    results.WhiteBall[n] = sortedList[n].Number;
                }
            }
            if (sortedPbList.Count >= 1)
            {
                results.PowerBall = sortedPbList[0].Number;
            }
            results.DrawDate = drawingDate;

            return results;
        }

        public void ProcessNumbers(DateTime drawDate)
        {
            drawDate = drawDate.AddDays(-1);
            BuildNumberResults(null, drawDate, drawDate, false, false);
            Drawing mostCommon = MostCommon(drawDate);
            Drawing leastCommon = LeastCommon(drawDate);
            Drawing leastCommonDateFactor = LeastCommonDateFactor(drawDate);
            DataService.OutputNumbers(mostCommon, leastCommon, leastCommonDateFactor);
        }

        private double CalculatePoints(DateTime drawDate, DateTime lastShownDate)
        {
            TimeSpan ts = drawDate - lastShownDate;

            double points = 0;
            if (ts.Days <= 4)
            {
                points = .5;
            }
            else if (ts.Days <= 14)
            {
                points = .10;
            }
            else if (ts.Days <= 21)
            {
                points = .05;
            }
            else if (ts.Days <= 45)
            {
                points = .01;
            }
            return points;
        }

        private double CalculateWbPoints(int number, DateTime drawDate)
        {
            List<Drawing> drawings = _drawings.Where(d => d.DrawDate < drawDate && d.DrawDate > drawDate.AddDays(-45)).OrderBy(d => d.DrawDate).ToList();
            double points = 0;
            int counter = 1;
            foreach (Drawing drawing in drawings)
            {
                if (drawing.WhiteBall.Contains(number))
                {
                    points += CalculatePoints(drawDate, drawing.DrawDate);
                }
                counter++;
            }

            return points;
        }

        private double CalculatePbPoints(int number, DateTime drawDate)
        {
            List<Drawing> drawings = _drawings.Where(d => d.DrawDate < drawDate && d.DrawDate > drawDate.AddDays(-45)).OrderBy(d => d.DrawDate).ToList();
            double points = 0;
            int counter = 1;
            foreach (Drawing drawing in drawings)
            {
                if (drawing.PowerBall == number)
                {
                    points += CalculatePoints(drawDate, drawing.DrawDate);
                }
            }

            return points;
        }

        private void AssignWbRand(Drawing results, int n, int max)
        {
            Random rnd = new Random();
            bool anotherRandom = true;
            while (anotherRandom)
            {
                var rand = rnd.Next(1, max);
                if (!results.WhiteBall.Contains(rand))
                {
                    results.WhiteBall[n] = rand;
                    anotherRandom = false;
                }
            }
        }

        private void AssignPbRand(Drawing results, int max)
        {
            Random rnd = new Random();
            var rand = rnd.Next(1, max);
            results.PowerBall = rand;
        }

        public Drawing Random(DateTime drawingDate)
        {

            Drawing results = new Drawing();
            results.WhiteBall = new int[5];
            
            for (int n = 0; n < 5; n++)
            {
                AssignWbRand(results, n, GetMaxWhiteBall(drawingDate));
            }

            AssignPbRand(results, GetMaxPowerBall(drawingDate));

            results.DrawDate = drawingDate;

            return results;
        }
    }
}
