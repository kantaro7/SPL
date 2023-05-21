using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoCustomer
    {
        public decimal CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerBaan { get; set; }
        public decimal? Active { get; set; }
        public decimal? CnFlag { get; set; }
        public decimal? IFlag { get; set; }
        public decimal? FuFlag { get; set; }
    }
}
