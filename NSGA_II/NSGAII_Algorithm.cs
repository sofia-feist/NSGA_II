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
    // Pseudocode at: http://www.cleveralgorithms.com/nature-inspired/evolution/nsga.html
    public class NSGAII_Algorithm    
    {
        public NSGAII_Algorithm(int PopulationSize) 
        {
            Population population = new Population(PopulationSize);
            population.fastNonDominatedSort();
            population.Evolve();
        }


        public void crowdingDistance(int numObjectives, List<Individual> front)
        {
            int size = front.Count;
            foreach (var p in front) p.crowdingDistance = 0;

            for (int m = 0; m < numObjectives; m++)
            {
                front = front.OrderBy(x => x.fitnesses[m]).ToList();    // Ascending or Descending?

                front[0].crowdingDistance = double.PositiveInfinity;
                front[size - 1].crowdingDistance = double.PositiveInfinity;

                for (int i = 1; i < size - 1; i++)
                {
                    double max = front.Max(p => p.fitnesses[m]);
                    double min = front.Min(p => p.fitnesses[m]);
                    front[i].crowdingDistance += (front[i+1].fitnesses[m] - front[i-1].fitnesses[m]) / (max - min);
                }
            }
        }

    }
}
