using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;


namespace NSGA_II
{
    class GH_Component_Attributes : GH_ComponentAttributes
    {
        public GH_Component_Attributes(IGH_Component NSGAII_GHComponent) : base(NSGAII_GHComponent) { }


        // Overrides the GHComponent attributes to allow mouse double click response: displays the NSGA-II Editor Form with double click
        public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            ((NSGAII_GHComponent)Owner).DisplayEditor();
            return GH_ObjectResponse.Handled;
        }
    }
}
