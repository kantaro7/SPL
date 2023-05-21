using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoGeneralDatum
    {
        public decimal GeneralDataId { get; set; }
        public decimal? OrderId { get; set; }
        public decimal? Active { get; set; }
        public decimal? Quantity { get; set; }
        public string Cm { get; set; }
        public string Userinitials { get; set; }
        public DateTime? Creationdate { get; set; }
        public decimal? Statusorder { get; set; }
        public string Customername { get; set; }
        public string Intermediary { get; set; }
        public string Finaluser { get; set; }
        public decimal? Typetrafoid { get; set; }
        public decimal? Applicationid { get; set; }
        public decimal? Standardid { get; set; }
        public decimal? CustomernameId { get; set; }
        public decimal? IntermediaryId { get; set; }
        public decimal? FinaluserId { get; set; }
        public string CreationUser { get; set; }
        public DateTime? CreationDate1 { get; set; }
        public string ModificationUser { get; set; }
        public DateTime? ModificationDate { get; set; }
        public decimal? Bankunitsid { get; set; }
    }
}
