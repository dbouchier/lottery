using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lottery.Core;
using Lottery.Services;

namespace Lottery.Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Drawing> drawings = DataService.ImportDrawings(@"..\..\..\data.txt");
            //AnalysisServices analysisServices = new AnalysisServices(drawings);
            SimulationService simulationService = new SimulationService(drawings);
            simulationService.SimulateDrawings();
        }
    }
}
