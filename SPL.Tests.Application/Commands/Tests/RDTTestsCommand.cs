namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;

    public class RDTTestsCommand : IRequest<ApiResponse<ResultRDTTestsDetails>>
    {
        public RDTTestsCommand(RDTTests pData) => this.Data = pData;
        #region Constructor
        public RDTTests Data { get; }
        #endregion
    }
}
