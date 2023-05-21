namespace SPL.WebApp.ViewModels.ETD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using SPL.WebApp.Domain.DTOs.ETD;

    public class StabilizationDetailsDataViewModel
    {
        public StabilizationDetailsDataViewModel()
        {
            this.IdReg = new();
            this.FechaHora = new();
            this.KwMedidos = new();
            this.AmpMedidos = new();
            this.CabSupRadBco1 = new();
            this.CanalSup1 = new();
            this.CabSupRadBco2 = new();
            this.CanalSup2 = new();
            this.CabSupRadBco3 = new();
            this.CanalSup3 = new();
            this.CabSupRadBco4 = new();
            this.CanalSup4 = new();
            this.CabSupRadBco5 = new();
            this.CanalSup5 = new();
            this.CabSupRadBco6 = new();
            this.CanalSup6 = new();
            this.CabSupRadBco7 = new();
            this.CanalSup7 = new();
            this.CabSupRadBco8 = new();
            this.CanalSup8 = new();
            this.CabSupRadBco9 = new();
            this.CanalSup9 = new();
            this.CabSupRadBco10 = new();
            this.CanalSup10 = new();
            this.PromRadSup = new();
            this.CabInfRadBco1 = new();
            this.CanalInf1 = new();
            this.CabInfRadBco2 = new();
            this.CanalInf2 = new();
            this.CabInfRadBco3 = new();
            this.CanalInf3 = new();
            this.CabInfRadBco4 = new();
            this.CanalInf4 = new();
            this.CabInfRadBco5 = new();
            this.CanalInf5 = new();
            this.CabInfRadBco6 = new();
            this.CanalInf6 = new();
            this.CabInfRadBco7 = new();
            this.CanalInf7 = new();
            this.CabInfRadBco8 = new();
            this.CanalInf8 = new();
            this.CabInfRadBco9 = new();
            this.CanalInf9 = new();
            this.CabInfRadBco10 = new();
            this.CanalInf10 = new();
            this.PromRadInf = new();
            this.Ambiente1 = new();
            this.CanalAmb1 = new();
            this.Ambiente2 = new();
            this.CanalAmb2 = new();
            this.Ambiente3 = new();
            this.CanalAmb3 = new();
            this.AmbienteProm = new();
            this.TempTapa = new();
            this.CanalTtapa = new();
            this.Aor = new();
            this.Tor = new();
            this.Bor = new();
            this.AorCorr = new();
            this.TorCorr = new();
            this.Ao = new();
            this.TempInstr1 = new();
            this.CanalInst1 = new();
            this.TempInstr2 = new();
            this.CanalInst2 = new();
            this.TempInstr3 = new();
            this.CanalInst3 = new();
            this.VerifVent1 = new();
            this.VerifVent2 = new();
            this.Presion = new();
            this.Estable = new();
        }

        public List<decimal?> IdReg { get; set; }
        public List<DateTime?> FechaHora { get; set; }
        public List<decimal?> KwMedidos { get; set; }
        public List<decimal?> AmpMedidos { get; set; }
        public List<decimal?> CabSupRadBco1 { get; set; }
        public decimal? CanalSup1 { get; set; }
        public List<decimal?> CabSupRadBco2 { get; set; }
        public decimal? CanalSup2 { get; set; }
        public List<decimal?> CabSupRadBco3 { get; set; }
        public decimal? CanalSup3 { get; set; }
        public List<decimal?> CabSupRadBco4 { get; set; }
        public decimal? CanalSup4 { get; set; }
        public List<decimal?> CabSupRadBco5 { get; set; }
        public decimal? CanalSup5 { get; set; }
        public List<decimal?> CabSupRadBco6 { get; set; }
        public decimal? CanalSup6 { get; set; }
        public List<decimal?> CabSupRadBco7 { get; set; }
        public decimal? CanalSup7 { get; set; }
        public List<decimal?> CabSupRadBco8 { get; set; }
        public decimal? CanalSup8 { get; set; }
        public List<decimal?> CabSupRadBco9 { get; set; }
        public decimal? CanalSup9 { get; set; }
        public List<decimal?> CabSupRadBco10 { get; set; }
        public decimal? CanalSup10 { get; set; }
        public List<decimal?> PromRadSup { get; set; }
        public List<decimal?> CabInfRadBco1 { get; set; }
        public decimal? CanalInf1 { get; set; }
        public List<decimal?> CabInfRadBco2 { get; set; }
        public decimal? CanalInf2 { get; set; }
        public List<decimal?> CabInfRadBco3 { get; set; }
        public decimal? CanalInf3 { get; set; }
        public List<decimal?> CabInfRadBco4 { get; set; }
        public decimal? CanalInf4 { get; set; }
        public List<decimal?> CabInfRadBco5 { get; set; }
        public decimal? CanalInf5 { get; set; }
        public List<decimal?> CabInfRadBco6 { get; set; }
        public decimal? CanalInf6 { get; set; }
        public List<decimal?> CabInfRadBco7 { get; set; }
        public decimal? CanalInf7 { get; set; }
        public List<decimal?> CabInfRadBco8 { get; set; }
        public decimal? CanalInf8 { get; set; }
        public List<decimal?> CabInfRadBco9 { get; set; }
        public decimal? CanalInf9 { get; set; }
        public List<decimal?> CabInfRadBco10 { get; set; }
        public decimal? CanalInf10 { get; set; }
        public List<decimal?> PromRadInf { get; set; }
        public List<decimal?> Ambiente1 { get; set; }
        public decimal? CanalAmb1 { get; set; }
        public List<decimal?> Ambiente2 { get; set; }
        public decimal? CanalAmb2 { get; set; }
        public List<decimal?> Ambiente3 { get; set; }
        public decimal? CanalAmb3 { get; set; }
        public List<decimal?> AmbienteProm { get; set; }
        public List<decimal?> TempTapa { get; set; }
        public decimal? CanalTtapa { get; set; }
        public List<decimal?> Aor { get; set; }
        public List<decimal?> Tor { get; set; }
        public List<decimal?> Bor { get; set; }
        public List<decimal?> AorCorr { get; set; }
        public List<decimal?> TorCorr { get; set; }
        public List<decimal?> Ao { get; set; }
        public List<decimal?> TempInstr1 { get; set; }
        public decimal? CanalInst1 { get; set; }
        public List<decimal?> TempInstr2 { get; set; }
        public decimal? CanalInst2 { get; set; }
        public List<decimal?> TempInstr3 { get; set; }
        public decimal? CanalInst3 { get; set; }
        public List<bool?> VerifVent1 { get; set; }
        public List<bool?> VerifVent2 { get; set; }
        public List<decimal?> Presion { get; set; }
        public List<bool?> Estable { get; set; }
    }
}
