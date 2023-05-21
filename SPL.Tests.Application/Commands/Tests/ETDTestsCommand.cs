namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.DPR;
    using SPL.Domain.SPL.Tests.ETD;
    using SPL.Domain.SPL.Tests.FPB;
    using SPL.Domain.SPL.Tests.FPC;
    using SPL.Domain.SPL.Tests.TDP;

    using System.Collections.Generic;

    public class ETDTestsCommand : IRequest<ApiResponse<ResultETDTests>>
    {
        public ETDTestsCommand(ETDTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public ETDTestsGeneral Data { get; }
        #endregion
    }
}
