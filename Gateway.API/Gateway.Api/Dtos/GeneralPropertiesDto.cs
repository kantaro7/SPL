using System;

namespace Gateway.Api.Dtos
{
    public class GeneralPropertiesDto
    {
        public GeneralPropertiesDto()
        {

        }
        public GeneralPropertiesDto(string clave, string descripcion)
        {
            Clave = clave;
            Descripcion = descripcion;
        }

        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
