using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace NSGA_II
{
    public class NSGA_II_Library : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "NSGAII";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "Implements a Multi-Objective Optimization library based on the NSGA-II algorithm.";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("4b78ebbc-5c58-4f13-b67b-3525b33a7f41");
            }
        }

        public override string AuthorName
        {
            get
            {
                return "Sofia Feist";
            }
        }
        public override string AuthorContact
        {
            get
            {
                return "";
            }
        }
    }
}
