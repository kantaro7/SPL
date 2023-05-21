using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.WebApp.Domain.DTOs
{
    public class CheckInfoRODDTO
    {
        public string Error { get; set; }
         public string Message { get; set; }

        public string ConexionPrueba { get; set; }
        public string ClavePrueba { get; set; }
    }
}
