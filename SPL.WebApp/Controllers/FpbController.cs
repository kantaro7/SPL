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
    public class FpbController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IFpbClientService _fpbClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IFpbService _fpbService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProfileSecurityService _profileClientService;
        public FpbController(
            IMasterHttpClientService masterHttpClientService,
            IFpbClientService fpbClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IFpbService fpbService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _fpbClientService = fpbClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _fpbService = fpbService;
            _correctionFactorService = correctionFactorService;
            _hostEnvironment = hostEnvironment;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;
                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.FactordePotenciayCapacitanciadeBoquillas)))
                {
                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new FpbViewModel { NoSerie = noSerie });
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
                FpbViewModel fpcViewModel = new();
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
                        response = new ApiResponse<FpbViewModel>
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
                        response = new ApiResponse<FpbViewModel>
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
                            response = new ApiResponse<FpbViewModel>
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
                            response = new ApiResponse<FpbViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee características",
                                Structure = null
                            }
                        });
                    }

                    fpcViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
                    fpcViewModel.UnitType = artifactDesing.GeneralArtifact.TipoUnidad;
                }

                ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _fpbClientService.GetFilter(noSerie);

                if (resultFilter.Code.Equals(1))
                {
                    InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.FPB.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.FPB.ToString() };
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

                                    spriteCssClass = "pdf",
                                    text = item.NombreArchivo.Split('.')[0] + "_" + item.IdCarga.ToString() + ".pdf",
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

                    fpcViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Factor de Potencia y Capacitancia de boquillas",
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
                    response = new ApiResponse<FpbViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = fpcViewModel
                    }
                });
            }
            catch(Exception e)
            {

                return Json(new
                {
                    response = new ApiResponse<FpbViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, string tangtDelta, string comment)
        {
            try
            {
                ApiResponse<SettingsToDisplayFPBReportsDTO> result = await _gatewayClientService.GetTemplate(noSerie, clavePrueba, claveIdioma, tangtDelta);

                if (result.Code.Equals(-1))
                {
                    return View("Excel", new FpbViewModel
                    {
                        Error = result.Description
                    });
                }

                SettingsToDisplayFPBReportsDTO reportInfo = result.Structure;

                #region Decode Template
                if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
                {
                    return View("Excel", new FpbViewModel
                    {
                        Error = "No existe plantilla para el filtro seleccionado",
                    });

                }

                if (clavePrueba == "AYD" && reportInfo.NozzlesByDesignDtos.NozzleInformation.Count >= 15)
                {
                    return View("Excel", new FpbViewModel
                    {
                        Error = "La cantidad de boquillas del aparato sobre pasa los 15 renglones de la plantilla de captura",
                    });

                }

                if ((clavePrueba == "SAN" || clavePrueba == "SDE") && reportInfo.NozzlesByDesignDtos.NozzleInformation.Count >= 36)
                {
                    return View("Excel", new FpbViewModel
                    {
                        Error = "La cantidad de boquillas del aparato sobre pasa los 36 renglones de la plantilla de captura",
                    });
                }

                reportInfo.NozzlesByDesignDtos.NozzleInformation = reportInfo.NozzlesByDesignDtos.NozzleInformation.OrderBy(x => x.Orden).ToList();

                long NoPrueba = reportInfo.NextTestNumber;

                byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
                Stream stream = new MemoryStream(bytes);

                Workbook workbook = Workbook.Load(stream, ".xlsx");

                _fpbService.PrepareTemplate_FPB(reportInfo, ref workbook, clavePrueba, tangtDelta);

                decimal valueAcceptanceCap = Convert.ToDecimal(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptacionCap"))?.Formato);
                decimal valueAcceptanceFP = Convert.ToDecimal(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptacionFP"))?.Formato);
                #endregion

                return View("Excel", new FpbViewModel()
                {
                    AcceptanceValueCap = valueAcceptanceCap,
                    AcceptanceValueFP = valueAcceptanceFP,
                    FPBReportsDTO = result.Structure,
                    Workbook = workbook,
                    NoPrueba = NoPrueba,
                    TanDelta = tangtDelta,
                    NoSerie = noSerie,
                    ClavePrueba = clavePrueba,
                    ClaveIdioma = claveIdioma,
                    Error = string.Empty,
                    Comments = comment
                });
            }
            catch(Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<FpbViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }

           
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] FpbViewModel viewModel)
        {
            try
            {
                ApiResponse<SettingsToDisplayFPBReportsDTO> result = await _gatewayClientService.GetTemplate(viewModel.NoSerie, viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.TanDelta);

                List<FPBTestsDTO> fpbTestDTOs = new()
            {
                new FPBTestsDTO()
                {
                    AcceptanceValueCap = viewModel.AcceptanceValueCap,
                    AcceptanceValueFP = viewModel.AcceptanceValueFP,
                    FPBTestsDetails = new List<FPBTestsDetailsDTO>(),
                    TanDelta = viewModel.TanDelta,
                    TempFP = 0,
                    TempTanD = 0,
                },

            };

                if (viewModel.ClavePrueba == "AYD")
                {
                    _ = decimal.TryParse(viewModel.FPBReportsDTO.ConfigurationReports.Where(x => x.Dato == "ToleranciaFP").FirstOrDefault().Formato, out decimal ToleranciaFP);
                    _ = decimal.TryParse(viewModel.FPBReportsDTO.ConfigurationReports.Where(x => x.Dato == "ToleranciaCap").FirstOrDefault().Formato, out decimal ToleranciaCap);

                    fpbTestDTOs.Add(new FPBTestsDTO()
                    {
                        AcceptanceValueCap = viewModel.AcceptanceValueCap,
                        AcceptanceValueFP = viewModel.AcceptanceValueFP,
                        FPBTestsDetails = new List<FPBTestsDetailsDTO>(),
                        TanDelta = viewModel.TanDelta,
                        TempFP = 0,
                        TempTanD = 0,
                        KeyTest = viewModel.ClavePrueba,
                        ToleranciaFP = ToleranciaFP,
                        ToleranciaCap = ToleranciaCap
                    });

                    fpbTestDTOs[0].ToleranciaCap = ToleranciaCap;
                    fpbTestDTOs[0].ToleranciaFP = ToleranciaFP;

                }

                if (!_fpbService.Verify_FPB_Columns(viewModel.Workbook, result.Structure, viewModel.ClavePrueba))
                {
                    return Json(new
                    {
                        response = new ApiResponse<FpbViewModel>
                        {
                            Code = -1,
                            Description = "Faltan datos por ingresar en la tabla"
                        }
                    });
                }

                string tensionPrueba1 = viewModel.Workbook.Sheets[0].Rows[9].Cells[3].Value.ToString();
                string gradosCentigrados1 = viewModel.Workbook.Sheets[0].Rows[9].Cells[8].Value.ToString();

                int[] validateTension = cantDigitsPoint(tensionPrueba1);
                int[] validateTemp = cantDigitsPoint(gradosCentigrados1);

                if (validateTension[0] > 6 || validateTension[1] > 3)
                {
                    return Json(new
                    {
                        response = new ApiResponse<FpbViewModel>
                        {
                            Code = -1,
                            Description = "La tensión de prueba en kV debe ser mayor a cero considerando 6 enteros con 3 decimales"
                        }
                    });

                }

                if (validateTemp[0] > 3 || validateTemp[1] > 1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<FpbViewModel>
                        {
                            Code = -1,
                            Description = "La temperatura en °C debe ser mayor a cero considerando 3 enteros con 1 decimal"
                        }
                    });

                }

                if (viewModel.ClavePrueba == "AYD")
                {

                    string tensionPrueba2 = viewModel.Workbook.Sheets[0].Rows[29].Cells[3].Value.ToString();
                    string gradosCentigrados2 = viewModel.Workbook.Sheets[0].Rows[29].Cells[8].Value.ToString();

                    int[] validateTension2 = cantDigitsPoint(tensionPrueba2);
                    int[] validateTemp2 = cantDigitsPoint(gradosCentigrados2);

                    if (validateTension2[0] > 6 || validateTension2[1] > 3)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<FpbViewModel>
                            {
                                Code = -1,
                                Description = "La tensión de prueba en kV debe ser mayor a cero considerando 6 enteros con 3 decimales"
                            }
                        });

                    }

                    if (validateTemp2[0] > 3 || validateTemp2[1] > 1)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<FpbViewModel>
                            {
                                Code = -1,
                                Description = "La temperatura en °C debe ser mayor a cero considerando 3 enteros con 1 decimal"
                            }
                        });

                    }
                }

                _fpbService.Prepare_FPB_Test(viewModel.Workbook, ref fpbTestDTOs, result.Structure, viewModel.ClavePrueba);
                int section = 1;
                List<string> errores = new();
                foreach (FPBTestsDTO item in fpbTestDTOs)
                {
                    foreach (var item2 in item.FPBTestsDetails.Select((value, i) => new { i, value }))
                    {
                        if (item2.value.CorrectionFactorSpecifications20Grados is null)
                        {
                            errores.Add($"No existe factor de corrección de 20 grados para las sección {section} en la posicion {item2.i + 1}");
                        }

                        if (item2.value.CorrectionFactorSpecificationsTemperature is null)
                        {
                            errores.Add($"No existe factor corrección de {item.Temperature} grados para las sección {section} en la posicion {item2.i + 1}");
                        }
                    }
                    section++;
                }

                string allErrosPrepare = errores.Any() ? errores.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
                if (errores.Count > 0)
                {
                    return Json(new
                    {
                        response = new ApiResponse<FpbViewModel>
                        {
                            Code = -1,
                            Description = allErrosPrepare
                        }
                    });
                }

                ApiResponse<ResultFPBTestsDTO> resultTest = await _fpbClientService.CalculateTestFPB(fpbTestDTOs);

                if (resultTest.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<FpbViewModel>
                        {
                            Code = -1,
                            Description = resultTest.Description,
                            Structure = viewModel
                        }
                    });
                }

                List<ValidateCapacitanceDTO> listCapacitanceFP = new();
                List<ValidateCapacitanceDTO> listCapacitanceCAP = new();

                foreach (FPBTestsDTO item in resultTest.Structure.FPBTests)
                {
                    int position = resultTest.Structure.FPBTests.IndexOf(item);
                    int indexCapacitancia = 0;
                    foreach (decimal item2 in item.CalValorAceptCAP)
                    {
                        int positionCap = item.CalValorAceptCAP.IndexOf(item2) + 1;

                        listCapacitanceCAP.Add(new ValidateCapacitanceDTO()
                        {
                            Seccion = position + 1,
                            Columnas = "Valor absoluto((Capacitancia / Capacitancia de la boquilla) * 100 - 100), LOS DATOS SON LOS SIGUIENTES: - > " + " Capacitancia -> " + item.FPBTestsDetails[indexCapacitancia].Capacitance + " Capacitancia de la boquilla -> " + item.FPBTestsDetails[indexCapacitancia].NozzlesByDesign.Capacitancia,

                            Valor = Math.Round(item2, 3).ToString()

                        });
                        indexCapacitancia++;
                    }

                    int indexFP = 0;
                    foreach (decimal item2 in item.CalValorAceptFP)
                    {
                        int positionCap = item.CalValorAceptFP.IndexOf(item2) + 1;

                        listCapacitanceFP.Add(new ValidateCapacitanceDTO()
                        {
                            Seccion = position + 1,
                            Columnas = item.TanDelta.ToUpper().Equals("SIN") ?

                            "Valor absoluto((%FP / Factor de potencia de la boquilla) * 100) - 100), LOS DATOS SON LOS SIGUIENTES: - > " + " %FP ->" + item.FPBTestsDetails[indexFP].PercentageA + " Factor de potencia de la boquilla - > " + item.FPBTestsDetails[indexFP].NozzlesByDesign.FactorPotencia + " que se obtiene con la marca, tipo de la boquilla y la temperatura de la sección" + " LOS DATOS SON LOS SIGUIENTES: - >" + " Marca -> " + item.FPBTestsDetails[indexFP].NozzlesByDesign.Marca + " Tipo -> " + item.FPBTestsDetails[indexFP].NozzlesByDesign.Tipo + " y la Temperatura -> " +
                              item.Temperature
                              :
                              "Valor absoluto((% Tan d /  Factor de potencia de la boquilla) * 100 - 100), LOS DATOS SON LOS SIGUIENTES: - > " + " % Tan d ->" + item.FPBTestsDetails[indexFP].PercentageB + " Factor de potencia de la boquilla - > " + item.FPBTestsDetails[indexFP].NozzlesByDesign.FactorPotencia + " que se obtiene con la marca, tipo de la boquilla y la temperatura de la sección" + " LOS DATOS SON LOS SIGUIENTES: - >" + " Marca -> " + item.FPBTestsDetails[indexFP].NozzlesByDesign.Marca + " Tipo -> " + item.FPBTestsDetails[indexFP].NozzlesByDesign.Tipo + " y la Temperatura -> " +
                               item.Temperature,

                            Valor = Math.Round(item2, 3).ToString()

                        });
                        indexFP++;

                    }
                }

                Workbook workbook = viewModel.Workbook;
                bool resultReport = !resultTest.Structure.Results.Any(x => x.Column != 77 && x.Fila != 77);
                fpbTestDTOs = resultTest.Structure.FPBTests.ToList();

                viewModel.ValidateCapacitancesFP = listCapacitanceFP;
                viewModel.ValidateCapacitancesCAP = listCapacitanceCAP;

                viewModel.IsReportAproved = resultReport;
                viewModel.FpbTestDTOs = fpbTestDTOs;
                _fpbService.PrepareIndexOfFPB(resultTest.Structure, result.Structure, viewModel.ClaveIdioma, ref workbook, resultReport, viewModel.ClavePrueba, viewModel.TanDelta);

                viewModel.Workbook = workbook;
                viewModel.OfficialWorkbook = workbook;
                viewModel.FPBReportsDTO = result.Structure;
                string errors = string.Empty;
                List<string> errorMessages = resultTest.Structure.Results.Select(k => k.Message).ToList();
                string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;

                return Json(new
                {
                    response = new ApiResponse<FpbViewModel>
                    {
                        Code = 1,
                        Description = allError,
                        Structure = viewModel
                    }
                });
            }
            catch(Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<FpbViewModel>
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
            catch(Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<FpbViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }

           
        }
        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] FpbViewModel viewModel)
        {
            try
            {
                if (!_fpbService.Verify_FPB_Columns(viewModel.Workbook, viewModel.FPBReportsDTO, viewModel.ClavePrueba))
                {
                    return Json(new
                    {
                        response = new { status = -1, description = "Faltan datos por ingresar en la tabla" }
                    });
                }
                try
                {
                    DateTime basedate = new(1899, 12, 30);

                    if (viewModel.ClavePrueba == "AYD")
                    {
                        if (int.TryParse(viewModel.FechaSec1, out int result1))
                        {
                            viewModel.FpbTestDTOs[0].Date = basedate.AddDays(result1);
                        }
                        else
                        {
                            string[] sec1 = viewModel.FechaSec1.Split('/');
                            viewModel.FpbTestDTOs[0].Date = new DateTime(Convert.ToInt32(sec1[2]), Convert.ToInt32(sec1[0]), Convert.ToInt32(sec1[1]));
                        }

                        if (int.TryParse(viewModel.FechaSec2, out int result2))
                        {
                            viewModel.FpbTestDTOs[1].Date = basedate.AddDays(result2);
                        }
                        else
                        {
                            string[] sec2 = viewModel.FechaSec2.Split('/');
                            viewModel.FpbTestDTOs[1].Date = new DateTime(Convert.ToInt32(sec2[2]), Convert.ToInt32(sec2[0]), Convert.ToInt32(sec2[1]));
                        }
                    }
                    else
                    {
                        if (int.TryParse(viewModel.FechaSec1, out int result1))
                        {
                            viewModel.FpbTestDTOs[0].Date = basedate.AddDays(result1);
                        }
                        else
                        {
                            string[] sec1 = viewModel.FechaSec1.Split('/');
                            viewModel.FpbTestDTOs[0].Date = new DateTime(Convert.ToInt32(sec1[2]), Convert.ToInt32(sec1[0]), Convert.ToInt32(sec1[1]));
                        }
                    }
                }
                catch (Exception ex)
                {

                    var resultResponse = new { status = -1, description = ex.Message + $"Fecha 1 {viewModel.FechaSec1} y Fecha 2 {viewModel.FechaSec2}" };

                    return Json(new
                    {
                        response = resultResponse
                    });
                }

                Telerik.Windows.Documents.Spreadsheet.Model.Workbook document;
                try
                {
                    document = viewModel.Workbook.ToDocument();
                }
                catch (Exception ex)
                {

                    var resultResponse = new { status = -1, description = ex.Message };

                    return Json(new
                    {
                        response = resultResponse
                    });
                }

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

                    rowCount = sheet.UsedCellRange.RowCount;
                    Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment allign = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(15, 2)].GetHorizontalAlignment().Value;

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

                FPBTestsGeneralDTO fpbTestsGeneralDTO = new()
                {
                    Capacity = viewModel.FPBReportsDTO.HeadboardReport.Capacity,
                    Comment = viewModel.Comments,
                    Creadopor = User.Identity.Name,
                    Customer = viewModel.FPBReportsDTO.HeadboardReport.Client,
                    Data = viewModel.FpbTestDTOs,
                    Fechacreacion = DateTime.Now,
                    File = pdfFile,
                    IdLoad = 0,
                    KeyTest = viewModel.ClavePrueba,
                    LanguageKey = viewModel.ClaveIdioma,
                    LoadDate = DateTime.Now,
                    Modificadopor = null,
                    Fechamodificacion = null,
                    NameFile = string.Concat("FPB", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                    Result = viewModel.IsReportAproved,
                    SerialNumber = viewModel.NoSerie,
                    TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                    TypeReport = ReportType.FPB.ToString(),
                    TypeUnit = viewModel.UnitType,
                    TanDelta = viewModel.TanDelta,
                    CantBoq = viewModel.FPBReportsDTO.NozzlesByDesignDtos.TotalQuantity,

                };

                try
                {
                    ApiResponse<long> result = await _fpbClientService.SaveReport(fpbTestsGeneralDTO);

                    var resultResponse = new { status = result.Code, description = result.Description, nameFile = fpbTestsGeneralDTO.NameFile, file = viewModel.Base64PDF };

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
            catch(Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<FpbViewModel>
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
            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            // Tipos de prueba
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.FPB.ToString(), "-1");
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

