using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsColonyTSP
{
    public class Ant
    {
        TSP tsp;
        private List<int> allowedCities;
        int[] tour;
        int tourIndex = -1;
        double totalDistanceTraveled; 

        public Ant(int numOfCities, int index, TSP tsp)
        {
            allowedCities = Enumerable.Range(0, numOfCities - 1).ToList();
            allowedCities.Remove(index);
            tour = new int[numOfCities];
            AddCityToPath(index);
            totalDistanceTraveled = 0;
            this.tsp = tsp;
        }

        private void AddCityToPath(int index)
        {
            tourIndex++;
            tour[tourIndex] = index;
        }

        public void VisitNextCity()
        {
            double[] productOfIntensityAndVisibility = new double[allowedCities.Count];
            double sumOfProducts = 0;
            double pheromoneIntensity, cityVisibility;
            for (int j=0; j<allowedCities.Count; j++)
            {
                pheromoneIntensity = tsp.PheromoneIntensity[tour[tourIndex], allowedCities[j]];
                cityVisibility = tsp.CitiesVisibility[tour[tourIndex], allowedCities[j]];
                productOfIntensityAndVisibility[j] = 
                    RegulateValue(pheromoneIntensity, tsp.Alpha) *
                    RegulateValue(cityVisibility, tsp.Beta);
                sumOfProducts += productOfIntensityAndVisibility[j];
            }

            double probabilitySum = 0;
            Random rnd = new Random();
            double rndNum = rnd.NextDouble();
            for(int j=0; j < allowedCities.Count; j++)
            {
                double probability = productOfIntensityAndVisibility[j] / sumOfProducts;
                if (rndNum >= probabilitySum && rndNum <= probabilitySum + probability)
                {
                    totalDistanceTraveled += tsp.Distance(tour[tourIndex], allowedCities[j]);
                    AddCityToPath(allowedCities[j]);
                    allowedCities.Remove(tour[tourIndex]);
                    break;
                }
                probabilitySum += probability;
            }

        }

        public static double RegulateValue(double value, double regulationParam)
        {
            return Math.Pow(value, regulationParam);
        }

        public void UpdatePheromoneLevels()
        {
            for (int i = 0; i < tour.Length - 1; i++)
            {
                UpdatePheromoneIntensityBetweenTwoCities(tour[i], tour[i + 1]);
                UpdatePheromoneIntensityBetweenTwoCities(tour[i + 1], tour[i]);
            }            
        }

        private void UpdatePheromoneIntensityBetweenTwoCities(int city1, int city2)
        {
            tsp.PheromoneIntensity[city1, city2] += tsp.PheromoneQuantity / totalDistanceTraveled;
        }
    }
}
