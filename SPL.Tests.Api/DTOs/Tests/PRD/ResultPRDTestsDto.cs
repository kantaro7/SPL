namespace SPL.Tests.Api.DTOs.Tests.PRD
{
    using System.Collections.Generic;

    public class ResultPRDTestsDto
    {
        public PRDTestsDto PRDTests { get; set; }
        public List<ErrorColumnsDto> Results { get; set; }
    }
}
