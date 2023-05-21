namespace SPL.WebApp.Controllers
{
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

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export;
    using Telerik.Windows.Documents.Spreadsheet.Model;
    using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
    using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

    /// <summary>
    /// Relación de Transformación
    /// </summary>
    public class RodController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IRodClientService _rodClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IRodService _rodService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IResistanceTwentyDegreesClientServices _resistanceTwentyDegreesClientServices;
        private readonly ILogger _logger;
        private readonly IProfileSecurityService _profileClientService;
        public RodController(
            IMasterHttpClientService masterHttpClientService,
            IRodClientService rodClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IRodService rodService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            ISidcoClientService sidcoClientService,
            IResistanceTwentyDegreesClientServices resistanceTwentyDegreesClientServices,
            ILogger logger,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _rodClientService = rodClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _rodService = rodService;
            _correctionFactorService = correctionFactorService;
            _hostEnvironment = hostEnvironment;
            _sidcoClientService = sidcoClientService;
            _resistanceTwentyDegreesClientServices = resistanceTwentyDegreesClientServices;
            _logger = logger;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.ResistenciaÓhmicadelosDevanados)))
                {

                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new RodViewModel { NoSerie = noSerie });
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

        public async Task<IActionResult> GetFilter(string noSerie, string measuring)
        {
            ApiResponse<PositionsDTO> positions = new();
            string message = "";
            try
            {

                RodViewModel rodViewModel = new();
                noSerie = noSerie.ToUpper().Trim();
                string noSerieSimple = string.Empty;

                if (!string.IsNullOrEmpty(noSerie))
                {
                    noSerieSimple = noSerie.Split("-")[0];
                }
                else
                {
                    return Json(new
                    {
                        response = new ApiResponse<RodViewModel>
                        {
                            Code = -1,
                            Description = "No. Serie inválido.",
                            Structure = null
                        }
                    });
                }

                bool isFromSPL = false;

                try
                {
                    isFromSPL = await _artifactClientService.CheckNumberOrder(noSerieSimple);
                }
                catch (Exception ex)
                {
                    _logger.Warning("ERROR en ROD CHECKNUMNER" + ex.Message, ex);

                    return Json(new
                    {
                        response = new ApiResponse<RodViewModel>
                        {
                            Code = -1,
                            Description = "CheckNumberOrder" + ex.Message + ex.InnerException,
                            Structure = null
                        }
                    });
                }

                if (!isFromSPL)
                {
                    return Json(new
                    {
                        response = new ApiResponse<RodViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no presente en SPL",
                            Structure = null
                        }
                    });
                }
                else
                {
                    InformationArtifactDTO artifactDesing = new();

                    try
                    {
                        artifactDesing = await _artifactClientService.GetArtifact(noSerieSimple);
                    }
                    catch (Exception)
                    {

                    }

                    if (artifactDesing.GeneralArtifact is null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RodViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee información general",
                                Structure = null
                            }
                        });
                    }
                    if (artifactDesing.GeneralArtifact.OrderCode == null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RodViewModel>
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
                            response = new ApiResponse<RodViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee características",
                                Structure = null
                            }
                        });
                    }

                    if (artifactDesing.GeneralArtifact.TypeTrafoId is null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RodViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee información de TypeTrafoId",
                                Structure = null
                            }
                        });
                    }

                    if (artifactDesing.VoltageKV is null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RodViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee voltages",
                                Structure = null
                            }
                        });
                    }

                    IEnumerable<CatSidcoDTO> sidcos = new List<CatSidcoDTO>();

                    try
                    {
                        sidcos = await _sidcoClientService.GetCatSIDCO();
                    }
                    catch (Exception ex)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RodViewModel>
                            {
                                Code = -1,
                                Description = "GetCatSIDCO" + ex.Message + ex.InnerException,
                                Structure = null
                            }
                        });
                    }

                    if (!sidcos.Any())
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RodViewModel>
                            {
                                Code = -1,
                                Description = "La Base de Datos no posee información de CatSIDCO",
                                Structure = null
                            }
                        });
                    }

                    CatSidcoDTO catSidco = sidcos.Where(s => s.AttributeId == 3 && s.Id == Convert.ToInt32(artifactDesing.GeneralArtifact.TypeTrafoId)).FirstOrDefault();

                    if (catSidco is null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RodViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee información de CatSIDCO",
                                Structure = null
                            }
                        });
                    }

                    //esto hay que modificarlo y buscar los datos que estan en resistencia a 20 greados HH,HN,XN,XX
                    //si es AUT puedo seleccionar del combo multiples item
                    // si es TRA debe permitir seleccionar uno solo
                    // se debe crear una tabla mapeando las combinaciones posibles que vienen en el word y validar con lo que venga en el select que hizp el usuario

                    ApiResponse<List<ResistDesignDTO>> resistanceAllVoltageResponse = _resistanceTwentyDegreesClientServices.GetResistDesignCustom(noSerie, measuring, "-1", 20, "-1").Result;

                    if (resistanceAllVoltageResponse.Code == -1)
                    {
                        resistanceAllVoltageResponse = _resistanceTwentyDegreesClientServices.GetResistDesignCustom(noSerieSimple, measuring, "-1", 20, "-1").Result;

                        if (resistanceAllVoltageResponse.Code == -1)
                        {
                            return Json(new
                            {
                                response = new ApiResponse<RodViewModel>
                                {
                                    Code = -1,
                                    Description = "No se puede ejecutar el reporte, falta registrar información de las resistencias de diseño a 20",
                                    Structure = null
                                }
                            });
                        }
                    }

                    List<GeneralPropertiesDTO> listaConexiones = resistanceAllVoltageResponse.Structure.Select(y => y.ConexionPrueba).Distinct().Select(x => new GeneralPropertiesDTO { Clave = x, Descripcion = x }).ToList();

                    rodViewModel.Connections = new(listaConexiones, "Clave", "Descripcion");
                    rodViewModel.IsAutrans = catSidco.ClaveSPL.Equals("AUT") == true;

                    try
                    {
                        positions = await _gatewayClientService.GetPositions(noSerie);
                    }
                    catch (Exception ex)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RodViewModel>
                            {
                                Code = -1,
                                Description = "GetPositions" + ex.Message + ex.InnerException,
                                Structure = null
                            }
                        });
                    }

                    if (positions == null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RodViewModel>
                            {
                                Code = -1,
                                Description = "Positions is nulo",
                                Structure = null
                            }
                        });
                    }

                    if (positions.Structure == null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RodViewModel>
                            {
                                Code = -1,
                                Description = positions.Description,
                                Structure = null
                            }
                        });
                    }

                    if (positions.Structure.AltaTension.Count() == 0 && positions.Structure.BajaTension.Count() == 0 && positions.Structure.Terciario.Count() == 0)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RodViewModel>
                            {
                                Code = -1,
                                Description = "El aparto no tiene tensiones de placa registradas. Por favor diríjase al módulo de tensión de la placa y registre las tensiones para el número de serie deseado",
                                Structure = null
                            }
                        });
                    }

                    rodViewModel.HasAT = positions.Structure.AltaTension.Any();
                    rodViewModel.HasBT = positions.Structure.BajaTension.Any();
                    rodViewModel.HasTer = positions.Structure.Terciario.Any();

                    //List<GeneralPropertiesDTO> selectTestType = new();

                    //bool[] coso = { positions.Structure.AltaTension.Count() > 0, positions.Structure.BajaTension.Count() > 0, positions.Structure.Terciario.Count() > 0 };

                    /*if (coso[0] && coso[1])
                    {
                        selectTestType.Add(new GeneralPropertiesDTO() { Clave = "AYB", Descripcion = "Alta Tensión y Baja Tensión" });
                    }

                    if (coso[0] && coso[2])
                    {
                        selectTestType.Add(new GeneralPropertiesDTO() { Clave = "AYT", Descripcion = "Alta Tensión y Terciario" });
                    }

                    if (coso[1] && coso[2])
                    {
                        selectTestType.Add(new GeneralPropertiesDTO() { Clave = "BYT", Descripcion = "Baja Tensión y Terciario" });
                    }

                    if (coso[0])
                    {
                        selectTestType.Add(new GeneralPropertiesDTO() { Clave = "SAT", Descripcion = "Alta Tensión" });
                    }

                    if (coso[1])
                    {
                        selectTestType.Add(new GeneralPropertiesDTO() { Clave = "SBT", Descripcion = "Baja Tensión" });
                    }

                    if (coso[2])
                    {
                        selectTestType.Add(new GeneralPropertiesDTO() { Clave = "STE", Descripcion = "Terciario" });
                    }*/

                    //rodViewModel.TypeTests = new(selectTestType, "Clave", "Descripcion");
                    rodViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
                    rodViewModel.UnitType = artifactDesing.GeneralArtifact.TipoUnidad;
                    IEnumerable<GeneralPropertiesDTO> unitTypes = await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetUnitTypes, (int)MicroservicesEnum.splmasters);
                    rodViewModel.UnitTypeName = unitTypes.FirstOrDefault(x => x.Clave == rodViewModel.UnitType)?.Descripcion;
                }

                ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = new();

                try
                {
                    resultFilter = await _rodClientService.GetFilter(noSerie);
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        response = new ApiResponse<RodViewModel>
                        {
                            Code = -1,
                            Description = "GetFilter" + ex.Message + ex.InnerException,
                            Structure = null
                        }
                    });
                }

                if (resultFilter.Code.Equals(1))
                {
                    message += "resultFilter.Code";
                    InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.ROD.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.ROD.ToString() };
                    IEnumerable<IGrouping<bool?, InfoGeneralReportsDTO>> reportRdtGroupStatus = reportRdt.ListDetails.GroupBy(c => c.Resultado);

                    List<TreeViewItemDTO> reportsAprooved = new();
                    List<TreeViewItemDTO> reportsRejected = new();
                    message += "reportRdt";
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
                    message += "fuera for";
                    rodViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Resistencia Óhmica de los Devanados",
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
                    message += "fuera for2";
                }

                //ApiResponse<PositionsDTO> dataSelect = await this._gatewayClientService.GetPositions(noSerie);
                //message += "PREGetPositions2";
                rodViewModel.Positions = positions.Structure;
                message += "GetPositions2";
                return Json(new
                {
                    response = new ApiResponse<RodViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = rodViewModel
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

        [HttpGet]
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

        public async Task<IActionResult> CheckInfoResistance20Grades(string noSerie, string connection, string unitOfMeasurement, int posicionesAt,
            int posicionesBt,
            int posicionesTer)
        {
            try
            {
                bool tensionAt = posicionesAt > 0;
                bool tensionBt = posicionesBt > 0;
                bool tensionTer = posicionesTer > 0;

                string noSerieSimple = string.Empty;
                if (!string.IsNullOrEmpty(noSerie))
                {
                    noSerieSimple = noSerie.Split("-")[0];
                }

                ApiResponse<List<ResistDesignDTO>> resistanceAllVoltageResponse = await _resistanceTwentyDegreesClientServices.GetResistDesignCustom(noSerie, unitOfMeasurement, connection, 20, "-1");

                if (resistanceAllVoltageResponse.Code == -1)
                {
                    resistanceAllVoltageResponse = await _resistanceTwentyDegreesClientServices.GetResistDesignCustom(noSerieSimple, unitOfMeasurement, connection, 20, "-1");

                    if (resistanceAllVoltageResponse.Code == -1)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RodViewModel>
                            {
                                Code = -1,
                                Description = "No se puede ejecutar el reporte, falta registrar información de las resistencias de diseño a 20",
                                Structure = null
                            }
                        });
                    }
                }

                if (tensionAt)
                {

                    bool A = resistanceAllVoltageResponse.Structure.Any(x => x.IdSeccion == "AT");
                    if (!A)
                    {

                        return Json(new
                        {
                            response = new ApiResponse<List<ResistDesignDTO>>
                            {
                                Code = -1,
                                Description = " No puede seleccionar posiciones de alta tensión ya que la conexión de prueba no contiene posiciones de alta tensión",
                                Structure = null
                            }
                        });
                    }
                }

                if (tensionBt)
                {
                    bool B = resistanceAllVoltageResponse.Structure.Any(x => x.IdSeccion == "BT");
                    if (!B)
                    {

                        return Json(new
                        {
                            response = new ApiResponse<List<ResistDesignDTO>>
                            {
                                Code = -1,
                                Description = " No puede seleccionar posiciones de baja tensión ya que la conexión de prueba no contiene posiciones de baja tensión",
                                Structure = null
                            }
                        });
                    }
                }

                if (tensionTer)
                {
                    bool T = resistanceAllVoltageResponse.Structure.Any(x => x.IdSeccion.ToUpper() == "TER");
                    if (!T)
                    {

                        return Json(new
                        {
                            response = new ApiResponse<List<ResistDesignDTO>>
                            {
                                Code = -1,
                                Description = " No puede seleccionar posiciones de terciario ya que la conexión de prueba no contiene posiciones de terciario",
                                Structure = null
                            }
                        });
                    }
                }

                return Json(new
                {
                    response = new ApiResponse<List<ResistDesignDTO>>
                    {
                        Code = 1,
                        Description = "",
                        Structure = resistanceAllVoltageResponse.Structure
                    }
                });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    response = new ApiResponse<bool>
                    {
                        Code = -1,
                        Description = ex.Message,
                        Structure = false
                    }
                });
            }
        }

        public async Task<IActionResult> ValidateFilter(string noSerie,
            string claveIdioma,
            string connection,
            string material,
            string unitOfMeasurement,
            decimal? testVoltage,
            string comment,
            int posicionesAt,
            int posicionesBt,
            int posicionesTer)
        {

            try
            {
                string noSerieSimple = noSerie.Split("-")[0];
                InformationArtifactDTO artifactDesing = await _artifactClientService.GetArtifact(noSerieSimple);
                List<PlateTensionDTO> tensions = (await _artifactClientService.GetPlateTension(noSerie, "-1")).Structure;

                if (tensions == null || !tensions.Any())
                {
                    return Json(new
                    {
                        response = new ApiResponse<bool>
                        {
                            Code = -1,
                            Description = "Artefacto no posee tensiones registradas",
                            Structure = false
                        }
                    });
                }

                bool tensionAt = posicionesAt > 0;
                bool tensionBt = posicionesBt > 0;
                bool tensionTer = posicionesTer > 0;

                string clavePrueba = string.Empty;
                int nunColumnas = 0;

                if (tensionAt && !tensionBt && !tensionTer && posicionesAt <= 36)
                {
                    clavePrueba = "SAT";
                    nunColumnas = 1;
                }
                else if (!tensionAt && tensionBt && !tensionTer && posicionesBt <= 36)
                {
                    clavePrueba = "SBT";
                    nunColumnas = 1;

                }
                else if (!tensionAt && !tensionBt && tensionTer && posicionesTer <= 36)
                {
                    clavePrueba = "STE";
                    nunColumnas = 1;

                }
                else if (tensionAt && tensionBt && !tensionTer)
                {
                    if (posicionesAt <= 15 && posicionesBt <= 15)
                    {
                        clavePrueba = "AB1";
                        nunColumnas = 1;

                    }
                    else if (posicionesAt > 15 && posicionesAt < 36 && (posicionesBt <= 15 || posicionesBt > 15) && posicionesBt < 36)
                    {
                        clavePrueba = "AB2";
                        nunColumnas = 2;

                    }
                    else if (posicionesBt > 15 && posicionesBt < 36 && (posicionesAt <= 15 || posicionesAt > 15) && posicionesAt < 36)
                    {
                        clavePrueba = "AB2";
                        nunColumnas = 2;

                    }
                    else
                    {
                        return Json(new
                        {
                            response = new ApiResponse<bool>
                            {
                                Code = -1,
                                Description = "No se ha podido identificar una clave de prueba para seleccionar el reporte.",
                                Structure = false
                            }
                        });
                    }
                }
                else if (tensionTer && tensionAt && !tensionBt)
                {
                    if (posicionesAt <= 15 && posicionesTer <= 15)
                    {
                        clavePrueba = "AYT";
                        nunColumnas = 1;
                    }
                    else if (posicionesAt > 15 && posicionesAt < 36 && (posicionesTer <= 15 || posicionesTer > 15) && posicionesTer < 36)
                    {
                        clavePrueba = "AT2";
                        nunColumnas = 2;
                    }
                    else
                    {
                        return Json(new
                        {
                            response = new ApiResponse<bool>
                            {
                                Code = -1,
                                Description = "No se ha podido identificar una clave de prueba para seleccionar el reporte.",
                                Structure = false
                            }
                        });
                    }
                }
                else if (tensionTer && !tensionAt && tensionBt)
                {
                    if (posicionesBt <= 15 && posicionesTer <= 15)
                    {
                        clavePrueba = "BYT";
                        nunColumnas = 1;
                    }
                    else if (posicionesBt > 15 && posicionesBt < 36 && (posicionesTer <= 15 || posicionesTer > 15) && posicionesTer < 36)
                    {
                        clavePrueba = "BT2";
                        nunColumnas = 2;
                    }
                    else
                    {
                        return Json(new
                        {
                            response = new ApiResponse<bool>
                            {
                                Code = -1,
                                Description = "No se ha podido identificar una clave de prueba para seleccionar el reporte.",
                                Structure = false
                            }
                        });
                    }
                }
                else if (tensionTer && tensionAt && tensionBt)
                {
                    if (posicionesBt <= 8 && posicionesAt <= 8 && posicionesTer <= 8)
                    {
                        clavePrueba = "TO1";
                        nunColumnas = 1;
                    }
                    else if (posicionesAt > 15 && posicionesAt < 36 && posicionesBt <= 15 && posicionesTer <= 15)
                    {
                        clavePrueba = "TA2";
                        nunColumnas = 2;
                    }
                    else if (posicionesBt > 15 && posicionesBt < 36 && posicionesAt <= 15 && posicionesTer <= 15)
                    {
                        clavePrueba = "TB2";
                        nunColumnas = 2;
                    }
                    else if (posicionesAt > 15 && posicionesAt < 36 && posicionesBt > 15 && posicionesBt < 36 && posicionesTer > 15 && posicionesTer < 36)
                    {
                        clavePrueba = "TO3";
                        nunColumnas = 3;
                    }
                    else
                    {
                        return Json(new
                        {
                            response = new ApiResponse<bool>
                            {
                                Code = -1,
                                Description = "No se ha podido identificar una clave de prueba para seleccionar el reporte.",
                                Structure = false
                            }
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        response = new ApiResponse<bool>
                        {
                            Code = -1,
                            Description = "No se ha podido identificar una clave de prueba para seleccionar el reporte.",
                            Structure = false
                        }
                    });
                }

                if (artifactDesing.VoltageKV is null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<bool>
                        {
                            Code = -1,
                            Description = "Artefacto no posee voltages",
                            Structure = false
                        }
                    });
                }

                if (testVoltage is null or 0)
                {
                    bool rAT = !((artifactDesing.VoltageKV.TensionKvAltaTension1 is 0 or null) || (artifactDesing.VoltageKV.TensionKvAltaTension3 is 0 or null));
                    bool rBT = !((artifactDesing.VoltageKV.TensionKvBajaTension1 is 0 or null) || (artifactDesing.VoltageKV.TensionKvBajaTension3 is 0 or null));
                    bool rTER = !((artifactDesing.VoltageKV.TensionKvTerciario1 is 0 or null) || (artifactDesing.VoltageKV.TensionKvTerciario3 is 0 or null));

                    if (rAT && (clavePrueba == TestType.AB1.ToString() || clavePrueba == TestType.AB2.ToString()))
                    {
                        return Json(new
                        {
                            response = new ApiResponse<bool>
                            {
                                Code = -1,
                                Description = "El campo tension de prueba es requerido para Alta",
                                Structure = false
                            }
                        });
                    }
                    if (rBT && (clavePrueba == TestType.AB1.ToString() || clavePrueba == TestType.AB2.ToString()))
                    {
                        return Json(new
                        {
                            response = new ApiResponse<bool>
                            {
                                Code = -1,
                                Description = "El campo tension de prueba es requerido para Baja",
                                Structure = false
                            }
                        });
                    }
                    if (rTER && (clavePrueba == TestType.TO1.ToString() || clavePrueba == TestType.TA2.ToString() || clavePrueba == TestType.TB2.ToString() || clavePrueba == TestType.TO3.ToString()))
                    {
                        return Json(new
                        {
                            response = new ApiResponse<bool>
                            {
                                Code = -1,
                                Description = "El campo tension de prueba es requerido para Terciario",
                                Structure = false
                            }
                        });
                    }
                }
                string[] unit = { "" };

                if (!string.IsNullOrEmpty(noSerie))
                    unit = noSerie.Split('-');
                if (connection == "null")
                {
                    return Json(new
                    {
                        response = new ApiResponse<bool>
                        {
                            Code = -1,
                            Description = "El campo conexión de prueba es requerido",
                            Structure = false
                        }
                    });
                }
                string[] conexiones = connection.Split(",");

                foreach (string item in conexiones)
                {
                    List<ResistDesignDTO> resistDesigns = (await _resistanceTwentyDegreesClientServices.GetResistDesignDTO(noSerie, unitOfMeasurement, item, 20)).Structure;

                    if (resistDesigns.Count() <= 0)
                        resistDesigns = (await _resistanceTwentyDegreesClientServices.GetResistDesignDTO(unit[0], unitOfMeasurement, item, 20)).Structure;

                    if (resistDesigns.Count == 0)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<bool>
                            {
                                Code = -1,
                                Description = "No existe resistencia a 20 grados para el tipo de conexión " + item
                            }
                        }); ;
                    }
                }

                return Json(new
                {
                    response = new ApiResponse<List<string>>
                    {
                        Code = 1,
                        Description = clavePrueba,
                        Structure = new List<string>() { clavePrueba, nunColumnas.ToString() }
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    response = new ApiResponse<bool>
                    {
                        Code = 1,
                        Description = ex.Message,
                        Structure = false
                    }
                });

            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string noSerie,
            string clavePrueba,
            string claveIdioma,
            string connection,
            string unitType,
            string material,
            string unitOfMeasurement,
            decimal? testVoltage,
            string comment,
            string posicionesAt,
            string posicionesBt,
            string posicionesTer,
            int numberColumns)

        {
            List<string> conList = connection.Split(',').ToList();

            int counTensions = (string.IsNullOrEmpty(posicionesAt) ? 0 : 1) + (string.IsNullOrEmpty(posicionesBt) ? 0 : 1) + (string.IsNullOrEmpty(posicionesTer) ? 0 : 1);

            /*if (conList.Count != counTensions)
            {
                return View("Excel", new RodViewModel
                {
                    Error = "Tiene que haber la misma cantidad de conexiones y tipos de tensión seleccionadas"
                });
            }*/

            //get tension
            List<PlateTensionDTO> tensions = (await _artifactClientService.GetPlateTension(noSerie, "-1")).Structure;

            ApiResponse<SettingsToDisplayRODReportsDTO> result
                = await _gatewayClientService.GetTemplate(noSerie, clavePrueba, claveIdioma, connection, unitType, material, unitOfMeasurement, testVoltage, numberColumns, posicionesAt, posicionesBt, posicionesTer);

            if (result.Code.Equals(-1))
            {
                return View("Excel", new RodViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayRODReportsDTO reportInfo = result.Structure;

            #region Decode Template
            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return View("Excel", new RodViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = reportInfo.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.Load(stream, ".xlsx");
            List<string> celdas = new();
            try
            {
                _rodService.PrepareTemplate_ROD(reportInfo, ref workbook, claveIdioma, ref celdas, unitOfMeasurement);
            }
            catch (Exception)
            {
                return View("Excel", new RodViewModel
                {
                    Error = "La plantilla esta mal configurada por favor verifiquela de inmediato"
                });
            }

            string acceptanceValueFas = reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valor_Acep_Fases"))?.Formato;
            string acceptanceValueMaDi = reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valor_AcMa_Diseno"))?.Formato;
            string acceptanceValueMiDi = reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valor_AcMi_Diseno"))?.Formato;
            #endregion

            return View("Excel", new RodViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = clavePrueba,
                Connection = connection,
                UnitType = unitType,
                Material = material,
                UnitOfMeasurement = unitOfMeasurement,
                TestVoltage = testVoltage.ToString(),
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                RODReportsDTO = result.Structure,
                AcceptanceValueFas = Convert.ToDecimal(acceptanceValueFas),
                AcceptanceValueMaDi = Convert.ToDecimal(acceptanceValueMaDi),
                AcceptanceValueMiDi = Convert.ToDecimal(acceptanceValueMiDi),
                Celdas = celdas,
                Workbook = workbook,
                Error = string.Empty,
                Comments = comment ?? string.Empty,
                PosAT = posicionesAt,
                PosBT = posicionesBt,
                NumeroColumnas = numberColumns,
                PosTER = posicionesTer
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetClaveAuth(string noSerie,
            string clavePrueba,
            string claveIdioma,
            string connection,
            string unitType,
            string material,
            string unitOfMeasurement,
            decimal? testVoltage,
            string comment,
            string posicionesAt,
            string posicionesBt,
            string posicionesTer,
            int numberColumns,
            string claveIntroducida)
        {
            try
            {
                ApiResponse<SettingsToDisplayRODReportsDTO> result
              = await _gatewayClientService.GetTemplate(noSerie, clavePrueba, claveIdioma, connection, unitType, material, unitOfMeasurement, testVoltage, numberColumns, posicionesAt, posicionesBt, posicionesTer);

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
        public async Task<IActionResult> ValidateData([FromBody] RodViewModel viewModel)
        {
            try
            {
                List<RODTestsDTO> rodTestDTOs = new()
            {
                new RODTestsDTO()
                {
                    ValorAcepPhases = viewModel.AcceptanceValueFas,
                    ValorAcMaDesign = viewModel.AcceptanceValueMaDi,
                    AcMiDesignValue = viewModel.AcceptanceValueMiDi,
                    WindingMaterial = viewModel.Material,
                    TempFP = 20,
                    TempTanD = viewModel.RODReportsDTO.TitleOfColumns[0].TempSE,
                    BoostTemperature = viewModel.RODReportsDTO.TitleOfColumns[0].TempSE,
                    Connection = viewModel.RODReportsDTO.TitleOfColumns[0].Connection!=null?viewModel.RODReportsDTO.TitleOfColumns[0].Connection:viewModel.Connection,
                    Section = viewModel.RODReportsDTO.TitleOfColumns[0].Position,
                    RODTestsDetails = new List<RODTestsDetailsDTO>()
                }
            };

                if (viewModel.RODReportsDTO.TitleOfColumns.Count >= 2)
                {
                    rodTestDTOs.Add(new RODTestsDTO()
                    {
                        ValorAcepPhases = viewModel.AcceptanceValueFas,
                        ValorAcMaDesign = viewModel.AcceptanceValueMaDi,
                        AcMiDesignValue = viewModel.AcceptanceValueMiDi,
                        WindingMaterial = viewModel.Material,
                        TempFP = 20,
                        TempTanD = viewModel.RODReportsDTO.TitleOfColumns[1].TempSE,
                        BoostTemperature = viewModel.RODReportsDTO.TitleOfColumns[1].TempSE,
                        Connection = viewModel.RODReportsDTO.TitleOfColumns[1].Connection,
                        Section = viewModel.RODReportsDTO.TitleOfColumns[1].Position,
                        RODTestsDetails = new List<RODTestsDetailsDTO>()
                    });
                }

                if (viewModel.RODReportsDTO.TitleOfColumns.Count == 3)
                {
                    rodTestDTOs.Add(new RODTestsDTO()
                    {
                        ValorAcepPhases = viewModel.AcceptanceValueFas,
                        ValorAcMaDesign = viewModel.AcceptanceValueMaDi,
                        AcMiDesignValue = viewModel.AcceptanceValueMiDi,
                        WindingMaterial = viewModel.Material,
                        TempFP = 20,
                        TempTanD = viewModel.RODReportsDTO.TitleOfColumns[2].TempSE,
                        BoostTemperature = viewModel.RODReportsDTO.TitleOfColumns[2].TempSE,
                        Connection = viewModel.RODReportsDTO.TitleOfColumns[2].Connection,
                        Section = viewModel.RODReportsDTO.TitleOfColumns[2].Position,
                        RODTestsDetails = new List<RODTestsDetailsDTO>()
                    });
                }
                if (!_rodService.VerifyPrepare_ROD_Test(viewModel.RODReportsDTO, viewModel.Workbook, ref rodTestDTOs))
                {
                    return Json(new
                    {
                        response = new ApiResponse<RodViewModel>
                        {
                            Code = -2,
                            Description = $"Faltan datos por ingresar en la tabla"
                        }
                    });
                }

                _rodService.Prepare_ROD_Test(viewModel.RODReportsDTO, viewModel.Workbook, ref rodTestDTOs);

                string[] unit = { "" };
                if (!string.IsNullOrEmpty(viewModel.NoSerie))
                    unit = viewModel.NoSerie.Split('-');

                List<ResistDesignDTO> allResistencias = new();
                string[] conexiones = rodTestDTOs.Select(e => e.Connection).ToArray();
                foreach (string item in conexiones.Distinct())
                {
                    List<ResistDesignDTO> resistDesigns = (await _resistanceTwentyDegreesClientServices.GetResistDesignDTO(viewModel.NoSerie, viewModel.UnitOfMeasurement, item, 20)).Structure;

                    if (resistDesigns.Count <= 0)
                    {
                        resistDesigns = (await _resistanceTwentyDegreesClientServices.GetResistDesignDTO(unit[0], viewModel.UnitOfMeasurement, item, 20)).Structure;

                        if (resistDesigns.Count == 0)
                        {
                            return Json(new
                            {
                                response = new ApiResponse<RodViewModel>
                                {
                                    Code = -2,
                                    Description = $"No existe resistencia a 20 grados"
                                }
                            });
                        }
                    }

                    allResistencias.AddRange(resistDesigns);

                }

                foreach (RODTestsDTO rodTestsDTO in rodTestDTOs)
                {
                    foreach (RODTestsDetailsDTO rodTestsDetailsDTO in rodTestsDTO.RODTestsDetails)
                    {
                        rodTestsDetailsDTO.ResistDesigns = allResistencias
                                .FirstOrDefault(x => x.Posicion == rodTestsDetailsDTO.Position && x.IdSeccion == rodTestsDTO.Section && x.ConexionPrueba == rodTestsDTO.Connection);

                        if (rodTestsDetailsDTO.ResistDesigns is null)
                        {
                            return Json(new
                            {
                                response = new ApiResponse<RodViewModel>
                                {
                                    Code = -2,
                                    Description = $"No existe resistencia a 20 grados en la sección {rodTestsDTO.Connection} en la posición {rodTestsDetailsDTO.Position}"
                                }
                            });
                        }
                    }
                }

                ApiResponse<ResultRODTestsDTO> resultTestROD_Response = await _rodClientService.CalculateTestROD(rodTestDTOs);

                if (resultTestROD_Response.Code.Equals(-1))
                {
                    return Json(new
                    {
                        response = new ApiResponse<RodViewModel>
                        {
                            Code = -1,
                            Description = string.Empty,
                            Structure = viewModel
                        }
                    });
                }

                Telerik.Web.Spreadsheet.Workbook workbook = viewModel.Workbook;

                _rodService.PrepareIndexOfROD(resultTestROD_Response.Structure, viewModel.RODReportsDTO, viewModel.ClaveIdioma, ref workbook);

                #region fillColumns
                rodTestDTOs = resultTestROD_Response.Structure.RODTests.ToList();
                #endregion

                viewModel.RodTestDTOs = rodTestDTOs.ToList();
                viewModel.Workbook = workbook;
                viewModel.OfficialWorkbook = workbook;
                bool resultReport = !resultTestROD_Response.Structure.Results.Any();
                viewModel.IsReportAproved = resultReport;

                string errors = string.Empty;
                List<string> errorMessages = resultTestROD_Response.Structure.Results.Select(k => k.Message).ToList();
                string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
                return Json(new
                {
                    response = new ApiResponse<RodViewModel>
                    {
                        Code = 1,
                        Description = allError,
                        Structure = viewModel
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
                        Structure = viewModel
                    }
                });
            }
        }

        [HttpPost]

        public async Task<IActionResult> SavePDF([FromBody] RodViewModel viewModel)
        {
            try
            {
                #region Export Excel
                int[] filas = new int[3];
                string[] lista = new string[] { "AYB", "AYT", "BYT" };
                int[] _positionWB;

                int tables = viewModel.RodTestDTOs.Count();

                for (int i = 0; i < tables; i++)
                {
                    _positionWB = GetRowColOfWorbook(viewModel.RODReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Tit_Term_1")).ElementAt(i).Celda);
                    string tittle1_sec1 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

                    _positionWB = GetRowColOfWorbook(viewModel.RODReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Tit_Term_2")).ElementAt(i).Celda);
                    string tittle2_sec1 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

                    _positionWB = GetRowColOfWorbook(viewModel.RODReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Tit_Term_3")).ElementAt(i).Celda);
                    string tittle3_sec1 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

                    viewModel.RodTestDTOs[i].Title1 = tittle1_sec1;
                    viewModel.RodTestDTOs[i].Title2 = tittle2_sec1;
                    viewModel.RodTestDTOs[i].Title3 = tittle3_sec1;

                    _positionWB = GetRowColOfWorbook(viewModel.RODReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura")).ElementAt(i).Celda);
                    viewModel.RodTestDTOs[i].UmTemperature = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1]?.Value.ToString();
                }

                filas[0] = viewModel.RODReportsDTO.TitleOfColumns[0].Positions.Count;
                filas[1] = viewModel.RODReportsDTO.TitleOfColumns.Count >= 2 ? viewModel.RODReportsDTO.TitleOfColumns[1].Positions.Count : 0;
                filas[2] = viewModel.RODReportsDTO.TitleOfColumns.Count == 3 ? viewModel.RODReportsDTO.TitleOfColumns[2].Positions.Count : 0;
                if (!_rodService.Verify_ROD_Columns(viewModel.RODReportsDTO, viewModel.Workbook, filas[0], filas[1], filas[2]))
                {
                    return Json(new
                    {
                        response = new { status = -1, description = "Faltan datos por ingresar en la tabla" }
                    });
                }

                DateTime date = _rodService.GetDate(viewModel.Workbook, viewModel.RODReportsDTO);
                foreach (RODTestsDTO item in viewModel.RodTestDTOs)
                {
                    item.Date = date;
                }

                Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

                PdfFormatProvider provider = new()
                {
                    ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
                };
                document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.OfficialWorkbook.ActiveSheet);
                FloatingImage image = null;
                int rowCount = 0;

                foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
                {
                    #region FormatPDF

                    Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment allign = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(15, 2)].GetHorizontalAlignment().Value;

                    IEnumerable<ConfigurationReportsDTO> starts = viewModel.RODReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura"));
                    foreach (ConfigurationReportsDTO item in starts)
                    {
                        _positionWB = GetRowColOfWorbook(item.Celda);
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetHorizontalAlignment(allign);
                        string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(FormatStringDecimal(val, 1));
                    }

                    /*IEnumerable<ConfigurationReportsDTO> startPos = viewModel.RODReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Posicion"));
                    foreach (ConfigurationReportsDTO item in startPos)
                    {
                        _positionWB = GetRowColOfWorbook(item.Celda);
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetHorizontalAlignment(allign);
                        string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 1));
                    }*/

                    int section = 0;
                    starts = viewModel.RODReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Terminal_1"));
                    foreach (ConfigurationReportsDTO item in starts)
                    {
                        _positionWB = GetRowColOfWorbook(item.Celda);
                        for (int i = 0; i < viewModel.RODReportsDTO.TitleOfColumns[section].Positions.Count; i++)
                        {
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] - 1)].SetHorizontalAlignment(allign);
                            string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] - 1)].GetValue().Value.RawValue;
                            if (int.TryParse(val, out int r))
                            {
                                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] - 1)].SetValueAsText(FormatStringDecimal(val, 0));
                            }

                            // Terminal 1
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1])].SetHorizontalAlignment(allign);
                            val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1])].GetValue().Value.RawValue;
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1])].SetValueAsText(FormatStringDecimal(val, 6));

                            // Terminal 2
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 1)].SetHorizontalAlignment(allign);
                            val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 1)].GetValue().Value.RawValue;
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 1)].SetValueAsText(FormatStringDecimal(val, 6));

                            // Terminal 3
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 2)].SetHorizontalAlignment(allign);
                            val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 2)].GetValue().Value.RawValue;
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 2)].SetValueAsText(FormatStringDecimal(val, 6));

                            // Resistencia Promedio
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 3)].SetHorizontalAlignment(allign);
                            val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 3)].GetValue().Value.RawValue;
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 3)].SetValueAsText(FormatStringDecimal(val, 4));

                            // Correccion 1
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 4)].SetHorizontalAlignment(allign);
                            val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 4)].GetValue().Value.RawValue;
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 4)].SetValueAsText(FormatStringDecimal(val, 4));

                            // Correccion 2
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 5)].SetHorizontalAlignment(allign);
                            val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 5)].GetValue().Value.RawValue;
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 5)].SetValueAsText(FormatStringDecimal(val, 4));

                            // Desv
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 6)].SetHorizontalAlignment(allign);
                            val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 6)].GetValue().Value.RawValue;
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1] + 6)].SetValueAsText(FormatStringDecimal(val, 2));
                        }
                    }
                    #endregion

                    rowCount = sheet.UsedCellRange.RowCount;

                    for (int i = 1; i <= viewModel.RODReportsDTO.BaseTemplate.ColumnasConfigurables; i++)
                    {

                        string celda = viewModel.RODReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("LogoProlec") && c.Seccion == i).Celda;
                        int posicion = Convert.ToInt32(celda.Remove(0, 1)) - 1;
                        image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(posicion, 0), 0, 0);
                        string path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
                        FileStream stream = new(path, FileMode.Open);
                        using (stream)
                        {
                            image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                        }

                        image.Width = 215;
                        image.Height = 38;

                        sheet.Shapes.Add(image);

                        celda = viewModel.RODReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ImgROD") && c.Seccion == i).Celda;
                        posicion = Convert.ToInt32(celda.Remove(0, 1)) - 3;
                        FloatingImage image2 = new(sheet, new CellIndex(posicion, 8), 30, 30);
                        path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "imgROD.jpg");
                        stream = new(path, FileMode.Open);
                        using (stream)
                        {
                            image2.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                        }
                        image2.Width = 30;
                        image2.Height = 30;

                        sheet.Shapes.Add(image2);

                        string celdapage = viewModel.RODReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado") && c.Seccion == i).Celda;
                        int posicionpage = Convert.ToInt32(celdapage.Remove(0, 1)) - 1;

                        PageBreaks pageBreaks = sheet.WorksheetPageSetup.PageBreaks;

                        _ = pageBreaks.TryInsertHorizontalPageBreak(posicionpage + 9, 0);

                    }

                    sheet.WorksheetPageSetup.PaperType = Telerik.Windows.Documents.Model.PaperTypes.A4;
                    sheet.WorksheetPageSetup.PageOrientation = Telerik.Windows.Documents.Model.PageOrientation.Portrait;
                    //sheet.WorksheetPageSetup.CenterVertically = true;
                    sheet.WorksheetPageSetup.CenterHorizontally = true;
                    sheet.WorksheetPageSetup.PrintOptions.PrintGridlines = false;
                    sheet.WorksheetPageSetup.ScaleFactor = new Telerik.Documents.Primitives.Size(0.9, 1);
                    sheet.WorksheetPageSetup.Margins =
                        new Telerik.Windows.Documents.Spreadsheet.Model.Printing.PageMargins(0
                        , 20, 0, 0);

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
                #endregion

                string aux = string.Empty;
                string[] union = new string[] { };
                if (viewModel.ClavePrueba == "TB2")
                {
                    union = viewModel.Connection.Split(",");
                    aux = union[0];
                    union[0] = union[1];
                    union[1] = aux;

                    aux = string.Join(",", union);
                }
                else
                {
                    aux = viewModel.Connection;
                }

                #region Save Report
                RODTestsGeneralDTO rodTestsGeneralDTO = new()
                {
                    TestConnection = aux,
                    Capacity = viewModel.RODReportsDTO.HeadboardReport.Capacity,
                    Comment = viewModel.Comments,
                    Creadopor = User.Identity.Name,
                    Customer = viewModel.RODReportsDTO.HeadboardReport.Client,
                    Data = viewModel.RodTestDTOs,
                    Fechacreacion = DateTime.Now,
                    File = pdfFile,
                    TestVoltage = string.IsNullOrEmpty(viewModel.TestVoltage) ? 0 : Convert.ToDecimal(viewModel.TestVoltage),
                    UnitOfMeasurement = viewModel.UnitOfMeasurement,
                    UnitType = viewModel.UnitType,
                    WindingMaterial = viewModel.Material,
                    IdLoad = 0,
                    KeyTest = viewModel.ClavePrueba,
                    LanguageKey = viewModel.ClaveIdioma,
                    LoadDate = DateTime.Now,
                    Modificadopor = null,
                    NameFile = string.Concat("ROD", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                    Result = viewModel.IsReportAproved,
                    SerialNumber = viewModel.NoSerie,
                    TestNumber = Convert.ToInt32(viewModel.NoPrueba) == 0 ? 1 : Convert.ToInt32(viewModel.NoPrueba),
                    TypeReport = ReportType.ROD.ToString(),
                    AutorizoCambio = viewModel.ClaveAutoriza ? "Si" : "No"
                };
                #endregion

                //Redondear valores 
                foreach (RODTestsDTO item in rodTestsGeneralDTO.Data)
                {
                    item.MaxPorc_Desv = Math.Round(item.MaxPorc_Desv > 100 ? 99 : item.MaxPorc_Desv, 2);
                    item.MaxPorc_DesvxDesign = Math.Round(item.MaxPorc_DesvxDesign > 100 ? 99 : item.MaxPorc_DesvxDesign, 2);
                    item.MinPorc_DesvxDesign = Math.Round(item.MinPorc_DesvxDesign > 100 ? 99 : item.MinPorc_DesvxDesign, 2);
                }

                rodTestsGeneralDTO.Data[0].UmTemperatureSE = rodTestsGeneralDTO.Data[0].UmTemperature;
                rodTestsGeneralDTO.Data[0].TemperatureSE = rodTestsGeneralDTO.Data[0].Temperature;

                //if (lista.Contains(viewModel.ClavePrueba))
                //{
                if (viewModel.RODReportsDTO.TitleOfColumns.Count >= 2)
                {
                    rodTestsGeneralDTO.Data[1].UmTemperatureSE = rodTestsGeneralDTO.Data[1].UmTemperature;
                    rodTestsGeneralDTO.Data[1].TemperatureSE = rodTestsGeneralDTO.Data[1].Temperature;
                }

                //}
                if (viewModel.RODReportsDTO.TitleOfColumns.Count == 3)
                {
                    rodTestsGeneralDTO.Data[2].UmTemperatureSE = rodTestsGeneralDTO.Data[2].UmTemperature;
                    rodTestsGeneralDTO.Data[2].TemperatureSE = rodTestsGeneralDTO.Data[2].Temperature;
                }
                string a = JsonConvert.SerializeObject(rodTestsGeneralDTO);
                ApiResponse<long> result = await _rodClientService.SaveReport(rodTestsGeneralDTO);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = rodTestsGeneralDTO.NameFile, file = rodTestsGeneralDTO.File };

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
        public IActionResult Error() => View();

        #region PrivateMethods
        public async Task PrepareForm()
        {
            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            // Tipos de prueba
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.ROD.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");

            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");

            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetUnitTypes, (int)MicroservicesEnum.splmasters));
            ViewBag.UnitTypes = new SelectList(generalProperties, "Clave", "Descripcion");
            ViewBag.Connections = new SelectList(new List<GeneralPropertiesDTO>().AsEnumerable(), "Clave", "Descripcion");
            ViewBag.Materials = new SelectList(new List<GeneralPropertiesDTO>()
            {
                new GeneralPropertiesDTO() {
                    Clave = "Cobre", Descripcion="Cobre"
                },
                new GeneralPropertiesDTO() {
                    Clave = "Aluminio", Descripcion="Aluminio"
                }
            }, "Clave", "Descripcion");

            ViewBag.UnitOfMeasurements = new SelectList(new List<GeneralPropertiesDTO>()
            {
                new GeneralPropertiesDTO() {
                    Clave = "Ohms", Descripcion="Ohms"
                },
                new GeneralPropertiesDTO() {
                    Clave = "Miliohms", Descripcion="Miliohms"
                }
            }, "Clave", "Descripcion");

            ViewBag.Select1 = new SelectList(new List<GeneralPropertiesDTO>()
            {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" } ,

            }.AsEnumerable(), "Clave", "Descripcion");
            ViewBag.Select2 = new SelectList(new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" },

            }.AsEnumerable(), "Clave", "Descripcion");
            ViewBag.Select3 = new SelectList(new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" } }.AsEnumerable(), "Clave", "Descripcion");
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyTestVoltage(string testVoltage, bool testVoltageRequired) => Json(!(testVoltageRequired && string.IsNullOrEmpty(testVoltage)));

        private static int[] GetRowColOfWorbook(string cell)
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
        #endregion
    }
}

