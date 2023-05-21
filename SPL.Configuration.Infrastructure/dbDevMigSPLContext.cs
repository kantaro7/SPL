
namespace SPL.Configuration.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using SPL.Configuration.Infrastructure.Entities;

    public partial class dbDevMigSPLContext : DbContext
    {
        public dbDevMigSPLContext()
        {
        }

        public dbDevMigSPLContext(DbContextOptions<dbDevMigSPLContext> options)
            : base(options)
        {
        }

        #region orm
        public virtual DbSet<SplInfoDetalleEtd> SplInfoDetalleEtds { get; set; }
        public virtual DbSet<SplInfoGeneralEtd> SplInfoGeneralEtds { get; set; }
        public virtual DbSet<ExtensionesArchivo> ExtensionesArchivos { get; set; }
        public virtual DbSet<PesoArchivo> PesoArchivos { get; set; }
        public virtual DbSet<SplAsignacionUsuario> SplAsignacionUsuarios { get; set; }
        public virtual DbSet<SplCatsidco> SplCatsidcos { get; set; }
        public virtual DbSet<SplCatsidcoOther> SplCatsidcoOthers { get; set; }
        public virtual DbSet<SplConfiguracionReporte> SplConfiguracionReportes { get; set; }
        public virtual DbSet<SplConfiguracionReporteOrd> SplConfiguracionReporteOrds { get; set; }
        public virtual DbSet<SplContgasCgd> SplContgasCgds { get; set; }
        public virtual DbSet<SplCortedetaEst> SplCortedetaEsts { get; set; }
        public virtual DbSet<SplCortegralEst> SplCortegralEsts { get; set; }
        public virtual DbSet<SplCorteseccEst> SplCorteseccEsts { get; set; }
        public virtual DbSet<SplDatosdetEst> SplDatosdetEsts { get; set; }
        public virtual DbSet<SplDatosgralEst> SplDatosgralEsts { get; set; }
        public virtual DbSet<SplDescFactorcor> SplDescFactorcors { get; set; }
        public virtual DbSet<SplDescFactorcorreccion> SplDescFactorcorreccions { get; set; }
        public virtual DbSet<SplDesplazamientoAngular> SplDesplazamientoAngulars { get; set; }
        public virtual DbSet<SplEspecificacion> SplEspecificacions { get; set; }
        public virtual DbSet<SplFactorCorreccion> SplFactorCorreccions { get; set; }
        public virtual DbSet<SplFactorcorEtd> SplFactorcorEtds { get; set; }
        public virtual DbSet<SplFactorcorFpb> SplFactorcorFpbs { get; set; }
        public virtual DbSet<SplFactorcorFpc> SplFactorcorFpcs { get; set; }
        public virtual DbSet<SplFiltrosreporte> SplFiltrosreportes { get; set; }
        public virtual DbSet<SplFrecuencia> SplFrecuencias { get; set; }
        public virtual DbSet<SplIdioma> SplIdiomas { get; set; }
        public virtual DbSet<SplInfoArchivosArf> SplInfoArchivosArves { get; set; }
        public virtual DbSet<SplInfoArchivosInd> SplInfoArchivosInds { get; set; }
        public virtual DbSet<SplInfoArchivosPim> SplInfoArchivosPims { get; set; }
        public virtual DbSet<SplInfoArchivosPir> SplInfoArchivosPirs { get; set; }
        public virtual DbSet<SplInfoCameTdp> SplInfoCameTdps { get; set; }
        public virtual DbSet<SplInfoDetalleArf> SplInfoDetalleArves { get; set; }
        public virtual DbSet<SplInfoDetalleCem> SplInfoDetalleCems { get; set; }
        public virtual DbSet<SplInfoDetalleCgd> SplInfoDetalleCgds { get; set; }
        public virtual DbSet<SplInfoDetalleDpr> SplInfoDetalleDprs { get; set; }
        public virtual DbSet<SplInfoDetalleFpa> SplInfoDetalleFpas { get; set; }
        public virtual DbSet<SplInfoDetalleFpb> SplInfoDetalleFpbs { get; set; }
        public virtual DbSet<SplInfoDetalleFpc> SplInfoDetalleFpcs { get; set; }
        public virtual DbSet<SplInfoDetalleInd> SplInfoDetalleInds { get; set; }
        public virtual DbSet<SplInfoDetalleIsz> SplInfoDetalleIszs { get; set; }
        public virtual DbSet<SplInfoDetalleNra> SplInfoDetalleNras { get; set; }
        public virtual DbSet<SplInfoDetallePce> SplInfoDetallePces { get; set; }
        public virtual DbSet<SplInfoDetallePci> SplInfoDetallePcis { get; set; }
        public virtual DbSet<SplInfoDetallePee> SplInfoDetallePees { get; set; }
        public virtual DbSet<SplInfoDetallePim> SplInfoDetallePims { get; set; }
        public virtual DbSet<SplInfoDetallePir> SplInfoDetallePirs { get; set; }
        public virtual DbSet<SplInfoDetallePlr> SplInfoDetallePlrs { get; set; }
        public virtual DbSet<SplInfoDetallePrd> SplInfoDetallePrds { get; set; }
        public virtual DbSet<SplInfoDetalleRad> SplInfoDetalleRads { get; set; }
        public virtual DbSet<SplInfoDetalleRan> SplInfoDetalleRans { get; set; }
        public virtual DbSet<SplInfoDetalleRct> SplInfoDetalleRcts { get; set; }
        public virtual DbSet<SplInfoDetalleRdd> SplInfoDetalleRdds { get; set; }
        public virtual DbSet<SplInfoDetalleRdt> SplInfoDetalleRdts { get; set; }
        public virtual DbSet<SplInfoDetalleRod> SplInfoDetalleRods { get; set; }
        public virtual DbSet<SplInfoDetalleRye> SplInfoDetalleRyes { get; set; }
        public virtual DbSet<SplInfoDetalleTap> SplInfoDetalleTaps { get; set; }
        public virtual DbSet<SplInfoDetalleTdp> SplInfoDetalleTdps { get; set; }
        public virtual DbSet<SplInfoGeneralArf> SplInfoGeneralArves { get; set; }
        public virtual DbSet<SplInfoGeneralBpc> SplInfoGeneralBpcs { get; set; }
        public virtual DbSet<SplInfoGeneralCem> SplInfoGeneralCems { get; set; }
        public virtual DbSet<SplInfoGeneralCgd> SplInfoGeneralCgds { get; set; }
        public virtual DbSet<SplInfoGeneralDpr> SplInfoGeneralDprs { get; set; }
        public virtual DbSet<SplInfoGeneralFpa> SplInfoGeneralFpas { get; set; }
        public virtual DbSet<SplInfoGeneralFpb> SplInfoGeneralFpbs { get; set; }
        public virtual DbSet<SplInfoGeneralFpc> SplInfoGeneralFpcs { get; set; }
        public virtual DbSet<SplInfoGeneralInd> SplInfoGeneralInds { get; set; }
        public virtual DbSet<SplInfoGeneralIsz> SplInfoGeneralIszs { get; set; }
        public virtual DbSet<SplInfoGeneralNra> SplInfoGeneralNras { get; set; }
        public virtual DbSet<SplInfoGeneralPce> SplInfoGeneralPces { get; set; }
        public virtual DbSet<SplInfoGeneralPci> SplInfoGeneralPcis { get; set; }
        public virtual DbSet<SplInfoGeneralPee> SplInfoGeneralPees { get; set; }
        public virtual DbSet<SplInfoGeneralPim> SplInfoGeneralPims { get; set; }
        public virtual DbSet<SplInfoGeneralPir> SplInfoGeneralPirs { get; set; }
        public virtual DbSet<SplInfoGeneralPlr> SplInfoGeneralPlrs { get; set; }
        public virtual DbSet<SplInfoGeneralPrd> SplInfoGeneralPrds { get; set; }
        public virtual DbSet<SplInfoGeneralRad> SplInfoGeneralRads { get; set; }
        public virtual DbSet<SplInfoGeneralRan> SplInfoGeneralRans { get; set; }
        public virtual DbSet<SplInfoGeneralRct> SplInfoGeneralRcts { get; set; }
        public virtual DbSet<SplInfoGeneralRdd> SplInfoGeneralRdds { get; set; }
        public virtual DbSet<SplInfoGeneralRdt> SplInfoGeneralRdts { get; set; }
        public virtual DbSet<SplInfoGeneralRod> SplInfoGeneralRods { get; set; }
        public virtual DbSet<SplInfoGeneralRye> SplInfoGeneralRyes { get; set; }
        public virtual DbSet<SplInfoGeneralTap> SplInfoGeneralTaps { get; set; }
        public virtual DbSet<SplInfoGeneralTdp> SplInfoGeneralTdps { get; set; }
        public virtual DbSet<SplInfoGeneralTin> SplInfoGeneralTins { get; set; }
        public virtual DbSet<SplInfoGraficaEtd> SplInfoGraficaEtds { get; set; }
        public virtual DbSet<SplInfoLaboratorio> SplInfoLaboratorios { get; set; }
        public virtual DbSet<SplInfoOctava> SplInfoOctavas { get; set; }
        public virtual DbSet<SplInfoRegRye> SplInfoRegRyes { get; set; }
        public virtual DbSet<SplInfoSeccionCgd> SplInfoSeccionCgds { get; set; }
        public virtual DbSet<SplInfoSeccionEtd> SplInfoSeccionEtds { get; set; }
        public virtual DbSet<SplInfoSeccionFpb> SplInfoSeccionFpbs { get; set; }
        public virtual DbSet<SplInfoSeccionFpc> SplInfoSeccionFpcs { get; set; }
        public virtual DbSet<SplInfoSeccionPce> SplInfoSeccionPces { get; set; }
        public virtual DbSet<SplInfoSeccionPci> SplInfoSeccionPcis { get; set; }
        public virtual DbSet<SplInfoSeccionRad> SplInfoSeccionRads { get; set; }
        public virtual DbSet<SplInfoSeccionRod> SplInfoSeccionRods { get; set; }
        public virtual DbSet<SplInfoaparatoApr> SplInfoaparatoAprs { get; set; }
        public virtual DbSet<SplInfoaparatoBoq> SplInfoaparatoBoqs { get; set; }
        public virtual DbSet<SplInfoaparatoBoqdet> SplInfoaparatoBoqdets { get; set; }
        public virtual DbSet<SplInfoaparatoCam> SplInfoaparatoCams { get; set; }
        public virtual DbSet<SplInfoaparatoCap> SplInfoaparatoCaps { get; set; }
        public virtual DbSet<SplInfoaparatoCar> SplInfoaparatoCars { get; set; }
        public virtual DbSet<SplInfoaparatoDg> SplInfoaparatoDgs { get; set; }
        public virtual DbSet<SplInfoaparatoEst> SplInfoaparatoEsts { get; set; }
        public virtual DbSet<SplInfoaparatoGar> SplInfoaparatoGars { get; set; }
        public virtual DbSet<SplInfoaparatoLab> SplInfoaparatoLabs { get; set; }
        public virtual DbSet<SplInfoaparatoNor> SplInfoaparatoNors { get; set; }
        public virtual DbSet<SplInfoaparatoTap> SplInfoaparatoTaps { get; set; }
        public virtual DbSet<SplMarcasBoq> SplMarcasBoqs { get; set; }
        public virtual DbSet<SplNorma> SplNormas { get; set; }
        public virtual DbSet<SplNormasrep> SplNormasreps { get; set; }
        public virtual DbSet<SplOpcione> SplOpciones { get; set; }
        public virtual DbSet<SplPerfile> SplPerfiles { get; set; }
        public virtual DbSet<SplPermiso> SplPermisos { get; set; }
        public virtual DbSet<SplPlantillaBase> SplPlantillaBases { get; set; }
        public virtual DbSet<SplPrueba> SplPruebas { get; set; }
        public virtual DbSet<SplRepConsolidado> SplRepConsolidados { get; set; }
        public virtual DbSet<SplReporte> SplReportes { get; set; }
        public virtual DbSet<SplResistDiseno> SplResistDisenos { get; set; }
        public virtual DbSet<SplSerieParalelo> SplSerieParalelos { get; set; }
        public virtual DbSet<SplTensionPlaca> SplTensionPlacas { get; set; }
        public virtual DbSet<SplTercerDevanadoTipo> SplTercerDevanadoTipos { get; set; }
        public virtual DbSet<SplTipoUnidad> SplTipoUnidads { get; set; }
        public virtual DbSet<SplTiposxmarcaBoq> SplTiposxmarcaBoqs { get; set; }
        public virtual DbSet<SplTitSerieparalelo> SplTitSerieparalelos { get; set; }
        public virtual DbSet<SplTituloColumnasCem> SplTituloColumnasCems { get; set; }
        public virtual DbSet<SplTituloColumnasFpc> SplTituloColumnasFpcs { get; set; }
        public virtual DbSet<SplTituloColumnasRad> SplTituloColumnasRads { get; set; }
        public virtual DbSet<SplTituloColumnasRdt> SplTituloColumnasRdts { get; set; }
        public virtual DbSet<SplUsuario> SplUsuarios { get; set; }
        public virtual DbSet<SplValidationTestsIsz> SplValidationTestsIszs { get; set; }
        public virtual DbSet<SplValorNominal> SplValorNominals { get; set; }
        public virtual DbSet<SysModulo> SysModulos { get; set; }
        public virtual DbSet<TipoArchivo> TipoArchivos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                ConfigurationBuilder builder = new();
                builder.AddJsonFile("appsettings.json", optional: false);
                IConfigurationRoot configuration = builder.Build();
#if RELEASE
                 optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
#else
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
#endif
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            _ = modelBuilder.Entity<ExtensionesArchivo>(entity =>
            {
                _ = entity.ToTable("EXTENSIONES_ARCHIVO");

                _ = entity.Property(e => e.Id).HasColumnName("id");

                _ = entity.Property(e => e.Active).HasColumnName("active");

                _ = entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("extension");

                _ = entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                _ = entity.Property(e => e.TipoArchivo).HasColumnName("tipo_archivo");
            });

            _ = modelBuilder.Entity<PesoArchivo>(entity =>
            {
                _ = entity.ToTable("PESO_ARCHIVO");

                _ = entity.Property(e => e.Id).HasColumnName("id");

                _ = entity.Property(e => e.ExtensionArchivo).HasColumnName("extension_archivo");

                _ = entity.Property(e => e.IdModulo).HasColumnName("id_modulo");

                _ = entity.Property(e => e.MaximoPeso)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("maximo_peso");

                _ = entity.HasOne(d => d.ExtensionArchivoNavigation)
                    .WithMany(p => p.PesoArchivos)
                    .HasForeignKey(d => d.ExtensionArchivo)
                    .HasConstraintName("FK_PESO_ARCHIVO_PESO_ARCHIVO");

                _ = entity.HasOne(d => d.IdModuloNavigation)
                    .WithMany(p => p.PesoArchivos)
                    .HasForeignKey(d => d.IdModulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PESO_ARCHIVO_MODULO");
            });

            _ = modelBuilder.Entity<SplAsignacionUsuario>(entity =>
            {
                _ = entity.HasKey(e => e.UserId);

                _ = entity.ToTable("SPL_ASIGNACION_USUARIOS");

                _ = entity.Property(e => e.UserId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                _ = entity.Property(e => e.ClavePerfil)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PERFIL");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("date")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("date")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplCatsidco>(entity =>
            {
                _ = entity.ToTable("SPL_CATSIDCO");

                _ = entity.HasIndex(e => e.Id, "PK_SPL_CATSIDCO_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.Id)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID");

                _ = entity.Property(e => e.AttributeId)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ATTRIBUTE_ID");

                _ = entity.Property(e => e.ClaveSpl)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_SPL");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplCatsidcoOther>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_CATSIDCO_OTHERS");

                _ = entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Dato)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("DATO");

                _ = entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplConfiguracionReporte>(entity =>
            {
                _ = entity.HasKey(e => new { e.TipoReporte, e.ClavePrueba, e.Apartado, e.Seccion, e.Dato })
                    .HasName("SPL_CR_PK");

                _ = entity.ToTable("SPL_CONFIGURACION_REPORTE");

                _ = entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.ClavePrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Apartado)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("APARTADO");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Dato)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("DATO");

                _ = entity.Property(e => e.Celda)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("CELDA");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Formato)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("FORMATO");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Obtencion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("OBTENCION");

                _ = entity.Property(e => e.TipoDato)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_DATO");
            });

            _ = modelBuilder.Entity<SplConfiguracionReporteOrd>(entity =>
            {
                _ = entity.HasKey(e => e.Dato)
                    .HasName("SPL_CRO_PK");

                _ = entity.ToTable("SPL_CONFIGURACION_REPORTE_ORD");

                _ = entity.Property(e => e.Dato)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("DATO");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Orden)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ORDEN");
            });

            _ = modelBuilder.Entity<SplContgasCgd>(entity =>
            {
                _ = entity.HasKey(e => e.Id);

                _ = entity.ToTable("SPL_CONTGAS_CGD");

                _ = entity.Property(e => e.Id)
                 .HasColumnType("numeric(4, 0)")
                  .ValueGeneratedOnAdd()
                 .HasColumnName("ID");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.LimiteMax)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("LIMITE_MAX");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.TipoAceite)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_ACEITE");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            _ = modelBuilder.Entity<SplCortedetaEst>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_CORTEDETA_EST");

                _ = entity.Property(e => e.IdCorte)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_CORTE");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Resistencia)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("RESISTENCIA");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.TempR)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMP_R");

                _ = entity.Property(e => e.Tiempo)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("TIEMPO");
            });

            _ = modelBuilder.Entity<SplCortegralEst>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_CORTEGRAL_EST");

                _ = entity.Property(e => e.Constante)
                    .HasColumnType("numeric(5, 1)")
                    .HasColumnName("CONSTANTE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.IdCorte)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_CORTE");

                _ = entity.Property(e => e.IdReg)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REG");

                _ = entity.Property(e => e.KwPrueba)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("KW_PRUEBA");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.PrimerCorte)
                    .HasColumnType("datetime")
                    .HasColumnName("PRIMER_CORTE");

                _ = entity.Property(e => e.SegundoCorte)
                    .HasColumnType("datetime")
                    .HasColumnName("SEGUNDO_CORTE");

                _ = entity.Property(e => e.TercerCorte)
                    .HasColumnType("datetime")
                    .HasColumnName("TERCER_CORTE");

                _ = entity.Property(e => e.UltimaHora)
                    .HasColumnType("datetime")
                    .HasColumnName("ULTIMA_HORA");
            });

            _ = modelBuilder.Entity<SplCorteseccEst>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_CORTESECC_EST");

                _ = entity.Property(e => e.AwrC)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AWR_C");

                _ = entity.Property(e => e.AwrE)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AWR_E");

                _ = entity.Property(e => e.CapturaEn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CAPTURA_EN");

                _ = entity.Property(e => e.FactorK)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("FACTOR_K");

                _ = entity.Property(e => e.GradienteCaC)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("GRADIENTE_CA_C");

                _ = entity.Property(e => e.GradienteCaE)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("GRADIENTE_CA_E");

                _ = entity.Property(e => e.HsrC)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("HSR_C");

                _ = entity.Property(e => e.HsrE)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("HSR_E");

                _ = entity.Property(e => e.HstC)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("HST_C");

                _ = entity.Property(e => e.HstE)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("HST_E");

                _ = entity.Property(e => e.IdCorte)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_CORTE");

                _ = entity.Property(e => e.LimiteEst)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("LIMITE_EST");

                _ = entity.Property(e => e.ResistZeroC)
                    .HasColumnType("numeric(8, 5)")
                    .HasColumnName("RESIST_ZERO_C");

                _ = entity.Property(e => e.ResistZeroE)
                    .HasColumnType("numeric(8, 5)")
                    .HasColumnName("RESIST_ZERO_E");

                _ = entity.Property(e => e.Resistencia)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("RESISTENCIA");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.TempAceiteProm)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("TEMP_ACEITE_PROM");

                _ = entity.Property(e => e.TempDevC)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMP_DEV_C");

                _ = entity.Property(e => e.TempDevE)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMP_DEV_E");

                _ = entity.Property(e => e.TempResistencia)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMP_RESISTENCIA");

                _ = entity.Property(e => e.Terminal)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TERMINAL");

                _ = entity.Property(e => e.UmResistencia)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_RESISTENCIA");

                _ = entity.Property(e => e.WindT)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("WIND_T");
            });

            _ = modelBuilder.Entity<SplDatosdetEst>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_DATOSDET_EST");

                _ = entity.Property(e => e.Ambiente1)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AMBIENTE_1");

                _ = entity.Property(e => e.Ambiente2)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AMBIENTE_2");

                _ = entity.Property(e => e.Ambiente3)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AMBIENTE_3");

                _ = entity.Property(e => e.AmbienteProm)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("AMBIENTE_PROM");

                _ = entity.Property(e => e.AmpMedidos)
                    .HasColumnType("numeric(5, 1)")
                    .HasColumnName("AMP_MEDIDOS");

                _ = entity.Property(e => e.Ao)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("AO");

                _ = entity.Property(e => e.Aor)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("AOR");

                _ = entity.Property(e => e.AorCorr)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("AOR_CORR");

                _ = entity.Property(e => e.Bor)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("BOR");

                _ = entity.Property(e => e.CabInfRadBco1)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_1");

                _ = entity.Property(e => e.CabInfRadBco10)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_10");

                _ = entity.Property(e => e.CabInfRadBco2)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_2");

                _ = entity.Property(e => e.CabInfRadBco3)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_3");

                _ = entity.Property(e => e.CabInfRadBco4)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_4");

                _ = entity.Property(e => e.CabInfRadBco5)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_5");

                _ = entity.Property(e => e.CabInfRadBco6)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_6");

                _ = entity.Property(e => e.CabInfRadBco7)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_7");

                _ = entity.Property(e => e.CabInfRadBco8)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_8");

                _ = entity.Property(e => e.CabInfRadBco9)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_9");

                _ = entity.Property(e => e.CabSupRadBco1)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_1");

                _ = entity.Property(e => e.CabSupRadBco10)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_10");

                _ = entity.Property(e => e.CabSupRadBco2)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_2");

                _ = entity.Property(e => e.CabSupRadBco3)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_3");

                _ = entity.Property(e => e.CabSupRadBco4)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_4");

                _ = entity.Property(e => e.CabSupRadBco5)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_5");

                _ = entity.Property(e => e.CabSupRadBco6)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_6");

                _ = entity.Property(e => e.CabSupRadBco7)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_7");

                _ = entity.Property(e => e.CabSupRadBco8)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_8");

                _ = entity.Property(e => e.CabSupRadBco9)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_9");

                _ = entity.Property(e => e.CanalAmb1)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_AMB_1");

                _ = entity.Property(e => e.CanalAmb2)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_AMB_2");

                _ = entity.Property(e => e.CanalAmb3)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_AMB_3");

                _ = entity.Property(e => e.CanalInf1)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_1");

                _ = entity.Property(e => e.CanalInf10)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_10");

                _ = entity.Property(e => e.CanalInf2)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_2");

                _ = entity.Property(e => e.CanalInf3)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_3");

                _ = entity.Property(e => e.CanalInf4)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_4");

                _ = entity.Property(e => e.CanalInf5)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_5");

                _ = entity.Property(e => e.CanalInf6)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_6");

                _ = entity.Property(e => e.CanalInf7)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_7");

                _ = entity.Property(e => e.CanalInf8)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_8");

                _ = entity.Property(e => e.CanalInf9)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_9");

                _ = entity.Property(e => e.CanalInst1)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INST_1");

                _ = entity.Property(e => e.CanalInst2)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INST_2");

                _ = entity.Property(e => e.CanalInst3)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INST_3");

                _ = entity.Property(e => e.CanalSup1)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_1");

                _ = entity.Property(e => e.CanalSup10)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_10");

                _ = entity.Property(e => e.CanalSup2)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_2");

                _ = entity.Property(e => e.CanalSup3)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_3");

                _ = entity.Property(e => e.CanalSup4)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_4");

                _ = entity.Property(e => e.CanalSup5)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_5");

                _ = entity.Property(e => e.CanalSup6)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_6");

                _ = entity.Property(e => e.CanalSup7)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_7");

                _ = entity.Property(e => e.CanalSup8)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_8");

                _ = entity.Property(e => e.CanalSup9)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_9");

                _ = entity.Property(e => e.CanalTtapa)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_TTAPA");

                _ = entity.Property(e => e.Estable).HasColumnName("ESTABLE");

                _ = entity.Property(e => e.FechaHora)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_HORA");

                _ = entity.Property(e => e.IdReg)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REG");

                _ = entity.Property(e => e.KwMedidos)
                    .HasColumnType("numeric(5, 1)")
                    .HasColumnName("KW_MEDIDOS");

                _ = entity.Property(e => e.Presion)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("PRESION");

                _ = entity.Property(e => e.PromRadInf)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("PROM_RAD_INF");

                _ = entity.Property(e => e.PromRadSup)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("PROM_RAD_SUP");

                _ = entity.Property(e => e.TempInstr1)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_INSTR_1");

                _ = entity.Property(e => e.TempInstr2)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_INSTR_2");

                _ = entity.Property(e => e.TempInstr3)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_INSTR_3");

                _ = entity.Property(e => e.TempTapa)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_TAPA");

                _ = entity.Property(e => e.Tor)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("TOR");

                _ = entity.Property(e => e.TorCorr)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("TOR_CORR");

                _ = entity.Property(e => e.VerifVent1).HasColumnName("VERIF_VENT_1");

                _ = entity.Property(e => e.VerifVent2).HasColumnName("VERIF_VENT_2");
            });

            _ = modelBuilder.Entity<SplDatosgralEst>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdReg, e.FechaDatos });

                _ = entity.ToTable("SPL_DATOSGRAL_EST");

                _ = entity.Property(e => e.AltitudeF1)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ALTITUDE_F1");

                _ = entity.Property(e => e.AltitudeF2)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ALTITUDE_F2");

                _ = entity.Property(e => e.CantTermoPares)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("CANT_TERMO_PARES");

                _ = entity.Property(e => e.Capacidad)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.CoolingType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                _ = entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("CORRIENTE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.DevanadoSplit)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DEVANADO_SPLIT");

                _ = entity.Property(e => e.Estatus).HasColumnName("ESTATUS");

                _ = entity.Property(e => e.FactAlt)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_ALT");

                _ = entity.Property(e => e.FactEnf)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_ENF");

                _ = entity.Property(e => e.FechaDatos)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_DATOS");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.IdReg)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REG");

                _ = entity.Property(e => e.Intervalo)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("INTERVALO");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.OverElevation)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("OVER_ELEVATION");

                _ = entity.Property(e => e.Perdidas)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("PERDIDAS");

                _ = entity.Property(e => e.PorcCarga)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("PORC_CARGA");

                _ = entity.Property(e => e.PosAt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                _ = entity.Property(e => e.PosBt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                _ = entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                _ = entity.Property(e => e.Sobrecarga)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SOBRECARGA");

                _ = entity.Property(e => e.UmIntervalo)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("UM_INTERVALO");
            });

            _ = modelBuilder.Entity<SplDescFactorcor>(entity =>
            {
                _ = entity.HasKey(e => new { e.Especificacion, e.ClaveIdioma });

                _ = entity.ToTable("SPL_DESC_FACTORCOR");

                _ = entity.Property(e => e.Especificacion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ESPECIFICACION");

                _ = entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplDescFactorcorreccion>(entity =>
            {
                _ = entity.HasKey(e => new { e.ClaveEsp, e.ClaveIdioma });

                _ = entity.ToTable("SPL_DESC_FACTORCORRECCION");

                _ = entity.HasIndex(e => new { e.ClaveEsp, e.ClaveIdioma }, "PK_SPL_DESC_FACTORCORRECCION_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.ClaveEsp)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CLAVE_ESP");

                _ = entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplDesplazamientoAngular>(entity =>
            {
                _ = entity.HasKey(e => e.Clave)
                    .HasName("SPL_DESPLAZAMIENTO_ANGULAR_PK");

                _ = entity.ToTable("SPL_DESPLAZAMIENTO_ANGULAR");

                _ = entity.HasIndex(e => e.Clave, "SPL_DESPLAZAMIENTO_ANGULAR_PK_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.Clave)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.HWye)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("H_WYE");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.TWye)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("T_WYE");

                _ = entity.Property(e => e.XWye)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("X_WYE");
            });

            _ = modelBuilder.Entity<SplEspecificacion>(entity =>
            {
                _ = entity.HasKey(e => e.Clave);

                _ = entity.ToTable("SPL_ESPECIFICACION");

                _ = entity.HasIndex(e => e.Clave, "PK_SPL_ESPECIFICACION_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.Clave)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CLAVE");

                _ = entity.Property(e => e.AplicaTangente)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("APLICA_TANGENTE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplFactorCorreccion>(entity =>
            {
                _ = entity.HasKey(e => new { e.ClaveEsp, e.Temperatura });

                _ = entity.ToTable("SPL_FACTOR_CORRECCION");

                _ = entity.HasIndex(e => new { e.ClaveEsp, e.Temperatura }, "PK_SPL_FACTOR_CORRECCION_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.ClaveEsp)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CLAVE_ESP");

                _ = entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TEMPERATURA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplFactorcorEtd>(entity =>
            {
                _ = entity.HasKey(e => new { e.CoolingType });

                _ = entity.ToTable("SPL_FACTORCOR_ETD");

                _ = entity.Property(e => e.CoolingType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplFactorcorFpb>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdMarca, e.IdTipo, e.Temperatura });

                _ = entity.ToTable("SPL_FACTORCOR_FPB");

                _ = entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_MARCA");

                _ = entity.Property(e => e.IdTipo)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_TIPO");

                _ = entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TEMPERATURA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplFactorcorFpc>(entity =>
            {
                _ = entity.HasKey(e => new { e.Especificacion, e.Temperatura });

                _ = entity.ToTable("SPL_FACTORCOR_FPC");

                _ = entity.Property(e => e.Especificacion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ESPECIFICACION");

                _ = entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TEMPERATURA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplFiltrosreporte>(entity =>
            {
                _ = entity.HasKey(e => new { e.TipoReporte, e.Posicion })
                    .HasName("SPL_FR_PK");

                _ = entity.ToTable("SPL_FILTROSREPORTE");

                _ = entity.HasIndex(e => new { e.TipoReporte, e.Posicion }, "SPL_FR_PK_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.Posicion)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("POSICION");

                _ = entity.Property(e => e.Catalogo)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("CATALOGO");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.TablaBd)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("TABLA_BD");
            });

            _ = modelBuilder.Entity<SplFrecuencia>(entity =>
            {
                _ = entity.HasKey(e => e.Clave);

                _ = entity.ToTable("SPL_FRECUENCIAS");

                _ = entity.HasIndex(e => e.Clave, "PK_SPL_FRECUENCIAS_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.Clave)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CLAVE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FRECUENCIA");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplIdioma>(entity =>
            {
                _ = entity.HasKey(e => e.ClaveIdioma)
                    .HasName("SPL_I_PK");

                _ = entity.ToTable("SPL_IDIOMAS");

                _ = entity.HasIndex(e => e.ClaveIdioma, "SPL_I_PK_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplInfoArchivosArf>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_ARCHIVOS_ARF");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                _ = entity.Property(e => e.Orden)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ORDEN");
            });

            _ = modelBuilder.Entity<SplInfoArchivosInd>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_ARCHIVOS_IND");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                _ = entity.Property(e => e.Orden)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ORDEN");
            });

            _ = modelBuilder.Entity<SplInfoArchivosPim>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_ARCHIVOS_PIM");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                _ = entity.Property(e => e.Orden)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ORDEN");
            });

            _ = modelBuilder.Entity<SplInfoArchivosPir>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_ARCHIVOS_PIR");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                _ = entity.Property(e => e.Orden)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ORDEN");
            });

            _ = modelBuilder.Entity<SplInfoCameTdp>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_CAME_TDP");

                _ = entity.Property(e => e.Calibracion1)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CALIBRACION_1");

                _ = entity.Property(e => e.Calibracion2)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CALIBRACION_2");

                _ = entity.Property(e => e.Calibracion3)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CALIBRACION_3");

                _ = entity.Property(e => e.Calibracion4)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CALIBRACION_4");

                _ = entity.Property(e => e.Calibracion5)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CALIBRACION_5");

                _ = entity.Property(e => e.Calibracion6)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CALIBRACION_6");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Medido1)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("MEDIDO_1");

                _ = entity.Property(e => e.Medido2)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("MEDIDO_2");

                _ = entity.Property(e => e.Medido3)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("MEDIDO_3");

                _ = entity.Property(e => e.Medido4)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("MEDIDO_4");

                _ = entity.Property(e => e.Medido5)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("MEDIDO_5");

                _ = entity.Property(e => e.Medido6)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("MEDIDO_6");

                _ = entity.Property(e => e.Notas)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");
            });

            _ = modelBuilder.Entity<SplInfoDetalleArf>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_ARF");

                _ = entity.Property(e => e.Boquillas)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BOQUILLAS");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.NivelAceite)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NIVEL_ACEITE");

                _ = entity.Property(e => e.NucleoHerraje)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NUCLEO_HERRAJE");

                _ = entity.Property(e => e.PosAt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                _ = entity.Property(e => e.PosBt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                _ = entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.TempAceite)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_ACEITE");

                _ = entity.Property(e => e.Terciario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TERCIARIO");
            });

            _ = modelBuilder.Entity<SplInfoDetalleCem>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_CEM");

                _ = entity.Property(e => e.CorrienteTerm1)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE_TERM_1");

                _ = entity.Property(e => e.CorrienteTerm2)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE_TERM_2");

                _ = entity.Property(e => e.CorrienteTerm3)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE_TERM_3");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.PosSecundaria)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_SECUNDARIA");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");
            });

            _ = modelBuilder.Entity<SplInfoDetalleCgd>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_CGD");

                _ = entity.Property(e => e.AceptacionPpm)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("ACEPTACION_PPM");

                _ = entity.Property(e => e.AntesPpm)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("ANTES_PPM");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.DespuesPpm)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("DESPUES_PPM");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.IncrementoPpm)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("INCREMENTO_PPM");

                _ = entity.Property(e => e.LimiteMax)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("LIMITE_MAX");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.ResultadoPpm)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RESULTADO_PPM");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Validacion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("VALIDACION");
            });

            _ = modelBuilder.Entity<SplInfoDetalleDpr>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_DPR");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");

                _ = entity.Property(e => e.Terminal1Mv)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TERMINAL1_MV");

                _ = entity.Property(e => e.Terminal1Pc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TERMINAL1_PC");

                _ = entity.Property(e => e.Terminal2Mv)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TERMINAL2_MV");

                _ = entity.Property(e => e.Terminal2Pc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TERMINAL2_PC");

                _ = entity.Property(e => e.Terminal3Mv)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TERMINAL3_MV");

                _ = entity.Property(e => e.Terminal3Pc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TERMINAL3_PC");

                _ = entity.Property(e => e.Tiempo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIEMPO");
            });

            _ = modelBuilder.Entity<SplInfoDetalleFpa>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_FPA");

                _ = entity.Property(e => e.Apertura)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("APERTURA");

                _ = entity.Property(e => e.Asmt)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ASMT");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Escala)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ESCALA");

                _ = entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                _ = entity.Property(e => e.FactorPotencia)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("FACTOR_POTENCIA");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.LimiteMax)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("LIMITE_MAX");

                _ = entity.Property(e => e.Medicion)
                    .HasColumnType("numeric(5, 3)")
                    .HasColumnName("MEDICION");

                _ = entity.Property(e => e.Promedio)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("PROMEDIO");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Validacion)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("VALIDACION");

                _ = entity.Property(e => e.Valor1)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("VALOR_1");

                _ = entity.Property(e => e.Valor2)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("VALOR_2");

                _ = entity.Property(e => e.Valor3)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("VALOR_3");

                _ = entity.Property(e => e.Valor4)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("VALOR_4");

                _ = entity.Property(e => e.Valor5)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("VALOR_5");
            });

            _ = modelBuilder.Entity<SplInfoDetalleFpb>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdRep, e.Seccion, e.Renglon });

                _ = entity.ToTable("SPL_INFO_DETALLE_FPB");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Capaci)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("CAPACI");

                _ = entity.Property(e => e.Capacitancia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("CAPACITANCIA");

                _ = entity.Property(e => e.ColumnaT)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_T");

                _ = entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE");

                _ = entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                _ = entity.Property(e => e.FactorCorr2)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR2");

                _ = entity.Property(e => e.FactorPotencia)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("FACTOR_POTENCIA");

                _ = entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_MARCA");

                _ = entity.Property(e => e.IdTipo)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_TIPO");

                _ = entity.Property(e => e.NoSerieBoq)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE_BOQ");

                _ = entity.Property(e => e.PorcFp)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_FP");

                _ = entity.Property(e => e.PorcFpCorr)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_FP_CORR");

                _ = entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                _ = entity.Property(e => e.Potencia)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("POTENCIA");
            });

            _ = modelBuilder.Entity<SplInfoDetalleFpc>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdRep, e.Seccion, e.Renglon });

                _ = entity.ToTable("SPL_INFO_DETALLE_FPC");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Capacitancia)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CAPACITANCIA");

                _ = entity.Property(e => e.CorrPorcFp)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORR_PORC_FP");

                _ = entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE");

                _ = entity.Property(e => e.DevE)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEV_E");

                _ = entity.Property(e => e.DevG)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEV_G");

                _ = entity.Property(e => e.DevT)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEV_T");

                _ = entity.Property(e => e.DevUst)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEV_UST");

                _ = entity.Property(e => e.IdCap)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID_CAP");

                _ = entity.Property(e => e.PorcFp)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_FP");

                _ = entity.Property(e => e.Potencia)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("POTENCIA");

                _ = entity.Property(e => e.TangPorcFp)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TANG_PORC_FP");
            });

            _ = modelBuilder.Entity<SplInfoDetalleInd>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_IND");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.NoPagina)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NO_PAGINA");

                _ = entity.Property(e => e.NoPaginaFin)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NO_PAGINA_FIN");

                _ = entity.Property(e => e.NoPaginaIni)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NO_PAGINA_INI");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.ValorKw)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("VALOR_KW");

                _ = entity.Property(e => e.ValorMva)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("VALOR_MVA");
            });

            _ = modelBuilder.Entity<SplInfoDetalleIsz>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_ISZ");

                _ = entity.Property(e => e.CorrienteIrms)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE_IRMS");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.PorcJxo)
                    .HasColumnType("numeric(4, 3)")
                    .HasColumnName("PORC_JXO");

                _ = entity.Property(e => e.PorcRo)
                    .HasColumnType("numeric(4, 3)")
                    .HasColumnName("PORC_RO");

                _ = entity.Property(e => e.PorcZo)
                    .HasColumnType("numeric(4, 3)")
                    .HasColumnName("PORC_ZO");

                _ = entity.Property(e => e.Posicion1)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION_1");

                _ = entity.Property(e => e.Posicion2)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION_2");

                _ = entity.Property(e => e.PotenciaCorrKw)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("POTENCIA_CORR_KW");

                _ = entity.Property(e => e.PotenciaKw)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("POTENCIA_KW");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Tension1)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("TENSION_1");

                _ = entity.Property(e => e.Tension2)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("TENSION_2");

                _ = entity.Property(e => e.TensionVrms)
                    .HasColumnType("numeric(7, 1)")
                    .HasColumnName("TENSION_VRMS");

                _ = entity.Property(e => e.ZBase)
                    .HasColumnType("numeric(8, 3)")
                    .HasColumnName("Z_BASE");

                _ = entity.Property(e => e.ZOhms)
                    .HasColumnType("numeric(8, 3)")
                    .HasColumnName("Z_OHMS");
            });

            _ = modelBuilder.Entity<SplInfoDetalleNra>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdRep, e.Seccion, e.Renglon, e.TipoInfo });

                _ = entity.ToTable("SPL_INFO_DETALLE_NRA");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.TipoInfo)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_INFO");

                _ = entity.Property(e => e.Altura)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ALTURA");

                _ = entity.Property(e => e.D1000).HasColumnType("numeric(11, 6)");

                _ = entity.Property(e => e.D10000).HasColumnType("numeric(11, 6)");

                _ = entity.Property(e => e.D125).HasColumnType("numeric(11, 6)");

                _ = entity.Property(e => e.D2000).HasColumnType("numeric(11, 6)");

                _ = entity.Property(e => e.D250).HasColumnType("numeric(11, 6)");

                _ = entity.Property(e => e.D315)
                    .HasColumnType("numeric(11, 6)")
                    .HasColumnName("D31_5");

                _ = entity.Property(e => e.D4000).HasColumnType("numeric(11, 6)");

                _ = entity.Property(e => e.D500).HasColumnType("numeric(11, 6)");

                _ = entity.Property(e => e.D63).HasColumnType("numeric(11, 6)");

                _ = entity.Property(e => e.D8000).HasColumnType("numeric(11, 6)");

                _ = entity.Property(e => e.DbaCorr)
                    .HasColumnType("numeric(11, 6)")
                    .HasColumnName("DBA_CORR");

                _ = entity.Property(e => e.DbaMedido)
                    .HasColumnType("numeric(11, 6)")
                    .HasColumnName("DBA_MEDIDO");

                _ = entity.Property(e => e.Pos)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS");
            });

            _ = modelBuilder.Entity<SplInfoDetallePce>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdRep, e.Seccion, e.Renglon });

                _ = entity.ToTable("SPL_INFO_DETALLE_PCE");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.CorrienteIrms)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("CORRIENTE_IRMS");

                _ = entity.Property(e => e.NominalKv)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("NOMINAL_KV");

                _ = entity.Property(e => e.PerdidasCorr20)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PERDIDAS_CORR20");

                _ = entity.Property(e => e.PerdidasKw)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PERDIDAS_KW");

                _ = entity.Property(e => e.PerdidasOnda)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PERDIDAS_ONDA");

                _ = entity.Property(e => e.PorcIexc)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_IEXC");

                _ = entity.Property(e => e.PorcVn)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("PORC_VN");

                _ = entity.Property(e => e.TensionAvg)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TENSION_AVG");

                _ = entity.Property(e => e.TensionRms)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TENSION_RMS");
            });

            modelBuilder.Entity<SplInfoDetallePci>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Seccion, e.Renglon });

                entity.ToTable("SPL_INFO_DETALLE_PCI");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.PosPri)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_PRI");

                entity.Property(e => e.PosSec)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_SEC");

                entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE");

                entity.Property(e => e.CorrienteIrms)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE_CD");

                entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");

                entity.Property(e => e.TensionRms)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_CD");

                entity.Property(e => e.Potencia)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("POTENCIA");

                entity.Property(e => e.PotenciaKw)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("POTENCIA_CD");

                entity.Property(e => e.ResisPri)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("RESIS_PRI");

                entity.Property(e => e.ResisSec)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("RESIS_SEC");

                entity.Property(e => e.TensionPri)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("TENSION_PRI");

                entity.Property(e => e.TensionSec)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("TENSION_SEC");

                entity.Property(e => e.VnomPri)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("VNOM_PRI");

                entity.Property(e => e.VnomSec)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("VNOM_SEC");

                entity.Property(e => e.InomPri)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("INOM_PRI");

                entity.Property(e => e.InomSec)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("INOM_SEC");

                entity.Property(e => e.I2rPri)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("I2R_PRI");

                entity.Property(e => e.I2rSec)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("I2R_SEC");

                entity.Property(e => e.I2rTot)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("I2R_TOT");

                entity.Property(e => e.I2rTotCorr)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("I2R_TOT_CORR");

                entity.Property(e => e.WcuCorr)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("WCU_CORR");

                entity.Property(e => e.Wind)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("WIND");

                entity.Property(e => e.WindCorr)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("WIND_CORR");

                entity.Property(e => e.Wcu)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("WCU");

                entity.Property(e => e.PorcR)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("PORC_R");

                entity.Property(e => e.PorcZ)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("PORC_Z");

                entity.Property(e => e.PorcX)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("PORC_X");

                entity.Property(e => e.Wfe20)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("WFE_20");

                entity.Property(e => e.PerdTotal)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("PERD_TOTAL");

                entity.Property(e => e.PerdCorregidas)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("PERD_CORREGIDAS");
            });

            _ = modelBuilder.Entity<SplInfoDetallePee>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdRep, e.Renglon });

                _ = entity.ToTable("SPL_INFO_DETALLE_PEE");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.CoolingType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                _ = entity.Property(e => e.CorrienteRms)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE_RMS");

                _ = entity.Property(e => e.KwauxGar)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWAUX_GAR");

                _ = entity.Property(e => e.MvaauxGar)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("MVAAUX_GAR");

                _ = entity.Property(e => e.PotenciaKw)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("POTENCIA_KW");

                _ = entity.Property(e => e.TensionRms)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_RMS");
            });

            _ = modelBuilder.Entity<SplInfoDetallePim>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_PIM");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Pagina)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PAGINA");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Terminal)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TERMINAL");
            });

            _ = modelBuilder.Entity<SplInfoDetallePir>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_PIR");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Pagina)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PAGINA");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Terminal)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TERMINAL");
            });

            _ = modelBuilder.Entity<SplInfoDetallePlr>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdRep, e.Renglon });

                _ = entity.ToTable("SPL_INFO_DETALLE_PLR");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE");

                _ = entity.Property(e => e.PorcDesv)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_DESV");

                _ = entity.Property(e => e.Reactancia)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("REACTANCIA");

                _ = entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");

                _ = entity.Property(e => e.Tiempo)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("TIEMPO");
            });

            _ = modelBuilder.Entity<SplInfoDetallePrd>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_DETALLE_PRD");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.C4F)
                    .HasColumnType("numeric(10, 8)")
                    .HasColumnName("C4_F");

                _ = entity.Property(e => e.CapKvar)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("CAP_KVAR");

                _ = entity.Property(e => e.CnF)
                    .HasColumnType("numeric(16, 14)")
                    .HasColumnName("CN_F");

                _ = entity.Property(e => e.Fc)
                    .HasColumnType("numeric(10, 6)")
                    .HasColumnName("FC");

                _ = entity.Property(e => e.Fc2)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("FC2");

                _ = entity.Property(e => e.GarantiaW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("GARANTIA_W");

                _ = entity.Property(e => e.IAmps)
                    .HasColumnType("numeric(8, 3)")
                    .HasColumnName("I_AMPS");

                _ = entity.Property(e => e.ImA)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("IM_A");

                _ = entity.Property(e => e.LxpH)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("LXP_H");

                _ = entity.Property(e => e.M3H)
                    .HasColumnType("numeric(9, 7)")
                    .HasColumnName("M3_H");

                _ = entity.Property(e => e.PW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("P_W");

                _ = entity.Property(e => e.PeW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("PE_W");

                _ = entity.Property(e => e.PfeW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("PFE_W");

                _ = entity.Property(e => e.PjmW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("PJM_W");

                _ = entity.Property(e => e.PjmcW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("PJMC_W");

                _ = entity.Property(e => e.PorcDesv)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("PORC_DESV");

                _ = entity.Property(e => e.PtW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("PT_W");

                _ = entity.Property(e => e.R4sOhms)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("R4S_OHMS");

                _ = entity.Property(e => e.RmOhms)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("RM_OHMS");

                _ = entity.Property(e => e.RxpOhms)
                    .HasColumnType("numeric(13, 3)")
                    .HasColumnName("RXP_OHMS");

                _ = entity.Property(e => e.TmC)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("TM_C");

                _ = entity.Property(e => e.TmpC)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("TMP_C");

                _ = entity.Property(e => e.TrC)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("TR_C");

                _ = entity.Property(e => e.U).HasColumnType("numeric(7, 2)");

                _ = entity.Property(e => e.VVolts)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("V_VOLTS");

                _ = entity.Property(e => e.VmV)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("VM_V");

                _ = entity.Property(e => e.XcOhms)
                    .HasColumnType("numeric(8, 2)")
                    .HasColumnName("XC_OHMS");

                _ = entity.Property(e => e.XmOhms)
                    .HasColumnType("numeric(8, 2)")
                    .HasColumnName("XM_OHMS");
            });

            _ = modelBuilder.Entity<SplInfoDetalleRad>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdCarga, e.Seccion, e.Tiempo, e.PosicionColumna });

                _ = entity.ToTable("SPL_INFO_DETALLE_RAD");

                _ = entity.Property(e => e.IdCarga)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_CARGA");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Tiempo)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("TIEMPO");

                _ = entity.Property(e => e.PosicionColumna)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("POSICION_COLUMNA");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.ValorColumna)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("VALOR_COLUMNA");
            });

            _ = modelBuilder.Entity<SplInfoDetalleRan>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdRep, e.Seccion, e.Renglon });

                _ = entity.ToTable("SPL_INFO_DETALLE_RAN");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Duracion)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DURACION");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.Limite)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("LIMITE");

                _ = entity.Property(e => e.Medicion)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("MEDICION");

                _ = entity.Property(e => e.Tiempo)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIEMPO");

                _ = entity.Property(e => e.UmMedicion)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_MEDICION");

                _ = entity.Property(e => e.UmTiempo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("UM_TIEMPO");

                _ = entity.Property(e => e.Valido)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("VALIDO");

                _ = entity.Property(e => e.Vcd)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("VCD");
            });

            _ = modelBuilder.Entity<SplInfoDetalleRct>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdRep, e.Seccion, e.Columna });

                _ = entity.ToTable("SPL_INFO_DETALLE_RCT");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Columna)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("COLUMNA");

                _ = entity.Property(e => e.Fase)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("FASE");

                _ = entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                _ = entity.Property(e => e.Resistencia)
                    .HasColumnType("numeric(11, 6)")
                    .HasColumnName("RESISTENCIA");

                _ = entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMPERATURA");

                _ = entity.Property(e => e.Terminal)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TERMINAL");

                _ = entity.Property(e => e.TipoMedicion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_MEDICION");

                _ = entity.Property(e => e.Unidad)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD");
            });

            _ = modelBuilder.Entity<SplInfoDetalleRdd>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_RDD");

                _ = entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE");

                _ = entity.Property(e => e.Fase)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("FASE");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Impedancia)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("IMPEDANCIA");

                _ = entity.Property(e => e.Perdidas)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("PERDIDAS");

                _ = entity.Property(e => e.PorcFp)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("PORC_FP");

                _ = entity.Property(e => e.PorcX)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("PORC_X");

                _ = entity.Property(e => e.Reactancia)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("REACTANCIA");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Resistencia)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("RESISTENCIA");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");
            });

            _ = modelBuilder.Entity<SplInfoDetalleRdt>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdCarga, e.Posicion, e.PosicionColumna });

                _ = entity.ToTable("SPL_INFO_DETALLE_RDT");

                _ = entity.Property(e => e.IdCarga)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_CARGA");

                _ = entity.Property(e => e.Posicion)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                _ = entity.Property(e => e.PosicionColumna)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("POSICION_COLUMNA");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Hvvolts)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("HVVOLTS");

                _ = entity.Property(e => e.Lvvolts)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("LVVOLTS");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.OrdenPosicion)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDEN_POSICION");

                _ = entity.Property(e => e.ValorColumna)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("VALOR_COLUMNA");

                _ = entity.Property(e => e.ValorDesv)
                    .HasColumnType("numeric(11, 4)")
                    .HasColumnName("VALOR_DESV");

                _ = entity.Property(e => e.ValorNominal)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("VALOR_NOMINAL");
            });

            _ = modelBuilder.Entity<SplInfoDetalleRod>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdRep, e.Seccion, e.Renglon });

                _ = entity.ToTable("SPL_INFO_DETALLE_ROD");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Correccion20)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("CORRECCION_20");

                _ = entity.Property(e => e.CorreccionSe)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("CORRECCION_SE");

                _ = entity.Property(e => e.PorcDesv)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("PORC_DESV");

                _ = entity.Property(e => e.PorcDesvDis)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("PORC_DESV_DIS");

                _ = entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                _ = entity.Property(e => e.ResDisenio)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("RES_DISENIO");

                _ = entity.Property(e => e.ResistenciaProm)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("RESISTENCIA_PROM");

                _ = entity.Property(e => e.Terminal1)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("TERMINAL_1");

                _ = entity.Property(e => e.Terminal2)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("TERMINAL_2");

                _ = entity.Property(e => e.Terminal3)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("TERMINAL_3");
            });

            _ = modelBuilder.Entity<SplInfoDetalleRye>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_RYE");

                _ = entity.Property(e => e.Eficiencia1)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_1");

                _ = entity.Property(e => e.Eficiencia2)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_2");

                _ = entity.Property(e => e.Eficiencia3)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_3");

                _ = entity.Property(e => e.Eficiencia4)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_4");

                _ = entity.Property(e => e.Eficiencia5)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_5");

                _ = entity.Property(e => e.Eficiencia6)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_6");

                _ = entity.Property(e => e.Eficiencia7)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_7");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.PorcMva)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("PORC_MVA");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");
            });

            _ = modelBuilder.Entity<SplInfoDetalleTap>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_TAP");

                _ = entity.Property(e => e.AmpCal)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("AMP_CAL");

                _ = entity.Property(e => e.Capacitancia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("CAPACITANCIA");

                _ = entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE");

                _ = entity.Property(e => e.DevAterrizado)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DEV_ATERRIZADO");

                _ = entity.Property(e => e.DevEnergizado)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DEV_ENERGIZADO");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.NivelTension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("NIVEL_TENSION");

                _ = entity.Property(e => e.PorcCorriente)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_CORRIENTE");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.TensionAplicada)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_APLICADA");

                _ = entity.Property(e => e.Tiempo)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIEMPO");
            });

            _ = modelBuilder.Entity<SplInfoDetalleTdp>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_TDP");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");

                _ = entity.Property(e => e.Terminal1)
                    .HasColumnType("numeric(6, 1)")
                    .HasColumnName("TERMINAL_1");

                _ = entity.Property(e => e.Terminal2)
                    .HasColumnType("numeric(6, 1)")
                    .HasColumnName("TERMINAL_2");

                _ = entity.Property(e => e.Terminal3)
                    .HasColumnType("numeric(6, 1)")
                    .HasColumnName("TERMINAL_3");

                _ = entity.Property(e => e.Terminal4)
                    .HasColumnType("numeric(6, 1)")
                    .HasColumnName("TERMINAL_4");

                _ = entity.Property(e => e.Terminal5)
                    .HasColumnType("numeric(6, 1)")
                    .HasColumnName("TERMINAL_5");

                _ = entity.Property(e => e.Terminal6)
                    .HasColumnType("numeric(6, 1)")
                    .HasColumnName("TERMINAL_6");

                _ = entity.Property(e => e.Tiempo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIEMPO");
            });

            _ = modelBuilder.Entity<SplInfoGeneralArf>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_ARF");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Equipo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("EQUIPO");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NivelesTension)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NIVELES_TENSION");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.Terciario2dabaja)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TERCIARIO_2DABAJA");

                _ = entity.Property(e => e.TerciarioDisp)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TERCIARIO_DISP");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.TotalPags)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TOTAL_PAGS");
            });

            _ = modelBuilder.Entity<SplInfoGeneralBpc>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_BPC");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.MetodologiaUsada)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("METODOLOGIA_USADA");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Notas)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMPERATURA");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.ValorObtenido)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("VALOR_OBTENIDO");
            });

            _ = modelBuilder.Entity<SplInfoGeneralCem>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_GENERAL_CEM");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.IdPosPrimaria)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ID_POS_PRIMARIA");

                _ = entity.Property(e => e.IdPosSecundaria)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ID_POS_SECUNDARIA");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.PosPrimaria)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_PRIMARIA");

                _ = entity.Property(e => e.PosSecundaria)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_SECUNDARIA");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.TituloTerm1)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TITULO_TERM_1");

                _ = entity.Property(e => e.TituloTerm2)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TITULO_TERM_2");

                _ = entity.Property(e => e.TituloTerm3)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TITULO_TERM_3");

                _ = entity.Property(e => e.VoltajePrueba)
                    .HasColumnType("numeric(7, 1)")
                    .HasColumnName("VOLTAJE_PRUEBA");
            });

            _ = modelBuilder.Entity<SplInfoGeneralCgd>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_CGD");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TipoAceite)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_ACEITE");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.ValAceptCg)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("VAL_ACEPT_CG");
            });

            _ = modelBuilder.Entity<SplInfoGeneralDpr>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_GENERAL_DPR");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.DescMayMv)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("DESC_MAY_MV");

                _ = entity.Property(e => e.DescMayPc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("DESC_MAY_PC");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("FRECUENCIA");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.IncMaxPc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("INC_MAX_PC");

                _ = entity.Property(e => e.Intervalo)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("INTERVALO");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NivelHora)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("NIVEL_HORA");

                _ = entity.Property(e => e.NivelRealce)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("NIVEL_REALCE");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Notas)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");

                _ = entity.Property(e => e.NumeroCiclo)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("NUMERO_CICLO");

                _ = entity.Property(e => e.OtroCiclo)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("OTRO_CICLO");

                _ = entity.Property(e => e.PosAt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                _ = entity.Property(e => e.PosBt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                _ = entity.Property(e => e.PosTer)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TensionPrueba)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TENSION_PRUEBA");

                _ = entity.Property(e => e.TerminalesPrueba)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TERMINALES_PRUEBA");

                _ = entity.Property(e => e.TiempoTotal)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIEMPO_TOTAL");

                _ = entity.Property(e => e.TipoMedicion)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_MEDICION");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            _ = modelBuilder.Entity<SplInfoGeneralFpa>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_FPA");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.HumedadRelativa)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("HUMEDAD_RELATIVA");

                _ = entity.Property(e => e.MarcaAceite)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MARCA_ACEITE");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Notas)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TempAmbiente)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_AMBIENTE");

                _ = entity.Property(e => e.TipoAceite)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_ACEITE");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            _ = modelBuilder.Entity<SplInfoGeneralFpb>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_FPB");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.CantBoq)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("CANT_BOQ");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TanDelta)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TAN_DELTA");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            _ = modelBuilder.Entity<SplInfoGeneralFpc>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_FPC");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Especificacion)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ESPECIFICACION");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("FRECUENCIA");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NivelesTension)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NIVELES_TENSION");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.TipoUnidad)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");
            });

            _ = modelBuilder.Entity<SplInfoGeneralInd>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_IND");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Anexo)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ANEXO");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TcComprados)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TC_COMPRADOS");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.TotalPags)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TOTAL_PAGS");
            });

            _ = modelBuilder.Entity<SplInfoGeneralIsz>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_GENERAL_ISZ");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.CantNeutros)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANT_NEUTROS");

                _ = entity.Property(e => e.CapBase)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("CAP_BASE");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.DevEnergizado)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DEV_ENERGIZADO");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.GradosCorr)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("GRADOS_CORR");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.ImpedanciaGar)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("IMPEDANCIA_GAR");

                _ = entity.Property(e => e.MaterialDevanado)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("MATERIAL_DEVANADO");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.PosAt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                _ = entity.Property(e => e.PosBt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                _ = entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMPERATURA");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.UmTemp)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMP");

                _ = entity.Property(e => e.UmcapBase)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UMCAP_BASE");
            });

            _ = modelBuilder.Entity<SplInfoGeneralNra>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_GENERAL_NRA");

                _ = entity.Property(e => e.Alfa)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ALFA");

                _ = entity.Property(e => e.Alimentacion)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ALIMENTACION");

                _ = entity.Property(e => e.Altura)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("ALTURA");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Area)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("AREA");

                _ = entity.Property(e => e.CantMediciones)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANT_MEDICIONES");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.CargarInfo).HasColumnName("CARGAR_INFO");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClaveNorma)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_NORMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.CoolingType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FactorK)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("FACTOR_K");

                _ = entity.Property(e => e.FechaDatos)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_DATOS");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Garantia)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("GARANTIA");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Laboratorio)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LABORATORIO");

                _ = entity.Property(e => e.MedAyd)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MED_AYD");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Notas)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");

                _ = entity.Property(e => e.Npplp)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("NPPLP");

                _ = entity.Property(e => e.Perimetro)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("PERIMETRO");

                _ = entity.Property(e => e.PosAt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                _ = entity.Property(e => e.PosBt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                _ = entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                _ = entity.Property(e => e.Prlw)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("PRLW");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.Sv)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("SV");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.UmAlimentacion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_ALIMENTACION");

                _ = entity.Property(e => e.UmAltura)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_ALTURA");

                _ = entity.Property(e => e.UmArea)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_AREA");

                _ = entity.Property(e => e.UmPerimetro)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_PERIMETRO");

                _ = entity.Property(e => e.ValorAlimentacion)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("VALOR_ALIMENTACION");
            });

            _ = modelBuilder.Entity<SplInfoGeneralPce>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_PCE");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.DevEnergizado)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DEV_ENERGIZADO");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.GarantiaCexitacion)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("GARANTIA_CEXITACION");

                _ = entity.Property(e => e.GarantiaPerdidas)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("GARANTIA_PERDIDAS");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.UmGarcexit)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_GARCEXIT");

                _ = entity.Property(e => e.UmGarperd)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_GARPERD");
            });

            modelBuilder.Entity<SplInfoGeneralPci>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_PCI");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.CapRedBaja)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CAP_RED_BAJA");

                entity.Property(e => e.Autotransformador)
                   .IsRequired()
                   .HasMaxLength(2)
                   .IsUnicode(false)
                   .HasColumnName("AUTOTRANSFORMADOR");

                entity.Property(e => e.Monofasico)
                   .IsRequired()
                   .HasMaxLength(2)
                   .IsUnicode(false)
                   .HasColumnName("MONOFASICO");

                entity.Property(e => e.MaterialDevanado)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("MATERIAL_DEVANADO");

                entity.Property(e => e.PosPri)
                   .IsRequired()
                   .HasMaxLength(3)
                   .IsUnicode(false)
                   .HasColumnName("POS_PRI");

                entity.Property(e => e.PosSec)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("POS_SEC");

                entity.Property(e => e.Kwcu)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("KWCU");

                entity.Property(e => e.KwcuMva)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("CAPACITANCIA_KWCU");

                entity.Property(e => e.Kwtot100)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("KWTOT_100");

                entity.Property(e => e.Kwtot100M)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("KWTOT_100_M");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");
            });

            _ = modelBuilder.Entity<SplInfoGeneralPee>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_PEE");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            _ = modelBuilder.Entity<SplInfoGeneralPim>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_GENERAL_PIM");

                _ = entity.Property(e => e.AplicaBaja)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("APLICA_BAJA");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Conexion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NivelTension)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("NIVEL_TENSION");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.TotalPags)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TOTAL_PAGS");
            });

            _ = modelBuilder.Entity<SplInfoGeneralPir>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_GENERAL_PIR");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Conexion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.IncluyeTerciario)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("INCLUYE_TERCIARIO");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NivelTension)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("NIVEL_TENSION");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.TotalPags)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TOTAL_PAGS");
            });

            _ = modelBuilder.Entity<SplInfoGeneralPlr>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_PLR");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.CantTensiones)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANT_TENSIONES");

                _ = entity.Property(e => e.CantTiempos)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANT_TIEMPOS");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.PorcDevtn)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_DEVTN");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.Rldtn)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("RLDTN");

                _ = entity.Property(e => e.TensionNominal)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_NOMINAL");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            _ = modelBuilder.Entity<SplInfoGeneralPrd>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_PRD");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.VoltajeNominal)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("VOLTAJE_NOMINAL");
            });

            _ = modelBuilder.Entity<SplInfoGeneralRad>(entity =>
            {
                _ = entity.HasKey(e => e.IdCarga)
                    .HasName("SPL_ID_RAD_PK");

                _ = entity.ToTable("SPL_INFO_GENERAL_RAD");

                _ = entity.Property(e => e.IdCarga)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_CARGA");

                _ = entity.Property(e => e.Archivo).HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaCarga)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_CARGA");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TercerDevanadoTipo)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TERCER_DEVANADO_TIPO");

                _ = entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.TipoUnidad)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");

                _ = entity.Property(e => e.ValorMinimo)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("VALOR_MINIMO");
            });

            _ = modelBuilder.Entity<SplInfoGeneralRan>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_RAN");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.CantMediciones)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANT_MEDICIONES");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            _ = modelBuilder.Entity<SplInfoGeneralRct>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_RCT");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TensionPrueba)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TENSION_PRUEBA");

                _ = entity.Property(e => e.Ter2baja)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TER_2BAJA");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD_MEDIDA");
            });

            _ = modelBuilder.Entity<SplInfoGeneralRdd>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_RDD");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.CapacidadPrueba)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("CAPACIDAD_PRUEBA");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Conexion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION");

                _ = entity.Property(e => e.ConfigDevanado)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CONFIG_DEVANADO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.DevCorto)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DEV_CORTO");

                _ = entity.Property(e => e.DevEnergizado)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DEV_ENERGIZADO");

                _ = entity.Property(e => e.DporcX)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("DPORC_X");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.PorcJx)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_JX");

                _ = entity.Property(e => e.PorcX)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("PORC_X");

                _ = entity.Property(e => e.PorcZ)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_Z");

                _ = entity.Property(e => e.PosAt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                _ = entity.Property(e => e.PosBt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.S3fV2)
                    .HasColumnType("numeric(12, 6)")
                    .HasColumnName("S3F_V2");

                _ = entity.Property(e => e.TensionCorto)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_CORTO");

                _ = entity.Property(e => e.TensionEnerg)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_ENERG");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.ValorAceptacion)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("VALOR_ACEPTACION");
            });

            _ = modelBuilder.Entity<SplInfoGeneralRdt>(entity =>
            {
                _ = entity.HasKey(e => e.IdCarga)
                    .HasName("SPL_IG_RDT_PK");

                _ = entity.ToTable("SPL_INFO_GENERAL_RDT");

                _ = entity.Property(e => e.IdCarga)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_CARGA");

                _ = entity.Property(e => e.Archivo).HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.ConexionSp)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CONEXION_SP");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.DesplazamientoAngular)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DESPLAZAMIENTO_ANGULAR");

                _ = entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA");

                _ = entity.Property(e => e.FechaCarga)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_CARGA");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Norma)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("NORMA");

                _ = entity.Property(e => e.PosAt)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                _ = entity.Property(e => e.PostPruebaBt)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("POST_PRUEBA_BT");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.Ter)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("TER");

                _ = entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            _ = modelBuilder.Entity<SplInfoGeneralRod>(entity =>
            {
                _ = entity.HasKey(e => e.IdRep);

                _ = entity.ToTable("SPL_INFO_GENERAL_ROD");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.ConexionPrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION_PRUEBA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.MaterialDevanado)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("MATERIAL_DEVANADO");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TensionPrueba)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TENSION_PRUEBA");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.TipoUnidad)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");

                _ = entity.Property(e => e.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD_MEDIDA");
            });

            _ = modelBuilder.Entity<SplInfoGeneralRye>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_GENERAL_RYE");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.CoolingType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoConexionesAt)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_CONEXIONES_AT");

                _ = entity.Property(e => e.NoConexionesBt)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_CONEXIONES_BT");

                _ = entity.Property(e => e.NoConexionesTer)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_CONEXIONES_TER");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TensionAt)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TENSION_AT");

                _ = entity.Property(e => e.TensionBt)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TENSION_BT");

                _ = entity.Property(e => e.TensionTer)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TENSION_TER");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            _ = modelBuilder.Entity<SplInfoGeneralTap>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_GENERAL_TAP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("FRECUENCIA");

                _ = entity.Property(e => e.IdCapAt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID_CAP_AT");

                _ = entity.Property(e => e.IdCapBt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID_CAP_BT");

                _ = entity.Property(e => e.IdCapTer)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID_CAP_TER");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.IdRepFpc)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP_FPC");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoConexionesAt)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_CONEXIONES_AT");

                _ = entity.Property(e => e.NoConexionesBt)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_CONEXIONES_BT");

                _ = entity.Property(e => e.NoConexionesTer)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_CONEXIONES_TER");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.TipoUnidad)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");

                _ = entity.Property(e => e.ValAcept)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("VAL_ACEPT");
            });

            _ = modelBuilder.Entity<SplInfoGeneralTdp>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_GENERAL_TDP");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.DescMayMv)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("DESC_MAY_MV");

                _ = entity.Property(e => e.DescMayPc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("DESC_MAY_PC");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("FRECUENCIA");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.IncMaxPc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("INC_MAX_PC");

                _ = entity.Property(e => e.Intervalo)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("INTERVALO");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NivelHora)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("NIVEL_HORA");

                _ = entity.Property(e => e.NivelRealce)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("NIVEL_REALCE");

                _ = entity.Property(e => e.NivelesTension)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NIVELES_TENSION");

                _ = entity.Property(e => e.NoCiclos)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("NO_CICLOS");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.PosAt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                _ = entity.Property(e => e.PosBt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                _ = entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.TerminalesPrueba)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TERMINALES_PRUEBA");

                _ = entity.Property(e => e.TiempoTotal)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("TIEMPO_TOTAL");

                _ = entity.Property(e => e.TipoMedicion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_MEDICION");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            _ = modelBuilder.Entity<SplInfoGeneralTin>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_GENERAL_TIN");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Conexion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.DevEnergizado)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DEV_ENERGIZADO");

                _ = entity.Property(e => e.DevInducido)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DEV_INDUCIDO");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("FRECUENCIA");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Notas)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");

                _ = entity.Property(e => e.PosAt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                _ = entity.Property(e => e.PosBt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                _ = entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                _ = entity.Property(e => e.RelTension)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("REL_TENSION");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");

                _ = entity.Property(e => e.TensionAplicada)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_APLICADA");

                _ = entity.Property(e => e.TensionInducida)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_INDUCIDA");

                _ = entity.Property(e => e.Tiempo)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TIEMPO");

                _ = entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            _ = modelBuilder.Entity<SplInfoGraficaEtd>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_GRAFICA_ETD");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.ValorX)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("VALOR_X");

                _ = entity.Property(e => e.ValorY)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("VALOR_Y");
            });

            _ = modelBuilder.Entity<SplInfoLaboratorio>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_LABORATORIO");

                _ = entity.Property(e => e.Alfa)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ALFA");

                _ = entity.Property(e => e.AreaPared1)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("AREA_PARED1");

                _ = entity.Property(e => e.AreaPared2)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("AREA_PARED2");

                _ = entity.Property(e => e.AreaPiso)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("AREA_PISO");

                _ = entity.Property(e => e.AreaPuerta1)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("AREA_PUERTA1");

                _ = entity.Property(e => e.AreaPuerta2)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("AREA_PUERTA2");

                _ = entity.Property(e => e.AreaTecho)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("AREA_TECHO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Laboratorio)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LABORATORIO");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Sv)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("SV");
            });

            _ = modelBuilder.Entity<SplInfoOctava>(entity =>
            {
                _ = entity.HasKey(e => new { e.Clave });

                _ = entity.ToTable("SPL_INFO_OCTAVAS");

                _ = entity.Property(e => e.Clave)
                    .HasColumnName("CLAVE").ValueGeneratedOnAdd();

                _ = entity.Property(e => e.Altura)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ALTURA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.D100).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D1000).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D10000).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D125).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D1250).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D16).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D160).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D1600).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D20).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D200).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D2000).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D25).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D250).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D2500).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D315)
                    .HasColumnType("numeric(16, 10)")
                    .HasColumnName("D31_5");

                _ = entity.Property(e => e.D3150).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D3151)
                    .HasColumnType("numeric(16, 10)")
                    .HasColumnName("D315");

                _ = entity.Property(e => e.D40).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D400).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D4000).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D50).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D500).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D5000).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D63).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D630).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D6300).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D80).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D800).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.D8000).HasColumnType("numeric(16, 10)");

                _ = entity.Property(e => e.FechaDatos)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_DATOS");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Hora).HasColumnName("HORA");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.TipoInfo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_INFO");
            });

            _ = modelBuilder.Entity<SplInfoRegRye>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_REG_RYE");

                _ = entity.Property(e => e.Capacidad)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.FactPot1)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_1");

                _ = entity.Property(e => e.FactPot2)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_2");

                _ = entity.Property(e => e.FactPot3)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_3");

                _ = entity.Property(e => e.FactPot4)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_4");

                _ = entity.Property(e => e.FactPot5)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_5");

                _ = entity.Property(e => e.FactPot6)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_6");

                _ = entity.Property(e => e.FactPot7)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_7");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.PerdidaCarga)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("PERDIDA_CARGA");

                _ = entity.Property(e => e.PerdidaEnf)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("PERDIDA_ENF");

                _ = entity.Property(e => e.PerdidaTotal)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("PERDIDA_TOTAL");

                _ = entity.Property(e => e.PerdidaVacio)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("PERDIDA_VACIO");

                _ = entity.Property(e => e.PorcR)
                    .HasColumnType("numeric(6, 4)")
                    .HasColumnName("PORC_R");

                _ = entity.Property(e => e.PorcReg1)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_1");

                _ = entity.Property(e => e.PorcReg2)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_2");

                _ = entity.Property(e => e.PorcReg3)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_3");

                _ = entity.Property(e => e.PorcReg4)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_4");

                _ = entity.Property(e => e.PorcReg5)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_5");

                _ = entity.Property(e => e.PorcReg6)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_6");

                _ = entity.Property(e => e.PorcReg7)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_7");

                _ = entity.Property(e => e.PorcX)
                    .HasColumnType("numeric(6, 4)")
                    .HasColumnName("PORC_X");

                _ = entity.Property(e => e.PorcZ)
                    .HasColumnType("numeric(5, 3)")
                    .HasColumnName("PORC_Z");

                _ = entity.Property(e => e.ValorG)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("VALOR_G");

                _ = entity.Property(e => e.ValorW)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("VALOR_W");

                _ = entity.Property(e => e.XEntreR)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("X_ENTRE_R");
            });

            _ = modelBuilder.Entity<SplInfoSeccionCgd>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_SECCION_CGD");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.HrsTemperatura)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("HRS_TEMPERATURA");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Metodo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("METODO");

                _ = entity.Property(e => e.Notas)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");

                _ = entity.Property(e => e.Resultado)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("RESULTADO");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");
            });

            _ = modelBuilder.Entity<SplInfoGeneralEtd>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_GENERAL_ETD");

                _ = entity.Property(e => e.IdRep)
                 .IsRequired()
                    .HasColumnType("numeric(7)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.FechaRep)
                 .IsRequired()
                  .HasColumnType("datetime")
                  .HasColumnName("FECHA_REP");

                _ = entity.Property(e => e.NoSerie)
                 .IsRequired()
                   .HasMaxLength(55)
                   .IsUnicode(false)
                   .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NoPrueba)
                 .IsRequired()
                  .HasMaxLength(2)
                  .IsUnicode(false)
                  .HasColumnName("NO_PRUEBA");

                _ = entity.Property(e => e.ClaveIdioma)
                 .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.Cliente)
                 .IsRequired()
             .HasMaxLength(512)
             .IsUnicode(false)
             .HasColumnName("CLIENTE");

                _ = entity.Property(e => e.Capacidad)
                 .IsRequired()
            .HasMaxLength(512)
            .IsUnicode(false)
            .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.Resultado).IsRequired().HasColumnName("RESULTADO");

                _ = entity.Property(e => e.NombreArchivo)
                     .IsRequired()
                     .HasMaxLength(64)
                     .IsUnicode(false)
                     .HasColumnName("NOMBRE_ARCHIVO");

                _ = entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.TipoReporte)
                 .IsRequired()
           .HasMaxLength(3)
           .IsUnicode(false)
           .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.ClavePrueba)
                 .IsRequired()
      .HasMaxLength(3)
      .IsUnicode(false)
      .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.IdCorte)
                 .IsRequired()
                 .HasColumnType("numeric(7)")
                 .HasColumnName("ID_CORTE");

                _ = entity.Property(e => e.IdReg)
                 .IsRequired()
               .HasColumnType("numeric(7)")
               .HasColumnName("ID_REG");

                _ = entity.Property(e => e.BtDifCap).IsRequired().HasColumnName("BT_DIF_CAP");

                _ = entity.Property(e => e.CapacidadBt)
             .HasColumnType("numeric(7,3)")
             .HasColumnName("CAPACIDAD_BT");

                _ = entity.Property(e => e.Terciario2b)
           .HasMaxLength(10)
            .IsRequired()
           .IsUnicode(false)
           .HasColumnName("TERCIARIO_2B");

                _ = entity.Property(e => e.TerCapRed).IsRequired().HasColumnName("TER_CAP_RED");

                _ = entity.Property(e => e.CapacidadTer)
          .HasColumnType("numeric(7,3)")
          .HasColumnName("CAPACIDAD_TER");

                _ = entity.Property(e => e.DevanadoSplit)
                 .IsRequired()
       .HasMaxLength(10)
       .IsUnicode(false)
       .HasColumnName("DEVANADO_SPLIT");

                _ = entity.Property(e => e.UltimaHora)
                 .IsRequired()
                .HasColumnType("datetime")
                .HasColumnName("ULTIMA_HORA");

                _ = entity.Property(e => e.FactorKw)
                 .IsRequired()
        .HasColumnType("numeric(5,2)")
        .HasColumnName("FACTOR_KW");

                _ = entity.Property(e => e.FactorAltitud)
                 .IsRequired()
       .HasColumnType("numeric(5,2)")
       .HasColumnName("FACTOR_ALTITUD");

                _ = entity.Property(e => e.TipoRegresion).IsRequired().HasColumnName("TIPO_REGRESION");

                _ = entity.Property(e => e.Comentario)
     .HasMaxLength(300)
     .IsUnicode(false)
     .HasColumnName("COMENTARIO");

                _ = entity.Property(e => e.Creadopor)
                 .IsRequired()
  .HasMaxLength(100)
  .IsUnicode(false)
  .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Modificadopor)
.HasMaxLength(100)
.IsUnicode(false)
.HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                 .IsRequired()
               .HasColumnType("datetime")
               .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
              .HasColumnType("datetime")
              .HasColumnName("FECHAMODIFICACION");

            });

            _ = modelBuilder.Entity<SplInfoDetalleEtd>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_DETALLE_ETD");

                _ = entity.Property(e => e.IdRep)
                 .IsRequired()
                    .HasColumnType("numeric(7)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Seccion)
                 .IsRequired()
                   .HasColumnType("numeric(1)")
                   .HasColumnName("SECCION");

                _ = entity.Property(e => e.Renglon)
                 .IsRequired()
                .HasColumnType("numeric(2)")
                .HasColumnName("RENGLON");

                _ = entity.Property(e => e.FechaHora)
                  .HasColumnType("datetime")
                  .HasColumnName("FECHA_HORA");

                _ = entity.Property(e => e.PromRadSup)

               .HasColumnType("numeric(5,2)")
               .HasColumnName("PROM_RAD_SUP");

                _ = entity.Property(e => e.PromRadInf)
            .HasColumnType("numeric(5,2)")
            .HasColumnName("PROM_RAD_INF");

                _ = entity.Property(e => e.AmbienteProm)
       .HasColumnType("numeric(4,2)")
       .HasColumnName("AMBIENTE_PROM");

                _ = entity.Property(e => e.TempTapa)
       .HasColumnType("numeric(4,1)")
       .HasColumnName("TEMP_TAPA");

                _ = entity.Property(e => e.Tor)
      .HasColumnType("numeric(4,2)")
      .HasColumnName("TOR");

                _ = entity.Property(e => e.Aor)
     .HasColumnType("numeric(4,2)")
     .HasColumnName("AOR");

                _ = entity.Property(e => e.Bor)
  .HasColumnType("numeric(4,2)")
  .HasColumnName("BOR");

                _ = entity.Property(e => e.ElevAceiteSup)
 .HasColumnType("numeric(4,2)")
 .HasColumnName("ELEV_ACEITE_SUP");

                _ = entity.Property(e => e.ElevAceiteInf)
.HasColumnType("numeric(4,2)")
.HasColumnName("ELEV_ACEITE_INF");

                _ = entity.Property(e => e.ElevAceiteProm)
.HasColumnType("numeric(4,2)")
.HasColumnName("ELEV_ACEITE_PROM");

                _ = entity.Property(e => e.Tiempo)
.HasColumnType("numeric(4,2)")
.HasColumnName("TIEMPO");

                _ = entity.Property(e => e.Tiempo)
.HasColumnType("numeric(7,4)")
.HasColumnName("RESISTENCIA");

            });

            _ = modelBuilder.Entity<SplInfoSeccionEtd>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_INFO_SECCION_ETD");

                _ = entity.Property(e => e.AltitudeF1)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ALTITUDE_F1");

                _ = entity.Property(e => e.AltitudeF2)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ALTITUDE_F2");

                _ = entity.Property(e => e.AwrLim)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("AWR_LIM");

                _ = entity.Property(e => e.Capacidad)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("CAPACIDAD");

                _ = entity.Property(e => e.CoolingType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                _ = entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("CORRIENTE");

                _ = entity.Property(e => e.ElevPromDev)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("ELEV_PROM_DEV");

                _ = entity.Property(e => e.ElevPtoMasCal)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("ELEV_PTO_MAS_CAL");

                _ = entity.Property(e => e.FactorK)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("FACTOR_K");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.GradienteDev)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("GRADIENTE_DEV");

                _ = entity.Property(e => e.GradienteLim)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_LIM");

                _ = entity.Property(e => e.HsrLim)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("HSR_LIM");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.NivelTension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("NIVEL_TENSION");

                _ = entity.Property(e => e.OverElevation)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("OVER_ELEVATION");

                _ = entity.Property(e => e.Perdidas)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("PERDIDAS");

                _ = entity.Property(e => e.PosAt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                _ = entity.Property(e => e.PosBt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                _ = entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                _ = entity.Property(e => e.ResistCorte)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("RESIST_CORTE");

                _ = entity.Property(e => e.ResistTcero)
                    .HasColumnType("numeric(8, 5)")
                    .HasColumnName("RESIST_TCERO");

                _ = entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Sobrecarga)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SOBRECARGA");

                _ = entity.Property(e => e.TempDev)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMP_DEV");

                _ = entity.Property(e => e.TempPromAceite)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("TEMP_PROM_ACEITE");

                _ = entity.Property(e => e.TempResistCorte)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMP_RESIST_CORTE");

                _ = entity.Property(e => e.Terminal)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TERMINAL");

                _ = entity.Property(e => e.TorLimite)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TOR_LIMITE");

                _ = entity.Property(e => e.UmResistencia)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_RESISTENCIA");
            });

            _ = modelBuilder.Entity<SplInfoSeccionFpb>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdRep, e.Seccion });

                _ = entity.ToTable("SPL_INFO_SECCION_FPB");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.AceptCap)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ACEPT_CAP");

                _ = entity.Property(e => e.AceptFp)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ACEPT_FP");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.TempFptand)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_FPTAND");

                _ = entity.Property(e => e.TempPorcfp)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_PORCFP");

                _ = entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMPERATURA");

                _ = entity.Property(e => e.TensionPrueba)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_PRUEBA");

                _ = entity.Property(e => e.UmTemp)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMP");

                _ = entity.Property(e => e.UmTension)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TENSION");
            });

            _ = modelBuilder.Entity<SplInfoSeccionFpc>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdRep, e.Seccion });

                _ = entity.ToTable("SPL_INFO_SECCION_FPC");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.AcepFp)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ACEP_FP");

                _ = entity.Property(e => e.AceptCap)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ACEPT_CAP");

                _ = entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.TempAceiteInf)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_ACEITE_INF");

                _ = entity.Property(e => e.TempAceiteSup)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_ACEITE_SUP");

                _ = entity.Property(e => e.TempCt)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TEMP_CT");

                _ = entity.Property(e => e.TempProm)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TEMP_PROM");

                _ = entity.Property(e => e.TensionPrueba)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_PRUEBA");

                _ = entity.Property(e => e.UmTempacInf)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMPAC_INF");

                _ = entity.Property(e => e.UmTempacSup)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMPAC_SUP");

                _ = entity.Property(e => e.UmTension)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TENSION");
            });

            _ = modelBuilder.Entity<SplInfoSeccionPce>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdRep, e.Seccion });

                _ = entity.ToTable("SPL_INFO_SECCION_PCE");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.CapMin)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("CAP_MIN");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("FRECUENCIA");

                _ = entity.Property(e => e.PosAt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                _ = entity.Property(e => e.PosBt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                _ = entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                _ = entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMPERATURA");

                _ = entity.Property(e => e.UmCapmin)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_CAPMIN");

                _ = entity.Property(e => e.UmFrec)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_FREC");

                _ = entity.Property(e => e.UmTemp)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMP");

                _ = entity.Property(e => e.UmVolbase)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_VOLBASE");

                _ = entity.Property(e => e.VnFin)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("VN_FIN");

                _ = entity.Property(e => e.VnInicio)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("VN_INICIO");

                _ = entity.Property(e => e.VnIntervalo)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("VN_INTERVALO");

                _ = entity.Property(e => e.VoltajeBase)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("VOLTAJE_BASE");
            });

            modelBuilder.Entity<SplInfoSeccionPci>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Seccion });

                entity.ToTable("SPL_INFO_SECCION_PCI");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.CapPrueba)
                    .HasColumnType("numeric(10, 3)")
                    .HasColumnName("CAP_PRUEBA");

                entity.Property(e => e.UmCapPrueba)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_CAP_PRUEBA");

                entity.Property(e => e.OverElevation)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("OVER_ELEVATION");

                entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("FRECUENCIA");

                entity.Property(e => e.UmFrec)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_FREC");

                entity.Property(e => e.Temp)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP");

                entity.Property(e => e.UmTemp)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMP");

                entity.Property(e => e.TempRespri)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("TEMP_RESPRI");

                entity.Property(e => e.TempRessec)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("TEMP_RESSEC");

                entity.Property(e => e.FacCorrPri)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("FAC_CORR_PRI");

                entity.Property(e => e.FacCorrSec)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("FAC_CORR_SEC");

                entity.Property(e => e.FacCorrSee)
                    .HasColumnType("numeric(17, 7)")
                    .HasColumnName("FAC_CORR_SEE");
            });

            _ = modelBuilder.Entity<SplInfoSeccionRad>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdCarga, e.Seccion })
                    .HasName("SPL_IS_RAD_PK");

                _ = entity.ToTable("SPL_INFO_SECCION_RAD");

                _ = entity.Property(e => e.IdCarga)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_CARGA");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(10, 1)")
                    .HasColumnName("TEMPERATURA");

                _ = entity.Property(e => e.Tension)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("TENSION");

                _ = entity.Property(e => e.Umtemp)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("UMTEMP");

                _ = entity.Property(e => e.Umtension)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("UMTENSION");
            });

            _ = modelBuilder.Entity<SplInfoSeccionRod>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdRep, e.Seccion });

                _ = entity.ToTable("SPL_INFO_SECCION_ROD");

                _ = entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                _ = entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                _ = entity.Property(e => e.Fc20)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("FC_20");

                _ = entity.Property(e => e.FcSe)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("FC_SE");

                _ = entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                _ = entity.Property(e => e.MaxDdis)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("MAX_DDIS");

                _ = entity.Property(e => e.MaxDesv)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("MAX_DESV");

                _ = entity.Property(e => e.MinDdis)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("MIN_DDIS");

                _ = entity.Property(e => e.TempSe)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_SE");

                _ = entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMPERATURA");

                _ = entity.Property(e => e.TitTerm1)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIT_TERM_1");

                _ = entity.Property(e => e.TitTerm2)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIT_TERM_2");

                _ = entity.Property(e => e.TitTerm3)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIT_TERM_3");

                _ = entity.Property(e => e.UmTemp)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMP");

                _ = entity.Property(e => e.UmTempse)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMPSE");

                _ = entity.Property(e => e.VaDesv)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("VA_DESV");

                _ = entity.Property(e => e.VaMaxDis)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("VA_MAX_DIS");

                _ = entity.Property(e => e.VaMinDis)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("VA_MIN_DIS");
            });

            _ = modelBuilder.Entity<SplInfoaparatoApr>(entity =>
            {
                _ = entity.HasKey(e => new { e.OrderCode, e.ColumnTypeId });

                _ = entity.ToTable("SPL_INFOAPARATO_APR");

                _ = entity.HasIndex(e => e.OrderCode, "I_infoaparatoApar_order");

                _ = entity.HasIndex(e => new { e.OrderCode, e.ColumnTypeId }, "PK_SPL_INFOAPARATO_APR_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                _ = entity.Property(e => e.ColumnTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COLUMN_TYPE_ID");

                _ = entity.Property(e => e.ColumnTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COLUMN_TITLE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.OrderIndex)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_INDEX");

                _ = entity.Property(e => e.Qty)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("QTY");
            });

            _ = modelBuilder.Entity<SplInfoaparatoBoq>(entity =>
            {
                _ = entity.HasKey(e => new { e.OrderCode, e.ColumnTypeId });

                _ = entity.ToTable("SPL_INFOAPARATO_BOQ");

                _ = entity.HasIndex(e => e.OrderCode, "I_infoaparatoBoqs_order");

                _ = entity.HasIndex(e => new { e.OrderCode, e.ColumnTypeId }, "PK_SPL_INFOAPARATO_BOQ_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                _ = entity.Property(e => e.ColumnTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COLUMN_TYPE_ID");

                _ = entity.Property(e => e.BilClass)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("BIL_CLASS");

                _ = entity.Property(e => e.BilClassOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BIL_CLASS_OTHER");

                _ = entity.Property(e => e.ColumnTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COLUMN_TITLE");

                _ = entity.Property(e => e.CorrienteUnidad)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE_UNIDAD");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.CurrentAmps)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CURRENT_AMPS");

                _ = entity.Property(e => e.CurrentAmpsReq)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CURRENT_AMPS_REQ");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.OrderIndex)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_INDEX");

                _ = entity.Property(e => e.Qty)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("QTY");

                _ = entity.Property(e => e.VoltageClass)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("VOLTAGE_CLASS");

                _ = entity.Property(e => e.VoltageClassOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("VOLTAGE_CLASS_OTHER");
            });

            _ = modelBuilder.Entity<SplInfoaparatoBoqdet>(entity =>
            {
                _ = entity.HasKey(e => new { e.NoSerie, e.NoSerieBoq });

                _ = entity.ToTable("SPL_INFOAPARATO_BOQDET");

                _ = entity.Property(e => e.NoSerie)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.NoSerieBoq)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE_BOQ");

                _ = entity.Property(e => e.Capacitancia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("CAPACITANCIA");

                _ = entity.Property(e => e.Capacitancia2)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("CAPACITANCIA2");

                _ = entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.FactorPotencia)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("FACTOR_POTENCIA");

                _ = entity.Property(e => e.FactorPotencia2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("FACTOR_POTENCIA2");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_MARCA");

                _ = entity.Property(e => e.IdTipo)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_TIPO");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Orden)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ORDEN");

                _ = entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                _ = entity.Property(e => e.Prueba).HasColumnName("PRUEBA");

                _ = entity.Property(e => e.Voltaje)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("VOLTAJE");
            });

            _ = modelBuilder.Entity<SplInfoaparatoCam>(entity =>
            {
                _ = entity.HasKey(e => new { e.OrderCode, e.ColumnTypeId });

                _ = entity.ToTable("SPL_INFOAPARATO_CAM");

                _ = entity.HasIndex(e => e.OrderCode, "I_infoaparatoCam_order");

                _ = entity.HasIndex(e => new { e.OrderCode, e.ColumnTypeId }, "PK_SPL_INFOAPARATO_CAM_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                _ = entity.Property(e => e.ColumnTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COLUMN_TYPE_ID");

                _ = entity.Property(e => e.ColumnTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COLUMN_TITLE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Deriv2Other)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("DERIV2_OTHER");

                _ = entity.Property(e => e.DerivId)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("DERIV_ID");

                _ = entity.Property(e => e.DerivId2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("DERIV_ID2");

                _ = entity.Property(e => e.DerivOther)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("DERIV_OTHER");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.FlagRcbnFcbn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FLAG_RCBN_FCBN");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.OperationId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("OPERATION_ID");

                _ = entity.Property(e => e.OrderIndex)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_INDEX");

                _ = entity.Property(e => e.Taps)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TAPS");
            });

            _ = modelBuilder.Entity<SplInfoaparatoCap>(entity =>
            {
                _ = entity.HasKey(e => new { e.OrderCode, e.Secuencia });

                _ = entity.ToTable("SPL_INFOAPARATO_CAP");

                _ = entity.HasIndex(e => e.OrderCode, "I_infoaparatoCaps_order");

                _ = entity.HasIndex(e => new { e.OrderCode, e.Secuencia }, "PK_SPL_INFOAPARATO_CAP_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                _ = entity.Property(e => e.Secuencia)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SECUENCIA");

                _ = entity.Property(e => e.CoolingType)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.DevAwr)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("DEV_AWR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Hstr)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("HSTR");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Mvaf1)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("MVAF1");

                _ = entity.Property(e => e.Mvaf2)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("MVAF2");

                _ = entity.Property(e => e.Mvaf3)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("MVAF3");

                _ = entity.Property(e => e.Mvaf4)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("MVAF4");

                _ = entity.Property(e => e.OverElevation)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("OVER_ELEVATION");
            });

            _ = modelBuilder.Entity<SplInfoaparatoCar>(entity =>
            {
                _ = entity.HasKey(e => e.OrderCode);

                _ = entity.ToTable("SPL_INFOAPARATO_CAR");

                _ = entity.HasIndex(e => e.OrderCode, "I_infoaparatoCars_order");

                _ = entity.HasIndex(e => e.OrderCode, "PK_SPL_INFOAPARATO_CAR_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                _ = entity.Property(e => e.ConexionEq)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION_EQ");

                _ = entity.Property(e => e.ConexionEq2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION_EQ_2");

                _ = entity.Property(e => e.ConexionEq3)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION_EQ_3");

                _ = entity.Property(e => e.ConexionEq4)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION_EQ_4");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Mvaf1ConnectionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF1_CONNECTION_ID");

                _ = entity.Property(e => e.Mvaf1ConnectionOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MVAF1_CONNECTION_OTHER");

                _ = entity.Property(e => e.Mvaf1DerDown)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_DER_DOWN");

                _ = entity.Property(e => e.Mvaf1DerUp)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_DER_UP");

                _ = entity.Property(e => e.Mvaf1Nbai1)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF1_NBAI1");

                _ = entity.Property(e => e.Mvaf1NbaiNeutro)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF1_NBAI_NEUTRO");

                _ = entity.Property(e => e.Mvaf1Taps)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MVAF1_TAPS");

                _ = entity.Property(e => e.Mvaf1Voltage1)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_VOLTAGE1");

                _ = entity.Property(e => e.Mvaf1Voltage2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_VOLTAGE2");

                _ = entity.Property(e => e.Mvaf1Voltage3)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_VOLTAGE3");

                _ = entity.Property(e => e.Mvaf1Voltage4)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_VOLTAGE4");

                _ = entity.Property(e => e.Mvaf2ConnectionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF2_CONNECTION_ID");

                _ = entity.Property(e => e.Mvaf2ConnectionOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MVAF2_CONNECTION_OTHER");

                _ = entity.Property(e => e.Mvaf2DerDown)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF2_DER_DOWN");

                _ = entity.Property(e => e.Mvaf2DerUp)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF2_DER_UP");

                _ = entity.Property(e => e.Mvaf2Nbai1)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF2_NBAI1");

                _ = entity.Property(e => e.Mvaf2NbaiNeutro)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF2_NBAI_NEUTRO");

                _ = entity.Property(e => e.Mvaf2Taps)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MVAF2_TAPS");

                _ = entity.Property(e => e.Mvaf2Voltage1)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF2_VOLTAGE1");

                _ = entity.Property(e => e.Mvaf2Voltage2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF2_VOLTAGE2");

                _ = entity.Property(e => e.Mvaf2Voltage3)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF2_VOLTAGE3");

                _ = entity.Property(e => e.Mvaf2Voltage4)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF2_VOLTAGE4");

                _ = entity.Property(e => e.Mvaf3ConnectionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF3_CONNECTION_ID");

                _ = entity.Property(e => e.Mvaf3ConnectionOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MVAF3_CONNECTION_OTHER");

                _ = entity.Property(e => e.Mvaf3DerDown)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF3_DER_DOWN");

                _ = entity.Property(e => e.Mvaf3DerUp)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF3_DER_UP");

                _ = entity.Property(e => e.Mvaf3Nbai1)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF3_NBAI1");

                _ = entity.Property(e => e.Mvaf3NbaiNeutro)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF3_NBAI_NEUTRO");

                _ = entity.Property(e => e.Mvaf3Taps)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MVAF3_TAPS");

                _ = entity.Property(e => e.Mvaf3Voltage1)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF3_VOLTAGE1");

                _ = entity.Property(e => e.Mvaf3Voltage2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF3_VOLTAGE2");

                _ = entity.Property(e => e.Mvaf3Voltage3)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF3_VOLTAGE3");

                _ = entity.Property(e => e.Mvaf3Voltage4)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF3_VOLTAGE4");

                _ = entity.Property(e => e.Mvaf4ConnectionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF4_CONNECTION_ID");

                _ = entity.Property(e => e.Mvaf4ConnectionOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MVAF4_CONNECTION_OTHER");

                _ = entity.Property(e => e.Mvaf4DerDown)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF4_DER_DOWN");

                _ = entity.Property(e => e.Mvaf4DerUp)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF4_DER_UP");

                _ = entity.Property(e => e.Mvaf4Nbai1)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF4_NBAI1");

                _ = entity.Property(e => e.Mvaf4NbaiNeutro)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF4_NBAI_NEUTRO");

                _ = entity.Property(e => e.Mvaf4Taps)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MVAF4_TAPS");

                _ = entity.Property(e => e.Mvaf4Voltage1)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF4_VOLTAGE1");

                _ = entity.Property(e => e.Mvaf4Voltage2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF4_VOLTAGE2");

                _ = entity.Property(e => e.Mvaf4Voltage3)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF4_VOLTAGE3");

                _ = entity.Property(e => e.Mvaf4Voltage4)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF4_VOLTAGE4");

                _ = entity.Property(e => e.RcbnFcbn1)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("RCBN_FCBN1");

                _ = entity.Property(e => e.RcbnFcbn2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("RCBN_FCBN2");

                _ = entity.Property(e => e.RcbnFcbn3)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("RCBN_FCBN3");

                _ = entity.Property(e => e.RcbnFcbn4)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("RCBN_FCBN4");
            });

            _ = modelBuilder.Entity<SplInfoaparatoDg>(entity =>
            {
                _ = entity.HasKey(e => e.OrderCode);

                _ = entity.ToTable("SPL_INFOAPARATO_DG");

                _ = entity.HasIndex(e => e.OrderCode, "I_infoaparatoDGs_order");

                _ = entity.HasIndex(e => e.OrderCode, "PK_SPL_INFOAPARATO_DG_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                _ = entity.Property(e => e.AltitudeF1)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ALTITUDE_F1");

                _ = entity.Property(e => e.AltitudeF2)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ALTITUDE_F2");

                _ = entity.Property(e => e.Applicationid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("APPLICATIONID");

                _ = entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.CustomerName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER_NAME");

                _ = entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.DesplazamientoAngular)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DESPLAZAMIENTO_ANGULAR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Frecuency)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FRECUENCY");

                _ = entity.Property(e => e.LanguageId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LANGUAGE_ID");

                _ = entity.Property(e => e.MarcaAceite)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MARCA_ACEITE");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Norma)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("NORMA");

                _ = entity.Property(e => e.Phases)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PHASES");

                _ = entity.Property(e => e.PoNumeric)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PO_NUMERIC");

                _ = entity.Property(e => e.PolarityId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("POLARITY_ID");

                _ = entity.Property(e => e.PolarityOther)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POLARITY_OTHER");

                _ = entity.Property(e => e.Standardid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("STANDARDID");

                _ = entity.Property(e => e.TipoAceite)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_ACEITE");

                _ = entity.Property(e => e.TipoUnidad)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");

                _ = entity.Property(e => e.Typetrafoid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TYPETRAFOID");
            });

            _ = modelBuilder.Entity<SplInfoaparatoEst>(entity =>
            {
                _ = entity.HasKey(e => e.NoSerie);

                _ = entity.ToTable("SPL_INFOAPARATO_EST");

                _ = entity.Property(e => e.AmbienteCte)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("AMBIENTE_CTE");

                _ = entity.Property(e => e.AorHenf)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AOR_HENF");

                _ = entity.Property(e => e.AorLimite)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("AOR_LIMITE");

                _ = entity.Property(e => e.AwrHenfAt)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AWR_HENF_AT");

                _ = entity.Property(e => e.AwrHenfBt)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AWR_HENF_BT");

                _ = entity.Property(e => e.AwrHenfTer)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AWR_HENF_TER");

                _ = entity.Property(e => e.AwrLimAt)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("AWR_LIM_AT");

                _ = entity.Property(e => e.AwrLimBt)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("AWR_LIM_BT");

                _ = entity.Property(e => e.AwrLimTer)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("AWR_LIM_TER");

                _ = entity.Property(e => e.Bor)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("BOR");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.CteTiempoTrafo)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("CTE_TIEMPO_TRAFO");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.GradienteHentAt)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_HENT_AT");

                _ = entity.Property(e => e.GradienteHentBt)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_HENT_BT");

                _ = entity.Property(e => e.GradienteHentTer)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_HENT_TER");

                _ = entity.Property(e => e.GradienteLimAt)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_LIM_AT");

                _ = entity.Property(e => e.GradienteLimBt)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_LIM_BT");

                _ = entity.Property(e => e.GradienteLimTer)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_LIM_TER");

                _ = entity.Property(e => e.HsrHenfAt)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("HSR_HENF_AT");

                _ = entity.Property(e => e.HsrHenfBt)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("HSR_HENF_BT");

                _ = entity.Property(e => e.HsrHenfTer)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("HSR_HENF_TER");

                _ = entity.Property(e => e.HsrLimAt)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("HSR_LIM_AT");

                _ = entity.Property(e => e.HsrLimBt)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("HSR_LIM_BT");

                _ = entity.Property(e => e.HsrLimTer)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("HSR_LIM_TER");

                _ = entity.Property(e => e.KwDiseno)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("KW_DISENO");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.Toi)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TOI");

                _ = entity.Property(e => e.ToiLimite)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TOI_LIMITE");

                _ = entity.Property(e => e.TorHenf)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TOR_HENF");

                _ = entity.Property(e => e.TorLimite)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TOR_LIMITE");
            });

            _ = modelBuilder.Entity<SplInfoaparatoGar>(entity =>
            {
                _ = entity.HasKey(e => e.OrderCode);

                _ = entity.ToTable("SPL_INFOAPARATO_GAR");

                _ = entity.HasIndex(e => e.OrderCode, "I_infoaparatoGars_order");

                _ = entity.HasIndex(e => e.OrderCode, "PK_SPL_INFOAPARATO_GAR_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Iexc100)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("IEXC_100");

                _ = entity.Property(e => e.Iexc110)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("IEXC_110");

                _ = entity.Property(e => e.Kwaux1)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWAUX_1");

                _ = entity.Property(e => e.Kwaux2)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWAUX_2");

                _ = entity.Property(e => e.Kwaux3)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWAUX_3");

                _ = entity.Property(e => e.Kwaux4)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWAUX_4");

                _ = entity.Property(e => e.Kwcu)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWCU");

                _ = entity.Property(e => e.KwcuKv)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWCU_KV");

                _ = entity.Property(e => e.KwcuMva)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWCU_MVA");

                _ = entity.Property(e => e.Kwfe100)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWFE_100");

                _ = entity.Property(e => e.Kwfe110)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWFE_110");

                _ = entity.Property(e => e.Kwtot100)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWTOT_100");

                _ = entity.Property(e => e.Kwtot110)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWTOT_110");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NoiseFa1)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_FA1");

                _ = entity.Property(e => e.NoiseFa2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_FA2");

                _ = entity.Property(e => e.NoiseOa)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_OA");

                _ = entity.Property(e => e.TolerancyKwAux)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_KW_AUX");

                _ = entity.Property(e => e.TolerancyKwCu)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_KW_CU");

                _ = entity.Property(e => e.TolerancyKwfe)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_KWFE");

                _ = entity.Property(e => e.TolerancyKwtot)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_KWTOT");

                _ = entity.Property(e => e.TolerancyZpositive)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TOLERANCY_ZPOSITIVE");

                _ = entity.Property(e => e.TolerancyZpositive2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TOLERANCY_ZPOSITIVE2");

                _ = entity.Property(e => e.ZPositiveHx)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("Z_POSITIVE_HX");

                _ = entity.Property(e => e.ZPositiveHy)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("Z_POSITIVE_HY");

                _ = entity.Property(e => e.ZPositiveMva)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("Z_POSITIVE_MVA");

                _ = entity.Property(e => e.ZPositiveXy)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("Z_POSITIVE_XY");
            });

            _ = modelBuilder.Entity<SplInfoaparatoLab>(entity =>
            {
                _ = entity.HasKey(e => e.OrderCode);

                _ = entity.ToTable("SPL_INFOAPARATO_LAB");

                _ = entity.HasIndex(e => e.OrderCode, "I_infoaparatoLabs_order");

                _ = entity.HasIndex(e => e.OrderCode, "PK_SPL_INFOAPARATO_LAB_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.TextTestDielectric)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TEXT_TEST_DIELECTRIC");

                _ = entity.Property(e => e.TextTestPrototype)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TEXT_TEST_PROTOTYPE");

                _ = entity.Property(e => e.TextTestRoutine)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TEXT_TEST_ROUTINE");
            });

            _ = modelBuilder.Entity<SplInfoaparatoNor>(entity =>
            {
                _ = entity.HasKey(e => new { e.OrderCode, e.Secuencia });

                _ = entity.ToTable("SPL_INFOAPARATO_NOR");

                _ = entity.HasIndex(e => e.OrderCode, "I_infoaparatoNorms_order");

                _ = entity.HasIndex(e => new { e.OrderCode, e.Secuencia }, "PK_SPL_INFOAPARATO_NOR_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                _ = entity.Property(e => e.Secuencia)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SECUENCIA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplInfoaparatoTap>(entity =>
            {
                _ = entity.HasKey(e => e.OrderCode);

                _ = entity.ToTable("SPL_INFOAPARATO_TAPS");

                _ = entity.HasIndex(e => e.OrderCode, "PK_SPL_INFOAPARATO_TAPS_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                _ = entity.Property(e => e.CantidadInfBc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CANTIDAD_INF_BC");

                _ = entity.Property(e => e.CantidadInfSc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CANTIDAD_INF_SC");

                _ = entity.Property(e => e.CantidadSupBc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CANTIDAD_SUP_BC");

                _ = entity.Property(e => e.CantidadSupSc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CANTIDAD_SUP_SC");

                _ = entity.Property(e => e.ComboNumericBc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COMBO_NUMERIC_BC");

                _ = entity.Property(e => e.ComboNumericSc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COMBO_NUMERIC_SC");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.IdentificacionBc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("IDENTIFICACION_BC");

                _ = entity.Property(e => e.IdentificacionSc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("IDENTIFICACION_SC");

                _ = entity.Property(e => e.InvertidoBc)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("INVERTIDO_BC");

                _ = entity.Property(e => e.InvertidoSc)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("INVERTIDO_SC");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NominalBc)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("NOMINAL_BC");

                _ = entity.Property(e => e.NominalSc)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("NOMINAL_SC");

                _ = entity.Property(e => e.PorcentajeInfBc)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORCENTAJE_INF_BC");

                _ = entity.Property(e => e.PorcentajeInfSc)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORCENTAJE_INF_SC");

                _ = entity.Property(e => e.PorcentajeSupBc)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORCENTAJE_SUP_BC");

                _ = entity.Property(e => e.PorcentajeSupSc)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORCENTAJE_SUP_SC");
            });

            _ = modelBuilder.Entity<SplMarcasBoq>(entity =>
            {
                _ = entity.HasKey(e => e.IdMarca);

                _ = entity.ToTable("SPL_MARCAS_BOQ");

                _ = entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(5, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_MARCA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Estatus).HasColumnName("ESTATUS");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplNorma>(entity =>
            {
                _ = entity.HasKey(e => e.Clave)
                    .HasName("SPL_N_PK");

                _ = entity.ToTable("SPL_NORMA");

                _ = entity.HasIndex(e => e.Clave, "SPL_N_PK_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.Clave)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplNormasrep>(entity =>
            {
                _ = entity.HasKey(e => new { e.ClaveNorma, e.ClaveIdioma, e.Secuencia });

                _ = entity.ToTable("SPL_NORMASREP");

                _ = entity.HasIndex(e => e.ClaveIdioma, "I_normasrep_idioma");

                _ = entity.HasIndex(e => e.ClaveNorma, "I_normasrep_norma");

                _ = entity.HasIndex(e => new { e.ClaveNorma, e.ClaveIdioma, e.Secuencia }, "PK_SPL_NORMASREP_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.ClaveNorma)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_NORMA");

                _ = entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.Secuencia)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("SECUENCIA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplOpcione>(entity =>
            {
                _ = entity.HasKey(e => e.Clave);

                _ = entity.ToTable("SPL_OPCIONES");

                _ = entity.Property(e => e.Clave)
                    .HasColumnType("decimal(38, 0)")
                    .HasColumnName("CLAVE");

                _ = entity.Property(e => e.ClavePadre)
                    .HasColumnType("decimal(38, 0)")
                    .HasColumnName("CLAVE_PADRE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Orden).HasColumnName("ORDEN");

                _ = entity.Property(e => e.SubMenu)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SUB_MENU");

                _ = entity.Property(e => e.Url)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            _ = modelBuilder.Entity<SplPerfile>(entity =>
            {
                _ = entity.HasKey(e => e.Clave);

                _ = entity.ToTable("SPL_PERFILES");

                _ = entity.Property(e => e.Clave)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("date")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("date")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplPermiso>(entity =>
            {
                _ = entity.HasKey(e => new { e.ClavePerfil, e.ClaveOpcion });

                _ = entity.ToTable("SPL_PERMISOS");

                _ = entity.Property(e => e.ClavePerfil)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PERFIL");

                _ = entity.Property(e => e.ClaveOpcion)
                    .HasMaxLength(38)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_OPCION");
            });

            _ = modelBuilder.Entity<SplPlantillaBase>(entity =>
            {
                _ = entity.HasKey(e => new { e.TipoReporte, e.ClavePrueba, e.ClaveIdioma, e.ColumnasConfigurables })
                    .HasName("SPL_PB_PK");

                _ = entity.ToTable("SPL_PLANTILLA_BASE");

                _ = entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.ClavePrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.ColumnasConfigurables)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("COLUMNAS_CONFIGURABLES");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Plantilla).HasColumnName("PLANTILLA");
            });

            _ = modelBuilder.Entity<SplPrueba>(entity =>
            {
                _ = entity.HasKey(e => new { e.TipoReporte, e.ClavePrueba })
                    .HasName("SPL_P_PK");

                _ = entity.ToTable("SPL_PRUEBAS");

                _ = entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.ClavePrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplRepConsolidado>(entity =>
            {
                _ = entity.HasKey(e => e.Idioma);

                _ = entity.ToTable("SPL_REP_CONSOLIDADO");

                _ = entity.HasIndex(e => e.Idioma, "PK_SPL_REP_CONSOLIDADO_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.Idioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("IDIOMA");

                _ = entity.Property(e => e.Archivo).HasColumnName("ARCHIVO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.NombreArchivo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");
            });

            _ = modelBuilder.Entity<SplReporte>(entity =>
            {
                _ = entity.HasKey(e => e.TipoReporte)
                    .HasName("SPL_R_PK");

                _ = entity.ToTable("SPL_REPORTES");

                _ = entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                _ = entity.Property(e => e.Agrupacion)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("AGRUPACION");

                _ = entity.Property(e => e.AgrupacionEn)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("AGRUPACION_EN");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.DescripcionEn)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION_EN");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplResistDiseno>(entity =>
            {
                _ = entity.HasKey(e => new { e.NoSerie, e.UnidadMedida, e.ConexionPrueba, e.Temperatura, e.IdSeccion, e.Orden });

                _ = entity.ToTable("SPL_RESIST_DISENO");

                _ = entity.Property(e => e.NoSerie)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                _ = entity.Property(e => e.UnidadMedida)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD_MEDIDA");

                _ = entity.Property(e => e.ConexionPrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION_PRUEBA");

                _ = entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMPERATURA");

                _ = entity.Property(e => e.IdSeccion)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ID_SECCION");

                _ = entity.Property(e => e.Orden)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ORDEN");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                _ = entity.Property(e => e.Resistencia)
                    .HasColumnType("numeric(12, 6)")
                    .HasColumnName("RESISTENCIA");
            });

            _ = modelBuilder.Entity<SplSerieParalelo>(entity =>
            {
                _ = entity.HasKey(e => e.Clave);

                _ = entity.ToTable("SPL_SERIE_PARALELO");

                _ = entity.HasIndex(e => e.Clave, "PK_SPL_SERIE_PARALELO_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.Clave)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CLAVE");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplTensionPlaca>(entity =>
            {
                _ = entity.HasKey(e => new { e.Unidad, e.TipoTension, e.Orden })
                    .HasName("SPL_TENSION_PLACA_PK");

                _ = entity.ToTable("SPL_TENSION_PLACA");

                _ = entity.HasIndex(e => new { e.Unidad, e.TipoTension, e.Orden }, "SPL_TENSION_PLACA_PK_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.Unidad)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD");

                _ = entity.Property(e => e.TipoTension)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_TENSION");

                _ = entity.Property(e => e.Orden)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ORDEN");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                _ = entity.Property(e => e.Tension)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("TENSION");
            });

            _ = modelBuilder.Entity<SplTercerDevanadoTipo>(entity =>
            {
                _ = entity.HasKey(e => e.Clave)
                    .HasName("SPL_TDT_PK");

                _ = entity.ToTable("SPL_TERCER_DEVANADO_TIPO");

                _ = entity.HasIndex(e => e.Clave, "SPL_TDT_PK_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.Clave)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplTipoUnidad>(entity =>
            {
                _ = entity.HasKey(e => e.Clave)
                    .HasName("SPL_TU_PK");

                _ = entity.ToTable("SPL_TIPO_UNIDAD");

                _ = entity.HasIndex(e => e.Clave, "SPL_TU_PK_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.Clave)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplTiposxmarcaBoq>(entity =>
            {
                _ = entity.HasKey(e => new { e.IdMarca, e.IdTipo });

                _ = entity.ToTable("SPL_TIPOSXMARCA_BOQ");

                _ = entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_MARCA");

                _ = entity.Property(e => e.IdTipo)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_TIPO");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Estatus).HasColumnName("ESTATUS");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplTitSerieparalelo>(entity =>
            {
                _ = entity.HasKey(e => new { e.ClaveSepa, e.ClaveIdioma });

                _ = entity.ToTable("SPL_TIT_SERIEPARALELO");

                _ = entity.HasIndex(e => new { e.ClaveSepa, e.ClaveIdioma }, "PK_SPL_TIT_SERIEPARALELO_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.ClaveSepa)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CLAVE_SEPA");

                _ = entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            _ = modelBuilder.Entity<SplTituloColumnasCem>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_TITULO_COLUMNAS_CEM");

                _ = entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.Columna1)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_1");

                _ = entity.Property(e => e.Columna2)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_2");

                _ = entity.Property(e => e.Columna3)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_3");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.PosSec)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("POS_SEC");

                _ = entity.Property(e => e.Tipo)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TIPO");

                _ = entity.Property(e => e.TituloPos)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TITULO_POS");
            });

            _ = modelBuilder.Entity<SplTituloColumnasFpc>(entity =>
            {
                _ = entity.HasKey(e => new { e.TipoUnidad, e.ClaveIdioma, e.Renglon });

                _ = entity.ToTable("SPL_TITULO_COLUMNAS_FPC");

                _ = entity.Property(e => e.TipoUnidad)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");

                _ = entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                _ = entity.Property(e => e.Columna1)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_1");

                _ = entity.Property(e => e.Columna2)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_2");

                _ = entity.Property(e => e.Columna3)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_3");

                _ = entity.Property(e => e.Columna4)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_4");

                _ = entity.Property(e => e.Columna5)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_5");

                _ = entity.Property(e => e.ConstanteX)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CONSTANTE_X");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.VcPorcFp)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("VC_PORC_FP");
            });

            _ = modelBuilder.Entity<SplTituloColumnasRad>(entity =>
            {
                _ = entity.HasKey(e => new { e.TipoUnidad, e.TercerDevanadoTipo, e.PosColumna, e.ClaveIdioma })
                    .HasName("SPL_TC_RAD_PK");

                _ = entity.ToTable("SPL_TITULO_COLUMNAS_RAD");

                _ = entity.HasIndex(e => new { e.TipoUnidad, e.TercerDevanadoTipo, e.PosColumna, e.ClaveIdioma }, "SPL_TC_RAD_PK_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.TipoUnidad)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");

                _ = entity.Property(e => e.TercerDevanadoTipo)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TERCER_DEVANADO_TIPO");

                _ = entity.Property(e => e.PosColumna)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("POS_COLUMNA");

                _ = entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Titulo)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");
            });

            _ = modelBuilder.Entity<SplTituloColumnasRdt>(entity =>
            {
                _ = entity.HasKey(e => new { e.ClavePrueba, e.DesplazamientoAngular, e.Norma, e.PosColumna, e.ClaveIdioma })
                    .HasName("SPL_TC_RDT_PK");

                _ = entity.ToTable("SPL_TITULO_COLUMNAS_RDT");

                _ = entity.HasIndex(e => new { e.ClavePrueba, e.DesplazamientoAngular, e.Norma, e.PosColumna, e.ClaveIdioma }, "SPL_TC_RDT_PK_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.ClavePrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                _ = entity.Property(e => e.DesplazamientoAngular)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DESPLAZAMIENTO_ANGULAR");

                _ = entity.Property(e => e.Norma)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("NORMA");

                _ = entity.Property(e => e.PosColumna)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("POS_COLUMNA");

                _ = entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                _ = entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.PrimerRenglon)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("PRIMER_RENGLON");

                _ = entity.Property(e => e.SegundoRenglon)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("SEGUNDO_RENGLON");

                _ = entity.Property(e => e.TitPos1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIT_POS_1");

                _ = entity.Property(e => e.TitPos2)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIT_POS_2");

                _ = entity.Property(e => e.TitPosUnica1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIT_POS_UNICA1");

                _ = entity.Property(e => e.TitPosUnica2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIT_POS_UNICA2");

                _ = entity.Property(e => e.Titulo)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");
            });

            _ = modelBuilder.Entity<SplUsuario>(entity =>
            {
                _ = entity.HasKey(e => e.NombreIdentificador);

                _ = entity.ToTable("SPL_USUARIOS");

                _ = entity.Property(e => e.NombreIdentificador)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_IDENTIFICADOR");

                _ = entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
            });

            _ = modelBuilder.Entity<SplValidationTestsIsz>(entity =>
            {
                _ = entity.HasNoKey();

                _ = entity.ToTable("SPL_VALIDATION_TESTS_ISZ");

                _ = entity.Property(e => e.ArregloDev)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ARREGLO_DEV");

                _ = entity.Property(e => e.Maximo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MAXIMO");

                _ = entity.Property(e => e.MedicionDev)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MEDICION_DEV");

                _ = entity.Property(e => e.Minimo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MINIMO");

                _ = entity.Property(e => e.Promedio)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PROMEDIO");

                _ = entity.Property(e => e.TipoUnidad)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");
            });

            _ = modelBuilder.Entity<SplValorNominal>(entity =>
            {
                _ = entity.HasKey(e => new { e.Unidad, e.OperationId, e.Orden })
                    .HasName("SPL_VALOR_NOMINAL_PK");

                _ = entity.ToTable("SPL_VALOR_NOMINAL");

                _ = entity.HasIndex(e => new { e.Unidad, e.OperationId, e.Orden }, "SPL_VALOR_NOMINAL_PK_UNIQUE")
                    .IsUnique();

                _ = entity.Property(e => e.Unidad)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD");

                _ = entity.Property(e => e.OperationId)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("OPERATION_ID");

                _ = entity.Property(e => e.Orden)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ORDEN");

                _ = entity.Property(e => e.ComboNumeric)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COMBO_NUMERIC");

                _ = entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                _ = entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                _ = entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                _ = entity.Property(e => e.Hvvolts)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("HVVOLTS");

                _ = entity.Property(e => e.Lvvolts)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("LVVOLTS");

                _ = entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                _ = entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");
            });

            _ = modelBuilder.Entity<SysModulo>(entity =>
            {
                _ = entity.ToTable("SYS_MODULOS");

                _ = entity.Property(e => e.Id).HasColumnName("id");

                _ = entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            _ = modelBuilder.Entity<TipoArchivo>(entity =>
            {
                _ = entity.ToTable("TIPO_ARCHIVO");

                _ = entity.Property(e => e.Id).HasColumnName("id");

                _ = entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
    #endregion 
}
