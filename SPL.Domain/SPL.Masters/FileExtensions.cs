using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Masters
{
    public class FileExtensions
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public long TipoArchivo { get; set; }
        public bool Active { get; set; }

    }
}
