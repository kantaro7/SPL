namespace SPL.WebApp.Domain.DTOs.ETD
{
    using System.Collections.Generic;

    public class GraphicETD
    {
        public List<decimal[][]> Coords { get; set; }
        public List<decimal> MaxX { get; set; }
        public List<decimal> MaxY { get; set; }
        public List<decimal> MinX { get; set; }
        public List<decimal> MinY { get; set; }
        public int Count { get; set; }
        public List<string> Images { get; set; }
    }
}
