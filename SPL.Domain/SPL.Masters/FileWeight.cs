using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Masters
{
    public class FileWeight
    {
        public long Id { get; set; }
        public long? ExtensionArchivo { get; set; }
        public string MaximoPeso { get; set; }
        public long IdModulo { get; set; }

        public  FileExtensions ExtensionArchivoNavigation { get; set; }
        
    }
}
