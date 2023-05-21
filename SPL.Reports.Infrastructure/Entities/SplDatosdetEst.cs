using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Reports.Infrastructure.Entities
{
    public partial class SplDatosdetEst
    {
        public decimal IdReg { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal Seccion { get; set; }
        public decimal KwMedidos { get; set; }
        public decimal AmpMedidos { get; set; }
        public decimal? CabSupRadBco1 { get; set; }
        public string CanalSup1 { get; set; }
        public decimal? CabSupRadBco2 { get; set; }
        public string CanalSup2 { get; set; }
        public decimal? CabSupRadBco3 { get; set; }
        public string CanalSup3 { get; set; }
        public decimal? CabSupRadBco4 { get; set; }
        public string CanalSup4 { get; set; }
        public decimal? CabSupRadBco5 { get; set; }
        public string CanalSup5 { get; set; }
        public decimal PromRadSup { get; set; }
        public decimal? CabInfRadBco1 { get; set; }
        public string CanalInf1 { get; set; }
        public decimal? CabInfRadBco2 { get; set; }
        public string CanalInf2 { get; set; }
        public decimal? CabInfRadBco3 { get; set; }
        public string CanalInf3 { get; set; }
        public decimal? CabInfRadBco4 { get; set; }
        public string CanalInf4 { get; set; }
        public decimal? CabInfRadBco5 { get; set; }
        public string CanalInf5 { get; set; }
        public decimal PromRadInf { get; set; }
        public decimal? Ambiente1 { get; set; }
        public string CanalAmb1 { get; set; }
        public decimal? Ambiente2 { get; set; }
        public string CanalAmb2 { get; set; }
        public decimal? Ambiente3 { get; set; }
        public string CanalAmb3 { get; set; }
        public decimal AmbienteProm { get; set; }
        public decimal TempTapa { get; set; }
        public string CanalTtapa { get; set; }
        public decimal Aor { get; set; }
        public decimal Tor { get; set; }
        public decimal Bor { get; set; }
        public decimal AorCorr { get; set; }
        public decimal TorCorr { get; set; }
        public decimal? TempInstr1 { get; set; }
        public string CanalInst1 { get; set; }
        public decimal? TempInstr2 { get; set; }
        public string CanalInst2 { get; set; }
        public decimal? TempInstr3 { get; set; }
        public string CanalInst3 { get; set; }
        public string VerifVentilador { get; set; }
        public string VerifMotobom { get; set; }
        public decimal? Presion { get; set; }
    }
}
