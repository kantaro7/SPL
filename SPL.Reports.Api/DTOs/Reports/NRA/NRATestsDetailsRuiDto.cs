using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.NRA
{
    public class NRATestsDetailsRuiDto
    {
        public List<MatrixNRATestsDto> MatrixNRAAntTests { get; set; }
        public List<MatrixNRATestsDto> MatrixNRADesTests { get; set; }
        public List<MatrixNRATestsDto> MatrixNRA13Tests { get; set; }
        public List<MatrixNRATestsDto> MatrixNRA23Tests { get; set; }


        public decimal AverageAmb { get; set; }
        public decimal AmbTrans { get; set; }

    }
}
