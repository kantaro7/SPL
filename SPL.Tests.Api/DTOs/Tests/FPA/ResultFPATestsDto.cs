namespace SPL.Tests.Api.DTOs.Tests.FPA
{
    using System.Collections.Generic;

    public class ResultFPATestsDto
    {
        public List<ErrorColumnsDto> Results { get; set; }
        public FPATestsDto FPATests { get; set; }
    }
}
