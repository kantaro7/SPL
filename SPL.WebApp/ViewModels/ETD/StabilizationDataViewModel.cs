namespace SPL.WebApp.ViewModels.ETD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs.ETD;

    public class StabilizationDataViewModel
    {
        public StabilizationDataViewModel()
        {
        }
        #region a
        public StabilizationDataViewModel(StabilizationDataDTO stabilizationDataDTO)
        {
            AltitudeF1 = stabilizationDataDTO.AltitudeF1;
            AltitudeF2 = stabilizationDataDTO.AltitudeF2;
            CantEstables = stabilizationDataDTO.CantEstables;
            CantInestables = stabilizationDataDTO.CantInestables;
            CantTermoPares = stabilizationDataDTO.CantTermoPares;
            Capacidad = stabilizationDataDTO.Capacidad;
            CoolingType = stabilizationDataDTO.CoolingType;
            Corriente = stabilizationDataDTO.Corriente;
            Creadopor = stabilizationDataDTO.Creadopor;
            DevanadoSplit = stabilizationDataDTO.DevanadoSplit;
            Estatus = stabilizationDataDTO.Estatus;
            FactAlt = stabilizationDataDTO.FactAlt;
            FactEnf = stabilizationDataDTO.FactEnf;
            Fechacreacion = stabilizationDataDTO.Fechacreacion;
            FechaDatos = stabilizationDataDTO.FechaDatos;
            Fechamodificacion = stabilizationDataDTO.Fechamodificacion;
            IdReg = stabilizationDataDTO.IdReg;
            Intervalo = stabilizationDataDTO.Intervalo;
            Modificadopor = stabilizationDataDTO.Modificadopor;
            NoSerie = stabilizationDataDTO.NoSerie;
            OverElevation = stabilizationDataDTO.OverElevation;
            Perdidas = stabilizationDataDTO.Perdidas;
            PorcCarga = stabilizationDataDTO.PorcCarga;
            PosAt = stabilizationDataDTO.PosAt;
            PosBt = stabilizationDataDTO.PosBt;
            PosTer = stabilizationDataDTO.PosTer;
            Sobrecarga = stabilizationDataDTO.Sobrecarga;
            UmIntervalo = stabilizationDataDTO.UmIntervalo;
            StabilizationDataDetails = new StabilizationDetailsDataViewModel()
            {
                Ambiente1 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.Ambiente1),
                Ambiente2 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.Ambiente2),
                Ambiente3 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.Ambiente3),
                AmbienteProm = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<decimal?>(x => x.AmbienteProm),
                AmpMedidos = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<decimal?>(x => x.AmpMedidos),
                Ao = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<decimal?>(x => x.Ao),
                Aor = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<decimal?>(x => x.Aor),
                AorCorr = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<decimal?>(x => x.AorCorr),
                Bor = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<decimal?>(x => x.Bor),
                CabInfRadBco1 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabInfRadBco1),
                CabInfRadBco2 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabInfRadBco2),
                CabInfRadBco3 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabInfRadBco3),
                CabInfRadBco4 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabInfRadBco4),
                CabInfRadBco5 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabInfRadBco5),
                CabInfRadBco6 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabInfRadBco6),
                CabInfRadBco7 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabInfRadBco7),
                CabInfRadBco8 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabInfRadBco7),
                CabInfRadBco9 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabInfRadBco9),
                CabInfRadBco10 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabInfRadBco10),
                CabSupRadBco1 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabSupRadBco1),
                CabSupRadBco2 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabSupRadBco2),
                CabSupRadBco3 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabSupRadBco3),
                CabSupRadBco4 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabSupRadBco4),
                CabSupRadBco5 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabSupRadBco5),
                CabSupRadBco6 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabSupRadBco6),
                CabSupRadBco7 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabSupRadBco7),
                CabSupRadBco8 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabSupRadBco8),
                CabSupRadBco9 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabSupRadBco9),
                CabSupRadBco10 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.CabSupRadBco10),
                CanalAmb1 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalAmb1,
                CanalAmb2 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalAmb2,
                CanalAmb3 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalAmb3,
                CanalInf1 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalInf1,
                CanalInf2 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalInf2,
                CanalInf3 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalInf3,
                CanalInf4 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalInf4,
                CanalInf5 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalInf5,
                CanalInf6 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalInf6,
                CanalInf7 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalInf7,
                CanalInf8 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalInf8,
                CanalInf9 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalInf9,
                CanalInf10 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalInf10,
                CanalInst1 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalInst1,
                CanalInst2 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalInst2,
                CanalInst3 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalInst3,
                CanalSup1 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalSup1,
                CanalSup2 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalSup2,
                CanalSup3 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalSup3,
                CanalSup4 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalSup4,
                CanalSup5 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalSup5,
                CanalSup6 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalSup6,
                CanalSup7 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalSup7,
                CanalSup8 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalSup8,
                CanalSup9 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalSup9,
                CanalSup10 = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalSup10,
                CanalTtapa = stabilizationDataDTO.StabilizationDataDetails.FirstOrDefault().CanalTtapa,
                Estable = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<bool?>(x => x.Estable),
                FechaHora = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<DateTime?>(x => x.FechaHora),
                IdReg = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<decimal?>(x => x.IdReg),
                KwMedidos = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<decimal?>(x => x.KwMedidos),
                Presion = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.Presion),
                PromRadInf = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<decimal?>(x => x.PromRadInf),
                PromRadSup = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<decimal?>(x => x.PromRadSup),
                TempInstr1 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.TempInstr1),
                TempInstr2 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.TempInstr2),
                TempInstr3 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll(x => x.TempInstr3),
                TempTapa = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<decimal?>(x => x.TempTapa),
                Tor = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<decimal?>(x => x.Tor),
                TorCorr = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<decimal?>(x => x.TorCorr),
                VerifVent1 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<bool?>(x => x.VerifVent1),
                VerifVent2 = stabilizationDataDTO.StabilizationDataDetails.ConvertAll<bool?>(x => x.VerifVent2)
            };
        }

        public StabilizationDataDTO GetStabilizationDataDTO()
        {
            StabilizationDataDTO aux = new()
            {
                AltitudeF1 = AltitudeF1 != null ? (decimal)AltitudeF1 : 0,
                AltitudeF2 = AltitudeF2,
                CantEstables = CantEstables != null ? (int)CantEstables : 0,
                CantInestables = CantInestables != null ? (int)CantInestables : 0,
                CantTermoPares = CantTermoPares != null ? (decimal)CantTermoPares : 0,
                Capacidad = Capacidad != null ? (decimal)Capacidad : 0,
                CoolingType = CoolingType,
                Corriente = Corriente,
                Creadopor = Creadopor,
                DevanadoSplit = DevanadoSplit,
                Estatus = Estatus != null && (bool)Estatus,
                FactAlt = FactAlt != null ? (decimal)FactAlt : 0,
                FactEnf = FactEnf != null ? (decimal)FactEnf : 0,
                Fechacreacion = Fechacreacion != null ? (DateTime)Fechacreacion : DateTime.Now,
                FechaDatos = FechaDatos != null ? (DateTime)FechaDatos : DateTime.Now,
                Fechamodificacion = Fechamodificacion,
                IdReg = IdReg != null ? (int)IdReg : 0,
                Intervalo = Intervalo != null ? (decimal)Intervalo : 0,
                Modificadopor = Modificadopor,
                NoSerie = NoSerie,
                OverElevation = OverElevation != null ? (decimal)OverElevation : 0,
                Perdidas = Perdidas,
                PorcCarga = PorcCarga != null ? (decimal)PorcCarga : 0,
                PosAt = PosAt,
                PosBt = PosBt,
                PosTer = PosTer,
                Sobrecarga = Sobrecarga,
                UmIntervalo = UmIntervalo,
                StabilizationDataDetails = new List<StabilizationDetailsDataDTO>()
            };

            for (int i = 0; i < StabilizationDataDetails.KwMedidos.Count(x => x is not null and not 0); i++)
            {
                aux.StabilizationDataDetails.Add(new StabilizationDetailsDataDTO
                {
                    Ambiente1 = StabilizationDataDetails.Ambiente1.ElementAtOrDefault(i) != null ? StabilizationDataDetails.Ambiente1[i] : null,
                    Ambiente2 = StabilizationDataDetails.Ambiente2.ElementAtOrDefault(i) != null ? StabilizationDataDetails.Ambiente2[i] : null,
                    Ambiente3 = StabilizationDataDetails.Ambiente3.ElementAtOrDefault(i) != null ? StabilizationDataDetails.Ambiente3[i] : null,
                    AmbienteProm = StabilizationDataDetails.AmbienteProm.ElementAtOrDefault(i) != null && StabilizationDataDetails.AmbienteProm[i] != null ? (decimal)StabilizationDataDetails.AmbienteProm[i] : 0,
                    AmpMedidos = StabilizationDataDetails.AmbienteProm.ElementAtOrDefault(i) != null && StabilizationDataDetails.AmpMedidos[i] != null ? (decimal)StabilizationDataDetails.AmpMedidos[i] : 0,
                    Ao = StabilizationDataDetails.Ao.ElementAtOrDefault(i) != null && StabilizationDataDetails.Ao[i] != null ? (decimal)StabilizationDataDetails.Ao[i] : 0,
                    Aor = StabilizationDataDetails.Aor.ElementAtOrDefault(i) != null && StabilizationDataDetails.Aor[i] != null ? (decimal)StabilizationDataDetails.Aor[i] : 0,
                    AorCorr = StabilizationDataDetails.AorCorr.ElementAtOrDefault(i) != null && StabilizationDataDetails.AorCorr[i] != null ? (decimal)StabilizationDataDetails.AorCorr[i] : 0,
                    Bor = StabilizationDataDetails.Bor.ElementAtOrDefault(i) != null && StabilizationDataDetails.Bor[i] != null ? (decimal)StabilizationDataDetails.Bor[i] : 0,
                    CabInfRadBco1 = StabilizationDataDetails.CabInfRadBco1.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabInfRadBco1[i] : null,
                    CabInfRadBco2 = StabilizationDataDetails.CabInfRadBco2.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabInfRadBco2[i] : null,
                    CabInfRadBco3 = StabilizationDataDetails.CabInfRadBco3.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabInfRadBco3[i] : null,
                    CabInfRadBco4 = StabilizationDataDetails.CabInfRadBco4.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabInfRadBco4[i] : null,
                    CabInfRadBco5 = StabilizationDataDetails.CabInfRadBco5.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabInfRadBco5[i] : null,
                    CabInfRadBco6 = StabilizationDataDetails.CabInfRadBco6.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabInfRadBco6[i] : null,
                    CabInfRadBco7 = StabilizationDataDetails.CabInfRadBco7.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabInfRadBco7[i] : null,
                    CabInfRadBco8 = StabilizationDataDetails.CabInfRadBco8.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabInfRadBco8[i] : null,
                    CabInfRadBco9 = StabilizationDataDetails.CabInfRadBco9.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabInfRadBco9[i] : null,
                    CabInfRadBco10 = StabilizationDataDetails.CabInfRadBco10.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabInfRadBco10[i] : null,
                    CabSupRadBco1 = StabilizationDataDetails.CabSupRadBco1.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabSupRadBco1[i] : null,
                    CabSupRadBco2 = StabilizationDataDetails.CabSupRadBco2.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabSupRadBco2[i] : null,
                    CabSupRadBco3 = StabilizationDataDetails.CabSupRadBco3.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabSupRadBco3[i] : null,
                    CabSupRadBco4 = StabilizationDataDetails.CabSupRadBco4.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabSupRadBco4[i] : null,
                    CabSupRadBco5 = StabilizationDataDetails.CabSupRadBco5.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabSupRadBco5[i] : null,
                    CabSupRadBco6 = StabilizationDataDetails.CabSupRadBco6.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabSupRadBco6[i] : null,
                    CabSupRadBco7 = StabilizationDataDetails.CabSupRadBco7.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabSupRadBco7[i] : null,
                    CabSupRadBco8 = StabilizationDataDetails.CabSupRadBco8.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabSupRadBco8[i] : null,
                    CabSupRadBco9 = StabilizationDataDetails.CabSupRadBco9.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabSupRadBco9[i] : null,
                    CabSupRadBco10 = StabilizationDataDetails.CabSupRadBco10.ElementAtOrDefault(i) != null ? StabilizationDataDetails.CabSupRadBco10[i] : null,
                    CanalAmb1 = StabilizationDataDetails.CanalAmb1,
                    CanalAmb2 = StabilizationDataDetails.CanalAmb2,
                    CanalAmb3 = StabilizationDataDetails.CanalAmb3,
                    CanalInf1 = StabilizationDataDetails.CanalInf1,
                    CanalInf2 = StabilizationDataDetails.CanalInf2,
                    CanalInf3 = StabilizationDataDetails.CanalInf3,
                    CanalInf4 = StabilizationDataDetails.CanalInf4,
                    CanalInf5 = StabilizationDataDetails.CanalInf5,
                    CanalInf6 = StabilizationDataDetails.CanalInf6,
                    CanalInf7 = StabilizationDataDetails.CanalInf7,
                    CanalInf8 = StabilizationDataDetails.CanalInf8,
                    CanalInf9 = StabilizationDataDetails.CanalInf9,
                    CanalInf10 = StabilizationDataDetails.CanalInf10,
                    CanalInst1 = StabilizationDataDetails.CanalInst1,
                    CanalInst2 = StabilizationDataDetails.CanalInst2,
                    CanalInst3 = StabilizationDataDetails.CanalInst3,
                    CanalSup1 = StabilizationDataDetails.CanalSup1,
                    CanalSup2 = StabilizationDataDetails.CanalSup2,
                    CanalSup3 = StabilizationDataDetails.CanalSup3,
                    CanalSup4 = StabilizationDataDetails.CanalSup4,
                    CanalSup5 = StabilizationDataDetails.CanalSup5,
                    CanalSup6 = StabilizationDataDetails.CanalSup6,
                    CanalSup7 = StabilizationDataDetails.CanalSup7,
                    CanalSup8 = StabilizationDataDetails.CanalSup8,
                    CanalSup9 = StabilizationDataDetails.CanalSup9,
                    CanalSup10 = StabilizationDataDetails.CanalSup10,
                    CanalTtapa = StabilizationDataDetails.CanalTtapa,
                    Estable = StabilizationDataDetails.Estable.ElementAtOrDefault(i) != null && StabilizationDataDetails.Estable[i] != null && (bool)StabilizationDataDetails.Estable[i],
                    FechaHora = StabilizationDataDetails.FechaHora.ElementAtOrDefault(i) != null && StabilizationDataDetails.FechaHora[i] != null ? (DateTime)StabilizationDataDetails.FechaHora[i] : new DateTime(),
                    IdReg = StabilizationDataDetails.IdReg.ElementAtOrDefault(i) != null && StabilizationDataDetails.IdReg[i] != null ? (decimal)StabilizationDataDetails.IdReg[i] : 0,
                    KwMedidos = StabilizationDataDetails.KwMedidos.ElementAtOrDefault(i) != null && StabilizationDataDetails.KwMedidos[i] != null ? (decimal)StabilizationDataDetails.KwMedidos[i] : 0,
                    Presion = StabilizationDataDetails.Presion.ElementAtOrDefault(i) != null ? StabilizationDataDetails.Presion[i] : null,
                    PromRadInf = StabilizationDataDetails.PromRadInf.ElementAtOrDefault(i) != null && StabilizationDataDetails.PromRadInf[i] != null ? (decimal)StabilizationDataDetails.PromRadInf[i] : 0,
                    PromRadSup = StabilizationDataDetails.PromRadSup.ElementAtOrDefault(i) != null && StabilizationDataDetails.PromRadSup[i] != null ? (decimal)StabilizationDataDetails.PromRadSup[i] : 0,
                    TempInstr1 = StabilizationDataDetails.TempInstr1.ElementAtOrDefault(i) != null ? StabilizationDataDetails.TempInstr1[i] : null,
                    TempInstr2 = StabilizationDataDetails.TempInstr2.ElementAtOrDefault(i) != null ? StabilizationDataDetails.TempInstr2[i] : null,
                    TempInstr3 = StabilizationDataDetails.TempInstr3.ElementAtOrDefault(i) != null ? StabilizationDataDetails.TempInstr3[i] : null,
                    TempTapa = StabilizationDataDetails.TempTapa.ElementAtOrDefault(i) != null && StabilizationDataDetails.TempTapa[i] != null ? (decimal)StabilizationDataDetails.TempTapa[i] : 0,
                    Tor = StabilizationDataDetails.Tor.ElementAtOrDefault(i) != null && StabilizationDataDetails.Tor[i] != null ? (decimal)StabilizationDataDetails.Tor[i] : 0,
                    TorCorr = StabilizationDataDetails.TorCorr.ElementAtOrDefault(i) != null && StabilizationDataDetails.TorCorr[i] != null ? (decimal)StabilizationDataDetails.TorCorr[i] : 0,
                    VerifVent1 = StabilizationDataDetails.VerifVent1.ElementAtOrDefault(i) != null && StabilizationDataDetails.VerifVent1[i] != null && (bool)StabilizationDataDetails.VerifVent1[i],
                    VerifVent2 = StabilizationDataDetails.VerifVent2.ElementAtOrDefault(i) != null && StabilizationDataDetails.VerifVent2[i] != null && (bool)StabilizationDataDetails.VerifVent2[i],
                });
            }

            return aux;
        }
        #endregion

        public decimal? IdReg { get; set; }
        public string NoSerie { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public DateTime? FechaDatos { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string CoolingType { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? OverElevation { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? FactEnf { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? Intervalo { get; set; }
        public string UmIntervalo { get; set; }
        public string PosAt { get; set; }
        public string PosBt { get; set; }
        public string PosTer { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [Range(0.001, 999.999, ErrorMessage = "La capacidad debe ser numérica mayor a cero considerando 3 enteros con 3 decimales")]
        public decimal? Capacidad { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string DevanadoSplit { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? AltitudeF1 { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string AltitudeF2 { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public decimal? FactAlt { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [Range(1, 999, ErrorMessage = "El % de carga debe ser numérico mayor a cero considerando 3 enteros sin decimales")]
        public decimal? PorcCarga { get; set; }
        [Required(ErrorMessage = "Requerido")]
        public string Sobrecarga { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [Range(1, 10, ErrorMessage = "Cantidad de termo pares debe ser mayor a cero pero menor o igual a 10")]
        public decimal? CantTermoPares { get; set; }
        public decimal? Perdidas { get; set; }
        [Required(ErrorMessage = "Requerido")]
        [Range(0.001, 999.999, ErrorMessage = "Corriente debe ser numérico mayor a cero considerando 3 enteros con 3 decimales")]
        public decimal? Corriente { get; set; }
        public bool? Estatus { get; set; }
        public int? CantEstables { get; set; }
        public int? CantInestables { get; set; }
        public StabilizationDetailsDataViewModel StabilizationDataDetails { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
