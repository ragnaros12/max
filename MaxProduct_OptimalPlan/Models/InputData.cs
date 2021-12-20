using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaxProduct_OptimalPlan.Models
{
    public class InputData
    {
        public List<SolverRow> products { get; set; }
        public double Al_Constraint { get; set; }
        public double Med_Constraint { get; set; }
        public double Olovo_Constraint { get; set; }
        public double Zink_Constraint { get; set; }
        public double Svin_Constraint { get; set; }
    }
}