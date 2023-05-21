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

    using Newtonsoft.Json;

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
    public class CgdController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly ICgdClientService _cgdClientService;
        private readonly IFpcClientService _fpcClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly ICgdService _cgdService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IResistanceTwentyDegreesClientServices _resistanceTwentyDegreesClientServices;
        private readonly IProfileSecurityService _profileClientService;
        public CgdController(
            IMasterHttpClientService masterHttpClientService,
            ICgdClientService cgdClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            ICgdService cgdService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            ISidcoClientService sidcoClientService,
            IResistanceTwentyDegreesClientServices resistanceTwentyDegreesClientServices,
            IFpcClientService fpcClientService,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _cgdClientService = cgdClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _cgdService = cgdService;
            _correctionFactorService = correctionFactorService;
            _hostEnvironment = hostEnvironment;
            _sidcoClientService = sidcoClientService;
            _resistanceTwentyDegreesClientServices = resistanceTwentyDegreesClientServices;
            _fpcClientService = fpcClientService;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.CromatografíadeGasesDisueltosenAceite)))
                {

                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new CgdViewModel { NoSerie = noSerie, OilType = "Seleccione..." });
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
            CgdViewModel cgdViewModel = new();
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
                    response = new ApiResponse<CgdViewModel>
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
                    response = new ApiResponse<CgdViewModel>
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
                        response = new ApiResponse<CgdViewModel>
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
                    return Json(new
                    {
                        response = new ApiResponse<CgdViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee información general",
                            Structure = null
                        }
                    });
                }

                ApiResponse<IEnumerable<ContGasCGDDTO>> olis = await _masterHttpClientService.GetContGasCGD("CGD", "-1", "-1");
                cgdViewModel.OilTypes = olis.Structure.ToList();
                cgdViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
                cgdViewModel.OilType = artifactDesing.GeneralArtifact.OilType;
            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _cgdClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.CGD.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.CGD.ToString() };
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

                cgdViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Cromatografía de Gases Disueltos en Aceite",
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
                response = new ApiResponse<CgdViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = cgdViewModel
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

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, int temperatureHour1, int temperatureHour2, int temperatureHour3, string oilType, string comment)
        {
            noSerie = noSerie.ToUpper().Trim();

            ApiResponse<SettingsToDisplayCGDReportsDTO> result = await _gatewayClientService.GetTemplateCGD(noSerie, clavePrueba, claveIdioma, temperatureHour1, temperatureHour2, temperatureHour3, oilType);

            if (result.Code.Equals(-1))
            {
                return View("Excel", new CgdViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayCGDReportsDTO reportInfo = result.Structure;

            #region Decode Template
            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return View("Excel", new CgdViewModel
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
                _cgdService.PrepareTemplate_CGD(reportInfo, clavePrueba, temperatureHour1, temperatureHour2, temperatureHour3, ref workbook);
            }
            catch (Exception)
            {
                return View("Excel", new CgdViewModel
                {
                    Error = "La plantilla esta mal configurada por favor verifiquela de inmediato"
                });
            }

            #endregion

            return View("Excel", new CgdViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = clavePrueba,
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                CGDReportsDTO = result.Structure,
                Workbook = workbook,
                TemperatureHour1 = temperatureHour1,
                TemperatureHour2 = temperatureHour2,
                TemperatureHour3 = temperatureHour3,
                OilType = oilType,
                Comments = comment,
                Error = string.Empty
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] CgdViewModel viewModel)
        {
            List<CGDTestsDTO> cgdTestsDTOs = new();

            if (viewModel.ClavePrueba.Equals(TestType.DDT.ToString()))
            {
                if (viewModel.TemperatureHour1 != 0)
                {
                    cgdTestsDTOs.Add(new()
                    {
                        CGDTestsDetails = new List<CGDTestsDetailsDTO>(),
                        KeyTest = viewModel.ClavePrueba,
                        OilType = viewModel.OilType,
                        Hour = viewModel.TemperatureHour1,
                        ValAcceptCg = 0.5M
                    });
                }

                if (viewModel.TemperatureHour2 != 0)
                {
                    cgdTestsDTOs.Add(new()
                    {
                        CGDTestsDetails = new List<CGDTestsDetailsDTO>(),
                        KeyTest = viewModel.ClavePrueba,
                        OilType = viewModel.OilType,
                        Hour = viewModel.TemperatureHour2,
                        ValAcceptCg = 0.5M
                    });
                }

                if (viewModel.TemperatureHour3 != 0)
                {
                    cgdTestsDTOs.Add(new()
                    {
                        CGDTestsDetails = new List<CGDTestsDetailsDTO>(),
                        KeyTest = viewModel.ClavePrueba,
                        OilType = viewModel.OilType,
                        Hour = viewModel.TemperatureHour3,
                        ValAcceptCg = 0.5M
                    });
                }
            }
            else
            {
                cgdTestsDTOs.Add(new()
                {
                    CGDTestsDetails = new List<CGDTestsDetailsDTO>(),
                    KeyTest = viewModel.ClavePrueba,
                    OilType = viewModel.OilType,
                    ValAcceptCg = 0.5M
                });
            }

            if (!_cgdService.Verify_CGD_ColumnsToCalculate(viewModel.CGDReportsDTO, viewModel.Workbook, viewModel.ClavePrueba))
            {
                return Json(new
                {
                    response = new { Code = -2, Description = "Faltan datos por ingresar en la tabla" }
                });
            }

            _cgdService.Prepare_CGD_Test(viewModel.CGDReportsDTO, viewModel.Workbook, viewModel.ClavePrueba, viewModel.ClaveIdioma, ref cgdTestsDTOs);

            ApiResponse<ResultCGDTestsDTO> resultTestCGD_Response = await _cgdClientService.CalculateTestCGD(cgdTestsDTOs);

            if (resultTestCGD_Response.Code.Equals(-1))
            {
                return Json(new
                {
                    response = new ApiResponse<CgdViewModel>
                    {
                        Code = -1,
                        Description = resultTestCGD_Response.Description,
                        Structure = viewModel
                    }
                });
            }

            Workbook workbook = viewModel.Workbook;

            _cgdService.PrepareIndexOfCGD(resultTestCGD_Response.Structure, viewModel.CGDReportsDTO, ref workbook, viewModel.ClaveIdioma);

            #region fillColumns
            cgdTestsDTOs = resultTestCGD_Response.Structure.CGDTests;
            #endregion

            viewModel.CgdTestsDTOs = cgdTestsDTOs;
            viewModel.Workbook = workbook;
            bool resultReport = !resultTestCGD_Response.Structure.Results.Any();
            viewModel.IsReportAproved = resultReport;

            string errors = string.Empty;
            List<string> errorMessages = resultTestCGD_Response.Structure.Results.Select(k => k.Message).ToList();
            string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
            return Json(new
            {
                response = new ApiResponse<CgdViewModel>
                {
                    Code = 1,
                    Description = allError,
                    Structure = viewModel
                }
            });
        }

        [HttpPost]

        public async Task<IActionResult> SavePDF([FromBody] CgdViewModel viewModel)
        {
            #region Export Excel
            int[] filas = new int[2];

            if (!_cgdService.Verify_CGD_ColumnsToCalculate(viewModel.CGDReportsDTO, viewModel.Workbook, viewModel.ClavePrueba))
            {
                return Json(new
                {
                    response = new { status = -2, description = "Faltan datos por ingresar en la tabla" }
                });
            }

            DateTime[] dates = _cgdService.GetDate(viewModel.Workbook, viewModel.CGDReportsDTO);

            string[] results = _cgdService.GetResults(viewModel.Workbook, viewModel.CGDReportsDTO);

            for (int i = 0; i < viewModel.CgdTestsDTOs.Count; i++)
            {
                viewModel.CgdTestsDTOs[i].Date = dates[i];
                viewModel.CgdTestsDTOs[i].Result = results[i];
            }

            Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

            PdfFormatProvider provider = new()
            {
                ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
            };
            document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.Workbook.ActiveSheet);
            FloatingImage image = null;
            int rowCount = 0; foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
            {
                rowCount = sheet.UsedCellRange.RowCount;
                image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(0, 0), 0, 0);

                #region LogoTwoPagesAndSaltoPage

                for (int i = 1; i <= viewModel.CGDReportsDTO.BaseTemplate.ColumnasConfigurables; i++)
                {

                    string celda = viewModel.CGDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("LogoProlec") && c.Seccion == i).Celda;
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

                    //var celdapage = viewModel.CGDReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado") && c.Seccion == i).Celda;
                    //int posicionpage = Convert.ToInt32(celdapage.Remove(0, 1)) - 1;

                    //PageBreaks pageBreaks = sheet.WorksheetPageSetup.PageBreaks;

                    //pageBreaks.TryInsertHorizontalPageBreak(posicionpage + 9, 0);

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

                #endregion

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
            CGDTestsGeneralDTO cgdTestsGeneralDTO = new()
            {
                Capacity = viewModel.CGDReportsDTO.HeadboardReport.Capacity,
                Comment = viewModel.Comments,
                Creadopor = User.Identity.Name,
                Customer = viewModel.CGDReportsDTO.HeadboardReport.Client,
                CGDTests = viewModel.CgdTestsDTOs,
                Fechacreacion = DateTime.Now,
                File = pdfFile,
                IdLoad = 0,
                KeyTest = viewModel.ClavePrueba,
                LanguageKey = viewModel.ClaveIdioma,
                LoadDate = DateTime.Now,
                Modificadopor = null,
                NameFile = string.Concat("CGD", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                Result = viewModel.IsReportAproved,
                SerialNumber = viewModel.NoSerie,
                TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                TypeReport = ReportType.CGD.ToString(),
                OilType = viewModel.OilType,
                ValAcceptCg = "Aceptado"
            };
            #endregion

            try
            {
                string a = JsonConvert.SerializeObject(cgdTestsGeneralDTO);
                ApiResponse<long> result = await _cgdClientService.SaveReport(cgdTestsGeneralDTO);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = cgdTestsGeneralDTO.NameFile, file = cgdTestsGeneralDTO.File };

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
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.CGD.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            ViewBag.UnitTypeItems = new SelectList(origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetUnitTypes, (int)MicroservicesEnum.splmasters)), "Clave", "Descripcion");

            ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");

            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");

            ViewBag.OilTypeItems = new List<SelectListItem>
            {

            new SelectListItem
                {
                    Text = "Seleccione...",
                    Value = ""
                },
                new SelectListItem
                {
                    Text = "Sintético",
                    Value = "Sintético"
                },
                new SelectListItem
                {
                    Text = "Mineral",
                    Value = "Mineral",
                    Selected = true
                },
                new SelectListItem
                {
                    Text = "Vegetal",
                    Value = "Vegetal"
                }
            };

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