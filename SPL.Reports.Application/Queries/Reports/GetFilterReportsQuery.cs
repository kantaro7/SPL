﻿using MediatR;

using SPL.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPL.Reports.Application.Queries.Reports
{
    public class GetFilterReportsQuery : IRequest<ApiResponse<List<SPL.Domain.SPL.Reports.FilterReports>>>
    {
        public GetFilterReportsQuery(string pTypeReport)
        {
       
            TypeReport = pTypeReport;

        }
        #region Constructor

 
        public string TypeReport { get; }
        #endregion
    }
}