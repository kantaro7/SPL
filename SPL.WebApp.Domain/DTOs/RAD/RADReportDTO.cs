namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class RADReportDTO
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
        public string TypeUnit { get; set; }
        public string ThirdWindingType { get; set; }
        public string Comment { get; set; }
        public RADTestsDTO DataTests { get; set; }
    }
}
