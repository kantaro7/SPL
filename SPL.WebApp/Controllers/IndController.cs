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
    public class IndController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IIndClientService _indClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IIndService _indService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IRodClientService _rodClientService;
        private readonly IPceClientService _pceService;
        private readonly IProfileSecurityService _profileClientService;
        public IndController(
            IMasterHttpClientService masterHttpClientService,
            IIndClientService indClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IIndService indService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            IRodClientService rodClientService,
            IPceClientService pceService,
            IProfileSecurityService profileClientService
            )
        {
            this._masterHttpClientService = masterHttpClientService;
            this._indClientService =indClientService;
            this._reportClientService = reportClientService;
            this._artifactClientService = artifactClientService;
            this._testClientService = testClientService;
            this._gatewayClientService = gatewayClientService;
            this._indService = indService;
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


                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.Indice2019)))
                {


                    await this.PrepareForm();
                    var noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return this.View(new IndViewModel { NoSerie = noSerie });
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
                IndViewModel pciViewModel = new();
                noSerie = noSerie.ToUpper().Trim();
                string noSerieSimple = string.Empty;

                ApiResponse<PositionsDTO> dataSelect = await this._gatewayClientService.GetPositions(noSerie);

                pciViewModel.Positions = dataSelect.Structure;

                if (!string.IsNullOrEmpty(noSerie))
                {
                    noSerieSimple = noSerie.Split("-")[0];
                }
                else
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<ArfViewModel>
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
                        response = new ApiResponse<ArfViewModel>
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
                            response = new ApiResponse<ArfViewModel>
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
                            response = new ApiResponse<ArfViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee características",
                                Structure = null
                            }
                        });
                    }
                   
                    pciViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;

                }

                ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await this._indClientService.GetFilter(noSerie);

                if (resultFilter.Code.Equals(1))
                {
                    InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.IND.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.IND.ToString() };
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
                        text = "Indice 2019",
                        expanded = true,
                        hasChildren = true,
                        spriteCssClass = "rootfolder",
                        status = null,
                        items = reportsAprooved
                    }
                };
                }

                return this.Json(new
                {
                    response = new ApiResponse<IndViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = pciViewModel
                    }
                });
            }
            catch(Exception e)
            {
                return this.Json(new
                {
                    response = new ApiResponse<TdpViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = null
                    }
                });
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string nroSerie, string keyTest, string lenguage, string comments, string tcBuyers)
        {
            ApiResponse<PositionsDTO> resultP = await this._gatewayClientService.GetPositions(nroSerie);


            if (resultP.Code.Equals(-1))
            {
                return this.View("Excel", new IndViewModel
                {
                    Error = resultP.Description
                });
            }
       

            ApiResponse<SettingsToDisplayINDReportsDTO> result = await this._gatewayClientService.GetTemplateIND(nroSerie, keyTest, lenguage, tcBuyers);

            if (result.Code.Equals(-1))
            {
                return this.View("Excel", new IndViewModel
                {
                    Error = result.Description
                });
            }

            if (result.Structure.BaseTemplate == null)
            {
                return this.View("Excel", new IndViewModel
                {
                    Error = "No se ha encontrado plantilla para reporte IND"
                });
            }

            if (string.IsNullOrEmpty(result.Structure.BaseTemplate.Plantilla))
            {
                return this.View("Excel", new IndViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            if (result.Structure.ConfigurationReports.Count() == 0)
            {
                return this.View("Excel", new IndViewModel
                {
                    Error = "No existen columnas configurables"
                });
            }


            long NoPrueba = result.Structure.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(result.Structure.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");
            SettingsToDisplayINDReportsDTO settings = result.Structure;

            this._indService.PrepareTemplate(settings, ref workbook, keyTest,lenguage, tcBuyers) ;

            return this.View("Excel", new IndViewModel
            {
                ClaveIdioma = lenguage,
                Pruebas = keyTest,
                NoPrueba = NoPrueba.ToString(),
                NoSerie = nroSerie,
                Workbook = workbook,
                Error = string.Empty,
                Comments = comments,
                Positions = resultP.Structure,
                SettingsIND = settings,
                LlevaTC = tcBuyers
            });
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] IndViewModel viewModel)
        {
            try
            {
                viewModel.Workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Validation = null));
                Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();
                List<INDTestsDetailsDTO> data = new List<INDTestsDetailsDTO>();
                int totalPag = 0;
                string anexo = string.Empty;
                string error = this._indService.ValidateTemplateIND(viewModel.SettingsIND,viewModel.Workbook,viewModel.Pruebas,viewModel.LlevaTC, ref data, ref totalPag, ref anexo);

                if (!string.IsNullOrEmpty(error))
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<IndViewModel>
                        {
                            Code = -1,
                            Description = "Faltan datos por ingresar en la tabla"
                        }
                    });
                }


                PdfFormatProvider provider = new()
                {
                    ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
                };
                document.ActiveSheet = document.Worksheets.FirstOrDefault();
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

                DateTime basedate = new(1899, 12, 30);
                int[] _positionWB = this.GetRowColOfWorbook(viewModel.SettingsIND.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
                string fecha = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

                fecha = fecha.Contains("/") ? (DateTime.Now.Date - basedate.Date).TotalDays.ToString() : fecha;
                viewModel.Date = fecha;





                INDTestsGeneralDTO pirGeneralTest = new()
                {
                    Capacity = viewModel.SettingsIND.HeadboardReport.Capacity,
                    Comment = viewModel.Comments,
                    Creadopor = User.Identity.Name,
                    Customer = viewModel.SettingsIND.HeadboardReport.Client,
                    Fechacreacion = DateTime.Now,
                    File = pdfFile,
                    IdLoad = 0,
                    KeyTest = viewModel.Pruebas,
                    LanguageKey = viewModel.ClaveIdioma,
                    Date = basedate.AddDays(int.Parse(viewModel.Date)),
                    LoadDate = basedate.AddDays(int.Parse(viewModel.Date)),
                    Modificadopor = null,
                    NameFile = string.Concat("IND", viewModel.Pruebas, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba),"-" + rowCount, ".pdf"),
                    Result = true,
                    SerialNumber = viewModel.NoSerie,
                    TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                    TypeReport = ReportType.IND.ToString(),
                    Files = archivos,
                    Data =data,
                    TcPurchased = viewModel.LlevaTC == "Select" ? null : viewModel.LlevaTC,
                    TotalPags = totalPag,
                    Exhibit = anexo
                };



                try
                {
                    ApiResponse<long> result = await this._indClientService.SaveReport(pirGeneralTest);

                    var resultResponse = new { status = result.Code, description = result.Description, nameFile = pirGeneralTest.NameFile, file = viewModel.Base64PDF };

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
            catch(Exception e)
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

        [HttpGet]
        public async Task<IActionResult> GetConfigurationFiles(int pIdModule)
        {
            ApiResponse<IEnumerable<FileWeightDTO>> getConfigModulResponse = await this._masterHttpClientService.GetConfigurationFiles(pIdModule);
            object resultado = new();
            if (getConfigModulResponse.Code == 1)
            {
                resultado = getConfigModulResponse.Structure.Select(x => new
                {
                    PesoMaximo = int.Parse(x.MaximoPeso),
                    x.ExtensionArchivoNavigation.Extension
                }).ToList();

                return this.Json(new
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

                return this.Json(new
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

        public IActionResult Error() => this.View();

        #region PrivateMethods
        public async Task PrepareForm()
        {

            /// var a = await this._gatewayClientService.GetPositions("G4135-01");

            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();


            IEnumerable<GeneralPropertiesDTO> llevaTC = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "Select", Descripcion = "Seleccione..." },
                new GeneralPropertiesDTO() { Clave = "Si", Descripcion = "Si" },
                new GeneralPropertiesDTO { Clave = "No", Descripcion = "No" } ,
          
            }.AsEnumerable();


            // Tipos de prueba
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await this._testClientService.GetTest(ReportType.IND.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            this.ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");
            this.ViewBag.LlevaTC = new SelectList(llevaTC, "Clave", "Descripcion");

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

