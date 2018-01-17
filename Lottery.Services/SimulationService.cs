using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lottery.Core;

namespace Lottery.Services
{
    public class SimulationService
    {
        private List<Drawing> _drawings { get; set; }
        private List<Drawing> _simulatedDrawingsLeast { get; set; }
        private List<Drawing> _simulatedDrawingsLeastDateFactor { get; set; }
        private List<Drawing> _simulatedDrawingsMost{ get; set; }
        private List<Drawing> _simulatedDrawingsRand { get; set; }


        public SimulationService(List<Drawing> drawings)
        {
            _drawings = drawings;
            _simulatedDrawingsLeast = new List<Drawing>();
            _simulatedDrawingsLeastDateFactor = new List<Drawing>();
            _simulatedDrawingsMost = new List<Drawing>();
            _simulatedDrawingsRand = new List<Drawing>();
        }

        public void SimulateDrawings()
        {
            AnalysisServices analysisService = new AnalysisServices(_drawings);
            int drawingCounter = 0;
            DateTime lastStartDate = DateTime.MinValue;
            foreach (Drawing drawing in _drawings)
            {
                Console.WriteLine($"Processing Drawing from {drawing.DrawDate:d}");
                if (drawing.DrawDate == DateTime.Parse("02/22/2006"))
                {
                    int x = 1;
                }
                bool appendOnly = drawingCounter != 0;
                analysisService.BuildNumberResults(lastStartDate, drawing.DrawDate.AddDays(-1), drawing.DrawDate, appendOnly, false);
                lastStartDate = drawing.DrawDate;
                DateTime drawDate;

                if (drawingCounter < _drawings.Count)
                {
                    drawDate = _drawings[drawingCounter].DrawDate;
                }
                else
                {
                    drawDate = DateTime.Now;
                }

                Drawing simulateDrawing = analysisService.LeastCommon(drawDate);
                simulateDrawing.WhiteBallDrawing = _drawings[drawingCounter].WhiteBall;
                simulateDrawing.PowerBallDrawing = _drawings[drawingCounter].PowerBall;
                if (drawingCounter < _drawings.Count - 1)
                {
                    CalculateCorrectNumbers(simulateDrawing);
                }

                _simulatedDrawingsLeast.Add(simulateDrawing);

                simulateDrawing = analysisService.MostCommon(drawDate);
                simulateDrawing.WhiteBallDrawing = _drawings[drawingCounter].WhiteBall;
                simulateDrawing.PowerBallDrawing = _drawings[drawingCounter].PowerBall;
                if (drawingCounter < _drawings.Count - 1)
                {
                    CalculateCorrectNumbers(simulateDrawing);
                }

                _simulatedDrawingsMost.Add(simulateDrawing);

                simulateDrawing = analysisService.Random(drawDate);
                simulateDrawing.WhiteBallDrawing = _drawings[drawingCounter].WhiteBall;
                simulateDrawing.PowerBallDrawing = _drawings[drawingCounter].PowerBall;
                if (drawingCounter < _drawings.Count - 1)
                {
                    CalculateCorrectNumbers(simulateDrawing);
                }

                _simulatedDrawingsRand.Add(simulateDrawing);

                simulateDrawing = analysisService.LeastCommonDateFactor(drawDate);
                simulateDrawing.WhiteBallDrawing = _drawings[drawingCounter].WhiteBall;
                simulateDrawing.PowerBallDrawing = _drawings[drawingCounter].PowerBall;
                if (drawingCounter < _drawings.Count - 1)
                {
                    CalculateCorrectNumbers(simulateDrawing);
                }

                _simulatedDrawingsLeastDateFactor.Add(simulateDrawing);

                drawingCounter++;
            }

            List<Drawing> BestSimulationLeast = _simulatedDrawingsLeast.OrderByDescending(s => s.CorrectNumbers).ToList();
            List<Drawing> BestSimulationMost = _simulatedDrawingsMost.OrderByDescending(s => s.CorrectNumbers).ToList();
            List<Drawing> BestSimulationRand = _simulatedDrawingsRand.OrderByDescending(s => s.CorrectNumbers).ToList();
            List<Drawing> BestSimulationLeastDateFactor = _simulatedDrawingsLeastDateFactor.OrderByDescending(s => s.CorrectNumbers).ToList();

            DataService.OutputTextFile(BestSimulationLeast.Where(s => s.CorrectNumbers > 1 || s.CorrectPb).OrderByDescending(s => s.CorrectNumbers), _drawings, "BestSimulationLeast.txt");
            DataService.OutputTextFile(BestSimulationMost.Where(s => s.CorrectNumbers > 1 || s.CorrectPb).OrderByDescending(s => s.CorrectNumbers), _drawings, "BestSimulationMost.txt");
            DataService.OutputTextFile(BestSimulationRand.Where(s => s.CorrectNumbers > 1 || s.CorrectPb).OrderByDescending(s => s.CorrectNumbers), _drawings, "BestSimulationRand.txt");
            DataService.OutputTextFile(BestSimulationLeastDateFactor.Where(s => s.CorrectNumbers > 1 || s.CorrectPb).OrderByDescending(s => s.CorrectNumbers), _drawings, "BestSimulationLeastDateFactor.txt");
            DataService.OutputTextFile(BestSimulationLeastDateFactor.OrderBy(s => s.DrawDate), _drawings, "BestSimulationLeastDateFactorDetailed.txt");
        }

        private void CalculateCorrectNumbers(Drawing simulateDrawing)
        {
            for (int n = 0; n < 5; n++)
            {
                if (simulateDrawing.WhiteBallDrawing.Contains(simulateDrawing.WhiteBall[n]))
                {
                    simulateDrawing.CorrectNumbers++;
                }
            }
            if (simulateDrawing.PowerBallDrawing == simulateDrawing.PowerBall)
            {
                simulateDrawing.CorrectPb = true;
            }
        }
    }
}
