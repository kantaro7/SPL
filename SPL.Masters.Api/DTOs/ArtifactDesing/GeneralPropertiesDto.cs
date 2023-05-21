namespace SPL.Masters.Api.DTOs.Artifactdesign
{
    using System;

    public class GeneralPropertiesDto
    {
        public GeneralPropertiesDto()
        {

        }
        public GeneralPropertiesDto(string clave, string descripcion)
        {
            this.Clave = clave;
            this.Descripcion = descripcion;
        }

        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public string CreadoPor { get; set; }
        public decimal H_wye { get; set; }
        public decimal X_wye { get; set; }
        public decimal T_wye { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
