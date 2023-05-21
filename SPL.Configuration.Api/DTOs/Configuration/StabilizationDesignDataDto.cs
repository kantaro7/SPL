namespace SPL.Configuration.Api.DTOs.Configuration
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class StabilizationDesignDataDto
    {

        public int Id { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(47, ErrorMessage = "El campo Número de serie solo puede tener {0} caracteres")]
        public string NoSerie { get; set; }

        [Range(0, 99, ErrorMessage = "El valor límite de Top Oil Rise debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal TorLimite { get; set; }

        [Range(0, 99.9, ErrorMessage = "El valor de la hoja de enfriamiento de Top Oil Rise debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        public decimal TorHenf { get; set; }

        [Range(0, 99, ErrorMessage = "El valor límite de Average Oil Rise debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? AorLimite { get; set; }

        [Range(0, 99.9, ErrorMessage = "El valor de la hoja de enfriamiento de Average Oil Rise debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        public decimal AorHenf { get; set; }

        [Range(0.01, 99.99, ErrorMessage = "El valor límite de Gradiente para alta tensión debe ser numérico mayor a cero considerando 2 enteros con 2 decimales")]
        public decimal? GradienteLimAt { get; set; }

        [Range(0, 99.99, ErrorMessage = "El valor de la hoja de enfriamiento de Gradiente para alta tensión debe ser numérico mayor a cero considerando 2 enteros con 2 decimales")]
        public decimal? GradienteHentAt { get; set; }

        [Range(0, 99.99, ErrorMessage = "El valor límite de Gradiente para baja tensión debe ser numérico mayor a cero considerando 2 enteros con 2 decimales")]
        public decimal? GradienteLimBt { get; set; }

        [Range(0, 99.99, ErrorMessage = "El valor de la hoja de enfriamiento de Gradiente para baja tensión debe ser numérico mayor a cero considerando 2 enteros con 2 decimales")]
        public decimal? GradienteHentBt { get; set; }

        [Range(0, 99.99, ErrorMessage = "El valor límite de Gradiente para terciario debe ser numérico mayor a cero considerando 2 enteros con 2 decimales")]
        public decimal? GradienteLimTer { get; set; }

        [Range(0, 99.99, ErrorMessage = "El valor de la hoja de enfriamiento de Gradiente para terciario debe ser numérico mayor a cero considerando 2 enteros con 2 decimales")]
        public decimal? GradienteHentTer { get; set; }

        [Range(0, 99, ErrorMessage = "El valor límite de Average Winding Rise para alta tensión debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? AwrLimAt { get; set; }

        [Range(0, 99.9, ErrorMessage = "El valor de la hoja de enfriamiento de Average Winding Rise para alta tensión debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        public decimal? AwrHenfAt { get; set; }

        [Range(0, 99, ErrorMessage = "El valor límite de Average Winding Rise para baja tensión debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? AwrLimBt { get; set; }

        [Range(0, 99.9, ErrorMessage = "El valor de la hoja de enfriamiento de Average Winding Rise para baja tensión debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        public decimal? AwrHenfBt { get; set; }

        [Range(0, 99, ErrorMessage = "El valor límite de Average Winding Rise para terciario debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal? AwrLimTer { get; set; }

        [Range(0, 99.9, ErrorMessage = "El valor de la hoja de enfriamiento de Average Winding Rise para terciario debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        public decimal? AwrHenfTer { get; set; }

        [Range(0, 999, ErrorMessage = "El valor límite de Hottest Spot Rise para alta tensión debe ser numérico mayor a cero considerando 3 enteros sin decimales")]
        public decimal? HsrLimAt { get; set; }

        [Range(0, 999.9, ErrorMessage = "El valor de la hoja de enfriamiento de Hottest Spot Rise para alta tensión debe ser numérico mayor a cero considerando 3 enteros con 1 decimal")]
        public decimal? HsrHenfAt { get; set; }

        [Range(0, 999, ErrorMessage = "El valor límite de Hottest Spot Rise para baja tensión debe ser numérico mayor a cero considerando 3 enteros sin decimales")]
        public decimal? HsrLimBt { get; set; }

        [Range(0, 999.9, ErrorMessage = "El valor de la hoja de enfriamiento de Hottest Spot Rise para baja tensión debe ser numérico mayor a cero considerando 3 enteros con 1 decimal")]
        public decimal? HsrHenfBt { get; set; }

        [Range(0, 999, ErrorMessage = "El valor límite de Hottest Spot Rise para terciario debe ser numérico mayor a cero considerando 3 enteros sin decimales")]
        public decimal? HsrLimTer { get; set; }

        [Range(0, 999.9, ErrorMessage = "El valor de la hoja de enfriamiento de Hottest Spot Rise para terciario debe ser numérico mayor a cero considerando 3 enteros con 1 decimal")]
        public decimal? HsrHenfTer { get; set; }

        [Required(ErrorMessage = "El campo CteTiempoTrafo es requerido")]
        [Range(0, 99.9, ErrorMessage = "Constante tiempo trafo en horas debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        public decimal CteTiempoTrafo { get; set; }

        [Required(ErrorMessage = "El campo AmbienteCte es requerido")]
        [Range(0, 99, ErrorMessage = "Ambiente constante en horas debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal AmbienteCte { get; set; }

        [Required(ErrorMessage = "El campo Bor es requerido")]
        [Range(0, 99.9, ErrorMessage = "Bottom Oil Rise debe ser numérico mayor a cero considerando 2 enteros con 1 decimal")]
        public decimal Bor { get; set; }

        [Required(ErrorMessage = "El campo KwDiseno es requerido")]
        [Range(0, 9999.999, ErrorMessage = "kW Diseño debe ser numérico mayor a cero considerando 4 enteros con 3 decimales")]
        public decimal KwDiseno { get; set; }

        [Required(ErrorMessage = "El campo Toi es requerido")]
        [Range(0, 99, ErrorMessage = "TOI debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal Toi { get; set; }

        [Required(ErrorMessage = "El campo Toi es requerido")]
        [Range(0, 99, ErrorMessage = "Límite TOI debe ser numérico mayor a cero considerando 2 enteros sin decimales")]
        public decimal ToiLimite { get; set; }

        [Required(ErrorMessage = "El campo Creadopor es requerido")]
        public string Creadopor { get; set; }

        [Required(ErrorMessage = "El campo Fechacreacion es requerido")]
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
