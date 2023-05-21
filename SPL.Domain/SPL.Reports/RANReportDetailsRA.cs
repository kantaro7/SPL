using System;

namespace SPL.Domain.SPL.Reports
{
    public class RANReportDetailsRA
    {
        public int Section { get; set; }
        public DateTime DateTest { get; set; }
        public string Description { get; set; }
        public decimal Measurement { get; set; }
        public string UMMeasurement { get; set; }
        public decimal VCD { get; set; }
        public decimal Limit { get; set; }
        public string Duration { get; set; }
        public decimal Time { get; set; }
        public string UMTime { get; set; }
    }
}