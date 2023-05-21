using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.TIN
{
   public class TINTestsGeneral
    {
        public long IdLoad { get; set; }
        public DateTime LoadDate { get; set; }
        public DateTime Date { get; set; }
        public int TestNumber { get; set; }
        public string LanguageKey { get; set; }
        public string Customer { get; set; }
        public string Capacity { get; set; }
        public string SerialNumber { get; set; }
        public bool Result { get; set; }
        public string NameFile { get; set; }
        public byte[] File { get; set; }
        public string TypeReport { get; set; }
        public string KeyTest { get; set; }



        public string Connection { get; set; }
        public decimal? Voltage { get; set; }
        public string RelVoltage { get; set; }
        public string PosAT { get; set; }
        public string PosBT { get; set; }
        public string PosTER { get; set; }
        public decimal Frecuency { get; set; }
        public int Time { get; set; }
        public string EnergizedWinding { get; set; }
        public string InducedWinding { get; set; }
        public decimal AppliedVoltage { get; set; }
        public decimal InducedVoltage { get; set; }
        public string Grades { get; set; }

      






        public string Comment { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }


    }
}
