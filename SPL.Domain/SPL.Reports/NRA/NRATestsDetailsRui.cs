using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.NRA
{
    public class NRATestsDetailsRui
    {
        public List<MatrixNRATests> MatrixNRAAntTests { get; set; }
        public List<MatrixNRATests> MatrixNRADesTests { get; set; }
        public List<MatrixNRATests> MatrixNRA13Tests { get; set; }
        public List<MatrixNRATests> MatrixNRA23Tests { get; set; }


        public decimal AverageAmb { get; set; }
        public decimal AmbTrans { get; set; }

    }
}
