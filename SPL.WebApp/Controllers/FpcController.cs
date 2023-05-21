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
    using SPL.WebApp.Domain.Services.FPC;
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
    public class FpcController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IFpcClientService _fpcClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IFpcService _fpcService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProfileSecurityService _profileClientService;
        public FpcController(
            IMasterHttpClientService masterHttpClientService,
            IFpcClientService fpcClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IFpcService fpcService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _fpcClientService = fpcClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _fpcService = fpcService;
            _correctionFactorService = correctionFactorService;
            _hostEnvironment = hostEnvironment;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.FactordePotenciayCapacitancia)))
                {

                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new FpcViewModel { NoSerie = noSerie });
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
                FpcViewModel fpcViewModel = new();
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
                        response = new ApiResponse<RdtViewModel>
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
                        response = new ApiResponse<RadViewModel>
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
                            response = new ApiResponse<FpcViewModel>
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
                            response = new ApiResponse<FpcViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee características",
                                Structure = null
                            }
                        });
                    }

                    if (artifactDesing.VoltageKV is null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<FpcViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee voltages",
                                Structure = null
                            }
                        });
                    }

                    fpcViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
                    fpcViewModel.UnitType = artifactDesing.GeneralArtifact.TipoUnidad;
                    fpcViewModel.Frequency = artifactDesing.GeneralArtifact.Frecuency ?? 0;
                    VoltageKVDTO voltage = artifactDesing.VoltageKV;
                }

                // ARBOL

                ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _fpcClientService.GetFilter(noSerie);

                if (resultFilter.Code.Equals(1))
                {
                    InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.FPC.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.FPC.ToString() };
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
                                    value = item.IdCarga.ToString(),
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
                                    value = item.IdCarga.ToString(),
                                    expanded = false,
                                    hasChildren = false,
                                    text = item.NombreArchivo.Split('.')[0] + "_" + item.IdCarga.ToString() + ".pdf",
                                    spriteCssClass = "pdf",
                                    status = false
                                });
                            }
                        }
                    }

                    fpcViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Factor de Potencia y Capacitancia",
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
                    response = new ApiResponse<FpcViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = fpcViewModel
                    }
                });
            }
            catch (Exception e)
            {

                return Json(new
                {
                    response = new ApiResponse<FpcViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, string unitType, decimal frequency, string specification, string comment)
        {
            try
            {
                ApiResponse<SettingsToDisplayFPCReportsDTO> result = await _gatewayClientService.GetTemplate(noSerie, clavePrueba, unitType, specification, "", frequency, claveIdioma);

                if (result.Code.Equals(-1))
                {
                    return View("Excel", new FpcViewModel
                    {
                        Error = result.Description
                    });
                }

                SettingsToDisplayFPCReportsDTO reportInfo = result.Structure;

                #region Decode Template
                if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
                {
                    return View("Excel", new FpcViewModel
                    {
                        Error = "No existe plantilla para el filtro seleccionado"
                    });
                }

                long NoPrueba = reportInfo.NextTestNumber;

                byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
                Stream stream = new MemoryStream(bytes);

                Workbook workbook = Workbook.Load(stream, ".xlsx");

                _fpcService.PrepareTemplate_FPC(reportInfo, ref workbook, specification, claveIdioma);

                decimal valueAcceptanceCap = Convert.ToDecimal(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptacionCap"))?.Formato);
                decimal valueAcceptanceFP = Convert.ToDecimal(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptacionFP"))?.Formato);
                #endregion
                IEnumerable<ConfigurationReportsDTO> startE = reportInfo.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna1"));
                IEnumerable<ConfigurationReportsDTO> startT = reportInfo.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna2"));
                IEnumerable<ConfigurationReportsDTO> startG = reportInfo.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna3"));
                IEnumerable<ConfigurationReportsDTO> startUST = reportInfo.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna4"));
                IEnumerable<ConfigurationReportsDTO> startID = reportInfo.ConfigurationReports.Where(c => c.Dato.Equals("TitColumna5"));

                return startE.Count() > 0 && startT.Count() > 0 && startG.Count() > 0 && startUST.Count() > 0 && startID.Count() > 0
                    ? View("Excel", new FpcViewModel
                    {
                        ClaveIdioma = claveIdioma,
                        ClavePrueba = clavePrueba,
                        NoPrueba = NoPrueba,
                        NoSerie = noSerie,
                        UnitType = unitType,
                        Frequency = frequency,
                        Specification = specification,
                        FPCReportsDTO = result.Structure,
                        AcceptanceValueCap = valueAcceptanceCap,
                        AcceptanceValueFP = valueAcceptanceFP,
                        Workbook = workbook,
                        Error = string.Empty,
                        Comments = comment
                    })
                    : (IActionResult)View("Excel", new FpcViewModel
                    {
                        ClaveIdioma = claveIdioma,
                        ClavePrueba = clavePrueba,
                        NoPrueba = NoPrueba,
                        NoSerie = noSerie,
                        UnitType = unitType,
                        Frequency = frequency,
                        Specification = specification,
                        FPCReportsDTO = result.Structure,
                        AcceptanceValueCap = valueAcceptanceCap,
                        AcceptanceValueFP = valueAcceptanceFP,
                        Workbook = workbook,
                        Error = "Las posiciones E,T,G, UST o ID no tienen registros",
                        Comments = comment
                    });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<FpcViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] FpcViewModel viewModel)
        {
            try
            {
                List<FPCTestsDTO> fpcTestDTOs = new()
            {
                new FPCTestsDTO()
                {
                    AcceptanceValueCap = viewModel.AcceptanceValueCap,
                    AcceptanceValueFP = viewModel.AcceptanceValueFP,
                    Frequency = viewModel.Frequency,
                    Specification = viewModel.Specification,
                    UnitType =  viewModel.UnitType,
                    FPCTestsDetails = new List<FPCTestsDetailsDTO>()
                }
            };

                if (viewModel.ClavePrueba.Equals(TestType.AYD.ToString()))
                {
                    fpcTestDTOs.Add(new FPCTestsDTO()
                    {
                        AcceptanceValueCap = viewModel.AcceptanceValueCap,
                        AcceptanceValueFP = viewModel.AcceptanceValueFP,
                        Frequency = viewModel.Frequency,
                        Specification = viewModel.Specification,
                        UnitType = viewModel.UnitType,
                        FPCTestsDetails = new List<FPCTestsDetailsDTO>()
                    });
                }

                _fpcService.Prepare_FPC_Test(viewModel.FPCReportsDTO, viewModel.Workbook, ref fpcTestDTOs);
                ApiResponse<List<CorrectionFactorDTO>> corr = await _correctionFactorService.GetAllDataFactor(viewModel.Specification, decimal.Round((fpcTestDTOs[0].UpperOilTemperature + fpcTestDTOs[0].LowerOilTemperature) / 2, 0, MidpointRounding.AwayFromZero), -1);

                if (corr.Code is -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<FpcViewModel>
                        {
                            Code = -2,
                            Description = "No existe factor de corrección asociado para la especificación y temperatura media ingresadas"
                        }
                    });
                }

                fpcTestDTOs[0].CorrectionFactorSpecifications = corr.Structure.First();

                if (viewModel.ClavePrueba.Equals(TestType.AYD.ToString()))
                {
                    corr = await _correctionFactorService.GetAllDataFactor(viewModel.Specification, decimal.Round((fpcTestDTOs[1].UpperOilTemperature + fpcTestDTOs[1].LowerOilTemperature) / 2, 0), -1);
                    if (corr.Code is -1)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<FpcViewModel>
                            {
                                Code = -2,
                                Description = "No existe factor de corrección asociado para la especificación y temperatura media ingresadas"
                            }
                        });
                    }
                    fpcTestDTOs[1].CorrectionFactorSpecifications = corr.Structure.First();
                }

                int rows = viewModel.FPCReportsDTO.TitleOfColumns.Count;
                foreach (FPCTestsDTO item in fpcTestDTOs)
                {
                    if (item.FPCTestsDetails.Count != rows || item.FPCTestsDetails.Exists(x => x.Power < 0) || item.FPCTestsDetails.Exists(x => x.Current < 0))
                    {
                        return Json(new
                        {
                            response = new ApiResponse<FpcViewModel>
                            {
                                Code = -2,
                                Description = "Faltan datos por ingresar en la tabla"
                            }
                        });
                    }
                    if (item.LowerOilTemperature == 0 || item.UpperOilTemperature == 0 || item.Tension == 0)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<FpcViewModel>
                            {
                                Code = -2,
                                Description = "Faltan datos por ingresar en la tabla"
                            }
                        });
                    }
                }

                ApiResponse<ResultFPCTestsDTO> resultTestFPC_Response = await _fpcClientService.CalculateTestFPC(fpcTestDTOs);

                if (resultTestFPC_Response.Code.Equals(-1))
                {
                    return Json(new
                    {
                        response = new ApiResponse<FpcViewModel>
                        {
                            Code = -1,
                            Description = resultTestFPC_Response.Description,
                            Structure = viewModel
                        }
                    });
                }
                for (int i = 0; i < resultTestFPC_Response.Structure.FPCTests.Count(); i++)
                {
                    resultTestFPC_Response.Structure.FPCTests[i].TempFP = (int)resultTestFPC_Response.Structure.FPCTests[i].TempFP;
                    resultTestFPC_Response.Structure.FPCTests[i].TempTanD = (int)resultTestFPC_Response.Structure.FPCTests[i].TempTanD;
                }

                Workbook workbook = viewModel.Workbook;

                _fpcService.PrepareIndexOfFPC(resultTestFPC_Response.Structure, viewModel.FPCReportsDTO, viewModel.ClaveIdioma, ref workbook);

                #region fillColumns
                fpcTestDTOs = resultTestFPC_Response.Structure.FPCTests.ToList();

                List<ValidateCapacitanceDTO> listCapacitance = new();
                foreach (FPCTestsDTO item in fpcTestDTOs)
                {
                    int position = fpcTestDTOs.IndexOf(item);

                    foreach (FPCTestsValidationsCapDTO item2 in item.Capacitance)
                    {
                        int positionCap = item.Capacitance.IndexOf(item2) + 1;

                        listCapacitance.Add(new ValidateCapacitanceDTO()
                        {
                            Operacion = item2.Operation,
                            Seccion = position + 1,
                            Columnas = item2.Value,
                            Valor = item2.Result

                        });

                    }
                }

                #endregion

                viewModel.FpcTestDTOs = fpcTestDTOs.ToList();
                viewModel.ValidateCapacitances1 = listCapacitance.Where(item => item.Seccion == 1).ToList();
                viewModel.ValidateCapacitances2 = listCapacitance.Where(item => item.Seccion == 2).ToList();

                viewModel.Workbook = workbook;
                viewModel.OfficialWorkbook = workbook;
                bool resultReport = !resultTestFPC_Response.Structure.Results.Any();
                viewModel.IsReportAproved = resultReport;

                string errors = string.Empty;
                List<string> errorMessages = resultTestFPC_Response.Structure.Results.Select(k => k.Message).ToList();
                string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
                return Json(new
                {
                    response = new ApiResponse<FpcViewModel>
                    {
                        Code = 1,
                        Description = allError,
                        Structure = viewModel
                    }
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<FpcViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }
        }

        [HttpPost]

        public async Task<IActionResult> SavePDF([FromBody] FpcViewModel viewModel)
        {

            try
            {
                #region Export Excel
                if (!_fpcService.Verify_FPC_Columns(viewModel.FPCReportsDTO, viewModel.Workbook, viewModel.FpcTestDTOs.First().FPCTestsDetails.Count))
                {
                    return Json(new
                    {
                        response = new { status = -2, description = "Faltan datos por ingresar en la tabla" }
                    });
                }

                Workbook workbook = viewModel.OfficialWorkbook;
                _fpcService.CloneWorkbook(viewModel.Workbook, viewModel.FPCReportsDTO, ref workbook, out List<DateTime> reportDates);
                int t = 0;
                foreach (FPCTestsDTO item in viewModel.FpcTestDTOs)
                {
                    item.Date = reportDates[t];
                    t++;
                }

                viewModel.OfficialWorkbook = workbook;

                Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.OfficialWorkbook.ToDocument();

                PdfFormatProvider provider = new()
                {
                    ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
                };
                document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.OfficialWorkbook.ActiveSheet);
                FloatingImage image = null;
                int rows = viewModel.FPCReportsDTO.TitleOfColumns.Count;
                int rowCount = 0; foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
                {
                    //double officialSize = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(15, 3)].GetFontSize().Value;
                    rowCount = sheet.UsedCellRange.RowCount;
                    #region FormatPDF

                    Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment allign = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(15, 2)].GetHorizontalAlignment().Value;

                    IEnumerable<ConfigurationReportsDTO> starts = viewModel.FPCReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TensionPrueba"));
                    int[] _positionWB;
                    foreach (ConfigurationReportsDTO item in starts)
                    {
                        _positionWB = GetRowColOfWorbook(item.Celda);
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetHorizontalAlignment(allign);
                        string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(FormatStringDecimal(val, 3));
                    }

                    starts = viewModel.FPCReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("TempAceiteSup") || c.Dato.Equals("TempAceiteInf"));
                    foreach (ConfigurationReportsDTO item in starts)
                    {
                        _positionWB = GetRowColOfWorbook(item.Celda);
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetHorizontalAlignment(allign);
                        string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(FormatStringDecimal(val, 1));
                    }

                    starts = viewModel.FPCReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Corriente") || c.Dato.Equals("Potencia") || c.Dato.Equals("PorcFP") || c.Dato.Equals("PorcTanD"));
                    foreach (ConfigurationReportsDTO item in starts)
                    {
                        _positionWB = GetRowColOfWorbook(item.Celda);
                        for (int i = 0; i < rows; i++)
                        {
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1])].SetHorizontalAlignment(allign);
                            string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1])].GetValue().Value.RawValue;
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1])].SetValueAsText(FormatStringDecimal(val, 3));
                        }
                    }

                    starts = viewModel.FPCReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Capacitancia"));
                    foreach (ConfigurationReportsDTO item in starts)
                    {
                        _positionWB = GetRowColOfWorbook(item.Celda);
                        for (int i = 0; i < rows; i++)
                        {
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1])].SetHorizontalAlignment(allign);
                            string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1])].GetValue().Value.RawValue;
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1])].SetValueAsText(FormatStringDecimal(val, 0));
                        }
                    }

                    starts = viewModel.FPCReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Renglon"));
                    foreach (ConfigurationReportsDTO item in starts)
                    {
                        _positionWB = GetRowColOfWorbook(item.Celda);
                        for (int i = 0; i < rows; i++)
                        {
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1])].SetHorizontalAlignment(allign);
                            string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1])].GetValue().Value.RawValue;
                            sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + i, _positionWB[1])].SetValueAsText(val);
                        }
                    }

                    // starts = viewModel.FPCReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Fecha"));

                    //foreach (ConfigurationReportsDTO item in starts)
                    //{

                    //    _positionWB = GetRowColOfWorbook(item.Celda);
                    //    sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetHorizontalAlignment(allign);
                    //    string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                    //    sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 3));
                    //}
                    #endregion

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
                #endregion

                #region Save Report
                FPCTestsGeneralDTO fpcTestsGeneralDTO = new()
                {
                    Capacity = viewModel.FPCReportsDTO.HeadboardReport.Capacity,
                    Comment = viewModel.Comments,
                    Creadopor = User.Identity.Name,
                    Customer = viewModel.FPCReportsDTO.HeadboardReport.Client,
                    Data = viewModel.FpcTestDTOs,
                    Fechacreacion = DateTime.Now,
                    File = viewModel.Base64PDF,
                    Frequency = viewModel.Frequency,
                    IdLoad = 0,
                    KeyTest = viewModel.ClavePrueba,
                    LanguageKey = viewModel.ClaveIdioma,
                    LoadDate = DateTime.Now,
                    Modificadopor = null,
                    Fechamodificacion = null,
                    NameFile = string.Concat("FPC", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                    Result = viewModel.IsReportAproved,
                    SerialNumber = viewModel.NoSerie,
                    Specification = viewModel.Specification,
                    TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                    TypeReport = ReportType.FPC.ToString(),
                    TypeUnit = viewModel.UnitType
                };
                #endregion

                try
                {
                    ApiResponse<long> result = await _fpcClientService.SaveReport(fpcTestsGeneralDTO);

                    var resultResponse = new { status = result.Code, description = result.Description, nameFile = fpcTestsGeneralDTO.NameFile, file = fpcTestsGeneralDTO.File };

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
            catch (Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<FpcViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPDFReport(long code, string typeReport)
        {
            try
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
            catch (Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<FpcViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }
        }

        public IActionResult Error() => View();

        #region PrivateMethods
        public async Task PrepareForm()
        {
            try
            {
                IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

                // Tipos de prueba
                ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.FPC.ToString(), "-1");
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

                ViewBag.Specifications = new SelectList(new List<GeneralPropertiesDTO>()
            {
                new GeneralPropertiesDTO() {
                    Clave = "Otros", Descripcion="Otros"
                },
                new GeneralPropertiesDTO() {
                    Clave = "Doble", Descripcion="Doble"
                }, new GeneralPropertiesDTO() {
                    Clave = "NMX", Descripcion = "NMX"
                }
            }, "Clave", "Descripcion");

                generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetRulesEquivalents, (int)MicroservicesEnum.splmasters));
                ViewBag.Norms = new SelectList(generalProperties, "Clave", "Descripcion");
            }
            catch (Exception)
            {
            }
        }

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

