using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Grasshopper.Kernel;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace NSGA_II
{
    public class NSGAII_Algorithm : UserControl
    {
        GH_Component GHComponent;
        NSGAII_Visualizer visualizer;

        public static int PopulationSize = 100;

        public static int currentGeneration = 0;
        public static int MaxGenerations = 50;

        public static Stopwatch stopWatch = new Stopwatch();
        public static double MaxDuration = 5400;   // In Seconds (1h30)

        public static bool Minimize = true;
        
        public static double probabilityCrossover = 0.98;

        public List<Individual>  population;



        public NSGAII_Algorithm(NSGAII_Visualizer _visualizer, GH_Component _GHComponent)
        {
            visualizer = _visualizer;
            GHComponent = _GHComponent;

            InitializeOptimization();
        }



        // InitializeOptimization: Initializes statistics and parameters for the optimization (also works for the Reset)
        public void InitializeOptimization()
        {
            currentGeneration = 0;
            stopWatch = new Stopwatch();

            visualizer.CurrentGeneration.Text = "Current Generation: " + currentGeneration;
            visualizer.TimeElapsed.Text = "Time Elapsed: " + stopWatch.Elapsed.TotalSeconds;

            for (int i = 0; i < visualizer.ParetoChart.Series.Count; i++)
                visualizer.ParetoChart.Series[i].Points.Clear();
        }


        // Stop Condition: Can be either a specified number of generations, a specified duration, or both (whichever comes first)
        bool StopCondition()
        {
            if (NSGAII_Editor.GenerationsChecked)
            {
                if (currentGeneration > MaxGenerations) return false;
            }
            if (NSGAII_Editor.TimeChecked)
            {
                if (stopWatch.Elapsed.TotalSeconds > MaxDuration) return false;
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
            //BackgroundWorker bw = new BackgroundWorker();
            //bw.DoWork += new DoWorkEventHandler(background_UpdateChart);

            stopWatch.Start();

            Population pop = new Population(GHComponent, PopulationSize);

            pop.FastNonDominatedSort();
            var offspring = pop.Offspring();

            while (StopCondition())
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

                visualizer.CurrentGeneration.Text = "Current Generation: " + currentGeneration;
                currentGeneration++;


                //bw.RunWorkerAsync();
                // GRAPHIC UPDATE
                visualizer.TimeElapsed.Text = "Time Elapsed: " + TimeSpan.FromSeconds(stopWatch.Elapsed.TotalSeconds).ToString(@"hh\:mm\:ss\.fff");

                pop.population.ForEach(i => visualizer.ParetoChart.Series[0].Points.AddXY(i.fitnesses[0], i.fitnesses[1]));

                visualizer.ParetoChart.Series[1].Points.Clear();
                pop.fronts[0].ForEach(i => visualizer.ParetoChart.Series[1].Points.AddXY(i.fitnesses[0], i.fitnesses[1]));
            }

            population = pop.population;

            stopWatch.Stop();
            visualizer.TimeElapsed.Text = "Time Elapsed: " + TimeSpan.FromSeconds(stopWatch.Elapsed.TotalSeconds).ToString(@"hh\:mm\:ss\.fff");
        }





        // ???????????????
        private void background_UpdateChart(object sender, DoWorkEventArgs e)
        {
            //BackgroundWorker worker = sender as BackgroundWorker;

            //visualizer.TimeElapsed.Text = "Time Elapsed: " + TimeSpan.FromSeconds(stopWatch.Elapsed.TotalSeconds).ToString(@"hh\:mm\:ss\.fff");

            //pop.population.ForEach(i => visualizer.ParetoChart.Series[0].Points.AddXY(i.fitnesses[0], i.fitnesses[1]));

            //visualizer.ParetoChart.Series[1].Points.Clear();
            //pop.fronts[0].ForEach(i => visualizer.ParetoChart.Series[1].Points.AddXY(i.fitnesses[0], i.fitnesses[1]));
    
        }

    }
}
