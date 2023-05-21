using System.Collections.Generic;

namespace Gateway.Api.Dtos
{
    public class MatrixTwoPromDto
    {
        #region Properties
        
        public int Position { get; set; }
        
        public string TypeInformation { get; set; }
        
        public string Height { get; set; }
        
        public List<double> Decibels { get; set; }
        
        public double SumRealDecibels { get; set; }

        #endregion
    }
}