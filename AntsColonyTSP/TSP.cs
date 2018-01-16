using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsColonyTSP
{
    public class TSP
    {
        private Point[] cities;
        private Ant[] ants;
        private double alpha;
        private double beta;
        private double rho;
        private double pheromoneQuantity;
        private double[,] pheromoneIntensity;
        private double[,] citiesVisibility;
        private int[] bestTour;
        private double bestTourDistance = double.MaxValue;

        public double[,] PheromoneIntensity { get => pheromoneIntensity; set => pheromoneIntensity = value; }
        public double[,] CitiesVisibility { get => citiesVisibility; set => citiesVisibility = value; }
        public double Alpha { get => alpha; set => alpha = value; }
        public double Beta { get => beta; set => beta = value; }
        public double PheromoneQuantity { get => pheromoneQuantity; set => pheromoneQuantity = value; }
        public Ant[] Ants { get => ants; set => ants = value; }
        public double BestTourDistance { get => bestTourDistance; set => bestTourDistance = value; }

        public TSP(int citiesNum, int antsNum, double alpha, double beta, double rho, double quantity)
        {
            cities = new Point[citiesNum];
            Ants = new Ant[antsNum];
            this.alpha = alpha;
            this.beta = beta;
            this.rho = rho;
            bestTour = new int[citiesNum + 1];
            PheromoneQuantity = quantity;
            PheromoneIntensity = new double[citiesNum, citiesNum];
            CitiesVisibility = new double[citiesNum, citiesNum];
            initializeRandomTSP();
        }

        private void initializeRandomTSP()
        {
            Random rnd = new Random();
            for (int i = 0; i < cities.Length; i++)
            {
                cities[i] = new Point(rnd.NextDouble(), rnd.NextDouble());
            }

            for (int i = 0; i < Ants.Length; i++)
            {
                Ants[i] = new Ant(cities.Length, rnd.Next(cities.Length), this);
            }

            for (int i=0; i< cities.Length; i++)
            {
                for (int j=0; j< cities.Length; j++)
                {
                    CitiesVisibility[i, j] = 1 / Distance(cities[i], cities[j]);
                    PheromoneIntensity[i, j] = 1 / Math.Pow(cities.Length, 2);
                }
            }
        }

        public static double Distance(Point firstPoint, Point secondPoint)
        {
            return Math.Sqrt(Math.Pow(firstPoint.X - secondPoint.X, 2) + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
        }

        public double Distance(int cityIndex1, int cityIndex2)
        {
            return Distance(cities[cityIndex1], cities[cityIndex2]);
        }

        public void UpdatePheromoneTrail()
        {
            for(int i=0; i<cities.Length; i++)
            {
                for(int j=0; j<cities.Length; j++)
                {
                    pheromoneIntensity[i, j] *= rho;
                }
            }

            for(int i=0; i < Ants.Length; i++)
            {
                Ants[i].UpdatePheromoneLevels();
            }
        }

        public void InitializeAntsTour()
        {
            for(int i=0; i<ants.Length; i++)
            {
                ants[i].InitializeTour(cities.Length, ants[i].GetCurrentCityIndex());
            }
        }

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
    }
}
