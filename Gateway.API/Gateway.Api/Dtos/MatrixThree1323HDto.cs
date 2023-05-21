using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Api.Dtos
{
    public class MatrixThree1323HDto
    {
      
        public int Position { get; set; }
        public string TypeInformation { get; set; }
        public string Height { get; set; }

        public decimal Dba { get; set; }
        public decimal CorDba { get; set; }

        public decimal Decibels315 { get; set; }
        public decimal Decibels63 { get; set; }
        public decimal Decibels125 { get; set; }
        public decimal Decibels250 { get; set; }
        public decimal Decibels500 { get; set; }
        public decimal Decibels1000 { get; set; }
        public decimal Decibels2000 { get; set; }
        public decimal Decibels4000 { get; set; }
        public decimal Decibels8000 { get; set; }
        public decimal Decibels10000 { get; set; }


    }
}
