using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

using SPL.Tests.Infrastructure.Entities;

#nullable disable

namespace SPL.Tests.Infrastructure
{
    public partial class dbTestsSPLContext : DbContext
    {
        public dbTestsSPLContext()
        {
        }

        public dbTestsSPLContext(DbContextOptions<dbTestsSPLContext> options)
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

            modelBuilder.Entity<ExtensionesArchivo>(entity =>
            {
                entity.ToTable("EXTENSIONES_ARCHIVO");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("extension");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.TipoArchivo).HasColumnName("tipo_archivo");
            });

            modelBuilder.Entity<PesoArchivo>(entity =>
            {
                entity.ToTable("PESO_ARCHIVO");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ExtensionArchivo).HasColumnName("extension_archivo");

                entity.Property(e => e.IdModulo).HasColumnName("id_modulo");

                entity.Property(e => e.MaximoPeso)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("maximo_peso");

                entity.HasOne(d => d.ExtensionArchivoNavigation)
                    .WithMany(p => p.PesoArchivos)
                    .HasForeignKey(d => d.ExtensionArchivo)
                    .HasConstraintName("FK_PESO_ARCHIVO_PESO_ARCHIVO");

                entity.HasOne(d => d.IdModuloNavigation)
                    .WithMany(p => p.PesoArchivos)
                    .HasForeignKey(d => d.IdModulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PESO_ARCHIVO_MODULO");
            });

            modelBuilder.Entity<SplAsignacionUsuario>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("SPL_ASIGNACION_USUARIOS");

                entity.Property(e => e.UserId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.Property(e => e.ClavePerfil)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PERFIL");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("date")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("date")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplCatsidco>(entity =>
            {
                entity.ToTable("SPL_CATSIDCO");

                entity.HasIndex(e => e.Id, "PK_SPL_CATSIDCO_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID");

                entity.Property(e => e.AttributeId)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ATTRIBUTE_ID");

                entity.Property(e => e.ClaveSpl)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_SPL");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplCatsidcoOther>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_CATSIDCO_OTHERS");

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Dato)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("DATO");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplConfiguracionReporte>(entity =>
            {
                entity.HasKey(e => new { e.TipoReporte, e.ClavePrueba, e.Apartado, e.Seccion, e.Dato })
                    .HasName("SPL_CR_PK");

                entity.ToTable("SPL_CONFIGURACION_REPORTE");

                entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.ClavePrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Apartado)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("APARTADO");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Dato)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("DATO");

                entity.Property(e => e.Celda)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("CELDA");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Formato)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("FORMATO");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Obtencion)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("OBTENCION");

                entity.Property(e => e.TipoDato)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_DATO");
            });

            modelBuilder.Entity<SplConfiguracionReporteOrd>(entity =>
            {
                entity.HasKey(e => e.Dato)
                    .HasName("SPL_CRO_PK");

                entity.ToTable("SPL_CONFIGURACION_REPORTE_ORD");

                entity.Property(e => e.Dato)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("DATO");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Orden)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ORDEN");
            });

            modelBuilder.Entity<SplContgasCgd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_CONTGAS_CGD");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.LimiteMax)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("LIMITE_MAX");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.TipoAceite)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_ACEITE");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            modelBuilder.Entity<SplCortedetaEst>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_CORTEDETA_EST");

                entity.Property(e => e.IdCorte)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_CORTE");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Resistencia)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("RESISTENCIA");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.TempR)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMP_R");

                entity.Property(e => e.Tiempo)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("TIEMPO");
            });

            modelBuilder.Entity<SplCortegralEst>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_CORTEGRAL_EST");

                entity.Property(e => e.Constante)
                    .HasColumnType("numeric(5, 1)")
                    .HasColumnName("CONSTANTE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.IdCorte)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_CORTE");

                entity.Property(e => e.IdReg)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REG");

                entity.Property(e => e.KwPrueba)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("KW_PRUEBA");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.PrimerCorte)
                    .HasColumnType("datetime")
                    .HasColumnName("PRIMER_CORTE");

                entity.Property(e => e.SegundoCorte)
                    .HasColumnType("datetime")
                    .HasColumnName("SEGUNDO_CORTE");

                entity.Property(e => e.TercerCorte)
                    .HasColumnType("datetime")
                    .HasColumnName("TERCER_CORTE");

                entity.Property(e => e.UltimaHora)
                    .HasColumnType("datetime")
                    .HasColumnName("ULTIMA_HORA");
            });

            modelBuilder.Entity<SplCorteseccEst>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_CORTESECC_EST");

                entity.Property(e => e.AwrC)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AWR_C");

                entity.Property(e => e.AwrE)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AWR_E");

                entity.Property(e => e.CapturaEn)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CAPTURA_EN");

                entity.Property(e => e.FactorK)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("FACTOR_K");

                entity.Property(e => e.GradienteCaC)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("GRADIENTE_CA_C");

                entity.Property(e => e.GradienteCaE)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("GRADIENTE_CA_E");

                entity.Property(e => e.HsrC)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("HSR_C");

                entity.Property(e => e.HsrE)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("HSR_E");

                entity.Property(e => e.HstC)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("HST_C");

                entity.Property(e => e.HstE)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("HST_E");

                entity.Property(e => e.IdCorte)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_CORTE");

                entity.Property(e => e.LimiteEst)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("LIMITE_EST");

                entity.Property(e => e.ResistZeroC)
                    .HasColumnType("numeric(8, 5)")
                    .HasColumnName("RESIST_ZERO_C");

                entity.Property(e => e.ResistZeroE)
                    .HasColumnType("numeric(8, 5)")
                    .HasColumnName("RESIST_ZERO_E");

                entity.Property(e => e.Resistencia)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("RESISTENCIA");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.TempAceiteProm)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("TEMP_ACEITE_PROM");

                entity.Property(e => e.TempDevC)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMP_DEV_C");

                entity.Property(e => e.TempDevE)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMP_DEV_E");

                entity.Property(e => e.TempResistencia)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMP_RESISTENCIA");

                entity.Property(e => e.Terminal)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TERMINAL");

                entity.Property(e => e.UmResistencia)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_RESISTENCIA");

                entity.Property(e => e.WindT)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("WIND_T");
            });

            modelBuilder.Entity<SplDatosdetEst>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_DATOSDET_EST");

                entity.Property(e => e.Ambiente1)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AMBIENTE_1");

                entity.Property(e => e.Ambiente2)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AMBIENTE_2");

                entity.Property(e => e.Ambiente3)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AMBIENTE_3");

                entity.Property(e => e.AmbienteProm)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("AMBIENTE_PROM");

                entity.Property(e => e.AmpMedidos)
                    .HasColumnType("numeric(5, 1)")
                    .HasColumnName("AMP_MEDIDOS");

                entity.Property(e => e.Ao)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("AO");

                entity.Property(e => e.Aor)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("AOR");

                entity.Property(e => e.AorCorr)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("AOR_CORR");

                entity.Property(e => e.Bor)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("BOR");

                entity.Property(e => e.CabInfRadBco1)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_1");

                entity.Property(e => e.CabInfRadBco10)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_10");

                entity.Property(e => e.CabInfRadBco2)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_2");

                entity.Property(e => e.CabInfRadBco3)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_3");

                entity.Property(e => e.CabInfRadBco4)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_4");

                entity.Property(e => e.CabInfRadBco5)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_5");

                entity.Property(e => e.CabInfRadBco6)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_6");

                entity.Property(e => e.CabInfRadBco7)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_7");

                entity.Property(e => e.CabInfRadBco8)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_8");

                entity.Property(e => e.CabInfRadBco9)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_INF_RAD_BCO_9");

                entity.Property(e => e.CabSupRadBco1)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_1");

                entity.Property(e => e.CabSupRadBco10)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_10");

                entity.Property(e => e.CabSupRadBco2)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_2");

                entity.Property(e => e.CabSupRadBco3)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_3");

                entity.Property(e => e.CabSupRadBco4)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_4");

                entity.Property(e => e.CabSupRadBco5)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_5");

                entity.Property(e => e.CabSupRadBco6)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_6");

                entity.Property(e => e.CabSupRadBco7)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_7");

                entity.Property(e => e.CabSupRadBco8)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_8");

                entity.Property(e => e.CabSupRadBco9)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("CAB_SUP_RAD_BCO_9");

                entity.Property(e => e.CanalAmb1)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_AMB_1");

                entity.Property(e => e.CanalAmb2)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_AMB_2");

                entity.Property(e => e.CanalAmb3)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_AMB_3");

                entity.Property(e => e.CanalInf1)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_1");

                entity.Property(e => e.CanalInf10)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_10");

                entity.Property(e => e.CanalInf2)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_2");

                entity.Property(e => e.CanalInf3)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_3");

                entity.Property(e => e.CanalInf4)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_4");

                entity.Property(e => e.CanalInf5)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_5");

                entity.Property(e => e.CanalInf6)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_6");

                entity.Property(e => e.CanalInf7)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_7");

                entity.Property(e => e.CanalInf8)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_8");

                entity.Property(e => e.CanalInf9)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INF_9");

                entity.Property(e => e.CanalInst1)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INST_1");

                entity.Property(e => e.CanalInst2)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INST_2");

                entity.Property(e => e.CanalInst3)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_INST_3");

                entity.Property(e => e.CanalSup1)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_1");

                entity.Property(e => e.CanalSup10)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_10");

                entity.Property(e => e.CanalSup2)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_2");

                entity.Property(e => e.CanalSup3)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_3");

                entity.Property(e => e.CanalSup4)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_4");

                entity.Property(e => e.CanalSup5)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_5");

                entity.Property(e => e.CanalSup6)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_6");

                entity.Property(e => e.CanalSup7)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_7");

                entity.Property(e => e.CanalSup8)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_8");

                entity.Property(e => e.CanalSup9)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_SUP_9");

                entity.Property(e => e.CanalTtapa)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANAL_TTAPA");

                entity.Property(e => e.Estable).HasColumnName("ESTABLE");

                entity.Property(e => e.FechaHora)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_HORA");

                entity.Property(e => e.IdReg)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REG");

                entity.Property(e => e.KwMedidos)
                    .HasColumnType("numeric(5, 1)")
                    .HasColumnName("KW_MEDIDOS");

                entity.Property(e => e.Presion)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("PRESION");

                entity.Property(e => e.PromRadInf)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("PROM_RAD_INF");

                entity.Property(e => e.PromRadSup)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("PROM_RAD_SUP");

                entity.Property(e => e.TempInstr1)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_INSTR_1");

                entity.Property(e => e.TempInstr2)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_INSTR_2");

                entity.Property(e => e.TempInstr3)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_INSTR_3");

                entity.Property(e => e.TempTapa)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_TAPA");

                entity.Property(e => e.Tor)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("TOR");

                entity.Property(e => e.TorCorr)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("TOR_CORR");

                entity.Property(e => e.VerifVent1).HasColumnName("VERIF_VENT_1");

                entity.Property(e => e.VerifVent2).HasColumnName("VERIF_VENT_2");
            });

            modelBuilder.Entity<SplDatosgralEst>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_DATOSGRAL_EST");

                entity.Property(e => e.AltitudeF1)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ALTITUDE_F1");

                entity.Property(e => e.AltitudeF2)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ALTITUDE_F2");

                entity.Property(e => e.CantTermoPares)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("CANT_TERMO_PARES");

                entity.Property(e => e.Capacidad)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.CoolingType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("CORRIENTE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.DevanadoSplit)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DEVANADO_SPLIT");

                entity.Property(e => e.Estatus).HasColumnName("ESTATUS");

                entity.Property(e => e.FactAlt)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_ALT");

                entity.Property(e => e.FactEnf)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_ENF");

                entity.Property(e => e.FechaDatos)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_DATOS");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.IdReg)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REG");

                entity.Property(e => e.Intervalo)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("INTERVALO");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.OverElevation)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("OVER_ELEVATION");

                entity.Property(e => e.Perdidas)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("PERDIDAS");

                entity.Property(e => e.PorcCarga)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("PORC_CARGA");

                entity.Property(e => e.PosAt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                entity.Property(e => e.PosBt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                entity.Property(e => e.Sobrecarga)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SOBRECARGA");

                entity.Property(e => e.UmIntervalo)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("UM_INTERVALO");
            });

            modelBuilder.Entity<SplDescFactorcor>(entity =>
            {
                entity.HasKey(e => new { e.Especificacion, e.ClaveIdioma });

                entity.ToTable("SPL_DESC_FACTORCOR");

                entity.Property(e => e.Especificacion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ESPECIFICACION");

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplDescFactorcorreccion>(entity =>
            {
                entity.HasKey(e => new { e.ClaveEsp, e.ClaveIdioma });

                entity.ToTable("SPL_DESC_FACTORCORRECCION");

                entity.HasIndex(e => new { e.ClaveEsp, e.ClaveIdioma }, "PK_SPL_DESC_FACTORCORRECCION_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ClaveEsp)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CLAVE_ESP");

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplDesplazamientoAngular>(entity =>
            {
                entity.HasKey(e => e.Clave)
                    .HasName("SPL_DESPLAZAMIENTO_ANGULAR_PK");

                entity.ToTable("SPL_DESPLAZAMIENTO_ANGULAR");

                entity.HasIndex(e => e.Clave, "SPL_DESPLAZAMIENTO_ANGULAR_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Clave)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.TWye)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("T_WYE");

                entity.Property(e => e.XWye)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("X_WYE");
            });

            modelBuilder.Entity<SplEspecificacion>(entity =>
            {
                entity.HasKey(e => e.Clave);

                entity.ToTable("SPL_ESPECIFICACION");

                entity.HasIndex(e => e.Clave, "PK_SPL_ESPECIFICACION_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Clave)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CLAVE");

                entity.Property(e => e.AplicaTangente)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("APLICA_TANGENTE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplFactorCorreccion>(entity =>
            {
                entity.HasKey(e => new { e.ClaveEsp, e.Temperatura });

                entity.ToTable("SPL_FACTOR_CORRECCION");

                entity.HasIndex(e => new { e.ClaveEsp, e.Temperatura }, "PK_SPL_FACTOR_CORRECCION_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ClaveEsp)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CLAVE_ESP");

                entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("TEMPERATURA");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplFactorcorEtd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_FACTORCOR_ETD");

                entity.Property(e => e.CoolingType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplFactorcorFpb>(entity =>
            {
                entity.HasKey(e => new { e.IdMarca, e.IdTipo, e.Temperatura });

                entity.ToTable("SPL_FACTORCOR_FPB");

                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_MARCA");

                entity.Property(e => e.IdTipo)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_TIPO");

                entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TEMPERATURA");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplFactorcorFpc>(entity =>
            {
                entity.HasKey(e => new { e.Especificacion, e.Temperatura });

                entity.ToTable("SPL_FACTORCOR_FPC");

                entity.Property(e => e.Especificacion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ESPECIFICACION");

                entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TEMPERATURA");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplFiltrosreporte>(entity =>
            {
                entity.HasKey(e => new { e.TipoReporte, e.Posicion })
                    .HasName("SPL_FR_PK");

                entity.ToTable("SPL_FILTROSREPORTE");

                entity.HasIndex(e => new { e.TipoReporte, e.Posicion }, "SPL_FR_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.Posicion)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("POSICION");

                entity.Property(e => e.Catalogo)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("CATALOGO");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.TablaBd)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("TABLA_BD");
            });

            modelBuilder.Entity<SplFrecuencia>(entity =>
            {
                entity.HasKey(e => e.Clave);

                entity.ToTable("SPL_FRECUENCIAS");

                entity.HasIndex(e => e.Clave, "PK_SPL_FRECUENCIAS_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Clave)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CLAVE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
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

                entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FRECUENCIA");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplIdioma>(entity =>
            {
                entity.HasKey(e => e.ClaveIdioma)
                    .HasName("SPL_I_PK");

                entity.ToTable("SPL_IDIOMAS");

                entity.HasIndex(e => e.ClaveIdioma, "SPL_I_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplInfoArchivosArf>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_ARCHIVOS_ARF");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Orden)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ORDEN");
            });

            modelBuilder.Entity<SplInfoArchivosInd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_ARCHIVOS_IND");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Orden)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ORDEN");
            });

            modelBuilder.Entity<SplInfoArchivosPim>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_ARCHIVOS_PIM");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Orden)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ORDEN");
            });

            modelBuilder.Entity<SplInfoArchivosPir>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_ARCHIVOS_PIR");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Orden)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ORDEN");
            });

            modelBuilder.Entity<SplInfoCameTdp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_CAME_TDP");

                entity.Property(e => e.Calibracion1)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CALIBRACION_1");

                entity.Property(e => e.Calibracion2)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CALIBRACION_2");

                entity.Property(e => e.Calibracion3)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CALIBRACION_3");

                entity.Property(e => e.Calibracion4)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CALIBRACION_4");

                entity.Property(e => e.Calibracion5)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CALIBRACION_5");

                entity.Property(e => e.Calibracion6)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CALIBRACION_6");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Medido1)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("MEDIDO_1");

                entity.Property(e => e.Medido2)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("MEDIDO_2");

                entity.Property(e => e.Medido3)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("MEDIDO_3");

                entity.Property(e => e.Medido4)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("MEDIDO_4");

                entity.Property(e => e.Medido5)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("MEDIDO_5");

                entity.Property(e => e.Medido6)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("MEDIDO_6");

                entity.Property(e => e.Notas)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");
            });

            modelBuilder.Entity<SplInfoDetalleArf>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_ARF");

                entity.Property(e => e.Boquillas)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BOQUILLAS");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.NivelAceite)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NIVEL_ACEITE");

                entity.Property(e => e.NucleoHerraje)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NUCLEO_HERRAJE");

                entity.Property(e => e.PosAt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                entity.Property(e => e.PosBt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.TempAceite)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_ACEITE");

                entity.Property(e => e.Terciario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TERCIARIO");
            });

            modelBuilder.Entity<SplInfoDetalleCem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_CEM");

                entity.Property(e => e.CorrienteTerm1)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE_TERM_1");

                entity.Property(e => e.CorrienteTerm2)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE_TERM_2");

                entity.Property(e => e.CorrienteTerm3)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE_TERM_3");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.PosSecundaria)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_SECUNDARIA");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");
            });

            modelBuilder.Entity<SplInfoDetalleCgd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_CGD");

                entity.Property(e => e.AceptacionPpm)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("ACEPTACION_PPM");

                entity.Property(e => e.AntesPpm)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("ANTES_PPM");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.DespuesPpm)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("DESPUES_PPM");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.IncrementoPpm)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("INCREMENTO_PPM");

                entity.Property(e => e.LimiteMax)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("LIMITE_MAX");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.ResultadoPpm)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("RESULTADO_PPM");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Validacion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("VALIDACION");
            });

            modelBuilder.Entity<SplInfoDetalleDpr>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_DPR");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");

                entity.Property(e => e.Terminal1Mv)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TERMINAL1_MV");

                entity.Property(e => e.Terminal1Pc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TERMINAL1_PC");

                entity.Property(e => e.Terminal2Mv)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TERMINAL2_MV");

                entity.Property(e => e.Terminal2Pc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TERMINAL2_PC");

                entity.Property(e => e.Terminal3Mv)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TERMINAL3_MV");

                entity.Property(e => e.Terminal3Pc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TERMINAL3_PC");

                entity.Property(e => e.Tiempo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIEMPO");
            });

            modelBuilder.Entity<SplInfoDetalleFpa>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_FPA");

                entity.Property(e => e.Apertura)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("APERTURA");

                entity.Property(e => e.Asmt)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ASMT");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Escala)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ESCALA");

                entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                entity.Property(e => e.FactorPotencia)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("FACTOR_POTENCIA");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.LimiteMax)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("LIMITE_MAX");

                entity.Property(e => e.Medicion)
                    .HasColumnType("numeric(5, 3)")
                    .HasColumnName("MEDICION");

                entity.Property(e => e.Promedio)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("PROMEDIO");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Validacion)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("VALIDACION");

                entity.Property(e => e.Valor1)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("VALOR_1");

                entity.Property(e => e.Valor2)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("VALOR_2");

                entity.Property(e => e.Valor3)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("VALOR_3");

                entity.Property(e => e.Valor4)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("VALOR_4");

                entity.Property(e => e.Valor5)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("VALOR_5");
            });

            modelBuilder.Entity<SplInfoDetalleFpb>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Seccion, e.Renglon });

                entity.ToTable("SPL_INFO_DETALLE_FPB");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Capaci)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("CAPACI");

                entity.Property(e => e.Capacitancia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("CAPACITANCIA");

                entity.Property(e => e.ColumnaT)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_T");

                entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE");

                entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                entity.Property(e => e.FactorCorr2)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR2");

                entity.Property(e => e.FactorPotencia)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("FACTOR_POTENCIA");

                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_MARCA");

                entity.Property(e => e.IdTipo)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_TIPO");

                entity.Property(e => e.NoSerieBoq)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE_BOQ");

                entity.Property(e => e.PorcFp)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_FP");

                entity.Property(e => e.PorcFpCorr)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_FP_CORR");

                entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                entity.Property(e => e.Potencia)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("POTENCIA");
            });

            modelBuilder.Entity<SplInfoDetalleFpc>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Seccion, e.Renglon });

                entity.ToTable("SPL_INFO_DETALLE_FPC");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Capacitancia)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CAPACITANCIA");

                entity.Property(e => e.CorrPorcFp)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORR_PORC_FP");

                entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE");

                entity.Property(e => e.DevE)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEV_E");

                entity.Property(e => e.DevG)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEV_G");

                entity.Property(e => e.DevT)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEV_T");

                entity.Property(e => e.DevUst)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEV_UST");

                entity.Property(e => e.IdCap)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID_CAP");

                entity.Property(e => e.PorcFp)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_FP");

                entity.Property(e => e.Potencia)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("POTENCIA");

                entity.Property(e => e.TangPorcFp)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TANG_PORC_FP");
            });

            modelBuilder.Entity<SplInfoDetalleInd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_IND");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.NoPagina)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NO_PAGINA");

                entity.Property(e => e.NoPaginaFin)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NO_PAGINA_FIN");

                entity.Property(e => e.NoPaginaIni)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NO_PAGINA_INI");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.ValorKw)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("VALOR_KW");

                entity.Property(e => e.ValorMva)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("VALOR_MVA");
            });

            modelBuilder.Entity<SplInfoDetalleIsz>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_ISZ");

                entity.Property(e => e.CorrienteIrms)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE_IRMS");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.PorcJxo)
                    .HasColumnType("numeric(4, 3)")
                    .HasColumnName("PORC_JXO");

                entity.Property(e => e.PorcRo)
                    .HasColumnType("numeric(4, 3)")
                    .HasColumnName("PORC_RO");

                entity.Property(e => e.PorcZo)
                    .HasColumnType("numeric(4, 3)")
                    .HasColumnName("PORC_ZO");

                entity.Property(e => e.Posicion1)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION_1");

                entity.Property(e => e.Posicion2)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION_2");

                entity.Property(e => e.PotenciaCorrKw)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("POTENCIA_CORR_KW");

                entity.Property(e => e.PotenciaKw)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("POTENCIA_KW");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Tension1)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("TENSION_1");

                entity.Property(e => e.Tension2)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("TENSION_2");

                entity.Property(e => e.TensionVrms)
                    .HasColumnType("numeric(7, 1)")
                    .HasColumnName("TENSION_VRMS");

                entity.Property(e => e.ZBase)
                    .HasColumnType("numeric(8, 3)")
                    .HasColumnName("Z_BASE");

                entity.Property(e => e.ZOhms)
                    .HasColumnType("numeric(8, 3)")
                    .HasColumnName("Z_OHMS");
            });

            modelBuilder.Entity<SplInfoDetalleNra>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Seccion, e.Renglon, e.TipoInfo });

                entity.ToTable("SPL_INFO_DETALLE_NRA");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.TipoInfo)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_INFO");

                entity.Property(e => e.Altura)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ALTURA");

                entity.Property(e => e.D1000).HasColumnType("numeric(11, 6)");

                entity.Property(e => e.D10000).HasColumnType("numeric(11, 6)");

                entity.Property(e => e.D125).HasColumnType("numeric(11, 6)");

                entity.Property(e => e.D2000).HasColumnType("numeric(11, 6)");

                entity.Property(e => e.D250).HasColumnType("numeric(11, 6)");

                entity.Property(e => e.D315)
                    .HasColumnType("numeric(11, 6)")
                    .HasColumnName("D31_5");

                entity.Property(e => e.D4000).HasColumnType("numeric(11, 6)");

                entity.Property(e => e.D500).HasColumnType("numeric(11, 6)");

                entity.Property(e => e.D63).HasColumnType("numeric(11, 6)");

                entity.Property(e => e.D8000).HasColumnType("numeric(11, 6)");

                entity.Property(e => e.DbaCorr)
                    .HasColumnType("numeric(11, 6)")
                    .HasColumnName("DBA_CORR");

                entity.Property(e => e.DbaMedido)
                    .HasColumnType("numeric(11, 6)")
                    .HasColumnName("DBA_MEDIDO");

                entity.Property(e => e.Pos)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS");
            });

            modelBuilder.Entity<SplInfoDetallePce>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Seccion, e.Renglon });

                entity.ToTable("SPL_INFO_DETALLE_PCE");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.CorrienteIrms)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("CORRIENTE_IRMS");

                entity.Property(e => e.NominalKv)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("NOMINAL_KV");

                entity.Property(e => e.PerdidasCorr20)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PERDIDAS_CORR20");

                entity.Property(e => e.PerdidasKw)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PERDIDAS_KW");

                entity.Property(e => e.PerdidasOnda)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PERDIDAS_ONDA");

                entity.Property(e => e.PorcIexc)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_IEXC");

                entity.Property(e => e.PorcVn)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("PORC_VN");

                entity.Property(e => e.TensionAvg)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TENSION_AVG");

                entity.Property(e => e.TensionRms)
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
                    .HasColumnType("numeric(12, 0)")
                    .HasColumnName("TENSION_CD");

                entity.Property(e => e.Potencia)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("POTENCIA");

                entity.Property(e => e.PotenciaKw)
                    .HasColumnType("numeric(12, 0)")
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

            modelBuilder.Entity<SplInfoDetallePee>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Renglon });

                entity.ToTable("SPL_INFO_DETALLE_PEE");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.CoolingType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                entity.Property(e => e.CorrienteRms)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE_RMS");

                entity.Property(e => e.KwauxGar)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("KWAUX_GAR");

                entity.Property(e => e.MvaauxGar)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("MVAAUX_GAR");

                entity.Property(e => e.PotenciaKw)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("POTENCIA_KW");

                entity.Property(e => e.TensionRms)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_RMS");
            });

            modelBuilder.Entity<SplInfoDetallePim>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_PIM");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Pagina)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PAGINA");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Terminal)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TERMINAL");
            });

            modelBuilder.Entity<SplInfoDetallePir>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_PIR");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Pagina)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PAGINA");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Terminal)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TERMINAL");
            });

            modelBuilder.Entity<SplInfoDetallePlr>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Renglon });

                entity.ToTable("SPL_INFO_DETALLE_PLR");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE");

                entity.Property(e => e.PorcDesv)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_DESV");

                entity.Property(e => e.Reactancia)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("REACTANCIA");

                entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");

                entity.Property(e => e.Tiempo)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("TIEMPO");
            });

            modelBuilder.Entity<SplInfoDetallePrd>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_DETALLE_PRD");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.C4F)
                    .HasColumnType("numeric(10, 8)")
                    .HasColumnName("C4_F");

                entity.Property(e => e.CapKvar)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("CAP_KVAR");

                entity.Property(e => e.CnF)
                    .HasColumnType("numeric(16, 14)")
                    .HasColumnName("CN_F");

                entity.Property(e => e.Fc)
                    .HasColumnType("numeric(10, 6)")
                    .HasColumnName("FC");

                entity.Property(e => e.Fc2)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("FC2");

                entity.Property(e => e.GarantiaW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("GARANTIA_W");

                entity.Property(e => e.IAmps)
                    .HasColumnType("numeric(8, 3)")
                    .HasColumnName("I_AMPS");

                entity.Property(e => e.ImA)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("IM_A");

                entity.Property(e => e.LxpH)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("LXP_H");

                entity.Property(e => e.M3H)
                    .HasColumnType("numeric(9, 7)")
                    .HasColumnName("M3_H");

                entity.Property(e => e.PW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("P_W");

                entity.Property(e => e.PeW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("PE_W");

                entity.Property(e => e.PfeW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("PFE_W");

                entity.Property(e => e.PjmW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("PJM_W");

                entity.Property(e => e.PjmcW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("PJMC_W");

                entity.Property(e => e.PorcDesv)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("PORC_DESV");

                entity.Property(e => e.PtW)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("PT_W");

                entity.Property(e => e.R4sOhms)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("R4S_OHMS");

                entity.Property(e => e.RmOhms)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("RM_OHMS");

                entity.Property(e => e.RxpOhms)
                    .HasColumnType("numeric(13, 3)")
                    .HasColumnName("RXP_OHMS");

                entity.Property(e => e.TmC)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("TM_C");

                entity.Property(e => e.TmpC)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("TMP_C");

                entity.Property(e => e.TrC)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("TR_C");

                entity.Property(e => e.U).HasColumnType("numeric(7, 2)");

                entity.Property(e => e.VVolts)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("V_VOLTS");

                entity.Property(e => e.VmV)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("VM_V");

                entity.Property(e => e.XcOhms)
                    .HasColumnType("numeric(8, 2)")
                    .HasColumnName("XC_OHMS");

                entity.Property(e => e.XmOhms)
                    .HasColumnType("numeric(8, 2)")
                    .HasColumnName("XM_OHMS");
            });

            modelBuilder.Entity<SplInfoDetalleRad>(entity =>
            {
                entity.HasKey(e => new { e.IdCarga, e.Seccion, e.Tiempo, e.PosicionColumna });

                entity.ToTable("SPL_INFO_DETALLE_RAD");

                entity.Property(e => e.IdCarga)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_CARGA");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Tiempo)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("TIEMPO");

                entity.Property(e => e.PosicionColumna)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("POSICION_COLUMNA");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.ValorColumna)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("VALOR_COLUMNA");
            });

            modelBuilder.Entity<SplInfoDetalleRan>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Seccion, e.Renglon });

                entity.ToTable("SPL_INFO_DETALLE_RAN");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Duracion)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DURACION");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.Limite)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("LIMITE");

                entity.Property(e => e.Medicion)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("MEDICION");

                entity.Property(e => e.Tiempo)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIEMPO");

                entity.Property(e => e.UmMedicion)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_MEDICION");

                entity.Property(e => e.UmTiempo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("UM_TIEMPO");

                entity.Property(e => e.Valido)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("VALIDO");

                entity.Property(e => e.Vcd)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("VCD");
            });

            modelBuilder.Entity<SplInfoDetalleRct>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Seccion, e.Columna });

                entity.ToTable("SPL_INFO_DETALLE_RCT");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Columna)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("COLUMNA");

                entity.Property(e => e.Fase)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("FASE");

                entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                entity.Property(e => e.Resistencia)
                    .HasColumnType("numeric(11, 6)")
                    .HasColumnName("RESISTENCIA");

                entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMPERATURA");

                entity.Property(e => e.Terminal)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TERMINAL");

                entity.Property(e => e.TipoMedicion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_MEDICION");

                entity.Property(e => e.Unidad)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD");
            });

            modelBuilder.Entity<SplInfoDetalleRdd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_RDD");

                entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE");

                entity.Property(e => e.Fase)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("FASE");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Impedancia)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("IMPEDANCIA");

                entity.Property(e => e.Perdidas)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("PERDIDAS");

                entity.Property(e => e.PorcFp)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("PORC_FP");

                entity.Property(e => e.PorcX)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("PORC_X");

                entity.Property(e => e.Reactancia)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("REACTANCIA");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Resistencia)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("RESISTENCIA");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");
            });

            modelBuilder.Entity<SplInfoDetalleRdt>(entity =>
            {
                entity.HasKey(e => new { e.IdCarga, e.Posicion, e.PosicionColumna });

                entity.ToTable("SPL_INFO_DETALLE_RDT");

                entity.Property(e => e.IdCarga)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_CARGA");

                entity.Property(e => e.Posicion)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                entity.Property(e => e.PosicionColumna)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("POSICION_COLUMNA");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Hvvolts)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("HVVOLTS");

                entity.Property(e => e.Lvvolts)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("LVVOLTS");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.OrdenPosicion)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDEN_POSICION");

                entity.Property(e => e.ValorColumna)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("VALOR_COLUMNA");

                entity.Property(e => e.ValorDesv)
                    .HasColumnType("numeric(11, 4)")
                    .HasColumnName("VALOR_DESV");

                entity.Property(e => e.ValorNominal)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("VALOR_NOMINAL");
            });

            modelBuilder.Entity<SplInfoDetalleRod>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Seccion, e.Renglon });

                entity.ToTable("SPL_INFO_DETALLE_ROD");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Correccion20)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("CORRECCION_20");

                entity.Property(e => e.CorreccionSe)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("CORRECCION_SE");

                entity.Property(e => e.PorcDesv)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("PORC_DESV");

                entity.Property(e => e.PorcDesvDis)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("PORC_DESV_DIS");

                entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                entity.Property(e => e.ResDisenio)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("RES_DISENIO");

                entity.Property(e => e.ResistenciaProm)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("RESISTENCIA_PROM");

                entity.Property(e => e.Terminal1)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("TERMINAL_1");

                entity.Property(e => e.Terminal2)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("TERMINAL_2");

                entity.Property(e => e.Terminal3)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("TERMINAL_3");
            });

            modelBuilder.Entity<SplInfoDetalleRye>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_RYE");

                entity.Property(e => e.Eficiencia1)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_1");

                entity.Property(e => e.Eficiencia2)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_2");

                entity.Property(e => e.Eficiencia3)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_3");

                entity.Property(e => e.Eficiencia4)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_4");

                entity.Property(e => e.Eficiencia5)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_5");

                entity.Property(e => e.Eficiencia6)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_6");

                entity.Property(e => e.Eficiencia7)
                    .HasColumnType("numeric(8, 4)")
                    .HasColumnName("EFICIENCIA_7");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.PorcMva)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("PORC_MVA");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");
            });

            modelBuilder.Entity<SplInfoDetalleTap>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_TAP");

                entity.Property(e => e.AmpCal)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("AMP_CAL");

                entity.Property(e => e.Capacitancia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("CAPACITANCIA");

                entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE");

                entity.Property(e => e.DevAterrizado)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DEV_ATERRIZADO");

                entity.Property(e => e.DevEnergizado)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DEV_ENERGIZADO");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.NivelTension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("NIVEL_TENSION");

                entity.Property(e => e.PorcCorriente)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_CORRIENTE");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.TensionAplicada)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_APLICADA");

                entity.Property(e => e.Tiempo)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIEMPO");
            });

            modelBuilder.Entity<SplInfoDetalleTdp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_TDP");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");

                entity.Property(e => e.Terminal1)
                    .HasColumnType("numeric(6, 1)")
                    .HasColumnName("TERMINAL_1");

                entity.Property(e => e.Terminal2)
                    .HasColumnType("numeric(6, 1)")
                    .HasColumnName("TERMINAL_2");

                entity.Property(e => e.Terminal3)
                    .HasColumnType("numeric(6, 1)")
                    .HasColumnName("TERMINAL_3");

                entity.Property(e => e.Terminal4)
                    .HasColumnType("numeric(6, 1)")
                    .HasColumnName("TERMINAL_4");

                entity.Property(e => e.Terminal5)
                    .HasColumnType("numeric(6, 1)")
                    .HasColumnName("TERMINAL_5");

                entity.Property(e => e.Terminal6)
                    .HasColumnType("numeric(6, 1)")
                    .HasColumnName("TERMINAL_6");

                entity.Property(e => e.Tiempo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIEMPO");
            });

            modelBuilder.Entity<SplInfoGeneralArf>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_ARF");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Equipo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("EQUIPO");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NivelesTension)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NIVELES_TENSION");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.Terciario2dabaja)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TERCIARIO_2DABAJA");

                entity.Property(e => e.TerciarioDisp)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TERCIARIO_DISP");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.TotalPags)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TOTAL_PAGS");
            });

            modelBuilder.Entity<SplInfoGeneralBpc>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_BPC");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.MetodologiaUsada)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("METODOLOGIA_USADA");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Notas)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMPERATURA");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.ValorObtenido)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("VALOR_OBTENIDO");
            });

            modelBuilder.Entity<SplInfoGeneralCem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_GENERAL_CEM");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.IdPosPrimaria)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ID_POS_PRIMARIA");

                entity.Property(e => e.IdPosSecundaria)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ID_POS_SECUNDARIA");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.PosPrimaria)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_PRIMARIA");

                entity.Property(e => e.PosSecundaria)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_SECUNDARIA");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.TituloTerm1)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TITULO_TERM_1");

                entity.Property(e => e.TituloTerm2)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TITULO_TERM_2");

                entity.Property(e => e.TituloTerm3)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TITULO_TERM_3");

                entity.Property(e => e.VoltajePrueba)
                    .HasColumnType("numeric(7, 1)")
                    .HasColumnName("VOLTAJE_PRUEBA");
            });

            modelBuilder.Entity<SplInfoGeneralCgd>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_CGD");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TipoAceite)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_ACEITE");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.ValAceptCg)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("VAL_ACEPT_CG");
            });

            modelBuilder.Entity<SplInfoGeneralDpr>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_GENERAL_DPR");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.DescMayMv)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("DESC_MAY_MV");

                entity.Property(e => e.DescMayPc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("DESC_MAY_PC");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("FRECUENCIA");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.IncMaxPc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("INC_MAX_PC");

                entity.Property(e => e.Intervalo)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("INTERVALO");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NivelHora)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("NIVEL_HORA");

                entity.Property(e => e.NivelRealce)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("NIVEL_REALCE");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Notas)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");

                entity.Property(e => e.NumeroCiclo)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("NUMERO_CICLO");

                entity.Property(e => e.OtroCiclo)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("OTRO_CICLO");

                entity.Property(e => e.PosAt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                entity.Property(e => e.PosBt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                entity.Property(e => e.PosTer)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TensionPrueba)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TENSION_PRUEBA");

                entity.Property(e => e.TerminalesPrueba)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TERMINALES_PRUEBA");

                entity.Property(e => e.TiempoTotal)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TIEMPO_TOTAL");

                entity.Property(e => e.TipoMedicion)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_MEDICION");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            modelBuilder.Entity<SplInfoGeneralFpa>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_FPA");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.HumedadRelativa)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("HUMEDAD_RELATIVA");

                entity.Property(e => e.MarcaAceite)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MARCA_ACEITE");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Notas)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TempAmbiente)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_AMBIENTE");

                entity.Property(e => e.TipoAceite)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_ACEITE");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            modelBuilder.Entity<SplInfoGeneralFpb>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_FPB");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.CantBoq)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("CANT_BOQ");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TanDelta)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("TAN_DELTA");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            modelBuilder.Entity<SplInfoGeneralFpc>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_FPC");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Especificacion)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ESPECIFICACION");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("FRECUENCIA");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NivelesTension)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NIVELES_TENSION");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.TipoUnidad)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");
            });

            modelBuilder.Entity<SplInfoGeneralInd>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_IND");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Anexo)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ANEXO");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TcComprados)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TC_COMPRADOS");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.TotalPags)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TOTAL_PAGS");
            });

            modelBuilder.Entity<SplInfoGeneralIsz>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_GENERAL_ISZ");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.CantNeutros)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANT_NEUTROS");


                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.GradosCorr)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("GRADOS_CORR");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.MaterialDevanado)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("MATERIAL_DEVANADO");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.PosAt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                entity.Property(e => e.PosBt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");


                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

            });

            modelBuilder.Entity<SplInfoGeneralNra>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_GENERAL_NRA");

                entity.Property(e => e.Alfa)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ALFA");

                entity.Property(e => e.Alimentacion)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ALIMENTACION");

                entity.Property(e => e.Altura)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("ALTURA");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Area)
                    .HasColumnType("numeric(7, 2)")
                    .HasColumnName("AREA");

                entity.Property(e => e.CantMediciones)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANT_MEDICIONES");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.CargarInfo).HasColumnName("CARGAR_INFO");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClaveNorma)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_NORMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.CoolingType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FactorK)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("FACTOR_K");

                entity.Property(e => e.FechaDatos)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_DATOS");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Garantia)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("GARANTIA");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Laboratorio)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LABORATORIO");

                entity.Property(e => e.MedAyd)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MED_AYD");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Notas)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");

                entity.Property(e => e.Npplp)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("NPPLP");

                entity.Property(e => e.Perimetro)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("PERIMETRO");

                entity.Property(e => e.PosAt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                entity.Property(e => e.PosBt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                entity.Property(e => e.Prlw)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("PRLW");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.Sv)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("SV");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.UmAlimentacion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_ALIMENTACION");

                entity.Property(e => e.UmAltura)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_ALTURA");

                entity.Property(e => e.UmArea)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_AREA");

                entity.Property(e => e.UmPerimetro)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_PERIMETRO");

                entity.Property(e => e.ValorAlimentacion)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("VALOR_ALIMENTACION");
            });

            modelBuilder.Entity<SplInfoGeneralPce>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_PCE");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.DevEnergizado)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DEV_ENERGIZADO");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.GarantiaCexitacion)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("GARANTIA_CEXITACION");

                entity.Property(e => e.GarantiaPerdidas)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("GARANTIA_PERDIDAS");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.UmGarcexit)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_GARCEXIT");

                entity.Property(e => e.UmGarperd)
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

            modelBuilder.Entity<SplInfoGeneralPee>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_PEE");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            modelBuilder.Entity<SplInfoGeneralPim>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_GENERAL_PIM");

                entity.Property(e => e.AplicaBaja)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("APLICA_BAJA");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Conexion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NivelTension)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("NIVEL_TENSION");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.TotalPags)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TOTAL_PAGS");
            });

            modelBuilder.Entity<SplInfoGeneralPir>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_GENERAL_PIR");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Conexion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.IncluyeTerciario)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("INCLUYE_TERCIARIO");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NivelTension)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("NIVEL_TENSION");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.TotalPags)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TOTAL_PAGS");
            });

            modelBuilder.Entity<SplInfoGeneralPlr>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_PLR");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.CantTensiones)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANT_TENSIONES");

                entity.Property(e => e.CantTiempos)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANT_TIEMPOS");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.PorcDevtn)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_DEVTN");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.Rldtn)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("RLDTN");

                entity.Property(e => e.TensionNominal)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_NOMINAL");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            modelBuilder.Entity<SplInfoGeneralPrd>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_PRD");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.VoltajeNominal)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("VOLTAJE_NOMINAL");
            });

            modelBuilder.Entity<SplInfoGeneralRad>(entity =>
            {
                entity.HasKey(e => e.IdCarga)
                    .HasName("SPL_ID_RAD_PK");

                entity.ToTable("SPL_INFO_GENERAL_RAD");

                entity.Property(e => e.IdCarga)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_CARGA");

                entity.Property(e => e.Archivo).HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaCarga)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_CARGA");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TercerDevanadoTipo)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TERCER_DEVANADO_TIPO");

                entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.TipoUnidad)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");

                entity.Property(e => e.ValorMinimo)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("VALOR_MINIMO");
            });

            modelBuilder.Entity<SplInfoGeneralRan>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_RAN");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.CantMediciones)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CANT_MEDICIONES");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            modelBuilder.Entity<SplInfoGeneralRct>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_RCT");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TensionPrueba)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TENSION_PRUEBA");

                entity.Property(e => e.Ter2baja)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TER_2BAJA");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD_MEDIDA");
            });

            modelBuilder.Entity<SplInfoGeneralRdd>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_RDD");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.CapacidadPrueba)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("CAPACIDAD_PRUEBA");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Conexion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION");

                entity.Property(e => e.ConfigDevanado)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CONFIG_DEVANADO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.DevCorto)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DEV_CORTO");

                entity.Property(e => e.DevEnergizado)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DEV_ENERGIZADO");

                entity.Property(e => e.DporcX)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("DPORC_X");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.PorcJx)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_JX");

                entity.Property(e => e.PorcX)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("PORC_X");

                entity.Property(e => e.PorcZ)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORC_Z");

                entity.Property(e => e.PosAt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                entity.Property(e => e.PosBt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.S3fV2)
                    .HasColumnType("numeric(12, 6)")
                    .HasColumnName("S3F_V2");

                entity.Property(e => e.TensionCorto)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_CORTO");

                entity.Property(e => e.TensionEnerg)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_ENERG");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.ValorAceptacion)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("VALOR_ACEPTACION");
            });

            modelBuilder.Entity<SplInfoGeneralRdt>(entity =>
            {
                entity.HasKey(e => e.IdCarga)
                    .HasName("SPL_IG_RDT_PK");

                entity.ToTable("SPL_INFO_GENERAL_RDT");

                entity.Property(e => e.IdCarga)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_CARGA");

                entity.Property(e => e.Archivo).HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.ConexionSp)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CONEXION_SP");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.DesplazamientoAngular)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DESPLAZAMIENTO_ANGULAR");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA");

                entity.Property(e => e.FechaCarga)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_CARGA");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Norma)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("NORMA");

                entity.Property(e => e.PosAt)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                entity.Property(e => e.PostPruebaBt)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("POST_PRUEBA_BT");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.Ter)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("TER");

                entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            modelBuilder.Entity<SplInfoGeneralRod>(entity =>
            {
                entity.HasKey(e => e.IdRep);

                entity.ToTable("SPL_INFO_GENERAL_ROD");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.AutorizoCambio)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("AUTORIZO_CAMBIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.MaterialDevanado)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("MATERIAL_DEVANADO");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TensionPrueba)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TENSION_PRUEBA");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.TipoUnidad)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");

                entity.Property(e => e.UnidadMedida)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD_MEDIDA");
            });

            modelBuilder.Entity<SplInfoGeneralRye>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_GENERAL_RYE");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.CoolingType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoConexionesAt)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_CONEXIONES_AT");

                entity.Property(e => e.NoConexionesBt)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_CONEXIONES_BT");

                entity.Property(e => e.NoConexionesTer)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_CONEXIONES_TER");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TensionAt)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TENSION_AT");

                entity.Property(e => e.TensionBt)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TENSION_BT");

                entity.Property(e => e.TensionTer)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TENSION_TER");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            modelBuilder.Entity<SplInfoGeneralTap>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_GENERAL_TAP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("FRECUENCIA");

                entity.Property(e => e.IdCapAt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID_CAP_AT");

                entity.Property(e => e.IdCapBt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID_CAP_BT");

                entity.Property(e => e.IdCapTer)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID_CAP_TER");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.IdRepFpc)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP_FPC");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoConexionesAt)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_CONEXIONES_AT");

                entity.Property(e => e.NoConexionesBt)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_CONEXIONES_BT");

                entity.Property(e => e.NoConexionesTer)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_CONEXIONES_TER");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.TipoUnidad)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");

                entity.Property(e => e.ValAcept)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("VAL_ACEPT");
            });

            modelBuilder.Entity<SplInfoGeneralTdp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_GENERAL_TDP");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.DescMayMv)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("DESC_MAY_MV");

                entity.Property(e => e.DescMayPc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("DESC_MAY_PC");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("FRECUENCIA");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.IncMaxPc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("INC_MAX_PC");

                entity.Property(e => e.Intervalo)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("INTERVALO");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NivelHora)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("NIVEL_HORA");

                entity.Property(e => e.NivelRealce)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("NIVEL_REALCE");

                entity.Property(e => e.NivelesTension)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NIVELES_TENSION");

                entity.Property(e => e.NoCiclos)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("NO_CICLOS");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.PosAt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                entity.Property(e => e.PosBt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.TerminalesPrueba)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TERMINALES_PRUEBA");

                entity.Property(e => e.TiempoTotal)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("TIEMPO_TOTAL");

                entity.Property(e => e.TipoMedicion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_MEDICION");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            modelBuilder.Entity<SplInfoGeneralTin>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_GENERAL_TIN");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

                entity.Property(e => e.Capacidad)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ClavePrueba)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Cliente)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLIENTE");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("COMENTARIO");

                entity.Property(e => e.Conexion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.DevEnergizado)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DEV_ENERGIZADO");

                entity.Property(e => e.DevInducido)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DEV_INDUCIDO");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.FechaRep)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_REP");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("FRECUENCIA");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoPrueba)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("NO_PRUEBA");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");

                entity.Property(e => e.Notas)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");

                entity.Property(e => e.PosAt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                entity.Property(e => e.PosBt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                entity.Property(e => e.RelTension)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("REL_TENSION");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.Tension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION");

                entity.Property(e => e.TensionAplicada)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_APLICADA");

                entity.Property(e => e.TensionInducida)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_INDUCIDA");

                entity.Property(e => e.Tiempo)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TIEMPO");

                entity.Property(e => e.TipoReporte)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");
            });

            modelBuilder.Entity<SplInfoGraficaEtd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_GRAFICA_ETD");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.ValorX)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("VALOR_X");

                entity.Property(e => e.ValorY)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("VALOR_Y");
            });

            modelBuilder.Entity<SplInfoLaboratorio>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_LABORATORIO");

                entity.Property(e => e.Alfa)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ALFA");

                entity.Property(e => e.AreaPared1)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("AREA_PARED1");

                entity.Property(e => e.AreaPared2)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("AREA_PARED2");

                entity.Property(e => e.AreaPiso)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("AREA_PISO");

                entity.Property(e => e.AreaPuerta1)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("AREA_PUERTA1");

                entity.Property(e => e.AreaPuerta2)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("AREA_PUERTA2");

                entity.Property(e => e.AreaTecho)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("AREA_TECHO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Laboratorio)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LABORATORIO");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Sv)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("SV");
            });

            modelBuilder.Entity<SplInfoOctava>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_OCTAVAS");

                entity.Property(e => e.Altura)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ALTURA");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.D100).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D1000).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D10000).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D125).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D1250).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D16).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D160).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D1600).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D20).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D200).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D2000).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D25).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D250).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D2500).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D315)
                    .HasColumnType("numeric(16, 10)")
                    .HasColumnName("D31_5");

                entity.Property(e => e.D3150).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D3151)
                    .HasColumnType("numeric(16, 10)")
                    .HasColumnName("D315");

                entity.Property(e => e.D40).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D400).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D4000).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D50).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D500).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D5000).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D63).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D630).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D6300).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D80).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D800).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.D8000).HasColumnType("numeric(16, 10)");

                entity.Property(e => e.FechaDatos)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_DATOS");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Hora).HasColumnName("HORA");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.TipoInfo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_INFO");
            });

            modelBuilder.Entity<SplInfoRegRye>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_REG_RYE");

                entity.Property(e => e.Capacidad)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.FactPot1)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_1");

                entity.Property(e => e.FactPot2)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_2");

                entity.Property(e => e.FactPot3)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_3");

                entity.Property(e => e.FactPot4)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_4");

                entity.Property(e => e.FactPot5)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_5");

                entity.Property(e => e.FactPot6)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_6");

                entity.Property(e => e.FactPot7)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACT_POT_7");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.PerdidaCarga)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("PERDIDA_CARGA");

                entity.Property(e => e.PerdidaEnf)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("PERDIDA_ENF");

                entity.Property(e => e.PerdidaTotal)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("PERDIDA_TOTAL");

                entity.Property(e => e.PerdidaVacio)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("PERDIDA_VACIO");

                entity.Property(e => e.PorcR)
                    .HasColumnType("numeric(6, 4)")
                    .HasColumnName("PORC_R");

                entity.Property(e => e.PorcReg1)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_1");

                entity.Property(e => e.PorcReg2)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_2");

                entity.Property(e => e.PorcReg3)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_3");

                entity.Property(e => e.PorcReg4)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_4");

                entity.Property(e => e.PorcReg5)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_5");

                entity.Property(e => e.PorcReg6)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_6");

                entity.Property(e => e.PorcReg7)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("PORC_REG_7");

                entity.Property(e => e.PorcX)
                    .HasColumnType("numeric(6, 4)")
                    .HasColumnName("PORC_X");

                entity.Property(e => e.PorcZ)
                    .HasColumnType("numeric(5, 3)")
                    .HasColumnName("PORC_Z");

                entity.Property(e => e.ValorG)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("VALOR_G");

                entity.Property(e => e.ValorW)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("VALOR_W");

                entity.Property(e => e.XEntreR)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("X_ENTRE_R");
            });

            modelBuilder.Entity<SplInfoSeccionCgd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_SECCION_CGD");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.HrsTemperatura)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("HRS_TEMPERATURA");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Metodo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("METODO");

                entity.Property(e => e.Notas)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTAS");

                entity.Property(e => e.Resultado)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("RESULTADO");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");
            });









            modelBuilder.Entity<SplInfoGeneralEtd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_GENERAL_ETD");

                entity.Property(e => e.IdRep)
                 .IsRequired()
                    .HasColumnType("numeric(7)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.FechaRep)
                 .IsRequired()
                  .HasColumnType("datetime")
                  .HasColumnName("FECHA_REP");

                entity.Property(e => e.NoSerie)
                 .IsRequired()
                   .HasMaxLength(55)
                   .IsUnicode(false)
                   .HasColumnName("NO_SERIE");

                entity.Property(e => e.NoPrueba)
                 .IsRequired()
                  .HasMaxLength(2)
                  .IsUnicode(false)
                  .HasColumnName("NO_PRUEBA");


                entity.Property(e => e.ClaveIdioma)
                 .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("CLAVE_IDIOMA");



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

                
                entity.Property(e => e.Resultado).IsRequired().HasColumnName("RESULTADO");


                entity.Property(e => e.NombreArchivo)
                     .IsRequired()
                     .HasMaxLength(64)
                     .IsUnicode(false)
                     .HasColumnName("NOMBRE_ARCHIVO");


                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("ARCHIVO");

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

              
                entity.Property(e => e.IdCorte)
                 .IsRequired()
                 .HasColumnType("numeric(7)")
                 .HasColumnName("ID_CORTE");

                entity.Property(e => e.IdReg)
                 .IsRequired()
               .HasColumnType("numeric(7)")
               .HasColumnName("ID_REG");

              
                entity.Property(e => e.BtDifCap).IsRequired().HasColumnName("BT_DIF_CAP");

                entity.Property(e => e.CapacidadBt)
             .HasColumnType("numeric(7,3)")
             .HasColumnName("CAPACIDAD_BT");

                entity.Property(e => e.Terciario2b)
           .HasMaxLength(10)
            .IsRequired()
           .IsUnicode(false)
           .HasColumnName("TERCIARIO_2B");

                entity.Property(e => e.TerCapRed).IsRequired().HasColumnName("TER_CAP_RED");

                entity.Property(e => e.CapacidadTer)
          .HasColumnType("numeric(7,3)")
          .HasColumnName("CAPACIDAD_TER");

                entity.Property(e => e.DevanadoSplit)
                 .IsRequired()
       .HasMaxLength(10)
       .IsUnicode(false)
       .HasColumnName("DEVANADO_SPLIT");



                entity.Property(e => e.UltimaHora)
                 .IsRequired()
                .HasColumnType("datetime")
                .HasColumnName("ULTIMA_HORA");

                entity.Property(e => e.FactorKw)
                 .IsRequired()
        .HasColumnType("numeric(5,2)")
        .HasColumnName("FACTOR_KW");

                entity.Property(e => e.FactorAltitud)
                 .IsRequired()
       .HasColumnType("numeric(5,2)")
       .HasColumnName("FACTOR_ALTITUD");

                entity.Property(e => e.TipoRegresion).IsRequired().HasColumnName("TIPO_REGRESION");


                entity.Property(e => e.Comentario)
     .HasMaxLength(300)
     .IsUnicode(false)
     .HasColumnName("COMENTARIO");

                entity.Property(e => e.Creadopor)
                 .IsRequired()
  .HasMaxLength(100)
  .IsUnicode(false)
  .HasColumnName("CREADOPOR");

                entity.Property(e => e.Modificadopor)
.HasMaxLength(100)
.IsUnicode(false)
.HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Fechacreacion)
                 .IsRequired()
               .HasColumnType("datetime")
               .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
              .HasColumnType("datetime")
              .HasColumnName("FECHAMODIFICACION");


            });





            modelBuilder.Entity<SplInfoDetalleEtd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_DETALLE_ETD");

                entity.Property(e => e.IdRep)
                 .IsRequired()
                    .HasColumnType("numeric(7)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                 .IsRequired()
                   .HasColumnType("numeric(1)")
                   .HasColumnName("SECCION");

                entity.Property(e => e.Renglon)
                 .IsRequired()
                .HasColumnType("numeric(2)")
                .HasColumnName("RENGLON");

                entity.Property(e => e.FechaHora)
                  .HasColumnType("datetime")
                  .HasColumnName("FECHA_HORA");


                entity.Property(e => e.PromRadSup)

               .HasColumnType("numeric(5,2)")
               .HasColumnName("PROM_RAD_SUP");


                entity.Property(e => e.PromRadInf)
            .HasColumnType("numeric(5,2)")
            .HasColumnName("PROM_RAD_INF");

                entity.Property(e => e.AmbienteProm)
       .HasColumnType("numeric(4,2)")
       .HasColumnName("AMBIENTE_PROM");

                entity.Property(e => e.TempTapa)
       .HasColumnType("numeric(4,1)")
       .HasColumnName("TEMP_TAPA");

                entity.Property(e => e.Tor)
      .HasColumnType("numeric(4,2)")
      .HasColumnName("TOR");

                entity.Property(e => e.Aor)
     .HasColumnType("numeric(4,2)")
     .HasColumnName("AOR");


                entity.Property(e => e.Bor)
  .HasColumnType("numeric(4,2)")
  .HasColumnName("BOR");


                entity.Property(e => e.ElevAceiteSup)
 .HasColumnType("numeric(4,2)")
 .HasColumnName("ELEV_ACEITE_SUP");


                entity.Property(e => e.ElevAceiteInf)
.HasColumnType("numeric(4,2)")
.HasColumnName("ELEV_ACEITE_INF");

                entity.Property(e => e.ElevAceiteProm)
.HasColumnType("numeric(4,2)")
.HasColumnName("ELEV_ACEITE_PROM");

                entity.Property(e => e.Tiempo)
.HasColumnType("numeric(4,2)")
.HasColumnName("TIEMPO");


                entity.Property(e => e.Tiempo)
.HasColumnType("numeric(7,4)")
.HasColumnName("RESISTENCIA");






            });





            modelBuilder.Entity<SplInfoSeccionEtd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFO_SECCION_ETD");

                entity.Property(e => e.AltitudeF1)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ALTITUDE_F1");

                entity.Property(e => e.AltitudeF2)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ALTITUDE_F2");

                entity.Property(e => e.AwrLim)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("AWR_LIM");

                entity.Property(e => e.Capacidad)
                    .HasColumnType("numeric(8, 0)")
                    .HasColumnName("CAPACIDAD");

                entity.Property(e => e.CoolingType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("CORRIENTE");

                entity.Property(e => e.ElevPromDev)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("ELEV_PROM_DEV");

                entity.Property(e => e.ElevPtoMasCal)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("ELEV_PTO_MAS_CAL");

                entity.Property(e => e.FactorK)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("FACTOR_K");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.GradienteDev)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("GRADIENTE_DEV");

                entity.Property(e => e.GradienteLim)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_LIM");

                entity.Property(e => e.HsrLim)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("HSR_LIM");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.NivelTension)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("NIVEL_TENSION");

                entity.Property(e => e.OverElevation)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("OVER_ELEVATION");

                entity.Property(e => e.Perdidas)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("PERDIDAS");

                entity.Property(e => e.PosAt)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                entity.Property(e => e.PosBt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                entity.Property(e => e.ResistCorte)
                    .HasColumnType("numeric(7, 4)")
                    .HasColumnName("RESIST_CORTE");

                entity.Property(e => e.ResistTcero)
                    .HasColumnType("numeric(8, 5)")
                    .HasColumnName("RESIST_TCERO");

                entity.Property(e => e.Resultado).HasColumnName("RESULTADO");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Sobrecarga)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SOBRECARGA");

                entity.Property(e => e.TempDev)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMP_DEV");

                entity.Property(e => e.TempPromAceite)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("TEMP_PROM_ACEITE");

                entity.Property(e => e.TempResistCorte)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMP_RESIST_CORTE");

                entity.Property(e => e.Terminal)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TERMINAL");

                entity.Property(e => e.TorLimite)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TOR_LIMITE");

                entity.Property(e => e.UmResistencia)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_RESISTENCIA");
            });

            modelBuilder.Entity<SplInfoSeccionFpb>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Seccion });

                entity.ToTable("SPL_INFO_SECCION_FPB");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.AceptCap)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ACEPT_CAP");

                entity.Property(e => e.AceptFp)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ACEPT_FP");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.TempFptand)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_FPTAND");

                entity.Property(e => e.TempPorcfp)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_PORCFP");

                entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMPERATURA");

                entity.Property(e => e.TensionPrueba)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_PRUEBA");

                entity.Property(e => e.UmTemp)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMP");

                entity.Property(e => e.UmTension)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TENSION");
            });

            modelBuilder.Entity<SplInfoSeccionFpc>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Seccion });

                entity.ToTable("SPL_INFO_SECCION_FPC");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.AcepFp)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ACEP_FP");

                entity.Property(e => e.AceptCap)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ACEPT_CAP");

                entity.Property(e => e.FactorCorr)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("FACTOR_CORR");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.TempAceiteInf)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_ACEITE_INF");

                entity.Property(e => e.TempAceiteSup)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_ACEITE_SUP");

                entity.Property(e => e.TempCt)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TEMP_CT");

                entity.Property(e => e.TempProm)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("TEMP_PROM");

                entity.Property(e => e.TensionPrueba)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("TENSION_PRUEBA");

                entity.Property(e => e.UmTempacInf)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMPAC_INF");

                entity.Property(e => e.UmTempacSup)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMPAC_SUP");

                entity.Property(e => e.UmTension)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TENSION");
            });

            modelBuilder.Entity<SplInfoSeccionPce>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Seccion });

                entity.ToTable("SPL_INFO_SECCION_PCE");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.CapMin)
                    .HasColumnType("numeric(10, 0)")
                    .HasColumnName("CAP_MIN");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("FRECUENCIA");

                entity.Property(e => e.PosAt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_AT");

                entity.Property(e => e.PosBt)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_BT");

                entity.Property(e => e.PosTer)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POS_TER");

                entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TEMPERATURA");

                entity.Property(e => e.UmCapmin)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_CAPMIN");

                entity.Property(e => e.UmFrec)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_FREC");

                entity.Property(e => e.UmTemp)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMP");

                entity.Property(e => e.UmVolbase)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UM_VOLBASE");

                entity.Property(e => e.VnFin)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("VN_FIN");

                entity.Property(e => e.VnInicio)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("VN_INICIO");

                entity.Property(e => e.VnIntervalo)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("VN_INTERVALO");

                entity.Property(e => e.VoltajeBase)
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

            modelBuilder.Entity<SplInfoSeccionRad>(entity =>
            {
                entity.HasKey(e => new { e.IdCarga, e.Seccion })
                    .HasName("SPL_IS_RAD_PK");

                entity.ToTable("SPL_INFO_SECCION_RAD");

                entity.Property(e => e.IdCarga)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_CARGA");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(10, 1)")
                    .HasColumnName("TEMPERATURA");

                entity.Property(e => e.Tension)
                    .HasColumnType("numeric(10, 4)")
                    .HasColumnName("TENSION");

                entity.Property(e => e.Umtemp)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("UMTEMP");

                entity.Property(e => e.Umtension)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("UMTENSION");
            });

            modelBuilder.Entity<SplInfoSeccionRod>(entity =>
            {
                entity.HasKey(e => new { e.IdRep, e.Seccion });

                entity.ToTable("SPL_INFO_SECCION_ROD");

                entity.Property(e => e.IdRep)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_REP");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Fc20)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("FC_20");

                entity.Property(e => e.FcSe)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("FC_SE");

                entity.Property(e => e.FechaPrueba)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHA_PRUEBA");

                entity.Property(e => e.MaxDdis)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("MAX_DDIS");

                entity.Property(e => e.MaxDesv)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("MAX_DESV");

                entity.Property(e => e.MinDdis)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("MIN_DDIS");

                entity.Property(e => e.TempSe)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMP_SE");

                entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMPERATURA");

                entity.Property(e => e.TitTerm1)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIT_TERM_1");

                entity.Property(e => e.TitTerm2)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIT_TERM_2");

                entity.Property(e => e.TitTerm3)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("TIT_TERM_3");

                entity.Property(e => e.UmTemp)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMP");

                entity.Property(e => e.UmTempse)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("UM_TEMPSE");

                entity.Property(e => e.VaDesv)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("VA_DESV");

                entity.Property(e => e.VaMaxDis)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("VA_MAX_DIS");

                entity.Property(e => e.VaMinDis)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("VA_MIN_DIS");
            });

            modelBuilder.Entity<SplInfoaparatoApr>(entity =>
            {
                entity.HasKey(e => new { e.OrderCode, e.ColumnTypeId });

                entity.ToTable("SPL_INFOAPARATO_APR");

                entity.HasIndex(e => e.OrderCode, "I_infoaparatoApar_order");

                entity.HasIndex(e => new { e.OrderCode, e.ColumnTypeId }, "PK_SPL_INFOAPARATO_APR_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                entity.Property(e => e.ColumnTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COLUMN_TYPE_ID");

                entity.Property(e => e.ColumnTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COLUMN_TITLE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.OrderIndex)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_INDEX");

                entity.Property(e => e.Qty)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("QTY");
            });

            modelBuilder.Entity<SplInfoaparatoBoq>(entity =>
            {
                entity.HasKey(e => new { e.OrderCode, e.ColumnTypeId });

                entity.ToTable("SPL_INFOAPARATO_BOQ");

                entity.HasIndex(e => e.OrderCode, "I_infoaparatoBoqs_order");

                entity.HasIndex(e => new { e.OrderCode, e.ColumnTypeId }, "PK_SPL_INFOAPARATO_BOQ_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                entity.Property(e => e.ColumnTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COLUMN_TYPE_ID");

                entity.Property(e => e.BilClass)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("BIL_CLASS");

                entity.Property(e => e.BilClassOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BIL_CLASS_OTHER");

                entity.Property(e => e.ColumnTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COLUMN_TITLE");

                entity.Property(e => e.CorrienteUnidad)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE_UNIDAD");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.CurrentAmps)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CURRENT_AMPS");

                entity.Property(e => e.CurrentAmpsReq)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CURRENT_AMPS_REQ");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.OrderIndex)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_INDEX");

                entity.Property(e => e.Qty)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("QTY");

                entity.Property(e => e.VoltageClass)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("VOLTAGE_CLASS");

                entity.Property(e => e.VoltageClassOther)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("VOLTAGE_CLASS_OTHER");
            });

            modelBuilder.Entity<SplInfoaparatoBoqdet>(entity =>
            {
                entity.HasKey(e => new { e.NoSerie, e.NoSerieBoq });

                entity.ToTable("SPL_INFOAPARATO_BOQDET");

                entity.Property(e => e.NoSerie)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.NoSerieBoq)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE_BOQ");

                entity.Property(e => e.Capacitancia)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("CAPACITANCIA");

                entity.Property(e => e.Capacitancia2)
                    .HasColumnType("numeric(6, 0)")
                    .HasColumnName("CAPACITANCIA2");

                entity.Property(e => e.Corriente)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("CORRIENTE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.FactorPotencia)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("FACTOR_POTENCIA");

                entity.Property(e => e.FactorPotencia2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("FACTOR_POTENCIA2");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_MARCA");

                entity.Property(e => e.IdTipo)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_TIPO");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Orden)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("ORDEN");

                entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                entity.Property(e => e.Prueba).HasColumnName("PRUEBA");

                entity.Property(e => e.Voltaje)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("VOLTAJE");
            });

            modelBuilder.Entity<SplInfoaparatoCam>(entity =>
            {
                entity.HasKey(e => new { e.OrderCode, e.ColumnTypeId });

                entity.ToTable("SPL_INFOAPARATO_CAM");

                entity.HasIndex(e => e.OrderCode, "I_infoaparatoCam_order");

                entity.HasIndex(e => new { e.OrderCode, e.ColumnTypeId }, "PK_SPL_INFOAPARATO_CAM_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                entity.Property(e => e.ColumnTypeId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COLUMN_TYPE_ID");

                entity.Property(e => e.ColumnTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COLUMN_TITLE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

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

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.FlagRcbnFcbn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FLAG_RCBN_FCBN");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.OperationId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("OPERATION_ID");

                entity.Property(e => e.OrderIndex)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ORDER_INDEX");

                entity.Property(e => e.Taps)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TAPS");
            });

            modelBuilder.Entity<SplInfoaparatoCap>(entity =>
            {
                entity.HasKey(e => new { e.OrderCode, e.Secuencia });

                entity.ToTable("SPL_INFOAPARATO_CAP");

                entity.HasIndex(e => e.OrderCode, "I_infoaparatoCaps_order");

                entity.HasIndex(e => new { e.OrderCode, e.Secuencia }, "PK_SPL_INFOAPARATO_CAP_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                entity.Property(e => e.Secuencia)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SECUENCIA");

                entity.Property(e => e.CoolingType)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("COOLING_TYPE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.DevAwr)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("DEV_AWR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Hstr)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("HSTR");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

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
                    .HasColumnName("MVAF4");

                entity.Property(e => e.OverElevation)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("OVER_ELEVATION");
            });

            modelBuilder.Entity<SplInfoaparatoCar>(entity =>
            {
                entity.HasKey(e => e.OrderCode);

                entity.ToTable("SPL_INFOAPARATO_CAR");

                entity.HasIndex(e => e.OrderCode, "I_infoaparatoCars_order");

                entity.HasIndex(e => e.OrderCode, "PK_SPL_INFOAPARATO_CAR_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                entity.Property(e => e.ConexionEq)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION_EQ");

                entity.Property(e => e.ConexionEq2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION_EQ_2");

                entity.Property(e => e.ConexionEq3)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION_EQ_3");

                entity.Property(e => e.ConexionEq4)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION_EQ_4");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Mvaf1ConnectionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF1_CONNECTION_ID");

                entity.Property(e => e.Mvaf1ConnectionOther)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MVAF1_CONNECTION_OTHER");

                entity.Property(e => e.Mvaf1DerDown)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_DER_DOWN");

                entity.Property(e => e.Mvaf1DerUp)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_DER_UP");

                entity.Property(e => e.Mvaf1Nbai1)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF1_NBAI1");

                entity.Property(e => e.Mvaf1NbaiNeutro)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF1_NBAI_NEUTRO");

                entity.Property(e => e.Mvaf1Taps)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MVAF1_TAPS");

                entity.Property(e => e.Mvaf1Voltage1)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_VOLTAGE1");

                entity.Property(e => e.Mvaf1Voltage2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_VOLTAGE2");

                entity.Property(e => e.Mvaf1Voltage3)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_VOLTAGE3");

                entity.Property(e => e.Mvaf1Voltage4)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("MVAF1_VOLTAGE4");

                entity.Property(e => e.Mvaf2ConnectionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF2_CONNECTION_ID");

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

                entity.Property(e => e.Mvaf2NbaiNeutro)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF2_NBAI_NEUTRO");

                entity.Property(e => e.Mvaf2Taps)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MVAF2_TAPS");

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

                entity.Property(e => e.Mvaf3ConnectionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF3_CONNECTION_ID");

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

                entity.Property(e => e.Mvaf3NbaiNeutro)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF3_NBAI_NEUTRO");

                entity.Property(e => e.Mvaf3Taps)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MVAF3_TAPS");

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

                entity.Property(e => e.Mvaf4ConnectionId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("MVAF4_CONNECTION_ID");

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

                entity.Property(e => e.Mvaf4NbaiNeutro)
                    .HasColumnType("numeric(4, 0)")
                    .HasColumnName("MVAF4_NBAI_NEUTRO");

                entity.Property(e => e.Mvaf4Taps)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("MVAF4_TAPS");

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

                entity.Property(e => e.RcbnFcbn1)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("RCBN_FCBN1");

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

            modelBuilder.Entity<SplInfoaparatoDg>(entity =>
            {
                entity.HasKey(e => e.OrderCode);

                entity.ToTable("SPL_INFOAPARATO_DG");

                entity.HasIndex(e => e.OrderCode, "I_infoaparatoDGs_order");

                entity.HasIndex(e => e.OrderCode, "PK_SPL_INFOAPARATO_DG_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                entity.Property(e => e.AltitudeF1)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ALTITUDE_F1");

                entity.Property(e => e.AltitudeF2)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ALTITUDE_F2");

                entity.Property(e => e.Applicationid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("APPLICATIONID");

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER_NAME");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.DesplazamientoAngular)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DESPLAZAMIENTO_ANGULAR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Frecuency)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FRECUENCY");

                entity.Property(e => e.LanguageId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LANGUAGE_ID");

                entity.Property(e => e.MarcaAceite)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MARCA_ACEITE");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Norma)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("NORMA");

                entity.Property(e => e.Phases)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("PHASES");

                entity.Property(e => e.PoNumeric)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PO_NUMERIC");

                entity.Property(e => e.PolarityId)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("POLARITY_ID");

                entity.Property(e => e.PolarityOther)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POLARITY_OTHER");

                entity.Property(e => e.Standardid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("STANDARDID");

                entity.Property(e => e.TipoAceite)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_ACEITE");

                entity.Property(e => e.TipoUnidad)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");

                entity.Property(e => e.Typetrafoid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TYPETRAFOID");
            });

            modelBuilder.Entity<SplInfoaparatoEst>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_INFOAPARATO_EST");

                entity.Property(e => e.AmbienteCte)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("AMBIENTE_CTE");

                entity.Property(e => e.AorHenf)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AOR_HENF");

                entity.Property(e => e.AorLimite)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("AOR_LIMITE");

                entity.Property(e => e.AwrHenfAt)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AWR_HENF_AT");

                entity.Property(e => e.AwrHenfBt)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AWR_HENF_BT");

                entity.Property(e => e.AwrHenfTer)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("AWR_HENF_TER");

                entity.Property(e => e.AwrLimAt)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("AWR_LIM_AT");

                entity.Property(e => e.AwrLimBt)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("AWR_LIM_BT");

                entity.Property(e => e.AwrLimTer)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("AWR_LIM_TER");

                entity.Property(e => e.Bor)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("BOR");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.CteTiempoTrafo)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("CTE_TIEMPO_TRAFO");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.GradienteHentAt)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_HENT_AT");

                entity.Property(e => e.GradienteHentBt)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_HENT_BT");

                entity.Property(e => e.GradienteHentTer)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_HENT_TER");

                entity.Property(e => e.GradienteLimAt)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_LIM_AT");

                entity.Property(e => e.GradienteLimBt)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_LIM_BT");

                entity.Property(e => e.GradienteLimTer)
                    .HasColumnType("numeric(4, 2)")
                    .HasColumnName("GRADIENTE_LIM_TER");

                entity.Property(e => e.HsrHenfAt)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("HSR_HENF_AT");

                entity.Property(e => e.HsrHenfBt)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("HSR_HENF_BT");

                entity.Property(e => e.HsrHenfTer)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("HSR_HENF_TER");

                entity.Property(e => e.HsrLimAt)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("HSR_LIM_AT");

                entity.Property(e => e.HsrLimBt)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("HSR_LIM_BT");

                entity.Property(e => e.HsrLimTer)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("HSR_LIM_TER");

                entity.Property(e => e.KwDiseno)
                    .HasColumnType("numeric(7, 3)")
                    .HasColumnName("KW_DISENO");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoSerie)
                    .IsRequired()
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.Toi)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TOI");

                entity.Property(e => e.ToiLimite)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TOI_LIMITE");

                entity.Property(e => e.TorHenf)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("TOR_HENF");

                entity.Property(e => e.TorLimite)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("TOR_LIMITE");
            });

            modelBuilder.Entity<SplInfoaparatoGar>(entity =>
            {
                entity.HasKey(e => e.OrderCode);

                entity.ToTable("SPL_INFOAPARATO_GAR");

                entity.HasIndex(e => e.OrderCode, "I_infoaparatoGars_order");

                entity.HasIndex(e => e.OrderCode, "PK_SPL_INFOAPARATO_GAR_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Iexc100)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("IEXC_100");

                entity.Property(e => e.Iexc110)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("IEXC_110");

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
                    .HasColumnType("numeric(9, 3)")
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

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NoiseFa1)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_FA1");

                entity.Property(e => e.NoiseFa2)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_FA2");

                entity.Property(e => e.NoiseOa)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("NOISE_OA");

                entity.Property(e => e.TolerancyKwAux)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_KW_AUX");

                entity.Property(e => e.TolerancyKwCu)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_KW_CU");

                entity.Property(e => e.TolerancyKwfe)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_KWFE");

                entity.Property(e => e.TolerancyKwtot)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TOLERANCY_KWTOT");

                entity.Property(e => e.TolerancyZpositive)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TOLERANCY_ZPOSITIVE");

                entity.Property(e => e.TolerancyZpositive2)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("TOLERANCY_ZPOSITIVE2");

                entity.Property(e => e.ZPositiveHx)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("Z_POSITIVE_HX");

                entity.Property(e => e.ZPositiveHy)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("Z_POSITIVE_HY");

                entity.Property(e => e.ZPositiveMva)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("Z_POSITIVE_MVA");

                entity.Property(e => e.ZPositiveXy)
                    .HasColumnType("numeric(9, 3)")
                    .HasColumnName("Z_POSITIVE_XY");
            });

            modelBuilder.Entity<SplInfoaparatoLab>(entity =>
            {
                entity.HasKey(e => e.OrderCode);

                entity.ToTable("SPL_INFOAPARATO_LAB");

                entity.HasIndex(e => e.OrderCode, "I_infoaparatoLabs_order");

                entity.HasIndex(e => e.OrderCode, "PK_SPL_INFOAPARATO_LAB_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.TextTestDielectric)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TEXT_TEST_DIELECTRIC");

                entity.Property(e => e.TextTestPrototype)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TEXT_TEST_PROTOTYPE");

                entity.Property(e => e.TextTestRoutine)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TEXT_TEST_ROUTINE");
            });

            modelBuilder.Entity<SplInfoaparatoNor>(entity =>
            {
                entity.HasKey(e => new { e.OrderCode, e.Secuencia });

                entity.ToTable("SPL_INFOAPARATO_NOR");

                entity.HasIndex(e => e.OrderCode, "I_infoaparatoNorms_order");

                entity.HasIndex(e => new { e.OrderCode, e.Secuencia }, "PK_SPL_INFOAPARATO_NOR_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                entity.Property(e => e.Secuencia)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("SECUENCIA");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplInfoaparatoTap>(entity =>
            {
                entity.HasKey(e => e.OrderCode);

                entity.ToTable("SPL_INFOAPARATO_TAPS");

                entity.HasIndex(e => e.OrderCode, "PK_SPL_INFOAPARATO_TAPS_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                entity.Property(e => e.CantidadInfBc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CANTIDAD_INF_BC");

                entity.Property(e => e.CantidadInfSc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CANTIDAD_INF_SC");

                entity.Property(e => e.CantidadSupBc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CANTIDAD_SUP_BC");

                entity.Property(e => e.CantidadSupSc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("CANTIDAD_SUP_SC");

                entity.Property(e => e.ComboNumericBc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COMBO_NUMERIC_BC");

                entity.Property(e => e.ComboNumericSc)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COMBO_NUMERIC_SC");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.IdentificacionBc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("IDENTIFICACION_BC");

                entity.Property(e => e.IdentificacionSc)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("IDENTIFICACION_SC");

                entity.Property(e => e.InvertidoBc)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("INVERTIDO_BC");

                entity.Property(e => e.InvertidoSc)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("INVERTIDO_SC");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NominalBc)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("NOMINAL_BC");

                entity.Property(e => e.NominalSc)
                    .HasColumnType("numeric(3, 0)")
                    .HasColumnName("NOMINAL_SC");

                entity.Property(e => e.PorcentajeInfBc)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORCENTAJE_INF_BC");

                entity.Property(e => e.PorcentajeInfSc)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORCENTAJE_INF_SC");

                entity.Property(e => e.PorcentajeSupBc)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORCENTAJE_SUP_BC");

                entity.Property(e => e.PorcentajeSupSc)
                    .HasColumnType("numeric(6, 3)")
                    .HasColumnName("PORCENTAJE_SUP_SC");
            });

            modelBuilder.Entity<SplMarcasBoq>(entity =>
            {
                entity.HasKey(e => e.IdMarca);

                entity.ToTable("SPL_MARCAS_BOQ");

                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(5, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_MARCA");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Estatus).HasColumnName("ESTATUS");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplNorma>(entity =>
            {
                entity.HasKey(e => e.Clave)
                    .HasName("SPL_N_PK");

                entity.ToTable("SPL_NORMA");

                entity.HasIndex(e => e.Clave, "SPL_N_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Clave)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplNormasrep>(entity =>
            {
                entity.HasKey(e => new { e.ClaveNorma, e.ClaveIdioma, e.Secuencia });

                entity.ToTable("SPL_NORMASREP");

                entity.HasIndex(e => e.ClaveIdioma, "I_normasrep_idioma");

                entity.HasIndex(e => e.ClaveNorma, "I_normasrep_norma");

                entity.HasIndex(e => new { e.ClaveNorma, e.ClaveIdioma, e.Secuencia }, "PK_SPL_NORMASREP_UNIQUE")
                    .IsUnique();

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
                    .HasMaxLength(100)
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplOpcione>(entity =>
            {
                entity.HasKey(e => e.Clave);

                entity.ToTable("SPL_OPCIONES");

                entity.Property(e => e.Clave)
                    .HasColumnType("decimal(38, 0)")
                    .HasColumnName("CLAVE");

                entity.Property(e => e.ClavePadre)
                    .HasColumnType("decimal(38, 0)")
                    .HasColumnName("CLAVE_PADRE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Orden).HasColumnName("ORDEN");

                entity.Property(e => e.SubMenu)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SUB_MENU");

                entity.Property(e => e.Url)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<SplPerfile>(entity =>
            {
                entity.HasKey(e => e.Clave);

                entity.ToTable("SPL_PERFILES");

                entity.Property(e => e.Clave)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("date")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("date")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplPermiso>(entity =>
            {
                entity.HasKey(e => new { e.ClavePerfil, e.ClaveOpcion });

                entity.ToTable("SPL_PERMISOS");

                entity.Property(e => e.ClavePerfil)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PERFIL");

                entity.Property(e => e.ClaveOpcion)
                    .HasMaxLength(38)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_OPCION");
            });

            modelBuilder.Entity<SplPlantillaBase>(entity =>
            {
                entity.HasKey(e => new { e.TipoReporte, e.ClavePrueba, e.ClaveIdioma, e.ColumnasConfigurables })
                    .HasName("SPL_PB_PK");

                entity.ToTable("SPL_PLANTILLA_BASE");

                entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.ClavePrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.ColumnasConfigurables)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("COLUMNAS_CONFIGURABLES");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Plantilla).HasColumnName("PLANTILLA");
            });

            modelBuilder.Entity<SplPrueba>(entity =>
            {
                entity.HasKey(e => new { e.TipoReporte, e.ClavePrueba })
                    .HasName("SPL_P_PK");

                entity.ToTable("SPL_PRUEBAS");

                entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.ClavePrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplRepConsolidado>(entity =>
            {
                entity.HasKey(e => e.Idioma);

                entity.ToTable("SPL_REP_CONSOLIDADO");

                entity.HasIndex(e => e.Idioma, "PK_SPL_REP_CONSOLIDADO_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Idioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("IDIOMA");

                entity.Property(e => e.Archivo).HasColumnName("ARCHIVO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.NombreArchivo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_ARCHIVO");
            });

            modelBuilder.Entity<SplReporte>(entity =>
            {
                entity.HasKey(e => e.TipoReporte)
                    .HasName("SPL_R_PK");

                entity.ToTable("SPL_REPORTES");

                entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.Agrupacion)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("AGRUPACION");

                entity.Property(e => e.AgrupacionEn)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("AGRUPACION_EN");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.DescripcionEn)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION_EN");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplResistDiseno>(entity =>
            {
                entity.HasKey(e => new { e.NoSerie, e.UnidadMedida, e.ConexionPrueba, e.Temperatura, e.IdSeccion, e.Orden });

                entity.ToTable("SPL_RESIST_DISENO");

                entity.Property(e => e.NoSerie)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("NO_SERIE");

                entity.Property(e => e.UnidadMedida)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD_MEDIDA");

                entity.Property(e => e.ConexionPrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CONEXION_PRUEBA");

                entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("TEMPERATURA");

                entity.Property(e => e.IdSeccion)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ID_SECCION");

                entity.Property(e => e.Orden)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ORDEN");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                entity.Property(e => e.Resistencia)
                    .HasColumnType("numeric(12, 6)")
                    .HasColumnName("RESISTENCIA");
            });

            modelBuilder.Entity<SplSerieParalelo>(entity =>
            {
                entity.HasKey(e => e.Clave);

                entity.ToTable("SPL_SERIE_PARALELO");

                entity.HasIndex(e => e.Clave, "PK_SPL_SERIE_PARALELO_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Clave)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CLAVE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
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

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplTensionPlaca>(entity =>
            {
                entity.HasKey(e => new { e.Unidad, e.TipoTension, e.Orden })
                    .HasName("SPL_TENSION_PLACA_PK");

                entity.ToTable("SPL_TENSION_PLACA");

                entity.HasIndex(e => new { e.Unidad, e.TipoTension, e.Orden }, "SPL_TENSION_PLACA_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Unidad)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD");

                entity.Property(e => e.TipoTension)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_TENSION");

                entity.Property(e => e.Orden)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ORDEN");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");

                entity.Property(e => e.Tension)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("TENSION");
            });

            modelBuilder.Entity<SplTercerDevanadoTipo>(entity =>
            {
                entity.HasKey(e => e.Clave)
                    .HasName("SPL_TDT_PK");

                entity.ToTable("SPL_TERCER_DEVANADO_TIPO");

                entity.HasIndex(e => e.Clave, "SPL_TDT_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Clave)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplTipoUnidad>(entity =>
            {
                entity.HasKey(e => e.Clave)
                    .HasName("SPL_TU_PK");

                entity.ToTable("SPL_TIPO_UNIDAD");

                entity.HasIndex(e => e.Clave, "SPL_TU_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Clave)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplTiposxmarcaBoq>(entity =>
            {
                entity.HasKey(e => new { e.IdMarca, e.IdTipo });

                entity.ToTable("SPL_TIPOSXMARCA_BOQ");

                entity.Property(e => e.IdMarca)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_MARCA");

                entity.Property(e => e.IdTipo)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ID_TIPO");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Estatus).HasColumnName("ESTATUS");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplTitSerieparalelo>(entity =>
            {
                entity.HasKey(e => new { e.ClaveSepa, e.ClaveIdioma });

                entity.ToTable("SPL_TIT_SERIEPARALELO");

                entity.HasIndex(e => new { e.ClaveSepa, e.ClaveIdioma }, "PK_SPL_TIT_SERIEPARALELO_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ClaveSepa)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("CLAVE_SEPA");

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplTituloColumnasCem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_TITULO_COLUMNAS_CEM");

                entity.Property(e => e.ClaveIdioma)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.Columna1)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_1");

                entity.Property(e => e.Columna2)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_2");

                entity.Property(e => e.Columna3)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_3");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.PosSec)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("POS_SEC");

                entity.Property(e => e.Tipo)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("TIPO");

                entity.Property(e => e.TituloPos)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TITULO_POS");
            });

            modelBuilder.Entity<SplTituloColumnasFpc>(entity =>
            {
                entity.HasKey(e => new { e.TipoUnidad, e.ClaveIdioma, e.Renglon });

                entity.ToTable("SPL_TITULO_COLUMNAS_FPC");

                entity.Property(e => e.TipoUnidad)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.Renglon)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("RENGLON");

                entity.Property(e => e.Columna1)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_1");

                entity.Property(e => e.Columna2)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_2");

                entity.Property(e => e.Columna3)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_3");

                entity.Property(e => e.Columna4)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_4");

                entity.Property(e => e.Columna5)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("COLUMNA_5");

                entity.Property(e => e.ConstanteX)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("CONSTANTE_X");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.VcPorcFp)
                    .HasColumnType("numeric(4, 1)")
                    .HasColumnName("VC_PORC_FP");
            });

            modelBuilder.Entity<SplTituloColumnasRad>(entity =>
            {
                entity.HasKey(e => new { e.TipoUnidad, e.TercerDevanadoTipo, e.PosColumna, e.ClaveIdioma })
                    .HasName("SPL_TC_RAD_PK");

                entity.ToTable("SPL_TITULO_COLUMNAS_RAD");

                entity.HasIndex(e => new { e.TipoUnidad, e.TercerDevanadoTipo, e.PosColumna, e.ClaveIdioma }, "SPL_TC_RAD_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.TipoUnidad)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");

                entity.Property(e => e.TercerDevanadoTipo)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TERCER_DEVANADO_TIPO");

                entity.Property(e => e.PosColumna)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("POS_COLUMNA");

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");
            });

            modelBuilder.Entity<SplTituloColumnasRdt>(entity =>
            {
                entity.HasKey(e => new { e.ClavePrueba, e.DesplazamientoAngular, e.Norma, e.PosColumna, e.ClaveIdioma })
                    .HasName("SPL_TC_RDT_PK");

                entity.ToTable("SPL_TITULO_COLUMNAS_RDT");

                entity.HasIndex(e => new { e.ClavePrueba, e.DesplazamientoAngular, e.Norma, e.PosColumna, e.ClaveIdioma }, "SPL_TC_RDT_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ClavePrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

                entity.Property(e => e.DesplazamientoAngular)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("DESPLAZAMIENTO_ANGULAR");

                entity.Property(e => e.Norma)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("NORMA");

                entity.Property(e => e.PosColumna)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("POS_COLUMNA");

                entity.Property(e => e.ClaveIdioma)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_IDIOMA");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.PrimerRenglon)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("PRIMER_RENGLON");

                entity.Property(e => e.SegundoRenglon)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("SEGUNDO_RENGLON");

                entity.Property(e => e.TitPos1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIT_POS_1");

                entity.Property(e => e.TitPos2)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIT_POS_2");

                entity.Property(e => e.TitPosUnica1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIT_POS_UNICA1");

                entity.Property(e => e.TitPosUnica2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIT_POS_UNICA2");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");
            });

            modelBuilder.Entity<SplUsuario>(entity =>
            {
                entity.HasKey(e => e.NombreIdentificador);

                entity.ToTable("SPL_USUARIOS");

                entity.Property(e => e.NombreIdentificador)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_IDENTIFICADOR");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
            });

            modelBuilder.Entity<SplValidationTestsIsz>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SPL_VALIDATION_TESTS_ISZ");

                entity.Property(e => e.ArregloDev)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ARREGLO_DEV");

                entity.Property(e => e.Maximo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MAXIMO");

                entity.Property(e => e.MedicionDev)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MEDICION_DEV");

                entity.Property(e => e.Minimo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MINIMO");

                entity.Property(e => e.Promedio)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PROMEDIO");

                entity.Property(e => e.TipoUnidad)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");
            });

            modelBuilder.Entity<SplValorNominal>(entity =>
            {
                entity.HasKey(e => new { e.Unidad, e.OperationId, e.Orden })
                    .HasName("SPL_VALOR_NOMINAL_PK");

                entity.ToTable("SPL_VALOR_NOMINAL");

                entity.HasIndex(e => new { e.Unidad, e.OperationId, e.Orden }, "SPL_VALOR_NOMINAL_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Unidad)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("UNIDAD");

                entity.Property(e => e.OperationId)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("OPERATION_ID");

                entity.Property(e => e.Orden)
                    .HasColumnType("numeric(5, 0)")
                    .HasColumnName("ORDEN");

                entity.Property(e => e.ComboNumeric)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("COMBO_NUMERIC");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

                entity.Property(e => e.Fechacreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHACREACION");

                entity.Property(e => e.Fechamodificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FECHAMODIFICACION");

                entity.Property(e => e.Hvvolts)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("HVVOLTS");

                entity.Property(e => e.Lvvolts)
                    .HasColumnType("numeric(11, 3)")
                    .HasColumnName("LVVOLTS");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");
            });

            modelBuilder.Entity<SysModulo>(entity =>
            {
                entity.ToTable("SYS_MODULOS");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<TipoArchivo>(entity =>
            {
                entity.ToTable("TIPO_ARCHIVO");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion)
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
