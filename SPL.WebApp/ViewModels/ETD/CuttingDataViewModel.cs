namespace SPL.WebApp.ViewModels.ETD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    using SPL.WebApp.Domain.DTOs.ETD;

    public class CuttingDataViewModel
    {
        public string NoSerie { get; set; }
        public List<HeaderCuttingDataDTO> HeaderCuttingDatas { get; set; }

        public HeaderCuttingDataDTO HeaderCuttingData { get; set; }

        public SectionCuttingDataDTO SectionCuttingData1 { get; set; }

        public SectionCuttingDataDTO SectionCuttingData2 { get; set; }

        public SectionCuttingDataDTO SectionCuttingData3 { get; set; }

        public DetailCuttingDataDTO DetailCuttingDataDTO { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string CoolingType { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string LastHour { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string FirstCut { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string SecondCut { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string ThirdCut { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal OverElevation { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public DateTime Date { get; set; }

        public decimal AmbProm { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [RegularExpression(@"^(([0-9]{0,4})|([0-9]{0,4}\.[0-9]{1}))$", ErrorMessage = "TOR x Altitud debe ser numérico mayor a cero considerando 4 enteros con 1 decimal")]
        public string TORxAltitud { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [RegularExpression(@"^(([0-9]{0,4})|([0-9]{0,4}\.[0-9]{1}))$", ErrorMessage = "AOR x Altitud debe ser numérico mayor a cero considerando 4 enteros con 1 decimal")]
        public string AORxAltitud { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string PosAT { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string PosBT { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string PosTer { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [RegularExpression(@"^(([0-9]{0,3})|([0-9]{0,3}\.[0-9]{1,3}))$", ErrorMessage = "La capacidad debe ser numérica mayor a cero considerando 3 enteros con 3 decimales")]
        public string Capacidad { get; set; }
        [RegularExpression(@"^(([0-9]{0,4})|([0-9]{0,4}\.[0-9]{1,3}))$", ErrorMessage = "Las pérdidas deben ser numérica mayor a cero considerando 4 enteros con 3 decimales")]
        public string Perdidas { get; set; }
        [RegularExpression(@"^(([0-9]{0,3})|([0-9]{0,3}\.[0-9]{1,3}))$", ErrorMessage = "La corriente debe ser numérico mayor a cero considerando 3 enteros con 3 decimales")]
        public string Corriente { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [RegularExpression(@"^(([0-9]{0,4})|([0-9]{0,4}\.[0-9]{1}))$", ErrorMessage = "La constante debe ser numérica mayor a cero considerando 4 enteros con 1 decimal")]
        public string Constante { get; set; }

        public string Error { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [RegularExpression(@"^(([0-9]{0,4})|([0-9]{0,4}\.[0-9]{1,3}))$", ErrorMessage = "Los kW de Prueba deben ser numérico mayor a cero considerando 4 enteros con 3 decimales")]
        public string Kw_Prueba { get; set; }
        public string Amp_Medidos { get; set; }
        public string Ambiente_prom { get; set; }
        public string Tor_cor { get; set; }
        public string Aor_cor { get; set; }
        public DataTable TableAT { get; set; }
        public DataTable TableBT { get; set; }
        public DataTable TableTER { get; set; }
        public List<string> PositionsAT { get; set; }
        public List<string> PositionsBT { get; set; }
        public List<string> PositionsTER { get; set; }
        public List<OptionsViewModel> CoolingTypes { get; set; }
        public List<OptionsViewModel> LastHours { get; set; }
        public List<OptionsViewModel> FirstCuts { get; set; }
        public List<OptionsViewModel> SecondCuts { get; set; }
        public List<OptionsViewModel> ThirdCuts { get; set; }
        public List<OptionsViewModel> Dates { get; set; }
        public List<StabilizationDataDTO> StabilizationDatas { get; set; }
        public StabilizationDataDTO StabilizationData { get; set; }

        public HeaderCuttingDataDTO TransforInHeader() => new()
        {
            Constante = Convert.ToDecimal(Constante),
            IdReg = StabilizationData.IdReg,
            KwPrueba = Convert.ToDecimal(Kw_Prueba),
            NoSerie = NoSerie,
            PrimerCorte = Convert.ToDateTime(FirstCut),
            SegundoCorte = Convert.ToDateTime(SecondCut),
            TercerCorte = Convert.ToDateTime(ThirdCut),
            SectionCuttingData = new() { SectionCuttingData1, SectionCuttingData2, SectionCuttingData3 },
            UltimaHora = Convert.ToDateTime(LastHour)
        };

        public void InsertResult(HeaderCuttingDataDTO result)
        {
            for (int i = 0; i < result.SectionCuttingData.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        SectionCuttingData1 = result.SectionCuttingData[i];
                        break;
                    case 1:
                        SectionCuttingData2 = result.SectionCuttingData[i];
                        break;
                    case 2:
                        SectionCuttingData3 = result.SectionCuttingData[i];
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
