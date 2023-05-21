namespace SPL.WebApp.Domain.DTOs
{
    public class PirPruebasDTO
    {
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
        public TensionesDTO Tensiones { get; set; }
    }

    public class TensionesDTO
    {
        public decimal? Tension1 { get; set; }
        public decimal? Tension2 { get; set; }
    } 
}
