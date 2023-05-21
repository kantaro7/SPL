namespace SPL.Tests.Api.DTOs.Tests.PEE
{
    using System.Collections.Generic;

    public class ResultPEETestsDto
    {
        public PEETestsDto PEETests { get; set; }
        public List<ErrorColumnsDto> Results { get; set; }
    }
}
