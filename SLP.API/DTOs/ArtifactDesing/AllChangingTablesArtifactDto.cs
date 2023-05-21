using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPL.Artifact.Api.DTOs.Artifactdesign
{
    public class AllChangingTablesArtifactDto
    {
        public List<ChangingTablesArtifactDto> Changetables { get; set; }
        public TapBaanDto Tapbaan { get; set; }
    }
}
