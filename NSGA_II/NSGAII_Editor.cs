using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NSGA_II
{
    public partial class NSGAII_Editor : Form
    {
        private NSGAII_Visualizer visualizer;

        internal static bool TimeChecked;
        internal static bool GenerationsChecked;
        private bool OptimizationRunning = false;

        internal static BackgroundWorker backgroundWorker;




        // Constructor
        public NSGAII_Editor()
        {
            visualizer = new NSGAII_Visualizer(this);

            InitializeComponent();     // <- Windows Forms Window
            InitializeOptimization();

            TimeChecked = TimeCheckBox.Checked;
            GenerationsChecked = GenerationsCheckBox.Checked;


            // Asynchronous Run Optimization
            backgroundWorker = new BackgroundWorker() 
            { 
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true      
            };
            backgroundWorker.DoWork += NSGAII_Algorithm.NSGAII;
            backgroundWorker.ProgressChanged += OptimizationInProgress;
            backgroundWorker.RunWorkerCompleted += OptimizationComplete;
        }

        




        // InitializeOptimization: Initializes statistics and parameters for the optimization (When Editor Window is opened and when Reset button is pressed)
        public void InitializeOptimization()
        {
            NSGAII_Algorithm.currentGeneration = 0;
            NSGAII_Algorithm.stopWatch = new Stopwatch();

            visualizer.CurrentGeneration.Text = "Current Generation: " + NSGAII_Algorithm.currentGeneration;
            visualizer.TimeElapsed.Text = "Time Elapsed: " + NSGAII_Algorithm.stopWatch.Elapsed.TotalSeconds;

            for (int i = 0; i < visualizer.ParetoChart.Series.Count; i++)
                visualizer.ParetoChart.Series[i].Points.Clear();
        }




        #region Background worker Event Handlers
        // OptimizationInProgress: Displays the optimization statistics in progress
        private void OptimizationInProgress(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 50)
            {
                visualizer.CurrentGeneration.Text = "Current Generation: " + (NSGAII_Algorithm.currentGeneration - 1);
                visualizer.TimeElapsed.Text = "Time Elapsed: " + TimeSpan.FromSeconds(NSGAII_Algorithm.stopWatch.Elapsed.TotalSeconds).ToString(@"hh\:mm\:ss");

                // Too heavy to compute points at every iteration
                //visualizer.ParetoChart.Series[0].Points.Clear();
                //NSGAII_Algorithm.currentPopulation.ForEach(i => visualizer.ParetoChart.Series[0].Points.AddXY(i.fitnesses[0], i.fitnesses[1]));
            }
        }


        // OptimizationComplete: Displays the final optimization statistics
        private void OptimizationComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            OptimizationRunning = false;
            stopResetButton.Text = "Reset";

            visualizer.CurrentGeneration.Text = "Current Generation: " + (NSGAII_Algorithm.currentGeneration - 1);
            visualizer.TimeElapsed.Text = "Time Elapsed: " + TimeSpan.FromSeconds(NSGAII_Algorithm.stopWatch.Elapsed.TotalSeconds).ToString(@"hh\:mm\:ss");  

            visualizer.AddPointsWithLabels(NSGAII_Algorithm.archive, visualizer.ParetoChart.Series[0]);
            visualizer.AddPointsWithLabels(NSGAII_Algorithm.paretoFrontHistory, visualizer.ParetoChart.Series[1]);
        }
        #endregion




        #region Editor Window Event Handlers

        ////////////////////////////////////////////////////////////////////////////
        /////////////////////// EDITOR WINDOW EVENT HANDLERS ///////////////////////
        ////////////////////////////////////////////////////////////////////////////


        // OkButton_Click: Event Method for when the OK Button is pressed
        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }


        // PopSizeInputField_ValueChanged: Event Method for when the Population Size Input Field is changed
        private void PopSizeInputField_ValueChanged(object sender, EventArgs e)
        {
            NSGAII_Algorithm.PopulationSize = (int)PopSizeInputField.Value;
            visualizer.Population.Text = "Population: " + NSGAII_Algorithm.PopulationSize;
        }


        // GenerationCheckBox_CheckedChanged: Event Method for when the Generation CheckBox is pressed
        private void GenerationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (GenerationsCheckBox.Checked == true)
            {
                GenerationsChecked = true;
                NGenerationsInputField.Enabled = true;

                GenerationsCheckBox.ForeColor = Color.Black;
                NGenerationsLabel.ForeColor = Color.Black;
            }
            else
            {
                GenerationsChecked = false;
                NGenerationsInputField.Enabled = false;

                NGenerationsLabel.ForeColor = SystemColors.ButtonShadow;
                GenerationsCheckBox.ForeColor = SystemColors.ButtonShadow;
            }
        }


        // NGenerationsInputField_ValueChanged: Event Method for when the N Generations Input Field is changed
        private void NGenerationsInputField_ValueChanged(object sender, EventArgs e)
        {
            NSGAII_Algorithm.MaxGenerations = (int)NGenerationsInputField.Value;
        }


        // TimeCheckBox_CheckedChanged: Event Method for when the Time CheckBox is pressed
        private void TimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TimeCheckBox.Checked == true)
            {
                TimeChecked = true;
                DurationTimeHours.Enabled = true;
                DurationTimeMinutes.Enabled = true;

                TimeCheckBox.ForeColor = Color.Black;
                DurationLabel.ForeColor = Color.Black;
            }
            else
            {
                TimeChecked = false;
                DurationTimeHours.Enabled = false;
                DurationTimeMinutes.Enabled = false;

                TimeCheckBox.ForeColor = SystemColors.ButtonShadow;
                DurationLabel.ForeColor = SystemColors.ButtonShadow;
            }
        }


        // DurationHours_ValueChanged: Event Method for when the Duration (HOURS) Input Field is changed
        private void DurationHours_ValueChanged(object sender, EventArgs e)
        {
            NSGAII_Algorithm.MaxDuration = (int)(DurationTimeHours.Value * 3600 + DurationTimeMinutes.Value * 60);
        }


        // DurationMinutes_ValueChanged: Event Method for when the Duration (MINUTES) Input Field is changed
        private void DurationMinutes_ValueChanged(object sender, EventArgs e)
        {
            NSGAII_Algorithm.MaxDuration = (int)(DurationTimeHours.Value * 3600 + DurationTimeMinutes.Value * 60);
        }


        // RunOptimizationButton_Click: Event Method for when the Run Optimization Button is pressed
        private void RunOptimizationButton_Click(object sender, EventArgs e)
        {
            if (GH_ParameterHandler.gh.Params.Input[0].Sources.Count < 1)
                MessageBox.Show("Component must have at least one slider gene input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (GH_ParameterHandler.gh.Params.Input[1].Sources.Count < 2)
                MessageBox.Show("Component must have at least two fitness inputs", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (TimeCheckBox.Checked == false && GenerationsCheckBox.Checked == false)
                MessageBox.Show("Select a Stop condition", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (backgroundWorker.IsBusy == false)
            {
                OptimizationRunning = true;

                // RunButton Appearance
                RunOptimizationButton.Enabled = false;
                RunOptimizationButton.ForeColor = SystemColors.ButtonShadow;
                RunOptimizationButton.FlatStyle = FlatStyle.Flat;

                // Start Optimization in the background worker
                backgroundWorker.RunWorkerAsync();
            }
        }


        // ResetButton_Click: Toggles between Stop/Reset Optimization if the the latter is running
        private void StopResetButton_Click(object sender, EventArgs e)
        {
            if (OptimizationRunning == true)
            {
                // Cancel Optimization
                backgroundWorker.CancelAsync();
                OptimizationRunning = false;

                // Reset Button Text
                stopResetButton.Text = "Reset";
            }
            else
            {
                // Reset Button Text
                stopResetButton.Text = "Stop";

                // RunButton Appearance
                RunOptimizationButton.Enabled = true;
                RunOptimizationButton.ForeColor = SystemColors.ControlText;
                RunOptimizationButton.FlatStyle = FlatStyle.Standard;

                // Initialize Parameters for new optimization
                InitializeOptimization();
            }
        }
        #endregion

        //GH_ParameterHandler.SetSliderValues(GH_ParameterHandler.gh.OnPingDocument()); //gh.OnPingDocument().ScheduleSolution(5, SetSliderValues);
        //GH_ParameterHandler.SetGeneInputs();

        //List<double> genes = GH_ParameterHandler.GetGeneValues();

        //Label x = new Label
        //{
        //    Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
        //    ForeColor = Color.Gray,
        //    Location = new Point(14, 500),
        //    Size = new Size(250, 20),
        //    Text = genes[0] + "," + genes[1] + "," + genes[2]
        //};

        //Controls.Add(x);
    }

}
