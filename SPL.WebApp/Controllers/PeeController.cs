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
    using Spire.Pdf;
    using SPL.WebApp.Helpers;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using Microsoft.Identity.Web;

    /// <summary>
    /// Relación de Transformación
    /// </summary>
    public class PeeController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IPeeClientService _peeClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IPeeService _peeService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IResistanceTwentyDegreesClientServices _resistanceTwentyDegreesClientServices;
        private readonly string _clavePrueba = "ALL";
        private readonly IProfileSecurityService _profileClientService;
        public PeeController(
            IMasterHttpClientService masterHttpClientService,
            IPeeClientService peeClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IPeeService peeService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            ISidcoClientService sidcoClientService,
            IResistanceTwentyDegreesClientServices resistanceTwentyDegreesClientServices,
            IProfileSecurityService profileClientService
            )
        {
            this._masterHttpClientService = masterHttpClientService;
            this._peeClientService = peeClientService;
            this._reportClientService = reportClientService;
            this._artifactClientService = artifactClientService;
            this._testClientService = testClientService;
            this._gatewayClientService = gatewayClientService;
            this._peeService = peeService;
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


                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.PérdidasdelEquipodeEnfriamiento)))
                {

                    await this.PrepareForm();
                    var noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return this.View(new PeeViewModel { NoSerie = noSerie });
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
            PeeViewModel peeViewModel = new();
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
                    response = new ApiResponse<PeeViewModel>
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
                    response = new ApiResponse<PeeViewModel>
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
                        response = new ApiResponse<PeeViewModel>
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
                        response = new ApiResponse<PeeViewModel>
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

                peeViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await this._peeClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.PEE.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.PEE.ToString() };
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

                peeViewModel.TreeViewItem = new List<TreeViewItemDTO>()
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
                response = new ApiResponse<PeeViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = peeViewModel
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
        public async Task<IActionResult> GetTemplate(string noSerie,  string claveIdioma, string comment)
        {
            noSerie = noSerie.ToUpper().Trim();

          

            ApiResponse<SettingsToDisplayPEEReportsDTO> result = await this._gatewayClientService.GetTemplate(noSerie, _clavePrueba, claveIdioma);

            if (result.Code.Equals(-1))
            {
                return this.View("Excel", new PeeViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayPEEReportsDTO reportInfo = result.Structure;

            #region Decode Template
            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return this.View("Excel", new PeeViewModel
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
                this._peeService.PrepareTemplate_PEE(reportInfo, ref workbook);
            }
            catch (Exception)
            {
                return this.View("Excel", new PeeViewModel
                {
                    Error = "La plantilla esta mal configurada por favor verifiquela de inmediato"
                });
            }
            #endregion

            return this.View("Excel", new PeeViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = _clavePrueba,
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                PEEReportsDTO = result.Structure,
                Workbook = workbook,
                Error = string.Empty,
                Comments = comment
            });
        }

      
        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] PeeViewModel viewModel)
        {

            PEETestsDTO peeTestDTO = new()
            {
                KeyTest = viewModel.ClavePrueba,
                PEETestsDetails = new List<PEETestsDetailsDTO>()
            };

            if (!_peeService.Verify_PEE_ColumnsToCalculate(viewModel.PEEReportsDTO, viewModel.Workbook))
            {
                return this.Json(new
                {
                    response = new { Code = -2, Description = "Faltan datos por ingresar en la tabla" }
                });
            }

            this._peeService.Prepare_PEE_Test(viewModel.PEEReportsDTO, viewModel.Workbook, ref peeTestDTO);

            // Prepare data
            WarrantiesArtifactDTO gar = viewModel.PEEReportsDTO.InfotmationArtifact.WarrantiesArtifact;
            List<string> errores = new();
            foreach (PEETestsDetailsDTO item in peeTestDTO.PEETestsDetails)
            {
                CharacteristicsArtifactDTO cooling = viewModel.PEEReportsDTO.InfotmationArtifact.CharacteristicsArtifact.FirstOrDefault(x => x.CoolingType.ToUpper().Equals(item.CoolingType));
                List<decimal> mvafs = new()
                {
                    cooling.Mvaf1 ?? 0,
                    cooling.Mvaf2 ?? 0,
                    cooling.Mvaf3 ?? 0,
                    cooling.Mvaf4 ?? 0
                };
                _ = mvafs.RemoveAll(x => x == 0);

                if(mvafs.Exists(x => x == (gar.Kwaux4 ?? 0))){
                    item.Mvaaux_gar = gar.Kwaux4 ?? 0;
                    item.Kwaux_gar = gar.Kwaux2 ?? 0;
                }else if (mvafs.Exists(x => x == (gar.Kwaux3 ?? 0)))
                {
                    item.Mvaaux_gar = gar.Kwaux3 ?? 0;
                    item.Kwaux_gar = gar.Kwaux1 ?? 0;
                }
                else
                {

                    errores.Add($"El tipo de enfriamiento {item.CoolingType} no posee mvaf que sea igual a Kwaux4: {gar.Kwaux4 ?? 0} o Kwaux3: {gar.Kwaux3 ?? 0}");
                    break;
                }
            
            }

            ApiResponse<ResultPEETestsDTO> resultTestPEE_Response = await this._peeClientService.CalculateTestPEE(peeTestDTO);

            if (resultTestPEE_Response.Code.Equals(-1))
            {
                errores.Add(resultTestPEE_Response.Description);
            }
            else if(resultTestPEE_Response.Code.Equals(5))
            {
                return this.Json(new
                {
                    response = new ApiResponse<PeeViewModel>
                    {
                        Code = 5,
                        Description = resultTestPEE_Response.Description,
                        Structure = viewModel
                    }
                });
            }

            if(resultTestPEE_Response.Structure != null)
            {
                resultTestPEE_Response.Structure.Results.ForEach(x => errores.Add(x.Message));
            }

            Workbook workbook = viewModel.Workbook;

            this._peeService.PrepareIndexOfPEE(resultTestPEE_Response.Structure, viewModel.PEEReportsDTO, ref workbook, viewModel.ClaveIdioma);

            #region fillColumns
            peeTestDTO = resultTestPEE_Response.Structure.PEETests;
            #endregion

            viewModel.PeeTestDTO = peeTestDTO;
            viewModel.Workbook = workbook;

            string errors = string.Empty;
            List<string> errorMessages = errores.Select(k => k).ToList();
            string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
            return this.Json(new
            {
                response = new ApiResponse<PeeViewModel>
                {
                    Code = 1,
                    Description = allError,
                    Structure = viewModel
                }
            });
        }

        [HttpPost]
        
        public async Task<IActionResult> SavePDF([FromBody] PeeViewModel viewModel)
        {
            try
            {


                #region Export Excel
                int[] filas = new int[2];
                int[] _positionWB;
                string reportResult = string.Empty;
                if (!_peeService.Verify_PEE_Columns(viewModel.PEEReportsDTO, viewModel.Workbook))
                {
                    return this.Json(new
                    {
                        response = new { status = -2, description = "Faltan datos por ingresar en la tabla" }
                    });
                }

                _positionWB = this.GetRowColOfWorbook(viewModel.PEEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
                reportResult = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();


                if (reportResult == "Accepted" || reportResult == "Aceptado")
                {
                    viewModel.IsReportAproved = true;
                }
                else
                {
                    viewModel.IsReportAproved = false;
                }

                DateTime date = this._peeService.GetDate(viewModel.Workbook, viewModel.PEEReportsDTO);

                Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

                PdfFormatProvider provider = new()
                {
                    ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
                };
                document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.Workbook.ActiveSheet);
                FloatingImage image = null;
                string val;
                int rowCount = 0; foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
                {
                    rowCount = sheet.UsedCellRange.RowCount;
                    _positionWB = this.GetRowColOfWorbook(viewModel.PEEReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TipoEnf")).Celda);
                    string coolingType = "Frio";
                    int row = 0;
                    while (!string.IsNullOrEmpty(coolingType))
                    {
                        // Voltage
                        val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + 1)].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + 1)].SetValueAsText(this.FormatStringDecimal(val, 3));

                        // Current
                        val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + 2)].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + 2)].SetValueAsText(this.FormatStringDecimal(val, 3));

                        // Power
                        val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + 3)].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + 3)].SetValueAsText(this.FormatStringDecimal(val, 3));

                        coolingType = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row + 1, _positionWB[1])].GetValue().Value.RawValue;
                        row++;
                    }

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

                pdfFile = pdfStream.ToArray();
                viewModel.Base64PDF = Convert.ToBase64String(pdfFile);


                List<Stream> filesStreams = new List<Stream>();
                filesStreams.Add(pdfStream);

                PdfDocumentBase resultaqui = PdfDocument.MergeFiles(filesStreams.ToArray());
                resultaqui.Save(pdfStream, Spire.Pdf.FileFormat.PDF);
                pdfFile = pdfStream.ToArray();
                #endregion

                #region Save Report
                PEETestsGeneralDTO peeTestsGeneralDTO = new()
                {
                    Capacity = viewModel.PEEReportsDTO.HeadboardReport.Capacity,
                    Comment = viewModel.Comments,
                    Creadopor = User.Identity.Name,
                    Customer = viewModel.PEEReportsDTO.HeadboardReport.Client,
                    PEETests = viewModel.PeeTestDTO,
                    Fechacreacion = DateTime.Now,
                    File = pdfFile,
                    IdLoad = 0,
                    KeyTest = _clavePrueba,
                    LanguageKey = viewModel.ClaveIdioma,
                    LoadDate = date,
                    Modificadopor = null,
                    Fechamodificacion = null,
                    NameFile = string.Concat("PEE", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                    Result = viewModel.IsReportAproved,
                    SerialNumber = viewModel.NoSerie,
                    TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                    TypeReport = ReportType.PEE.ToString()
                };
                #endregion

                try
                {
                    string a = JsonConvert.SerializeObject(peeTestsGeneralDTO);
                    ApiResponse<long> result = await this._peeClientService.SaveReport(peeTestsGeneralDTO);

                    var resultResponse = new { status = result.Code, description = result.Description, nameFile = peeTestsGeneralDTO.NameFile, file = peeTestsGeneralDTO.File };

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
                return this.Json(new
                {
                    response = new ApiResponse<PeeViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }
        }
        public IActionResult Error() => this.View();

        #region PrivateMethods
        public async Task PrepareForm()
        {
            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            // Tipos de prueba
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await this._testClientService.GetTest(ReportType.PEE.ToString(), "-1");
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
                    if(decimals != 0)
                    {
                        num += ".".PadRight(decimals+1, '0');
                    }
                }
            }
            return num;
        }
        #endregion
    }
}