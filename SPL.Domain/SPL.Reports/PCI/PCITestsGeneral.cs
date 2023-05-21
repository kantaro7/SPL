namespace SPL.Domain.SPL.Reports.PCI
{
    using System;
    using System.Collections.Generic;

    public class PCITestGeneral
    {
        public long IdLoad { get; set; }

        public string TypeReport { get; set; }

        public string KeyTest { get; set; }

        public string LanguageKey { get; set; }

        public DateTime LoadDate { get; set; }

        public int TestNumber { get; set; }

        public DateTime Date { get; set; }

        public string Customer { get; set; }

        public string Capacity { get; set; }

        public string SerialNumber { get; set; }

        public bool ReducedCapacityAtLowVoltage { get; set; }

        public bool Autotransformer { get; set; }

        public bool Monofasico { get; set; }

        public string WindingMaterial { get; set; }

        public string Comment { get; set; }

        public decimal Kwcu { get; set; }

        public decimal KwcuMva { get; set; }

        public decimal Kwtot100 { get; set; }

        public decimal Kwtot100M { get; set; }

        public bool Result { get; set; }

        public string NameFile { get; set; }

        public byte[] File { get; set; }

        public string Creadopor { get; set; }

        public DateTime Fechacreacion { get; set; }

        public string Modificadopor { get; set; }

        public DateTime? Fechamodificacion { get; set; }
  
        public List<PCITestRating> Ratings { get; set; } = new List<PCITestRating>();
    }
}