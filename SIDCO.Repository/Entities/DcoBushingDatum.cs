using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoBushingDatum
    {
        public decimal BushingDataId { get; set; }
        public decimal BushingId { get; set; }
        public decimal Active { get; set; }
        public decimal? ColumnTypeId { get; set; }
        public decimal? QtyId { get; set; }
        public decimal? BrandId { get; set; }
        public decimal? TypeId { get; set; }
        public decimal? ApplicationId { get; set; }
        public decimal? MaterialId { get; set; }
        public decimal? VoltageClass { get; set; }
        public decimal? BilClass { get; set; }
        public decimal? CurrentAmp { get; set; }
        public string LeakageDistanceId { get; set; }
        public decimal? Localization1Id { get; set; }
        public decimal? Localization2Id { get; set; }
        public decimal? Localization3Id { get; set; }
        public decimal? ColorId { get; set; }
        public decimal? ConnectorId { get; set; }
        public decimal CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string ModificationUser { get; set; }
        public DateTime? ModificationDatetime { get; set; }
        public string BrandOther { get; set; }
        public string ColorOther { get; set; }
        public string ConnectorOther { get; set; }
        public string LeakageOther { get; set; }
        public decimal? SubTypeId { get; set; }
        public string TypeOther { get; set; }
        public string SubTypeOther { get; set; }
        public string BilClassOther { get; set; }
        public string VoltageClassOther { get; set; }
        public decimal? CurrentAmpReq { get; set; }
        public decimal? AcometidaId { get; set; }
        public decimal? LandNetId { get; set; }
        public decimal? LandNet2Id { get; set; }
        public string Bushing { get; set; }
        public decimal? Kvln { get; set; }
        public string Description { get; set; }
        public decimal? UsoId { get; set; }
        public string UsoOther { get; set; }
        public decimal? MontajeId { get; set; }
        public string MontajeOther { get; set; }
        public decimal? BilespId { get; set; }
        public string BilespOther { get; set; }
        public decimal? CorrienteespId { get; set; }
        public string CorrienteespOther { get; set; }
        public decimal? BoquillaespId { get; set; }
        public string BoquillaespOther { get; set; }
        public decimal? TiempoentregaId { get; set; }
        public string TiempoentregaOther { get; set; }
        public decimal? BoquilladosfasesId { get; set; }
        public string BoquilladosfasesOther { get; set; }
        public decimal? MsnmId { get; set; }
        public decimal? FugammId { get; set; }
        public decimal? CtpocketminmmId { get; set; }
        public decimal? CtpocketmininId { get; set; }
        public string CatalogoboqId { get; set; }
        public decimal? ConexioninternaId { get; set; }
    }
}
