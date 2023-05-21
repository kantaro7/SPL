using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos.Reports.CGD
{
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
