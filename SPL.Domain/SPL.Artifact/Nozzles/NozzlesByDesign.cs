using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Artifact.Nozzles
{
    public class NozzlesByDesign
    {
        public int TotalQuantity { get; set; }
        public List<RecordNozzleInformation> NozzleInformation { get; set; }
    }
}
