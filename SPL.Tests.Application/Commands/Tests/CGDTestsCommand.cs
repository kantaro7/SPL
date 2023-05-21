namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests.CGD;
    using SPL.Domain.SPL.Tests.PLR;

    using System.Collections.Generic;

    public class CGDTestsCommand : IRequest<ApiResponse<ResultCGDTests>>
    {
        public CGDTestsCommand(List<CGDTests> pData) => this.Data = pData;
        #region Constructor
        public List<CGDTests> Data { get; }
        #endregion
    }
}
