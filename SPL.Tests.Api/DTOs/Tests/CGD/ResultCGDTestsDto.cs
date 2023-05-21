namespace SPL.Tests.Api.DTOs.Tests.CGD
{
    using System.Collections.Generic;

    public class ResultCGDTestsDto
    {
        public List<ErrorColumnsDto> Results { get; set; }
        public List<CGDTestsDto> CGDTests { get; set; }
    }
}
