namespace SPL.Artifact.Application.Handlers.Artifactdesign
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using SPL.Artifact.Application.Queries.Artifactdesign;
    using SPL.Domain.SPL.Artifact.ArtifactDesign;

    public class GetArtifactDesignHandler : IRequestHandler<GetArtifactDesignQuery, InformationArtifact?>
    {

        private readonly IArtifactdesignInfrastructure _infrastructure;

        public GetArtifactDesignHandler(IArtifactdesignInfrastructure infrastructure) => this._infrastructure = infrastructure;

        #region Methods
        public async Task<InformationArtifact> Handle(GetArtifactDesignQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.NroSerie))
            {
                return new InformationArtifact { };
            }
            InformationArtifact result = await this._infrastructure.GetGeneralArtifactdesign(request.NroSerie);

            InfoCarLocal resultCarLocal = await this._infrastructure.GetInfoCarLocal(request.NroSerie);

            if (resultCarLocal == null)
            {
                return new InformationArtifact { };
            }

            result.NBAINeutro = new NBAINeutro
            {
                valornbaineutroaltatension = resultCarLocal.Mvaf1NbaiNeutro,
                valornbaineutrobajatension = resultCarLocal.Mvaf2NbaiNeutro,
                valornbaineutrosegundabaja = resultCarLocal.Mvaf3NbaiNeutro,
                valornbaineutrotercera = resultCarLocal.Mvaf4NbaiNeutro
            };

            result.Derivations = new Derivations
            {
                valorderivacionupaltatension = resultCarLocal.Mvaf1DerUp,
                valorderivaciondownaltatension = resultCarLocal.Mvaf1DerDown,
                valorderivacionupbajatension = resultCarLocal.Mvaf2DerUp,
                valorderivaciondownbajatension = resultCarLocal.Mvaf2DerDown,

                valorderivacionupsegundatension = resultCarLocal.Mvaf3DerUp,
                valorderivaciondownsegundatension = resultCarLocal.Mvaf3DerDown,

                valorderivacionupterceratension = resultCarLocal.Mvaf4DerUp,
                valorderivaciondownterceratension = resultCarLocal.Mvaf4DerDown,

                tipoderivacionaltatension = resultCarLocal.RcbnFcbn1?.ToString(),
                tipoderivacionbajatension = resultCarLocal.RcbnFcbn2?.ToString(),
                tipoderivacionsegundatension = resultCarLocal.RcbnFcbn3?.ToString(),
                tipoderivacionterceratension = resultCarLocal.RcbnFcbn4?.ToString(),

                conexionequivalente = resultCarLocal.ConexionEq?.ToString(),
                conexionequivalente_2 = resultCarLocal.ConexionEq2?.ToString(),
                conexionequivalente_3 = resultCarLocal.ConexionEq3?.ToString(),
                conexionequivalente_4 = resultCarLocal.ConexionEq4?.ToString(),

                idConexionEquivalente = resultCarLocal.Mvaf1ConnectionId == null ? -1 : Convert.ToInt32(resultCarLocal.Mvaf1ConnectionId),
                idConexionEquivalente2 = resultCarLocal.Mvaf2ConnectionId == null ? -1 : Convert.ToInt32(resultCarLocal.Mvaf2ConnectionId),
                idConexionEquivalente3 = resultCarLocal.Mvaf3ConnectionId == null ? -1 : Convert.ToInt32(resultCarLocal.Mvaf3ConnectionId),
                idConexionEquivalente4 = resultCarLocal.Mvaf4ConnectionId == null ? -1 : Convert.ToInt32(resultCarLocal.Mvaf4ConnectionId),

            };

            result.NBAI = new NBAIBilKv
            {
                nbaialtatension = resultCarLocal.Mvaf1Nbai1,
                nbaibajatension = resultCarLocal.Mvaf2Nbai1,
                nbaisegundabaja = resultCarLocal.Mvaf3Nbai1,
                nabaitercera = resultCarLocal.Mvaf4Nbai1
            };

            result.Connections = new ConnectionTypes
            {
                idconexionaltatension = resultCarLocal.Mvaf1ConnectionId == null ? -1 : Convert.ToInt32(resultCarLocal.Mvaf1ConnectionId),
                idconexionbajatension = resultCarLocal.Mvaf2ConnectionId == null ? -1 : Convert.ToInt32(resultCarLocal.Mvaf2ConnectionId),
                idconexionsegundabaja = resultCarLocal.Mvaf3ConnectionId == null ? -1 : Convert.ToInt32(resultCarLocal.Mvaf3ConnectionId),
                idconexiontercera = resultCarLocal.Mvaf4ConnectionId == null ? -1 : Convert.ToInt32(resultCarLocal.Mvaf4ConnectionId),
                otraconexionbajatension = resultCarLocal.Mvaf2ConnectionOther,
                otraconexionsegundabaja = resultCarLocal.Mvaf3ConnectionOther,
                otraconexiontercera = resultCarLocal.Mvaf4ConnectionOther
            };

            result.Taps = new Taps
            {

                tapsaltatension = resultCarLocal.Mvaf1Taps,
                tapsbajatension = resultCarLocal.Mvaf2Taps,
                tapssegundabaja = resultCarLocal.Mvaf3Taps,
                tapsterciario = resultCarLocal.Mvaf4Taps,
            };

            result.Tapbaan = await this._infrastructure.GetTapBaan(request.NroSerie);

            result.VoltageKV = new VoltageKV
            {
                tensionkvaltatension1 = resultCarLocal.Mvaf1Voltage1,
                tensionkvaltatension2 = resultCarLocal.Mvaf1Voltage2,

                tensionkvbajatension1 = resultCarLocal.Mvaf2Voltage1,
                tensionkvbajatension2 = resultCarLocal.Mvaf2Voltage2,

                tensionkvsegundabaja1 = resultCarLocal.Mvaf3Voltage1,
                tensionkvsegundabaja2 = resultCarLocal.Mvaf3Voltage2,

                tensionkvterciario1 = resultCarLocal.Mvaf4Voltage1,
                tensionkvterciario2 = resultCarLocal.Mvaf4Voltage2,

                tensionkvaltatension3 = resultCarLocal.Mvaf1Voltage3,
                tensionkvaltatension4 = resultCarLocal.Mvaf1Voltage4,

                tensionkvbajatension3 = resultCarLocal.Mvaf2Voltage3,
                tensionkvbajatension4 = resultCarLocal.Mvaf2Voltage4,

                tensionkvsegundabaja3 = resultCarLocal.Mvaf3Voltage3,
                tensionkvsegundabaja4 = resultCarLocal.Mvaf3Voltage4,

                tensionkvterciario3 = resultCarLocal.Mvaf4Voltage3,
                tensionkvterciario4 = resultCarLocal.Mvaf4Voltage4,
                
            };
            return result;
        }
        #endregion
    }
}
