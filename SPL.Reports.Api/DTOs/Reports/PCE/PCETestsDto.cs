namespace SPL.Reports.Api.DTOs.Reports.PCE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PCETestsDto
    {
        public decimal VoltajeBase { get; set; }
        public string UmVoltajeBase { get; set; } 
        
        public decimal Capacidad { get; set; }
        public string UmCapacidad { get; set; }

        public decimal Frecuencia { get; set; }
        public string UmFrecuencia { get; set; }

        public string PosPruebaAT { get; set; }
        public string PosPruebaBT { get; set; }
        public string PosPruebaTER { get; set; }

        public decimal Temperatura { get; set; }
        public string UmTemperatura { get; set; }

        [Range(0.01, 9999999.999, ErrorMessage = "El campo Garantía Pérdidas debe ser numérico mayor a cero considerando hasta 7 enteros y 2 decimales")]
        public decimal GarPerdidas { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "El campo Unidad de Medida de la garantía pérdidas solo puede tener {0} caracteres")]
        public string UmGarPerdidas { get; set; }

        [Range(0.01, 9999999.999, ErrorMessage = "El campo Garantía de la corriente de excitación debe ser numérico mayor a cero considerando hasta 7 enteros y 2 decimales")]
        public decimal GarCExcitacion { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(15, ErrorMessage = "El campo Unidad de Medida de la garantía de la corriente de excitación solo puede tener {0} caracteres")]
        public string UmGarCExcitacion { get; set; }

        public long IInicio { get; set; }
        public long IFin { get; set; }
        public long Intervalo { get; set; }

        public DateTime Date { get; set; }
     

        public List<PCETestsDetailsDto> PCETestsDetails { get; set; }
    }
}
