namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Tests.CGD;
    using SPL.Domain.SPL.Tests.PLR;
    using SPL.Domain.SPL.Tests.RDD;

    using System.Collections.Generic;

    public class RDDTestsCommand : IRequest<ApiResponse<ResultRDDTests>>
    {
        public RDDTestsCommand(RDDTestsGeneral pData) => this.Data = pData;
        #region Constructor
        public RDDTestsGeneral Data { get; }
        #endregion
    }
}
