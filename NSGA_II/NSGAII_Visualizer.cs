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


        string chartAxisLabelX = "FITNESS 1";  // Allow Textbox to change the name of the fitnesses
        string chartAxisLabelY = "FITNESS 2";


        public Chart ParetoChart;

        public Label Population;
        public Label CurrentGeneration;
        public Label TimeElapsed;
        public Label nGenes;
        public Label nObjectives;

        private TextBox solutionInfo;



        public NSGAII_Visualizer(NSGAII_Editor editor, GH_Component _GHComponent)
        {
            Editor = editor;
            gh = _GHComponent;

            InitializeParetoChart();
            InitializeStatistics();
            InitializeTexbox();
        }




        ////////////////////////////////////////////////////////////////////////////
        /////////////////////////////// PARETO CHART ///////////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        // InitializeParetoChart: Initializes the Pareto Chart
        public void InitializeParetoChart()
        {
            // CHART
            ParetoChart = new Chart
            {
                Name = "Pareto Chart",
                Location = new Point(313, 31),
                Margin = new Padding(3, 4, 3, 4),
                Size = new Size(773, 639),
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom
            };

            // CHART AREA
            ChartArea chartArea = new ChartArea
            { 
                Name = "ChartArea",
                BackColor = Color.White
            };
            // Axis Info   
            chartArea.AxisX.Title = chartAxisLabelX;   // Custom Titles? (Named Genes)
            chartArea.AxisY.Title = chartAxisLabelY;
            chartArea.AxisX.RoundAxisValues();
            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.AxisY.RoundAxisValues();
            chartArea.AxisY.ScaleView.Zoomable = true;
            //Scroll Bar
            AxisScrollBar scrollX = new AxisScrollBar
            {
                Size = 8,
                ButtonColor = Color.FromArgb(210,210,210),
                ButtonStyle = ScrollBarButtonStyles.None

            };
            chartArea.AxisX.ScrollBar = scrollX;
            AxisScrollBar scrollY = new AxisScrollBar
            {
                Size = 8,
                ButtonColor = Color.FromArgb(210, 210, 210),
                ButtonStyle = ScrollBarButtonStyles.None

            };
            chartArea.AxisY.ScrollBar = scrollY;
            ParetoChart.ChartAreas.Add(chartArea);


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
            ParetoChart.Series[0].Points.AddXY(1, 1); 
            ParetoChart.Series[0].Points.AddXY(10, 10);
            ParetoChart.Series[1].Points.AddXY(3, 3);

            // EVENTS
            ParetoChart.MouseWheel += Chart_MouseWheel;
            ParetoChart.MouseMove += Chart_MouseMove;

            Editor.Controls.Add(ParetoChart);
        }


        // Chart_MouseMove: Event handler for the mouse mouvement over the Pareto chart -> if the mouse moves over a chart data point, the respective solution info textbox will be displayed
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


        // Chart_MouseWheel: Event handler for a zoom functionality with the mouse screenwheel
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


        // AddPointsWithLabels: Adds data points to a given series using labels to store solutions' information (genes/fitness)
        // x and y values of the datapoints correspoint to the first two fitness values of the solutions
        // 3D+ DIMENTIONS NOT IMPLEMENTED YET
        internal void AddPointsWithLabels(List<Individual> solutions, Series series)
        {
            foreach (Individual individual in solutions)
            {
                string geneLabel = "";
                string fitnessLabel = "";

                for (int i = 0; i < individual.genes.Count; i++)                                    // ROUND NEEDED?
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


        ////////////////////////////////////////////////////////////////////////////
        /////////////////////////// SOLUTION INFO TEXTBOX //////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        // InitializeTexbox: Initializes the textbox containing the solutions' information
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



        ////////////////////////////////////////////////////////////////////////////
        ///////////////////////// OPTIMIZATION STATISTICS //////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        // InitializeStatistics: Initializes the optimization statistics labels
        private void InitializeStatistics()
        {
            Population = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                ForeColor = Color.Gray,
                Location = new Point(14, 530),
                Size = new Size(200, 20),
                Text = "Population: " + NSGAII_Algorithm.PopulationSize
            };
            Editor.Controls.Add(Population);
            

            CurrentGeneration = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                ForeColor = Color.Gray,
                Location = new Point(14, 560),
                Size = new Size(200, 20),
                Text = "Current Generation: " + NSGAII_Algorithm.currentGeneration
            };
            Editor.Controls.Add(CurrentGeneration);


            TimeElapsed = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                ForeColor = Color.Gray,
                Location = new Point(14, 590),
                Size = new Size(200, 20),
                Text = "Time Elapsed: " + NSGAII_Algorithm.stopWatch.Elapsed.TotalSeconds 
            };
            Editor.Controls.Add(TimeElapsed);


            nGenes = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                ForeColor = Color.Gray,
                Location = new Point(14, 620),
                Size = new Size(100, 20),
                Text = "No of Genes: " + gh.Params.Input[0].Sources.Count
            };
            Editor.Controls.Add(nGenes);


            nObjectives = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                ForeColor = Color.Gray,
                Location = new Point(14, 650), //440
                Size = new Size(125, 20),
                Text = "No of Objectives: " + gh.Params.Input[1].Sources.Count
            };
            Editor.Controls.Add(nObjectives);
        }

        public void UpdateStatistics()    // DO THIS HERE?
        { }

    }
}
