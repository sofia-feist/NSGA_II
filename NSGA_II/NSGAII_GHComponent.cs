using System;
using System.Drawing;
using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.GUI;
using System.Windows.Forms;


namespace NSGA_II
{
    public class NSGAII_GHComponent : GH_Component
    {
        
        
        //// Constructor
        public NSGAII_GHComponent()
          : base("NSGA-II Optimization", "NSGA-II",
              "Multi-Objective search and optimization using the NSGA-II algorithm",
              "NSGA-II", "Optimization")
        { }

        


        // Registers all the input parameters for this component.
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        { 
            pManager.AddNumberParameter("Genes", "G", "Parameters to optimize", GH_ParamAccess.list);
            pManager.AddNumberParameter("Objectives", "O", "Objectives/Fitnesses to optimize", GH_ParamAccess.list);   // Fitness
        }


        // Registers all the output parameters for this component.
        protected override void RegisterOutputParams(GH_OutputParamManager pManager) { }


        // Solves the Component Solution
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }


        // Provides an Icon (24x24 pixels) for every component that will be visible in the User Interface.
        protected override Bitmap Icon
        {
            //return Resources.IconForThisComponent;
            get { return null; }
        }


        // Sets Component's Guid identifier
        public override Guid ComponentGuid
        {
            get { return new Guid("2f27ece1-cec4-4cad-90ee-d185d418f01b"); }
        }


        // CreateAttributes: Creates specific attributes for the component (in this case, overrides the double click properties)
        public override void CreateAttributes()
        {
            m_attributes = new NSGAII_GHComponent_Attributes(this);
        }


        // Display Form: Displays the NSGA-II Editor Form
        public void DisplayForm()
        {
            NSGAII_Editor editor = new NSGAII_Editor(this);
            GH_WindowsFormUtil.CenterFormOnCursor(editor, true);

            editor.ShowDialog();
        }


        
    }
}
