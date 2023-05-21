namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.FPC;

    using System.Collections.Generic;

    public class FPCTestsCommand : IRequest<ApiResponse<ResultFPCTests>>
    {
        public FPCTestsCommand(List<FPCTests> pData) => this.Data = pData;
        #region Constructor
        public List<FPCTests> Data { get; }
        #endregion
    }
}
