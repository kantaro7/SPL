﻿namespace SPL.Masters.Application.Queries.Artifactdesign
{
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain.SPL.Masters;

    public class GetRuleEquivalentsQuery : IRequest<List<GeneralProperties>>
    {
        public GetRuleEquivalentsQuery()
        {
        }
    }
}
