namespace SPL.Tests.Api.DTOs.Tests.FPB
{
    using SPL.Tests.Api.DTOs.Tests;
    using SPL.Tests.Api.DTOs.Tests.FPB;

    using System.Collections.Generic;

    public class ResultFPBTestsDto
    {
        public List<ErrorColumnsDto> Results { get; set; }
        public List<FPBTestsDto> FPBTests { get; set; }
    }
}
