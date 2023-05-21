using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Tests.Api.DTOs.Tests.FPC
{
    public class ResultFPCTestsDto
    {
        public List<ErrorColumnsDto> results { get; set; }
        public List<FPCTestsDto> FPCTests { get; set; }
 
    }
   

}
