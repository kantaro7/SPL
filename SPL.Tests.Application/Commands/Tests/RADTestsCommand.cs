namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;

    public class RADTestsCommand : IRequest<ApiResponse<ResultRADTests>>
    {
        public RADTestsCommand(RADTests pData) => this.Data = pData;
        #region Constructor
        public RADTests Data { get; }
        #endregion
    }
}
