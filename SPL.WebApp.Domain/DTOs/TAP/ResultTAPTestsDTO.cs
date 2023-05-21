namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;
    public class ResultTAPTestsDTO
    {
        public List<ErrorColumnsDTO> Results { get; set; }
        public TAPTestsDTO TAPTests { get; set; }
    }
}
