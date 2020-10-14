
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApWebApiGeneratorXml.Models
{
    public class ReveneuLog
    {
        public DateTime TimestampUtc { get; set; }      
        public double? kWh_Forn { get; set; }
        public double? kVARh_Forn { get; set; }
        public double? Ia_mean { get; set; }
        public double? Ib_mean { get; set; }
        public double? Ic_mean { get; set; }
        public double? kWh_FornCP { get; set; }
        public double? kWh_RecCP { get; set; }
        public double? kVARh_FornCP { get; set; }
        public double? kVARh_RecCP { get; set; }
        public double? kVARh_Rec { get; set; }
        public double? Vln_a_mean { get; set; }
        public double? Vln_b_mean { get; set; }
        public double? Vln_c_mean { get; set; }
        public double? kWh_Rec { get; set; }
    }
}
