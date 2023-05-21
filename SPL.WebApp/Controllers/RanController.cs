namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
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

    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export;
    using Telerik.Windows.Documents.Spreadsheet.Model;
    using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

    /// <summary>
    /// Resistencia de Aislamiento a los Núcleos
    /// </summary>
    public class RanController : Controller
    {

        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IRanClientService _ranClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IRanService _ranService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProfileSecurityService _profileClientService;
        public RanController(
            IMasterHttpClientService masterHttpClientService,
            IRanClientService ranClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IRanService ranService,
            IWebHostEnvironment hostEnvironment,
            IProfileSecurityService profileClientService)
        {
            _masterHttpClientService = masterHttpClientService;
            _ranClientService = ranClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _ranService = ranService;
            _hostEnvironment = hostEnvironment;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.ResistenciadeAislamientodelosNúcleosyHerrajes)))
                {

                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new RanViewModel() { Measuring = 1, NoSerie = noSerie });
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
            RanViewModel ranViewModel = new();
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
                    response = new ApiResponse<RanViewModel>
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
                    response = new ApiResponse<RanViewModel>
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
                        response = new ApiResponse<RanViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no encontrado",
                            Structure = null
                        }
                    });
                }

                ranViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _ranClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(-1))
            {
                return Json(new
                {

                    response = new ApiResponse<RanViewModel>
                    {
                        Code = -1,
                        Description = resultFilter.Description,
                        Structure = ranViewModel
                    }
                });
            }
            if (resultFilter.Code.Equals(1) && resultFilter.Structure.Any())
            {
                InfoGeneralTypesReportsDTO reportRad = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.RAN.ToString()));

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

                ranViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Resistencia del Aislamiento de los Núcleos y Herrajes",
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

                response = new ApiResponse<RanViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = ranViewModel
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
        [HttpPost]
        public async Task<IActionResult> Validate([FromBody] RanViewModel viewModel)
        {
            RANTestsDetailsDTO ranTestDTO = new();

            _ranService.Prepare_RAN_Test(viewModel.ClavePrueba, viewModel.Measuring, viewModel.RANReportsDTO, viewModel.Workbook, ref ranTestDTO);
            ApiResponse<ResultRANTestsDTO> resultTestRAN_Response = await _ranClientService.CalculateTestRAN(ranTestDTO);

            if (resultTestRAN_Response.Code.Equals(-1))
            {
                return Json(new
                {
                    response = resultTestRAN_Response
                });
            }

            Telerik.Web.Spreadsheet.Workbook workbook = viewModel.Workbook;

            _ranService.PrepareIndexOfRAN(resultTestRAN_Response.Structure, viewModel.RANReportsDTO, viewModel.ClaveIdioma, ref workbook, viewModel.ClavePrueba);

            viewModel.RanTestDTO = ranTestDTO;
            viewModel.Workbook = workbook;

            bool resultReport = true;

            resultReport = resultTestRAN_Response.Structure.MessageErrors.Any();

            viewModel.IsReportAproved = !resultReport;

            string errors = string.Empty;
            List<string> errorMessages = resultTestRAN_Response.Structure.MessageErrors.Select(k => k.Message).ToList();
            string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
            return Json(new
            {
                response = new ApiResponse<RanViewModel>
                {
                    Code = 1,
                    Description = allError,
                    Structure = viewModel
                }
            });

        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, int measuring, string comment)
        {

            ApiResponse<SettingsToDisplayRANReportsDTO> result = await _gatewayClientService.GetTemplate(ReportType.RAN.ToString(), noSerie, clavePrueba, measuring, claveIdioma);

            if (result.Code.Equals(-1))
            {
                return View("Excel", new RanViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayRANReportsDTO reportInfo = result.Structure;

            #region Decode Template

            if (reportInfo.BaseTemplate is null && string.IsNullOrEmpty(reportInfo.BaseTemplate?.Plantilla))
            {
                return View("Excel", new RanViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = reportInfo.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.Load(stream, ".xlsx");

            if (clavePrueba.Equals(TestType.AYD.ToString()))
            {
                _ranService.PrepareTemplate_RAN_AYD(measuring, claveIdioma, reportInfo, ref workbook);
            }
            else
            {
                _ranService.PrepareTemplate_RAN_APD(measuring, claveIdioma, reportInfo, ref workbook);
            }

            #endregion
            return View("Excel", new RanViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = clavePrueba,
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                Measuring = measuring,
                RANReportsDTO = result.Structure,
                Workbook = workbook,
                Error = string.Empty,
                Comments = comment
            });
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] RanViewModel viewModel)
        {
            #region Export Excel

            #region ReCalculate PDF

            RANTestsDetailsDTO ranTestDTO = new();
            _ranService.Prepare_RAN_Test(viewModel.ClavePrueba, viewModel.Measuring, viewModel.RANReportsDTO, viewModel.Workbook, ref ranTestDTO);

            ApiResponse<ResultRANTestsDTO> resultTestRAN_Response = await _ranClientService.CalculateTestRAN(ranTestDTO);



            Telerik.Web.Spreadsheet.Workbook workbook = viewModel.Workbook;

            _ranService.PrepareIndexOfRAN(resultTestRAN_Response.Structure, viewModel.RANReportsDTO, viewModel.ClaveIdioma, ref workbook, viewModel.ClavePrueba);
            _ranService.DeleteValid(viewModel.ClavePrueba, viewModel.Measuring, viewModel.RANReportsDTO, ref workbook);
            viewModel.RanTestDTO = ranTestDTO;
            viewModel.Workbook = workbook;

            bool resultReport = true;

            resultReport = resultTestRAN_Response.Structure.MessageErrors.Any();

            viewModel.IsReportAproved = !resultReport;

            List<DateTime> dates = _ranService.GetDate(viewModel.Workbook, viewModel.RANReportsDTO, viewModel.ClavePrueba);

            #endregion

            Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

            PdfFormatProvider provider = new()
            {
                ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
            };

            document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.Workbook.ActiveSheet);
            FloatingImage image = null;

            int rowCount = 0; foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
            {
                image = new FloatingImage(sheet, new CellIndex(0, 0), 0, 0);
                string path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
                rowCount = sheet.UsedCellRange.RowCount;
                FileStream stream = new(path, FileMode.Open);
                using (stream)
                {
                    image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                }

                image.Width = 215;
                image.Height = 38;

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

            #region Save Report
            RANReportDTO radReportDTO = new()
            {
                Capacity = viewModel.RANReportsDTO.HeadboardReport.Capacity,
                Comment = viewModel.Comments,
                Customer = viewModel.RANReportsDTO.HeadboardReport.Client,
                LoadDate = DateTime.Now,
                Fechacreacion = DateTime.Now,

                TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                LanguageKey = viewModel.ClaveIdioma,
                SerialNumber = viewModel.NoSerie,
                Result = viewModel.IsReportAproved,
                TypeReport = ReportType.RAN.ToString(),
                NameFile = string.Concat("RAN", viewModel.ClaveIdioma, viewModel.ClavePrueba, viewModel.Measuring, viewModel.NoSerie, "-" + rowCount, ".pdf"),
                File = viewModel.Base64PDF,
                KeyTest = viewModel.ClavePrueba,
                NumberMeasurements = viewModel.Measuring,
                Creadopor = User.Identity.Name,
                Modificadopor = null,
                Fechamodificacion = null,
            };

            //viewModel.RanTestDTO.DateTest = DateTime.Now;

            foreach (RANTestsDetailsRADTO item in viewModel.RanTestDTO.RANTestsDetailsRAs)
            {
                item.DateTest = dates[0];
            }

            foreach (RANTestsDetailsTADTO item in viewModel.RanTestDTO.RANTestsDetailsTAs)
            {
                item.DateTest = (dates.Count() > 1) ? dates[1] : dates[0];
            }

            radReportDTO.Rans = new List<RANTestsDetailsDTO>
            {
                viewModel.RanTestDTO
            };

            #endregion

            try
            {
                ApiResponse<long> result = await _ranClientService.SaveReport(radReportDTO);

                JsonSerializerOptions options = new() { WriteIndented = true };
                string jsonString = System.Text.Json.JsonSerializer.Serialize(radReportDTO, options);

                var resultResponse = new { result.Code, result.Description, nameFile = radReportDTO.NameFile, file = radReportDTO.File };

                return Json(new
                {
                    response = resultResponse
                });
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    response = new { Code = -1, Description = ex.Message, nameFile = "", file = "" }
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

            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.RAN.ToString(), "-1");
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
        #endregion
    }
}
