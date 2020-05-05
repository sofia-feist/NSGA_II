using System;
using System.Collections.Generic;

namespace NSGA_II
{
    public class Individual
    {
        public Genotype genotype;

        public int rank;
        public double crowdingDistance;
        public int dominationCount;
        public List<Individual> dominated;

        public List<double> fitnesses;


        public Individual()
        {
            genotype = new Genotype();
            fitnesses = new List<double>();
        }


        // Evaluate: Evaluates an individual's fitness
        public void Evaluate() //  UPDATE TO RECEIVE OUTSIDE FITNESSES (performance analyses)   ///////////////
        {
            double x = genotype.Decode() * (1000 + 1000) - 1000; 
            double fitness1 = Math.Pow(x, 2.0);
            double fitness2 = Math.Pow(x - 2.0, 2.0);

            fitnesses.Add(fitness1);
            fitnesses.Add(fitness2);
        }


        // Dominates: Checks if individual dominates over a given other
        public bool Dominates(Individual other)
        {
            if (NSGAII_Algorithm.Minimize)
            {
                for (int i = 0; i < fitnesses.Count; i++)
                    if (fitnesses[i] >= other.fitnesses[i]) { return false; } 
            }
            else
            { 
                for (int i = 0; i < fitnesses.Count; i++)
                    if (fitnesses[i] <= other.fitnesses[i]) { return false; }
            }

            return true;
        }

    }
}
