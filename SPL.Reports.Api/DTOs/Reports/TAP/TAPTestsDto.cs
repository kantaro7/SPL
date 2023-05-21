using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.TAP
{
    public class TAPTestsDto
    {
     

        public decimal Freacuency { get; set; }
        public int ValueAcep { get; set; }
        public List<TAPTestsDetailsDto> TAPTestsDetails { get; set; }

      

    }
}
