using System;
using System.Collections.Generic;
using System.Linq;

namespace NSGA_II
{
    public class Individual
    {
         public Genotype genotype;

        public List<double> fitnesses;

        public int rank;
        public double crowdingDistance;
        public int dominationCount;
        public List<Individual> dominated;

        public Individual()
        {
            genotype = new Genotype();
            fitnesses = new List<double>();

            dominationCount = 0;
            dominated = new List<Individual>();
        }


        // Evaluate: Evaluates an individual's fitness
        public void Evaluate() //  UPDATE PROPERLY (multiple objective, multiple parameter types)   ///////////////
        {
            double x = genotype.Decode(genotype.gene) * 10.0;  // REMAP to domain
            double fitness = Math.Pow(-x, 2.0);  

            fitnesses.Add(fitness);
        }


        // Dominate: Checks if individual dominates over a given other
        public bool Dominates(Individual other, bool Minimize = true)  // Here?
        {
            for (int i = 0; i < fitnesses.Count; i++)
            {
                if (Minimize)
                {
                    if (fitnesses[i] > other.fitnesses[i]) return false;
                }
                else
                {
                    if (fitnesses[i] < other.fitnesses[i]) return false;
                }
            }

            return true;
        }
    }
}
