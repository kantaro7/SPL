using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos
{
    public class HeadboardReportsDto
    {
        public string Client { get; set; }
        public string Capacity { get; set; }
        public string NoSerie { get; set; }
        public string ParallelSeriesTitle { get; set; }
        public string NoteFc { get; set; }
    }
}
