namespace SPL.Tests.Api.DTOs.Tests.NRA
{
    using System.Collections.Generic;

    public class ResultNRATestsDto
    {
        public List<ErrorColumnsDto> Results { get; set; }
        public NRATestsGeneralDto NRATestsGeneral { get; set; }
    }
}
