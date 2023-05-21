namespace SPL.Masters.Application.Queries.Configuration
{
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain;
    using SPL.Domain.SPL.Masters;

    public class GetConfigurationFilesQuery : IRequest<ApiResponse<List<FileWeight>>>
    {
        public GetConfigurationFilesQuery(long pModule) => this.Module = pModule;
        #region Constructor
        public long Module { get; }
        #endregion
    }
}
