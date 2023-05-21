using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Artifact.ArtifactDesign
{
   public class AllChangingTablesArtifact
    {
        public List<ChangingTablesArtifact> Changetables { get; set; }
        public TapBaan Tapbaan { get; set; }
    }
}
