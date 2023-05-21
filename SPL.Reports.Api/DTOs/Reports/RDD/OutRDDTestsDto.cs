namespace SPL.Reports.Api.DTOs.Reports.RDD
{
    using System;
    using System.Collections.Generic;

    using global::SPL.Domain.SPL.Artifact.ArtifactDesign;
    using global::SPL.Domain.SPL.Configuration;

    public class OutRDDTestsDto
    {
        public List<RDDTestsDetailsDto> RDDTestsDetails { get; set; }
     
    }
}
