namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests.PLR;

    using System.Collections.Generic;

    public class PLRTestsCommand : IRequest<ApiResponse<ResultPLRTests>>
    {
        public PLRTestsCommand(PLRTests pData) => this.Data = pData;
        #region Constructor
        public PLRTests Data { get; }
        #endregion
    }
}
