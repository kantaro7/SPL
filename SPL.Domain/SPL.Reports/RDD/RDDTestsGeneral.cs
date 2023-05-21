namespace SPL.Domain.SPL.Reports.RDD
{
    using System;
    using System.Collections.Generic;

    using global::SPL.Domain.SPL.Artifact.ArtifactDesign;
    using global::SPL.Domain.SPL.Configuration;

    public class RDDTestsGeneral
    {


        public long IdLoad { get; set; }
        public DateTime LoadDate { get; set; }
        public DateTime DateTest { get; set; }
        public int TestNumber { get; set; }
        public string LanguageKey { get; set; }
        public string Customer { get; set; }
        public string CapacityReport { get; set; }
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



        public decimal S3F_V2 { get; set; }
        public string Connection { get; set; }
        public decimal Capacity { get; set; }
        public string PositionAT { get; set; }
        public string PositionBT { get; set; }
        public string EnergizedWinding { get; set; }
        public string ConfigWinding { get; set; }
        public decimal ValueAcep { get; set; }
        public decimal VoltageEW { get; set; }
        public string ShortWinding { get; set; }
        public decimal VoltageSW { get; set; }
        public decimal PorcZ { get; set; }
        public decimal TporX { get; set; }
        public decimal PorcJx { get; set; }
        public List<OutRDDTests> OutRDDTests { get; set; }


    }
}
