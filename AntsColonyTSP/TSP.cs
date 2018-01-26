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

        /// <summary>
        /// Calculates the distance between two given points.
        /// </summary>
        /// <returns>the distance between The two points</returns>
        public static double Distance(Point firstPoint, Point secondPoint)
        {
            return Math.Sqrt(Math.Pow(firstPoint.X - secondPoint.X, 2) + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
        }

        /// <summary>
        /// Calculates the distance between two cities in the TSP.
        /// </summary>
        /// <param name="cityIndex1">The index of the first city</param>
        /// <param name="cityIndex2">The index of the second city</param>
        /// <returns>The distance between the two cities</returns>
        public double Distance(int cityIndex1, int cityIndex2)
        {
            return Distance(cities[cityIndex1], cities[cityIndex2]);
        }

        /// <summary>
        /// Solves the current TSP using the Ant Colony Algorithm.
        /// </summary>
        /// <param name="numOfIterations">The number of iterations to perform</param>
        /// <param name="antsNum">The number of ants in the colony</param>
        /// <param name="alpha">The parameter to regulate the influence of the pheromone intensity</param>
        /// <param name="beta">The parameter to regulate the influence of the cities visibility</param>
        /// <param name="rho">The vaporization level of pheromones</param>
        /// <param name="quantity">The quantity of pheromones of each ant</param>
        /// <returns>The length of the shortest tour found using the ant colony algorithm.</returns>
        public double SolveUsingAntColony(int numOfIterations, int antsNum, double alpha, double beta, double rho, double quantity)
        {
            AntColonyTSP antColonyTsp = new AntColonyTSP(this, antsNum, alpha, beta, rho, quantity);
            return antColonyTsp.Solve(numOfIterations);
        }

        /// <summary>
        ///  Solves the current TSP by choosing the best tour out of many random tours.
        /// </summary>
        /// <param name="numOfTours">The number of random tours from which to choose the shortest tour.</param>
        /// <returns>The length of the shortest of the random tours.</returns>
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
