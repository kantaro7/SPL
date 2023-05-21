namespace SPL.Tests.Api.DTOs.Tests.PCE
{
    using System.Collections.Generic;

    public class ResultPCETestsDto
    {
        public List<ErrorColumnsDto> Results { get; set; }
        public List<PCETestsDto> PCETests { get; set; }
    }
}
