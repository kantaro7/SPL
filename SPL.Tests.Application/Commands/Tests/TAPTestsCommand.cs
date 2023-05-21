namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.FPC;
    using SPL.Domain.SPL.Tests.ISZ;
    using SPL.Domain.SPL.Tests.TAP;

    using System.Collections.Generic;

    public class TAPTestsCommand : IRequest<ApiResponse<ResultTAPTests>>
    {
        public TAPTestsCommand(TAPTests pData) => this.Data = pData;
        #region Constructor
        public TAPTests Data { get; }
        #endregion
    }
}
