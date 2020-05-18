using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Grasshopper.Kernel;


namespace NSGA_II
{
    public class NSGAII_Visualizer
    {
        private NSGAII_Editor Editor;
        private GH_Component gh;

        internal List<string> fitnessNames;


        public Chart ParetoChart;

        public Label Population;
        public Label CurrentGeneration;
        public Label TimeElapsed;
        public Label nGenes;
        public Label nObjectives;

        private TextBox solutionInfo;

        private CheckBox HistorySeriesCheckBox;
        private CheckBox ParetoSeriesCheckBox;




        public NSGAII_Visualizer(NSGAII_Editor editor, GH_Component _GHComponent)
        {
            Editor = editor;
            gh = _GHComponent;


            InitializeParetoChart();
            InitializeStatistics();
            InitializeTexbox();
            InitializeLegendCheckBoxes();
        }




        ////////////////////////////////////////////////////////////////////////////
        /////////////////////////////// PARETO CHART ///////////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        // InitializeParetoChart: Initializes the Pareto Chart
        private void InitializeParetoChart()
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
                Color = Color.SteelBlue
            };
            ParetoChart.Series.Add(paretoSeries);


            // EVENTS
            ParetoChart.MouseWheel += Chart_MouseWheel;
            ParetoChart.MouseMove += Chart_MouseMove;

            Editor.Controls.Add(ParetoChart);
            ParetoChart.SendToBack();
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
                xAxis.ScaleView.ZoomReset();   
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


        // InitializeLegendCheckBoxes: Initializes the checkboxes for the legend
        private void InitializeLegendCheckBoxes()
        {
            HistorySeriesCheckBox = new CheckBox
            {
                Checked = true,
                FlatStyle = FlatStyle.Flat,
                ForeColor = SystemColors.ControlDark,
                Location = new Point(900, 58),
                Size = new Size(12, 12),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            HistorySeriesCheckBox.CheckedChanged += new EventHandler(HistorySeriesCheckBox_CheckedChanged);

            Editor.Controls.Add(HistorySeriesCheckBox);
            HistorySeriesCheckBox.BringToFront();


            ParetoSeriesCheckBox = new CheckBox
            {
                Checked = true,
                FlatStyle = FlatStyle.Flat,
                ForeColor = SystemColors.ControlDark,
                Location = new Point(900, 75),
                Size = new Size(12, 12),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            ParetoSeriesCheckBox.CheckedChanged += new EventHandler(ParetoSeriesCheckBox_CheckedChanged);

            Editor.Controls.Add(ParetoSeriesCheckBox);
            ParetoSeriesCheckBox.BringToFront();
        }


        // HistorySeriesCheckBox_CheckedChanged: Enables/Disables the History Series on the chart
        private void HistorySeriesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Series historySeries = ParetoChart.Series["Solution History"];

            if (HistorySeriesCheckBox.Checked)
                historySeries.Color = Color.LightGray;
            else
                historySeries.Color = Color.FromArgb(0, 0, 0, 0);
        }


        // ParetoSeriesCheckBox_CheckedChanged: Enables/Disables the Pareto Series on the chart
        private void ParetoSeriesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Series paretoSeries = ParetoChart.Series["Pareto Solutions"];

            if (ParetoSeriesCheckBox.Checked)
                paretoSeries.Color = Color.SteelBlue;
            else
                paretoSeries.Color = Color.FromArgb(0, 0, 0, 0);
        }




        // AddPointsWithLabels: Adds data points to a given series using labels to store solutions' information (genes/fitness)
        // x and y values of the datapoints correspoint to the first two fitness values of the solutions
        // 3D+ DIMENTIONS NOT IMPLEMENTED ON CHART YET
        internal void AddPointsWithLabels(List<Individual> solutions, Series series)
        {
            foreach (Individual individual in solutions)
            {
                string geneLabel = "";
                string fitnessLabel = "";

                // Build Gene Label
                for (int i = 0; i < individual.genes.Count; i++)                                   
                    geneLabel += $"Gene {i + 1}: { Math.Round(individual.genes[i], 3) }\r\n";       

                // Build Fitness Label
                for (int i = 0; i < individual.fitnesses.Count; i++)
                    fitnessLabel += $"{fitnessNames[i]}: {Math.Round(individual.fitnesses[i], 3)}\r\n";
                
                double chosenFitness1 = individual.fitnesses[0];    // IMPROVE : Allow user to select fitnesses to display on Chart
                double chosenFitness2 = individual.fitnesses[1];

                DataPoint pt = new DataPoint(chosenFitness1, chosenFitness2);     
                pt.Label = geneLabel + fitnessLabel.Substring(0, fitnessLabel.Length - 2);
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
                Location = new Point(14, 650), 
                Size = new Size(125, 20),
                Text = "No of Objectives: " + gh.Params.Input[1].Sources.Count
            };
            Editor.Controls.Add(nObjectives);
        }

    }
}
