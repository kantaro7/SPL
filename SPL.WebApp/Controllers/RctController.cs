namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
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

    public class RctController : Controller
    {

        private readonly ITestClientService _testClientService;
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly IRctClientService _rctClientService;
        private readonly IRctService _rctService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IReportClientService _reportClientService;
        private readonly IProfileSecurityService _profileClientService;
        public RctController(
            ITestClientService testClientService,
            IMasterHttpClientService masterHttpClientService,
            IArtifactClientService artifactClientService,
            IGatewayClientService gatewayClientService,
            IWebHostEnvironment hostEnvironment,
            IReportClientService reportClientService,
            IRctService rctService,
            IRctClientService rctClientService,
            IProfileSecurityService profileClientService)
        {
            _testClientService = testClientService;
            _masterHttpClientService = masterHttpClientService;
            _artifactClientService = artifactClientService;
            _rctClientService = rctClientService;
            _reportClientService = reportClientService;
            _rctService = rctService;
            _hostEnvironment = hostEnvironment;
            _gatewayClientService = gatewayClientService;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;
                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.ResistenciaÓhmicaparaelCortedeTemperatura)))
                {

                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new RctViewModel { NoSerie = noSerie });
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

        [HttpGet]
        private async Task<ApiResponse<IEnumerable<GeneralPropertiesDTO>>> GetTestAsync(InformationArtifactDTO informationArtifactDTO)
        {
            string terSecondDown = string.Empty;
            string claveIdioma = string.Empty;
            bool mvaf1 = false, mvaf2 = false, mvaf3 = false, mvaf4 = false;

            try
            {
                #region Get the artefact
                if (informationArtifactDTO is null)
                {
                    return new ApiResponse<IEnumerable<GeneralPropertiesDTO>>
                    {
                        Code = -1,
                        Description = "Artefacto no valido.",
                        Structure = null
                    };
                }
                else
                {
                    claveIdioma = informationArtifactDTO.GeneralArtifact.ClaveIdioma;
                    mvaf1 = informationArtifactDTO.CharacteristicsArtifact.All(c => c.Mvaf1.HasValue && c.Mvaf1.Value > 0);
                    mvaf2 = informationArtifactDTO.CharacteristicsArtifact.All(c => c.Mvaf2.HasValue && c.Mvaf2.Value > 0);
                    mvaf3 = informationArtifactDTO.CharacteristicsArtifact.All(c => c.Mvaf3.HasValue && c.Mvaf3.Value > 0);
                    mvaf4 = informationArtifactDTO.CharacteristicsArtifact.All(c => c.Mvaf4.HasValue && c.Mvaf4.Value > 0);
                }
                #endregion

                ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.RCT.ToString(), "-1");
                List<GeneralPropertiesDTO> reportsGP = new();

                reportResult.Structure = mvaf1 && mvaf2 && (mvaf3 || mvaf4) ? reportResult.Structure : mvaf1 && mvaf2 && !mvaf3 && !mvaf4 ? reportResult.Structure.Where(c => c.ClavePrueba is "ABI" or "ABS" or "ATI") : new List<TestsDTO>();

                foreach (TestsDTO item in reportResult.Structure)
                {
                    reportsGP.Add(new GeneralPropertiesDTO
                    {
                        Clave = item.ClavePrueba,
                        Descripcion = item.Descripcion
                    });
                }

                return new ApiResponse<IEnumerable<GeneralPropertiesDTO>>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = reportsGP
                };

            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<GeneralPropertiesDTO>>
                {
                    Code = -1,
                    Description = ex.Message,
                    Structure = null
                };
            }
        }

        public async Task<IActionResult> GetFilter(string noSerie)
        {
            RctViewModel viewModel = new();
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
                    response = new ApiResponse<RctViewModel>
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
                    response = new ApiResponse<RctViewModel>
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
                        response = new ApiResponse<RctViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no encontrado",
                            Structure = null
                        }
                    });
                }

                ApiResponse<IEnumerable<GeneralPropertiesDTO>> tests = await GetTestAsync(artifactDesing);
                if (tests.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<RctViewModel>
                        {
                            Code = -1,
                            Description = tests.Description,
                            Structure = null
                        }
                    });
                }

                viewModel.Tests = tests.Structure;

                viewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;

                bool mvaf1 = artifactDesing.CharacteristicsArtifact.Any(c => c.Mvaf1.HasValue && c.Mvaf1.Value > 0);
                bool mvaf2 = artifactDesing.CharacteristicsArtifact.Any(c => c.Mvaf2.HasValue && c.Mvaf2.Value > 0);
                bool mvaf3 = artifactDesing.CharacteristicsArtifact.Any(c => c.Mvaf3.HasValue && c.Mvaf3.Value > 0);
                bool mvaf4 = artifactDesing.CharacteristicsArtifact.Any(c => c.Mvaf4.HasValue && c.Mvaf4.Value > 0);

                viewModel.TensionTestRequired =

                    (mvaf1 && mvaf2) ||
                    (mvaf1 && mvaf3) ||
                    (mvaf1 && mvaf4) ||

                    (mvaf2 && mvaf3) ||
                    (mvaf2 && mvaf4) ||

                    (mvaf3 && mvaf4);

                bool cap2da = artifactDesing.CharacteristicsArtifact.All(x => x.Mvaf3 is not null and > 0);
                bool capTer = artifactDesing.CharacteristicsArtifact.All(x => x.Mvaf4 is not null and > 0);

                viewModel.ThirdSecondDown = cap2da && !capTer ? "LV1/LV2" : !cap2da && capTer ? "LV/TV" : string.Empty;




                viewModel.AT = new();
                viewModel.ABT = new();
                viewModel.ABTER = new();

                if (artifactDesing.VoltageKV.TensionKvAltaTension1 is not 0 and not null)
                {
                    viewModel.AT.Add(artifactDesing.VoltageKV.TensionKvAltaTension1.ToString());
                    viewModel.ABT.Add(artifactDesing.VoltageKV.TensionKvAltaTension1.ToString());
                    viewModel.ABTER.Add(artifactDesing.VoltageKV.TensionKvAltaTension1.ToString());
                }

                if (artifactDesing.VoltageKV.TensionKvAltaTension3 is not 0 and not null)
                {
                    viewModel.AT.Add(artifactDesing.VoltageKV.TensionKvAltaTension3.ToString());
                    viewModel.ABT.Add(artifactDesing.VoltageKV.TensionKvAltaTension3.ToString());
                    viewModel.ABTER.Add(artifactDesing.VoltageKV.TensionKvAltaTension3.ToString());
                }

                if (artifactDesing.VoltageKV.TensionKvBajaTension1 is not 0 and not null)
                {
                    viewModel.ABT.Add(artifactDesing.VoltageKV.TensionKvBajaTension1.ToString());
                    viewModel.ABTER.Add(artifactDesing.VoltageKV.TensionKvBajaTension1.ToString());
                }

                if (artifactDesing.VoltageKV.TensionKvBajaTension3 is not 0 and not null)
                {
                    viewModel.ABT.Add(artifactDesing.VoltageKV.TensionKvBajaTension3.ToString());
                    viewModel.ABTER.Add(artifactDesing.VoltageKV.TensionKvBajaTension3.ToString());
                }

                if (artifactDesing.VoltageKV.TensionKvTerciario1 is not 0 and not null)
                {
                    viewModel.ABTER.Add(artifactDesing.VoltageKV.TensionKvTerciario1.ToString());
                }

                if (artifactDesing.VoltageKV.TensionKvTerciario3 is not 0 and not null)
                {
                    viewModel.ABTER.Add(artifactDesing.VoltageKV.TensionKvTerciario3.ToString());
                }

                viewModel.ATR = !((artifactDesing.VoltageKV.TensionKvAltaTension1 is 0 or null) || (artifactDesing.VoltageKV.TensionKvAltaTension3 is 0 or null));
                viewModel.BTR = !((artifactDesing.VoltageKV.TensionKvBajaTension1 is 0 or null) || (artifactDesing.VoltageKV.TensionKvBajaTension3 is 0 or null));
                viewModel.TERR = !((artifactDesing.VoltageKV.TensionKvTerciario1 is 0 or null) || (artifactDesing.VoltageKV.TensionKvTerciario3 is 0 or null));



                viewModel.ThirdSecondDownRequired = cap2da || capTer;

            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _rctClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(-1))
            {
                return Json(new
                {

                    response = new ApiResponse<RctViewModel>
                    {
                        Code = -1,
                        Description = resultFilter.Description,
                        Structure = viewModel
                    }
                });
            }
            if (resultFilter.Code.Equals(1) && resultFilter.Structure.Any())
            {
                InfoGeneralTypesReportsDTO reports = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.RCT.ToString()));

                IEnumerable<IGrouping<bool?, InfoGeneralReportsDTO>> reportRadGroupStatus = reports.ListDetails.GroupBy(c => c.Resultado);

                List<TreeViewItemDTO> reportsAprooved = new();
                List<TreeViewItemDTO> reportsRejected = new();

                foreach (IGrouping<bool?, InfoGeneralReportsDTO> reportGrouped in reportRadGroupStatus)
                {
                    foreach (InfoGeneralReportsDTO item in reportGrouped)
                    {
                        if (reportGrouped.Key.Value)
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

                viewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Resistencia Óhmica para el Corte de Temperatura",
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

                response = new ApiResponse<RctViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = viewModel
                }
            });

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

        public async Task<IActionResult> ValidateFilter(string noSerie, string clavePrueba, string claveIdioma, string terciario, string unitOfMeasurement, decimal? testVoltage, string comment)
        {
            string noSerieSimple = noSerie.Split("-")[0];
            ApiResponse<PositionsDTO> resultP = await _gatewayClientService.GetPositions(noSerie);

            if (resultP.Code == -1)
            {
                return Json(new
                {
                    response = new ApiResponse<bool>
                    {
                        Code = -1,
                        Description = resultP.Description,
                        Structure = false
                    }
                });
            }

            InformationArtifactDTO artifactDesing = await _artifactClientService.GetArtifact(noSerieSimple);

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

                if (rAT)
                {
                    return Json(new
                    {
                        response = new ApiResponse<bool>
                        {
                            Code = -1,
                            Description = "EL campo tension de prueba es requerido para Alta",
                            Structure = false
                        }
                    });
                }
                if (rBT && clavePrueba != TestType.ATI.ToString())
                {
                    return Json(new
                    {
                        response = new ApiResponse<bool>
                        {
                            Code = -1,
                            Description = "EL campo tension de prueba es requerido para Baja",
                            Structure = false
                        }
                    });
                }
                if (rTER && (clavePrueba == TestType.TIS.ToString() || clavePrueba == TestType.TOI.ToString() || clavePrueba == TestType.TOS.ToString() || clavePrueba == TestType.TSI.ToString()))
                {
                    return Json(new
                    {
                        response = new ApiResponse<bool>
                        {
                            Code = -1,
                            Description = "EL campo tension de prueba es requerido para Terciario",
                            Structure = false
                        }
                    });
                }
            }

            return Json(new
            {
                response = new ApiResponse<bool>
                {
                    Code = 1,
                    Description = "",
                    Structure = true
                }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, string terciario, string unitOfMeasurement, decimal? testVoltage, string comment)
        {
            string noSerieSimple = noSerie.Split("-")[0];
            int numberColumns = clavePrueba switch
            {
                "ATI" => 1,
                "ABS" => 2,
                "ABI" => 2,
                "TSI" => 3,
                "TOS" => 3,
                "TOI" => 3,
                "TIS" => 3,
                _ => 0,
            };

            ApiResponse<PositionsDTO> resultP = await _gatewayClientService.GetPositions(noSerie);
            PositionsDTO positionsDTO = resultP.Structure;
            ApiResponse<SettingsToDisplayRCTReportsDTO> result = await _gatewayClientService.GetTemplate(noSerie, clavePrueba, claveIdioma, unitOfMeasurement, terciario, testVoltage);

            if (result.Code.Equals(-1))
            {
                return View("Excel", new RctViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayRCTReportsDTO reportInfo = result.Structure;

            #region Decode Template
            if (string.IsNullOrEmpty(reportInfo.BaseTemplate?.Plantilla))
            {
                return View("Excel", new RctViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = reportInfo.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");
            try
            {
                _rctService.PrepareTemplate_RCT(reportInfo, ref workbook, claveIdioma, numberColumns, unitOfMeasurement, testVoltage, positionsDTO, clavePrueba);
            }
            catch (Exception)
            {
                return View("Excel", new RctViewModel
                {
                    Error = "La plantilla esta mal configurada por favor verifiquela de inmediato"
                });
            }

            string acceptanceValueFas = reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valor_Acep_Fases"))?.Formato;
            #endregion
            return View("Excel", new RctViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = clavePrueba,
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                RCTReportsDTO = result.Structure,
                AcceptanceValue = Convert.ToDecimal(acceptanceValueFas),
                Workbook = workbook,
                ThirdSecondDown = terciario,
                UnitMeasuring = unitOfMeasurement,
                TensionTest = testVoltage.ToString(),
                Error = string.Empty,
                Comments = comment,
                NumberColumns = numberColumns
            });
        }

        [HttpPost]
        public async Task<IActionResult> Validate([FromBody] RctViewModel viewModel)
        {

            List<RCTTestsDTO> rctTestDTOs = new()
            {
                new RCTTestsDTO()
                {
                    AcceptanceValue = viewModel.AcceptanceValue,
                    RCTTestsDetails = new List<RCTTestsDetailsDTO>()
                },
                new RCTTestsDTO()
                {
                    AcceptanceValue = viewModel.AcceptanceValue,
                    RCTTestsDetails = new List<RCTTestsDetailsDTO>()
                },
                new RCTTestsDTO()
                {
                    AcceptanceValue = viewModel.AcceptanceValue,
                    RCTTestsDetails = new List<RCTTestsDetailsDTO>()
                }
            };

            _rctService.Prepare_RCT_Test(viewModel.RCTReportsDTO, viewModel.Workbook, ref rctTestDTOs, viewModel.NumberColumns);

            //int rows = viewModel.RODReportsDTO.TitleOfColumns.Count;
            //foreach (RODTestsDTO item in rodTestDTOs)
            //{
            //    if(item.RODTestsDetails.Count != rows || item.RODTestsDetails.Exists(x => x.Power < 0) || item.RODTestsDetails.Exists(x => x.Current < 0))
            //    {
            //        return this.Json(new
            //        {
            //            response = new ApiResponse<RodViewModel>
            //            {
            //                Code = -2,
            //                Description = "Faltan datos por ingresar en la tabla"
            //            }
            //        });
            //    }
            //}

            ApiResponse<ResultRCTTestsDTO> resultTestRCT_Response = await _rctClientService.CalculateTestRCT(rctTestDTOs);

            if (resultTestRCT_Response.Code.Equals(-1))
            {
                return Json(new
                {
                    response = new ApiResponse<RctViewModel>
                    {
                        Code = -1,
                        Description = string.Empty,
                        Structure = viewModel
                    }
                });
            }

            Workbook workbook = viewModel.Workbook;

            _rctService.PrepareIndexOfRCT(resultTestRCT_Response.Structure, viewModel.RCTReportsDTO, viewModel.ClaveIdioma, ref workbook);

            #region fillColumns
            // rctTestDTOs = resultTestRCT_Response.Structure.RCTTests.ToList();
            #endregion

            viewModel.RctTestDTOs = new List<RCTTestsDTO>();
            viewModel.Workbook = workbook;
            bool resultReport = !resultTestRCT_Response.Structure.Results.Any();
            viewModel.IsReportAproved = resultReport;
            viewModel.RctTestDTOs = rctTestDTOs;
            string errors = string.Empty;
            List<string> errorMessages = resultTestRCT_Response.Structure.Results.Select(k => k.Message).ToList();
            string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
            return Json(new
            {
                response = new ApiResponse<RctViewModel>
                {
                    Code = 1,
                    Description = allError,
                    Structure = viewModel
                }
            });
        }

        [HttpPost]

        public async Task<IActionResult> SavePDF([FromBody] RctViewModel viewModel)
        {
            #region Export Excel
            int[] filas = new int[2];

            if (!_rctService.Verify_RCT_Columns(viewModel.RCTReportsDTO, viewModel.Workbook, viewModel.NumberColumns))
            {
                return Json(new
                {
                    response = new { status = -2, description = "Faltan datos por ingresar en la tabla" }
                });
            }

            DateTime date = _rctService.GetDate(viewModel.Workbook, viewModel.RCTReportsDTO);
            string measurementType = _rctService.GetMeasurementType(viewModel.Workbook, viewModel.RCTReportsDTO);

            Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

            PdfFormatProvider provider = new()
            {
                ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
            };
            document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.Workbook.ActiveSheet);
            FloatingImage image = null;
            int rowCount = 0; foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
            {
                #region FormatPDF
                int[] _positionWB;
                IEnumerable<ConfigurationReportsDTO> starts = viewModel.RCTReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Resistencia"));
                foreach (ConfigurationReportsDTO item in starts)
                {
                    for (int i = 0; i < viewModel.NumberColumns; i++)
                    {
                        _positionWB = GetRowColOfWorbook(item.Celda);
                        string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1] + i)].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1] + i)].SetValueAsText(FormatStringDecimal(val, 6));
                    }
                }

                starts = viewModel.RCTReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura"));
                foreach (ConfigurationReportsDTO item in starts)
                {
                    for (int i = 0; i < viewModel.NumberColumns; i++)
                    {
                        _positionWB = GetRowColOfWorbook(item.Celda);
                        string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1] + i)].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1] + i)].SetValueAsText(FormatStringDecimal(val, 1));
                    }
                }

                #endregion
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
            #endregion

            #region Save Report
            RCTTestsGeneralDTO rctTestsGeneralDTO = new()
            {
                Capacity = viewModel.RCTReportsDTO.HeadboardReport.Capacity,
                Comment = viewModel.Comments,
                Creadopor = User.Identity.Name,
                Customer = viewModel.RCTReportsDTO.HeadboardReport.Client,
                Fechacreacion = DateTime.Now,
                File = pdfFile,
                MeasurementType = measurementType,
                RCTTests = viewModel.RctTestDTOs,
                TensionTest = (viewModel.TensionTest != null && viewModel.TensionTest != "") ? Convert.ToDecimal(viewModel.TensionTest) : 0,
                Ter2low = (viewModel.TensionTest != null && viewModel.TensionTest != "") ? viewModel.ThirdSecondDown : " ",
                UnitMeasure = viewModel.UnitMeasuring,
                IdLoad = 0,
                KeyTest = viewModel.ClavePrueba,
                LanguageKey = viewModel.ClaveIdioma,
                LoadDate = date,
                Modificadopor = null,
                Fechamodificacion = null,
                NameFile = string.Concat("RCT", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba),"-" + rowCount, ".pdf"),
                Result = viewModel.IsReportAproved,
                SerialNumber = viewModel.NoSerie,
                TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                TypeReport = ReportType.RCT.ToString(),
            };
            #endregion

            try
            {
                ApiResponse<long> result = await _rctClientService.SaveReport(rctTestsGeneralDTO);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = rctTestsGeneralDTO.NameFile, file = rctTestsGeneralDTO.File };

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

        private async Task PrepareForm()
        {
            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            List<SelectListItem> measuringList = new()
            {
                new SelectListItem()
                {
                    Selected = true,
                    Text = "Seleccione...",
                    Value = string.Empty
                }
            };

            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            foreach (int item in Enum.GetValues(typeof(MeasuringResistance)))
            {
                measuringList.Add(new SelectListItem()
                {
                    Text = $"{myTI.ToTitleCase(Enum.GetName(typeof(MeasuringResistance), item).ToLower())}",
                    Value = $"{myTI.ToTitleCase(Enum.GetName(typeof(MeasuringResistance), item).ToLower())}"
                });
            }

            ViewBag.UnitMeasuringItems = measuringList;

            ViewBag.TestItems = new SelectList(origingeneralProperties, "Clave", "Descripcion");

            List<SelectListItem> tercSecDownItems = new()
            {
                new SelectListItem()
                {
                    Selected = true,
                    Text = "Seleccione...",
                    Value = string.Empty
                }
            };

            ViewBag.TestConnectionItems = tercSecDownItems;

            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");

            List<SelectListItem> thirSecList = new()
            {
                new SelectListItem()
                {
                    Selected = true,
                    Text = "Seleccione...",
                    Value = string.Empty
                },
                new SelectListItem()
                {
                    Selected = false,
                    Text = "LV1/LV2",
                    Value = "LV1/LV2"
                },
                new SelectListItem()
                {
                    Selected = false,
                    Text = "LV/TV",
                    Value = "LV/TV"
                }
            };

            ViewBag.thirSecItems = thirSecList;

            List<SelectListItem> tensionsTest = new()
            {
                new SelectListItem()
                {
                    Selected = true,
                    Text = "Seleccione...",
                    Value = string.Empty
                }
            };

            ViewBag.TensionTestItems = tensionsTest;
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
                        num += "." + formDecimals((decimals));
                    }
                }
            }
            return num;
        }
        private string formDecimals(int decimals)
        {
            string result = "";
            for(int i = 0; i < decimals; i++)
            {
                result += "0";
            }
            return result;
        }
    }
}
