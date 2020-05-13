using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

namespace NSGA_II
{
    public class GH_ParameterHandler
    {
        internal static GH_Component gh;
        internal static List<GH_NumberSlider> geneInputSliders;

        internal static List<IGH_Param> fitnessInputs;

        private static Random random = new Random();


        public GH_ParameterHandler(GH_Component _GHComponent)
        {
            gh = _GHComponent;

            SetGeneInputs();
            SetFitnessInputs();
        }


        // Initializes list of input sliders
        private void SetGeneInputs()
        {
            geneInputSliders = new List<GH_NumberSlider>();

            //if (gh.Params.Input[0].Sources.Count < 1)
            //    MessageBox.Show("", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            foreach (IGH_Param source in gh.Params.Input[0].Sources)
            {
                GH_NumberSlider slider = source as GH_NumberSlider;  // Add gene pools as well

                if (slider != null)
                    geneInputSliders.Add(slider);
            }
        }


        // Initializes list of fitness inputs
        private void SetFitnessInputs()
        {
            fitnessInputs = (List<IGH_Param>)gh.Params.Input[1].Sources;     //  ??????????????
        }


        //NSGAII_Editor.gh.OnPingDocument().ScheduleSolution(5, ChangeGenes);
        public List<double> ChangeSliderValues()
        {
            var genes = new List<double>();

            foreach (var slider in geneInputSliders)
            {
                slider.Slider.RaiseEvents = false;
                slider.TickValue = random.Next(slider.TickCount + 1);    // +1 (include Max value)
                genes.Add((double)slider.Slider.Value);
                slider.ExpireSolution(false);
                slider.Slider.RaiseEvents = true;
            }

            while (gh.OnPingDocument().SolutionState != GH_ProcessStep.PreProcess || gh.OnPingDocument().SolutionDepth != 0) { }

            gh.OnPingDocument().NewSolution(true);

            while (gh.OnPingDocument().SolutionState != GH_ProcessStep.PostProcess || gh.OnPingDocument().SolutionDepth != 0) { }

            return genes;
        }


        public List<double> GetFitnessValues()
        {
            var fitnesses = new List<double>();

            foreach (IGH_Param source in fitnessInputs)
            {
                foreach (var item in source.VolatileData.AllData(false))
                {
                    double fitness;
                    if (GH_Convert.ToDouble(item, out fitness, GH_Conversion.Both))
                        fitnesses.Add(fitness);
                }
            }

            return fitnesses;
        }

    }
}
