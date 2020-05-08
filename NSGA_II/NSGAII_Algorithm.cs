using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Grasshopper.Kernel;
using System.Windows.Forms.DataVisualization.Charting;

namespace NSGA_II
{
    public class NSGAII_Algorithm    
    {
        GH_Component GHComponent;
        NSGAII_Visualizer visualizer;

        public static int PopulationSize = 100;

        public static int currentGeneration = 0;
        public static int MaxGenerations = 50;

        public Stopwatch stopWatch;
        public static double MaxDuration;   // in Minutes

        public static bool Minimize = true;
        
        public static double probabilityCrossover = 0.98;

        public List<Individual>  population;



        public NSGAII_Algorithm(NSGAII_Visualizer _visualizer, GH_Component _GHComponent)
        {
            visualizer = _visualizer;
            GHComponent = _GHComponent;

            stopWatch = new Stopwatch();
        }



        // Stop Condition: Can be either a specified number of generations, a specified duration, or both (whichever comes first)
        bool StopCondition()
        {
            if (NSGAII_Editor.GenerationsChecked == true)
            {
                if (currentGeneration > MaxGenerations) return false;
            }
            if (NSGAII_Editor.TimeChecked == true)
            {
                if (stopWatch.Elapsed.TotalMinutes > MaxDuration) return false;
            } 
            
            return true;
        }





        ////////////////////////////////////////////////////////////////////////
        ////////////////////////    NSGA-II Algorithm   ////////////////////////
        ////////////////////////////////////////////////////////////////////////

        // REFERENCES:
        // Deb, K., Pratap, A., Agarwal, S., & Meyarivan, T. A. M. T. (2002). 
        // A fast and elitist multiobjective genetic algorithm: NSGA-II.
        // IEEE transactions on evolutionary computation, 6(2), pp. 182-197.
        // &
        // http://www.cleveralgorithms.com/nature-inspired/evolution/nsga.html
        public void NSGAII() 
        {
            if (PopulationSize < 2)
                throw new InvalidOperationException("Population must have at least 2 individuals.");

            stopWatch.Start();

            Population pop = new Population(GHComponent, PopulationSize);

            pop.FastNonDominatedSort();
            var offspring = pop.Offspring();

            while (StopCondition())
            {
                pop.population.AddRange(offspring);
                pop.FastNonDominatedSort();

                if (pop.population.Count < PopulationSize)
                    throw new InvalidOperationException("Population prob.");      // FIX !!!!!!!!!!!!!!!!

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

                currentGeneration++;


                // GRAPHIC UPDATE
                visualizer.CurrentGeneration.Text = "Current Generation: " + (currentGeneration - 1);
                pop.population.ForEach(i => visualizer.ParetoChart.Series[1].Points.AddXY(i.fitnesses[0], i.fitnesses[1]));

                visualizer.ParetoChart.Series[0].Points.Clear();
                pop.fronts[0].ForEach(i => visualizer.ParetoChart.Series[0].Points.AddXY(i.fitnesses[0], i.fitnesses[1]));
            }

            population = pop.population;
            stopWatch.Stop();

            visualizer.TimeElapsed.Text = "Time Elapsed: " + stopWatch.Elapsed.TotalSeconds;
        }

    }
}
