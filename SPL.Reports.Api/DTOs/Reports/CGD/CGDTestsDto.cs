namespace SPL.Reports.Api.DTOs.Reports.CGD
{
    using System;
    using System.Collections.Generic;

    public class CGDTestsDto
    {
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public string KeyTest { get; set; }
        public string OilType { get; set; }
        public decimal ValAcceptCg { get; set; }
        public string Method { get; set; }
        public string Result { get; set; }
        public string Grades { get; set; }
        public List<CGDTestsDetailsDto> CGDTestsDetails { get; set; }
    }
}
