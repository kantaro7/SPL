﻿namespace SPL.WebApp.Domain.DTOs
{
    using System;
    using System.Collections.Generic;

    public class RANReportDTO
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
        public string File { get; set; }
        public string TypeReport { get; set; }
        public string KeyTest { get; set; }
        public int NumberMeasurements { get; set; }
        public string Comment { get; set; }

        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }

        public List<RANTestsDetailsDTO> Rans { get; set; }
    }
}
