using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsColonyTSP
{
    class Program
    {
        const int CITIES_NUM = 100;
        const int ANTS_NUM = 10;
        const double ALPHA = 1;
        const double BETA = 5;
        const double RHO = 0.8;
        const double PHEROMONE_QUANTITY = 10;
        const int ITERATION_NUM = 100;
        
        static void Main(string[] args)
        {
            TSP tsp = new TSP(CITIES_NUM);
            tsp.InitializeRandomTSP();
            double antColonyBestTour = tsp.SolveUsingAntColony(ITERATION_NUM, ANTS_NUM, ALPHA, BETA, RHO, PHEROMONE_QUANTITY);
            double bestRandomTour = tsp.SolveUsingBestRandomTour(ITERATION_NUM * ANTS_NUM);
            Console.WriteLine("Best distance using Ant Colony: " + antColonyBestTour);
            Console.WriteLine("Best distance using Random algorithm: " + bestRandomTour);
            Console.ReadLine();
        }
    }
}
