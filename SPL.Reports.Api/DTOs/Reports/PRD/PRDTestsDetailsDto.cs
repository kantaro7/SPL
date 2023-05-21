using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Reports.Api.DTOs.Reports.PRD
{
    public class PRDTestsDetailsDto
    {
        /// <summary>
        /// /Data Inicial
        /// </summary>
        public decimal Cn { get; set; }
        public decimal M3 { get; set; }
        public decimal C4 { get; set; }
        public decimal Vm { get; set; }
        public decimal U { get; set; }
        public decimal Tmp { get; set; }
        public decimal R4s { get; set; }
        public decimal Im { get; set; }
        public decimal Cap { get; set; }

        /// <summary>
        /// /Data Inicial 2
        /// </summary>
        public decimal Tm { get; set; }
        public decimal Rm { get; set; }
        public decimal Tr { get; set; }
        public decimal Pfe { get; set; }
        public decimal Warranty { get; set; }

        /// <summary>
        /// /Calculos
        /// </summary>
        public decimal V { get; set; }
        public decimal I { get; set; }
        public decimal Lxp { get; set; }
        public decimal Rxp { get; set; }
        public decimal P { get; set; }
        public decimal Xm { get; set; }
        public decimal Xc { get; set; }
        public decimal PorcDesv { get; set; }
        public decimal Pjm { get; set; }
        public decimal Fc { get; set; }
        public decimal Pjmc { get; set; }
        public decimal Pe { get; set; }
        public decimal Fc2 { get; set; }
        public decimal Pt { get; set; }

    }
}
