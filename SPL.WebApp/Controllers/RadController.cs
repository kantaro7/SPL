namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Identity.Web;

    using Serilog;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.Helpers;
    using SPL.WebApp.ViewModels;

    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export;
    using Telerik.Windows.Documents.Spreadsheet.Model;
    using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

    using Workbook = Telerik.Web.Spreadsheet.Workbook;

    /// <summary>
    /// Resistencia de Aislamiento a los Devanados
    /// </summary>
    public class RadController : Controller
    {

        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IRadClientService _radClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IRadService _radService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProfileSecurityService _profileClientService;
        private readonly ILogger _logger;
        public RadController(
            IMasterHttpClientService masterHttpClientService,
            IRadClientService radClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IRadService radService,
            IWebHostEnvironment hostEnvironment,
            IProfileSecurityService profileClientService)
        {
            _masterHttpClientService = masterHttpClientService;
            _radClientService = radClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _radService = radService;
            _hostEnvironment = hostEnvironment;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.ResistenciadeAislamientoalosDevanados)))
                {

                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new RadViewModel { NoSerie = noSerie });
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
                RadViewModel radViewModel = new();
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
                        response = new ApiResponse<RadViewModel>
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
                            response = new ApiResponse<RadViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no encontrado",
                                Structure = null
                            }
                        });
                    }

                    radViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
                    radViewModel.UnitType = artifactDesing.GeneralArtifact.TipoUnidad;
                }

                ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _radClientService.GetFilter(noSerie);

                if (resultFilter.Code.Equals(1))
                {
                    InfoGeneralTypesReportsDTO reportRad = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.RAD.ToString()));

                    IEnumerable<IGrouping<bool?, InfoGeneralReportsDTO>> reportRadGroupStatus = reportRad.ListDetails.GroupBy(c => c.Resultado);

                    List<TreeViewItemDTO> reportsAprooved = new();
                    List<TreeViewItemDTO> reportsRejected = new();

                    foreach (IGrouping<bool?, InfoGeneralReportsDTO> reports in reportRadGroupStatus)
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

                    radViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Resistencia de Aislamiento a los Devanados",
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

                    response = new ApiResponse<RadViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = radViewModel
                    }
                });
            }
            catch (Exception)
            {
                return Json(new
                {

                    response = new ApiResponse<RadViewModel>
                    {
                        Code = -1,
                        Description = string.Empty,
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

        [HttpPost]
        public async Task<IActionResult> Validate([FromBody] RadViewModel viewModel)
        {
            RADTestsDTO radTestDTO = new()
            {
                AcceptanceValue = viewModel.AcceptanceValue,
                Headers = new List<HeaderRADTestsDTO>()
            };

            _radService.Prepare_RAD_Test(viewModel.ClavePrueba, viewModel.RADReportsDTO, viewModel.Workbook, ref radTestDTO);
            ApiResponse<ResultRADTestsDTO> resultTestRAD_Response = await _radClientService.CalculateTestRAD(radTestDTO);

            if (resultTestRAD_Response.Code.Equals(-1))
            {
                return Json(new
                {
                    response = resultTestRAD_Response
                });
            }

            Workbook workbook = viewModel.Workbook;

            _radService.PrepareIndexOfRAD(resultTestRAD_Response.Structure, viewModel.RADReportsDTO, viewModel.ClaveIdioma, ref workbook);

            viewModel.RadTestDTO = radTestDTO;
            viewModel.Workbook = workbook;

            bool resultReport = true;
            foreach (ResultRADTestsDetailsDTO item in resultTestRAD_Response.Structure.results)
            {
                resultReport = !item.MessageErrors.Any();
                if (!resultReport)
                    break;
            }

            viewModel.IsReportAproved = resultReport;

            string errors = string.Empty;
            List<string> errorMessages = resultTestRAD_Response.Structure.results.SelectMany(c => c.MessageErrors).Select(k => k.Message).ToList();
            string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
            return Json(new
            {
                response = new ApiResponse<RadViewModel>
                {
                    Code = 1,
                    Description = allError,
                    Structure = viewModel
                }
            });

        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, string unityType, string thirtyWindingType, string comment)
        {

            ApiResponse<SettingsToDisplayRADReportsDTO> result = await _gatewayClientService.GetTemplate(ReportType.RAD.ToString(), noSerie, clavePrueba, unityType, thirtyWindingType, claveIdioma);

            if (result.Code.Equals(-1))
            {
                return View("Excel", new RadViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayRADReportsDTO reportInfo = result.Structure;

            #region Decode Template

            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return View("Excel", new RadViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = reportInfo.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");

            if (clavePrueba.Equals(TestType.AYD.ToString()))
            {
                _radService.PrepareTemplate_RAD_CAYDES(reportInfo, ref workbook);
            }
            else
            {
                _radService.PrepareTemplate_RAD_SA(reportInfo, ref workbook);
            }

            string valueAcceptance = reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptacion"))?.Formato;
            #endregion

            return View("Excel", new RadViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = clavePrueba,
                UnitType = unityType,
                ThirtyWindingType = thirtyWindingType,
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                RADReportsDTO = result.Structure,
                AcceptanceValue = Convert.ToDecimal(valueAcceptance),
                Workbook = workbook,
                Error = string.Empty,
                Comments = comment
            });
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] RadViewModel viewModel)
        {

            try
            {

                #region Validate Field Required

                //read errors
                string errors = _radService.Validate(viewModel.ClavePrueba, viewModel.Workbook, viewModel.RADReportsDTO);

                if (!string.IsNullOrEmpty(errors))
                {
                    return Json(new
                    {
                        response = new ApiResponse<RadViewModel>
                        {
                            Code = -1,
                            Description = errors.Replace("*", "\n"),
                            Structure = viewModel
                        }
                    });
                }

                RADTestsDTO radTestDTO = new()
                {
                    AcceptanceValue = viewModel.AcceptanceValue,
                    Headers = new List<HeaderRADTestsDTO>()
                };

                _radService.Prepare_RAD_Test(viewModel.ClavePrueba, viewModel.RADReportsDTO, viewModel.Workbook, ref radTestDTO);
                ApiResponse<ResultRADTestsDTO> resultTestRAD_Response = await _radClientService.CalculateTestRAD(radTestDTO);

                if (resultTestRAD_Response.Code.Equals(-1))
                {
                    return Json(new
                    {
                        response = resultTestRAD_Response
                    });
                }

                Workbook workbook = viewModel.Workbook;

                _radService.PrepareIndexOfRAD(resultTestRAD_Response.Structure, viewModel.RADReportsDTO, viewModel.ClaveIdioma, ref workbook);

                viewModel.RadTestDTO = radTestDTO;
                viewModel.Workbook = workbook;

                bool resultReport = true;
                foreach (ResultRADTestsDetailsDTO item in resultTestRAD_Response.Structure.results)
                {
                    resultReport = !item.MessageErrors.Any();
                    if (!resultReport)
                        break;
                }

                viewModel.IsReportAproved = resultReport;

                #endregion

                #region Export Excel

                Workbook officialWorkbook = viewModel.Workbook;

                Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = officialWorkbook.ToDocument();

                PdfFormatProvider provider = new()
                {
                    ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
                };

                document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == officialWorkbook.ActiveSheet);
                FloatingImage image = null;
                int rowCount = 0; foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
                {
                    rowCount = sheet.UsedCellRange.RowCount;

                    string voltage1 = sheet.Cells[new CellIndex(8, 4)].GetValue().Value.RawValue;
                    sheet.Cells[new CellIndex(8, 4)].SetValueAsText(FormatStringDecimal(voltage1));
                    sheet.Cells[new CellIndex(8, 4)].SetHorizontalAlignment(RadHorizontalAlignment.Right);

                    string temp1 = sheet.Cells[new CellIndex(8, 7)].GetValue().Value.RawValue;
                    sheet.Cells[new CellIndex(8, 7)].SetValueAsText(FormatStringDecimalTemp(temp1));
                    sheet.Cells[new CellIndex(8, 7)].SetHorizontalAlignment(RadHorizontalAlignment.Right);

                    string voltage2 = sheet.Cells[new CellIndex(28, 4)].GetValue().Value.RawValue;
                    sheet.Cells[new CellIndex(28, 4)].SetValueAsText(FormatStringDecimal(voltage2));
                    sheet.Cells[new CellIndex(28, 4)].SetHorizontalAlignment(RadHorizontalAlignment.Right);

                    string temp2 = sheet.Cells[new CellIndex(28, 7)].GetValue().Value.RawValue;
                    sheet.Cells[new CellIndex(28, 7)].SetValueAsText(FormatStringDecimalTemp(temp2));
                    sheet.Cells[new CellIndex(28, 7)].SetHorizontalAlignment(RadHorizontalAlignment.Right);

                    for (int i = 4; i < 8; i++)
                    {
                        for (int j = 10; j < 46; j++)
                        {
                            if (j is < 27 or > 29)
                            {
                                string val = sheet.Cells[new CellIndex(j, i)].GetValue().Value.RawValue;
                                sheet.Cells[new CellIndex(j, i)].SetValueAsText(FormatStringDecimal(val));
                            }
                        }
                    }

                    image = new FloatingImage(sheet, new CellIndex(0, 0), 0, 0);
                    string path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
                    FileStream stream = new(path, FileMode.Open);
                    using (stream)
                    {
                        image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                    }

                    image.Width = 215;
                    image.Height = 38;

                    sheet.Shapes.Add(image);

                    image = new FloatingImage(sheet, new CellIndex(5, 9), 0, 10);
                    path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "imgRAD.jpg");
                    stream = new(path, FileMode.Open);
                    using (stream)
                    {
                        image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                    }

                    image.Width = 50;
                    image.Height = 40;

                    sheet.Shapes.Add(image);

                    Telerik.Windows.Documents.Spreadsheet.Model.Printing.WorksheetPageSetup pageSetup = sheet.WorksheetPageSetup;
                    pageSetup.PaperType = Telerik.Windows.Documents.Model.PaperTypes.A4;
                    pageSetup.PageOrientation = Telerik.Windows.Documents.Model.PageOrientation.Portrait;

                    //pageSetup.CenterVertically = true;
                    pageSetup.CenterHorizontally = true;

                    pageSetup.PrintOptions.PrintGridlines = false;

                    pageSetup.ScaleFactor = new Telerik.Documents.Primitives.Size(0.9, 0.9);

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
                // Cargando Excel en Spire Workbook
                Spire.Xls.Workbook wbFromStream = new();
                wbFromStream.LoadFromStream(new MemoryStream(excelFile));
                MemoryStream pdfStream = new();

                // Exportando Excel en Pdf
                wbFromStream.SaveToStream(pdfStream, Spire.Xls.FileFormat.PDF);
                pdfFile = pdfStream.ToArray();
                viewModel.Base64PDF = Convert.ToBase64String(pdfFile);

                #endregion
                viewModel.RadTestDTO.Creadopor = User.Identity.Name;
                viewModel.RadTestDTO.Fechacreacion = DateTime.Now;
                viewModel.RadTestDTO.Modificadopor = null;
                viewModel.RadTestDTO.Fechamodificacion = null;

                #region Save Report
                RADReportDTO radReportDTO = new()
                {
                    Capacity = viewModel.RADReportsDTO.HeadboardReport.Capacity,
                    Comment = viewModel.Comments,
                    Customer = viewModel.RADReportsDTO.HeadboardReport.Client,
                    LoadDate = DateTime.Now,
                    TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                    LanguageKey = viewModel.ClaveIdioma,
                    SerialNumber = viewModel.NoSerie,

                    Result = viewModel.IsReportAproved,
                    TypeReport = ReportType.RAD.ToString(),
                    NameFile = string.Concat("RAD", viewModel.ClaveIdioma, viewModel.ClavePrueba, viewModel.UnitType, viewModel.ThirtyWindingType, viewModel.NoSerie, "-" + rowCount, ".pdf"),
                    File = viewModel.Base64PDF,
                    KeyTest = viewModel.ClavePrueba,
                    TypeUnit = viewModel.UnitType,
                    ThirdWindingType = viewModel.ThirtyWindingType,
                    DataTests = viewModel.RadTestDTO
                };

                #endregion

                ApiResponse<long> result = await _radClientService.SaveReport(radReportDTO);

                var resultResponse = new { result.Code, description = result.Description, nameFile = radReportDTO.NameFile, file = radReportDTO.File };

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

            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");

            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetUnitTypes, (int)MicroservicesEnum.splmasters));
            ViewBag.TipoUnidadItems = new SelectList(generalProperties, "Clave", "Descripcion");

            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetThirdWinding, (int)MicroservicesEnum.splmasters));
            ViewBag.ThirdWindingItems = new SelectList(generalProperties, "Clave", "Descripcion");

            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.RAD.ToString(), "-1");
            List<GeneralPropertiesDTO> reportsGP = new();

            foreach (TestsDTO item in reportResult.Structure)
            {
                reportsGP.Add(new GeneralPropertiesDTO
                {
                    Clave = item.ClavePrueba,
                    Descripcion = item.Descripcion
                });
            }

            generalProperties = origingeneralProperties.Concat(reportsGP);
            ViewBag.TestItems = new SelectList(generalProperties, "Clave", "Descripcion");
        }

        private string FormatStringDecimal(string num)
        {
            if (!string.IsNullOrEmpty(num))
            {
                if (num.Split('.').Length == 2)
                {
                    string entero = num.Split('.')[0];
                    string decima = num.Split('.')[1];

                    decima = decima.Length > 2 ? decima[..2] : decima.PadRight(2, '0');
                    num = $"{entero}.{decima}";
                }
                else
                {
                    num += ".00";
                }
            }
            return num;
        }

        private string FormatStringDecimalTemp(string num)
        {
            if (!string.IsNullOrEmpty(num))
            {
                if (num.Split('.').Length == 2)
                {
                    string entero = num.Split('.')[0];
                    string decima = num.Split('.')[1];

                    decima = decima.Length > 2 ? decima[..2] : decima.PadRight(1, '0');
                    num = $"{entero}.{decima}";
                }
                else
                {
                    num += ".0";
                }
            }
            return num;
        }

        #endregion
    }
}
