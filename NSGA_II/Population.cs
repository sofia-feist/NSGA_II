using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Linq;


namespace NSGA_II
{
    internal class Population
    {

        static Random random = new Random();

        int populationSize;
        public List<Individual> population;
        public List<List<Individual>> fronts;
        public List<Individual> nonDominatedFront = new List<Individual>();



        public Population(int size)
        {
            populationSize = size;

            nonDominatedFront = new List<Individual>();
            fronts = new List<List<Individual>>();

            population = new List<Individual>();

            for (int i = 0; i < populationSize; i++)
            {
                Individual individual = new Individual();
                individual.Evaluate();
                population.Add(individual);
            }
        }



        // Offspring: Produce offspring
        public List<Individual> Offspring()
        {
            Individual a, b, c;
            List<Individual> offspring = new List<Individual>(populationSize);

            for (int i = 0; i < populationSize; i++)
            {
                a = SelectParent();
                b = SelectParent();
                c = Breed(a, b);
                c.fitnesses.Clear();    // DELETE LATER
                c.Evaluate();

                offspring.Add(c);
            }

            return offspring;
        }
        

        private Individual SelectParent()
        {
            int chosen = (int)Math.Floor(((double)population.Count - 1e-3) * Math.Pow((random.NextDouble()), 2));
            return population[chosen];
        }

        private Individual Breed(Individual a, Individual b)
        {
            Individual child = Crossover(a, b);
            child.Mutate();
            return child;
        }


        private Individual Crossover(Individual a, Individual b)
        {
            if (random.NextDouble() > NSGAII_Algorithm.probabilityCrossover)
                return random.NextDouble() < 0.5 ? a : b;

            Individual c = new Individual();
            for (int i = 0; i < c.genes.Count; i++)
            {
                if (random.NextDouble() < 0.5)
                    c.genes[i] = a.genes[i];
                else
                    c.genes[i] = b.genes[i];
            }

            return c;
        }


        // FastNonDominatedSort: Sorts population
        public void FastNonDominatedSort()
        {
            nonDominatedFront.Clear();
            fronts.Clear();

            for (int i = 0; i < population.Count; i++)
            {
                Individual p = population[i];

                p.dominated = new List<Individual>();
                p.dominationCount = 0;

                for (int j = 0; j < population.Count; j++)
                {
                    if (i == j) continue;
                    Individual q = population[j];

                    if (p.Dominates(q))
                        p.dominated.Add(q);
                    else if (q.Dominates(p))
                        p.dominationCount++;
                }

                if (p.dominationCount == 0)
                {
                    p.rank = 0;
                    nonDominatedFront.Add(p);
                }
            }

            fronts.Add(nonDominatedFront);
            int currentFront = 0;

            while (population.Any(p => p.dominationCount > 0))    //currentFront < fronts.Count)
            {
                List<Individual> nextFront = new List<Individual>();

                foreach (var p in fronts[currentFront])
                {
                    foreach (var q in p.dominated)
                    {
                        q.dominationCount--;

                        if (q.dominationCount == 0)
                        {
                            q.rank = currentFront + 1;
                            nextFront.Add(q);
                        }
                    }
                }

                currentFront++;
                if (nextFront.Count != 0)
                    fronts.Add(nextFront);

                if (currentFront == fronts.Count) currentFront = 0;  //Restart
            }

            //if (fronts.SelectMany(i => i).Count() < populationSize)
            //    throw new InvalidOperationException("Population bug; FIX THIS.");

            population = fronts.SelectMany(i => i).ToList();
        }


        // CrowdingDistance: Calculates Crowding Distance for each individual of a given front
        public void CrowdingDistance(List<Individual> front)
        {
            int size = front.Count;
            int numObjectives = front[0].fitnesses.Count;    // Refactor this?
            front.ForEach(p => p.crowdingDistance = 0);

            for (int m = 0; m < numObjectives; m++)
            {
                front = front.OrderBy(x => x.fitnesses[m]).ToList();  

                front[0].crowdingDistance = double.PositiveInfinity;
                front[size - 1].crowdingDistance = double.PositiveInfinity;

                for (int i = 1; i < size - 1; i++)
                {
                    double max = front.Max(p => p.fitnesses[m]);     
                    double min = front.Min(p => p.fitnesses[m]);
                    front[i].crowdingDistance += (front[i + 1].fitnesses[m] - front[i - 1].fitnesses[m]) / (max - min);
                }
            }
        }

        
    }
}
