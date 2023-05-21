namespace SPL.Tests.Application.Commands.Tests
{
    using MediatR;

    using SPL.Domain;
 
    using SPL.Domain.SPL.Tests;
    using SPL.Domain.SPL.Tests.RCT;

    using System.Collections.Generic;

    public class RCTTestsCommand : IRequest<ApiResponse<ResultRCTTests>>
    {
        public RCTTestsCommand(List<RCTTests> pData) => this.Data = pData;
        #region Constructor
        public List<RCTTests> Data { get; }
        #endregion
    }
}
