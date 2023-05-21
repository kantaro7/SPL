using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.NRA
{
   public class NRATestsGeneral
    {

        public long IdLoad { get; set; }
        public DateTime LoadDate { get; set; }
        public int TestNumber { get; set; }
        public string LanguageKey { get; set; }
        public string Customer { get; set; }
        public string Capacity { get; set; }
        public string SerialNumber { get; set; }
        public bool Result { get; set; }
        public string NameFile { get; set; }
        public byte[] File { get; set; }
        public string TypeReport { get; set; }


        public string Comment { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }

        public string KeyTest { get; set; }
        public string Laboratory { get; set; }
        public string Rule { get; set; }
        public string Feeding { get; set; }
        public decimal FeedValue { get; set; }
        public string UmFeeding { get; set; }
        public string CoolingType { get; set; }
        public decimal QtyMeasurements { get; set; }
        public bool LoadInfo { get; set; }
        public DateTime DateData { get; set; }
        public int MediAyd { get; set; }
        public DateTime TestDate { get; set; }




        public string Grades { get; set; }




        public NRATests Data { get; set; }
    }
}
