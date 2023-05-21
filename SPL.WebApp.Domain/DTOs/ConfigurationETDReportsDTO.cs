namespace SPL.WebApp.Domain.DTOs
{
    public class ConfigurationETDReportsDTO
    {
        public string Proceso { get; set; }
        public string Hoja { get; set; }
        public decimal Orden { get; set; }
        public string ClaveIdioma { get; set; }
        public string Etiqueta { get; set; }
        public decimal? Seccion { get; set; }
        public decimal? Consecutivo { get; set; }
        public string IniEtiqueta { get; set; }
        public string FinEtiqueta { get; set; }
        public string IniDato { get; set; }
        public string FinDato { get; set; }
        public string TipoDato { get; set; }
        public string Formato { get; set; }
        public decimal? CantDatos { get; set; }
        public string Validacion { get; set; }
        public string Campo1 { get; set; }
        public string Tabla1 { get; set; }
        public string Campo2 { get; set; }
        public string Tabla2 { get; set; }
    }

    public class ParamETD
    {
        public ParamETD(string value1)
        {
            this.Tipo = "A";
            this.Value1 = value1;
            this.Value2 = 0;
            this.Decimals = 0;
        }

        public ParamETD(decimal value2, int decim = 0)
        {
            this.Tipo = "N";
            this.Value1 = string.Empty;
            this.Value2 = value2;
            this.Decimals = decim;
        }

        public ParamETD() { }

        public string Tipo { get; set; }
        public string Value1 { get; set; }
        public decimal Value2 { get; set; }
        public int Decimals { get; set; }

        public object GetValue() => this.Tipo.ToUpper().Equals("A") ? this.Value1 : this.Value2;
    }
}
