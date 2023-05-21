namespace SPL.Tests.Application.Queries.Tests
{
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;

    public class GetTestsQuery : IRequest<ApiResponse<List<SPL.Domain.SPL.Tests.Tests>>>
    {
        public GetTestsQuery(string pTypeReport, string pKeyTests)
        {
            this.TypeReport = pTypeReport;
            this.KeyTests = pKeyTests;

        }
        #region Constructor

        public string TypeReport { get; }
        public string KeyTests { get; }
        #endregion
    }
}
