namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests.CGD;
    using SPL.Domain.SPL.Tests.FPA;
    using SPL.Domain.SPL.Tests.PLR;
    using SPL.Domain.SPL.Tests.RDD;

    using System.Collections.Generic;

    public class FPATestsCommand : IRequest<ApiResponse<ResultFPATests>>
    {
        public FPATestsCommand(FPATests pData) => this.Data = pData;
        #region Constructor
        public FPATests Data { get; }
        #endregion
    }
}
