using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntsColonyTSP
{
    public class Ant
    {
        AntColonyTSP antColony;
        private List<int> allowedCities;
        int[] tour;
        int tourIndex = -1;
        double totalDistanceTraveled;

        public double TotalDistanceTraveled { get => totalDistanceTraveled; set => totalDistanceTraveled = value; }
        public int[] Tour { get => tour; set => tour = value; }

        public Ant(AntColonyTSP antColony, int initialCity)
        {
            Tour = new int[antColony.Tsp.NumOfCities + 1];
            this.antColony = antColony;
            InitializeTour(antColony.Tsp.NumOfCities, initialCity);
        }

        public void InitializeTour(int numOfCities, int initialiCity)
        {
            tourIndex = -1;
            TotalDistanceTraveled = 0;
            allowedCities = Enumerable.Range(0, numOfCities - 1).ToList();
            AddCityToTour(initialiCity);
        }

        public int GetCurrentCityIndex()
        {
            return Tour[tourIndex];
        }

        private void AddCityToTour(int index)
        {
            if (tourIndex>=0)
                TotalDistanceTraveled += antColony.Tsp.Distance(GetCurrentCityIndex(), index);
            Tour[++tourIndex] = index;
            allowedCities.Remove(index);
        }

        public void VisitNextCity()
        {
            // if there are no more allowed cities, return to the first city
            if (allowedCities.Count == 0)
                AddCityToTour(Tour[0]);
            
            // else proceed to the next city by probability
            else
            {
                double[] productOfIntensityAndVisibility = new double[allowedCities.Count];
                double sumOfProducts = 0;
                double pheromoneIntensity, cityVisibility;
                for (int j = 0; j < allowedCities.Count; j++)
                {
                    pheromoneIntensity = antColony.PheromoneIntensity[GetCurrentCityIndex(), allowedCities[j]];
                    cityVisibility = antColony.CitiesVisibility[GetCurrentCityIndex(), allowedCities[j]];

                    productOfIntensityAndVisibility[j] =
                        RegulateValue(pheromoneIntensity, antColony.Alpha) *
                        RegulateValue(cityVisibility, antColony.Beta);

                    sumOfProducts += productOfIntensityAndVisibility[j];
                }

                double probabilitySum = 0;
                Random rnd = new Random();
                double rndNum = rnd.NextDouble();
                for (int j = 0; j < allowedCities.Count; j++)
                {
                    double probability = productOfIntensityAndVisibility[j] / sumOfProducts;
                    if (rndNum >= probabilitySum && rndNum <= probabilitySum + probability)
                    {
                        AddCityToTour(allowedCities[j]);
                        break;
                    }
                    probabilitySum += probability;
                }
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
            antColony.PheromoneIntensity[city1, city2] += antColony.PheromoneQuantity / TotalDistanceTraveled;
        }
    }
}
