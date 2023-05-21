using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCO.Infrastructure.Functions
{
   public  class FnGetInfoReqEsp
    {
        public int Polarity_id { get; set; }
        public string Polarity_other { get; set; }
        public string DESPLAZAMIENTO_ANGULAR { get; set; }
    }
}
