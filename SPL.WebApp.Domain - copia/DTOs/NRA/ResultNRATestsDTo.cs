using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.WebApp.Domain.DTOs.NRA
{
    public class ResultNRATestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public NRATestsGeneralDTO NRATestsGeneral { get; set; }
    }
}
