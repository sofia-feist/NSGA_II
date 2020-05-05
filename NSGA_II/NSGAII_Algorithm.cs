using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSGA_II
{

    // NSGA-II REFERENCES:
    // Deb, K., Pratap, A., Agarwal, S., & Meyarivan, T. A. M. T. (2002). 
    // A fast and elitist multiobjective genetic algorithm: NSGA-II.
    // IEEE transactions on evolutionary computation, 6(2), pp. 182-197.
    // &
    // http://www.cleveralgorithms.com/nature-inspired/evolution/nsga.html
    public class NSGAII_Algorithm    
    {
        public static bool Minimize = true;
        public static double probabilityCrossover = 0.98;

        public List<Individual>  population;

        public NSGAII_Algorithm(int PopulationSize, int nGenerations) 
        {
            int iteration = 0;
            Population pop = new Population(PopulationSize);

            pop.FastNonDominatedSort();
            var offspring = pop.Offspring();

            while (iteration < nGenerations)
            {
                pop.population.AddRange(offspring);
                pop.FastNonDominatedSort();

                var newGeneration = new List<Individual>();

                foreach (var front in pop.fronts)
                {
                    pop.CrowdingDistance(front);

                    if (newGeneration.Count + front.Count > PopulationSize)
                    {
                        var orderedFront = front.OrderBy(p => p.rank).ThenByDescending(p => p.crowdingDistance).ToList();
                        for (int j = newGeneration.Count; j < PopulationSize; j++)
                            newGeneration.Add(orderedFront[j - newGeneration.Count]);
                        break;
                    }
                    else
                    {
                        newGeneration.AddRange(front);
                    }
                }

                pop.population = newGeneration;
                offspring = pop.Offspring();

                iteration++;
            }

            population = pop.population;
        }

        

    }
}
