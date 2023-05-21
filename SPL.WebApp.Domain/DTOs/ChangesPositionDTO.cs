namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ChangesPositionDTO
    {
        public ChangesPositionDTO()
        {
            Values = new List<ValuesChanges>();
        }

        public string Position { get; set; }
        public string Identification { get; set; }
        public bool Reversed { get; set; }
        public List<ValuesChanges> Values { get; set; }
    }

    public class ValuesChanges
    {
        public bool Selected { get; set; }
        public string Value { get; set; }
    }
}
