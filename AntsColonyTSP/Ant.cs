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

        public double TotalDistanceTraveled { get => totalDistanceTraveled; set => totalDistanceTraveled = value; }
        public int[] Tour { get => tour; set => tour = value; }

        public Ant(int numOfCities, int index, TSP tsp)
        {
            Tour = new int[numOfCities];
            this.tsp = tsp;
            InitializeTour(numOfCities, index);
        }

        public void InitializeTour(int numOfCities, int index)
        {
            tourIndex = -1;
            allowedCities = Enumerable.Range(0, numOfCities - 1).ToList();
            allowedCities.Remove(index);
            AddCityToTour(index);
            TotalDistanceTraveled = 0;
        }

        public int GetCurrentCityIndex()
        {
            return Tour[tourIndex];
        }

        private void AddCityToTour(int index)
        {
            tourIndex++;
            Tour[tourIndex] = index;
        }

        public void VisitNextCity()
        {
            double[] productOfIntensityAndVisibility = new double[allowedCities.Count];
            double sumOfProducts = 0;
            double pheromoneIntensity, cityVisibility;
            for (int j=0; j<allowedCities.Count; j++)
            {
                pheromoneIntensity = tsp.PheromoneIntensity[Tour[tourIndex], allowedCities[j]];
                cityVisibility = tsp.CitiesVisibility[Tour[tourIndex], allowedCities[j]];
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
                    TotalDistanceTraveled += tsp.Distance(Tour[tourIndex], allowedCities[j]);
                    AddCityToTour(allowedCities[j]);
                    allowedCities.Remove(Tour[tourIndex]);
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
            for (int i = 0; i < Tour.Length - 1; i++)
            {
                UpdatePheromoneIntensityBetweenTwoCities(Tour[i], Tour[i + 1]);
                UpdatePheromoneIntensityBetweenTwoCities(Tour[i + 1], Tour[i]);
            }            
        }

        private void UpdatePheromoneIntensityBetweenTwoCities(int city1, int city2)
        {
            tsp.PheromoneIntensity[city1, city2] += tsp.PheromoneQuantity / TotalDistanceTraveled;
        }
    }
}
