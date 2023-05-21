namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests.NRA;

    public class NRATestsCommand : IRequest<ApiResponse<ResultNRATests>>
    {
        public NRATestsCommand(NRATestsGeneral pData) => this.Data = pData;
        #region Constructor
        public NRATestsGeneral Data { get; }
        #endregion
    }
}
