using System;
using System.Linq;
using System.Collections.Generic;



namespace NSGA_II
{
    internal class Population
    {
        private static GH_ParameterHandler ghHandler;
        static Random random = new Random();

        private int populationSize;
        public List<Individual> population;
        public List<List<Individual>> fronts;
        public List<Individual> nonDominatedFront = new List<Individual>();



        public Population(int size, GH_ParameterHandler _ghHandler)
        {
            populationSize = size;
            ghHandler = _ghHandler;

            nonDominatedFront = new List<Individual>();
            fronts = new List<List<Individual>>();

            population = new List<Individual>();

            for (int i = 0; i < populationSize; i++)
            {
                Individual individual = new Individual(ghHandler);
                individual.fitnesses = ghHandler.GetFitnessValues();
                population.Add(individual);
            }
        }



        // Offspring: Produces the entire new generation of offspring 
        public List<Individual> Offspring()
        {
            Individual parent1, parent2, child;
            List<Individual> offspring = new List<Individual>(populationSize);

            for (int i = 0; i < populationSize; i++)
            {
                parent1 = SelectParent();
                parent2 = SelectParent();
                child = Breed(parent1, parent2);
                child.fitnesses = ghHandler.GetFitnessValues();

                offspring.Add(child);
            }

            return offspring;
        }


        // SelectParent: Selects an individual from the population to reproduce, while giving priority to individuals in the beginning (= better ranked) of the list
        private Individual SelectParent()
        {
            int chosen = (int)Math.Floor(((double)population.Count - 1e-3) * Math.Pow((random.NextDouble()), 2));
            return population[chosen];
        }


        // Breed: Produces a new child by process of Crossover and Mutation
        private Individual Breed(Individual parent1, Individual parent2)
        {
            Individual child = Crossover(parent1, parent2);
            ghHandler.SetSliders(child.genes);
            child.Mutate();
            return child;
        }


        // Crossover: Combines the genes of two parents to generate new offspring
        private Individual Crossover(Individual parent1, Individual parent2)
        {
            if (random.NextDouble() > NSGAII_Algorithm.probabilityCrossover)
                return random.NextDouble() < 0.5 ? parent1 : parent2;

            Individual child = new Individual(ghHandler);
            for (int i = 0; i < child.genes.Count; i++)
            {
                if (random.NextDouble() < 0.5)
                    child.genes[i] = parent1.genes[i];
                else
                    child.genes[i] = parent2.genes[i];
            }

            return child;
        }


        // FastNonDominatedSort: Sorts the population by Pareto Dominance and subdivides it in different fronts
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

            while (currentFront < fronts.Count) 
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
            }

            population = fronts.SelectMany(i => i).ToList();
        }


        // CrowdingDistance: Calculates Crowding Distance for each individual of a given front
        public void CrowdingDistance(List<Individual> front)
        {
            int size = front.Count;
            int numObjectives = GH_ParameterHandler.gh.Params.Input[1].Sources.Count; 
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
