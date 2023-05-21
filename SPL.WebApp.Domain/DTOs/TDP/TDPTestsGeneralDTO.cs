﻿namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class TDPTestsGeneralDTO
    {

        public long IdLoad { get; set; }
        public DateTime LoadDate { get; set; }
        public DateTime Date { get; set; }
        public int TestNumber { get; set; }
        public string LanguageKey { get; set; }
        public string Customer { get; set; }
        public string Capacity { get; set; }
        public string SerialNumber { get; set; }
        public bool Result { get; set; }
        public string NameFile { get; set; }
        public byte[] File { get; set; }
        public string TypeReport { get; set; }
        public string KeyTest { get; set; }
        public string Comment { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }

        public int Tolerance { get; set; }
        public int NoCycles { get; set; }
        public int TotalTime { get; set; }
        public int Interval { get; set; }
        public decimal TimeLevel { get; set; }
        public decimal OutputLevel { get; set; }
        public int DescMayPc { get; set; }
        public int DescMayMv { get; set; }
        public int IncMaxPc { get; set; }
        public string VoltageLevels { get; set; }
        public string TerminalsTest { get; set; }
        public string MeasurementType { get; set; }
        public int Frequency { get; set; }
        public string Pos_At { get; set; }
        public string Pos_Bt { get; set; }
        public string Pos_Ter { get; set; }
        public TDPTestsDTO TDPTest { get; set; }
    }
}
