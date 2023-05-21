namespace SPL.Tests.Api.DTOs.Tests.PLR
{
    using System.Collections.Generic;

    public class ResultPLRTestsDto
    {
        public PLRTestsDto PLRTests { get; set; }
        public List<ErrorColumnsDto> Results { get; set; }
    }
}
