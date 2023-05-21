namespace SPL.Tests.Api.DTOs.Tests.ROD
{
    using System.Collections.Generic;

    public class ResultRODTestsDto
    {
        public List<ErrorColumnsDto> results { get; set; }
        public List<RODTestsDto> RODTests { get; set; }
    }
}
