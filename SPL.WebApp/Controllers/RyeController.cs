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

    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export;
    using Telerik.Web.Spreadsheet;
    using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
    using Microsoft.AspNetCore.Hosting;
    using Newtonsoft.Json;
    using Telerik.Documents.Primitives;
    using SPL.WebApp.Helpers;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
    using Microsoft.Identity.Web;

    /// <summary>
    /// Relación de Transformación
    /// </summary>
    public class RyeController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IRyeClientService _ryeClientService;
        private readonly IPceClientService _pceClientService;
        private readonly IPciClientService _pciClientService;
        private readonly IPeeClientService _peeClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IRyeService _ryeService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IResistanceTwentyDegreesClientServices _resistanceTwentyDegreesClientServices;

        private readonly IProfileSecurityService _profileClientService;

        public RyeController(
            IMasterHttpClientService masterHttpClientService,
            IRyeClientService ryeClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IRyeService ryeService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            ISidcoClientService sidcoClientService,
            IResistanceTwentyDegreesClientServices resistanceTwentyDegreesClientServices,
            IPceClientService pceClientService,
            IPciClientService pciClientService,
            IPeeClientService peeClientService,
            IProfileSecurityService profileClientService
            )
        {
            this._masterHttpClientService = masterHttpClientService;
            this._ryeClientService = ryeClientService;
            this._reportClientService = reportClientService;
            this._artifactClientService = artifactClientService;
            this._testClientService = testClientService;
            this._gatewayClientService = gatewayClientService;
            this._ryeService = ryeService;
            this._correctionFactorService = correctionFactorService;
            this._hostEnvironment = hostEnvironment;
            this._sidcoClientService = sidcoClientService;
            this._resistanceTwentyDegreesClientServices = resistanceTwentyDegreesClientServices;
            this._pceClientService = pceClientService;
            this._pciClientService = pciClientService;
            this._peeClientService = peeClientService;
            this._profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(User.Identity.Name).Result;


                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.RegulaciónyEficiencia)))
                {


                    await this.PrepareForm();
                    var noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return this.View(new RyeViewModel { NoSerie = noSerie });
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
            RyeViewModel ryeViewModel = new();
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
                    response = new ApiResponse<RyeViewModel>
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
                    response = new ApiResponse<RyeViewModel>
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
                        response = new ApiResponse<RyeViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no encontrado",
                            Structure = null
                        }
                    });
                }
                //DG
                if (artifactDesing.GeneralArtifact is null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<RyeViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee información general",
                            Structure = null
                        }
                    });
                }

                // CAP
                if (artifactDesing.CharacteristicsArtifact.Count == 0)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee características",
                            Structure = null
                        }
                    });
                }

                // CAR
                if (artifactDesing.VoltageKV is null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee voltages",
                            Structure = null
                        }
                    });
                }

                // GAR
                if (artifactDesing.WarrantiesArtifact is null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee garantias",
                            Structure = null
                        }
                    });
                }

                ryeViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;

                ryeViewModel.ATConnectionsAmount = artifactDesing.VoltageKV.TensionKvAltaTension1 is not null and not 0
                    ? artifactDesing.VoltageKV.TensionKvAltaTension3 is not null and not 0 ? 2 : 1
                    : 0;
                ryeViewModel.BTConnectionsAmount = artifactDesing.VoltageKV.TensionKvBajaTension1 is not null and not 0
                    ? artifactDesing.VoltageKV.TensionKvBajaTension3 is not null and not 0 ? 2 : 1
                    : 0;
                ryeViewModel.TerConnectionsAmount = artifactDesing.VoltageKV.TensionKvTerciario1 is not null and not 0
                    ? artifactDesing.VoltageKV.TensionKvTerciario3 is not null and not 0 ? 2 : 1
                    : 0;

                ryeViewModel.ATTestVoltage = artifactDesing.VoltageKV.TensionKvAltaTension1 is not null and not 0
                    ? artifactDesing.VoltageKV.TensionKvAltaTension3 is not null and not 0 ? null : artifactDesing.VoltageKV.TensionKvAltaTension1 ?? null
                    : null;
                ryeViewModel.BTTestVoltage = artifactDesing.VoltageKV.TensionKvBajaTension1 is not null and not 0
                   ? artifactDesing.VoltageKV.TensionKvBajaTension3 is not null and not 0 ? null : artifactDesing.VoltageKV.TensionKvBajaTension1 ?? null
                   : null;
                ryeViewModel.TerTestVoltage = artifactDesing.VoltageKV.TensionKvTerciario1 is not null and not 0
                   ? artifactDesing.VoltageKV.TensionKvTerciario3 is not null and not 0 ? null : artifactDesing.VoltageKV.TensionKvTerciario1 ?? null
                   : null;

                ryeViewModel.CoolingTypes = artifactDesing.CharacteristicsArtifact.Select(x => $"{x.CoolingType}-{x.OverElevation}").ToList();
            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await this._ryeClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.RYE.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.RYE.ToString() };
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

                ryeViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Pérdidas del Equipo de Enfriamiento",
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
                response = new ApiResponse<RyeViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = ryeViewModel
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
        
        [HttpGet]
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, int aTConnectionsAmount, int bTConnectionsAmount, int terConnectionsAmount, decimal aTTestVoltage, decimal bTTestVoltage, decimal terTestVoltage, string coolingType, string comment)
        {
            noSerie = noSerie.ToUpper().Trim();

            ApiResponse<SettingsToDisplayRYEReportsDTO> result = await this._gatewayClientService.GetTemplate(noSerie, clavePrueba, claveIdioma, aTConnectionsAmount, bTConnectionsAmount, terConnectionsAmount, aTTestVoltage, bTTestVoltage, terTestVoltage, coolingType);

            if (result.Code.Equals(-1))
            {
                return this.View("Excel", new RyeViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayRYEReportsDTO reportInfo = result.Structure;

            #region Decode Template
            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return this.View("Excel", new RyeViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }
            ApiResponse<PositionsDTO> resultPositions = await this._gatewayClientService.GetPositions(noSerie);

            if (resultPositions.Code.Equals(-1))
            {
                return this.View("Excel", new RyeViewModel
                {
                    Error = resultPositions.Description
                });
            }

            ApiResponse<PCETestsGeneralDTO> resultPCE = await this._pceClientService.GetInfoReportPCE(noSerie, TestType.SAN.ToString(), true);

            // Perdidas en Vacio
            decimal lostCorr20 = 0;

            if (resultPCE.Structure is not null)
            {
                PCETestsDTO seccion = resultPCE.Structure.Data.FirstOrDefault(x => x.PosPruebaAT.Equals(resultPositions.Structure.ATNom) && x.PosPruebaBT.Equals(resultPositions.Structure.BTNom));

                if (seccion is not null)
                {
                    PCETestsDetailsDTO row100 = seccion.PCETestsDetails.FirstOrDefault(x => x.PorcentajeVN == 100);

                    if (row100 is not null)
                    {
                        lostCorr20 = row100.Corregidas20KW;
                    }
                }
            }
            
            // Perdidas del Equipo de Enfriamient
            ApiResponse<PEETestsGeneralDTO> resultPEE = await this._peeClientService.GetInfoReportPEE(noSerie, TestType.ALL.ToString(), true);

            decimal potenciaKW = 0;

            if (resultPEE.Structure is not null)
            {
                PEETestsDetailsDTO detail = resultPEE.Structure.PEETests.PEETestsDetails.FirstOrDefault(x => x.CoolingType.Equals(coolingType.Split('-')[0]));
                if (detail is not null)
                {
                    potenciaKW = detail.PowerKW;
                }
            }

            //// Pérdidas debidas a la carga e impedancia
            //ApiResponse<PCITestGeneralDTO> resultPCI = await this._pciClientService.GetInfoReportPCI(noSerie, TestType.AYB.ToString(), true);

            decimal porZ = 0;
            decimal lostCorr = 0;

            //if (resultPCI.Structure is not null)
            //{
            //    if(resultPCI.Structure.PCIOutTests.BaseRating == Convert.ToDecimal(reportInfo.CoolingCapacity))
            //    {
            //        PCITestsDetailsDTO detail = resultPCI.Structure.PCIOutTests.PCITests.FirstOrDefault(x => x.ValueTapPositions == resultPositions.Structure.ATNom || x.ValueTapPositions == resultPositions.Structure.BTNom).PCITestsDetails.FirstOrDefault(x => x.Position == resultPositions.Structure.ATNom || x.Position == resultPositions.Structure.BTNom);

            //        if (detail is not null)
            //        {
            //            porZ = detail.PorcentajeZ;
            //            lostCorr = detail.LossesCorrected;
            //        }
            //    }
            //}

            long NoPrueba = reportInfo.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");

            reportInfo.CoolingType = coolingType;

            try
            {
                this._ryeService.PrepareTemplate_RYE(reportInfo, ref workbook, lostCorr20, potenciaKW, porZ, lostCorr);
            }
            catch (Exception)
            {
                return this.View("Excel", new RyeViewModel
                {
                    Error = "La plantilla esta mal configurada por favor verifiquela de inmediato"
                });
            }
            #endregion

            return this.View("Excel", new RyeViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = clavePrueba,
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                RYEReportsDTO = result.Structure,
                Workbook = workbook,
                ATConnectionsAmount = aTConnectionsAmount,
                ATTestVoltage = aTTestVoltage,
                CoolingType = coolingType,
                BTConnectionsAmount = bTConnectionsAmount,
                BTTestVoltage = bTTestVoltage,
                TerConnectionsAmount = terConnectionsAmount,
                TerTestVoltage = terTestVoltage,
                Comments = comment,
                Error = string.Empty
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] RyeViewModel viewModel)
        {
            try
            {
                OutRYETestsDTO ryeTestDTO = new()
                {
                    RYETestsDetails = new List<RYETestsDetailsDTO>()
                };

                if (!_ryeService.Verify_RYE_ColumnsToCalculate(viewModel.RYEReportsDTO, viewModel.Workbook))
                {
                    return this.Json(new
                    {
                        response = new { Code = -1, Description = "Faltan datos por ingresar en la tabla" }
                    });
                }

                this._ryeService.Prepare_RYE_Test(viewModel.RYEReportsDTO, viewModel.Workbook, ref ryeTestDTO);

                ApiResponse<ResultRYETestsDTO> resultTestRYE_Response = await this._ryeClientService.CalculateTestRYE(ryeTestDTO);

                if (resultTestRYE_Response.Code.Equals(-1))
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<RyeViewModel>
                        {
                            Code = -1,
                            Description = resultTestRYE_Response.Description,
                            Structure = viewModel
                        }
                    });
                }

                Workbook workbook = viewModel.Workbook;

                this._ryeService.PrepareIndexOfRYE(resultTestRYE_Response.Structure, viewModel.RYEReportsDTO, ref workbook);

                #region fillColumns
                ryeTestDTO = resultTestRYE_Response.Structure.RYETests;
                #endregion

                viewModel.RyeTestDTO = ryeTestDTO;
                viewModel.Workbook = workbook;
                bool resultReport = !resultTestRYE_Response.Structure.Results.Any();
                viewModel.IsReportAproved = resultReport;

                string errors = string.Empty;
                List<string> errorMessages = resultTestRYE_Response.Structure.Results.Select(k => k.Message).ToList();
                string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
                return this.Json(new
                {
                    response = new ApiResponse<RyeViewModel>
                    {
                        Code = 1,
                        Description = allError,
                        Structure = viewModel
                    }
                });
            }
            catch(Exception e)
            {
                return this.Json(new
                {
                    response = new ApiResponse<RyeViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }
            
        }

        [HttpPost]
        
        public async Task<IActionResult> SavePDF([FromBody] RyeViewModel viewModel)
        {
            #region Export Excel
            int[] filas = new int[2];
            int[] _positionWB;

            if (!_ryeService.Verify_RYE_Columns(viewModel.RYEReportsDTO, viewModel.Workbook))
            {
                return this.Json(new
                {
                    response = new { status = -2, description = "Faltan datos por ingresar en la tabla" }
                });
            }

            DateTime date = this._ryeService.GetDate(viewModel.Workbook, viewModel.RYEReportsDTO);

            viewModel.RyeTestDTO.Date = date;

            Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

            PdfFormatProvider provider = new()
            {
                ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
            };
            document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.Workbook.ActiveSheet);
            FloatingImage image = null;
            string val;
            int rowCount = 0;  foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
            {
                #region FormatPDF

                // CapacidadEnf
                _positionWB = this.GetRowColOfWorbook(viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("CapacidadEnf")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 0));

                // PorcZ
                _positionWB = this.GetRowColOfWorbook(viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcZ")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 3));

                // PerdidasVacio
                _positionWB = this.GetRowColOfWorbook(viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidasVacio")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 3));

                // PerdidasCarga
                _positionWB = this.GetRowColOfWorbook(viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidasCarga")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 3));

                // PerdidasEnf
                _positionWB = this.GetRowColOfWorbook(viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidasEnf")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 3));

                // PerdidasTotales
                _positionWB = this.GetRowColOfWorbook(viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PerdidasTotales")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 3));

                // PorcZ2
                _positionWB = this.GetRowColOfWorbook(viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Porc_Z")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 4));

                // PorcR
                _positionWB = this.GetRowColOfWorbook(viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcR")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 4));

                // PorcX
                _positionWB = this.GetRowColOfWorbook(viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcX")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 4));

                // XEntreR
                _positionWB = this.GetRowColOfWorbook(viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("XEntreR")).Celda);
                val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 4));

                // PorcReg
                _positionWB = this.GetRowColOfWorbook(viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PorcReg")).Celda);
                for (int i = 0; i < 7; i++)
                {
                    val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1] + i)].GetValue().Value.RawValue;
                    sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1] + i)].SetValueAsText(this.FormatStringDecimal(val, 4));
                }
                // FACTOR DE POTENCIA
                _positionWB = this.GetRowColOfWorbook(viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Eficiencia")).Celda);

                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 7; col++)
                    {
                        val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + col)].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + col)].SetValueAsText(this.FormatStringDecimal(val, 4));
                    }
                }
                #endregion
                rowCount = sheet.UsedCellRange.RowCount;
                //image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(0, 0), 0, 0);
                //string path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
                //FileStream stream = new(path, FileMode.Open);
                //using (stream)
                //{
                //    image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                //}




                for (int i = 1; i <= viewModel.RYEReportsDTO.BaseTemplate.ColumnasConfigurables; i++)
                {

                    string celda = viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("LogoProlec") && c.Seccion == i).Celda;
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

                    string celdapage = viewModel.RYEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado") && c.Seccion == i).Celda;
                    int posicionpage = Convert.ToInt32(celdapage.Remove(0, 1)) - 1;

                    PageBreaks pageBreaks = sheet.WorksheetPageSetup.PageBreaks;

                    _ = pageBreaks.TryInsertHorizontalPageBreak(posicionpage + 9, 0);

                }







                image.Width = 215;
                image.Height = 38;
                sheet.WorksheetPageSetup.PaperType = Telerik.Windows.Documents.Model.PaperTypes.A4;
                sheet.WorksheetPageSetup.PageOrientation = Telerik.Windows.Documents.Model.PageOrientation.Portrait;
                sheet.WorksheetPageSetup.CenterHorizontally = true;
                sheet.WorksheetPageSetup.PrintOptions.PrintGridlines = false;
                sheet.WorksheetPageSetup.ScaleFactor = new Telerik.Documents.Primitives.Size(0.9, 0.9);
                sheet.WorksheetPageSetup.Margins =
                      new Telerik.Windows.Documents.Spreadsheet.Model.Printing.PageMargins(0, 20, 0, 20);
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

            RYETestsGeneralDTO ryeTestsGeneralDTO = new()
            {
                Capacity = viewModel.RYEReportsDTO.HeadboardReport.Capacity,
                Comment = viewModel.Comments,
                Creadopor = User.Identity.Name,
                Customer = viewModel.RYEReportsDTO.HeadboardReport.Client,
                Data = viewModel.RyeTestDTO,
                Fechacreacion = DateTime.Now,
                File = pdfFile,
                IdLoad = 0,
                KeyTest = viewModel.ClavePrueba,
                LanguageKey = viewModel.ClaveIdioma,
                LoadDate = DateTime.Now,
                Modificadopor = string.Empty,
                NameFile = string.Concat("RYE", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                Result = viewModel.IsReportAproved,
                SerialNumber = viewModel.NoSerie,
                TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                TypeReport = ReportType.RYE.ToString(),
                CoolingType = viewModel.CoolingType,
                NoConnectiosAT = viewModel.ATConnectionsAmount,
                NoConnectiosBT = viewModel.BTConnectionsAmount,
                NoConnectiosTER = viewModel.TerConnectionsAmount,
                VoltageAT = viewModel.ATTestVoltage == null ? 0 : (decimal)viewModel.ATTestVoltage,
                VoltageBT =  viewModel.BTTestVoltage == null ? 0 : (decimal)viewModel.BTTestVoltage,
                VoltageTER =   viewModel.TerTestVoltage == null ? 0 : (decimal)viewModel.TerTestVoltage
            };
            #endregion

            try
            {
                string a = JsonConvert.SerializeObject(ryeTestsGeneralDTO);
                ApiResponse<long> result = await this._ryeClientService.SaveReport(ryeTestsGeneralDTO);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = ryeTestsGeneralDTO.NameFile, file = ryeTestsGeneralDTO.File };

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
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await this._testClientService.GetTest(ReportType.RYE.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            this.ViewBag.CoolingTypeItems = new SelectList(origingeneralProperties, "Clave", "Descripcion");

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
                    if(decimals != 0)
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