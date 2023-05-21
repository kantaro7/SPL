namespace SPL.WebApp.Domain.DTOs
{
    public class FPCTestsDetailsDTO
    {
        public int Row { get; set; }
        public string WindingA { get; set; }
        public string WindingB { get; set; }
        public string WindingC { get; set; }
        public string WindingD { get; set; }
        public decimal Current { get; set; }
        public decimal Power { get; set; }

        /// <summary>
        /// %FP
        /// </summary>
        public decimal PercentageA { get; set; }
        /// <summary>
        /// %FPTan
        /// </summary>
        public decimal PercentageB { get; set; }
        /// <summary>
        /// %FPCorreccion
        /// </summary>
        public decimal PercentageC { get; set; }

        public decimal Capacitance { get; set; }
        public string Id { get; set; }
    }
}
