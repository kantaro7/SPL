using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.IND
{
    public class INDTestsDetailsDto
    {

        public string NoPage { get; set; }
        public string NoInitPage { get; set; }
        public string NoPagEnd { get; set; }
        public decimal MvaValue { get; set; }
        public decimal KwValue { get; set; }
    }
}
