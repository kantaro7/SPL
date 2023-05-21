namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.FPC;
    using SPL.Domain.SPL.Tests.ISZ;

    using System.Collections.Generic;

    public class ISZTestsCommand : IRequest<ApiResponse<ResultISZTests>>
    {
        public ISZTestsCommand(OutISZTests pData) => this.Data = pData;
        #region Constructor
        public OutISZTests Data { get; }
        #endregion
    }
}
