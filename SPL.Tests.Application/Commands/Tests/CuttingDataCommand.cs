namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.NRA;

    public class CuttingDataCommand : IRequest<ApiResponse<ResultCuttingDataTests>>
    {
        public CuttingDataCommand(HeaderCuttingData pData) => this.Data = pData;
        #region Constructor
        public HeaderCuttingData Data { get; }
        #endregion
    }
}
