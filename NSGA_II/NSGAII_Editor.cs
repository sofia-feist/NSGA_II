using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Grasshopper.Kernel;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NSGA_II
{
    public partial class NSGAII_Editor : Form
    {
        NSGAII_Visualizer visualizer;
        NSGAII_Algorithm nsgaII;

        public static bool TimeChecked;
        public static bool GenerationsChecked;


        // Visualizer and algoritm
        public NSGAII_Editor(GH_Component _GHComponent)
        {
            visualizer = new NSGAII_Visualizer(this, _GHComponent);
            nsgaII = new NSGAII_Algorithm(visualizer, _GHComponent);
            

            InitializeComponent(); 

            TimeChecked = TimeCheckBox.Checked;
            GenerationsChecked = GenerationsCheckBox.Checked;

        }

        private void NSGAII_EditorWindow(object sender, EventArgs e)
        {
            

        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PopSizeInputField_ValueChanged(object sender, EventArgs e)
        {
            NSGAII_Algorithm.PopulationSize = (int)PopSizeInputField.Value;
            visualizer.Population.Text = "Population: " + NSGAII_Algorithm.PopulationSize;
        }

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
                NGenerationsInputField.Enabled = false;
                NGenerationsLabel.ForeColor = SystemColors.ButtonShadow;
                GenerationsCheckBox.ForeColor = SystemColors.ButtonShadow;
            }
        }

        private void NGenerationsInputField_ValueChanged(object sender, EventArgs e)
        {
            NSGAII_Algorithm.MaxGenerations = (int)NGenerationsInputField.Value;
        }

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
                DurationTimeHours.Enabled = false;
                DurationTimeMinutes.Enabled = false;

                TimeCheckBox.ForeColor = SystemColors.ButtonShadow;
                DurationLabel.ForeColor = SystemColors.ButtonShadow;
            }
        }

        private void DurationHours_ValueChanged(object sender, EventArgs e)
        {
            NSGAII_Algorithm.MaxDuration = (double) (DurationTimeHours.Value * 60 + DurationTimeMinutes.Value);
        }

        private void DurationMinutes_ValueChanged(object sender, EventArgs e)
        {
            NSGAII_Algorithm.MaxDuration = (double)(DurationTimeHours.Value * 60 + DurationTimeMinutes.Value);
        }

        private void RunOptimizationButton_Click(object sender, EventArgs e)
        {
            if (TimeCheckBox.Checked || GenerationsCheckBox.Checked)
                nsgaII.NSGAII();
            else   // IMPROVE!!!!
                throw new InvalidOperationException("Select a Stop condition"); //AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Select a Stop condition"); 

        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            NSGAII_Algorithm.currentGeneration = 0;
            visualizer.CurrentGeneration.Text = "Current Generation: " + NSGAII_Algorithm.currentGeneration;


            visualizer.ParetoChart.Legends.Clear();
            visualizer.ParetoChart.Series.Clear();
            visualizer.ParetoChart.ChartAreas.Clear();
            visualizer.InitializeParetoChart();
            //visualizer.ParetoChart.Invalidate();
        }


    }
}
