﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.FPB
{
   public class FPBTestsGeneral
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
        public string KeyTest { get; set; }
        public string TypeUnit { get; set; }
        public int CantBoq { get; set; }
        public string TanDelta { get; set; }
        public string Comment { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }

        public List<FPBTests> Data { get; set; }
    }
}
