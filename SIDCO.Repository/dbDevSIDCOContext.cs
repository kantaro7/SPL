using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

using SIDCO.Infrastructure.Entities;
using SIDCO.Infrastructure.Functions;

#nullable disable

namespace SIDCO.Infrastructure
{
    public partial class dbDevSIDCOContext : DbContext
    {
        public dbDevSIDCOContext()
        {
        }

        public dbDevSIDCOContext(DbContextOptions<dbDevSIDCOContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CoreCatalog> CoreCatalogs { get; set; }
        public virtual DbSet<DcoArrester> DcoArresters { get; set; }
        public virtual DbSet<DcoBushing> DcoBushings { get; set; }
        public virtual DbSet<DcoBushingDatum> DcoBushingData { get; set; }
        public virtual DbSet<DcoChanger> DcoChangers { get; set; }
        public virtual DbSet<DcoCharacteristic> DcoCharacteristics { get; set; }
        public virtual DbSet<DcoCustomer> DcoCustomers { get; set; }
        public virtual DbSet<DcoGaranty> DcoGaranties { get; set; }
        public virtual DbSet<DcoGeneralDatum> DcoGeneralData { get; set; }
        public virtual DbSet<DcoLabTestMain> DcoLabTestMains { get; set; }
        public virtual DbSet<DcoMvaCharacteristic> DcoMvaCharacteristics { get; set; }
        public virtual DbSet<DcoMvaColumnType> DcoMvaColumnTypes { get; set; }
        public virtual DbSet<DcoMvaList> DcoMvaLists { get; set; }
        public virtual DbSet<DcoOrder> DcoOrders { get; set; }
        public virtual DbSet<DcoSpecialRequirement> DcoSpecialRequirements { get; set; }
        public virtual DbSet<DcoSubmittalsDoc> DcoSubmittalsDocs { get; set; }
        public virtual DbSet<SplDesplazamientoAngular> SplDesplazamientoAngulars { get; set; }
        public virtual DbSet<SplIdioma> SplIdiomas { get; set; }
        public virtual DbSet<SplNorma> SplNormas { get; set; }
        public virtual DbSet<SplNormasrep> SplNormasreps { get; set; }
        public virtual DbSet<SplTipoUnidad> SplTipoUnidads { get; set; }

    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile("appsettings.json", optional: false);
                var configuration = builder.Build();

                #if RELEASE
                             optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
                #else
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
                #endif

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CoreCatalog>(entity =>
            {
                entity.ToTable("CORE_CATALOGS");

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ID");

                entity.Property(e => e.AparentId)
                    .HasMaxLength(4000)
                    .IsUnicode(false)
                    .HasColumnName("APARENT_ID");

                entity.Property(e => e.AttributeId)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ATTRIBUTE_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.ParentId)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("PARENT_ID");
            });

            modelBuilder.Entity<DcoArrester>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DCO_ARRESTER");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE");

                entity.Property(e => e.ArresterId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ARRESTER_ID");

                entity.Property(e => e.BaseIns)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BASE_INS");

                entity.Property(e => e.BrandId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BRAND_ID");

                entity.Property(e => e.BrandOther)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BRAND_OTHER");

                entity.Property(e => e.ColumnTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COLUMN_TYPE_ID");

                entity.Property(e => e.CountDown)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COUNT_DOWN");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATION_DATE")
                    .HasComment("INDIVIDUAL=1, CORRIDO=0");

                entity.Property(e => e.CreationUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.DistanceLeak)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("DISTANCE_LEAK");

                entity.Property(e => e.DistanceLeak2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("DISTANCE_LEAK2");

                entity.Property(e => e.LandNet2Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LAND_NET2_ID");

                entity.Property(e => e.LandNetId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LAND_NET_ID");

                entity.Property(e => e.MaterialId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MATERIAL_ID");

                entity.Property(e => e.MaterialOther)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("MATERIAL_OTHER");

                entity.Property(e => e.McovId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MCOV_ID");

                entity.Property(e => e.McovId2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MCOV_ID2");

                entity.Property(e => e.McovOther)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MCOV_OTHER");

                entity.Property(e => e.McovOther2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MCOV_OTHER2");

                entity.Property(e => e.ModificationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("MODIFICATION_DATETIME");

                entity.Property(e => e.ModificationUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICATION_USER");

                entity.Property(e => e.OrderId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.QtyId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("QTY_ID");

                entity.Property(e => e.SopId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SOP_ID");

                entity.Property(e => e.SopType)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SOP_TYPE");

                entity.Property(e => e.TypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TYPE_ID");

                entity.Property(e => e.TypeOther)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TYPE_OTHER");

                entity.Property(e => e.VoltageId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("VOLTAGE_ID");

                entity.Property(e => e.VoltageId2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("VOLTAGE_ID2");

                entity.Property(e => e.VoltageOther)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("VOLTAGE_OTHER");

                entity.Property(e => e.VoltageOther2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("VOLTAGE_OTHER2");
            });

            modelBuilder.Entity<DcoBushing>(entity =>
            {
                entity.HasKey(e => e.BushingId)
                    .HasName("DCO_BUSHING_PK");

                entity.ToTable("DCO_BUSHING");

                entity.Property(e => e.BushingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BUSHING_ID");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATION_DATETIME")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreationUser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.ModificationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("MODIFICATION_DATETIME");

                entity.Property(e => e.ModificationUser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICATION_USER");

                entity.Property(e => e.OrderId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.StandardId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("STANDARD_ID");

                entity.Property(e => e.StandardOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("STANDARD_OTHER");
            });

            modelBuilder.Entity<DcoBushingDatum>(entity =>
            {
                entity.HasKey(e => e.BushingDataId)
                    .HasName("DCO_BUSHING_DATA_PK");

                entity.ToTable("DCO_BUSHING_DATA");

                entity.Property(e => e.BushingDataId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BUSHING_DATA_ID")
                    .HasComment("PK");

                entity.Property(e => e.AcometidaId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACOMETIDA_ID");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE")
                    .HasComment("1");

                entity.Property(e => e.ApplicationId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("APPLICATION_ID");

                entity.Property(e => e.BilClass)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("BIL_CLASS");

                entity.Property(e => e.BilClassOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BIL_CLASS_OTHER");

                entity.Property(e => e.BilespId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BILESP_ID");

                entity.Property(e => e.BilespOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BILESP_OTHER");

                entity.Property(e => e.BoquilladosfasesId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BOQUILLADOSFASES_ID");

                entity.Property(e => e.BoquilladosfasesOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BOQUILLADOSFASES_OTHER");

                entity.Property(e => e.BoquillaespId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BOQUILLAESP_ID");

                entity.Property(e => e.BoquillaespOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BOQUILLAESP_OTHER");

                entity.Property(e => e.BrandId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BRAND_ID");

                entity.Property(e => e.BrandOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BRAND_OTHER");

                entity.Property(e => e.Bushing)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BUSHING");

                entity.Property(e => e.BushingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BUSHING_ID")
                    .HasComment("FK");

                entity.Property(e => e.CatalogoboqId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CATALOGOBOQ_ID");

                entity.Property(e => e.ColorId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COLOR_ID");

                entity.Property(e => e.ColorOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("COLOR_OTHER");

                entity.Property(e => e.ColumnTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COLUMN_TYPE_ID")
                    .HasComment("FK");

                entity.Property(e => e.ConexioninternaId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CONEXIONINTERNA_ID");

                entity.Property(e => e.ConnectorId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CONNECTOR_ID");

                entity.Property(e => e.ConnectorOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONNECTOR_OTHER");

                entity.Property(e => e.CorrienteespId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CORRIENTEESP_ID");

                entity.Property(e => e.CorrienteespOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CORRIENTEESP_OTHER");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATION_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreationUser)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.CtpocketmininId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CTPOCKETMININ_ID");

                entity.Property(e => e.CtpocketminmmId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CTPOCKETMINMM_ID");

                entity.Property(e => e.CurrentAmp)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CURRENT_AMP");

                entity.Property(e => e.CurrentAmpReq)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CURRENT_AMP_REQ");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.FugammId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FUGAMM_ID");

                entity.Property(e => e.Kvln)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("KVLN");

                entity.Property(e => e.LandNet2Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LAND_NET2_ID");

                entity.Property(e => e.LandNetId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LAND_NET_ID");

                entity.Property(e => e.LeakageDistanceId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LEAKAGE_DISTANCE_ID");

                entity.Property(e => e.LeakageOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LEAKAGE_OTHER");

                entity.Property(e => e.Localization1Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LOCALIZATION1_ID");

                entity.Property(e => e.Localization2Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LOCALIZATION2_ID");

                entity.Property(e => e.Localization3Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LOCALIZATION3_ID");

                entity.Property(e => e.MaterialId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MATERIAL_ID");

                entity.Property(e => e.ModificationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("MODIFICATION_DATETIME");

                entity.Property(e => e.ModificationUser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICATION_USER");

                entity.Property(e => e.MontajeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MONTAJE_ID");

                entity.Property(e => e.MontajeOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MONTAJE_OTHER");

                entity.Property(e => e.MsnmId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MSNM_ID");

                entity.Property(e => e.QtyId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("QTY_ID");

                entity.Property(e => e.SubTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SUB_TYPE_ID");

                entity.Property(e => e.SubTypeOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SUB_TYPE_OTHER");

                entity.Property(e => e.TiempoentregaId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TIEMPOENTREGA_ID");

                entity.Property(e => e.TiempoentregaOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TIEMPOENTREGA_OTHER");

                entity.Property(e => e.TypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TYPE_ID");

                entity.Property(e => e.TypeOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TYPE_OTHER");

                entity.Property(e => e.UsoId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("USO_ID");

                entity.Property(e => e.UsoOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("USO_OTHER");

                entity.Property(e => e.VoltageClass)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("VOLTAGE_CLASS");

                entity.Property(e => e.VoltageClassOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("VOLTAGE_CLASS_OTHER");
            });

            modelBuilder.Entity<DcoChanger>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DCO_CHANGERS");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE");

                entity.Property(e => e.BilId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BIL_ID");

                entity.Property(e => e.BilOther)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("BIL_OTHER");

                entity.Property(e => e.BrandId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BRAND_ID");

                entity.Property(e => e.BrandOther)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("BRAND_OTHER");

                entity.Property(e => e.CapInt)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CAP_INT");

                entity.Property(e => e.Changer)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CHANGER");

                entity.Property(e => e.ChangerId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CHANGER_ID");

                entity.Property(e => e.ColumnTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COLUMN_TYPE_ID");

                entity.Property(e => e.ContactFinishId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CONTACT_FINISH_ID");

                entity.Property(e => e.ContactFinishOther)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("CONTACT_FINISH_OTHER");

                entity.Property(e => e.CreationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATION_DATETIME");

                entity.Property(e => e.CreationUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.CtrAutOther)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CTR_AUT_OTHER");

                entity.Property(e => e.CtrlAutId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CTRL_AUT_ID");

                entity.Property(e => e.CtrlEmpId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CTRL_EMP_ID");

                entity.Property(e => e.CtrlEmpOther)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CTRL_EMP_OTHER");

                entity.Property(e => e.Deriv2Other)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("DERIV2_OTHER");

                entity.Property(e => e.DerivId)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("DERIV_ID");

                entity.Property(e => e.DerivId2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("DERIV_ID2");

                entity.Property(e => e.DerivOther)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("DERIV_OTHER");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Imax)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("IMAX");

                entity.Property(e => e.LineNeutral)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LINE_NEUTRAL");

                entity.Property(e => e.LocationId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LOCATION_ID");

                entity.Property(e => e.LocationOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION_OTHER");

                entity.Property(e => e.MechanismHeightId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MECHANISM_HEIGHT_ID");

                entity.Property(e => e.MechanismHeightInf)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MECHANISM_HEIGHT_INF");

                entity.Property(e => e.MechanismHeightOther)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("MECHANISM_HEIGHT_OTHER");

                entity.Property(e => e.MechanismHeightSup)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MECHANISM_HEIGHT_SUP");

                entity.Property(e => e.ModelId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MODEL_ID");

                entity.Property(e => e.ModelOther)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("MODEL_OTHER");

                entity.Property(e => e.ModificationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("MODIFICATION_DATETIME");

                entity.Property(e => e.ModificationUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICATION_USER");

                entity.Property(e => e.OperationId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("OPERATION_ID");

                entity.Property(e => e.OrderId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.RcbnFcbn)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("RCBN_FCBN");

                entity.Property(e => e.RowIndex)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ROW_INDEX");

                entity.Property(e => e.Taps)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TAPS");

                entity.Property(e => e.TrafoRelation)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TRAFO_RELATION");

                entity.Property(e => e.TrafoSerieId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TRAFO_SERIE_ID");

                entity.Property(e => e.Type)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TYPE");

                entity.Property(e => e.TypeEngineId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TYPE_ENGINE_ID");

                entity.Property(e => e.TypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TYPE_ID");

                entity.Property(e => e.TypeOther)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("TYPE_OTHER");
            });

            modelBuilder.Entity<DcoCharacteristic>(entity =>
            {
                entity.HasKey(e => e.CharacteristicsId)
                    .HasName("DCO_CHARACTERISTICS_PK");

                entity.ToTable("DCO_CHARACTERISTICS");

                entity.Property(e => e.CharacteristicsId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CHARACTERISTICS_ID")
                    .HasComment("PK");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE");

                entity.Property(e => e.AltitudeF1Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ALTITUDE_F1_ID");

                entity.Property(e => e.AltitudeF1Other)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ALTITUDE_F1_OTHER");

                entity.Property(e => e.AltitudeF2Id)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ALTITUDE_F2_ID");

                entity.Property(e => e.AltitudeF2Other)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ALTITUDE_F2_OTHER");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATION_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreationUser)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.DevAwr)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DEV_AWR");

                entity.Property(e => e.FrequencyId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FREQUENCY_ID");

                entity.Property(e => e.HstrId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("HSTR_ID");

                entity.Property(e => e.HstrOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("HSTR_OTHER");

                entity.Property(e => e.IsolatorIncluded)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ISOLATOR_INCLUDED")
                    .HasComment("Boolean");

                entity.Property(e => e.LiquidIsolatorId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LIQUID_ISOLATOR_ID");

                entity.Property(e => e.LiquidIsolatorOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LIQUID_ISOLATOR_OTHER");

                entity.Property(e => e.ModificationDate)
                    .HasColumnType("date")
                    .HasColumnName("MODIFICATION_DATE");

                entity.Property(e => e.ModificationUser)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICATION_USER");

                entity.Property(e => e.NucleousElevation)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("NUCLEOUS_ELEVATION");

                entity.Property(e => e.OilDefinedbyId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("OIL_DEFINEDBY_ID");

                entity.Property(e => e.OilElevation)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("OIL_ELEVATION");

                entity.Property(e => e.OilLocationId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("OIL_LOCATION_ID");

                entity.Property(e => e.OilPreservationId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("OIL_PRESERVATION_ID");

                entity.Property(e => e.OrderId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_ID")
                    .HasComment("ORDER FK");

                entity.Property(e => e.PhaseId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PHASE_ID");

                entity.Property(e => e.StandardIsolatorId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("STANDARD_ISOLATOR_ID");

                entity.Property(e => e.StandardIsolatorOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("STANDARD_ISOLATOR_OTHER");

                entity.Property(e => e.TankElevation)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("TANK_ELEVATION");
            });

            modelBuilder.Entity<DcoCustomer>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("DCO_CUSTOMERS_PK");

                entity.ToTable("DCO_CUSTOMERS");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CUSTOMER_ID");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE");

                entity.Property(e => e.CnFlag)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CN_FLAG")
                    .HasDefaultValueSql("((0))")
                    .HasComment("CUSTOMER NAME FLAG USE");

                entity.Property(e => e.CustomerBaan)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER_BAAN");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER_NAME");

                entity.Property(e => e.FuFlag)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FU_FLAG")
                    .HasDefaultValueSql("((0))")
                    .HasComment("FINAL USER FLAG");

                entity.Property(e => e.IFlag)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("I_FLAG")
                    .HasDefaultValueSql("((0))")
                    .HasComment("INTERMEDIARY FLAG");
            });

            modelBuilder.Entity<DcoGaranty>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DCO_GARANTY");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE");

                entity.Property(e => e.CreationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATION_DATETIME");

                entity.Property(e => e.CreationUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.GarantyId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("GARANTY_ID");

                entity.Property(e => e.Iexc100)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("IEXC_100");

                entity.Property(e => e.Iexc110)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("IEXC_110");

                entity.Property(e => e.InducCl)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("INDUC_CL");

                entity.Property(e => e.Kwaux1)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWAUX_1");

                entity.Property(e => e.Kwaux2)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWAUX_2");

                entity.Property(e => e.Kwaux3)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWAUX_3");

                entity.Property(e => e.Kwaux4)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWAUX_4");

                entity.Property(e => e.Kwcu)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("KWCU");

                entity.Property(e => e.KwcuKv)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWCU_KV");

                entity.Property(e => e.KwcuMva)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWCU_MVA");

                entity.Property(e => e.Kwfe100)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWFE_100");

                entity.Property(e => e.Kwfe110)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWFE_110");

                entity.Property(e => e.Kwtot100)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWTOT_100");

                entity.Property(e => e.Kwtot110)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWTOT_110");

                entity.Property(e => e.ModificationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("MODIFICATION_DATETIME");

                entity.Property(e => e.ModificationUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICATION_USER");

                entity.Property(e => e.NoiseFa1)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_FA1");

                entity.Property(e => e.NoiseFa2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_FA2");

                entity.Property(e => e.NoiseLowFa1)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_LOW_FA1");

                entity.Property(e => e.NoiseLowFa2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_LOW_FA2");

                entity.Property(e => e.NoiseLowOa)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_LOW_OA");

                entity.Property(e => e.NoiseNemaFa1)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_NEMA_FA1");

                entity.Property(e => e.NoiseNemaFa2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_NEMA_FA2");

                entity.Property(e => e.NoiseNemaOa)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_NEMA_OA");

                entity.Property(e => e.NoiseOa)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_OA");

                entity.Property(e => e.OrderId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PadsOut)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("PADS_OUT");

                entity.Property(e => e.PanelOut)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("PANEL_OUT");

                entity.Property(e => e.PenaltyAux)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PENALTY_AUX");

                entity.Property(e => e.PenaltyCu)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PENALTY_CU");

                entity.Property(e => e.PenaltyCurrency)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PENALTY_CURRENCY");

                entity.Property(e => e.PenaltyCurrencyOther)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PENALTY_CURRENCY_OTHER");

                entity.Property(e => e.PenaltyKwfe)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PENALTY_KWFE");

                entity.Property(e => e.PenaltyMvaAux)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PENALTY_MVA_AUX");

                entity.Property(e => e.PenaltyMvaCu)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PENALTY_MVA_CU");

                entity.Property(e => e.PenaltyTot)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PENALTY_TOT");

                entity.Property(e => e.PowerFactor)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("POWER_FACTOR");

                entity.Property(e => e.ReacCl)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("REAC_CL");

                entity.Property(e => e.RelXr)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("REL_XR");

                entity.Property(e => e.ResOhm)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("RES_OHM");

                entity.Property(e => e.ResinEpoxy)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("RESIN_EPOXY");

                entity.Property(e => e.RestraintPadsInn)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("RESTRAINT_PADS_INN");

                entity.Property(e => e.SaveData)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SAVE_DATA");

                entity.Property(e => e.TolerancyKwAux)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_KW_AUX");

                entity.Property(e => e.TolerancyKwCu)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TOLERANCY_KW_CU");

                entity.Property(e => e.TolerancyKwfe)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_KWFE");

                entity.Property(e => e.TolerancyKwtot)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_KWTOT");

                entity.Property(e => e.TolerancyReac)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_REAC");

                entity.Property(e => e.TolerancyZdblvoltage)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_ZDBLVOLTAGE");

                entity.Property(e => e.TolerancyZdblvoltage2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_ZDBLVOLTAGE2");

                entity.Property(e => e.TolerancyZpositive)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TOLERANCY_ZPOSITIVE");

                entity.Property(e => e.TolerancyZpositive2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TOLERANCY_ZPOSITIVE2");

                entity.Property(e => e.TolerancyZzero)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_ZZERO");

                entity.Property(e => e.TolerancyZzero2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_ZZERO2");

                entity.Property(e => e.TolerancyZzerodblvoltage)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_ZZERODBLVOLTAGE");

                entity.Property(e => e.TolerancyZzerodblvoltage2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_ZZERODBLVOLTAGE2");

                entity.Property(e => e.VibrationHz)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("VIBRATION_HZ");

                entity.Property(e => e.VibrationMic)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("VIBRATION_MIC");

                entity.Property(e => e.ZDblvoltageHz)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_DBLVOLTAGE_HZ");

                entity.Property(e => e.ZDblvoltageMva)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_DBLVOLTAGE_MVA");

                entity.Property(e => e.ZDblvoltageXz)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_DBLVOLTAGE_XZ");

                entity.Property(e => e.ZDblvoltageYz)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_DBLVOLTAGE_YZ");

                entity.Property(e => e.ZPositiveHx)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("Z_POSITIVE_HX");

                entity.Property(e => e.ZPositiveHy)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("Z_POSITIVE_HY");

                entity.Property(e => e.ZPositiveMva)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("Z_POSITIVE_MVA");

                entity.Property(e => e.ZPositiveTerc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_POSITIVE_TERC");

                entity.Property(e => e.ZPositiveXy)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("Z_POSITIVE_XY");

                entity.Property(e => e.ZZeroDblvoltageHz)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_ZERO_DBLVOLTAGE_HZ");

                entity.Property(e => e.ZZeroDblvoltageMva)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_ZERO_DBLVOLTAGE_MVA");

                entity.Property(e => e.ZZeroDblvoltageXz)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_ZERO_DBLVOLTAGE_XZ");

                entity.Property(e => e.ZZeroDblvoltageYz)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_ZERO_DBLVOLTAGE_YZ");

                entity.Property(e => e.ZZeroHx)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_ZERO_HX");

                entity.Property(e => e.ZZeroHy)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_ZERO_HY");

                entity.Property(e => e.ZZeroMva)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_ZERO_MVA");

                entity.Property(e => e.ZZeroTerc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_ZERO_TERC");

                entity.Property(e => e.ZZeroXy)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("Z_ZERO_XY");
            });

            modelBuilder.Entity<DcoGeneralDatum>(entity =>
            {
                entity.HasKey(e => e.GeneralDataId);

                entity.ToTable("DCO_GENERAL_DATA");

                entity.Property(e => e.GeneralDataId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("GENERAL_DATA_ID");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE");

                entity.Property(e => e.Applicationid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("APPLICATIONID");

                entity.Property(e => e.Bankunitsid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("BANKUNITSID");

                entity.Property(e => e.Cm)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("CM");

                entity.Property(e => e.CreationDate1)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATION_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreationUser)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.Creationdate)
                    .HasColumnType("date")
                    .HasColumnName("CREATIONDATE");

                entity.Property(e => e.Customername)
                    .HasMaxLength(100)
                    .HasColumnName("CUSTOMERNAME");

                entity.Property(e => e.CustomernameId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CUSTOMERNAME_ID");

                entity.Property(e => e.Finaluser)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FINALUSER");

                entity.Property(e => e.FinaluserId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FINALUSER_ID");

                entity.Property(e => e.Intermediary)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("INTERMEDIARY");

                entity.Property(e => e.IntermediaryId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("INTERMEDIARY_ID");

                entity.Property(e => e.ModificationDate)
                    .HasColumnType("date")
                    .HasColumnName("MODIFICATION_DATE");

                entity.Property(e => e.ModificationUser)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICATION_USER");

                entity.Property(e => e.OrderId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.Quantity)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("QUANTITY");

                entity.Property(e => e.Standardid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("STANDARDID");

                entity.Property(e => e.Statusorder)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("STATUSORDER");

                entity.Property(e => e.Typetrafoid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TYPETRAFOID");

                entity.Property(e => e.Userinitials)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("USERINITIALS");
            });

            modelBuilder.Entity<DcoLabTestMain>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DCO_LAB_TEST_MAIN");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE");

                entity.Property(e => e.Bilinducido)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("BILINDUCIDO");

                entity.Property(e => e.CreationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATION_DATETIME");

                entity.Property(e => e.CreationUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.FlagChanger)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FLAG_CHANGER");

                entity.Property(e => e.FlagDeriv)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FLAG_DERIV");

                entity.Property(e => e.LabTestMainId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LAB_TEST_MAIN_ID");

                entity.Property(e => e.MaxBil)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MAX_BIL");

                entity.Property(e => e.MaxMva)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MAX_MVA");

                entity.Property(e => e.MaxVoltage)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MAX_VOLTAGE");

                entity.Property(e => e.ModificationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("MODIFICATION_DATETIME");

                entity.Property(e => e.ModificationUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICATION_USER");

                entity.Property(e => e.OrderId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PhaseId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PHASE_ID");

                entity.Property(e => e.SaveData)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SAVE_DATA");

                entity.Property(e => e.StandardId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("STANDARD_ID");

                entity.Property(e => e.TextTestDielectric)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TEXT_TEST_DIELECTRIC");

                entity.Property(e => e.TextTestId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TEXT_TEST_ID");

                entity.Property(e => e.TextTestPrototype)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TEXT_TEST_PROTOTYPE");

                entity.Property(e => e.TextTestRoutine)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TEXT_TEST_ROUTINE");

                entity.Property(e => e.TrafoType)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TRAFO_TYPE");
            });

            modelBuilder.Entity<DcoMvaCharacteristic>(entity =>
            {
                entity.HasKey(e => e.MvaCharId)
                    .HasName("DCO_MVA_CHARACTERISTICS_PK");

                entity.ToTable("DCO_MVA_CHARACTERISTICS");

                entity.Property(e => e.MvaCharId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVA_CHAR_ID")
                    .HasComment("PK");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CapacitiesNote)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CAPACITIES_NOTE");

                entity.Property(e => e.Mvaf1Changer)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF1_CHANGER");

                entity.Property(e => e.Mvaf1ConnectionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF1_CONNECTION_ID")
                    .HasComment("FK");

                entity.Property(e => e.Mvaf1ConnectionOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MVAF1_CONNECTION_OTHER");

                entity.Property(e => e.Mvaf1DerDown)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_DER_DOWN");

                entity.Property(e => e.Mvaf1DerDown2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF1_DER_DOWN_2");

                entity.Property(e => e.Mvaf1DerUp)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_DER_UP");

                entity.Property(e => e.Mvaf1DerUp2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF1_DER_UP_2");

                entity.Property(e => e.Mvaf1Nbai1)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF1_NBAI1");

                entity.Property(e => e.Mvaf1Nbai2)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF1_NBAI2");

                entity.Property(e => e.Mvaf1NbaiNeutro)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF1_NBAI_NEUTRO");

                entity.Property(e => e.Mvaf1SerPar)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF1_SER_PAR")
                    .HasComment("SER PAR FLAG");

                entity.Property(e => e.Mvaf1Taps)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MVAF1_TAPS");

                entity.Property(e => e.Mvaf1Taps2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF1_TAPS_2");

                entity.Property(e => e.Mvaf1TblSwch)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF1_TBL_SWCH");

                entity.Property(e => e.Mvaf1Voltage1)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_VOLTAGE1")
                    .HasComment("rds value");

                entity.Property(e => e.Mvaf1Voltage2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_VOLTAGE2")
                    .HasComment("voltage 1 / root of 3. they ask to save this field for future reference instead of being calculated everytime");

                entity.Property(e => e.Mvaf1Voltage3)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_VOLTAGE3")
                    .HasComment("Shows when SER PAR = true");

                entity.Property(e => e.Mvaf1Voltage4)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_VOLTAGE4")
                    .HasComment("Shows when SER PAR = true voltage3 / root of 3");

                entity.Property(e => e.Mvaf2Changer)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF2_CHANGER");

                entity.Property(e => e.Mvaf2ConnectionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF2_CONNECTION_ID")
                    .HasComment("FK");

                entity.Property(e => e.Mvaf2ConnectionOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MVAF2_CONNECTION_OTHER");

                entity.Property(e => e.Mvaf2DerDown)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF2_DER_DOWN");

                entity.Property(e => e.Mvaf2DerUp)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF2_DER_UP");

                entity.Property(e => e.Mvaf2Nbai1)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF2_NBAI1");

                entity.Property(e => e.Mvaf2Nbai2)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF2_NBAI2");

                entity.Property(e => e.Mvaf2NbaiNeutro)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF2_NBAI_NEUTRO");

                entity.Property(e => e.Mvaf2SerPar)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF2_SER_PAR");

                entity.Property(e => e.Mvaf2Taps)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MVAF2_TAPS");

                entity.Property(e => e.Mvaf2TblSwch)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF2_TBL_SWCH");

                entity.Property(e => e.Mvaf2Voltage1)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF2_VOLTAGE1");

                entity.Property(e => e.Mvaf2Voltage2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF2_VOLTAGE2");

                entity.Property(e => e.Mvaf2Voltage3)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF2_VOLTAGE3");

                entity.Property(e => e.Mvaf2Voltage4)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF2_VOLTAGE4");

                entity.Property(e => e.Mvaf3Changer)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF3_CHANGER");

                entity.Property(e => e.Mvaf3ConnectionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF3_CONNECTION_ID")
                    .HasComment("FK");

                entity.Property(e => e.Mvaf3ConnectionOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MVAF3_CONNECTION_OTHER");

                entity.Property(e => e.Mvaf3DerDown)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF3_DER_DOWN");

                entity.Property(e => e.Mvaf3DerUp)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF3_DER_UP");

                entity.Property(e => e.Mvaf3Nbai1)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF3_NBAI1");

                entity.Property(e => e.Mvaf3Nbai2)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF3_NBAI2");

                entity.Property(e => e.Mvaf3NbaiNeutro)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF3_NBAI_NEUTRO");

                entity.Property(e => e.Mvaf3SerPar)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF3_SER_PAR");

                entity.Property(e => e.Mvaf3Taps)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MVAF3_TAPS");

                entity.Property(e => e.Mvaf3TblSwch)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF3_TBL_SWCH");

                entity.Property(e => e.Mvaf3Voltage1)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF3_VOLTAGE1");

                entity.Property(e => e.Mvaf3Voltage2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF3_VOLTAGE2");

                entity.Property(e => e.Mvaf3Voltage3)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF3_VOLTAGE3");

                entity.Property(e => e.Mvaf3Voltage4)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF3_VOLTAGE4");

                entity.Property(e => e.Mvaf4Changer)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF4_CHANGER");

                entity.Property(e => e.Mvaf4ConnectionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF4_CONNECTION_ID")
                    .HasComment("FK");

                entity.Property(e => e.Mvaf4ConnectionOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MVAF4_CONNECTION_OTHER");

                entity.Property(e => e.Mvaf4DerDown)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF4_DER_DOWN");

                entity.Property(e => e.Mvaf4DerUp)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF4_DER_UP");

                entity.Property(e => e.Mvaf4Nbai1)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF4_NBAI1");

                entity.Property(e => e.Mvaf4Nbai2)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF4_NBAI2");

                entity.Property(e => e.Mvaf4NbaiNeutro)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF4_NBAI_NEUTRO");

                entity.Property(e => e.Mvaf4SerPar)
                    .HasMaxLength(4000)
                    .IsUnicode(false)
                    .HasColumnName("MVAF4_SER_PAR");

                entity.Property(e => e.Mvaf4Taps)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MVAF4_TAPS");

                entity.Property(e => e.Mvaf4TblSwch)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF4_TBL_SWCH");

                entity.Property(e => e.Mvaf4Voltage1)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF4_VOLTAGE1");

                entity.Property(e => e.Mvaf4Voltage2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF4_VOLTAGE2");

                entity.Property(e => e.Mvaf4Voltage3)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF4_VOLTAGE3");

                entity.Property(e => e.Mvaf4Voltage4)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF4_VOLTAGE4");

                entity.Property(e => e.OrderId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_ID")
                    .HasComment("FK");

                entity.Property(e => e.RcbnFcbn)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("RCBN_FCBN")
                    .HasComment("RCBN  = 0 FCBN = 1");

                entity.Property(e => e.RcbnFcbn12)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("RCBN_FCBN1_2");

                entity.Property(e => e.RcbnFcbn2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("RCBN_FCBN2");

                entity.Property(e => e.RcbnFcbn3)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("RCBN_FCBN3");

                entity.Property(e => e.RcbnFcbn4)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("RCBN_FCBN4");
            });

            modelBuilder.Entity<DcoMvaColumnType>(entity =>
            {
                entity.HasKey(e => new { e.ColumnTypeId, e.Type })
                    .HasName("DCO_MVA_COLUMN_TYPE_PK");

                entity.ToTable("DCO_MVA_COLUMN_TYPE");

                entity.Property(e => e.ColumnTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COLUMN_TYPE_ID")
                    .HasComment("PK");

                entity.Property(e => e.Type)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TYPE");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ColumnTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COLUMN_TITLE");

                entity.Property(e => e.ConnectionCondition)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONNECTION_CONDITION")
                    .HasComment("EXTRA CONDITIONS");

                entity.Property(e => e.ConnectionField)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CONNECTION_FIELD");

                entity.Property(e => e.Derivation2Field)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DERIVATION2_FIELD");

                entity.Property(e => e.DerivationField)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DERIVATION_FIELD");

                entity.Property(e => e.DeviceId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("DEVICE_ID")
                    .HasComment("FK. Transformer type (no the catalog one)");

                entity.Property(e => e.MvaField)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MVA_FIELD");

                entity.Property(e => e.NbaiField)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NBAI_FIELD");

                entity.Property(e => e.OrderIndex)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_INDEX")
                    .HasComment("ORDER");

                entity.Property(e => e.RcbnFcbnField)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RCBN_FCBN_FIELD");

                entity.Property(e => e.SectionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SECTION_ID");

                entity.Property(e => e.SerParField)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SER_PAR_FIELD");

                entity.Property(e => e.TapsField)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TAPS_FIELD");

                entity.Property(e => e.Voltage2Field)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("VOLTAGE2_FIELD");

                entity.Property(e => e.VoltageField)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("VOLTAGE_FIELD");
            });

            modelBuilder.Entity<DcoMvaList>(entity =>
            {
                entity.HasKey(e => e.MvaId)
                    .HasName("DCO_MVA_LIST_PK");

                entity.ToTable("DCO_MVA_LIST");

                entity.Property(e => e.MvaId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVA_ID")
                    .HasComment("PK");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CoolingTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COOLING_TYPE_ID");

                entity.Property(e => e.CoolingTypeOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE_OTHER");

                entity.Property(e => e.DevAwr)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("DEV_AWR");

                entity.Property(e => e.HstrId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("HSTR_ID");

                entity.Property(e => e.HstrOther)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("HSTR_OTHER");

                entity.Property(e => e.Mvaf1)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("MVAF1");

                entity.Property(e => e.Mvaf2)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("MVAF2");

                entity.Property(e => e.Mvaf3)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("MVAF3");

                entity.Property(e => e.Mvaf4)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("MVAF4")
                    .HasComment("TERCIARIO");

                entity.Property(e => e.OrCoolingTypeId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OR_COOLING_TYPE_ID");

                entity.Property(e => e.OrDevAwr)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OR_DEV_AWR");

                entity.Property(e => e.OrHstrId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OR_HSTR_ID");

                entity.Property(e => e.OrMvaf1)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OR_MVAF1");

                entity.Property(e => e.OrMvaf2)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OR_MVAF2");

                entity.Property(e => e.OrMvaf3)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OR_MVAF3");

                entity.Property(e => e.OrMvaf4)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OR_MVAF4");

                entity.Property(e => e.OrOverElevationId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OR_OVER_ELEVATION_ID");

                entity.Property(e => e.OrderId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_ID")
                    .HasComment("FK");

                entity.Property(e => e.OverElevationId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("OVER_ELEVATION_ID");

                entity.Property(e => e.OverElevationOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OVER_ELEVATION_OTHER");
            });

            modelBuilder.Entity<DcoOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.ToTable("DCO_ORDER");

                entity.Property(e => e.OrderId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE");

                entity.Property(e => e.Comments)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("COMMENTS");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("date")
                    .HasColumnName("CREATION_DATE");

                entity.Property(e => e.CreationUser)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.FinalConsecutive)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("FINAL_CONSECUTIVE");

                entity.Property(e => e.IdSpct)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ID_SPCT");

                entity.Property(e => e.InitialConsecutive)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("INITIAL_CONSECUTIVE");

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                entity.Property(e => e.Origin)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORIGIN")
                    .HasComment("CATALOG ID FOR RDS OR MANUAL");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnType("date")
                    .HasColumnName("RELEASE_DATE");

                entity.Property(e => e.ReleaseUser)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("RELEASE_USER");

                entity.Property(e => e.ReportDi)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("REPORT_DI");

                entity.Property(e => e.Revision)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("REVISION");

                entity.Property(e => e.RevisionUser)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("REVISION_USER");

                entity.Property(e => e.StatusId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("STATUS_ID");
            });

            modelBuilder.Entity<DcoSpecialRequirement>(entity =>
            {
                entity.HasKey(e => e.SpecialReqId);

                entity.ToTable("DCO_SPECIAL_REQUIREMENTS");

                entity.Property(e => e.SpecialReqId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SPECIAL_REQ_ID");

                entity.Property(e => e.AEmb)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("A_EMB");

                entity.Property(e => e.AOpe)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("A_OPE");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CapExtraBoq)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CAP_EXTRA_BOQ");

                entity.Property(e => e.CapExtraBoqOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CAP_EXTRA_BOQ_OTHER");

                entity.Property(e => e.CapExtraCamCCarga)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CAP_EXTRA_CAM_C_CARGA");

                entity.Property(e => e.CapExtraCamCCargaOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CAP_EXTRA_CAM_C_CARGA_OTHER");

                entity.Property(e => e.CapExtraCamSCarga)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CAP_EXTRA_CAM_S_CARGA");

                entity.Property(e => e.CapExtraCamSCargaOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CAP_EXTRA_CAM_S_CARGA_OTHER");

                entity.Property(e => e.CapExtraComUnit)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CAP_EXTRA_COM_UNIT");

                entity.Property(e => e.CapExtraComUnitOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CAP_EXTRA_COM_UNIT_OTHER");

                entity.Property(e => e.CapExtraTc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CAP_EXTRA_TC");

                entity.Property(e => e.CapExtraTcOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CAP_EXTRA_TC_OTHER");

                entity.Property(e => e.CcStdId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CC_STD_ID");

                entity.Property(e => e.CcStdOther)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("CC_STD_OTHER");

                entity.Property(e => e.Consultant)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("CONSULTANT");

                entity.Property(e => e.Crc)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("CRC");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATION_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreationUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.CrimperId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CRIMPER_ID");

                entity.Property(e => e.CrimperOther)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("CRIMPER_OTHER");

                entity.Property(e => e.EarthquakeCertificationId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("EARTHQUAKE_CERTIFICATION_ID");

                entity.Property(e => e.EarthquakeCertificationOther)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("EARTHQUAKE_CERTIFICATION_OTHER");

                entity.Property(e => e.FluidConditionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FLUID_CONDITION_ID");

                entity.Property(e => e.FluidConditionOther)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FLUID_CONDITION_OTHER");

                entity.Property(e => e.Format)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("FORMAT");

                entity.Property(e => e.FreeBucklingId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FREE_BUCKLING_ID");

                entity.Property(e => e.FreeBucklingOther)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("FREE_BUCKLING_OTHER");

                entity.Property(e => e.HEmbca)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("H_EMBCA");

                entity.Property(e => e.HEmbsa)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("H_EMBSA");

                entity.Property(e => e.HOpe)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("H_OPE");

                entity.Property(e => e.LEmb)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("L_EMB");

                entity.Property(e => e.LOpe)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("L_OPE");

                entity.Property(e => e.MaxContinousOperation)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("MAX_CONTINOUS_OPERATION");

                entity.Property(e => e.MaxContinousTempMva)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("MAX_CONTINOUS_TEMP_MVA");

                entity.Property(e => e.MaxContinousTempTmo)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("MAX_CONTINOUS_TEMP_TMO");

                entity.Property(e => e.MaxTempContinousOperation)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("MAX_TEMP_CONTINOUS_OPERATION");

                entity.Property(e => e.ModificationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("MODIFICATION_DATE");

                entity.Property(e => e.ModificationUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICATION_USER");

                entity.Property(e => e.OrderId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.OverExcitationStdId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("OVER_EXCITATION_STD_ID");

                entity.Property(e => e.OverExcitationStdOther)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("OVER_EXCITATION_STD_OTHER");

                entity.Property(e => e.OverloadStdId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("OVERLOAD_STD_ID");

                entity.Property(e => e.OverloadStdOther)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("OVERLOAD_STD_OTHER");

                entity.Property(e => e.PEmbca)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("P_EMBCA");

                entity.Property(e => e.PEmbsa)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("P_EMBSA");

                entity.Property(e => e.POpe)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("P_OPE");

                entity.Property(e => e.ParallelOperationId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PARALLEL_OPERATION_ID");

                entity.Property(e => e.ParallelOperationOther)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("PARALLEL_OPERATION_OTHER");

                entity.Property(e => e.PolarityId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("POLARITY_ID");

                entity.Property(e => e.PolarityOther)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POLARITY_OTHER");

                entity.Property(e => e.SafetyMarginId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SAFETY_MARGIN_ID");

                entity.Property(e => e.SafetyMarginOther)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SAFETY_MARGIN_OTHER");

                entity.Property(e => e.TensionLimit)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("TENSION_LIMIT");

                entity.Property(e => e.VOpe)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("V_OPE");
            });

            modelBuilder.Entity<DcoSubmittalsDoc>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DCO_SUBMITTALS_DOCS");

                entity.Property(e => e.Active)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ACTIVE");

                entity.Property(e => e.CreationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATION_DATETIME");

                entity.Property(e => e.CreationUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CREATION_USER");

                entity.Property(e => e.GeReqNumeric)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("GE_REQ_NUMERIC");

                entity.Property(e => e.LanguageId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LANGUAGE_ID");

                entity.Property(e => e.ModificationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("MODIFICATION_DATETIME");

                entity.Property(e => e.ModificationUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICATION_USER");

                entity.Property(e => e.MomClientNumeric)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MOM_CLIENT_NUMERIC");

                entity.Property(e => e.OrderId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PhotograpsId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PHOTOGRAPS_ID");

                entity.Property(e => e.PhotograpsNumeric)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PHOTOGRAPS_NUMERIC");

                entity.Property(e => e.PoNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PO_NUMBER");

                entity.Property(e => e.SubmittalsDocsId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SUBMITTALS_DOCS_ID");

                entity.Property(e => e.Substation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SUBSTATION");

                entity.Property(e => e.TagNumeric)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TAG_NUMERIC");

                entity.Property(e => e.UnitsId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("UNITS_ID");

                entity.Property(e => e.UnitsOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("UNITS_OTHER");
            });

            modelBuilder.Entity<SplDesplazamientoAngular>(entity =>
            {
                entity.HasKey(e => e.Clave)
                    .HasName("SPL_DESPLAZAMIENTO_ANGULAR_PK");

                entity.ToTable("SPL_DESPLAZAMIENTO_ANGULAR");

                entity.Property(e => e.Clave)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.HWye)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("H_WYE");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.TWye)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("T_WYE");

                entity.Property(e => e.XWye)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("X_WYE");
            });

            modelBuilder.Entity<SplIdioma>(entity =>
            {
                entity.HasKey(e => e.ClaveIdioma)
                    .HasName("SPL_I_PK");

                entity.ToTable("SPL_IDIOMAS");

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplNorma>(entity =>
            {
                entity.HasKey(e => e.Clave)
                    .HasName("SPL_N_PK");

                entity.ToTable("SPL_NORMA");

                entity.Property(e => e.Clave)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplNormasrep>(entity =>
            {
                entity.HasKey(e => new { e.ClaveNorma, e.ClaveIdioma, e.Secuencia });

                entity.ToTable("SPL_NORMASREP");

                entity.Property(e => e.ClaveNorma)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_NORMA");

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.Secuencia)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("SECUENCIA");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplTipoUnidad>(entity =>
            {
                entity.HasKey(e => e.Clave)
                    .HasName("SPL_TIPO_UNIDAD_PK");

                entity.ToTable("SPL_TIPO_UNIDAD");

                entity.Property(e => e.Clave)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Modifiadopor)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIADOPOR");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
