using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lottery.Core;
using Lottery.Services;

namespace Lottery.Numbers
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Drawing> drawings = DataService.ImportDrawings(@"..\..\..\data.txt");
            //AnalysisServices analysisServices = new AnalysisServices(drawings);
            AnalysisServices analysisService = new AnalysisServices(drawings);
            analysisService.ProcessNumbers(DateTime.Now);
            //analysisService.ProcessNumbers(DateTime.Parse("07/29/2017"));
        }
    }
}
