namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;

    using System.Collections.Generic;

    public class RANTestsCommand : IRequest<ApiResponse<ResultRANTests>>
    {
        public RANTestsCommand(List<RANTestsDetails> pData) => this.Data = pData;
        #region Constructor
        public List<RANTestsDetails> Data { get; }
        #endregion
    }
}
