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
        const int ITERATION_NUM = 1000;
        
        static void Main(string[] args)
        {

            TSP tsp = new TSP(CITIES_NUM, ANTS_NUM, ALPHA, BETA, RHO, PHEROMONE_QUANTITY);
            double antColonyBestTour = Solve(tsp);
            tsp.Alpha = 0;
            tsp.Beta = 0;
            double bestRandomTour = Solve(tsp);
            Console.WriteLine("Best distance using Ant Colony: " + antColonyBestTour);
            Console.WriteLine("Best distance using Random algorithm: " + bestRandomTour);
            Console.ReadLine();
        }

        public static double Solve(TSP tsp)
        {
            double bestTour = 0;
            for (int t = 0; t < ITERATION_NUM; t++)
            {
                for (int k = 0; k < ANTS_NUM; k++)
                {
                    for (int i = 0; i < CITIES_NUM; i++)
                    {
                        tsp.Ants[k].VisitNextCity();
                    }
                }
                bestTour = tsp.GetBestTourDistance();
                tsp.UpdatePheromoneTrail();
                tsp.InitializeAntsTour();
            }
            tsp.BestTourDistance = double.MaxValue;
            return bestTour;
        }
    }
}
