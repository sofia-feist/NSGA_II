using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

namespace NSGA_II
{
    internal class Individual
    {
        private static Random random = new Random();
        private static GH_Component GHComponent;

        public List<double> genes;
        public List<double> fitnesses;

        public int rank;
        public double crowdingDistance;
        public int dominationCount;
        public List<Individual> dominated;
        


        public Individual()
        {
            GHComponent = GH_ParameterHandler.gh;

            genes = new List<double>(); //GH_ParameterHandler.GetGeneValues(); //new List<double>();

            for (int i = 0; i < 2; i++)
            {
                genes.Add(Math.Round(random.NextDouble(), 2));
            }

            fitnesses = new List<double>();
        }




        // Mutate: Gives genes a small probability of mutation
        public void Mutate()
        {
            for (int i = 0; i < genes.Count; i++)
            {
                if (random.NextDouble() < NSGAII_Algorithm.probabilityMutation)
                    genes[i] = random.NextDouble();   //Set in Slider !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
        }


        // Evaluate: Evaluates an individual's fitness
        public void Evaluate() //  UPDATE TO RECEIVE OUTSIDE FITNESSES (performance analyses)   ///////////////
        {
            double fitness1 = Math.Pow(genes[0], 2.0);
            double fitness2 = Math.Pow(genes[1] - 2.0, 2.0);

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
