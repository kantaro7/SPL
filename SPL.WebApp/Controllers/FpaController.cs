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
    public class FpaController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IFpaClientService _fpaClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IFpaService _fpaService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IProfileSecurityService _profileClientService;
        public FpaController(
            IMasterHttpClientService masterHttpClientService,
            IFpaClientService fpaClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IFpaService fpaService,
            IWebHostEnvironment hostEnvironment,
            ISidcoClientService sidcoClientService,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _fpaClientService = fpaClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _fpaService = fpaService;
            _hostEnvironment = hostEnvironment;
            _sidcoClientService = sidcoClientService;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.FactordePotenciaenAceite)))
                {

                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new FpaViewModel { NoSerie = noSerie, OilType = "Seleccione..." });
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
                FpaViewModel fpaViewModel = new();
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
                    // DG
                    InformationArtifactDTO artifactDesing = await _artifactClientService.GetArtifact(noSerieSimple);
                    if (artifactDesing.GeneralArtifact.OrderCode == null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<FpaViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no encontrado",
                                Structure = null
                            }
                        });
                    }

                    if (artifactDesing.GeneralArtifact.TypeTrafoId is null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RodViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee información de TypeTrafoId",
                                Structure = null
                            }
                        });
                    }

                    ApiResponse<IEnumerable<ContGasCGDDTO>> olis = await _masterHttpClientService.GetContGasCGD("FPA", "-1", "-1");
                    fpaViewModel.OilTypes = olis.Structure.ToList();
                    fpaViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
                    fpaViewModel.OilType = artifactDesing.GeneralArtifact.OilType;
                }

                // ARBOL

                ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _fpaClientService.GetFilter(noSerie);

                if (resultFilter.Code.Equals(1))
                {
                    InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.FPA.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.FPA.ToString() };
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

                    fpaViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Factor de Potencia en Aceite Aislantes",
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
                    response = new ApiResponse<FpaViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = fpaViewModel
                    }
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<FpaViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, string oilType, string comment, bool incluirSegundaFila)
        {
            ApiResponse<SettingsToDisplayFPAReportsDTO> result = await _gatewayClientService.GetTemplateFPA(noSerie, clavePrueba, claveIdioma, oilType, incluirSegundaFila ? 2 : 1);

            if (result.Code.Equals(-1))
            {
                return View("Excel", new FpaViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayFPAReportsDTO reportInfo = result.Structure;

            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return View("Excel", new FpaViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = reportInfo.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");

            _fpaService.PrepareTemplate_FPA(reportInfo, ref workbook, incluirSegundaFila, clavePrueba, oilType);

            return View("Excel", new FpaViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = clavePrueba,
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                OilType = oilType,
                FPAReportsDTO = result.Structure,
                Workbook = workbook,
                Error = string.Empty,
                Comments = comment,
                IncluirSegundaFila = incluirSegundaFila
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] FpaViewModel viewModel)
        {

            FPATestsDTO fpaTestsDTO = new()
            {
                OilType = viewModel.OilType,
                ClavePrueba = viewModel.ClavePrueba,
                KeyLanguage = viewModel.ClaveIdioma,
                FPATestsDetails = new FPATestsDetailsDTO()
                {
                    FPADielectricStrength = new List<FPADielectricStrengthDTO>() {
                        new FPADielectricStrengthDTO(),
                        new FPADielectricStrengthDTO()
                    },
                    FPAPowerFactor = new List<FPAPowerFactorDTO>() {
                        new FPAPowerFactorDTO(),
                        new FPAPowerFactorDTO()
                    },
                    FPAGasContent = new FPAGasContentDTO(),
                    FPAWaterContent = new FPAWaterContentDTO()
                }
            };

            if (!_fpaService.Verify_FPA_Columns(viewModel.FPAReportsDTO, viewModel.Workbook, viewModel.IncluirSegundaFila, viewModel.ClavePrueba))
            {
                return Json(new
                {
                    response = new ApiResponse<FpaViewModel>
                    {
                        Code = -1,
                        Description = "Faltan datos por ingresar en la tabla",
                        Structure = viewModel
                    }
                });
            }

            _fpaService.Prepare_FPA_Test(viewModel.FPAReportsDTO, viewModel.Workbook, ref fpaTestsDTO, viewModel.IncluirSegundaFila);

            ApiResponse<ResultFPATestsDTO> resultTestFPA_Response = await _fpaClientService.CalculateTestFPA(fpaTestsDTO);

            if (resultTestFPA_Response.Code.Equals(-1))
            {
                return Json(new
                {
                    response = new ApiResponse<FpaViewModel>
                    {
                        Code = -1,
                        Description = resultTestFPA_Response.Description,
                        Structure = viewModel
                    }
                });
            }

            if (viewModel.ClavePrueba == "DDP")
            {
                fpaTestsDTO.FPATestsDetails.FPAGasContent.Limit2 = viewModel.ClaveIdioma == "EN" ? "Accepted" : "Aceptado";
            }

            Workbook workbook = viewModel.Workbook;

            _fpaService.PrepareIndexOfFPA(resultTestFPA_Response.Structure, viewModel.FPAReportsDTO, viewModel.ClaveIdioma, ref workbook, viewModel.IncluirSegundaFila, viewModel.ClavePrueba);

            #region fillColumns
            fpaTestsDTO = resultTestFPA_Response.Structure.FPATests;
            #endregion

            viewModel.FPATestsDTO = fpaTestsDTO;
            viewModel.Workbook = workbook;
            bool resultReport = !resultTestFPA_Response.Structure.Results.Any();
            viewModel.IsReportAproved = resultReport;

            string errors = string.Empty;
            List<string> errorMessages = resultTestFPA_Response.Structure.Results.Select(k => k.Message).ToList();
            string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
            return Json(new
            {
                response = new ApiResponse<FpaViewModel>
                {
                    Code = 1,
                    Description = allError,
                    Structure = viewModel
                }
            });
        }

        [HttpPost]

        public async Task<IActionResult> SavePDF([FromBody] FpaViewModel viewModel)
        {
            #region Export Excel
            if (!_fpaService.Verify_FPA_Columns(viewModel.FPAReportsDTO, viewModel.Workbook, viewModel.IncluirSegundaFila, viewModel.ClavePrueba))
            {
                return Json(new
                {
                    response = new { status = -2, description = "Faltan datos por ingresar en la tabla" }
                });
            }

            DateTime date = _fpaService.GetDate(viewModel.FPAReportsDTO, viewModel.Workbook);
            string grade = _fpaService.GetGrades(viewModel.FPAReportsDTO, viewModel.Workbook);

            if (grade.Length > 100)
            {
                return Json(new
                {
                    response = new { status = -1, description = "Notas no puede exceder de 100 caracteres." }
                });

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
            FPATestsGeneralDTO cgdTestsGeneralDTO = new()
            {
                Capacity = viewModel.FPAReportsDTO.HeadboardReport.Capacity,
                Comment = viewModel.Comments,
                Creadopor = User.Identity.Name,
                Customer = viewModel.FPAReportsDTO.HeadboardReport.Client,
                BrandOil = viewModel.OilType,
                Date = date,
                Grades = grade,
                Data = viewModel.FPATestsDTO,
                Fechacreacion = DateTime.Now,
                File = pdfFile,
                IdLoad = 0,
                KeyTest = viewModel.ClavePrueba,
                LanguageKey = viewModel.ClaveIdioma,
                LoadDate = DateTime.Now,
                Modificadopor = string.Empty,
                NameFile = string.Concat("FPA", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                Result = viewModel.IsReportAproved,
                SerialNumber = viewModel.NoSerie,
                TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                TypeReport = ReportType.FPA.ToString()
            };
            #endregion

            try
            {
                ApiResponse<long> result = await _fpaClientService.SaveReport(cgdTestsGeneralDTO);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = cgdTestsGeneralDTO.NameFile, file = viewModel.Base64PDF };

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

        public IActionResult Error() => View();

        #region PrivateMethods
        public async Task PrepareForm()
        {
            try
            {
                IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

                // Tipos de prueba
                ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.FPA.ToString(), "-1");
                IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
                {
                    Clave = item.ClavePrueba,
                    Descripcion = item.Descripcion
                });

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