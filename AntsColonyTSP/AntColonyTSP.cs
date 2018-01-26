using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsColonyTSP
{
    public class AntColonyTSP
    {
        private TSP tsp;
        private Ant[] ants;
        private double alpha;
        private double beta;
        private double rho;
        private double pheromoneQuantity;
        private double[,] pheromoneIntensity;
        private double[,] citiesVisibility;
        private int[] bestTour;
        private double bestTourDistance = double.MaxValue;

        public double[,] PheromoneIntensity { get => pheromoneIntensity; }
        public double[,] CitiesVisibility { get => citiesVisibility; }
        public double Alpha { get => alpha; }
        public double Beta { get => beta; }
        public double PheromoneQuantity { get => pheromoneQuantity; set => pheromoneQuantity = value; }
        public Ant[] Ants { get => ants;  }
        public double BestTourDistance { get => bestTourDistance; set => bestTourDistance = value; }
        public TSP Tsp { get => tsp; }

        /// <summary>
        ///  Initializes a new AntColony for a given TSP.
        /// </summary>
        /// <param name="tsp">The TSP associated with the colony</param>
        /// <param name="antsNum">The number of ants in the colony</param>
        /// <param name="alpha">The parameter to regulate the influence of the pheromone intensity</param>
        /// <param name="beta">The parameter to regulate the influence of the cities visibility</param>
        /// <param name="rho">The vaporization level of pheromones</param>
        /// <param name="quantity">The quantity of pheromones of each ant</param>
        public AntColonyTSP(TSP tsp, int antsNum, double alpha, double beta, double rho, double quantity)
        {
            this.tsp = tsp;
            this.ants = new Ant[antsNum];
            this.alpha = alpha;
            this.beta = beta;
            this.rho = rho;
            this.bestTour = new int[tsp.NumOfCities + 1];
            this.PheromoneQuantity = quantity;
            this.pheromoneIntensity = new double[tsp.NumOfCities, tsp.NumOfCities];
            this.citiesVisibility  = new double[tsp.NumOfCities, tsp.NumOfCities];
            InitializeAntColony();
        }

        public AntColonyTSP()
        {
        }

        private void InitializeAntColony()
        {
            Random rnd = new Random();

            for (int i = 0; i < Ants.Length; i++)
            {
                Ants[i] = new Ant(this, rnd.Next(tsp.NumOfCities));
            }

            for (int i=0; i< tsp.NumOfCities; i++)
            {
                for (int j=0; j< tsp.NumOfCities; j++)
                {
                    CitiesVisibility[i, j] = 1 / tsp.Distance(i,j);
                    PheromoneIntensity[i, j] = 1 / Math.Pow(tsp.NumOfCities, 2);
                }
            }
        }


        /// <summary>
        /// Updates the intensity of pheromones between all pairs of cities.
        /// This procedure should be called after all the ants completed a tour.
        /// </summary>
        public void UpdatePheromoneTrail()
        {
            for(int i=0; i<tsp.NumOfCities; i++)
            {
                for(int j=0; j<tsp.NumOfCities; j++)
                {
                    pheromoneIntensity[i, j] *= rho;
                }
            }

            for(int i=0; i < Ants.Length; i++)
            {
                Ants[i].UpdatePheromoneLevels();
            }
        }

        /// <summary>
        /// Initializes each ant's tour to contain only one initial city city.
        /// This procedure should be called before every iteration.
        /// </summary>
        public void InitializeAntsTour()
        {
            for(int i=0; i<ants.Length; i++)
            {
                ants[i].InitializeTour(tsp.NumOfCities, ants[i].GetCurrentCityIndex());
            }
        }

        /// <summary>
        /// Iterates over all the ants to find the ant whose tour is the shortest.
        /// </summary>
        /// <returns>The length of the shortest tour</returns>
        public double GetBestTourDistance()
        {
            foreach(Ant ant in ants)
            {
                if(ant.TotalDistanceTraveled < BestTourDistance)
                {
                    BestTourDistance = ant.TotalDistanceTraveled;
                    ant.Tour.CopyTo(bestTour,0);
                }
            }
            return BestTourDistance;
        }

        public double Solve (int NumOfIterations)
        {
            double bestTour = 0;

            for (int t = 0; t < NumOfIterations; t++)
            {
                for (int k = 0; k < Ants.Length; k++)
                {
                    Ants[k].DoATour();
                }
                bestTour = GetBestTourDistance();
                UpdatePheromoneTrail();
                InitializeAntsTour();
            }
            BestTourDistance = double.MaxValue;
            return bestTour;
        }
    }
}
