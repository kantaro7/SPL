namespace SPL.Domain.SPL.Tests.NRA
{
    using System;
    using System.Collections.Generic;

    public class MatrixNRATests
    {

        public string Position { get; set; }
        public string TypeInformation { get; set; }
        public string Height { get; set; }

        public int Section { get; set; }
        public decimal Dba { get; set; }
        public decimal? DbaCor { get; set; }  /*--Solo aplica para la prueba nivel de ruido */

        public decimal Decibels315 { get; set; }
        public decimal Decibels63 { get; set; }
        public decimal Decibels125 { get; set; }
        public decimal Decibels250 { get; set; }
        public decimal Decibels500 { get; set; }
        public decimal Decibels1000 { get; set; }
        public decimal Decibels2000 { get; set; }
        public decimal Decibels4000 { get; set; }
        public decimal Decibels8000 { get; set; }
        public decimal Decibels10000 { get; set; }

    }
}
