namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.NRA;

    public class StabilizationDataTestsCommand : IRequest<ApiResponse<ResultStabilizationDataTests>>
    {
        public StabilizationDataTestsCommand(StabilizationData pData) => this.Data = pData;
        #region Constructor
        public StabilizationData Data { get; }
        #endregion
    }
}
