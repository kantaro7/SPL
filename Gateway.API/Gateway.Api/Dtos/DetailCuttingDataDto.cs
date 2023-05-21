using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos
{
    public class DetailCuttingDataDto
    {
        public decimal IdCorte { get; set; }
        public decimal Seccion { get; set; }
        public decimal Renglon { get; set; }
        public decimal Tiempo { get; set; }
        public decimal Resistencia { get; set; }
        public decimal TempR { get; set; }
    }
}
