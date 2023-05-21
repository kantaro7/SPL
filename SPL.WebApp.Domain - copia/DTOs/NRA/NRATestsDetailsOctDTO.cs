using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.WebApp.Domain.DTOs.NRA
{
    public class NRATestsDetailsOctDTO
    {
        public List<MatrixNRATestsDTO> MatrixNRAAntTests { get; set; }
        public List<MatrixNRATestsDTO> MatrixNRADesTests { get; set; }
        public List<MatrixNRATestsDTO> MatrixNRACoolingTypeTests { get; set; }
        public MatrixNRATestsDTO MatrixNRAAmbProm { get; set; }
        public MatrixNRATestsDTO MatrixNRAAmbTrans { get; set; }

        public List<MatrixOneDTO>MatrizAntesDespuesPura { get; set; }
        public List<MatrixTwoDTO> MatrizDeEnfriamientoPura { get; set; }
    }
}
