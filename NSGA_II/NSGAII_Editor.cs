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
using Grasshopper.Kernel.Special;

namespace NSGA_II
{
    public partial class NSGAII_Editor : Form
    {
        GH_Component gh;
        NSGAII_Visualizer visualizer;
        NSGAII_Algorithm nsgaII;

        public static bool TimeChecked;
        public static bool GenerationsChecked;

        public List<GH_NumberSlider> geneInputs;
        public List<IGH_Param> fitnessInputs;


        public NSGAII_Editor(GH_Component _GHComponent)
        {
            visualizer = new NSGAII_Visualizer(this, _GHComponent);
            nsgaII = new NSGAII_Algorithm(visualizer, _GHComponent);

            gh = _GHComponent;

            InitializeComponent(); 

            TimeChecked = TimeCheckBox.Checked;
            GenerationsChecked = GenerationsCheckBox.Checked;



            geneInputs = new List<GH_NumberSlider>();

            foreach (IGH_Param source in gh.Params.Input[0].Sources)
            {
                GH_NumberSlider slider = source as GH_NumberSlider;

                if (slider != null)
                    geneInputs.Add(slider); 
            }

            fitnessInputs = (List<IGH_Param>) gh.Params.Input[1].Sources;
        }

        private void NSGAII_EditorWindow(object sender, EventArgs e)
        {

            //foreach (GH_NumberSlider slider in parameterSliders)
            //{
            //    slider.Slider.Value = (decimal)random.NextDouble() * (slider.Slider.Maximum - slider.Slider.Minimum) + slider.Slider.Minimum;
            //}
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
                GenerationsChecked = false;
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
                TimeChecked = false;
                DurationTimeHours.Enabled = false;
                DurationTimeMinutes.Enabled = false;

                TimeCheckBox.ForeColor = SystemColors.ButtonShadow;
                DurationLabel.ForeColor = SystemColors.ButtonShadow;
            }
        }

        private void DurationHours_ValueChanged(object sender, EventArgs e)
        {
            NSGAII_Algorithm.MaxDuration = (double) (DurationTimeHours.Value * 3600 + DurationTimeMinutes.Value * 60);
        }

        private void DurationMinutes_ValueChanged(object sender, EventArgs e)
        {
            NSGAII_Algorithm.MaxDuration = (double)(DurationTimeHours.Value * 3600 + DurationTimeMinutes.Value * 60);
        }

        private void RunOptimizationButton_Click(object sender, EventArgs e)
        {
            if (TimeCheckBox.Checked || GenerationsCheckBox.Checked)
                nsgaII.NSGAII();
            else 
                MessageBox.Show("Select a Stop condition", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            nsgaII.InitializeOptimization();
        }

    }
}
