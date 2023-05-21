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
    using SPL.WebApp.Domain.DTOs.DPR;
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
    public class DprController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IDprClientService _dprClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IDprService _dprService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IRodClientService _rodClientService;
        private readonly IPceClientService _pceService;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IProfileSecurityService _profileClientService;
        public DprController(
            IMasterHttpClientService masterHttpClientService,
            IDprClientService dprClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IDprService dprService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            IRodClientService rodClientService,
            IPceClientService pceService,
            ISidcoClientService sidcoClientService,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _dprClientService = dprClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _dprService = dprService;
            _correctionFactorService = correctionFactorService;
            _rodClientService = rodClientService;
            _hostEnvironment = hostEnvironment;
            _pceService = pceService;
            _sidcoClientService = sidcoClientService;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.SobrevoltajeconMedicióndeDescargasParcialesaReactoresdeDerivación)))
                {

                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new DprViewModel { NoSerie = noSerie });
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
                DprViewModel pciViewModel = new();
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
                        response = new ApiResponse<DprViewModel>
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
                        response = new ApiResponse<DprViewModel>
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
                    if (artifactDesing.GeneralArtifact == null || artifactDesing.GeneralArtifact.OrderCode == null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<DprViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no encontrado",
                                Structure = null
                            }
                        });
                    }

                    IEnumerable<CatSidcoDTO> catSidco = await _sidcoClientService.GetCatSIDCO();

                    CatSidcoDTO data = catSidco.FirstOrDefault(x => x.Id == artifactDesing.GeneralArtifact.TypeTrafoId);

                    if (!data.ClaveSPL.ToUpper().Equals("REA") && artifactDesing.GeneralArtifact.Phases != 1)
                    {

                        return Json(new
                        {
                            response = new ApiResponse<DprViewModel>
                            {
                                Code = -1,
                                Description = "El reporte solo se permite a reactores monofásicos",
                                Structure = null
                            }
                        });
                    }

                    if (artifactDesing.CharacteristicsArtifact.Count == 0)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<DprViewModel>
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

                    // pciViewModel.VoltageLevels = new(selectV, "Clave", "Descripcion");

                }

                ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _dprClientService.GetFilter(noSerie);

                if (resultFilter.Code.Equals(1))
                {
                    InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.DPR.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.DPR.ToString() };
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
                    response = new ApiResponse<DprViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = pciViewModel
                    }
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    response = new ApiResponse<DprViewModel>
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
            int noCycles, int totalTime, int interval, decimal timeLevel, decimal outputLevel, int descMayPc, int descMayMv, int incMaxPc
           , string measurementType, string terminalsTest, string comments)
        {
            ApiResponse<PositionsDTO> resultP = await _gatewayClientService.GetPositions(nroSerie);

            if (resultP.Code.Equals(-1))
            {
                return View("Excel", new DprViewModel
                {
                    Error = resultP.Description
                });
            }
            else
            {
                if (string.IsNullOrEmpty(resultP.Structure.ATNom) || string.IsNullOrEmpty(resultP.Structure.BTNom))
                {
                    return View("Excel", new DprViewModel
                    {
                        Error = "Debe registrar posiciones en tensión de placa para el aparato " + nroSerie
                    });
                }
            }

            ApiResponse<SettingsToDisplayDPRReportsDTO> result = await _gatewayClientService.GetTemplateDPR(nroSerie, keyTest, lenguage,
             noCycles, totalTime, interval, timeLevel, outputLevel, descMayPc, descMayMv, incMaxPc, measurementType, terminalsTest);

            if (result.Code.Equals(-1))
            {
                return View("Excel", new DprViewModel
                {
                    Error = result.Description
                });
            }

            if (result.Structure.BaseTemplate == null)
            {
                return View("Excel", new DprViewModel
                {
                    Error = "No se ha encontrado plantilla para reporte DPR"
                });
            }

            if (string.IsNullOrEmpty(result.Structure.BaseTemplate.Plantilla))
            {
                return View("Excel", new DprViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = result.Structure.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(result.Structure.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");

            _dprService.PrepareTemplate(result.Structure, ref workbook, keyTest, lenguage);

            CeldasValidate celdas = new("EN")
            {
                CeldaAT = result.Structure.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT")).Celda,
                CeldaBT = result.Structure.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT")).Celda,
                CeldaTer = result.Structure.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer")).Celda
            };

            return View("Excel", new DprViewModel
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
                SettingsDPR = result.Structure,
                TiempoIntervalo = interval.ToString(),
                TiempoTotal = totalTime.ToString(),
                DescargaPC = descMayPc.ToString(),
                DescargaUV = descMayMv.ToString(),
                IncrementoMaxPC = incMaxPc.ToString(),
                NroCiclosText = noCycles.ToString(),
                NivelHora = timeLevel.ToString(),
                NivelRealce = outputLevel.ToString(),
                KeyTest = keyTest,
                MeasurementType = measurementType,
                TerminalesProbar = terminalsTest,
                TipoMedicion = measurementType,
                ColumnsConfi = (int)result.Structure.BaseTemplate.ColumnasConfigurables,

            }); ;
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] DprViewModel viewModel)
        {
            try
            {
                int[] _positionWB;
                string error = _dprService.ValidateTemplateDPR(viewModel.SettingsDPR, viewModel.Workbook);
                if (error != string.Empty)
                {
                    return Json(new
                    {
                        response = new ApiResponse<DprViewModel>
                        {
                            Code = -1,
                            Description = error
                        }
                    });
                }

                List<DPRTerminalsDTO> terminales = new();
                DPRTerminalsDTO terminal = new();

                List<DPRTestsDetailsDTO> details = new();
                DPRTestsDetailsDTO detail = new();

                for (int i = 0; i < viewModel.SettingsDPR.Times.Count; i++)
                {
                    if (viewModel.SettingsDPR.BaseTemplate.ColumnasConfigurables == 3)
                    {
                        detail = new DPRTestsDetailsDTO();
                        terminal = new DPRTerminalsDTO();
                        terminales = new List<DPRTerminalsDTO>();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_1")).Celda);
                        string valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsDPR.TitTerminal1;
                        terminal.pC = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new DPRTerminalsDTO();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_2")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsDPR.TitTerminal2;
                        terminal.pC = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new DPRTerminalsDTO();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_3")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsDPR.TitTerminal3;
                        terminal.pC = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new DPRTerminalsDTO();

                        detail.Time = viewModel.SettingsDPR.Times[i];
                        detail.Voltage = decimal.Parse(viewModel.SettingsDPR.Voltages[i]);
                        detail.DPRTerminals = terminales;

                        details.Add(detail);
                    }
                    else
                    {
                        detail = new DPRTestsDetailsDTO();
                        terminal = new DPRTerminalsDTO();
                        terminales = new List<DPRTerminalsDTO>();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_1_1")).Celda);
                        string valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsDPR.TitTerminal1;
                        terminal.pC = int.Parse(valor);

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_1_2")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        terminal.µV = int.Parse(valor);

                        terminales.Add(terminal);
                        terminal = new DPRTerminalsDTO();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_2_1")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsDPR.TitTerminal2;
                        terminal.pC = int.Parse(valor);

                        detail.Time = viewModel.SettingsDPR.Times[i];
                        detail.Voltage = decimal.Parse(viewModel.SettingsDPR.Voltages[i]);
                        detail.DPRTerminals = terminales;

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_2_2")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.µV = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new DPRTerminalsDTO();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_3_1")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsDPR.TitTerminal3;
                        terminal.pC = int.Parse(valor);

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal_3_2")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.µV = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new DPRTerminalsDTO();

                        detail.Time = viewModel.SettingsDPR.Times[i];
                        detail.Voltage = decimal.Parse(viewModel.SettingsDPR.Voltages[i]);
                        detail.DPRTerminals = terminales;

                        details.Add(detail);
                    }
                }

                if (viewModel.SettingsDPR.BaseTemplate.ColumnasConfigurables == 3)
                {

                    int[] _positionWBH42 = GetRowColOfWorbook("H42");
                    int[] _positionWBH43 = GetRowColOfWorbook("H43");
                    for (int i = 0; i < 3; i++)
                    {
                        detail = new DPRTestsDetailsDTO();
                        terminal = new DPRTerminalsDTO();
                        terminales = new List<DPRTerminalsDTO>();

                        string valor = viewModel.Workbook.Sheets[0].Rows[_positionWBH42[0]].Cells[_positionWBH42[1] + i]?.Value?.ToString();

                        terminal.Terminal = (i == 0) ? viewModel.SettingsDPR.TitTerminal1 : (i == 1) ? viewModel.SettingsDPR.TitTerminal2 : (i == 2) ? viewModel.SettingsDPR.TitTerminal3 : "";
                        terminal.pC = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new DPRTerminalsDTO();

                        detail.Time = "";
                        detail.Voltage = 0;
                        detail.DPRTerminals = terminales;

                        details.Add(detail);

                        detail = new DPRTestsDetailsDTO();
                        terminal = new DPRTerminalsDTO();
                        terminales = new List<DPRTerminalsDTO>();

                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWBH43[0]].Cells[_positionWBH43[1] + i]?.Value?.ToString();

                        terminal.Terminal = (i == 0) ? viewModel.SettingsDPR.TitTerminal1 : (i == 1) ? viewModel.SettingsDPR.TitTerminal2 : (i == 2) ? viewModel.SettingsDPR.TitTerminal3 : "";
                        terminal.pC = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new DPRTerminalsDTO();

                        detail.Time = "";
                        detail.Voltage = 0;
                        detail.DPRTerminals = terminales;

                        details.Add(detail);

                    }
                }
                else
                {
                    int[] _positionWBH42 = GetRowColOfWorbook("E42");
                    int[] _positionWBH43 = GetRowColOfWorbook("E43");
                    for (int i = 0; i < 6; i++)
                    {
                        detail = new DPRTestsDetailsDTO();
                        terminal = new DPRTerminalsDTO();
                        terminales = new List<DPRTerminalsDTO>();

                        _positionWB = GetRowColOfWorbook("E42");

                        string valor = viewModel.Workbook.Sheets[0].Rows[_positionWBH42[0]].Cells[_positionWBH42[1] + i]?.Value?.ToString();

                        terminal.Terminal = (i == 0) ? viewModel.SettingsDPR.TitTerminal1 : (i == 1) ? viewModel.SettingsDPR.TitTerminal2 : (i == 2) ? viewModel.SettingsDPR.TitTerminal3 : (i == 3) ? viewModel.SettingsDPR.TitTerminal4 : (i == 4) ? viewModel.SettingsDPR.TitTerminal5 : (i == 5) ? viewModel.SettingsDPR.TitTerminal6 : "";
                        terminal.pC = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new DPRTerminalsDTO();

                        detail.Time = "";
                        detail.Voltage = 0;
                        detail.DPRTerminals = terminales;

                        details.Add(detail);

                        detail = new DPRTestsDetailsDTO();
                        terminal = new DPRTerminalsDTO();
                        terminales = new List<DPRTerminalsDTO>();

                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWBH43[0]].Cells[_positionWBH43[1] + i]?.Value?.ToString();

                        terminal.Terminal = (i == 0) ? viewModel.SettingsDPR.TitTerminal1 : (i == 1) ? viewModel.SettingsDPR.TitTerminal2 : (i == 2) ? viewModel.SettingsDPR.TitTerminal3 : (i == 3) ? viewModel.SettingsDPR.TitTerminal4 : (i == 4) ? viewModel.SettingsDPR.TitTerminal5 : (i == 5) ? viewModel.SettingsDPR.TitTerminal6 : "";
                        terminal.pC = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new DPRTerminalsDTO();

                        detail.Time = "";
                        detail.Voltage = 0;
                        detail.DPRTerminals = terminales;

                        details.Add(detail);
                    }
                }

                DPRTestsDTO tDPTestsDTO = new()
                {
                    DPRTestsDetails = details,

                };

                DPRTestsGeneralDTO test = new()
                {
                    Interval = int.Parse(viewModel.TiempoIntervalo),
                    TotalTime = int.Parse(viewModel.TiempoTotal),
                    DescMayMv = int.Parse(viewModel.DescargaUV),
                    DescMayPc = int.Parse(viewModel.DescargaPC),
                    IncMaxPc = int.Parse(viewModel.IncrementoMaxPC),
                    MeasurementType = viewModel.MeasurementType,
                    Tolerance = 3,
                    KeyTest = viewModel.KeyTest,
                    DPRTest = tDPTestsDTO
                };

                ApiResponse<ResultDPRTestsDTO> calculateResult = await _dprClientService.CalculateTestDPR(test);
                List<string> errores = new();
                if (calculateResult.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<DprViewModel>
                        {
                            Code = -1,
                            Description = "Error en validaciones DPR " + calculateResult.Description
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
                viewModel.TestDPR = test;

                _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = viewModel.ClaveIdioma.ToUpper() == "ES"
                    ? resultReport ? "Aceptado" : (object)"Rechazado"
                    : resultReport ? "Accepted" : (object)"Rejected";

                return Json(new
                {
                    response = new ApiResponse<DprViewModel>
                    {
                        Code = 1,
                        Description = allErrosPrepare,
                        Structure = viewModel
                    }
                });

            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] DprViewModel viewModel)
        {

            string error = _dprService.ValidateTemplateDPR(viewModel.SettingsDPR, viewModel.Workbook);
            if (error != string.Empty)
            {
                return Json(new
                {
                    response = new ApiResponse<DprViewModel>
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
            int[] _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
            string fecha = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

            fecha = fecha.Contains("/") ? (DateTime.Now.Date - basedate.Date).TotalDays.ToString() : fecha;
            viewModel.Date = fecha;

            string AT = string.Empty;
            string BT = string.Empty;
            string Ter = string.Empty;

            _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT")).Celda);
            AT = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

            _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT")).Celda);
            BT = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

            _positionWB = GetRowColOfWorbook(viewModel.SettingsDPR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer")).Celda);
            Ter = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

            viewModel.TestDPR.Capacity = viewModel.SettingsDPR.HeadboardReport.Capacity;
            viewModel.TestDPR.Comment = viewModel.Comments;
            viewModel.TestDPR.Grades = viewModel.Notes;

            viewModel.TestDPR.Creadopor = User.Identity.Name;
            viewModel.TestDPR.Customer = viewModel.SettingsDPR.HeadboardReport.Client;
            viewModel.TestDPR.Fechacreacion = DateTime.Now;
            viewModel.TestDPR.File = pdfFile;
            viewModel.TestDPR.IdLoad = 0;
            viewModel.TestDPR.KeyTest = viewModel.Pruebas;
            viewModel.TestDPR.LanguageKey = viewModel.ClaveIdioma;
            viewModel.TestDPR.LoadDate = basedate.AddDays(int.Parse(viewModel.Date));
            viewModel.TestDPR.Modificadopor = string.Empty;
            viewModel.TestDPR.NameFile = string.Concat("DPR", viewModel.Pruebas, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf");
            viewModel.TestDPR.Result = viewModel.IsReportAproved;
            viewModel.TestDPR.SerialNumber = viewModel.NoSerie;
            viewModel.TestDPR.TestNumber = Convert.ToInt32(viewModel.NoPrueba);
            viewModel.TestDPR.TypeReport = ReportType.DPR.ToString();
            viewModel.TestDPR.Tolerance = 3;
            viewModel.TestDPR.NoCycles = int.Parse(viewModel.NroCiclosText);
            viewModel.TestDPR.TotalTime = int.Parse(viewModel.TiempoTotal);
            viewModel.TestDPR.Interval = int.Parse(viewModel.TiempoIntervalo);
            viewModel.TestDPR.TimeLevel = decimal.Parse(viewModel.NivelHora);
            viewModel.TestDPR.OutputLevel = decimal.Parse(viewModel.NivelRealce);
            viewModel.TestDPR.DescMayMv = int.Parse(viewModel.DescargaUV);
            viewModel.TestDPR.DescMayPc = int.Parse(viewModel.DescargaPC);
            viewModel.TestDPR.IncMaxPc = int.Parse(viewModel.IncrementoMaxPC);
            viewModel.TestDPR.VoltageTest = viewModel.VoltageTest;
            viewModel.TestDPR.TerminalsTest = viewModel.TerminalesProbar;
            viewModel.TestDPR.MeasurementType = viewModel.TipoMedicion;
            viewModel.TestDPR.Frequency = (int)viewModel.SettingsDPR.Frequency;
            viewModel.TestDPR.Pos_At = AT;
            viewModel.TestDPR.Pos_Bt = BT;
            viewModel.TestDPR.Pos_Ter = Ter;
            viewModel.TestDPR.Date = basedate.AddDays(int.Parse(viewModel.Date));

            try
            {
                ApiResponse<long> result = await _dprClientService.SaveReport(viewModel.TestDPR);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = viewModel.TestDPR.NameFile, file = viewModel.Base64PDF };

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
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.DPR.ToString(), "-1");
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

