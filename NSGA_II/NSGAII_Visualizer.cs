using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
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

        GH_Component GHComponent;

        public Chart ParetoChart;

        public Label Population;
        public Label CurrentGeneration;
        public Label TimeElapsed;
        public Label nGenes;
        public Label nObjectives;

        TextBox solutionInfo;
        

        public NSGAII_Visualizer(NSGAII_Editor editor, GH_Component _GHComponent)
        {
            Editor = editor;
            GHComponent = _GHComponent;

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
            ParetoChart.ChartAreas.Add(chartArea);
            //ParetoChart.ChartAreas["ChartArea"].AxisX.Minimum = 0;     // UPDATE WITH SLIDER INFO
            //ParetoChart.ChartAreas["ChartArea"].AxisX.Interval = 4;
            ParetoChart.ChartAreas["ChartArea"].AxisX.ScaleView.Zoomable = true;
            //ParetoChart.ChartAreas["ChartArea"].AxisY.Minimum = 0;
            //ParetoChart.ChartAreas["ChartArea"].AxisY.Interval = 4;
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
                //string xLabel = 
                double xCoord = Math.Round(result.Series.Points[result.PointIndex].XValue, 3);
                double yCoord = Math.Round(result.Series.Points[result.PointIndex].YValues[0], 3);

                solutionInfo.Visible = true;
                solutionInfo.Location = new Point(ParetoChart.Location.X + e.X, ParetoChart.Location.Y + e.Y);
                solutionInfo.Text = $"x: {xCoord}\r\ny: {yCoord}";
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
                AutoSize = true,
                Multiline = true,
                Size = new Size(100, 50),
                Cursor = Cursors.Arrow,
                ForeColor = Color.Black,
                BackColor = Color.FromArgb(250,250,250),
                TextAlign = HorizontalAlignment.Center,
                BorderStyle = BorderStyle.None,
                Enabled = false,
                Visible = false
            };

            Editor.Controls.Add(solutionInfo);
        }
        //RectangleAnnotation solutionInfo = new RectangleAnnotation();
        ////solutionInfo.Visible = false;
        //solutionInfo.AllowAnchorMoving = true;
        //solutionInfo.ForeColor = Color.Black;
        //solutionInfo.X = double.NaN;
        //solutionInfo.Y = double.NaN;
        //solutionInfo.AnchorX = double.NaN;
        //solutionInfo.AnchorY = double.NaN;
        ////solutionInfo.Font = new Font("Source Sans Pro", 9); ;
        //solutionInfo.LineWidth = 0;
        //solutionInfo.Text = "x: 0000";
        //solutionInfo.BackColor = Color.FromArgb(30,220,220,220);
        //solutionInfo.Width = 30;
        //solutionInfo.Height = 15;
        //solutionInfo.BringToFront();

        //ParetoChart.Annotations.Add(solutionInfo);

        //solutionInfo.SetAnchor(result.Series.Points[result.PointIndex]);
        //solutionInfo.AnchorDataPoint = result.Series.Points[result.PointIndex];




        // InitializeStatistics: Initialiazes the optimization statistics labels
        public void InitializeStatistics()
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
                Text = "Time Elapsed: " + NSGAII_Algorithm.stopWatch.Elapsed.TotalSeconds 
            };
            Editor.Controls.Add(TimeElapsed);


            nGenes = new Label
            {
                ForeColor = Color.Gray,
                Location = new Point(14, 620),
                Size = new Size(100, 20),
                Text = "No of Genes: " + GHComponent.Params.Input[0].Sources.Count
            };
            Editor.Controls.Add(nGenes);


            nObjectives = new Label
            {
                ForeColor = Color.Gray,
                Location = new Point(14, 650), //440
                Size = new Size(125, 20),
                Text = "No of Objectives: " + GHComponent.Params.Input[1].Sources.Count
            };
            Editor.Controls.Add(nObjectives);
        }

    }
}
