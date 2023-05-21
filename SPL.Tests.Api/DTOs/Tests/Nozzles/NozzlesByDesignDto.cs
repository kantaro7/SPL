using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Tests.Api.DTOs.Tests.Nozzles
{
    public class NozzlesByDesignDto
    {
        public int TotalQuantity { get; set; }
        public List<RecordNozzleInformationDto> NozzleInformation { get; set; }
    }
}
