namespace Gateway.Api.Dtos.Reports.TAP
{
    public class TAPBodyDto
    {
        public string WindingEnergized { get; set; }
        public string WindingGrounded { get; set; }
        public decimal LevelVoltage { get; set; }
    }
}
