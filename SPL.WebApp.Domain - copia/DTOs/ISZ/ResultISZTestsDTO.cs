namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ResultISZTestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public OutISZTestsDTO ISZTests { get; set; }
    }
}
