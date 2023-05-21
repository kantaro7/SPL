using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplInfoSeccionPci
    {
        #region Properties

        public decimal IdRep { get; set; }

        public decimal Seccion { get; set; }

        public decimal CapPrueba { get; set; }

        public string UmCapPrueba { get; set; }

        public decimal OverElevation { get; set; }

        public decimal Frecuencia { get; set; }

        public string UmFrec { get; set; }

        public decimal Temp { get; set; }

        public string UmTemp { get; set; }

        public decimal TempRespri { get; set; }

        public decimal TempRessec { get; set; }

        public decimal FacCorrPri { get; set; }

        public decimal FacCorrSec { get; set; }

        public decimal FacCorrSee { get; set; }

        #endregion
    }
}
