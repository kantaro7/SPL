namespace SPL.WebApp.Domain.DTOs
{
    public class CeldasValidate
    {
        public CeldasValidate(string idioma)
        {
            this.DevanadoEnergizado = idioma == "EN" ? "HV" : "AT";
            this.DevanadoInducido = idioma == "EN" ? "LV" : "BT";
        }

        public string CeldaAT { get; set; }
        public string CeldaBT { get; set; }
        public string CeldaTer { get; set; }
        public string CeldaDevanadoEnergizado { get; set; }
        public string CeldaDevanadoInducido { get; set; }
        public string DevanadoEnergizado { get; set; }
        public string DevanadoInducido { get; set; }
        public string FrecuenciaPrueba { get; set; }
        public string RelTension { get; set; }
        public string CeldaTiempo { get; set; }
    }
}
