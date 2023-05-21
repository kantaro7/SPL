using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Tests.Api.DTOs.Tests.NRA
{
    public class NRATestsDetailsOctDto
    {

        public List<MatrixNRATestsDto> MatrixNRAAntTests { get; set; }
        public List<MatrixNRATestsDto> MatrixNRADesTests { get; set; }
        public List<MatrixNRATestsDto> MatrixNRACoolingTypeTests { get; set; }
        public MatrixNRATestsDto MatrixNRAAmbProm { get; set; }
        public MatrixNRATestsDto MatrixNRAAmbTrans { get; set; }
        public List<MatrixOneDto> MatrizAntesDespuesPura { get; set; }
        public List<MatrixTwoDto> MatrizDeEnfriamientoPura { get; set; }


    }
}
