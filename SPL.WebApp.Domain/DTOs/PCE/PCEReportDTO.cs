namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class PCEReportDTO
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
        public string AngularDisplacement { get; set; }
        public string Rule { get; set; }
        public string PostTestBt { get; set; }
        public string Ter { get; set; }
        public string Comment { get; set; }
        public decimal Connection_sp { get; set; }
        public string Pos_at { get; set; }
        public DateTime Date { get; set; }
        public RODTestsDTO DataTests { get; set; }
    }
}
