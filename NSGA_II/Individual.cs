using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

namespace NSGA_II
{
    internal class Individual
    {
        private static Random random = new Random();
        private static GH_ParameterHandler ghHandler;

        public List<double> genes;
        public List<double> fitnesses;

        public int rank;
        public double crowdingDistance;
        public int dominationCount;
        public List<Individual> dominated;
        


        public Individual(GH_ParameterHandler _ghHandler)
        {
            ghHandler = _ghHandler;
            genes = ghHandler.GetSetGeneValues();
        }




        // Mutate: Gives genes a small probability of mutation
        public void Mutate()
        {
            for (int i = 0; i < genes.Count; i++)
            {
                if (random.NextDouble() < NSGAII_Algorithm.probabilityMutation)
                    genes[i] = ghHandler.MutateGeneValue(i); 
            }
        }


        // Dominates: Checks if individual dominates over a given other
        public bool Dominates(Individual other)
        {
            for (int i = 0; i < fitnesses.Count; i++)
            {
                if (NSGAII_Algorithm.ObjectiveList[i] == Objectives.Minimize) 
                {
                    if (fitnesses[i] >= other.fitnesses[i]) { return false; }
                }
                else if (NSGAII_Algorithm.ObjectiveList[i] == Objectives.Maximize)
                {
                    if (fitnesses[i] <= other.fitnesses[i]) { return false; }
                }
            }

            return true;
        }

    }
}
