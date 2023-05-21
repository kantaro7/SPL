using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Masters
{
   public  class GeneralProperties
    {
        public GeneralProperties(string clave, string descripcion)
        {
            Clave = clave;
            Descripcion = descripcion;
        }
        public GeneralProperties(string clave, string descripcion, decimal h_wye, decimal x_wye, decimal t_wye)
        {
            Clave = clave;
            Descripcion = descripcion;
            H_wye = h_wye;
            X_wye = x_wye;
            T_wye = t_wye;
        }

        public GeneralProperties()
        {

        }

        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public decimal H_wye { get; set; }
        public decimal X_wye { get; set; }
        public decimal T_wye { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
