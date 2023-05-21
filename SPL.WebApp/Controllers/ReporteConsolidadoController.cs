//extern alias FIXEDDOCUMENTS;

namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    //using FIXEDDOCUMENTS::Telerik.Windows.Documents.UI;
    using AutoMapper;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Identity.Web;

    using Newtonsoft.Json;

    using Serilog;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.Helpers;
    using SPL.WebApp.ViewModels;

    using Telerik.Documents.Common.Model;
    using Telerik.Documents.Core.Fonts;
    using Telerik.Windows.Documents.Fixed.FormatProviders;
    using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
    using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
    using Telerik.Windows.Documents.Fixed.Model;
    using Telerik.Windows.Documents.Fixed.Model.Collections;
    using Telerik.Windows.Documents.Fixed.Utilities.Rendering;
    using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
    using Telerik.Windows.Documents.Flow.Model;
    using Telerik.Windows.Documents.Flow.Model.Fields;
    using Telerik.Windows.Documents.Flow.Model.Styles;

    using Winnovative.PdfToImage;

    public class ReporteConsolidadoController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly INraClientService _nraClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly INraService _nraService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfigurationClientService _configurationClientService;
        private readonly ILogger _logger;
        private readonly IArtifactRecordService _artifactRecordService;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IMapper _mapper;
        private readonly IProfileSecurityService _profileClientService;
        public ReporteConsolidadoController(
         IMasterHttpClientService masterHttpClientService,
         INraClientService nraClientService,
         IReportClientService reportClientService,
         IArtifactClientService artifactClientService,
         ITestClientService testClientService,
         IGatewayClientService gatewayClientService,
         INraService nraService,
         IWebHostEnvironment hostEnvironment,
         IConfigurationClientService configurationClientService,
         IArtifactRecordService artifactRecordService,
         IMapper mapper,
         ISidcoClientService sidcoClientService,
         ILogger logger,
            IProfileSecurityService profileClientService
         )
        {
            _masterHttpClientService = masterHttpClientService;
            _nraClientService = nraClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _artifactRecordService = artifactRecordService;
            _gatewayClientService = gatewayClientService;
            _nraService = nraService;
            _hostEnvironment = hostEnvironment;
            _configurationClientService = configurationClientService;
            _sidcoClientService = sidcoClientService;
            _logger = logger;
            _mapper = mapper;
            _profileClientService = profileClientService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.ReporteConsolidado)))
                {

                    // await this.GetReports(new ReporteConsolidadoViewModel());
                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new ReporteConsolidadoViewModel { NoSerie = noSerie });
                }
                else
                {
                    return View("~/Views/PageConstruction/PermissionDenied.cshtml");

                }

            }
            catch (MicrosoftIdentityWebChallengeUserException e)
            {
                return View("~/Views/PageConstruction/Error.cshtml");


            }
            catch (Exception ex)
            {

                return View("~/Views/PageConstruction/Error.cshtml");

            }
        
        }

        public async Task PrepareForm()
        {
            //ApiResponse<ReportPDFDto> result = await this._reportClientService.GetPDFReport(12,"FPB");
            //var a = this.Convertir(result.Structure.File, 59);7

            //var a = await this._gatewayClientService.GetPositions("G4135-01");
            // var a = await this._gatewayClientService.("G4135-01");

            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            // Tipos de prueba7

            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.NRA.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");

            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetRulesEquivalents, (int)MicroservicesEnum.splmasters));

            Thread.Sleep(5000);

        }

        public async Task<IActionResult> CheckInfo(string noSerie, string idioma)
        {
            ReporteConsolidadoViewModel model = new();
            ApiResponse<List<ConsolidatedReportDTO>> apiResponse = await _reportClientService.GetConsolidatedReport(noSerie, idioma);

            if (apiResponse.Code == -1)
            {
                return Json(new ApiResponse<ReporteConsolidadoViewModel>
                {
                    Structure = null,
                    Code = -1,
                    Description = apiResponse.Description
                });
            }

            if (apiResponse.Structure.Count() == 0)
            {
                return Json(new ApiResponse<ReporteConsolidadoViewModel>
                {
                    Structure = null,
                    Code = -1,
                    Description = "No se encontraron resultados"
                });
            }

            model.datasource = new List<DataSourceTreeView>();
            var data = apiResponse.Structure.GroupBy(x => new { x.TIPO_REPORTE, x.NOMBRE_REPORTE }).Select(c => new { c.Key.TIPO_REPORTE, c.Key.NOMBRE_REPORTE }).ToList();

            foreach (var item in data)
            {

                DataSourceTreeView lista = new()
                {
                    isParent = true,
                    expanded = false,
                    rowLevel = 1,
                    ID_REP = 0,
                    NOMBRE_REPORTE = item.NOMBRE_REPORTE,
                    TIPO_REPORTE = item.TIPO_REPORTE,
                    items = new List<DataSourceTreeView2>()

                };

                foreach (ConsolidatedReportDTO item2 in apiResponse.Structure.Where(x => x.TIPO_REPORTE == item.TIPO_REPORTE))
                {
                    lista.items.Add(new DataSourceTreeView2
                    {
                        isParent = false,
                        expanded = false,
                        rowLevel = 2,
                        ID_REP = item2.ID_REP,
                        TIPO_REPORTE = item2.TIPO_REPORTE,
                        NOMBRE_REPORTE = item2.NOMBRE_REPORTE,
                        FILTROS = string.IsNullOrEmpty(item2.FILTROS) ? string.Empty : item2.FILTROS,
                        COMENTARIOS = string.IsNullOrEmpty(item2.COMENTARIOS) ? string.Empty : item2.COMENTARIOS,
                        ID_PRUEBA = item2.ID_PRUEBA,
                        PRUEBA = item2.PRUEBA,
                        DESCRIPCION_EN = item2.DESCRIPCION_EN,
                        AGRUPACION_EN = item2.AGRUPACION_EN,
                        AGRUPACION = item2.AGRUPACION,
                        FECHA = item2.FECHA,
                        isChecked = ""
                    });
                }
                lista.items = lista.items.OrderByDescending(x => x.ID_REP).ToList();
                long id = lista.items.OrderByDescending(x => x.FECHA).FirstOrDefault().ID_REP;

                lista.items.Where(x => x.ID_REP == id).FirstOrDefault().isChecked = "checked";
                lista.items.Where(x => x.ID_REP == id).FirstOrDefault().isDefaultChecked = true;

                model.datasource.Add(lista);

            }

            return Ok(new ApiResponse<ReporteConsolidadoViewModel>
            {
                Structure = model,
                Code = 1,
                Description = "asdasd"
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetIdioma(string noSerie)
        {

            InformationArtifactDTO general = await _artifactClientService.GetArtifact(noSerie.Split('-')[0]);
            ReporteConsolidadoViewModel model = new()
            {
                ClaveIdioma = !string.IsNullOrEmpty(general?.GeneralArtifact?.ClaveIdioma) ? general?.GeneralArtifact?.ClaveIdioma : ""
            };

            return Ok(new ApiResponse<ReporteConsolidadoViewModel>
            {
                Structure = model,
                Code = 1,
                Description = "asdasd"
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetReports([FromBody] ReporteConsolidadoViewModel viewModel)
        {
            try
            {
                List<DataSourceTreeView2> reportes = viewModel.datasource.SelectMany(x => x.items).Where(y => y.isChecked == "checked").ToList();

                InformationArtifactDTO general = await _artifactClientService.GetArtifact(viewModel.NoSerie.Split('-')[0]);

                if (!reportes.Any())
                {
                    return Json(new
                    {
                        response = new ApiResponse<ReporteConsolidadoViewModel>
                        {
                            Code = -1,
                            Structure = null,
                            Description = " No se han seleccionado reportes para imprimir"
                        }
                    });
                }

                if (general.GeneralArtifact == null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<ReporteConsolidadoViewModel>
                        {
                            Code = -1,
                            Structure = null,
                            Description = "No se ha encontrado información general para el apto. " + viewModel.NoSerie
                        }
                    });
                }

                viewModel.ReportesConsolidados = new List<ReporteConsolidadoModel>();
                foreach (DataSourceTreeView2 reporte in reportes)
                {
                    ApiResponse<ReportPDFDto> result = await _reportClientService.GetPDFReport(reporte.ID_REP, reporte.TIPO_REPORTE);

                    if (result.Code.Equals(-1))
                    {
                        return Json(new
                        {
                            response = result
                        });
                    }
                    else
                    {
                        viewModel.ReportesConsolidados.Add(new ReporteConsolidadoModel
                        {
                            Reporte = result.Structure.File,
                            NombreReporte = result.Structure.ReportName,
                            DESCRIPCION = reporte.NOMBRE_REPORTE,
                            DESCRIPCION_EN = reporte.DESCRIPCION_EN,
                            AGRUPACION_EN = reporte.AGRUPACION_EN,
                            AGRUPACION = reporte.AGRUPACION

                        });
                    }
                }

                IEnumerable<CatSidcoOthersDTO> catSidcoOthers = await _masterHttpClientService.GetCatSidcoOthers();
                _artifactRecordService.FixConnections(catSidcoOthers, ref general);

                ArtifactRecordViewModel viewModel2 = _mapper.Map<ArtifactRecordViewModel>(general);
                ApiResponse<BaseTemplateConsolidatedReportDTO> reportTemplete = await _artifactClientService.GetBaseTemplateConsolidatedReport(viewModel.ClaveIdioma);
                byte[] data = null;
                if (reportTemplete.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<ReporteConsolidadoViewModel>
                        {
                            Structure = null,
                            Code = -1,
                            Description = reportTemplete.Description
                        }
                    });

                }
                else
                {
                    if (string.IsNullOrEmpty(reportTemplete.Structure.Plantilla))
                    {
                        return Json(new
                        {
                            response = new ApiResponse<ReporteConsolidadoViewModel>
                            {
                                Structure = null,
                                Code = -1,
                                Description = "No hay plantilla configurada para reporte consolidado"
                            }
                        });
                    }
                    data = Convert.FromBase64String(reportTemplete.Structure.Plantilla);
                }

                IEnumerable<GeneralPropertiesDTO> typeTransformers = await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetTypeTransformers, (int)MicroservicesEnum.splsidco);
                GeneralPropertiesDTO current = typeTransformers.FirstOrDefault(x => x.Id == general.GeneralArtifact.TypeTrafoId);

                ApiResponse<List<TypeConsolidatedReportDTO>> tyInformationReport = await _reportClientService.GetTypeSectionConsolidatedReport(viewModel.NoSerie.Split('-')[0], viewModel.ClaveIdioma);

                _logger.Warning("GENERAL ARTIFAC -> " + JsonConvert.SerializeObject(general));
                using Stream input = new MemoryStream(data);
                DocxFormatProvider docxFormatProvider = new();
                RadFlowDocument document = docxFormatProvider.Import(input);

                Telerik.Windows.Documents.Flow.Model.Editing.RadFlowDocumentEditor editor = new(document); // document is an instance of the RadFlowDocument class 

                List<decimal?> listaCapacidades = general.CharacteristicsArtifact.Select(x => x.Mvaf1).ToList();
                string tensiones = general.VoltageKV.TensionKvAltaTension1 + "-" + general.VoltageKV.TensionKvBajaTension1 + "-" + general.VoltageKV.TensionKvSegundaBaja1 + "-" + general.VoltageKV.TensionKvTerciario1;
                List<string> enfriamientos = general.CharacteristicsArtifact.Select(x => x.CoolingType).ToList();

                editor.ReplaceText("#vCapacidadMVA", string.Join('/', listaCapacidades));
                editor.ReplaceText("#vTensionKV", tensiones);
                editor.ReplaceText("#vTipoEnfriamiento", string.Join('/', enfriamientos));
                editor.ReplaceText("#vNoSerie", viewModel.NoSerie);
                editor.ReplaceText("#vNombreCliente", general.GeneralArtifact.CustomerName);
                editor.ReplaceText("#vFecha2", DateTime.Now.ToString("MM-dd-yyyy"));
                editor.ReplaceText("#vFechaReporte", DateTime.Now.ToString("MMM/dd/yyyy"));

                editor.ReplaceText("#vDescripcion", general.GeneralArtifact.Descripcion?.ToString());
                editor.ReplaceText("#vFases", general.GeneralArtifact.Phases?.ToString());
                editor.ReplaceText("#vFrecuencia", general.GeneralArtifact.Frecuency?.ToString());
                editor.ReplaceText("#vAltitudDesc", general.GeneralArtifact.AltitudeF2?.ToString());
                editor.ReplaceText("#vAltitud", general.GeneralArtifact.AltitudeF1?.ToString());
                editor.ReplaceText("#vApplicationDesc", current != null && !string.IsNullOrEmpty(current.Description) ? current.Description : "");

                int lastIndex = general.CharacteristicsArtifact.Count() + 1;
                foreach (var item in general.CharacteristicsArtifact.Select((value, i) => new { i, value }))
                {
                    editor.ReplaceText("#vCooling_" + (item.i + 1), string.IsNullOrEmpty(item.value.CoolingType?.ToString()) ? "" : item.value.CoolingType?.ToString());
                    editor.ReplaceText("#vMVAF1_" + (item.i + 1), string.IsNullOrEmpty(item.value.Mvaf1?.ToString()) ? "" : item.value.Mvaf1?.ToString());
                    editor.ReplaceText("#vElevation_" + (item.i + 1), string.IsNullOrEmpty(item.value.OverElevation?.ToString()) ? "" : item.value.OverElevation?.ToString());
                }

                for (int i = lastIndex; i <= 4; i++)
                {
                    editor.ReplaceText("#vCooling_" + i, "");
                    editor.ReplaceText("#vMVAF1_" + i, "");
                    editor.ReplaceText("#vElevation_" + i + " °C", "");
                }

                #region LLENADO DE INFORMACIÓN de primer HEADEr
                bool at = false;
                bool bt = false;
                bool ter = false;
                IEnumerable<Run> a = document.EnumerateChildrenOfType<Run>();

                foreach (ChangingTablesArtifactDTO item in general.ChangingTablesArtifact)
                {
                    if (item.ColumnTitle.ToLower() is "dev serie" or "alta tensión")
                    {
                        editor.ReplaceText("HIGH_TC (H)", "HIGH (H)");
                        editor.ReplaceText("#vTipoC1", "AT");
                        at = true;
                    }

                    if (item.ColumnTitle.ToLower() is "dev común" or "baja tensión")
                    {
                        editor.ReplaceText("LOW_TC (X)", "LOW (X)");
                        editor.ReplaceText("#vTipoC2", "BT");
                        bt = true;
                    }

                    if (item.ColumnTitle.ToLower() is "terciario" or "terc")
                    {
                        editor.ReplaceText("TERCIARIO_TC", "TERCIARIO");
                        editor.ReplaceText("#vTipoC3", "NO DEF.");
                        ter = true;
                    }
                }

                if (!bt)
                {
                    if (viewModel.ClaveIdioma == "ES")
                    {
                        editor.ReplaceText("BAJA_TC (X)", "");
                    }
                    else
                    {
                        editor.ReplaceText("LOW_TC (X)", "");

                    }

                    editor.ReplaceText("#vTipoC2", "");
                    editor.ReplaceText("#vPos2", "");
                    editor.ReplaceText("#vPasos22", "");
                    editor.ReplaceText("#vPasos2", "");
                    editor.ReplaceText("#vNominal2", "");
                }

                if (!at)
                {
                    if (viewModel.ClaveIdioma == "ES")
                    {
                        editor.ReplaceText("ALTA_TC (H)", "");
                    }
                    else
                    {
                        editor.ReplaceText("HIGH_TC (H)", "");

                    }

                    editor.ReplaceText("#vTipoC1", "");
                    editor.ReplaceText("#vPos1", "");
                    editor.ReplaceText("#vPasos12", "");
                    editor.ReplaceText("#vNominal1", "");
                    editor.ReplaceText("#vPasos1", "");

                }

                if (!ter)
                {
                    editor.ReplaceText("TERCIARIO_TC", "");
                    editor.ReplaceText("#vTipoC3", "");
                    editor.ReplaceText("#vPos3", "");
                    editor.ReplaceText("#vPasos32", "");
                    editor.ReplaceText("#vNominal3", "");
                    editor.ReplaceText("#vPasos3", "");
                }

                #endregion

                if (viewModel2.ComboNumericSc is "2" or "4")//es BT en cambiadores
                {
                    editor.ReplaceText("#vPos2", (int.Parse(viewModel2.TapsBajaTension) + 1).ToString());
                    editor.ReplaceText("#vNominal2", viewModel2.NominalSc);
                    if (viewModel2.PorcentajeSupSc == viewModel2.PorcentajeInfSc)
                    {
                        editor.ReplaceText("#vPasos22", viewModel2.PorcentajeSupSc);
                    }
                    else
                    {
                        editor.ReplaceText("#vPasos22", "+" + viewModel2.PorcentajeSupSc + "-" + viewModel2.PorcentajeInfSc);
                    }

                    editor.ReplaceText("#vPasos2", "+" + viewModel2.CantidadSupSc + "-" + viewModel2.CantidadInfSc);
                }

                if (viewModel2.ComboNumericSc is "3" or "5")//es AT en cambiadores
                {
                    editor.ReplaceText("#vPos1", (int.Parse(viewModel2.TapsAltaTension) + 1).ToString());
                    editor.ReplaceText("#vNominal1", viewModel2.NominalSc);
                    if (viewModel2.PorcentajeSupSc == viewModel2.PorcentajeInfSc)
                    {
                        editor.ReplaceText("#vPasos12", viewModel2.PorcentajeSupSc);
                    }
                    else
                    {
                        editor.ReplaceText("#vPasos12", "+" + viewModel2.PorcentajeSupSc + "-" + viewModel2.PorcentajeInfSc);
                    }

                    editor.ReplaceText("#vPasos1", "+" + viewModel2.CantidadSupSc + "-" + viewModel2.CantidadInfSc);
                }

                if (viewModel2.ComboNumericSc == "1")//es terciario en cambiadores
                {
                    editor.ReplaceText("#vPos3", (int.Parse(viewModel2.TapsTerciario) + 1).ToString());
                    editor.ReplaceText("#vNominal3", viewModel2.NominalSc);

                    if (viewModel2.PorcentajeSupSc == viewModel2.PorcentajeInfSc)
                    {
                        editor.ReplaceText("#vPasos32", viewModel2.PorcentajeSupSc);
                    }
                    else
                    {
                        editor.ReplaceText("#vPasos32", "+" + viewModel2.PorcentajeSupSc + "-" + viewModel2.PorcentajeInfSc);
                    }

                    editor.ReplaceText("#vPasos3", "+" + viewModel2.CantidadSupSc + "-" + viewModel2.CantidadInfSc);
                }

                if (viewModel2.ComboNumericBc is "2" or "4")//es BT en cambiadores
                {
                    editor.ReplaceText("#vPos2", (int.Parse(viewModel2.TapsBajaTension) + 1).ToString());
                    editor.ReplaceText("#vNominal2", viewModel2.NominalBc);

                    if (viewModel2.PorcentajeSupBc == viewModel2.PorcentajeInfBc)
                    {
                        editor.ReplaceText("#vPasos22", viewModel2.PorcentajeSupBc);
                    }
                    else
                    {
                        editor.ReplaceText("#vPasos22", "+" + viewModel2.PorcentajeSupBc + "-" + viewModel2.PorcentajeInfBc);
                    }

                    editor.ReplaceText("#vPasos2", "+" + viewModel2.CantidadSupBc + "-" + viewModel2.CantidadInfBc);
                }

                if (viewModel2.ComboNumericBc is "3" or "5")//es AT en cambiadores
                {
                    editor.ReplaceText("#vPos1", (int.Parse(viewModel2.TapsAltaTension) + 1).ToString());
                    editor.ReplaceText("#vNominal1", viewModel2.NominalBc);

                    if (viewModel2.PorcentajeSupBc == viewModel2.PorcentajeInfBc)
                    {
                        editor.ReplaceText("#vPasos12", viewModel2.PorcentajeSupBc);
                    }
                    else
                    {
                        editor.ReplaceText("#vPasos12", "+" + viewModel2.PorcentajeSupBc + "-" + viewModel2.PorcentajeInfBc);
                    }

                    editor.ReplaceText("#vPasos1", "+" + viewModel2.CantidadSupBc + "-" + viewModel2.CantidadInfBc);
                }

                if (viewModel2.ComboNumericBc == "1")//es terciario en cambiadores
                {
                    editor.ReplaceText("#vPos3", (int.Parse(viewModel2.TapsTerciario) + 1).ToString());
                    editor.ReplaceText("#vNominal3", viewModel2.NominalBc);

                    if (viewModel2.PorcentajeSupBc == viewModel2.PorcentajeInfBc)
                    {
                        editor.ReplaceText("#vPasos32", viewModel2.PorcentajeSupBc);
                    }
                    else
                    {
                        editor.ReplaceText("#vPasos32", "+" + viewModel2.PorcentajeSupBc + "-" + viewModel2.PorcentajeInfBc);
                    }

                    editor.ReplaceText("#vPasos3", "+" + viewModel2.CantidadSupBc + "-" + viewModel2.CantidadInfBc);
                }

                #region LLENADO DE INFORMACIÓN EN EL SEGUNDO APARTAD 
                if (general.VoltageKV.TensionKvAltaTension1 is not null and not 0)
                {
                    editor.ReplaceText("#vConection1", general.Derivations.ConexionEquivalente);
                    editor.ReplaceText("#vNBI1", general.NBAI.NbaiAltaTension != null ? general.NBAI.NbaiAltaTension?.ToString() : "");
                    editor.ReplaceText("#vNBIN1", general.NBAINeutro.ValorNbaiNeutroAltaTension != null ? general.NBAINeutro.ValorNbaiNeutroAltaTension?.ToString() : 0.ToString());
                    editor.ReplaceText("#vTension1", general.VoltageKV.TensionKvAltaTension1.ToString());

                }
                else
                {
                    editor.ReplaceText("#vConection1", "");
                    editor.ReplaceText("#vNBI1", "");
                    editor.ReplaceText("#vNBIN1", "");
                    editor.ReplaceText("#vTension1", "");
                    if (viewModel.ClaveIdioma == "EN")
                    {
                        editor.ReplaceText("ALTA (H)", "");
                    }
                    else
                    {
                        editor.ReplaceText("HIGH (H)", "");

                    }
                }

                if (general.VoltageKV.TensionKvBajaTension1 is not null and not 0)
                {
                    editor.ReplaceText("#vConection2", general.Derivations.ConexionEquivalente_2);
                    editor.ReplaceText("#vNBI2", general.NBAI.NbaiBajaTension != null ? general.NBAI.NbaiBajaTension?.ToString() : "");
                    editor.ReplaceText("#vNBIN2", general.NBAINeutro.ValorNbaiNeutroBajaTension != null ? general.NBAINeutro.ValorNbaiNeutroBajaTension?.ToString() : 0.ToString());
                    editor.ReplaceText("#vTension2", general.VoltageKV.TensionKvBajaTension1.ToString());

                }
                else
                {
                    editor.ReplaceText("#vConection2", "");
                    editor.ReplaceText("#vNBI2", "");
                    editor.ReplaceText("#vNBIN2", "");
                    editor.ReplaceText("#vTension2", "");

                    if (viewModel.ClaveIdioma == "EN")
                    {
                        editor.ReplaceText("BAJA (X)", "");
                    }
                    else
                    {
                        editor.ReplaceText("LOW (X)", "");

                    }
                }

                if (general.VoltageKV.TensionKvTerciario1 is not null and not 0)
                {
                    editor.ReplaceText("#vConection3", general.Derivations.ConexionEquivalente_4);
                    editor.ReplaceText("#vNBI3", general.NBAI.NabaiTercera != null ? general.NBAI.NabaiTercera?.ToString() : "");
                    editor.ReplaceText("#vNBIN3", general.NBAINeutro.ValorNbaiNeutroTercera != null ? general.NBAINeutro.ValorNbaiNeutroTercera?.ToString() : 0.ToString());
                    editor.ReplaceText("#vTension3", general.VoltageKV.TensionKvTerciario1.ToString());

                }
                else
                {
                    editor.ReplaceText("#vConection3", "");
                    editor.ReplaceText("#vNBI3", "");
                    editor.ReplaceText("#vNBIN3", "");
                    editor.ReplaceText("#vTension3", "");
                    editor.ReplaceText("TERCIARIO", "");
                }

                #endregion

                if (general.TapBaan != null)
                {
                }
                List<string> rules = viewModel2.RulesArtifacts.Select(x => x.Descripcion).ToList();
                string str = string.Join("\n\n", rules);
                string str2 = string.Join("", rules);
                //editor.ReplaceText("#vNormas",str );

                _logger.Warning("normas con salto de linea " + str);
                _logger.Warning("normas sin salto de linea " + str2);

                //editor.ReplaceText("GENERAL CHARACTERISTICS", "");
                //ReemplazarInformacionGeneral();
                IEnumerable<Section> tg = document.EnumerateChildrenOfType<Section>();
                IEnumerable<Run> spanElements = document.EnumerateChildrenOfType<Run>();

                Run aaa = spanElements.FirstOrDefault(x => x.Text == "#vNormas");
                editor.ReplaceText("#vNormas", rules[0]);
                rules.RemoveAt(0);
                editor.MoveToInlineEnd(aaa.Paragraph.Inlines[0]);

                for (int i = 0; i < rules.Count; i++)
                {
                    _ = editor.InsertText("\n#vNormas" + (i + 1));
                    aaa = spanElements.FirstOrDefault(x => x.Text == "#vNormas" + (i + 1));
                    editor.ReplaceText("#vNormas" + (i + 1), rules[i]);

                }

                List<string> ee = spanElements.Select(o => o.Text).ToList();
                Run element = spanElements.Where(x => x.Text.ToLower() == (viewModel.ClaveIdioma == "EN" ? "index" : "contenido")).FirstOrDefault(); // Search for the word Index in the first section
                Run element2 = spanElements.Where(x => x.Text.ToLower() == (viewModel.ClaveIdioma == "EN" ? "general characteristics" : "características generales")).FirstOrDefault(); // Search for the word Index in the first section
                Paragraph parent = element.Parent as Paragraph; //cast the element to Paragraph

                Paragraph parent2 = element2.Parent as Paragraph; //cast the element to Paragraph
                parent2.OutlineLevel = OutlineLevel.Level1;
                element2.Paragraph.OutlineLevel = OutlineLevel.Level1;
                element2.FontSize = 21;
                element2.FontWeight = FontWeights.Bold;
                List<string> encabezadosPrincipales = viewModel.ClaveIdioma == "EN" ?
                    viewModel.ReportesConsolidados.Select(y => y.AGRUPACION_EN).Distinct().ToList() :
                    viewModel.ReportesConsolidados.Select(y => y.AGRUPACION).Distinct().ToList();

                bool esprimero = true;
                foreach (string item in encabezadosPrincipales.OrderBy(x => x).Distinct())
                {
                    Section currentseccion = null;
                    currentseccion = editor.InsertSection();
                    currentseccion.SectionType = SectionType.NextPage;
                    currentseccion.PageSize = new Telerik.Documents.Primitives.Size(816, 1056);
                    Run run = new(document)
                    {
                        Text = item,
                        FontWeight = FontWeights.Bold,
                        FontFamily = new ThemableFontFamily("Arial"),
                        FontSize = 21
                    };

                    Paragraph paragraph = currentseccion.Blocks.AddParagraph();
                    paragraph.TextAlignment = Alignment.Left;
                    paragraph.Inlines.Add(run);
                    paragraph.OutlineLevel = OutlineLevel.Level1;
                    //paragraph.ListLevel = this._ssa.Loca;
                    run.Paragraph.OutlineLevel = OutlineLevel.Level1;

                    List<string> agrupados = viewModel.ClaveIdioma == "EN" ?
                        viewModel.ReportesConsolidados.Where(x => x.AGRUPACION_EN == item).Select(y => y.DESCRIPCION_EN).Distinct().ToList() :
                        viewModel.ReportesConsolidados.Where(x => x.AGRUPACION == item).Select(y => y.DESCRIPCION).Distinct().ToList();

                    foreach (string report in agrupados)
                    {
                        List<ReporteConsolidadoModel> subreportes =
                            viewModel.ClaveIdioma == "EN" ? viewModel.ReportesConsolidados.Where(x => x.DESCRIPCION_EN == report).ToList() :
                             viewModel.ReportesConsolidados.Where(x => x.DESCRIPCION == report).ToList();

                        if (!esprimero)
                        {
                            currentseccion = editor.InsertSection();
                            currentseccion.SectionType = SectionType.NextPage;
                            Run run2 = new(document)
                            {
                                Text = report,
                                FontWeight = FontWeights.Bold,
                                FontFamily = new ThemableFontFamily("Arial"),
                                FontSize = 21
                            };

                            Paragraph paragraph2 = currentseccion.Blocks.AddParagraph();
                            paragraph2.TextAlignment = Alignment.Left;
                            paragraph2.Inlines.Add(run2);
                            paragraph2.OutlineLevel = OutlineLevel.Level2;
                            run2.Paragraph.OutlineLevel = OutlineLevel.Level2;
                            foreach (ReporteConsolidadoModel subreporte in subreportes)
                            {

                                string result = InsertarImagenes(subreporte, document, currentseccion, esprimero);

                                if (!string.IsNullOrEmpty(result))
                                {
                                    return Json(new
                                    {
                                        response = new ApiResponse<byte[]>
                                        {
                                            Structure = null,
                                            Code = -1,
                                            Description = result
                                        }
                                    });
                                }
                            }
                        }
                        else
                        {
                            Run run2 = new(document)
                            {
                                Text = report,
                                FontWeight = FontWeights.Bold,
                                FontFamily = new ThemableFontFamily("Arial"),
                                FontSize = 21
                            };

                            Paragraph paragraph2 = currentseccion.Blocks.AddParagraph();
                            paragraph2.TextAlignment = Alignment.Left;
                            paragraph2.Inlines.Add(run2);
                            paragraph2.OutlineLevel = OutlineLevel.Level2;
                            run2.Paragraph.OutlineLevel = OutlineLevel.Level2;

                            foreach (ReporteConsolidadoModel subreporte in subreportes)
                            {
                                string result = InsertarImagenes(subreporte, document, currentseccion, esprimero);

                                if (!string.IsNullOrEmpty(result))
                                {
                                    return Json(new
                                    {
                                        response = new ApiResponse<byte[]>
                                        {
                                            Structure = null,
                                            Code = -1,
                                            Description = result
                                        }
                                    });
                                }
                            }

                            esprimero = false;

                        }
                    }
                    esprimero = true;
                }

                editor.MoveToInlineEnd(element.Paragraph.Inlines[0]);
                FieldInfo tocField = editor.InsertField(@"TOC \o \" + @"1-3" + @" \h \z \u \n " + @"1-1", string.Empty);
                tocField.IsDirty = true;
                //editor.InsertBreak(BreakType.PageBreak);

                Telerik.Windows.Documents.Flow.FormatProviders.Docx.DocxFormatProvider provider3 = new();
                byte[] r = provider3.Export(document);
                //System.IO.File.WriteAllBytes(@"C:\Users\Barboza\Downloads\salida.docx", r);

                // return this.Json(new { Code = 1, Data = r, FileName = "reporte.docx" });

                return Json(new
                {
                    response = new ApiResponse<byte[]>
                    {
                        Structure = r,
                        Code = 1,
                        Description = ""
                    }
                });
            }
            catch (Exception ex)
            {

                int lineNumber = 0;
                const string lineSearch = ":line ";
                int index = ex.StackTrace.LastIndexOf(lineSearch);
                if (index != -1)
                {
                    string lineNumberText = ex.StackTrace[(index + lineSearch.Length)..];
                    if (int.TryParse(lineNumberText, out lineNumber))
                    {
                        _logger.Information(ex, "REPORTE CONSOLIDADO ERROR EN LINEA: " + lineNumber);
                    }
                }

                return Json(new
                {
                    response = new ApiResponse<OnafViewModel>
                    {
                        Code = -1,
                        Description = "Hubo un problema al descargar el archivo",
                        Structure = null
                    }
                });
            }
        }

        private string InsertarImagenes(ReporteConsolidadoModel subreporte, RadFlowDocument document, Section currentseccion, bool esPrimero)
        {
            string[] splits = new string[] { };

            try
            {
                splits = Path.GetFileNameWithoutExtension(subreporte.NombreReporte).Split("-");

                ConversionImg resultado = Convertir(subreporte.Reporte, subreporte.DESCRIPCION, int.Parse(splits[2]));

                string allErrosPrepare = resultado.Errore.Any() ? resultado.Errore.Aggregate((c, n) => c + "\n*" + n) : string.Empty;

                if (!string.IsNullOrEmpty(allErrosPrepare))
                {
                    return allErrosPrepare;
                }
                if (splits.Length != 3)
                {

                    return "No se ha podido calcular la cantidad de filas del reporte";
                }

                foreach (byte[] img in resultado.Imgs)
                {
                    Telerik.Windows.Documents.Flow.Model.Shapes.ImageInline imageInline =
                       new(document);
                    Telerik.Windows.Documents.Media.ImageSource src = new(img, "jpg");

                    Paragraph paragraph3 = currentseccion.Blocks.AddParagraph();
                    paragraph3.TextAlignment = Alignment.Center;
                    _ = paragraph3.Inlines.AddImageInline();

                    imageInline.Image.ImageSource = src;
                    imageInline.Image.SetHeight(false, esPrimero ? 710 : 730);
                    imageInline.Image.SetWidth(false, 620);
                    paragraph3.Inlines.Insert(1, imageInline);
                }

                return string.Empty;

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private ConversionImg Convertir(string base64, string nombreReporte, double filas)
        {
            try
            {
                List<byte[]> imagenes = new();
                Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.PdfFormatProvider docProvider2 = new();
                Telerik.Windows.Documents.Fixed.Model.RadFixedDocument documen2 = new();

                byte[] file = Convert.FromBase64String(base64);

                documen2 = docProvider2.Import(file);
                byte[] data = docProvider2.Export(documen2);

                PdfImportSettings settings = new()
                {
                    ReadingMode = ReadingMode.OnDemand
                };
                PdfFormatProvider provider = new()
                {
                    ImportSettings = settings
                };
                RadFixedDocument doc = provider.Import(data);
                PageCollection pages = doc.Pages;

                PdfToImageConverter pdfToImageConverter = new()
                {
                    LicenseKey = "0F5PX0tPX09fSVFPX0xOUU5NUUZGRkZfTw==",

                    // set the color space of the resulted images
                    ColorSpace = PdfPageImageColorSpace.RGB,

                    // set the resolution of the resulted images
                    Resolution = 150
                };

                int paginas = 1;
                ConversionImg resultado = null;
                byte[] imageBytes = null;
                List<string> Errores = new();
                foreach (RadFixedPage page in pages)
                {
                    int w = 0;
                    int h = 0;
                    try
                    {
                        w = (int)PageLayoutHelper.GetActualWidth(pages[0]);
                        h = (int)PageLayoutHelper.GetActualHeight(pages[0]);

                        byte[] sourcePdfBytes = Convert.FromBase64String(base64);

                        // convert the PDF pages to images in memory
                        PdfPageImage[] pdfPageImages = pdfToImageConverter.ConvertPdfPagesToImage(sourcePdfBytes, paginas, paginas);

                        if (pdfPageImages.Length == 0)
                        {
                            Errores.Add("El pdf no cuenta con paginas para convertir");
                        }

                        try
                        {
                            // get the image object for the first PDF page in range
                            Image imgObj = pdfPageImages[0].ImageObject;

                            imageBytes = GetImageData(imgObj);
                            //System.IO.File.WriteAllBytes(@"C:\Users\Barboza\Downloads\img" + paginas + ".jpg", imageBytes);
                        }
                        catch (Exception e)
                        {
                            Errores.Add("Error de conversion de imagen a bytes para el reporte " + nombreReporte + ": " + e.Message);
                        }
                        finally
                        {
                            // dispose the page images
                            for (int ii = 0; ii < pdfPageImages.Length; ii++)
                                pdfPageImages[ii].Dispose();
                        }

                        int porcentaje = 0;
                        if (filas == 49)
                        {
                            porcentaje = 840;
                        }
                        if (filas == 59)
                        {
                            porcentaje = 600;
                        }
                        else if (filas == 69)
                        {
                            porcentaje = 350;
                        }
                        else if (filas == 79)
                        {
                            porcentaje = 600;
                        }

                        Stream guardar = new MemoryStream(imageBytes);
                        Bitmap bmpImage = new(guardar);
                        Size pp = bmpImage.Size;

                        Rectangle ab = new();
                        ab = porcentaje == 0 ? new Rectangle(0, 0, pp.Width, pp.Height) : new Rectangle(0, 190, pp.Width, pp.Height - porcentaje);

                        Bitmap p2 = bmpImage.Clone(ab, bmpImage.PixelFormat);

                        using (MemoryStream memory = new())
                        {
                            p2.Save(memory, ImageFormat.Jpeg);
                            byte[] bytes = memory.ToArray();
                            //System.IO.File.WriteAllBytes(@"C:\Users\Barboza\Downloads\imgSalida" + paginas + ".jpg", bytes);
                            imagenes.Add(bytes);
                        }
                        paginas++;
                    }
                    catch (Exception e)
                    {
                        Errores.Add("Error de ajuste de imagen para el reporte " + nombreReporte + ": " + e.Message);
                    }
                }

                resultado = new ConversionImg
                {
                    Errore = Errores,
                    Imgs = imagenes
                };

                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private byte[] GetImageData(Image imgObj)
        {
            try
            {
                // save the image data to a memory stream
                MemoryStream imgDataStream = new();
                imgObj.Save(imgDataStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // get the stream data into a buffer
                byte[] imgData = new byte[imgDataStream.Length];
                Array.Copy(imgDataStream.GetBuffer(), imgData, imgData.Length);

                imgDataStream.Close();

                return imgData;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
