namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Identity.Web;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.Helpers;
    using SPL.WebApp.ViewModels;

    using Telerik.Web.Spreadsheet;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export;
    using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

    /// <summary>
    /// Relación de Transformación
    /// </summary>
    public class TdpController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly ITdpClientService _tdpClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly ITdpService _tdpService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IRodClientService _rodClientService;
        private readonly IPceClientService _pceService;
        private readonly IProfileSecurityService _profileClientService;
        public TdpController(
            IMasterHttpClientService masterHttpClientService,
            ITdpClientService tdpClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            ITdpService tdpService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            IRodClientService rodClientService,
            IPceClientService pceService
            ,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _tdpClientService = tdpClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _tdpService = tdpService;
            _correctionFactorService = correctionFactorService;
            _rodClientService = rodClientService;
            _hostEnvironment = hostEnvironment;
            _pceService = pceService;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.TensiónInducidaconMedicióndeDescargasParciales)))
                {
                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new TdpViewModel { NoSerie = noSerie });
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

        public async Task<IActionResult> GetFilter(string noSerie)
        {
            try
            {
                TdpViewModel pciViewModel = new();
                noSerie = noSerie.ToUpper().Trim();
                string noSerieSimple = string.Empty;

                ApiResponse<PositionsDTO> dataSelect = await _gatewayClientService.GetPositions(noSerie);

                pciViewModel.Positions = dataSelect.Structure;

                if (!string.IsNullOrEmpty(noSerie))
                {
                    noSerieSimple = noSerie.Split("-")[0];
                }
                else
                {
                    return Json(new
                    {
                        response = new ApiResponse<TdpViewModel>
                        {
                            Code = -1,
                            Description = "No. Serie inválido.",
                            Structure = null
                        }
                    });
                }

                bool isFromSPL = await _artifactClientService.CheckNumberOrder(noSerieSimple);

                if (!isFromSPL)
                {
                    return Json(new
                    {
                        response = new ApiResponse<TdpViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no presente en SPL",
                            Structure = null
                        }
                    });
                }
                else
                {
                    InformationArtifactDTO artifactDesing = await _artifactClientService.GetArtifact(noSerieSimple);
                    if (artifactDesing.GeneralArtifact.OrderCode == null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<TdpViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no encontrado",
                                Structure = null
                            }
                        });
                    }

                    if (artifactDesing.CharacteristicsArtifact.Count == 0)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<TdpViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee características",
                                Structure = null
                            }
                        });
                    }
                    else
                    {
                        pciViewModel.CharacteristicsArtifact = artifactDesing.CharacteristicsArtifact;
                    }

                    pciViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
                    VoltageKVDTO voltage = artifactDesing.VoltageKV;
                    List<GeneralPropertiesDTO> selectV = new()
                    {
                        new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." }
                    };

                    if (voltage.TensionKvAltaTension1 > 0 && voltage.TensionKvBajaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}" });

                    }

                    if (voltage.TensionKvAltaTension1 > 0 && voltage.TensionKvBajaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}" });

                    }

                    if (voltage.TensionKvAltaTension1 > 0 && voltage.TensionKvSegundaBaja1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}" });
                    }

                    if (voltage.TensionKvAltaTension1 > 0 && voltage.TensionKvTerciario1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}" });

                    }

                    if (voltage.TensionKvBajaTension1 > 0 && voltage.TensionKvTerciario1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}" });
                    }

                    if (voltage.TensionKvSegundaBaja1 > 0 && voltage.TensionKvTerciario1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}" });
                    }

                    if (voltage.TensionKvAltaTension3 > 0 && voltage.TensionKvBajaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}" });
                    }

                    if (voltage.TensionKvAltaTension3 > 0 && voltage.TensionKvBajaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}" });
                    }

                    if (voltage.TensionKvAltaTension3 > 0 && voltage.TensionKvSegundaBaja3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}" });
                    }

                    if (voltage.TensionKvAltaTension3 > 0 && voltage.TensionKvTerciario3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}" });
                    }

                    if (voltage.TensionKvBajaTension3 > 0 && voltage.TensionKvTerciario3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}" });

                    }

                    if (voltage.TensionKvSegundaBaja3 > 0 && voltage.TensionKvTerciario3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}" });
                    }

                    if (voltage.TensionKvBajaTension1 > 0 && voltage.TensionKvAltaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}" });
                    }

                    if (voltage.TensionKvBajaTension1 > 0 && voltage.TensionKvAltaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}" });
                    }

                    if (voltage.TensionKvSegundaBaja1 > 0 && voltage.TensionKvAltaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}" });
                    }

                    if (voltage.TensionKvTerciario1 > 0 && voltage.TensionKvAltaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}" });
                    }

                    if (voltage.TensionKvTerciario1 > 0 && voltage.TensionKvBajaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}" });
                    }

                    if (voltage.TensionKvTerciario1 > 0 && voltage.TensionKvSegundaBaja1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}" });
                    }

                    if (voltage.TensionKvBajaTension3 > 0 && voltage.TensionKvAltaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}" });
                    }

                    if (voltage.TensionKvBajaTension3 > 0 && voltage.TensionKvAltaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}" });
                    }

                    if (voltage.TensionKvSegundaBaja3 > 0 && voltage.TensionKvAltaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}" });
                    }

                    if (voltage.TensionKvTerciario3 > 0 && voltage.TensionKvAltaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}" });
                    }

                    if (voltage.TensionKvTerciario3 > 0 && voltage.TensionKvBajaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}" });
                    }

                    if (voltage.TensionKvTerciario3 > 0 && voltage.TensionKvSegundaBaja3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}" });
                    }

                    selectV = selectV.FindAll(x => !x.Clave.Contains("-778899"));
                    pciViewModel.VoltageLevels = new(selectV, "Clave", "Descripcion");

                    ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _tdpClientService.GetFilter(noSerie);

                    if (resultFilter.Code.Equals(1))
                    {
                        InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.TDP.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.TDP.ToString() };
                        IEnumerable<IGrouping<bool?, InfoGeneralReportsDTO>> reportRdtGroupStatus = reportRdt.ListDetails.GroupBy(c => c.Resultado);

                        List<TreeViewItemDTO> reportsAprooved = new();
                        List<TreeViewItemDTO> reportsRejected = new();

                        foreach (IGrouping<bool?, InfoGeneralReportsDTO> reports
                            in reportRdtGroupStatus)
                        {
                            foreach (InfoGeneralReportsDTO item in reports)
                            {
                                if (reports.Key.Value)
                                {
                                    reportsAprooved.Add(new TreeViewItemDTO
                                    {
                                        id = item.IdCarga.ToString(),
                                        expanded = false,
                                        hasChildren = false,
                                        text = item.NombreArchivo.Split('.')[0] + "_" + item.IdCarga.ToString() + ".pdf",
                                        spriteCssClass = "pdf",
                                        status = true
                                    });
                                }
                                else
                                {
                                    reportsRejected.Add(new TreeViewItemDTO
                                    {
                                        id = item.IdCarga.ToString(),
                                        expanded = false,
                                        hasChildren = false,
                                        text = item.NombreArchivo.Split('.')[0] + "_" + item.IdCarga.ToString() + ".pdf",
                                        spriteCssClass = "pdf",
                                        status = false
                                    });
                                }
                            }
                        }
                        pciViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                    {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Tensión Inducida con Medición de Descargas Parciales",
                        expanded = true,
                        hasChildren = true,
                        spriteCssClass = "rootfolder",
                        status = null,
                        items = new List<TreeViewItemDTO>
                        {
                            new TreeViewItemDTO
                            {
                                id = "2",
                                text = "Aprobado",
                                expanded = true,
                                hasChildren = true,
                                spriteCssClass = "folder",
                                status = null,
                                items = reportsAprooved
                            },
                            new TreeViewItemDTO
                            {
                                id = "3",
                                text = "Rechazados",
                                expanded = true,
                                hasChildren = true,
                                spriteCssClass = "folder",
                                status = null,
                                items = reportsRejected
                            }
                        }
                    }
                };
                    }

                    return Json(new
                    {
                        response = new ApiResponse<TdpViewModel>
                        {
                            Code = 1,
                            Description = string.Empty,
                            Structure = pciViewModel
                        }
                    });
                }
            }
            catch (Exception)
            {
                return Json(new
                {
                    response = new ApiResponse<TdpViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = null
                    }
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string nroSerie, string keyTest, string lenguage,
            int noCycles, int totalTime, int interval, decimal timeLevel, decimal outputLevel, int descMayPc, int descMayMv, int incMaxPc,
            string voltageLevels, string measurementType, string terminalsTest, string comments)
        {
            ApiResponse<PositionsDTO> resultP = await _gatewayClientService.GetPositions(nroSerie);

            if (resultP.Code.Equals(-1))
            {
                return View("Excel", new TdpViewModel
                {
                    Error = resultP.Description
                });
            }
            else
            {
                if (string.IsNullOrEmpty(resultP.Structure.ATNom) || string.IsNullOrEmpty(resultP.Structure.BTNom))
                {
                    return View("Excel", new TdpViewModel
                    {
                        Error = "Debe registrar posiciones en tensión de placa para el aparato " + nroSerie
                    });
                }
            }

            ApiResponse<SettingsToDisplayTDPReportsDTO> result = await _gatewayClientService.GetTemplateTDP(nroSerie, keyTest, lenguage,
             noCycles, totalTime, interval, timeLevel, outputLevel, descMayPc, descMayMv, incMaxPc,
             voltageLevels, measurementType, terminalsTest);

            if (result.Code.Equals(-1))
            {
                return View("Excel", new TdpViewModel
                {
                    Error = result.Description
                });
            }

            if (result.Structure.BaseTemplate == null)
            {
                return View("Excel", new TdpViewModel
                {
                    Error = "No se ha encontrado plantilla para reporte TDP"
                });
            }

            if (string.IsNullOrEmpty(result.Structure.BaseTemplate.Plantilla))
            {
                return View("Excel", new TdpViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = result.Structure.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(result.Structure.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");

            _tdpService.PrepareTemplate(result.Structure, ref workbook, keyTest, lenguage, voltageLevels, noCycles);

            CeldasValidate celdas = new("EN")
            {
                CeldaAT = result.Structure.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosicionAT")).Celda,
                CeldaBT = result.Structure.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosicionBT")).Celda,
                CeldaTer = result.Structure.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosicionTer")).Celda
            };

            return View("Excel", new TdpViewModel
            {
                ClaveIdioma = lenguage,
                Pruebas = keyTest,
                NoPrueba = NoPrueba.ToString(),
                NoSerie = nroSerie,
                Workbook = workbook,
                Error = string.Empty,
                Comments = comments,
                Positions = resultP.Structure,
                CeldasPositions = celdas,
                SettingsTDP = result.Structure,
                TiempoIntervalo = interval.ToString(),
                TiempoTotal = totalTime.ToString(),
                DescargaPC = descMayPc.ToString(),
                DescargaUV = descMayMv.ToString(),
                IncrementoMaxPC = incMaxPc.ToString(),
                NroCiclosText = noCycles.ToString(),
                NivelHora = timeLevel.ToString(),
                NivelRealce = outputLevel.ToString(),
                MeasurementType = measurementType,
                VoltageLevel = voltageLevels,
                TerminalesProbar = terminalsTest,
                TipoMedicion = measurementType,

            });
        }

        [HttpGet]
        public async Task<IActionResult> GetClaveAuth(
            string nroSerie, 
            string keyTest, 
            string lenguage,
            int noCycles, 
            int totalTime, 
            int interval, 
            decimal timeLevel, 
            decimal outputLevel, 
            int descMayPc, 
            int descMayMv, 
            int incMaxPc,
            string voltageLevels, 
            string measurementType, 
            string terminalsTest,
            string claveIntroducida)
        {
            try
            {
                ApiResponse<SettingsToDisplayTDPReportsDTO> result = await _gatewayClientService.GetTemplateTDP(nroSerie, keyTest, lenguage,
             noCycles, totalTime, interval, timeLevel, outputLevel, descMayPc, descMayMv, incMaxPc,
             voltageLevels, measurementType, terminalsTest);

                if (result.Code.Equals(-1))
                {
                    return Json(new
                    {
                        response = new ApiResponse<RodViewModel>
                        {
                            Code = -1,
                            Description = result.Description,
                            Structure = null
                        }
                    });
                }

                string clave = result.Structure.ConfigurationReports.Where(x => x.Dato == "ClaveAutoriza")?.FirstOrDefault()?.Formato;

                return string.IsNullOrEmpty(clave)
                    ? Json(new
                    {
                        response = new ApiResponse<RodViewModel>
                        {
                            Code = -1,
                            Description = "No se ha podido obtener la clave de autorización de la configuración del reporte.",
                            Structure = null
                        }
                    })
                    : clave.ToUpper().Trim() != claveIntroducida.ToUpper().Trim()
                    ? Json(new
                    {
                        response = new ApiResponse<bool>
                        {
                            Code = -1,
                            Description = "La clave introducida es incorrecta.",
                            Structure = false
                        }
                    })
                    : (IActionResult)Json(new
                    {
                        response = new ApiResponse<bool>
                        {
                            Code = 1,
                            Description = "",
                            Structure = true
                        }
                    });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    response = new ApiResponse<RodViewModel>
                    {
                        Code = -1,
                        Description = ex.Message,
                        Structure = null
                    }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] TdpViewModel viewModel)
        {
            try
            {
                int[] _positionWB;
                string error = _tdpService.ValidateTemplateTDP(viewModel.SettingsTDP, viewModel.Workbook);
                if (error != string.Empty)
                {
                    return Json(new
                    {
                        response = new ApiResponse<TdpViewModel>
                        {
                            Code = -1,
                            Description = error
                        }
                    });
                }

                List<TDPTerminalsDTO> terminales = new();
                TDPTerminalsDTO terminal = new();

                List<TDPTestsDetailsDTO> details = new();
                TDPTestsDetailsDTO detail = new();

                for (int i = 0; i < viewModel.SettingsTDP.Times.Count; i++)
                {
                    if (viewModel.SettingsTDP.BaseTemplate.ColumnasConfigurables == 3)
                    {
                        detail = new TDPTestsDetailsDTO();
                        terminal = new TDPTerminalsDTO();
                        terminales = new List<TDPTerminalsDTO>();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal1")).Celda);
                        string valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsTDP.TitTerminal1;
                        terminal.pC = decimal.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new TDPTerminalsDTO();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal2")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsTDP.TitTerminal2;
                        terminal.pC = decimal.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new TDPTerminalsDTO();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal3")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsTDP.TitTerminal3;
                        terminal.pC = decimal.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new TDPTerminalsDTO();

                        if (i == 2)//posicion de tiempo cambiante
                        {
                            _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension")).Celda);
                            valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + 2].Cells[_positionWB[1] - 1]?.Value?.ToString();
                        }
                        else
                        {
                            valor = viewModel.SettingsTDP.Times[i];
                        }

                        detail.Time = valor;
                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension")).Celda);
                        detail.Voltage = decimal.Parse(viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());
                        detail.TDPTerminals = terminales;

                        details.Add(detail);
                    }
                    else
                    {
                        detail = new TDPTestsDetailsDTO();
                        terminal = new TDPTerminalsDTO();
                        terminales = new List<TDPTerminalsDTO>();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal1")).Celda);
                        string valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsTDP.TitTerminal1;
                        terminal.pC = decimal.Parse(valor);

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal2")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        terminal.µV = decimal.Parse(valor);

                        terminales.Add(terminal);
                        terminal = new TDPTerminalsDTO();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal3")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsTDP.TitTerminal2;
                        terminal.pC = decimal.Parse(valor);

                        if (i == 2)//posicion de tiempo cambiante
                        {
                            _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension")).Celda);
                            valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + 2].Cells[_positionWB[1] - 1]?.Value?.ToString();
                        }
                        else
                        {
                            valor = viewModel.SettingsTDP.Times[i];
                        }

                        detail.Time = valor;
                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension")).Celda);
                        detail.Voltage = decimal.Parse(viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());
                        detail.TDPTerminals = terminales;

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal4")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.µV = decimal.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new TDPTerminalsDTO();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal5")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsTDP.TitTerminal3;
                        terminal.pC = decimal.Parse(valor);

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal6")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.µV = decimal.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new TDPTerminalsDTO();

                        if (i == 2)//posicion de tiempo cambiante
                        {
                            _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension")).Celda);
                            valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + 2].Cells[_positionWB[1] - 1]?.Value?.ToString();
                        }
                        else
                        {
                            valor = viewModel.SettingsTDP.Times[i];
                        }

                        detail.Time = valor;
                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension")).Celda);
                        detail.Voltage = decimal.Parse(viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString());
                        detail.TDPTerminals = terminales;

                        details.Add(detail);
                    }
                }

                TDPCalibrationMeasurementDTO calibraciones = new();

                if (viewModel.SettingsTDP.BaseTemplate.ColumnasConfigurables == 3)
                {
                    string valor1;
                    string valor2;
                    string valor3;

                    _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelCalibracion")).Celda);
                    valor1 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();
                    valor2 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1]?.Value?.ToString();
                    valor3 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2]?.Value?.ToString();

                    calibraciones.Calibration1 = int.Parse(valor1);
                    calibraciones.Calibration2 = int.Parse(valor2);
                    calibraciones.Calibration3 = int.Parse(valor3);

                    _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelMedido")).Celda);
                    valor1 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();
                    valor2 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1]?.Value?.ToString();
                    valor3 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2]?.Value?.ToString();

                    calibraciones.Measured1 = int.Parse(valor1);
                    calibraciones.Measured2 = int.Parse(valor2);
                    calibraciones.Measured3 = int.Parse(valor3);

                    calibraciones.Grades = viewModel.Notes?.Trim();

                }
                else
                {
                    string valor1;
                    string valor2;
                    string valor3;
                    string valor4;
                    string valor5;
                    string valor6;

                    _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelCalibracion")).Celda);
                    valor1 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();
                    valor2 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1]?.Value?.ToString();
                    valor3 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2]?.Value?.ToString();
                    valor4 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3]?.Value?.ToString();
                    valor5 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4]?.Value?.ToString();
                    valor6 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5]?.Value?.ToString();

                    calibraciones.Calibration1 = int.Parse(valor1);
                    calibraciones.Calibration2 = int.Parse(valor2);
                    calibraciones.Calibration3 = int.Parse(valor3);
                    calibraciones.Calibration4 = int.Parse(valor4);
                    calibraciones.Calibration5 = int.Parse(valor5);
                    calibraciones.Calibration6 = int.Parse(valor6);

                    _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelMedido")).Celda);
                    valor1 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();
                    valor2 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1]?.Value?.ToString();
                    valor3 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2]?.Value?.ToString();
                    valor4 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3]?.Value?.ToString();
                    valor5 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4]?.Value?.ToString();
                    valor6 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5]?.Value?.ToString();

                    calibraciones.Measured1 = int.Parse(valor1);
                    calibraciones.Measured2 = int.Parse(valor2);
                    calibraciones.Measured3 = int.Parse(valor3);
                    calibraciones.Measured4 = int.Parse(valor4);
                    calibraciones.Measured5 = int.Parse(valor5);
                    calibraciones.Measured6 = int.Parse(valor6);

                    calibraciones.Grades = viewModel.Notes?.Trim();
                }

                TDPTestsDTO tDPTestsDTO = new()
                {
                    TDPTestsDetails = details,
                    TDPTestsDetailsCalibration = calibraciones
                };

                TDPTestsGeneralDTO test = new()
                {
                    Interval = int.Parse(viewModel.TiempoIntervalo),
                    TotalTime = int.Parse(viewModel.TiempoTotal),
                    DescMayMv = int.Parse(viewModel.DescargaUV),
                    DescMayPc = int.Parse(viewModel.DescargaPC),
                    IncMaxPc = int.Parse(viewModel.IncrementoMaxPC),
                    Tolerance = 3,
                    MeasurementType = viewModel.MeasurementType,
                    TDPTest = tDPTestsDTO
                };

                ApiResponse<ResultTDPTestsDTO> calculateResult = await _tdpClientService.CalculateTestTdp(test);
                List<string> errores = new();
                if (calculateResult.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<TdpViewModel>
                        {
                            Code = -1,
                            Description = "Error en validaciones TDP " + calculateResult.Description
                        }
                    });
                }

                Workbook workbook = viewModel.Workbook;

                viewModel.Workbook = workbook;

                errores.AddRange(calculateResult.Structure.Results.Select(k => k.Message).ToList());
                string allErrosPrepare = errores.Any() ? errores.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
                bool resultReport = !calculateResult.Structure.Results.Any();

                viewModel.IsReportAproved = resultReport;
                viewModel.Workbook = workbook;
                viewModel.OfficialWorkbook = workbook;
                viewModel.TestTDP = test;

                _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = viewModel.ClaveIdioma.ToUpper() == "ES"
                    ? resultReport ? "Aceptado" : (object)"Rechazado"
                    : resultReport ? "Accepted" : (object)"Rejected";

                return Json(new
                {
                    response = new ApiResponse<TdpViewModel>
                    {
                        Code = 1,
                        Description = allErrosPrepare,
                        Structure = viewModel
                    }
                });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    response = new ApiResponse<TdpViewModel>
                    {
                        Code = -1,
                        Description = ex.Message,
                        Structure = new TdpViewModel { }
                    }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] TdpViewModel viewModel)
        {
            try
            {
                string error = _tdpService.ValidateTemplateTDP(viewModel.SettingsTDP, viewModel.Workbook);
                if (error != string.Empty)
                {
                    return Json(new
                    {
                        response = new ApiResponse<TdpViewModel>
                        {
                            Code = -1,
                            Description = error
                        }
                    });
                }

                Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

                PdfFormatProvider provider = new()
                {
                    ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
                };

                document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.OfficialWorkbook.ActiveSheet);
                FloatingImage image = null;
                //int rows = viewModel.FPBReportsDTO.TitleOfColumns.Count;

                int rowCount = 0; foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
                {
                    //double officialSize = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(15, 3)].GetFontSize().Value;

                    Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment allign = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(15, 2)].GetHorizontalAlignment().Value;
                    rowCount = sheet.UsedCellRange.RowCount;
                    image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(0, 0), 0, 0);
                    string path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
                    FileStream stream = new(path, FileMode.Open);
                    using (stream)
                    {
                        image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                    }

                    image.Width = 215;
                    image.Height = 38;

                    sheet.Shapes.Add(image);
                    sheet.WorksheetPageSetup.PaperType = Telerik.Windows.Documents.Model.PaperTypes.A4;
                    sheet.WorksheetPageSetup.PageOrientation = Telerik.Windows.Documents.Model.PageOrientation.Portrait;
                    //sheet.WorksheetPageSetup.CenterVertically = true;
                    sheet.WorksheetPageSetup.CenterHorizontally = true;
                    sheet.WorksheetPageSetup.PrintOptions.PrintGridlines = false;
                    sheet.WorksheetPageSetup.ScaleFactor = new Telerik.Documents.Primitives.Size(0.9, 0.9);
                    sheet.WorksheetPageSetup.Margins =
                         new Telerik.Windows.Documents.Spreadsheet.Model.Printing.PageMargins(0
                         , 20, 0, 20);
                }

                Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();
                byte[] excelFile;
                byte[] pdfFile;
                string file;
                // Generando Excel
                using (MemoryStream stream = new())
                {
                    formatProvider.Export(document, stream);
                    excelFile = stream.ToArray();
                    file = Convert.ToBase64String(stream.ToArray());
                }

                Spire.Xls.Workbook wbFromStream = new();

                wbFromStream.LoadFromStream(new MemoryStream(excelFile));
                MemoryStream pdfStream = new();
                wbFromStream.SaveToStream(pdfStream, Spire.Xls.FileFormat.PDF);

                pdfFile = pdfStream.ToArray();
                viewModel.Base64PDF = Convert.ToBase64String(pdfFile);

                DateTime basedate = new(1899, 12, 30);
                int[] _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
                string fecha = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

                fecha = fecha.Contains("/") ? (DateTime.Now.Date - basedate.Date).TotalDays.ToString() : fecha;
                viewModel.Date = fecha;

                string AT = string.Empty;
                string BT = string.Empty;
                string Ter = string.Empty;

                _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosicionAT")).Celda);
                AT = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosicionBT")).Celda);
                BT = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosicionTer")).Celda);
                Ter = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Frecuencia")).Celda);
                int frecuencia = int.Parse(viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString());

                viewModel.TestTDP.Capacity = viewModel.SettingsTDP.HeadboardReport.Capacity;
                viewModel.TestTDP.Comment = viewModel.Comments;
                viewModel.TestTDP.Creadopor = User.Identity.Name;
                viewModel.TestTDP.Customer = viewModel.SettingsTDP.HeadboardReport.Client;
                viewModel.TestTDP.Fechacreacion = DateTime.Now;
                viewModel.TestTDP.File = pdfFile;
                viewModel.TestTDP.IdLoad = 0;
                viewModel.TestTDP.KeyTest = viewModel.Pruebas;
                viewModel.TestTDP.LanguageKey = viewModel.ClaveIdioma;
                viewModel.TestTDP.LoadDate = basedate.AddDays(int.Parse(viewModel.Date));
                viewModel.TestTDP.NameFile = string.Concat("TDP", viewModel.Pruebas, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf");
                viewModel.TestTDP.Result = viewModel.IsReportAproved;
                viewModel.TestTDP.SerialNumber = viewModel.NoSerie;
                viewModel.TestTDP.TestNumber = Convert.ToInt32(viewModel.NoPrueba);
                viewModel.TestTDP.TypeReport = ReportType.TDP.ToString();
                viewModel.TestTDP.Tolerance = 3;
                viewModel.TestTDP.NoCycles = int.Parse(viewModel.NroCiclosText);
                viewModel.TestTDP.TotalTime = int.Parse(viewModel.TiempoTotal);
                viewModel.TestTDP.Interval = int.Parse(viewModel.TiempoIntervalo);
                viewModel.TestTDP.TimeLevel = decimal.Parse(viewModel.NivelHora);
                viewModel.TestTDP.OutputLevel = decimal.Parse(viewModel.NivelRealce);
                viewModel.TestTDP.DescMayMv = int.Parse(viewModel.DescargaUV);
                viewModel.TestTDP.DescMayPc = int.Parse(viewModel.DescargaPC);
                viewModel.TestTDP.IncMaxPc = int.Parse(viewModel.IncrementoMaxPC);
                viewModel.TestTDP.VoltageLevels = viewModel.VoltageLevel;
                viewModel.TestTDP.TerminalsTest = viewModel.TerminalesProbar;
                viewModel.TestTDP.MeasurementType = viewModel.TipoMedicion;
                viewModel.TestTDP.Frequency = frecuencia;
                viewModel.TestTDP.Pos_At = AT;
                viewModel.TestTDP.Pos_Bt = BT;
                viewModel.TestTDP.Pos_Ter = Ter;
                viewModel.TestTDP.Date = basedate.AddDays(int.Parse(viewModel.Date));

                ApiResponse<long> result = await _tdpClientService.SaveReport(viewModel.TestTDP);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = viewModel.TestTDP.NameFile, file = viewModel.Base64PDF };

                return Json(new
                {
                    response = resultResponse
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    response = new { status = -1, description = ex.Message, nameFile = "", file = "" }
                });
            }
        }

        public async Task<IActionResult> GetPDFReport(long code, string typeReport)
        {
            ApiResponse<ReportPDFDto> result = await _reportClientService.GetPDFReport(code, typeReport);

            if (result.Code.Equals(-1))
            {
                return Json(new
                {
                    response = result
                });
            }

            byte[] bytes = Convert.FromBase64String(result.Structure.File);
            _ = new MemoryStream(bytes);

            return Json(new
            {
                data = bytes,

            });
        }

        public IActionResult Error() => View();

        #region PrivateMethods
        public async Task PrepareForm()
        {

            /// var a = await this._gatewayClientService.GetPositions("G4135-01");

            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> cicloItems = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." },
                new GeneralPropertiesDTO { Clave = "7200", Descripcion = "7200" } ,
                new GeneralPropertiesDTO { Clave = "6000", Descripcion = "6000" } ,
                new GeneralPropertiesDTO { Clave = "1500", Descripcion = "1500" } ,
                new GeneralPropertiesDTO { Clave = "Otro", Descripcion = "Otro" } ,
            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> tipo = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "Picolumns", Descripcion = "Picolumns" },
                new GeneralPropertiesDTO { Clave = "Microvolts", Descripcion = "Microvolts" } ,
                new GeneralPropertiesDTO { Clave = "Ambos", Descripcion = "Ambos" } ,
            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> terminales = new List<GeneralPropertiesDTO>() {
                         new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." },
                new GeneralPropertiesDTO() { Clave = "H1, H2, H3", Descripcion = "H1, H2, H3" },
                new GeneralPropertiesDTO { Clave = "X1, X2, X3", Descripcion = "X1, X2, X3" } ,
                new GeneralPropertiesDTO { Clave = "Y1, Y2, Y3", Descripcion = "Y1, Y2, Y3" } ,
            }.AsEnumerable();

            // Tipos de prueba
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.TDP.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");
            ViewBag.CicloItems = new SelectList(cicloItems, "Clave", "Descripcion");
            ViewBag.NivelTension = new SelectList(origingeneralProperties, "Clave", "Descripcion");
            ViewBag.TiposMediciones = new SelectList(tipo, "Clave", "Descripcion");
            ViewBag.TerminalesProbar = new SelectList(terminales, "Clave", "Descripcion");
            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");
            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetRulesEquivalents, (int)MicroservicesEnum.splmasters));
        }

        private int[] GetRowColOfWorbook(string cell)
        {
            int[] position = new int[2];
            string row = string.Empty, col = string.Empty;

            for (int i = 0; i < cell.Length; i++)
            {
                if (char.IsDigit(cell[i]))
                {
                    col += cell[i];
                }
                else
                {
                    row += cell[i];
                }
            }

            position[0] = Convert.ToInt32(col);

            for (int i = 0; i < row.Length; i++)
            {
                position[1] += char.ToUpper(row[i]) - 64;
            }

            position[0] += -1;
            position[1] += -1;

            return position;
        }

        private string FormatStringDecimal(string num, int decimals)
        {
            if (!string.IsNullOrEmpty(num))
            {
                if (num.Split('.').Length == 2)
                {
                    string entero = num.Split('.')[0];
                    string decima = num.Split('.')[1];

                    decima = decima.Length > decimals ? decima[..decimals] : decima.PadRight(decimals, '0');
                    num = $"{entero}.{decima}";
                }
                else
                {
                    if (decimals != 0)
                    {
                        num += ".".PadRight(decimals, '0');
                    }
                }
            }
            return num;
        }

        public int[] cantDigitsPoint(string number)
        {
            int[] result = new int[2];
            int dec0, dec1;
            string[] digits;

            if (number.Contains('.'))
                digits = number.Split('.');
            else if (number.Contains(","))
                digits = number.Split(',');
            else
                return new int[] { number.Length, 0 };

            dec0 = digits[0] is null ? 0 : digits[0].Length;

            dec1 = digits.Length == 2 ? digits[1] is null ? 0 : digits[1].Length : 0;

            result[0] = dec0;
            result[1] = dec1;
            return result;
        }
        #endregion
    }
}

