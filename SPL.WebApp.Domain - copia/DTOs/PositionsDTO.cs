namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class PositionsDTO
    {
        public List<string> AltaTension { get; set; }
        public List<string> BajaTension { get; set; }
        public List<string> Terciario { get; set; }
        public string ATNom { get; set; }
        public string BTNom { get; set; }
        public string TerNom { get; set; }
    }
}
