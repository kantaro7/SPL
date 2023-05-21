namespace SPL.Artifact.Application.Commands.Reports
{
    using System;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Reports.PCI;
    using SPL.Domain.SPL.Reports.ROD;

    public class SaveReportPCICommand : IRequest<ApiResponse<long>>
    {
        #region Constructor

        public SaveReportPCICommand(PCITestGeneral pData) => this.Data = pData;

        #endregion

        #region Properties

        public PCITestGeneral Data { get; }

        #endregion
    }
}