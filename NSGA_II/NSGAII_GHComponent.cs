using System;
using System.Drawing;
using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.GUI;
using System.Windows.Forms;
using Grasshopper.Kernel.Special;

namespace NSGA_II
{
    public class NSGAII_GHComponent : GH_Component
    {
        private NSGAII_Editor editor;


        //// Constructor
        public NSGAII_GHComponent()
          : base("NSGA-II Optimization", "NSGA-II",
              "Multi-Objective search and optimization using the NSGA-II algorithm",
              "NSGA-II", "Optimization")
        { }




        // Registers all the input parameters for this component.
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Genes", "G", 
                "Genes are the parameters that define the solution to optimize. Represented by one or more number sliders.", GH_ParamAccess.list);   // OR Genepools?
            pManager.AddNumberParameter("Objectives", "O", 
                "Objectives to optimize: these represent the fitnesses produced by the changing variables. Should be two or more Number values.", GH_ParamAccess.list);
        }


        // Registers all the output parameters for this component.
        protected override void RegisterOutputParams(GH_OutputParamManager pManager) { } 


        // Solves the Component Solution
        protected override void SolveInstance(IGH_DataAccess DA) { }


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
            m_attributes = new GH_Component_Attributes(this);
        }


        // Display Form: Displays the NSGA-II Editor Form
        public void DisplayEditor()
        {
            if (editor == null || editor.IsDisposed)
                editor = new NSGAII_Editor(this);
                            
            GH_WindowsFormUtil.CenterFormOnCursor(editor, true);
            editor.Show();
        }


        
    }
}
