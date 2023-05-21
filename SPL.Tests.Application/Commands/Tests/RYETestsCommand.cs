namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.FPC;
    using SPL.Domain.SPL.Tests.ISZ;

    using System.Collections.Generic;

    public class RYETestsCommand : IRequest<ApiResponse<ResultRYETests>>
    {
        public RYETestsCommand(OutRYETests pData) => this.Data = pData;
        #region Constructor
        public OutRYETests Data { get; }
        #endregion
    }
}
