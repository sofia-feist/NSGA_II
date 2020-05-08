using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;


namespace NSGA_II
{
    public class NSGAII_Visualizer
    {
        NSGAII_Editor Editor;

        public Chart ParetoChart;

        public Label TimeElapsed;
        public Label CurrentGeneration;
        public Label Population;

        TextBox infoTextBox;
        ToolTip solutionInformation = new ToolTip();

        public NSGAII_Visualizer(NSGAII_Editor editor, GH_Component _GHComponent)
        {
            Editor = editor;

            InitializeParetoChart();
            Editor.Controls.Add(ParetoChart);

            InitializeTexbox();
            InitializeStatistics(_GHComponent);
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
            ChartArea chartArea = new ChartArea();
            ParetoChart.ChartAreas.Add(chartArea);
            //ParetoChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            //ParetoChart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;

            // LEGEND
            Legend legend = new Legend();
            ParetoChart.Legends.Add(legend);

            // SERIES
            Series paretoSeries = new Series
            {
                Name = "Pareto Solutions",
                ChartType = SeriesChartType.FastPoint,
                Palette = ChartColorPalette.SeaGreen
            };
            ParetoChart.Series.Add(paretoSeries);

            Series historySeries = new Series
            {
                Name = "Solution History",
                ChartType = SeriesChartType.FastPoint,
                Color = Color.LightGray
            };
            ParetoChart.Series.Add(historySeries);

            // EVENTS
            ParetoChart.MouseWheel += Chart_MouseWheel;
            ParetoChart.MouseMove += Chart_MouseMove;
        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            solutionInformation.RemoveAll();

            HitTestResult result = ParetoChart.HitTest(e.X, e.Y);

            if (result.ChartElementType == ChartElementType.DataPoint)
            {            
                double x = result.Series.Points[result.PointIndex].XValue;
                double y = result.Series.Points[result.PointIndex].YValues[0];
                //solutionInformation.Show("x: " + x + ", y: " + y, ParetoChart);
                solutionInformation.SetToolTip(infoTextBox, "x: " + x + ", y: " + y);

                // DO STUFF HERE
            }
        }

        private void Chart_MouseWheel(object sender, MouseEventArgs e)
        {
            var xAxis = ParetoChart.ChartAreas[0].AxisX;
            var yAxis = ParetoChart.ChartAreas[0].AxisY;

            var xMin = xAxis.ScaleView.ViewMinimum;
            var xMax = xAxis.ScaleView.ViewMaximum;
            var yMin = yAxis.ScaleView.ViewMinimum;
            var yMax = yAxis.ScaleView.ViewMaximum;

            var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
            var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;
            var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 4;
            var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 4;

            xAxis.ScaleView.Zoom(posXStart, posXFinish);
            yAxis.ScaleView.Zoom(posYStart, posYFinish);
        }


        private void InitializeTexbox()
        {
            infoTextBox = new TextBox
            {
                //Location = new Point(e.X, e.Y),
                Multiline = true,
                Size = new Size(100, 50),
                ReadOnly = true,
                Text = "x: 2,22\r\ny: 5, 56",
                TextAlign = HorizontalAlignment.Center
            };

            infoTextBox.BringToFront();
            Editor.Controls.Add(infoTextBox);
        }





        // InitializeStatistics: Initialiazes the optimization statistics labels
        private void InitializeStatistics(GH_Component gh)
        {
            Population = new Label
            {
                ForeColor = Color.Gray,
                Location = new Point(14, 530),
                Size = new Size(200, 20),
                Text = "Population: " + NSGAII_Algorithm.PopulationSize
            };
            Editor.Controls.Add(Population);


            CurrentGeneration = new Label
            {
                ForeColor = Color.Gray,
                Location = new Point(14, 560),
                Size = new Size(200, 20),
                Text = "Current Generation: " + NSGAII_Algorithm.currentGeneration
            };
            Editor.Controls.Add(CurrentGeneration);


            TimeElapsed = new Label
            {
                ForeColor = Color.Gray,
                Location = new Point(14, 590),
                Size = new Size(200, 20),
                Text = "Time Elapsed: " + 0  // Update this
            };
            Editor.Controls.Add(TimeElapsed);


            Label nGenes = new Label
            {
                ForeColor = Color.Gray,
                Location = new Point(14, 620),
                Size = new Size(100, 20),
                Text = "No of Genes: " + gh.Params.Input[0].Sources.Count
            };
            Editor.Controls.Add(nGenes);


            Label nObjectives = new Label
            {
                ForeColor = Color.Gray,
                Location = new Point(14, 650), //440
                Size = new Size(125, 20),
                Text = "No of Objectives: " + gh.Params.Input[1].Sources.Count
            };
            Editor.Controls.Add(nObjectives);
        }

    }
}
