namespace SPL.WebApp.Domain.DTOs
{
    using System;
    using System.Collections.Generic;
    public class OutTAPTestsDTO
    {
        public decimal Capacity { get; set; }
        public decimal PorcZ { get; set; }
        public decimal EmptyLosses { get; set; }
        public decimal Lostload { get; set; }
        public decimal LostCooldown { get; set; }
        public decimal TotalLosses { get; set; }
        public decimal PorcX { get; set; }
        public decimal PorcR { get; set; }
        public decimal XIntoR { get; set; }
        public decimal FactPot1 { get; set; }
        public decimal FactPot2 { get; set; }
        public decimal FactPot3 { get; set; }
        public decimal FactPot4 { get; set; }
        public decimal FactPot5 { get; set; }
        public decimal FactPot6 { get; set; }
        public decimal FactPot7 { get; set; }
        public decimal PorcReg1 { get; set; }
        public decimal PorcReg2 { get; set; }
        public decimal PorcReg3 { get; set; }
        public decimal PorcReg4 { get; set; }
        public decimal PorcReg5 { get; set; }
        public decimal PorcReg6 { get; set; }
        public decimal PorcReg7 { get; set; }
        public decimal ValueW { get; set; }
        public decimal ValueG { get; set; }
        public DateTime Date { get; set; }
        public List<TAPTestsDetailsDTO> TAPTestsDetails { get; set; }
    }
}
