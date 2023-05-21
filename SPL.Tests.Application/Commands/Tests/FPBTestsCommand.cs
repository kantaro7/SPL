namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.FPB;
    using SPL.Domain.SPL.Tests.FPC;

    using System.Collections.Generic;

    public class FPBTestsCommand : IRequest<ApiResponse<ResultFPBTests>>
    {
        public FPBTestsCommand(List<FPBTests> pData) => this.Data = pData;
        #region Constructor
        public List<FPBTests> Data { get; }
        #endregion
    }
}
