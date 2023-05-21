namespace SPL.Tests.Application.Commands.Tests
{
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests.ROD;

    public class RODTestsCommand : IRequest<ApiResponse<ResultRODTests>>
    {
        public RODTestsCommand(List<RODTests> pData) => this.Data = pData;
        #region Constructor
        public List<RODTests> Data { get; }
        #endregion
    }
}
