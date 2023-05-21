namespace SPL.WebApp.Domain.DTOs
{
    public class TAPTestsDetailsDTO
    {
        public string WindingEnergized { get; set; }
        public string WindingGrounded { get; set; }
        public decimal VoltageLevel { get; set; }
        public decimal AppliedkV { get; set; }
        public decimal CurrentkV { get; set; }
        public int Time { get; set; }
        public decimal Capacitance { get; set; }
        public decimal AmpCal { get; set; }
        public decimal CurrentPercentage { get; set; }
    }
}
