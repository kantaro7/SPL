using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Reports.NRA
{
    public class NRATestsDetailsOct
    {

        public List<MatrixNRATests> MatrixNRAAntTests { get; set; }
        public List<MatrixNRATests> MatrixNRADesTests { get; set; }
        public List<MatrixNRATests> MatrixNRACoolingTypeTests { get; set; }
        public MatrixNRATests MatrixNRAAmbProm { get; set; }
        public MatrixNRATests MatrixNRAAmbTrans { get; set; }

      

    }
}
