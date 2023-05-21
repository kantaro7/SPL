using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIDCO.Domain.Artifactdesign
{
    public class GeneralProperties
    {
        
        public decimal Id { get; set; }
        public string Description { get; set; }
        public decimal? AttributeId { get; set; }
        public decimal? ParentId { get; set; }
        public string AparentId { get; set; }
    }
}
