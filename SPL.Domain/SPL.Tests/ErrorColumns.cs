using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Tests
{
    public class ErrorColumns
    {
        public int Column { get; set; }
        public int Fila { get; set; }
        public string Message { get; set; }

       public ErrorColumns(int pColumn, int pFila, string pMessage) {
            this.Column = pColumn;
            this.Fila = pFila;
            this.Message = pMessage;
        }
    }
}
