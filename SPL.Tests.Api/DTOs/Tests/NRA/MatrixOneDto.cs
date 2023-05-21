using System.Collections.Generic;


namespace SPL.Tests.Api.DTOs.Tests.NRA
{
    public class MatrixOneDto
    {
        public int Position { get; set; }
        public string TypeInformation { get; set; }
        public string Height { get; set; }
        public List<decimal> Decibels { get; set; }
    }
}
