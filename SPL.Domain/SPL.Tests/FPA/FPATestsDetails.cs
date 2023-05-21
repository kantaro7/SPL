using SPL.Domain.SPL.Artifact.Nozzles;
using SPL.Domain.SPL.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Tests.FPA
{
    public class FPATestsDetails
    {
        public List<FPAPowerFactor> FPAPowerFactor { get; set; }
        public List<FPADielectricStrength> FPADielectricStrength { get; set; }
        public FPAWaterContent FPAWaterContent { get; set; }
        public FPAGasContent FPAGasContent { get; set; }

       

    }
}
