using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Tests
{
    public class RADTests
    {
   
        public decimal AcceptanceValue { get; set; }
        public List<HeaderRADTests> Headers { get; set; }
        public List<Column> Columns { get; set; }
        public List<Column> Columns2 { get; set; }
        public List<string> Times { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }

    }

  
}
