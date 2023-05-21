namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests.PRD;

    using System.Collections.Generic;

    public class PRDTestsCommand : IRequest<ApiResponse<ResultPRDTests>>
    {
        public PRDTestsCommand(PRDTests pData) => this.Data = pData;
        #region Constructor
        public PRDTests Data { get; }
        #endregion
    }
}
