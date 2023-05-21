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
    public class PlrController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IPlrClientService _plrClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IPlrService _plrService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IRodClientService _rodClientService;
        private readonly IPceClientService _pceService;
        private readonly IProfileSecurityService _profileClientService;
        public PlrController(
            IMasterHttpClientService masterHttpClientService,
            IPlrClientService plrClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IPlrService plrService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            IRodClientService rodClientService,
            IPceClientService pceService,
            IProfileSecurityService profileClientService
            )
        {
            this._masterHttpClientService = masterHttpClientService;
            this._plrClientService = plrClientService;
            this._reportClientService = reportClientService;
            this._artifactClientService = artifactClientService;
            this._testClientService = testClientService;
            this._gatewayClientService = gatewayClientService;
            this._plrService = plrService;
            this._correctionFactorService = correctionFactorService;
            this._rodClientService = rodClientService;
            this._hostEnvironment = hostEnvironment;
            this._pceService = pceService;
            this._profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(User.Identity.Name).Result;


                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.LinealidadyReactanciaenReactoresdePotenciaenDerivación)))
                {

                    await this.PrepareForm();
                    var noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return this.View(new PlrViewModel { NoSerie = noSerie });
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

            PlrViewModel pciViewModel = new();
            noSerie = noSerie.ToUpper().Trim();
            string noSerieSimple = string.Empty;

            ApiResponse<PositionsDTO> dataSelect = await this._gatewayClientService.GetPositions(noSerie);
             
            if (dataSelect.Code == -1)
            {
                return this.Json(new
                {
                    response = new ApiResponse<PlrViewModel>
                    {
                        Code = -1,
                        Description = "Error al obtener el voltaje para el numero de Serie " + noSerie,
                        Structure = null
                    }
                });
            }
            else
            {
                pciViewModel.Positions = dataSelect.Structure;
            }

            if (!string.IsNullOrEmpty(noSerie))
            {
                noSerieSimple = noSerie.Split("-")[0];
            }
            else
            {
                return this.Json(new
                {
                    response = new ApiResponse<PlrViewModel>
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
                    response = new ApiResponse<PlrViewModel>
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
                        response = new ApiResponse<PlrViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no encontrado",
                            Structure = null
                        }
                    });
                }

                if (artifactDesing.CharacteristicsArtifact.Count == 0)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PlrViewModel>
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

                    if (artifactDesing.VoltageKV.TensionKvAltaTension2 != null)
                    {
                        pciViewModel.TensionesNominales.Add((decimal)artifactDesing.VoltageKV.TensionKvAltaTension2);
                    }

                    if (artifactDesing.VoltageKV.TensionKvBajaTension2 != null)
                    {
                        pciViewModel.TensionesNominales.Add((decimal)artifactDesing.VoltageKV.TensionKvBajaTension2);
                    }

                    if (artifactDesing.VoltageKV.TensionKvSegundaBaja2 != null)
                    {
                        pciViewModel.TensionesNominales.Add((decimal)artifactDesing.VoltageKV.TensionKvSegundaBaja2);
                    }

                    if (artifactDesing.VoltageKV.TensionKvTerciario2 != null)
                    {
                        pciViewModel.TensionesNominales.Add((decimal)artifactDesing.VoltageKV.TensionKvTerciario2);
                    }
                }

                pciViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await this._plrClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.PLR.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.PLR.ToString() };
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
                        text = "Linealidad y Reactancia en Reactores de Potencia en Derivación",
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
                response = new ApiResponse<PlrViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = pciViewModel
                }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string nroSerie, string keyTest, string lenguage, decimal rldnt = 0, decimal nominalVoltage = 0, int amountOfTensions = 0, int amountOfTime = 0, string comment = "")
        {
            ApiResponse<SettingsToDisplayPLRReportsDTO> result = await this._gatewayClientService.GetTemplate(nroSerie, keyTest, lenguage, rldnt, nominalVoltage, amountOfTensions, amountOfTime);

            if (result.Code.Equals(-1))
            {
                return this.View("Excel", new PlrViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayPLRReportsDTO reportInfo = result.Structure;

            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return this.View("Excel", new PlrViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = reportInfo.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");

            this._plrService.PrepareTemplate_PLR(reportInfo, ref workbook, keyTest, amountOfTensions, amountOfTime, rldnt);

            return this.View("Excel", new PlrViewModel
            {
                ClaveIdioma = lenguage,
                ClavePrueba = keyTest,
                NoPrueba = NoPrueba,
                NoSerie = nroSerie,
                SettingsPLR = result.Structure,
                ReactanciaLinealDeDiseno = rldnt.ToString(),
                TensionNominal = nominalVoltage.ToString(),
                CantidadDeTension = amountOfTensions.ToString(),
                CantidadDeTiempo = amountOfTime.ToString(),
                Workbook = workbook,
                Error = string.Empty,
                Comments = comment
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] PlrViewModel viewModel)
        {
            try
            {

                ApiResponse<SettingsToDisplayPLRReportsDTO> result = await this._gatewayClientService.GetTemplate(viewModel.NoSerie, viewModel.ClavePrueba, viewModel.ClaveIdioma, !string.IsNullOrEmpty(viewModel.ReactanciaLinealDeDiseno) ? decimal.Parse(viewModel.ReactanciaLinealDeDiseno) : 0, !string.IsNullOrEmpty(viewModel.TensionNominal) ? decimal.Parse(viewModel.TensionNominal) : 0, !string.IsNullOrEmpty(viewModel.CantidadDeTension) ? int.Parse(viewModel.CantidadDeTension) : 0, !string.IsNullOrEmpty(viewModel.CantidadDeTiempo) ? int.Parse(viewModel.CantidadDeTiempo) : 0);

                if (result.Code.Equals(-1))
                {
                    return this.Json(new
                    {
                        response = result
                    });
                }
                SettingsToDisplayPLRReportsDTO reportInfo = result.Structure;
                /******** VALIDAR CAMPOS INPUT del EXCEL ****************/
                bool flag = true;
                int[] positionTension = this.GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("Tension")).Celda);
                int[] positionCorriente = this.GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("Corriente")).Celda);
                //var viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

                for (int i = 1; i <= int.Parse(viewModel.CantidadDeTension); i++)
                {
                    string tension = viewModel.Workbook.Sheets[0].Rows[positionTension[0] + (i - 1)].Cells[positionTension[1]]?.Value?.ToString();
                    string corriente = viewModel.Workbook.Sheets[0].Rows[positionCorriente[0] + (i - 1)].Cells[positionCorriente[1]]?.Value?.ToString();

                    if (string.IsNullOrEmpty(tension) || string.IsNullOrEmpty(corriente))
                    {
                        flag = false;
                        break;
                    }
                }

                if (!flag)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PlrViewModel>
                        {
                            Code = -1,
                            Description = "Debe llenar todas las pociones de Corriente y Tension",
                            Structure = viewModel
                        }
                    });
                }

                PLRTestsDTO test = new()
                {
                    KeyTest = viewModel.ClavePrueba,
                    NominalVoltage = decimal.Parse(viewModel.TensionNominal),
                    Rldnt = decimal.Parse(viewModel.ReactanciaLinealDeDiseno),
                    PorcDeviationNV = 0,
                    PLRTestsDetails = new List<PLRTestsDetailsDTO>()
                };

                for (int i = 1; i <= int.Parse(viewModel.CantidadDeTension); i++)
                {
                    string tension = viewModel.Workbook.Sheets[0].Rows[positionTension[0] + (i - 1)].Cells[positionTension[1]]?.Value?.ToString();
                    string corriente = viewModel.Workbook.Sheets[0].Rows[positionCorriente[0] + (i - 1)].Cells[positionCorriente[1]]?.Value?.ToString();

                    test.PLRTestsDetails.Add(new PLRTestsDetailsDTO
                    {
                        Current = decimal.Parse(viewModel.Workbook.Sheets[0].Rows[positionCorriente[0] + (i - 1)].Cells[positionCorriente[1]]?.Value?.ToString()),
                        Tension = decimal.Parse(viewModel.Workbook.Sheets[0].Rows[positionTension[0] + (i - 1)].Cells[positionTension[1]]?.Value?.ToString()),
                    });
                }

                ApiResponse<ResultPLRTestsDTO> calculateResult = await this._plrClientService.CalculateTestPLR(test);

                List<string> errores = new();
                if (calculateResult.Code == -1)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PlrViewModel>
                        {
                            Code = -1,
                            Description = "Error en calculo de pruebas PLR: " + calculateResult.Description
                        }
                    });
                }

                errores.AddRange(calculateResult.Structure.Results.Select(k => k.Message).ToList());
                string allErrosPrepare = errores.Any() ? errores.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
                Workbook workbook = viewModel.Workbook;
                bool resultReport = !calculateResult.Structure.Results.Any();

                int[] _positionWB = this.GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Reactancia") && c.ClavePrueba == viewModel.ClavePrueba).Celda);
                viewModel.PLRTestsDTO = new PLRTestsDTO
                {
                    KeyTest = viewModel.ClavePrueba,
                    PorcDeviationNV = calculateResult.Structure.PLRTests.PorcDeviationNV,
                    Rldnt = !string.IsNullOrEmpty(viewModel.ReactanciaLinealDeDiseno) ? decimal.Parse(viewModel.ReactanciaLinealDeDiseno) : 0,
                    NominalVoltage = !string.IsNullOrEmpty(viewModel.TensionNominal) ? decimal.Parse(viewModel.TensionNominal) : 0,
                    PLRTestsDetails = new List<PLRTestsDetailsDTO>()
                };
                foreach (var item in calculateResult.Structure.PLRTests.PLRTestsDetails.Select((value, i) => new { i, value }))
                {
                    viewModel.PLRTestsDTO.PLRTestsDetails.Add(new PLRTestsDetailsDTO
                    {
                        Current = item.value.Current,
                        Tension = item.value.Tension,
                        Reactance = item.value.Reactance,
                        PorcD = item.value.PorcD
                    });

                    viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + item.i].Cells[_positionWB[1]].Value = item.value.Reactance;
                }

                _positionWB = this.GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DesvTenNom") && c.ClavePrueba == viewModel.ClavePrueba).Celda);
                workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = calculateResult.Structure.PLRTests.PorcDeviationNV.ToString();

                viewModel.IsReportAproved = resultReport;
                viewModel.SettingsPLR = reportInfo;
                viewModel.Workbook = workbook;
                viewModel.OfficialWorkbook = workbook;
                viewModel.ResultPLRTestsDTO = calculateResult.Structure;

                return this.Json(new
                {
                    response = new ApiResponse<PlrViewModel>
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
        public async Task<IActionResult> SavePDF([FromBody] PlrViewModel viewModel)
        {

            /* if (!_pciService.Verify_FPB_Columns(viewModel.Workbook, viewModel.FPBReportsDTO, viewModel.ClavePrueba))
             {
                 return this.Json(new
                 {
                     response = new { status = -1, description = "Faltan datos por ingresar en la tabla" }
                 });
             }
            */

            if (viewModel.ClavePrueba == "TIE")
            {
                int[] posTiempo = this.GetRowColOfWorbook(viewModel.SettingsPLR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo") && c.ClavePrueba == viewModel.ClavePrueba).Celda);
                int[] posiCorriente = this.GetRowColOfWorbook(viewModel.SettingsPLR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Corriente") && c.ClavePrueba == viewModel.ClavePrueba).Celda);
                int[] posiTension = this.GetRowColOfWorbook(viewModel.SettingsPLR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension") && c.ClavePrueba == viewModel.ClavePrueba).Celda);
                viewModel.PLRTestsDTO = new PLRTestsDTO
                {
                    KeyTest = viewModel.ClavePrueba,
                    PLRTestsDetails = new List<PLRTestsDetailsDTO>()
                };

                for (int i = 1; i <= int.Parse(viewModel.CantidadDeTiempo); i++)
                {
                    viewModel.PLRTestsDTO.PLRTestsDetails.Add(new PLRTestsDetailsDTO
                    {
                        Time = decimal.Parse(viewModel.Workbook.Sheets[0].Rows[posTiempo[0] + (i - 1)].Cells[posTiempo[1]].Value.ToString()),
                        Current = decimal.Parse(viewModel.Workbook.Sheets[0].Rows[posiCorriente[0] + (i - 1)].Cells[posiCorriente[1]].Value.ToString()),
                        Tension = decimal.Parse(viewModel.Workbook.Sheets[0].Rows[posiTension[0] + (i - 1)].Cells[posiTension[1]].Value.ToString()),

                    });
                }
            }

            Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

            PdfFormatProvider provider = new()
            {
                ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
            };
            document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.OfficialWorkbook.ActiveSheet);
            FloatingImage image = null;
            //int rows = viewModel.FPBReportsDTO.TitleOfColumns.Count;

            int rowCount = 0;  foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
            {
                //double officialSize = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(15, 3)].GetFontSize().Value;

                Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment allign = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(15, 2)].GetHorizontalAlignment().Value;
                rowCount = sheet.UsedCellRange.RowCount;
                image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(0, 0), 0, 0);
                string path = Path.Combine(this._hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
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

            PLRTestsGeneralDTO pciTestGeneral = new()
            {
                Capacity = viewModel.SettingsPLR.HeadboardReport.Capacity,
                Comment = viewModel.Comments,
                Creadopor = User.Identity.Name,
                Customer = viewModel.SettingsPLR.HeadboardReport.Client,
                Fechacreacion = DateTime.Now,
                File = pdfFile,
                IdLoad = 0,
                KeyTest = viewModel.ClavePrueba,
                LanguageKey = viewModel.ClaveIdioma,
                LoadDate = DateTime.Now,
                Modificadopor = string.Empty,
                NameFile = string.Concat("PLR", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                Result = viewModel.IsReportAproved,
                SerialNumber = viewModel.NoSerie,
                TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                TypeReport = ReportType.PLR.ToString(),
                Data = viewModel.PLRTestsDTO

            };

            try
            {
                ApiResponse<long> result = await this._plrClientService.SaveReport(pciTestGeneral);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = pciTestGeneral.NameFile, file = viewModel.Base64PDF };

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

        public IActionResult Error() => this.View();

        #region PrivateMethods
        public async Task PrepareForm()
        {

            /// var a = await this._gatewayClientService.GetPositions("G4135-01");

            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();
            IEnumerable<GeneralPropertiesDTO> tensionNominalItems = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> voltaje = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." }, new GeneralPropertiesDTO { Clave = "Aluminio", Descripcion = "Aluminio" } }.AsEnumerable();
            // Tipos de prueba
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await this._testClientService.GetTest(ReportType.PLR.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            this.ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");
            this.ViewBag.TensionNominalItems = new SelectList(tensionNominalItems, "Clave", "Descripcion");

            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            this.ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");

            generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetRulesEquivalents, (int)MicroservicesEnum.splmasters));
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

