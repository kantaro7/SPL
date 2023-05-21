using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Tests.Api.DTOs.Tests.Artifactdesign
{
    public class LabTestsArtifactDto
    {
        public string OrderCode { get; set; }
        public string TextTestRoutine { get; set; }
        public string TextTestDielectric { get; set; }
        public string TextTestPrototype { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
