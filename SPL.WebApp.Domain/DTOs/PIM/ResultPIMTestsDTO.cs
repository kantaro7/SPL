namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class ResultPIMTestsDTO
    {
        public PIMTestsDTO PIMTests { get; set; }
        public List<ErrorColumnsDTO> Results { get; set; }
    }
}
