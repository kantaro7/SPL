﻿namespace SPL.Security.Application.Queries.Security
{
    using MediatR;
    using SPL.Domain;
    using SPL.Domain.SPL.Reports.FPC;
    using SPL.Domain.SPL.Security;

    using System.Collections.Generic;

    public class GetOptionsMenuQuery : IRequest<ApiResponse<List<UserOptions>>>
    {
        public GetOptionsMenuQuery()
        {

        }
     

    }
}
