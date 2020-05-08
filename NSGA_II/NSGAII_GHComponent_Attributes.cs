using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;


namespace NSGA_II
{
    class NSGAII_GHComponent_Attributes : GH_ComponentAttributes
    {
        public NSGAII_GHComponent_Attributes(IGH_Component NSGAII_GHComponent) : base(NSGAII_GHComponent) { }


        // Overrides GHComponent attributes to allow mouse double click response: displays the NSGA-II Editor Form with double click
        public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            ((NSGAII_GHComponent)Owner).DisplayForm();
            return GH_ObjectResponse.Handled;
        }
    }
}
