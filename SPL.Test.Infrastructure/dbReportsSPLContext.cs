using System;

using System.IO;

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

        public virtual DbSet<SplCatsidco> SplCatsidcos { get; set; }
        public virtual DbSet<SplCatsidcoOther> SplCatsidcoOthers { get; set; }
        public virtual DbSet<SplConfiguracionReporte> SplConfiguracionReportes { get; set; }
        public virtual DbSet<SplConfiguracionReporteOrd> SplConfiguracionReporteOrds { get; set; }
        public virtual DbSet<SplDescFactorcorreccion> SplDescFactorcorreccions { get; set; }
        public virtual DbSet<SplDesplazamientoAngular> SplDesplazamientoAngulars { get; set; }
        public virtual DbSet<SplEspecificacion> SplEspecificacions { get; set; }
        public virtual DbSet<SplFactorCorreccion> SplFactorCorreccions { get; set; }
        public virtual DbSet<SplFiltrosreporte> SplFiltrosreportes { get; set; }
        public virtual DbSet<SplFrecuencia> SplFrecuencias { get; set; }
        public virtual DbSet<SplIdioma> SplIdiomas { get; set; }
        public virtual DbSet<SplInfoDetalleRad> SplInfoDetalleRads { get; set; }
        public virtual DbSet<SplInfoDetalleRdt> SplInfoDetalleRdts { get; set; }
        public virtual DbSet<SplInfoGeneralRad> SplInfoGeneralRads { get; set; }
        public virtual DbSet<SplInfoGeneralRdt> SplInfoGeneralRdts { get; set; }
        public virtual DbSet<SplInfoSeccionRad> SplInfoSeccionRads { get; set; }
        public virtual DbSet<SplInfoaparatoApr> SplInfoaparatoAprs { get; set; }
        public virtual DbSet<SplInfoaparatoBoq> SplInfoaparatoBoqs { get; set; }
        public virtual DbSet<SplInfoaparatoCam> SplInfoaparatoCams { get; set; }
        public virtual DbSet<SplInfoaparatoCap> SplInfoaparatoCaps { get; set; }
        public virtual DbSet<SplInfoaparatoCar> SplInfoaparatoCars { get; set; }
        public virtual DbSet<SplInfoaparatoDg> SplInfoaparatoDgs { get; set; }
        public virtual DbSet<SplInfoaparatoGar> SplInfoaparatoGars { get; set; }
        public virtual DbSet<SplInfoaparatoLab> SplInfoaparatoLabs { get; set; }
        public virtual DbSet<SplInfoaparatoNor> SplInfoaparatoNors { get; set; }
        public virtual DbSet<SplInfoaparatoTap> SplInfoaparatoTaps { get; set; }
        public virtual DbSet<SplNorma> SplNormas { get; set; }
        public virtual DbSet<SplNormasrep> SplNormasreps { get; set; }
        public virtual DbSet<SplPlantillaBase> SplPlantillaBases { get; set; }
        public virtual DbSet<SplPrueba> SplPruebas { get; set; }
        public virtual DbSet<SplRepConsolidado> SplRepConsolidados { get; set; }
        public virtual DbSet<SplReporte> SplReportes { get; set; }
        public virtual DbSet<SplSerieParalelo> SplSerieParalelos { get; set; }
        public virtual DbSet<SplTensionPlaca> SplTensionPlacas { get; set; }
        public virtual DbSet<SplTercerDevanadoTipo> SplTercerDevanadoTipos { get; set; }
        public virtual DbSet<SplTipoUnidad> SplTipoUnidads { get; set; }
        public virtual DbSet<SplTitSerieparalelo> SplTitSerieparalelos { get; set; }
        public virtual DbSet<SplTituloColumnasRad> SplTituloColumnasRads { get; set; }
        public virtual DbSet<SplTituloColumnasRdt> SplTituloColumnasRdts { get; set; }
        public virtual DbSet<SplValorNominal> SplValorNominals { get; set; }

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
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlConnectionDEV"));
                #endif

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
            });

            modelBuilder.Entity<SplConfiguracionReporte>(entity =>
            {
                entity.HasKey(e => new { e.TipoReporte, e.ClavePrueba, e.Apartado, e.Seccion, e.Dato })
                    .HasName("SPL_CR_PK");

                entity.ToTable("SPL_CONFIGURACION_REPORTE");

                entity.HasIndex(e => new { e.TipoReporte, e.ClavePrueba, e.Apartado, e.Seccion, e.Dato }, "SPL_CR_PK_UNIQUE")
                    .IsUnique();

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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
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

                entity.HasIndex(e => e.Dato, "SPL_CRO_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Dato)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("DATO");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

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

                entity.Property(e => e.Orden)
                    .HasColumnType("numeric(2, 0)")
                    .HasColumnName("ORDEN");
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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

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

                entity.Property(e => e.Frecuencia)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("FRECUENCIA");

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(32)
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

            modelBuilder.Entity<SplInfoDetalleRad>(entity =>
            {
                entity.HasKey(e => new { e.IdCarga, e.Seccion, e.Tiempo, e.PosicionColumna });

                entity.ToTable("SPL_INFO_DETALLE_RAD");

                entity.HasIndex(e => new { e.IdCarga, e.Seccion, e.Tiempo, e.PosicionColumna }, "PK_SPL_INFO_DETALLE_RAD_UNIQUE")
                    .IsUnique();

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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

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

                entity.Property(e => e.ValorColumna)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("VALOR_COLUMNA");
            });

            modelBuilder.Entity<SplInfoDetalleRdt>(entity =>
            {
                entity.HasKey(e => new { e.IdCarga, e.Posicion, e.PosicionColumna });

                entity.ToTable("SPL_INFO_DETALLE_RDT");

                entity.HasIndex(e => new { e.IdCarga, e.Posicion, e.PosicionColumna }, "PK_SPL_INFO_DETALLE_RDT_UNIQUE")
                    .IsUnique();

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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
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

            modelBuilder.Entity<SplInfoGeneralRad>(entity =>
            {
                entity.HasKey(e => e.IdCarga)
                    .HasName("SPL_ID_RAD_PK");

                entity.ToTable("SPL_INFO_GENERAL_RAD");

                entity.HasIndex(e => e.IdCarga, "SPL_ID_RAD_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdCarga)
                    .HasColumnType("numeric(7, 0)")
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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
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

            modelBuilder.Entity<SplInfoGeneralRdt>(entity =>
            {
                entity.HasKey(e => e.IdCarga)
                    .HasName("SPL_IG_RDT_PK");

                entity.ToTable("SPL_INFO_GENERAL_RDT");

                entity.HasIndex(e => e.IdCarga, "SPL_IG_RDT_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdCarga)
                    .HasColumnType("numeric(7, 0)")
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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
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
                    .HasMaxLength(5)
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

            modelBuilder.Entity<SplInfoSeccionRad>(entity =>
            {
                entity.HasKey(e => new { e.IdCarga, e.Seccion })
                    .HasName("SPL_IS_RAD_PK");

                entity.ToTable("SPL_INFO_SECCION_RAD");

                entity.HasIndex(e => new { e.IdCarga, e.Seccion }, "SPL_IS_RAD_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdCarga)
                    .HasColumnType("numeric(7, 0)")
                    .HasColumnName("ID_CARGA");

                entity.Property(e => e.Seccion)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("SECCION");

                entity.Property(e => e.Creadopor)
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Temperatura)
                    .HasColumnType("numeric(10, 4)")
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

            modelBuilder.Entity<SplInfoaparatoApr>(entity =>
            {
                entity.HasKey(e => new { e.OrderCode, e.ColumnTypeId });

                entity.ToTable("SPL_INFOAPARATO_APR");

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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
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

            modelBuilder.Entity<SplInfoaparatoCam>(entity =>
            {
                entity.HasKey(e => new { e.OrderCode, e.ColumnTypeId });

                entity.ToTable("SPL_INFOAPARATO_CAM");

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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

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
                    .HasMaxLength(32)
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

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(32)
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

                entity.Property(e => e.TipoUnidad)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_UNIDAD");

                entity.Property(e => e.Typetrafoid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("TYPETRAFOID");
            });

            modelBuilder.Entity<SplInfoaparatoGar>(entity =>
            {
                entity.HasKey(e => e.OrderCode);

                entity.ToTable("SPL_INFOAPARATO_GAR");

                entity.HasIndex(e => e.OrderCode, "PK_SPL_INFOAPARATO_GAR_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
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

                entity.HasIndex(e => e.OrderCode, "PK_SPL_INFOAPARATO_LAB_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.OrderCode)
                    .HasMaxLength(47)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_CODE");

                entity.Property(e => e.Creadopor)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
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

            modelBuilder.Entity<SplPlantillaBase>(entity =>
            {
                entity.HasKey(e => new { e.TipoReporte, e.ClavePrueba, e.ClaveIdioma, e.ColumnasConfigurables })
                    .HasName("SPL_PB_PK");

                entity.ToTable("SPL_PLANTILLA_BASE");

                entity.HasIndex(e => new { e.TipoReporte, e.ClavePrueba, e.ClaveIdioma, e.ColumnasConfigurables }, "SPL_PB_PK_UNIQUE")
                    .IsUnique();

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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

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

                entity.Property(e => e.Plantilla).HasColumnName("PLANTILLA");
            });

            modelBuilder.Entity<SplPrueba>(entity =>
            {
                entity.HasKey(e => new { e.TipoReporte, e.ClavePrueba })
                    .HasName("SPL_P_PK");

                entity.ToTable("SPL_PRUEBAS");

                entity.HasIndex(e => new { e.TipoReporte, e.ClavePrueba }, "SPL_P_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

                entity.Property(e => e.ClavePrueba)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE_PRUEBA");

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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

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

                entity.HasIndex(e => e.TipoReporte, "SPL_R_PK_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.TipoReporte)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_REPORTE");

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

                entity.Property(e => e.Modificadopor)
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");
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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREADOPOR");

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
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TIT_POS_UNICA2");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");
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
                    .HasMaxLength(32)
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
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICADOPOR");

                entity.Property(e => e.Posicion)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("POSICION");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
