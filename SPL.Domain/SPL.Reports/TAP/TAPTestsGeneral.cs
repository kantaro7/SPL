using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.TAP
{
   public class TAPTestsGeneral
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

 

        public string UnitType { get; set; }
        public int NoConnectionsAT { get; set; }
        public int NoConnectionsBT { get; set; }
        public int NoConnectionsTER { get; set; }
        public string IdCapAT { get; set; }
        public string IdCapBT { get; set; }
        public string IdCapTER { get; set; }
      
        public long IdRepFPC { get; set; }
   





        public string Comment { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }


        public TAPTests TAPTests { get; set; }

    }
}
