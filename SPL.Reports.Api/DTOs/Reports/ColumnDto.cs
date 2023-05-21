namespace SPL.Reports.Api.DTOs.Reports
{
    using System.Collections.Generic;

    public class ColumnDto
    {
        public string Name { get; set; }
        public List<decimal> Values { get; set; }
    }
}
