namespace SPL.Tests.Api.DTOs.Tests.PCI
{
    using System.Collections.Generic;

    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.PCI;

    public class ResultPCITestDto
    {
        public List<ResponseMessage> Messages { get; set; }

        public List<RatingPCITestResult> Results { get; set; } = new List<RatingPCITestResult>();

        public decimal Kwtot100M { get; set; }

        public bool IsApproved { get; set; }
    }
}