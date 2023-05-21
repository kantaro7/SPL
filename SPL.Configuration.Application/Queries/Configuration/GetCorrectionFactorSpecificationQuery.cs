namespace SPL.Configuration.Application.Queries.Configuration
{
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;

    public class GetCorrectionFactorSpecificationQuery : IRequest<ApiResponse<List<CorrectionFactorSpecification>>>
    {
        public GetCorrectionFactorSpecificationQuery(string pSpecification, decimal pTemperature, decimal pCorrectionFactor)
        {
            this.Specification = pSpecification;
            this.Temperature = pTemperature;
            this.CorrectionFactor = pCorrectionFactor;
        }
        #region Constructor
        public string Specification { get; }
        public decimal Temperature { get; }
        public decimal CorrectionFactor { get; }
        #endregion
    }
}
