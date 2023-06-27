namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Identity.Web;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ETD;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.Helpers;
    using SPL.WebApp.ViewModels;

    using Telerik.Web.Spreadsheet;

    public class ETDFileController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IEtdClientService _etdClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IEtdService _etdService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IResistanceTwentyDegreesClientServices _resistanceTwentyDegreesClientServices;
        private readonly IProfileSecurityService _profileClientService;

        public ETDFileController(
            IMasterHttpClientService masterHttpClientService, IProfileSecurityService profileClientService, IArtifactClientService artifactClientService,
                    IWebHostEnvironment hostEnvironment,
            IGatewayClientService gatewayClientService, IEtdService edtService)
        {
            this._masterHttpClientService = masterHttpClientService;
            this._profileClientService = profileClientService;
            this._artifactClientService = artifactClientService;
            this._gatewayClientService = gatewayClientService;
            this._hostEnvironment = hostEnvironment;
            this._etdService = edtService;
        }

        public async Task<IActionResult> Index()
        {
            EtdFileViewModel model = new() { };
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(this.User.Identity.Name).Result;
                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.Resistenciaa20C)))
                {

                    await this.PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(this.Request.Query);
                    return this.View(model);
                }
                else
                {
                    return this.View("~/Views/PageConstruction/PermissionDenied.cshtml");
                }
            }
            catch (MicrosoftIdentityWebChallengeUserException)
            {
                return this.View("~/Views/PageConstruction/Error.cshtml");

            }
            catch (Exception)
            {

                return this.View("~/Views/PageConstruction/Error.cshtml");

            }
        }

        public async Task<IActionResult> DownloadFile(string noSerie, string clavePrueba, string claveIdioma, string posAT, string posBT, string posTer, string coolingType, string otherCoolingType, decimal capacity, decimal otherCapacity, decimal altitud1, string altitud2, string clientName, string reportCapacities, decimal grados)
        {
            try
            {
                noSerie = noSerie.ToUpper().Trim();
                ApiResponse<SettingsToDisplayETDReportsDTO> result = await this._gatewayClientService.GetDownloadTemplateETD(nroSerie: noSerie, clavePrueba, lenguage: claveIdioma);

                if (result.Code.Equals(-1))
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<string>
                        {
                            Code = -1,
                            Description = "Error",
                            Structure = result.Description
                        }
                    });

                }

                SettingsToDisplayETDReportsDTO reportInfo = result.Structure;

                #region Decode Template
                if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<string>
                        {
                            Code = -1,
                            Description = "Error",
                            Structure = "No existe plantilla para el filtro seleccionado"
                        }
                    });

                }

                byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);

                Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook;
                // the XLSX format provider is used for demo purposes and can be replaced with any format provider implementing the IWorkbookFormatProvider interface
                //XlsxFormatProvider formatProvider = new XlsxFormatProvider();

                Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();

                using (Stream input = new MemoryStream(bytes))
                //using (Stream input = new FileStream(path, FileMode.Open))
                {
                    workbook = formatProvider.Import(input);
                }

                Dictionary<string, ParamETD> parameters = new()
                {
                {"txtNoSerie", new ParamETD(noSerie)},
                {"cmbPosAT", new ParamETD(posAT)},
                {"cmbPosBT", new ParamETD(posBT)},
                {"cmbPosTer", new ParamETD(string.IsNullOrEmpty(posTer) ? "NA" : posTer)},
                {"txtAltitudF1", new ParamETD(altitud1)},
                {"txtCapacidad", new ParamETD(capacity <= 0 ? otherCapacity : capacity)},
                {"cmbTipoEnfriamiento", new ParamETD(coolingType.ToUpper().Equals("OTRO") ? otherCoolingType : coolingType)},
                {"txtCliente", new ParamETD(clientName)},
                {"txtCapacidadesRep", new ParamETD(reportCapacities)}
            };

                if (altitud2.Equals("FASL"))
                    parameters.Add("txtAltitudF2", new ParamETD(1));
                else if (altitud2.Equals("MSNM"))
                    parameters.Add("txtAltitudF2", new ParamETD(2));
                else
                    parameters.Add("txtAltitudF2", new ParamETD(""));

                if (coolingType.ToUpper().Equals("OTRO"))
                    parameters.Add("txtSETipoEnfriamiento", new ParamETD(""));
                else
                    parameters.Add("txtSETipoEnfriamiento", new ParamETD(grados));

                try
                {
                    string errors = this._etdService.PrepareDownloadTemplate_ETD(reportInfo, parameters, ref workbook);
                    if (!string.IsNullOrEmpty(errors))
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<string>
                            {
                                Code = -1,
                                Description = "Error",
                                Structure = errors
                            }
                        });
                    }
                }
                catch (Exception)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<string>
                        {
                            Code = -1,
                            Description = "Error",
                            Structure = "La plantilla esta mal configurada por favor verifiquela de inmediato"
                        }
                    });
                }

                #endregion
                try
                {
                    //// Generando Excel
                    //using (MemoryStream streamm = new())
                    //{
                    //    formatProvider.Export(workbook, output: streamm);
                    //    file = Convert.ToBase64String(streamm.ToArray());
                    //}

                    //FormatHelper.CultureHelper = new SpreadsheetCultureHelper(new CultureInfo("en-US"));

                    // save the current culture
                    //var currentCulture = Thread.CurrentThread.CurrentCulture;
                    //var currentUICulture = Thread.CurrentThread.CurrentUICulture;
                    //// set the current culture to English
                    //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
                    //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
                    //// load the file
                    //MySheet.Provider = provider;
                    //MySheet.DataBind();
                    // restore the current culture
                    //Thread.CurrentThread.CurrentCulture = currentCulture;
                    //Thread.CurrentThread.CurrentUICulture = currentUICulture;
                    //workbook.Styles
                    byte[] bytes1;
                    using (MemoryStream output = new())
                    {
                        formatProvider.Export(workbook, output);
                        bytes1 = output.ToArray();
                    }

                    Stream stream = new MemoryStream(bytes1);

                    Telerik.Web.Spreadsheet.Workbook workbook2 = Telerik.Web.Spreadsheet.Workbook.Load(input: stream, ".xlsx");

                    for (int j = 0; j < workbook2.Sheets.Count; j++)
                    {
                        for (int i = 0; i < workbook2.Sheets[j].Rows.Count; i++)
                        {
                            if (workbook2.Sheets[j].Rows[i] != null)
                            {
                                if (workbook2.Sheets[j].Rows[i].Cells != null)
                                {
                                    foreach (Cell cell in workbook2.Sheets[j].Rows[i].Cells)
                                    {
                                        if (cell != null && cell.Formula != null)
                                        {
                                            cell.Formula = cell.Formula.ToString().Replace(";", ",");
                                        }
                                        else
                                        {
                                            //data += "null,";
                                        }
                                    }
                                }
                                else
                                {
                                    //data += "null";
                                }
                            }
                            //data += Environment.NewLine;
                        }
                    }



                    string base64FromByteArray = Convert.ToBase64String(bytes1);

                    return this.Json(new
                    {
                        response = new ApiResponse<Telerik.Web.Spreadsheet.Workbook>
                        {
                            Code = 1,
                            Description = "file",
                            Structure = workbook2
                        }
                    });
                }
                catch (Exception ex)
                {

                    return this.Json(new
                    {
                        response = new ApiResponse<string>
                        {
                            Code = -1,
                            Description = "Error",
                            Structure = ex.Message + ex.InnerException.ToString()
                        }
                    });
                }
            }
            catch (Exception ex)
            {

                return this.Json(new
                {
                    response = new ApiResponse<string>
                    {
                        Code = -1,
                        Description = "Error",
                        Structure = ex.Message + ex.InnerException.ToString()
                    }
                });
            }
        }
        //hacer una funcion que lea un excel y procese los datos

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

            position[0] = System.Convert.ToInt32(col);

            for (int i = 0; i < row.Length; i++)
            {
                position[1] += char.ToUpper(row[i]) - 64;
            }

            position[0] += -1;
            position[1] += -1;

            return position;
        }

        //[HttpPost]
        //public async Task<IActionResult> SavePdf([FromBody] EtdFileViewModel viewModel)
        //{
        //    try
        //    {

        //        try
        //        {

        //            byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Barboza\Downloads\ETD.xlsx");
        //            Stream stream2 = new MemoryStream(bytes);

        //            Workbook workbook = Workbook.Load(stream2, ".xlsx");
        //            Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = workbook.ToDocument();

        //            //Se busca la  hoja del reporte donde el nombre sea Rep.F3
        //            document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == "Rep.F3");
        //            var p2 = document.ActiveSheet;
        //            //Por alguna razon no setea el sheet cuando la hoja hace match

        //            foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
        //            {

        //                if (sheet.Name != "Rep.F3")
        //                {
        //                    //se ocultan las hojas que no interesan
        //                    sheet.Visibility = Telerik.Windows.Documents.Spreadsheet.Model.SheetVisibility.Hidden;
        //                    //document.Worksheets.Remove(sheet);
        //                }
        //                else
        //                {
        //                    /***************************SE AGREGAN LOS LOGOS A LOS HEADERS DE CADA HOJA *****************************************/
        //                    var image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(0, 0), 0, 0);
        //                    string path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
        //                    FileStream stream = new(path, FileMode.Open);
        //                    using (stream)
        //                    {
        //                        image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
        //                    }

        //                    image.Width = 215;
        //                    image.Height = 38;

        //                    sheet.Shapes.Add(image);

        //                    image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(59, 0), 0, 0);
        //                    path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
        //                    stream = new(path, FileMode.Open);
        //                    using (stream)
        //                    {
        //                        image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
        //                    }

        //                    image.Width = 215;
        //                    image.Height = 38;
        //                    sheet.Shapes.Add(image);

        //                    image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(118, 0), 0, 0);
        //                    path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
        //                    stream = new(path, FileMode.Open);
        //                    using (stream)
        //                    {
        //                        image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
        //                    }

        //                    image.Width = 215;
        //                    image.Height = 38;
        //                    sheet.Shapes.Add(image);

        //                    /**********************************************************************************************************/

        //                    image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(81, 7), 0, 0);
        //                    image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(Convert.FromBase64String(viewModel.Img64.Split(",")[1]), "jpg");

        //                    image.Width = 400;
        //                    image.Height = 400;
        //                    sheet.Shapes.Add(image);

        //                    image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(139, 7), 0, 0);
        //                    image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(Convert.FromBase64String(viewModel.Img64.Split(",")[1]), "jpg");
        //                    image.Width = 400;
        //                    image.Height = 400;
        //                    sheet.Shapes.Add(image);

        //                    PageBreaks pageBreaks = sheet.WorksheetPageSetup.PageBreaks;

        //                    _ = pageBreaks.TryInsertHorizontalPageBreak(59, 0);
        //                    _ = pageBreaks.TryInsertHorizontalPageBreak(118, 0);
        //                    // _ = pageBreaks.TryInsertHorizontalPageBreak(59, 0);
        //                    //sheet.Rows.Insert(58);
        //                    sheet.WorksheetPageSetup.PaperType = Telerik.Windows.Documents.Model.PaperTypes.A4;
        //                    sheet.WorksheetPageSetup.PageOrientation = Telerik.Windows.Documents.Model.PageOrientation.Portrait;
        //                    sheet.WorksheetPageSetup.CenterHorizontally = true;
        //                    sheet.WorksheetPageSetup.PrintOptions.PrintGridlines = false;
        //                    sheet.WorksheetPageSetup.ScaleFactor = new Telerik.Documents.Primitives.Size(0.9, 1);
        //                    sheet.WorksheetPageSetup.Margins =
        //                        new Telerik.Windows.Documents.Spreadsheet.Model.Printing.PageMargins(0
        //                        , 20, 0, 0);
        //                }

        //            }

        //            Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();
        //            byte[] excelFile;
        //            byte[] pdfFile;
        //            string file;
        //            // Generando Excel
        //            using (MemoryStream stream = new())
        //            {
        //                formatProvider.Export(document, stream);
        //                excelFile = stream.ToArray();
        //                file = Convert.ToBase64String(stream.ToArray());
        //            }

        //            Spire.Xls.Workbook wbFromStream = new();

        //            wbFromStream.LoadFromStream(new MemoryStream(excelFile));
        //            MemoryStream pdfStream = new();
        //            wbFromStream.SaveToStream(pdfStream, Spire.Xls.FileFormat.PDF);

        //            pdfFile = pdfStream.ToArray();
        //            viewModel.Base64PDF = Convert.ToBase64String(pdfFile);

        //        }
        //        catch (Exception e)
        //        {

        //        }

        //        var resultResponse = new { status = 1, description = "", nameFile = "", file = viewModel.Base64PDF };

        //        return Json(new
        //        {
        //            response = resultResponse
        //        });

        //    }
        //    catch (Exception)
        //    {
        //        return this.Json(new
        //        {

        //            response = new ApiResponse<EtdFileViewModel>
        //            {
        //                Code = -1,
        //                Description = "Error al tratar de obtener los tipos de pruebas",
        //                Structure = null
        //            }
        //        });
        //    }
        //}

        [HttpGet]
        public IActionResult GetTestDataExcel(List<ETDTestsDTO> ETDTests)
        {
            try
            {
                return this.Json(new
                {
                    response = this.DataTest(ETDTests)
                });
            }
            catch (Exception)
            {
                return this.Json(new
                {

                    response = new ApiResponse<TestsDTO>
                    {
                        Code = -1,
                        Description = "Error al tratar de obtener los tipos de pruebas",
                        Structure = null
                    }
                });
            }
        }

        //Data de prueba, simulacion.
        private GraphicETD DataTest(List<ETDTestsDTO> ETDTests)
        {
            GraphicETD graphicETD = new() { Count = ETDTests.Count, Coords = new List<decimal[][]>(), MaxX = new List<decimal>(), MaxY = new List<decimal>(), MinX = new List<decimal>(), MinY = new List<decimal>() };
            foreach (ETDTestsDTO ETDTest in ETDTests.Where(x => x.Seccion != 1))
            {
                IEnumerable<Coord2> coordenadas = ETDTest.GraphicETDTests.Select(g => new Coord2 { x = g.ValorX, y = g.ValorY });
                graphicETD.MaxY.Add(coordenadas.Max(x => x.y) + 0.2M);
                graphicETD.MinY.Add(coordenadas.Min(x => x.y) - 0.2M);
                graphicETD.MaxX.Add(coordenadas.Max(x => x.x));
                graphicETD.MinX.Add(coordenadas.Min(x => x.x));
                graphicETD.Coords.Add(coordenadas.Where(x => x.y != 0).Select(x => new decimal[] { x.x, x.y }).ToArray());
            }

            return graphicETD;
        }

        public async Task<IActionResult> LoadFile([FromForm] EtdFileViewModel viewModel)
        {
            viewModel.NoSerie = viewModel.NoSerie.ToUpper().Trim();

            byte[] Array = null;
            if (viewModel.File != null)
            {
                using MemoryStream memoryStream = new();
                await viewModel.File.CopyToAsync(memoryStream);
                Array = memoryStream.ToArray();

            }
            // the XLSX format provider is used for demo purposes and can be replaced with any format provider implementing the IWorkbookFormatProvider interface
            //XlsxFormatProvider formatProvider = new XlsxFormatProvider();

            //Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();

            //using (Stream input = new MemoryStream(Array))
            ////using (Stream input = new FileStream(path, FileMode.Open))
            //{
            //    workbook = formatProvider.Import(input);
            //}

            Stream stream = new MemoryStream(Array);

            Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.Load(stream, ".xlsx");

            ApiResponse<SettingsToDisplayETDReportsDTO> result = await this._gatewayClientService.GetDownloadTemplateETD(nroSerie: viewModel.NoSerie, "", lenguage: viewModel.ClaveIdioma2);

            if (result.Code.Equals(-1))
            {
                return this.Json(new
                {
                    response = new ApiResponse<string>
                    {
                        Code = -1,
                        Description = "Error",
                        Structure = result.Description
                    }
                });

            }

            List<bool> listSheets = new()
            {
                viewModel.Check1,
                viewModel.Check2,
                viewModel.Check3
            };

            ETDUploadResultDTO resultload = this._etdService.PrepareUploadConfiguration_ETD(result.Structure, listSheets, workbook, viewModel.ClaveIdioma2);
            List<GraphicETD> graficos = new List<GraphicETD>();
            if (resultload != null)
            {
                foreach (ETDReportDTO ETDReport in resultload.ETDReports)
                {
                    graficos.Add(this.DataTest(ETDReport.ETDTestsGeneral.ETDTests));
                }

           

                return this.Json(new
                {
                    response = new ApiResponse<List<ErrorColumnsDTO>>
                    {
                        Code = -1,
                        Description = "Data",
                        Structure = resultload.Errors
                    }
                });
            }

            //ApiResponse<SettingsToDisplayETDReportsDTO> result = await this._gatewayClientService.(noSerie, clavePrueba, claveIdioma);

            //if (result.Code.Equals(-1))
            //{
            //    return this.View("Excel", new EtdViewModel
            //    {
            //        Error = result.Description
            //    });
            //}

            //_ = result.Structure;

            //return this.View("Excel", new EtdViewModel
            //{
            //    Error = result.Description
            //});
            return this.Json(new
            {
                response = new ApiResponse<EtdFileViewModel>
                {
                    Code = -1,
                    Description = "No. Serie inválido.",
                    Structure = null
                }
            });
        }

        public async Task<IActionResult> GetFilter(string noSerie)
        {
            try
            {
                EtdFileViewModel etdFileViewModel = new();
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
                        response = new ApiResponse<EtdFileViewModel>
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
                        response = new ApiResponse<EtdFileViewModel>
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
                            response = new ApiResponse<EtdFileViewModel>
                            {
                                Code = -1,
                                Description = "El número de serie no existe o no tiene registrada su información general proveniente de diseño",
                                Structure = null
                            }
                        });
                    }

                    if (artifactDesing.CharacteristicsArtifact.Count == 0)
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<EtdFileViewModel>
                            {
                                Code = -1,
                                Description = "El número de serie no existe o no tiene registrada su información de características proveniente de diseño",
                                Structure = null
                            }
                        });
                    }

                    if (artifactDesing.TapBaan.OrderCode == null)
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<EtdFileViewModel>
                            {
                                Code = -1,
                                Description = "El número de serie no existe o no tiene registrada su información de cambiadores proveniente de diseño",
                                Structure = null
                            }
                        });
                    }

                    List<PlateTensionDTO> tensions = (await this._artifactClientService.GetPlateTension(noSerie, "-1")).Structure;

                    if (tensions.Count is 0)
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<EtdFileViewModel>
                            {
                                Code = -1,
                                Description = "El número de serie no existe o no tiene registrada su información de tensión de la placa",
                                Structure = null
                            }
                        });
                    }

                    ApiResponse<PositionsDTO> resultP = await this._gatewayClientService.GetPositions(noSerie);

                    if (resultP.Code == -1)
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<EtdFileViewModel>
                            {
                                Code = -1,
                                Description = resultP.Description,
                                Structure = null
                            }
                        });
                    }

                    etdFileViewModel.PositionsDTO = resultP.Structure;
                    etdFileViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
                    etdFileViewModel.Altitud1 = artifactDesing.GeneralArtifact.AltitudeF1;
                    etdFileViewModel.Altitud2 = artifactDesing.GeneralArtifact.AltitudeF2;
                    etdFileViewModel.Cliente = artifactDesing.GeneralArtifact.CustomerName;

                    List<decimal?> listaCap = new();

                    foreach (CharacteristicsArtifactDTO item in artifactDesing.CharacteristicsArtifact)
                    {
                        if (item.Mvaf1 > 0)
                        {
                            listaCap.Add(item.Mvaf1);
                        }
                        if (item.Mvaf2 > 0)
                        {
                            listaCap.Add(item.Mvaf2);
                        }
                        if (item.Mvaf3 > 0)
                        {
                            listaCap.Add(item.Mvaf3);
                        }
                        if (item.Mvaf4 > 0)
                        {
                            listaCap.Add(item.Mvaf4);
                        }
                    }

                    etdFileViewModel.CapacidadReporte = string.Join('/', listaCap.Select(u => u.Value.ToString("F3"))) + " MVA";

                    etdFileViewModel.CapacidadesList = listaCap.Distinct().ToList();
                    etdFileViewModel.CapacidadesList.Add(null);
                    etdFileViewModel.EnfriamientosList = artifactDesing.CharacteristicsArtifact.Select(x => x.CoolingType).ToList();

                }

                return this.Json(new
                {
                    response = new ApiResponse<EtdFileViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = etdFileViewModel
                    }
                });
            }
            catch (Exception e)
            {

                return this.Json(new
                {
                    response = new ApiResponse<FpbViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }
        }

        private async Task PrepareForm()
        {
            _ = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Selected = true,
                    Text = "Seleccione...",
                    Value = string.Empty
                }
            };
            _ = new CultureInfo("en-US", false).TextInfo;
            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            this.ViewBag.Select1 = new SelectList(new List<GeneralPropertiesDTO>()
            {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } ,

            }.AsEnumerable(), "Clave", "Descripcion");
            this.ViewBag.Select2 = new SelectList(new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." },

            }.AsEnumerable(), "Clave", "Descripcion");
            this.ViewBag.Select3 = new SelectList(new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable(), "Clave", "Descripcion");
            List<SelectListItem> testList = new()
            {
                new SelectListItem()
                {
                    Selected = true,
                    Text = "Seleccione...",
                    Value = string.Empty
                }
            };

            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            this.ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");
            this.ViewBag.TestConnectionItems = testList;
            this.ViewBag.Enfriamiento = new SelectList(origingeneralProperties, "Clave", "Descripcion");
            this.ViewBag.SelectCapacidades = new SelectList(origingeneralProperties, "Clave", "Descripcion");

        }
    }
}
