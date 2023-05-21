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
    public class TinController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly ITinClientService _tinClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly ITinService _tinService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProfileSecurityService _profileClientService;
        public TinController(
            IMasterHttpClientService masterHttpClientService,
            ITinClientService tinClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            ITinService tinService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            IProfileSecurityService profileClientService
            )
        {
            this._masterHttpClientService = masterHttpClientService;
            this._tinClientService = tinClientService;
            this._reportClientService = reportClientService;
            this._artifactClientService = artifactClientService;
            this._testClientService = testClientService;
            this._gatewayClientService = gatewayClientService;
            this._tinService = tinService;
            this._correctionFactorService = correctionFactorService;
            this._hostEnvironment = hostEnvironment;
            this._profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(User.Identity.Name).Result;


                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.TensiónInducida)))
                {

                    await this.PrepareForm();
                    var noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return this.View(new TinViewModel { NoSerie = noSerie });
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

            TinViewModel tinViewModel = new();
            noSerie = noSerie.ToUpper().Trim();
            string noSerieSimple = string.Empty;

            ApiResponse<PositionsDTO> dataSelect = await this._gatewayClientService.GetPositions(noSerie);
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await this._testClientService.GetTest(ReportType.TIN.ToString(), "-1");
            //iszViewModel.ListaPruebas = reportResult.Structure.ToList();

            if (dataSelect.Code == -1)
            {
                return this.Json(new
                {
                    response = new ApiResponse<TinViewModel>
                    {
                        Code = -1,
                        Description = "Error al obtener las posiciones para el numero de Serie " + noSerie,
                        Structure = null
                    }
                });
            }
            else
            {
                tinViewModel.Positions = dataSelect.Structure;
            }

            if (!string.IsNullOrEmpty(noSerie))
            {
                noSerieSimple = noSerie.Split("-")[0];
            }
            else
            {
                return this.Json(new
                {
                    response = new ApiResponse<TinViewModel>
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
                    response = new ApiResponse<TinViewModel>
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
                        response = new ApiResponse<TinViewModel>
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
                        response = new ApiResponse<TinViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee características",
                            Structure = null
                        }
                    });
                }
                else
                {
                    tinViewModel.CharacteristicsArtifact = artifactDesing.CharacteristicsArtifact;
                }

                if (artifactDesing.VoltageKV == null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<TinViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no tiene tensiones",
                            Structure = null
                        }
                    });
                }
                else
                {
                    tinViewModel.VoltageKV = artifactDesing.VoltageKV;
                }

                tinViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;

                if (artifactDesing.Derivations == null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<TinViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no tiene devrivaciones",
                            Structure = null
                        }
                    });
                }
            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await this._tinClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.TIN.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.TIN.ToString() };
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

                tinViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Tensión Inducida",
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
                response = new ApiResponse<TinViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = tinViewModel
                }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string nroSerie, string keyTest, string lenguage, string connection, string tension,string comments)
        {
            ApiResponse<SettingsToDisplayTINReportsDTO> result = await this._gatewayClientService.GetTemplateTIM(nroSerie, keyTest, lenguage, connection, tension);

            if (result.Code.Equals(-1))
            {
                return this.View("Excel", new TinViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayTINReportsDTO reportInfo = result.Structure;

            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return this.View("Excel", new TinViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = reportInfo.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");
            int filas = 0;
            CeldasValidate celdas = new CeldasValidate(lenguage);
            this._tinService.PrepareTemplate_Tin(reportInfo, ref workbook, keyTest, lenguage, connection, tension, ref celdas);
            ApiResponse<PositionsDTO> Positions = await this._gatewayClientService.GetPositions(nroSerie);

            return this.View("Excel", new TinViewModel
                {
                    SettingsTIN = reportInfo,
                    ClaveIdioma = lenguage,
                    Pruebas = keyTest,
                    Filas = filas,
                    Conexion = connection,
                    NoPrueba = NoPrueba.ToString(),
                    NoSerie = nroSerie,
                    Workbook = workbook,
                    Error = string.Empty,
                    Celdas = celdas,
                    Positions = Positions.Structure,
                    Tension = tension,
                    Comments = comments
                });
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] TinViewModel viewModel)
        {
            try
            {
                int[] _positionWB;
                object tensionInducida;
                object tensionAplicada;
                object relTension;

                object AT;
                object BT;
                object Ter;

                object devanadoEnergizado;
                object devanadoInducido;
                object tiempo;

                object frecuenciaPrueba;

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsTIN.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("RelTension")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = null;
                relTension = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value;
                if(relTension == null )
                {
                        return this.Json(new
                        {
                            response = new ApiResponse<TinViewModel>
                            {
                                Code = -1,
                                Description = "Favor de proporcionar la relación de tensión",
                                Structure = null
                            }
                        });
                
                }
                
                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsTIN.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosAT")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = null;
                AT = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value;
                if (!string.IsNullOrEmpty(viewModel.Positions.ATNom))
                {
                    if (  AT == null)
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<TinViewModel>
                            {
                                Code = -1,
                                Description = "Favor de proporcionar la posición en AT",
                                Structure = null
                            }
                        });
                    }
       
                }
            

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsTIN.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosBT")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = null;
                BT = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value;
                if (!string.IsNullOrEmpty(viewModel.Positions.BTNom)  )
                {
                    if(BT == null)
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<TinViewModel>
                            {
                                Code = -1,
                                Description = "Favor de proporcionar la posición en BT",
                                Structure = null
                            }
                        });
                    }
                
                }

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsTIN.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosTer")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = null;
                Ter = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value;
                if (!string.IsNullOrEmpty(viewModel.Positions.TerNom) )
                {
                    if(Ter == null)
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<TinViewModel>
                            {
                                Code = -1,
                                Description = "Favor de proporcionar la posición en Terciaria",
                                Structure = null
                            }
                        });
                    }
                
                }

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsTIN.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("FrecPrueba")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = null;
                frecuenciaPrueba = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value;
                if (frecuenciaPrueba == null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<TinViewModel>
                        {
                            Code = -1,
                            Description = "Favor de proporcionar la frecuencia de prueba",
                            Structure = null
                        }
                    });
                }

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsTIN.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tiempo")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = null;
                tiempo = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value;
                if (tiempo == null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<TinViewModel>
                        {
                            Code = -1,
                            Description = "Favor de proporcionar el tiempo",
                            Structure = null
                        }
                    });
                }

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsTIN.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoEner")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = null;
                devanadoEnergizado = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value;
                if (devanadoEnergizado == null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<TinViewModel>
                        {
                            Code = -1,
                            Description = "Favor de proporcionar el devanado energizado",
                            Structure = null
                        }
                    });
                }


                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsTIN.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("DevanadoIndu")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = null;
                devanadoInducido = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value;
                if (devanadoInducido == null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<TinViewModel>
                        {
                            Code = -1,
                            Description = "Favor de proporcionar el devanado inducido",
                            Structure = null
                        }
                    });
                }


                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsTIN.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionAplicada")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = null;
                tensionAplicada = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value;
                if (tensionAplicada == null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<TinViewModel>
                        {
                            Code = -1,
                            Description = "Favor de proporcionar la tensión aplicada",
                            Structure = null
                        }
                    });
                }





                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsTIN.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionInducida")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Validation = null;
                tensionInducida = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value;
                if (tensionInducida == null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<TinViewModel>
                        {
                            Code = -1,
                            Description = "Favor de proporcionar la tensión inducida",
                            Structure = null
                        }
                    });
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

                DateTime basedate = new(1899, 12, 30);



                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsTIN.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
                string fecha = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

                fecha = fecha.Contains("/") ? (DateTime.Now.Date - basedate.Date).TotalDays.ToString() : fecha;
                viewModel.Date = fecha;


                TINTestsGeneralDTO pirGeneralTest = new()
                {
                    Capacity = viewModel.SettingsTIN.HeadboardReport.Capacity,
                    Comment = viewModel.Comments,
                    Creadopor = User.Identity.Name,
                    Customer = viewModel.SettingsTIN.HeadboardReport.Client,
                    Fechacreacion =DateTime.Now,
                    File = pdfFile,
                    IdLoad = 0,
                    KeyTest = viewModel.Pruebas,
                    LanguageKey = viewModel.ClaveIdioma,
                    Date = basedate.AddDays(int.Parse(viewModel.Date)),
                    LoadDate = basedate.AddDays(int.Parse(viewModel.Date)),
                    Modificadopor = null,
                    NameFile = string.Concat("TIN", viewModel.Pruebas, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                    Result = viewModel.IsReportAproved,
                    SerialNumber = viewModel.NoSerie,
                    TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                    TypeReport = ReportType.TIN.ToString(),
                    Connection = viewModel.Conexion,
                    Voltage = !string.IsNullOrEmpty(viewModel.Tension) ? decimal.Parse(viewModel.Tension) : null,
                    RelVoltage = relTension.ToString(),
                    PosAT = AT?.ToString(),
                    PosBT = BT?.ToString(),
                    PosTER = Ter?.ToString(),
                    Frecuency = decimal.Parse(frecuenciaPrueba.ToString()),
                    Time = int.Parse(tiempo.ToString()),
                    EnergizedWinding = devanadoEnergizado.ToString(),
                    InducedWinding = devanadoInducido.ToString(),
                    AppliedVoltage = decimal.Parse(tensionAplicada.ToString()),
                    InducedVoltage = decimal.Parse(tensionInducida.ToString()),
                    Grades = !string.IsNullOrEmpty(viewModel.Notas) ? viewModel.Notas.Trim() : string.Empty,
                };
                string p = JsonConvert.SerializeObject(pirGeneralTest);

                try
                {
                    ApiResponse<long> result = await this._tinClientService.SaveReport(pirGeneralTest);

                    var resultResponse = new { Code = result.Code, Description = result.Description, nameFile = pirGeneralTest.NameFile, file = viewModel.Base64PDF };

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
            catch (Exception ex)
            {
                return null;
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
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione" },
                new GeneralPropertiesDTO() { Clave = "Si", Descripcion = "Si" },
                new GeneralPropertiesDTO { Clave = "No", Descripcion = "No" } }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> conexitems = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "No Aplica", Descripcion = "No Aplica" },
                new GeneralPropertiesDTO() { Clave = "Serie", Descripcion = "Serie" },
                new GeneralPropertiesDTO { Clave = "Paralelo", Descripcion = "Paralelo" } ,
                new GeneralPropertiesDTO { Clave = "Tensión 1", Descripcion = "Tensión 1" } ,
                new GeneralPropertiesDTO { Clave = "Tensión 2", Descripcion = "Tensión 2" }
            }.AsEnumerable();

            // Tipos de prueba

            ApiResponse<IEnumerable<TestsDTO>> reportResult = await this._testClientService.GetTest(ReportType.TIN.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            this.ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");

            this.ViewBag.ConexionItems = new SelectList(conexitems, "Clave", "Descripcion");
            this.ViewBag.IncluyeTerciarioItems = new SelectList(terciarioItems, "Clave", "Descripcion");
            this.ViewBag.NivelTensionItems = new SelectList(origingeneralProperties, "Clave", "Descripcion");

            this.ViewBag.Select1 = new SelectList(new List<GeneralPropertiesDTO>()
            {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" } ,

            }.AsEnumerable(), "Clave", "Descripcion");
            this.ViewBag.Select2 = new SelectList(new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" },

            }.AsEnumerable(), "Clave", "Descripcion");
            this.ViewBag.Select3 = new SelectList(new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" } }.AsEnumerable(), "Clave", "Descripcion");

            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            this.ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");
            this.ViewBag.Grados = new SelectList(gradosCorreccion, "Clave", "Descripcion");
            this.ViewBag.DevanadoEnergizado = new SelectList(devanadaoEnergizado, "Clave", "Descripcion");

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

        private List<PirPruebasDTO> GetPirPruebas(VoltageKVDTO tensiones)
        {
            List<PirPruebasDTO> PirPruebasDTO = new();
            if (tensiones.TensionKvAltaTension1 != null || tensiones.TensionKvAltaTension3 != null)
            {
                decimal? ten1 = null;
                decimal? ten2 = null;

                if (tensiones.TensionKvAltaTension1 != null)
                {
                    ten1 = tensiones.TensionKvAltaTension1;
                }

                if (tensiones.TensionKvAltaTension3 != null)
                {
                    ten2 = tensiones.TensionKvAltaTension3;
                }

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

            if (tensiones.TensionKvBajaTension1 != null || tensiones.TensionKvBajaTension3 != null)
            {
                decimal? ten1 = null;
                decimal? ten2 = null;

                if (tensiones.TensionKvBajaTension1 != null)
                {
                    ten1 = tensiones.TensionKvBajaTension1;
                }

                if (tensiones.TensionKvBajaTension3 != null)
                {
                    ten2 = tensiones.TensionKvBajaTension3;
                }

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

            if (tensiones.TensionKvTerciario1 != null || tensiones.TensionKvTerciario3 != null)
            {
                decimal? ten1 = null;
                decimal? ten2 = null;

                if (tensiones.TensionKvTerciario1 != null)
                {
                    ten1 = tensiones.TensionKvTerciario1;
                }

                if (tensiones.TensionKvTerciario3 != null)
                {
                    ten2 = tensiones.TensionKvTerciario3;
                }

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

            if (PirPruebasDTO.Count > 1)
            {
                PirPruebasDTO.Add(new PirPruebasDTO
                {
                    Abreviatura = "TD",
                    Nombre = "Todas"
                });
            }

            return PirPruebasDTO;
        }
        #endregion
    }
}

