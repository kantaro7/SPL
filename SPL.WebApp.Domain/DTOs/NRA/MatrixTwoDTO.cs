using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.WebApp.Domain.DTOs.NRA
{
    public class MatrixTwoDTO
    {
        public int Position { get; set; }
        public string TypeInformation { get; set; }
        public string Height { get; set; }
        public List<decimal> Decibels { get; set; }
        public decimal SumRealDecibels { get; set; }
    }
}
