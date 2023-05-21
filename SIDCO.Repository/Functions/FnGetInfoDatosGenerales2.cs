using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCO.Infrastructure.Functions
{
    public class FnGetInfoDatosGenerales2
    {
        [Key]
        public string Frec { get; set; }
        public string Altitud { get; set; }
        public string Altitud_F_M { get; set; }
        public string Fases { get; set; }

    }
}
