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

        public TSP(int numOfCities)
        {
            cities = new Point[numOfCities];
        }

        public TSP(Point[] cities)
        {
            this.cities = cities;
        }

        public Point GetIthCity(int i)
        {
            return cities[i];
        } 
        public int NumOfCities
        {
            get
            {
                return cities.Length;
            }
        }

        public void InitializeRandomTSP()
        {
            Random rnd = new Random();
            for (int i = 0; i < cities.Length; i++)
            {
                cities[i] = new Point(rnd.NextDouble(), rnd.NextDouble());
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

        public double SolveUsingAntColony(int numOfIterations, int antsNum, double alpha, double beta, double rho, double quantity)
        {
            AntColonyTSP antColonyTsp = new AntColonyTSP(this, antsNum, alpha, beta, rho, quantity);
            return antColonyTsp.Solve(numOfIterations);
        }

        public double SolveUsingBestRandomTour(int numOfTours)
        {
            AntColonyTSP antColonyTSP = new AntColonyTSP(
                tsp: this,
                antsNum: 1,
                alpha: 0,
                beta: 0,
                rho: 0,
                quantity: 0);
            return antColonyTSP.Solve(numOfTours);
        }
    }
}
