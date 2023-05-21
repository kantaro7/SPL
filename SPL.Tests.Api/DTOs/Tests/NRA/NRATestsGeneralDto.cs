using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Tests.Api.DTOs.Tests.NRA
{
   public class NRATestsGeneralDto
    {
       
        public string KeyTest { get; set; }
        public string Rule { get; set; }
        public string CoolingType { get; set; }
        public string LanguageKey { get; set; }
        public bool InformationLoad { get; set; }
       
        public NRATestsDto Data { get; set; }
    }
}
