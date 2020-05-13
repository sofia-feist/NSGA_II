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

        internal BackgroundWorker TimerThread;
        internal BackgroundWorker backgroundWorker;



        public NSGAII_Editor(GH_Component _GHComponent)
        {
            visualizer = new NSGAII_Visualizer(this, _GHComponent);

            InitializeComponent();     // <- Windows Forms Window
            InitializeOptimization();

            TimeChecked = TimeCheckBox.Checked;
            GenerationsChecked = GenerationsCheckBox.Checked;


            // Asynchronous Run Optimization
            backgroundWorker = new BackgroundWorker() 
            { 
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true       // IMPLEMENT Cancellation
            };
            backgroundWorker.DoWork += background_RunOptimization;
            //backgroundWorker.ProgressChanged += background_OptimizationProgress;
            backgroundWorker.RunWorkerCompleted += background_OptimizationComplete;
       


            //TimerThread = new BackgroundWorker();
            //TimerThread.DoWork += new DoWorkEventHandler(visualizer.background_Timer);
            

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
        // background_RunOptimization: Starts Optimization
        private void background_RunOptimization(object sender, DoWorkEventArgs e)
        {
            NSGAII_Algorithm.NSGAII();
        }


        // background_OptimizationComplete: Method Called when optimization is completed
        private void background_OptimizationComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            visualizer.CurrentGeneration.Text = "Current Generation: " + (NSGAII_Algorithm.currentGeneration - 1);
            visualizer.TimeElapsed.Text = "Time Elapsed: " + TimeSpan.FromSeconds(NSGAII_Algorithm.stopWatch.Elapsed.TotalSeconds).ToString(@"hh\:mm\:ss\.fff");

            visualizer.AddPointsWithLabels(NSGAII_Algorithm.archive, visualizer.ParetoChart.Series[0]);
            visualizer.AddPointsWithLabels(NSGAII_Algorithm.paretoFront, visualizer.ParetoChart.Series[1]);
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
            if (backgroundWorker.IsBusy == false)
            {
                if (TimeCheckBox.Checked || GenerationsCheckBox.Checked)
                {
                    // RunButton Appearance
                    RunOptimizationButton.Enabled = false;
                    RunOptimizationButton.ForeColor = SystemColors.ButtonShadow;
                    RunOptimizationButton.FlatStyle = FlatStyle.Flat;

                    // Start Optimization in the background
                    backgroundWorker.RunWorkerAsync();
                    //TimerThread.RunWorkerAsync();
                }
                    
                else
                    MessageBox.Show("Select a Stop condition", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }


        // ResetButton_Click: Event Method for when the Reset Button is pressed
        private void ResetButton_Click(object sender, EventArgs e)
        {
            // Initialize Parameters for new optimization
            InitializeOptimization();

            // RunButton Appearance
            RunOptimizationButton.Enabled = true;
            RunOptimizationButton.ForeColor = SystemColors.ControlText;
            RunOptimizationButton.FlatStyle = FlatStyle.Standard;
        }
        #endregion
    }
}
