using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;


namespace NSGA_II
{
    public class NSGAII_Visualizer
    {
        private NSGAII_Editor Editor;
        private GH_Component gh;


        public Chart ParetoChart;

        public Label Population;
        public Label CurrentGeneration;
        public Label TimeElapsed;
        public Label nGenes;
        public Label nObjectives;

        public TextBox solutionInfo;



        public NSGAII_Visualizer(NSGAII_Editor editor, GH_Component _ghComponent)
        {
            Editor = editor;
            gh = _ghComponent;

            InitializeParetoChart();
            InitializeStatistics();
            InitializeTexbox();

        }



        public void InitializeParetoChart()
        {
            // CHART
            ParetoChart = new Chart
            {
                Name = "Pareto Chart",
                Location = new Point(313, 31),
                Margin = new Padding(3, 4, 3, 4),
                Size = new Size(773, 639),
            };

            // CHART AREA
            ChartArea chartArea = new ChartArea
            { 
                Name = "ChartArea",
                BackColor = Color.White
            };
            ParetoChart.ChartAreas.Add(chartArea);                                  // UPDATE AXES WITH SLIDER INFO
            ParetoChart.ChartAreas["ChartArea"].AxisX.RoundAxisValues();
            ParetoChart.ChartAreas["ChartArea"].AxisX.ScaleView.Zoomable = true;
            ParetoChart.ChartAreas["ChartArea"].AxisY.RoundAxisValues();
            ParetoChart.ChartAreas["ChartArea"].AxisY.ScaleView.Zoomable = true;
             
            // LEGEND
            Legend legend = new Legend();
            ParetoChart.Legends.Add(legend);

            // SERIES
            Series historySeries = new Series
            {
                Name = "Solution History",
                ChartType = SeriesChartType.FastPoint,
                MarkerSize = 6,
                Color = Color.LightGray
            };
            ParetoChart.Series.Add(historySeries);

            Series paretoSeries = new Series
            {
                Name = "Pareto Solutions",
                ChartType = SeriesChartType.FastPoint,
                MarkerSize = 8,
                Palette = ChartColorPalette.SeaGreen
            };
            ParetoChart.Series.Add(paretoSeries);

            // EVENTS
            ParetoChart.MouseWheel += Chart_MouseWheel;
            ParetoChart.MouseMove += Chart_MouseMove;

            Editor.Controls.Add(ParetoChart);
        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            HitTestResult result = ParetoChart.HitTest(e.X, e.Y);

            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                DataPoint point = result.Series.Points[result.PointIndex];

                solutionInfo.Visible = true;
                solutionInfo.Location = new Point(ParetoChart.Location.X + e.X, ParetoChart.Location.Y + e.Y);
                solutionInfo.Text = point.Label;

                Size size = TextRenderer.MeasureText(solutionInfo.Text, solutionInfo.Font);
                solutionInfo.Size = size;

                solutionInfo.BringToFront();
            }
            else
            {
                solutionInfo.Visible = false;
            }
        }

        private void Chart_MouseWheel(object sender, MouseEventArgs e)
        {
            var xAxis = ParetoChart.ChartAreas[0].AxisX;
            var yAxis = ParetoChart.ChartAreas[0].AxisY;

            double xMin = xAxis.ScaleView.ViewMinimum;
            double xMax = xAxis.ScaleView.ViewMaximum;
            double yMin = yAxis.ScaleView.ViewMinimum;
            double yMax = yAxis.ScaleView.ViewMaximum;

            if (e.Delta < 0)
            {
                xAxis.ScaleView.ZoomReset();    // Fix Zoom out?
                yAxis.ScaleView.ZoomReset();
            }
            else if (e.Delta > 0)
            {
                var posXStart = xAxis.PixelPositionToValue(e.X) - (xMax - xMin) / 2;
                var posXFinish = xAxis.PixelPositionToValue(e.X) + (xMax - xMin) / 2;
                var posYStart = yAxis.PixelPositionToValue(e.Y) - (yMax - yMin) / 2;
                var posYFinish = yAxis.PixelPositionToValue(e.Y) + (yMax - yMin) / 2;

                xAxis.ScaleView.Zoom(posXStart, posXFinish);
                yAxis.ScaleView.Zoom(posYStart, posYFinish);
            }
        }


        private void InitializeTexbox()
        {
            solutionInfo = new TextBox
            {
                Enabled = false,
                Multiline = true,
                ForeColor = Color.Black,
                BackColor = Color.FromArgb(250,250,250),
                TextAlign = HorizontalAlignment.Left,
                BorderStyle = BorderStyle.None,
                Visible = false
            };

            Editor.Controls.Add(solutionInfo);
        }



        // InitializeStatistics: Initialiazes the optimization statistics labels
        public void InitializeStatistics()
        {
            Population = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                ForeColor = Color.Gray,
                Location = new Point(14, 530),
                Size = new Size(200, 20),
                Text = "Population: " + NSGAII_Algorithm.PopulationSize
            };
            Editor.Controls.Add(Population);
            

            CurrentGeneration = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                ForeColor = Color.Gray,
                Location = new Point(14, 560),
                Size = new Size(200, 20),
                Text = "Current Generation: " + NSGAII_Algorithm.currentGeneration
            };
            Editor.Controls.Add(CurrentGeneration);


            TimeElapsed = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                ForeColor = Color.Gray,
                Location = new Point(14, 590),
                Size = new Size(200, 20),
                Text = "Time Elapsed: " + NSGAII_Algorithm.stopWatch.Elapsed.TotalSeconds 
            };
            Editor.Controls.Add(TimeElapsed);


            nGenes = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                ForeColor = Color.Gray,
                Location = new Point(14, 620),
                Size = new Size(100, 20),
                Text = "No of Genes: " + gh.Params.Input[0].Sources.Count
            };
            Editor.Controls.Add(nGenes);


            nObjectives = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                ForeColor = Color.Gray,
                Location = new Point(14, 650), //440
                Size = new Size(125, 20),
                Text = "No of Objectives: " + gh.Params.Input[1].Sources.Count
            };
            Editor.Controls.Add(nObjectives);
        }

        internal void AddPointsWithLabels(List<Individual> solutions, Series series)
        {
            foreach (Individual individual in solutions)
            {
                string geneLabel = "";
                string fitnessLabel = "";

                for (int i = 0; i < individual.genes.Count; i++)
                    geneLabel += $"Gene {i + 1}: { Math.Round(individual.genes[i], 3) }\r\n";         // NAME GENES/OBJECTIVES??

                for (int i = 0; i < individual.fitnesses.Count; i++)
                {
                    if (i != individual.fitnesses.Count - 1)
                        fitnessLabel += $"Fitness {i + 1}: {Math.Round(individual.fitnesses[i], 3)}\r\n";
                    else
                        fitnessLabel += $"Fitness {i + 1}: {Math.Round(individual.fitnesses[i], 3)}";  // Last one doesn't have a Newline at the end
                }

                DataPoint pt = new DataPoint(individual.fitnesses[0], individual.fitnesses[1]);      // MORE DIMENTIONS?
                pt.Label = geneLabel + fitnessLabel;
                series.Points.Add(pt);
            }
        }

        public void UpdateStatistics()    // DO THIS IN THIS CLASS?
        { }

    }
}
