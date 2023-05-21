namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ColumnDTO
    {
        public string Name { get; set; }

        public List<decimal> Values { get; set; }
    }
}
