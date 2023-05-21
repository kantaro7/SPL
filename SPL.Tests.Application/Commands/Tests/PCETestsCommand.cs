namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.FPB;
    using SPL.Domain.SPL.Tests.FPC;
    using SPL.Domain.SPL.Tests.PCE;

    using System.Collections.Generic;

    public class PCETestsCommand : IRequest<ApiResponse<ResultPCETests>>
    {
        public PCETestsCommand(List<PCETests> pData) => this.Data = pData;
        #region Constructor
        public List<PCETests> Data { get; }
        #endregion
    }
}
