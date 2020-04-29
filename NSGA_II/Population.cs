using System;
using System.Collections.Generic;
using System.Linq;


namespace NSGA_II
{
    public class Population
    {
        Random random = new Random();

        int populationSize;
        public List<Individual> population;
        public List<Individual> offspring;           // Separate population from new population
        List<List<Individual>> fronts;
        List<Individual> nonDominatedFront = new List<Individual>();

        public Population(int size)
        {
            populationSize = size;
            population = new List<Individual>();
            offspring = new List<Individual>();

            for (int i = 0; i < populationSize; i++)
            {
                population[i] = new Individual();
                population[i].Evaluate();
            }

            fronts = new List<List<Individual>>();
            //SortPopulation();
        }

        public void Evolve()
        {
            Individual a, b, x;
            offspring.Clear();

            for (int i = 0; i < population.Count; i++)
            {
                a = selectIndividual();
                b = selectIndividual();
                x = Breed(a, b);
                x.Evaluate();

                offspring.Add(x);       //NOT REPLACE; MERGE
            }

            //population.AddRange(offspring);
            fastNonDominatedSort();
        }

        public void fastNonDominatedSort()
        {
            for (int i = 0; i < population.Count; i++)
            {
                Individual p = population[i];

                p.dominated.Clear();
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

        

        private Individual selectIndividual()
        {
            int chosen = (int)Math.Floor(((double)population.Count - 1e-3) * (1.0 - Math.Pow((random.NextDouble()), 2)));
            return population[chosen];
        }

        private Individual Breed(Individual a, Individual b)
        {
            Individual c = new Individual();
            c.genotype = Crossover(a.genotype, b.genotype);
            c.genotype.Mutate();
            return c;
        }


        private Genotype Crossover(Genotype a, Genotype b)
        {
            double probabilityCrossover = 0.98; 
            if (random.NextDouble() > probabilityCrossover) 
                return random.NextDouble() < 0.5 ? a : b;

            Genotype c = new Genotype();
            string newGene = "";

            for (int i = 0; i < c.gene.Length; i++)
                newGene += random.NextDouble() < 0.5 ? a.gene[i] : b.gene[i];

            c.gene = newGene;
            return c;
        }
    }
}
