namespace SPL.Masters.Application.Queries.Artifactdesign
{
    using System.Collections.Generic;

    using MediatR;

    using SPL.Domain.SPL.Masters;

    public class GetRulesRepQuery : IRequest<List<RulesRep>>
    {
        public GetRulesRepQuery(string pNorma, string pIdioma)
        {
            this.Norma = pNorma;
            this.Idioma = pIdioma;
        }

        public string Norma { get; }
        public string Idioma { get; }
    }
}
