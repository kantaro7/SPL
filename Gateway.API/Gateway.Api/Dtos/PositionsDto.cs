namespace Gateway.Api.Dtos
{
    using System.Collections.Generic;

    public class PositionsDto
    {
        public List<string> AltaTension { get; set; }
        public List<string> BajaTension { get; set; }
        public List<string> Terciario { get; set; }
        public string ATNom { get; set; }
        public string BTNom { get; set; }
        public string TerNom { get; set; }
    }
}
