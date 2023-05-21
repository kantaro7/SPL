namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Identity.Web;

    using Newtonsoft.Json;

    using Spire.Pdf;

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
    public class PirController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IPirClientService _pirClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IPirService _pirService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProfileSecurityService _profileClientService;
        public PirController(
            IMasterHttpClientService masterHttpClientService,
            IPirClientService pirClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IPirService pirService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _pirClientService = pirClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _pirService = pirService;
            _correctionFactorService = correctionFactorService;
            _hostEnvironment = hostEnvironment;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.ImpulsoporRayo)))
                {

                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new PirViewModel { NoSerie = noSerie });
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

            PirViewModel pirViewModel = new();
            noSerie = noSerie.ToUpper().Trim();
            string noSerieSimple = string.Empty;

            ApiResponse<PositionsDTO> dataSelect = await _gatewayClientService.GetPositions(noSerie);
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.PIR.ToString(), "-1");
            //iszViewModel.ListaPruebas = reportResult.Structure.ToList();

            if (dataSelect.Code == -1)
            {
                return Json(new
                {
                    response = new ApiResponse<PirViewModel>
                    {
                        Code = -1,
                        Description = "Error al obtener las posiciones para el numero de Serie " + noSerie,
                        Structure = null
                    }
                });
            }
            else
            {
                pirViewModel.Positions = dataSelect.Structure;
            }

            if (!string.IsNullOrEmpty(noSerie))
            {
                noSerieSimple = noSerie.Split("-")[0];
            }
            else
            {
                return Json(new
                {
                    response = new ApiResponse<PirViewModel>
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
                    response = new ApiResponse<PirViewModel>
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
                        response = new ApiResponse<PirViewModel>
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
                        response = new ApiResponse<PirViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee características",
                            Structure = null
                        }
                    });
                }
                else
                {
                    pirViewModel.CharacteristicsArtifact = artifactDesing.CharacteristicsArtifact;
                }

                if (artifactDesing.VoltageKV == null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<PirViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no tiene tensiones",
                            Structure = null
                        }
                    });
                }
                else
                {
                    pirViewModel.VoltageKV = artifactDesing.VoltageKV;
                    pirViewModel.PirPruebasDTO = GetPirPruebas(artifactDesing.VoltageKV);
                }

                pirViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;

                if (artifactDesing.Derivations == null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<PirViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no tiene devrivaciones",
                            Structure = null
                        }
                    });
                }
            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _pirClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.PIR.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.PIR.ToString() };
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

                pirViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Impulso por Rayo",
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
                response = new ApiResponse<PirViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = pirViewModel
                }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetConfigurationFiles(int pIdModule)
        {
            ApiResponse<IEnumerable<FileWeightDTO>> getConfigModulResponse = await _masterHttpClientService.GetConfigurationFiles(pIdModule);
            object resultado = new();
            if (getConfigModulResponse.Code == 1)
            {
                resultado = getConfigModulResponse.Structure.Select(x => new
                {
                    PesoMaximo = int.Parse(x.MaximoPeso),
                    x.ExtensionArchivoNavigation.Extension
                }).ToList();

                return Json(new
                {
                    response = new ApiResponse<object>
                    {
                        Code = 1,
                        Description = "",
                        Structure = resultado
                    }
                });
            }
            else
            {

                return Json(new
                {
                    response = new ApiResponse<object>
                    {
                        Code = getConfigModulResponse.Code,
                        Description = getConfigModulResponse.Description,
                        Structure = null
                    }
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string nroSerie, string keyTest, string lenguage, string connection, string includesTertiary, IFormFile file, string comments)
        {
            List<string> connections = connection.Split(",").ToList();
            nroSerie = nroSerie.ToUpper().Trim();

            bool haveAt = connections.Exists(x => x.Contains("Alta"));
            bool haveBt = connections.Exists(x => x.Contains("Baja"));
            bool haveTer = connections.Exists(x => x.Contains("Terciario"));

            string connectionAt = haveAt ? string.Join(',', connections.FindAll(x => x.Contains("Alta"))) ?? "" : string.Empty;
            string connectionBt = haveBt ? string.Join(',', connections.FindAll(x => x.Contains("Baja"))) ?? "" : string.Empty;
            string connectionTer = haveBt ? string.Join(',', connections.FindAll(x => x.Contains("Terciario"))) ?? "" : string.Empty;
            ApiResponse<SettingsToDisplayPIRReportsDTO> result = await _gatewayClientService.GetTemplatePIR(nroSerie, keyTest, lenguage, connectionAt, connectionBt, connectionTer, includesTertiary);

            if (result.Code.Equals(-1))
            {
                return View("Excel", new PirViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayPIRReportsDTO reportInfo = result.Structure;

            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return View("Excel", new PirViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = reportInfo.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");
            int filas = 0;
            int count = 0;
            _pirService.PrepareTemplate_Pir(reportInfo, ref workbook, keyTest, lenguage, reportInfo.InfotmationArtifact.Derivations, connectionAt, connectionBt, connectionTer, ref count);

            string connectionVal = connectionAt != string.Empty && connectionBt != string.Empty && connectionTer != string.Empty ? "Todas" :
                    connectionAt != string.Empty && connectionBt != string.Empty ? "AT&BT" :
                    connectionAt != string.Empty && connectionTer != string.Empty ? "AT&TER" :
                    connectionBt != string.Empty && connectionTer != string.Empty ? "BT&TER" :
                    connectionAt != string.Empty ? "AT" :
                    connectionBt != string.Empty ? "BT" : "TER";

            return count == 0
                ? View("Excel", new PirViewModel
                {
                    ClaveIdioma = lenguage,
                    Pruebas = keyTest,
                    Filas = filas,
                    NoPrueba = NoPrueba.ToString(),
                    NoSerie = nroSerie,
                    Workbook = workbook,
                    Comments = comments,
                    Error = "No se ha encontrado información de Tipo de Conexion (DELTA / ESTRELLA) para el aparato"

                })
                : (IActionResult)View("Excel", new PirViewModel
                {
                    SettingsPIR = reportInfo,
                    ClaveIdioma = lenguage,
                    Pruebas = keyTest,
                    Filas = filas,
                    Conexion = connectionVal,
                    NivelTension = "1000",
                    IncluyeTerciario = includesTertiary,
                    NoPrueba = NoPrueba.ToString(),
                    NoSerie = nroSerie,
                    Workbook = workbook,
                    Error = string.Empty,
                    Contador = count,
                    Comments = comments
                });
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] PirViewModel viewModel)
        {
            try
            {
                int[] _positionWB = GetRowColOfWorbook(viewModel.SettingsPIR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TotalPagina")).Celda);

                int totalPagina = Convert.ToInt32(viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString());
                if (totalPagina <= 0)
                {
                    return Json(new
                    {
                        response = new ApiResponse<PirViewModel>
                        {
                            Code = -1,
                            Description = "El valor de total de paginas no puede estar vacio",
                            Structure = null
                        }
                    });
                }

                _positionWB = GetRowColOfWorbook(viewModel.SettingsPIR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal")).Celda);
                for (int i = 1; i <= viewModel.Contador; i++)
                {
                    Validation a = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[6].Validation;
                    viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[6].Validation = null;

                    object pagina = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[6].Value;
                    if (pagina == null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<PirViewModel>
                            {
                                Code = -1,
                                Description = "Debe introducir todos los valores de las paginas",
                                Structure = null
                            }
                        });
                    }
                }

                Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

                PdfFormatProvider provider = new()
                {
                    ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
                };
                document.ActiveSheet = document.Worksheets.FirstOrDefault();
                FloatingImage image = null;
                //int rows = viewModel.FPBReportsDTO.TitleOfColumns.Count;

                int rowCount = 0; foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
                {
                    //double officialSize = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(15, 3)].GetFontSize().Value;

                    Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment allign = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(15, 2)].GetHorizontalAlignment().Value;
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

                DateTime basedate = new(1899, 12, 30);

                List<FilesDTO> archivos = new();
                List<Stream> filesStreams = new()
                {
                    pdfStream
                };
                foreach (ArchivosFrontViewModel item in viewModel.Archivos)
                {
                    archivos.Add(new FilesDTO
                    {
                        File = Convert.FromBase64String(item.Base64.Split(',')[1]),
                        Name = item.Name
                    });

                    filesStreams.Add(new MemoryStream(Convert.FromBase64String(item.Base64.Split(',')[1])));
                }

                //PRIMER ARCHIVO BASE64 CARGADO DESDE EL MODAL
                PdfDocumentBase resultaqui = PdfDocument.MergeFiles(filesStreams.ToArray());
                resultaqui.Save(pdfStream, Spire.Pdf.FileFormat.PDF);
                pdfFile = pdfStream.ToArray();

                viewModel.Base64PDF = Convert.ToBase64String(pdfFile);

                List<PIRTestsDetailsDTO> Data = new();

                for (int i = 1; i <= viewModel.Contador; i++)
                {
                    _positionWB = GetRowColOfWorbook(viewModel.SettingsPIR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal")).Celda);
                    object terminal = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value;

                    _positionWB = GetRowColOfWorbook(viewModel.SettingsPIR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pagina")).Celda);
                    object pagina = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + (i - 1)].Cells[_positionWB[1]].Value;

                    Data.Add(new PIRTestsDetailsDTO
                    {
                        Terminal = terminal.ToString(),
                        Page = pagina.ToString()
                    });
                }

                _positionWB = GetRowColOfWorbook(viewModel.SettingsPIR.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
                string fecha = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

                fecha = fecha.Contains("/") ? (DateTime.Now.Date - basedate.Date).TotalDays.ToString() : fecha;
                viewModel.Date = fecha;

                string objVoltaje = viewModel.NivelTension.ToString();

                PIRTestsGeneralDTO pirGeneralTest = new()
                {
                    Capacity = viewModel.SettingsPIR.HeadboardReport.Capacity,
                    Comment = viewModel.Comments,
                    Creadopor = User.Identity.Name,
                    Customer = viewModel.SettingsPIR.HeadboardReport.Client,
                    Fechacreacion = DateTime.Now,
                    File = pdfFile,
                    IdLoad = 0,
                    KeyTest = viewModel.Pruebas,
                    LanguageKey = viewModel.ClaveIdioma,
                    Date = basedate.AddDays(int.Parse(viewModel.Date)),
                    LoadDate = basedate.AddDays(int.Parse(viewModel.Date)),
                    Modificadopor = null,
                    Fechamodificacion = null,
                    NameFile = string.Concat("PIR", viewModel.Pruebas, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                    Result = true,
                    SerialNumber = viewModel.NoSerie,
                    TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                    TypeReport = ReportType.PIR.ToString(),
                    Connection = viewModel.Conexion,
                    TotalPags = totalPagina,
                    IncludeTertiary = viewModel.IncluyeTerciario,
                    Data = Data,
                    Files = archivos,
                    VoltageLevel = viewModel.NivelTension == "Todas" ? "Todas" : null,
                    Voltage = viewModel.NivelTension == "Todas" ? 0 : decimal.Parse(viewModel.NivelTension),

                };
                string p = JsonConvert.SerializeObject(pirGeneralTest);

                try
                {
                    ApiResponse<long> result = await _pirClientService.SaveReport(pirGeneralTest);

                    var resultResponse = new { result.Code, result.Description, nameFile = pirGeneralTest.NameFile, file = viewModel.Base64PDF };

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
            catch (Exception)
            {
                return null;
            }
        }

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

            //var a = await this._gatewayClientService.GetPositions("G4135-01");

            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> gradosCorreccion = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } ,
                new GeneralPropertiesDTO { Clave = "75", Descripcion = "75" } ,
                new GeneralPropertiesDTO { Clave = "85", Descripcion = "85" },
                new GeneralPropertiesDTO { Clave = "Otro", Descripcion = "Otro" }
            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> devanadaoEnergizado = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } ,
                new GeneralPropertiesDTO { Clave = "AT", Descripcion = "AT" } ,
                new GeneralPropertiesDTO { Clave = "BT", Descripcion = "BT" },
                new GeneralPropertiesDTO { Clave = "Ter", Descripcion = "Ter" }
            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> terciarioItems = new List<GeneralPropertiesDTO>() {
                  new GeneralPropertiesDTO { Clave = "No", Descripcion = "No" },
             new GeneralPropertiesDTO() { Clave = "Si", Descripcion = "Si" } }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> conexitems = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione" },
                new GeneralPropertiesDTO() { Clave = "AT", Descripcion = "Alta Tensión" },
                new GeneralPropertiesDTO { Clave = "BT", Descripcion = "Baja Tensión" } ,
                new GeneralPropertiesDTO { Clave = "Ter", Descripcion = "Terciario" } ,
                new GeneralPropertiesDTO { Clave = "TD", Descripcion = "Todas" }
            }.AsEnumerable();

            // Tipos de prueba

            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.PIR.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");

            ViewBag.ConexionItems = new SelectList(conexitems, "Clave", "Descripcion");
            ViewBag.IncluyeTerciarioItems = new SelectList(terciarioItems, "Clave", "Descripcion");
            ViewBag.NivelTensionItems = new SelectList(origingeneralProperties, "Clave", "Descripcion");

            ViewBag.Select1 = new SelectList(new List<GeneralPropertiesDTO>()
            {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" } ,

            }.AsEnumerable(), "Clave", "Descripcion");
            ViewBag.Select2 = new SelectList(new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" },

            }.AsEnumerable(), "Clave", "Descripcion");
            ViewBag.Select3 = new SelectList(new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" } }.AsEnumerable(), "Clave", "Descripcion");

            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");
            ViewBag.Grados = new SelectList(gradosCorreccion, "Clave", "Descripcion");
            ViewBag.DevanadoEnergizado = new SelectList(devanadaoEnergizado, "Clave", "Descripcion");

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

        private List<PirPruebasDTO> GetPirPruebas(VoltageKVDTO tensiones)
        {
            List<PirPruebasDTO> PirPruebasDTO = new();
            if (tensiones.TensionKvAltaTension1 is not null and not 0 || tensiones.TensionKvAltaTension3 is not null and not 0)
            {
                decimal ten1 = tensiones.TensionKvAltaTension1 ?? 0;
                decimal ten2 = tensiones.TensionKvAltaTension3 ?? 0;

                PirPruebasDTO.Add(new PirPruebasDTO
                {
                    Abreviatura = "AT",
                    Nombre = "Alta Tension",
                    Tensiones = new TensionesDTO
                    {
                        Tension1 = ten1,
                        Tension2 = ten2
                    }
                });
            }

            if (tensiones.TensionKvBajaTension1 is not null and not 0 || tensiones.TensionKvBajaTension3 is not null and not 0)
            {
                decimal ten1 = tensiones.TensionKvBajaTension1 ?? 0;
                decimal ten2 = tensiones.TensionKvBajaTension3 ?? 0;

                PirPruebasDTO.Add(new PirPruebasDTO
                {
                    Abreviatura = "BT",
                    Nombre = "Baja Tension",
                    Tensiones = new TensionesDTO
                    {
                        Tension1 = ten1,
                        Tension2 = ten2
                    }
                });
            }

            if (tensiones.TensionKvTerciario1 is not null and not 0 || tensiones.TensionKvTerciario3 is not null and not 0)
            {
                decimal ten1 = tensiones.TensionKvTerciario1 ?? 0;
                decimal ten2 = tensiones.TensionKvTerciario3 ?? 0;

                PirPruebasDTO.Add(new PirPruebasDTO
                {
                    Abreviatura = "Ter",
                    Nombre = "Terciario",
                    Tensiones = new TensionesDTO
                    {
                        Tension1 = ten1,
                        Tension2 = ten2
                    }
                });
            }

            return PirPruebasDTO;
        }
        #endregion
    }
}

