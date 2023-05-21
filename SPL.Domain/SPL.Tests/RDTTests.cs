using SPL.Domain.SPL.Masters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Tests
{
    public class RDTTests
    {
   
        public decimal AcceptanceValue { get; set; }
        public List<Column> Columns { get; set; }
        public List<string> Positions { get; set; }
        public List<int> OrderPositions { get; set; }
        public List<decimal> Tensions { get; set; }
    
        public string AllTension { get; set; }
        public string UnitTension { get; set; }
        public decimal UnitValue { get; set; }
        public GeneralProperties AngularDisplacement { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }

    }

  
}
