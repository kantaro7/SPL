namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.FPB;
    using SPL.Domain.SPL.Tests.FPC;
    using SPL.Domain.SPL.Tests.TDP;

    using System.Collections.Generic;

    public class TDPTestsCommand : IRequest<ApiResponse<ResultTDPTests>>
    {
        public TDPTestsCommand(TDPTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public TDPTestsGeneral Data { get; }
        #endregion
    }
}
