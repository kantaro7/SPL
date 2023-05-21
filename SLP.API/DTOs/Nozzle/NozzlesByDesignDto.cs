using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Artifact.Api.DTOs.Nozzles
{
    public class NozzlesByDesignDto
    {
        public int TotalQuantity { get; set; }
        public List<RecordNozzleInformationDto> NozzleInformation { get; set; }
    }
}
