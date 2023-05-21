namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests.PEE;
    using SPL.Domain.SPL.Tests.PLR;

    using System.Collections.Generic;

    public class PEETestsCommand : IRequest<ApiResponse<ResultPEETests>>
    {
        public PEETestsCommand(PEETests pData) => this.Data = pData;
        #region Constructor
        public PEETests Data { get; }
        #endregion
    }
}
