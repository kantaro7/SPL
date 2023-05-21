using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCO.Infrastructure.Functions
{
    public class FnGetInfoDatosGenerales
    {
        public string OrderCode { get; set; }
        public string Cliente { get;set;  }
     
        public long Applicationid {  get;set; }

        public string Aplicacion {  get;set; }

        public long Standardid {  get;set; }

        public string Norma_Aplicable {  get;set; }

        public long Typetrafoid {  get;set; }

        public string Tipo {  get;set; }
    }
}
