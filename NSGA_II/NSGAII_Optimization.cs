using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace NSGA_II
{
    
    public class NSGAII_Optimization : GH_Component
    {
        //Random random = new Random();
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public NSGAII_Optimization()
          : base("NSGA-II Optimization", "NSGA-II",
              "Multi-Objective search and optimization using the NSGA-II algorithm",
              "NSGA-II", "Optimization")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Pop Size", "S", "Population size", GH_ParamAccess.item, 100);
            pManager.AddIntegerParameter("N Iterations", "I", "Number of Iterations to run the optimization", GH_ParamAccess.item, 100);

            //pManager.AddNumberParameter("Parameters", "P", "Parameters to optimize", GH_ParamAccess.list);
            //pManager.AddNumberParameter("Objectives", "O", "Objectives to optimize", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Output", "O", "Output Result", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int popSize = 0;
            if (!DA.GetData("Pop Size", ref popSize)) { return; }
            if (popSize < 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Population Size must be a positive integer");
                return;
            }


            int nGenerations = 0;
            if (!DA.GetData("N Iterations", ref nGenerations)) { return; }
            if (nGenerations < 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Number of Iterations must be a positive integer");
                return;
            }



            //List<GH_NumberSlider> parameterSliders = new List<GH_NumberSlider>();

            //foreach (IGH_Param source in Params.Input[2].Sources)
            //{
            //    GH_NumberSlider slider = source as GH_NumberSlider;

            //    if (slider != null)
            //        parameterSliders.Add(slider);
            //}

            //foreach (GH_NumberSlider slider in parameterSliders)
            //{
            //    slider.Slider.Value = (decimal)random.NextDouble() * (slider.Slider.Maximum - slider.Slider.Minimum) + slider.Slider.Minimum;
            //}

            //List<double> parameters = new List<double>();
            //if (!DA.GetDataList("Parameters", parameters)) { return; }
            //if ((parameters.Count <= 0)) { return; }

            //List<double> objectives = new List<double>();
            //if (!DA.GetDataList("Objectives", objectives)) { return; }
            //if ((objectives.Count <= 0)) { return; }


            NSGAII_Algorithm nsgaII = new NSGAII_Algorithm(popSize, nGenerations);
            List<Point2d> solutions = new List<Point2d>();

            foreach (var individual in nsgaII.population)
            {
                double x = individual.fitnesses[0];
                double y = individual.fitnesses[1];
                Point2d pt = new Point2d(x,y);
                solutions.Add(pt);
            }
            
            DA.SetDataList("Output", solutions);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2f27ece1-cec4-4cad-90ee-d185d418f01b"); }
        }
    }
}
