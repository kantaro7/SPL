using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.WebApp.Domain.DTOs.NRA
{
    public class NRATestsDetailsRuiDTO
    {
        public List<MatrixNRATestsDTO> MatrixNRAAntTests { get; set; }
        public List<MatrixNRATestsDTO> MatrixNRADesTests { get; set; }
        public List<MatrixNRATestsDTO> MatrixNRA13Tests { get; set; }
        public List<MatrixNRATestsDTO> MatrixNRA23Tests { get; set; }


        public decimal AverageAmb { get; set; }
        public decimal AmbTrans { get; set; }
    }
}
