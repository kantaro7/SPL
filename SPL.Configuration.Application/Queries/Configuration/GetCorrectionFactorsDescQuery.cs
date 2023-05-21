namespace SPL.Configuration.Application.Queries.Configuration
{
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Configuration;

    public class GetCorrectionFactorsDescQuery : IRequest<ApiResponse<CorrectionFactorsDesc>>
    {
        public GetCorrectionFactorsDescQuery(string pSpecification, string pKeyLenguage)
        {
            this.Specification = pSpecification;
            this.KeyLenguage = pKeyLenguage;

        }
        #region Constructor
        public string Specification { get; }
        public string KeyLenguage { get; }

        #endregion
    }
}
