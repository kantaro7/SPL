namespace SPL.Tests.Api.DTOs.Tests.ETD
{
    using System.Collections.Generic;

    public class ResultETDTestsDto
    {
        public List<ErrorColumnsDto> Results { get; set; }
        public ETDTestsGeneralDto ETDTestsGeneral { get; set; }
    }
}
