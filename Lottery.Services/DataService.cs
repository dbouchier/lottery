using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lottery.Core;

namespace Lottery.Services
{
    public static class DataService
    {
        public static List<Drawing> ImportDrawings(string fileName)
        {
            List<Drawing> drawings = new List<Drawing>();
            using (StreamReader sr = File.OpenText(fileName))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string[] dataString = s.Split(new[] {"  "}, StringSplitOptions.None);
                    Drawing drawing = new Drawing();
                    drawing.DrawDate = DateTime.Parse(dataString[0]);
                    drawing.WhiteBall = new int[5];
                    for (int n = 0; n < 5; n++)
                    {
                        drawing.WhiteBall[n] = Convert.ToInt16(dataString[n+1]);
                    }
                    drawing.PowerBall = Convert.ToInt16(dataString[6]);
                    drawings.Add(drawing);
                }
            }

            return drawings.OrderBy(d => d.DrawDate).ToList();
        }

        public static void OutputTextFile(IEnumerable<Drawing> simulatedDrawings, List<Drawing> actualDrawings, string fileName)
        {
            using (StreamWriter sw = File.CreateText(fileName))
            {
                string line;
                line = $"At least 3 WhiteBalls: {simulatedDrawings.Count(wb => wb.CorrectNumbers > 2)}";
                sw.WriteLine(line);
                line = $"At least 2 WhiteBalls: {simulatedDrawings.Count(wb => wb.CorrectNumbers > 1)}";
                sw.WriteLine(line);
                line = $"Correct PowerBall: {simulatedDrawings.Count(wb => wb.CorrectPb)}";
                sw.WriteLine(line);
                sw.WriteLine("");
                foreach (Drawing simulatedDrawing in simulatedDrawings)
                {
                    line =
                    $"Drawing Date: {simulatedDrawing.DrawDate:MM/dd/yyyy}\r\nCorrect Numbers: {simulatedDrawing.CorrectNumbers}";
                    sw.WriteLine(line);

                    line = $"Simulated: {simulatedDrawing.WhiteBall[0]} {simulatedDrawing.WhiteBall[1]} {simulatedDrawing.WhiteBall[2]} " +
                        $"{simulatedDrawing.WhiteBall[3]} {simulatedDrawing.WhiteBall[4]} PB: {simulatedDrawing.PowerBall}";
                    sw.WriteLine(line);

                    
                    line =
                        $"Actual:   {simulatedDrawing.WhiteBallDrawing[0]} {simulatedDrawing.WhiteBallDrawing[1]} {simulatedDrawing.WhiteBallDrawing[2]} " +
                        $"{simulatedDrawing.WhiteBallDrawing[3]} {simulatedDrawing.WhiteBallDrawing[4]} PB: {simulatedDrawing.PowerBallDrawing}";
                    sw.WriteLine(line);
                    
                    
                    sw.WriteLine("");
                }
            }
        }

        public static void OutputNumbers(Drawing mostCommon, Drawing leastCommon, Drawing leastCommonDateFactor)
        {
            using (StreamWriter sw = File.CreateText(@"..\..\..\Numbers.txt"))
            {
                string line;
                line = $"Most Common:  {mostCommon.WhiteBall[0]} {mostCommon.WhiteBall[1]} {mostCommon.WhiteBall[2]} " +
                       $"{mostCommon.WhiteBall[3]} {mostCommon.WhiteBall[4]} PB: {mostCommon.PowerBall}";
                sw.WriteLine(line);
                line = $"Least Common: {leastCommon.WhiteBall[0]} {leastCommon.WhiteBall[1]} {leastCommon.WhiteBall[2]} " +
                       $"{leastCommon.WhiteBall[3]} {leastCommon.WhiteBall[4]} PB: {leastCommon.PowerBall}";
                sw.WriteLine(line);
                line = $"Date Factor:  {leastCommonDateFactor.WhiteBall[0]} {leastCommonDateFactor.WhiteBall[1]} {leastCommonDateFactor.WhiteBall[2]} " +
                       $"{leastCommonDateFactor.WhiteBall[3]} {leastCommonDateFactor.WhiteBall[4]} PB: {leastCommonDateFactor.PowerBall}";
                sw.WriteLine(line);
            }
        }
    }
}
