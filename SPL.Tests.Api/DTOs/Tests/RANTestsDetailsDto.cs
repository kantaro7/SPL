using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Tests.Api.DTOs.Tests
{
    public class RANTestsDetailsDto
    {
   
        public List<RANTestsDetailsRADto> RANTestsDetailsRAs { get; set; }
        public List<RANTestsDetailsTADto> RANTestsDetailsTAs { get; set; }
        public DateTime DateTest { get; set; }
      

    }

  
}
