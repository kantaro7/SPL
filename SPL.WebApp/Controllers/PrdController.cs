namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Data;
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.ViewModels;

    using Telerik.Web.Spreadsheet;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export;
    using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

    using Microsoft.AspNetCore.Hosting;
    using Newtonsoft.Json;
    using SPL.WebApp.Helpers;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using Microsoft.Identity.Web;

    /// <summary>
    /// Relación de Transformación
    /// </summary>
    public class PrdController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IPrdClientService _prdClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IPrdService _prdService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IResistanceTwentyDegreesClientServices _resistanceTwentyDegreesClientServices;
        private readonly IProfileSecurityService _profileClientService;
        public PrdController(
            IMasterHttpClientService masterHttpClientService,
            IPrdClientService prdClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IPrdService prdService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            ISidcoClientService sidcoClientService,
            IResistanceTwentyDegreesClientServices resistanceTwentyDegreesClientServices,
            IProfileSecurityService profileClientService
            )
        {
            this._masterHttpClientService = masterHttpClientService;
            this._prdClientService = prdClientService;
            this._reportClientService = reportClientService;
            this._artifactClientService = artifactClientService;
            this._testClientService = testClientService;
            this._gatewayClientService = gatewayClientService;
            this._prdService = prdService;
            this._correctionFactorService = correctionFactorService;
            this._hostEnvironment = hostEnvironment;
            this._sidcoClientService = sidcoClientService;
            this._resistanceTwentyDegreesClientServices = resistanceTwentyDegreesClientServices;
            this._profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(User.Identity.Name).Result;


                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.PérdidasTotalesenReactoresdeDerivación)))
                {

                    await this.PrepareForm();
                    var noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return this.View(new PrdViewModel { NoSerie = noSerie });
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
            PrdViewModel prdViewModel = new();
            noSerie = noSerie.ToUpper().Trim();
            string noSerieSimple = string.Empty;

            if (!string.IsNullOrEmpty(noSerie))
            {
                noSerieSimple = noSerie.Split("-")[0];
            }
            else
            {
                return this.Json(new
                {
                    response = new ApiResponse<PrdViewModel>
                    {
                        Code = -1,
                        Description = "No. Serie inválido.",
                        Structure = null
                    }
                });
            }

            bool isFromSPL = await this._artifactClientService.CheckNumberOrder(noSerieSimple);

            if (!isFromSPL)
            {
                return this.Json(new
                {
                    response = new ApiResponse<PrdViewModel>
                    {
                        Code = -1,
                        Description = "Artefacto no presente en SPL",
                        Structure = null
                    }
                });
            }
            else
            {
                InformationArtifactDTO artifactDesing = await this._artifactClientService.GetArtifact(noSerieSimple);
                if (artifactDesing.GeneralArtifact.OrderCode == null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PrdViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no encontrado",
                            Structure = null
                        }
                    });
                }

                if (artifactDesing.GeneralArtifact is null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PrdViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee información general",
                            Structure = null
                        }
                    });
                }

                // CAR
                if (artifactDesing.VoltageKV is null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PrdViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee voltages",
                            Structure = null
                        }
                    });
                }

                prdViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await this._prdClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.PRD.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.PRD.ToString() };
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

                prdViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Pérdidas Totales en Reactores de Derivación",
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

            return this.Json(new
            {
                response = new ApiResponse<PrdViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = prdViewModel
                }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetPDFReport(long code, string typeReport)
        {
            ApiResponse<ReportPDFDto> result = await this._reportClientService.GetPDFReport(code, typeReport);

            if (result.Code.Equals(-1))
            {
                return this.Json(new
                {
                    response = result
                });
            }

            byte[] bytes = Convert.FromBase64String(result.Structure.File);
            _ = new MemoryStream(bytes);

            return this.Json(new
            {
                data = bytes,
            });
        }

        public async Task<IActionResult> ValidateFilter(string noSerie, string clavePrueba, string claveIdioma, decimal nominalVoltage, string comment)
        {
            noSerie = noSerie.ToUpper().Trim();
            string noSerieSimple = string.Empty;

            if (!string.IsNullOrEmpty(noSerie))
            {
                noSerieSimple = noSerie.Split("-")[0];
            }
            else
            {
                return this.Json(new
                {
                    response = new ApiResponse<bool>
                    {
                        Code = -1,
                        Description = "No. Serie inválido.",
                        Structure = false
                    }
                });
            }

            InformationArtifactDTO artifactDesing = await this._artifactClientService.GetArtifact(noSerieSimple);

            List<decimal> voltages = new();

            if (artifactDesing.VoltageKV.TensionKvAltaTension1 is not null)
            {
                voltages.Add(artifactDesing.VoltageKV.TensionKvAltaTension1 ?? 0);
            }

            if (artifactDesing.VoltageKV.TensionKvBajaTension1 is not null)
            {
                voltages.Add(artifactDesing.VoltageKV.TensionKvBajaTension1 ?? 0);
            }

            if (artifactDesing.VoltageKV.TensionKvSegundaBaja1 is not null)
            {
                voltages.Add(artifactDesing.VoltageKV.TensionKvSegundaBaja1 ?? 0);
            }

            if (artifactDesing.VoltageKV.TensionKvTerciario1 is not null)
            {
                voltages.Add(artifactDesing.VoltageKV.TensionKvTerciario1 ?? 0);
            }

            if (voltages.Any())
            {
                if (voltages.Where(x => decimal.Round(x, 3) == decimal.Round(nominalVoltage, 3)).Count() == 0)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<bool>
                        {
                            Code = -1,
                            Description = "Voltage Nominal debe pertenecer tener uno de estos valores: " + string.Join('-', voltages),
                            Structure = false
                        }
                    });
                }
            }

            return this.Json(new
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
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, decimal nominalVoltage, string comment)
        {
            noSerie = noSerie.ToUpper().Trim();

            ApiResponse<SettingsToDisplayPRDReportsDTO> result = await this._gatewayClientService.GetTemplate(noSerie, clavePrueba, claveIdioma, nominalVoltage);

            if (result.Code.Equals(-1))
            {
                return this.View("Excel", new PrdViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayPRDReportsDTO reportInfo = result.Structure;

            #region Decode Template
            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return this.View("Excel", new PrdViewModel
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
                this._prdService.PrepareTemplate_PRD(reportInfo, ref workbook);
            }
            catch (Exception)
            {
                return this.View("Excel", new PrdViewModel
                {
                    Error = "La plantilla esta mal configurada por favor verifiquela de inmediato"
                });
            }
            #endregion

            IEnumerable<string> celdas = result.Structure.ConfigurationReports.Where(x => x.Dato.Contains("Imagen")).Select(x => x.Celda);

            IEnumerable<string> nombres = result.Structure.ConfigurationReports.Where(x => x.Dato.Contains("Imagen")).Select(x => x.Formato);

            return this.View("Excel", new PrdViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = clavePrueba,
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                PRDReportsDTO = result.Structure,
                Workbook = workbook,
                NominalVoltage = nominalVoltage,
                Celdas = celdas.ToList(),
                Nombres = nombres.ToList(),
                Error = string.Empty,
                Comments = comment
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] PrdViewModel viewModel)
        {

            PRDTestsDTO prdTestDTO = new()
            {
                NominalVoltage = viewModel.NominalVoltage,
                KeyTest = viewModel.ClavePrueba,
                PRDTestsDetails = new PRDTestsDetailsDTO()
            };

            this._prdService.Prepare_PRD_Test(viewModel.PRDReportsDTO, viewModel.Workbook, ref prdTestDTO);

            if (!_prdService.Verify_PRD_ColumnsToCalculate(viewModel.PRDReportsDTO, viewModel.Workbook))
            {
                return this.Json(new
                {
                    response = new ApiResponse<PrdViewModel>
                    {
                        Code = -1,
                        Description = "Faltan datos por ingresar en la tabla",
                        Structure = viewModel
                    }
                });
            }

            ApiResponse<ResultPRDTestsDTO> resultTestPRD_Response = await this._prdClientService.CalculateTestPRD(prdTestDTO);

            if (resultTestPRD_Response.Code.Equals(-1))
            {
                return this.Json(new
                {
                    response = new ApiResponse<PrdViewModel>
                    {
                        Code = -1,
                        Description = string.Empty,
                        Structure = viewModel
                    }
                });
            }

            Workbook workbook = viewModel.Workbook;

            this._prdService.PrepareIndexOfPRD(resultTestPRD_Response.Structure, viewModel.PRDReportsDTO, ref workbook);

            #region fillColumns
            prdTestDTO = resultTestPRD_Response.Structure.PRDTests;
            #endregion

            viewModel.PrdTestDTO = prdTestDTO;
            viewModel.Workbook = workbook;
            viewModel.OfficialWorkbook = workbook;
            bool resultReport = !resultTestPRD_Response.Structure.Results.Any();
            viewModel.IsReportAproved = resultReport;

            string errors = string.Empty;
            List<string> errorMessages = resultTestPRD_Response.Structure.Results.Select(k => k.Message).ToList();
            string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
            return this.Json(new
            {
                response = new ApiResponse<PrdViewModel>
                {
                    Code = 1,
                    Description = allError,
                    Structure = viewModel
                }
            });
        }

        [HttpPost]

        public async Task<IActionResult> SavePDF([FromBody] PrdViewModel viewModel)
        {
            #region Export Excel
            int[] filas = new int[2];
            int[] _positionWB;

            if (!_prdService.Verify_PRD_Columns(viewModel.PRDReportsDTO, viewModel.Workbook))
            {
                return this.Json(new
                {
                    response = new { status = -2, description = "Faltan datos por ingresar en la tabla" }
                });
            }

            DateTime date = this._prdService.GetDate(viewModel.Workbook, viewModel.PRDReportsDTO);

            Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

            PdfFormatProvider provider = new()
            {
                ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
            };
            document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.OfficialWorkbook.ActiveSheet);
            FloatingImage image = null;
            int rowCount = 0;  foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
            {
                #region FormatPDF
                #region DataFill

                // Cn
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cn")).Celda);
                string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                // sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 14));

                // M3
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("M3")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                // sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 7));

                // C4
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("C4")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                // sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 8));

                // Vm
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Vm")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 0));

                // U
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("U")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 2));

                // Tmp
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tmp")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 2));

                // R4s
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("R4s")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 2));

                // Im
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Im")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 2));

                // Cap
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Cap")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 0));

                // Tm
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tm")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 2));

                // Rm
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Rm")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 4));

                // Tr
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tr")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 2));

                // Pfe
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pfe")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 0));

                // GARANTIA
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Garantia")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 0));
                #endregion

                #region DataCalculate
                // V
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("V")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 0));

                // I
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("I")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 3));

                // Lxp
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Lxp")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 2));

                // Rxp
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Rxp")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 3));

                // P
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("P")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 0));

                // Xm
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Xm")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 2));

                // Xc
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Xc")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 2));

                // Porc_Desv
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Porc_Desv")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 2));

                // Pjm
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pjm")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 0));

                // Fc
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fc")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 5));

                // Pjmc
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pjmc")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 0));

                // Pe
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pe")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 0));

                // Fc2
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fc2")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 3));

                // Pt
                _positionWB = this.GetRowColOfWorbook(viewModel.PRDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pt")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 0));

                #endregion

                #endregion
                #region Imagenes
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
                IEnumerable<string> celdas = viewModel.PRDReportsDTO.ConfigurationReports.Where(x => x.Dato.Contains("Imagen")).Select(x => x.Celda);

                IEnumerable<string> nombres = viewModel.PRDReportsDTO.ConfigurationReports.Where(x => x.Dato.Contains("Imagen")).Select(x => x.Formato);

                for (int i = 0; i < celdas.Count(); i++)
                {
                    if (nombres.ElementAt(i).Contains("ImgU") || nombres.ElementAt(i).Contains("ImgR4s") || nombres.ElementAt(i).Contains("ImgReactLineal") || nombres.ElementAt(i).Contains("ImgReactancia"))
                    {
                        _positionWB = this.GetRowColOfWorbook(celdas.ElementAt(i));
                        image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1]), 0, 0);
                        path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", nombres.ElementAt(i));
                        stream = new(path, FileMode.Open);
                        using (stream)
                        {
                            image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                        }

                        image.Width = 30;
                        image.Height = 30;

                        sheet.Shapes.Add(image);
                    }
                    else if (nombres.ElementAt(i).Contains("ImgPjm") || nombres.ElementAt(i).Contains("ImgPjmc") || nombres.ElementAt(i).Contains("ImgPt") || nombres.ElementAt(i).Contains("ImgPe"))
                    {
                        _positionWB = this.GetRowColOfWorbook(celdas.ElementAt(i));
                        image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1]), 0, 0);
                        path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", nombres.ElementAt(i));
                        stream = new(path, FileMode.Open);
                        using (stream)
                        {
                            image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                        }

                        image.Width = 160;
                        image.Height = 33;

                        sheet.Shapes.Add(image);
                    }
                    else if (nombres.ElementAt(i).Contains("ImgP") || nombres.ElementAt(i).Contains("ImgXc") || nombres.ElementAt(i).Contains("ImgXm"))
                    {
                        _positionWB = this.GetRowColOfWorbook(celdas.ElementAt(i));
                        image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1]), 0, 0);
                        path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", nombres.ElementAt(i));
                        stream = new(path, FileMode.Open);
                        using (stream)
                        {
                            image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                        }

                        image.Width = 55;
                        image.Height = 33;

                        sheet.Shapes.Add(image);
                    }
                    else
                    {
                        _positionWB = this.GetRowColOfWorbook(celdas.ElementAt(i));
                        image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1]), 0, 0);
                        path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", nombres.ElementAt(i));
                        stream = new(path, FileMode.Open);
                        using (stream)
                        {
                            image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                        }

                        image.Width = 120;
                        image.Height = 30;

                        sheet.Shapes.Add(image);
                    }
                }
                #endregion
                rowCount = sheet.UsedCellRange.RowCount;
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
            PRDTestsGeneralDTO prdTestsGeneralDTO = new()
            {
                Capacity = viewModel.PRDReportsDTO.HeadboardReport.Capacity,
                Comment = viewModel.Comments,
                Creadopor = User.Identity.Name,
                Customer = viewModel.PRDReportsDTO.HeadboardReport.Client,
                PRDTests = viewModel.PrdTestDTO,
                Fechacreacion = DateTime.Now,
                File = pdfFile,
                IdLoad = 0,
                KeyTest = viewModel.ClavePrueba,
                LanguageKey = viewModel.ClaveIdioma,
                LoadDate = DateTime.Now,
                Modificadopor = string.Empty,
                NameFile = string.Concat("PRD", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                Result = viewModel.IsReportAproved,
                SerialNumber = viewModel.NoSerie,
                TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                TypeReport = ReportType.PRD.ToString()

            };
            #endregion

            try
            {
                string a = JsonConvert.SerializeObject(prdTestsGeneralDTO);
                ApiResponse<long> result = await this._prdClientService.SaveReport(prdTestsGeneralDTO);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = prdTestsGeneralDTO.NameFile, file = prdTestsGeneralDTO.File };

                return this.Json(new
                {
                    response = resultResponse
                });
            }
            catch (Exception ex)
            {
                return this.Json(new
                {
                    response = new { status = -1, description = ex.Message, nameFile = "", file = "" }
                });
            }
        }
        public IActionResult Error() => this.View();

        #region PrivateMethods
        public async Task PrepareForm()
        {
            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            // Tipos de prueba
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await this._testClientService.GetTest(ReportType.PRD.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            this.ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");

            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            this.ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");
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
        #endregion
    }
}

