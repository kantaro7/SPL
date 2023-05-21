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
    public class RdtController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IRdtClientService _rdtClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IRdtService _rdtService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProfileSecurityService _profileClientService;
        public RdtController(
            IMasterHttpClientService masterHttpClientService,
            IRdtClientService rdtClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IRdtService rdtService,
            IWebHostEnvironment hostEnvironment,
            IProfileSecurityService profileClientService)
        {
            this._masterHttpClientService = masterHttpClientService;
            this._rdtClientService = rdtClientService;
            this._reportClientService = reportClientService;
            this._artifactClientService = artifactClientService;
            this._testClientService = testClientService;
            this._gatewayClientService = gatewayClientService;
            this._rdtService = rdtService;
            this._hostEnvironment = hostEnvironment;
            this._profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.RelacióndeTransformación)))
                {

                    await this.PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return this.View(new RdtViewModel { NoSerie = noSerie });
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
            RdtViewModel rdtViewModel = new()
            {
                ATs = new List<PlateTensionDTO>(),
                BTs = new List<PlateTensionDTO>(),
                Ters = new List<PlateTensionDTO>()
            };
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
                    response = new ApiResponse<RdtViewModel>
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
                    response = new ApiResponse<RdtViewModel>
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
                        response = new ApiResponse<RdtViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no encontrado",
                            Structure = null
                        }
                    });
                }

                rdtViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
                rdtViewModel.AngularDisplacement = artifactDesing.GeneralArtifact.DesplazamientoAngular;
                rdtViewModel.Norm = artifactDesing.GeneralArtifact.Norma;

                List<PlateTensionDTO> tensions = (await this._artifactClientService.GetPlateTension(noSerie, "-1")).Structure;

                if (tensions.Count == 0)
                {

                    tensions = (await this._artifactClientService.GetPlateTension(noSerieSimple, "-1")).Structure;

                    if (tensions.Count == 0)
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<RdtViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee tensiones",
                                Structure = null
                            }
                        });
                    }
                }

                rdtViewModel.ATs = tensions.Where(ten => ten.TipoTension.Equals("AT"));
                rdtViewModel.BTs = tensions.Where(ten => ten.TipoTension.Equals("BT"));
                rdtViewModel.Ters = tensions.Where(ten => ten.TipoTension.ToUpper().Equals("TER"));

                rdtViewModel.ATCount = rdtViewModel.ATs.Count();
                rdtViewModel.BTCount = rdtViewModel.BTs.Count();
                rdtViewModel.TerCount = rdtViewModel.Ters.Count();

                List<PlateTensionDTO> baseT = new() { new PlateTensionDTO() { Orden = -1, Posicion = "Todas" } };
                rdtViewModel.ATs = rdtViewModel.ATCount >= 1 ? baseT.Concat(rdtViewModel.ATs) : rdtViewModel.ATs;
                rdtViewModel.BTs = rdtViewModel.BTCount >= 1 ? baseT.Concat(rdtViewModel.BTs) : rdtViewModel.BTs;
                rdtViewModel.Ters = rdtViewModel.TerCount >= 1 ? baseT.Concat(rdtViewModel.Ters) : rdtViewModel.Ters;

                if (rdtViewModel.ATCount == 0 || rdtViewModel.BTCount == 0 || rdtViewModel.TerCount == 0)
                {
                    ApiResponse<IEnumerable<TestsDTO>> reportResult = await this._testClientService.GetTest(ReportType.RDT.ToString(), "-1");
                    IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
                    {
                        Clave = item.ClavePrueba,
                        Descripcion = item.Descripcion
                    });
                    rdtViewModel.Tests = (rdtViewModel.ATCount == 0 && rdtViewModel.BTCount == 0) || (rdtViewModel.BTCount == 0 && rdtViewModel.TerCount == 0) || (rdtViewModel.ATCount == 0 && rdtViewModel.TerCount == 0)
                        ? new List<GeneralPropertiesDTO>()
                        : rdtViewModel.ATCount == 0
                        ? reportsGP.Where(x => x.Clave == "BTT")
                        : rdtViewModel.BTCount == 0 ? reportsGP.Where(x => x.Clave == "ATT") : reportsGP.Where(x => x.Clave == "ABT");
                }
            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await this._rdtClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.RDT.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.RDT.ToString() };
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

                rdtViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Relación de Tranformación",
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
                response = new ApiResponse<RdtViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = rdtViewModel
                }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, string angular, string norm, int conexion, string posAT, string posBT, string posTer, string comment)
        {
            try
            {
                ApiResponse<SettingsToDisplayRDTReportsDTO> result = await this._gatewayClientService.GetTemplate(noSerie, clavePrueba, claveIdioma, angular, norm, conexion, posAT, posBT, posTer);

                if (result.Code.Equals(-1))
                {
                    return this.View("Excel", new RdtViewModel
                    {
                        Error = result.Description
                    });
                }

                SettingsToDisplayRDTReportsDTO reportInfo = result.Structure;

                #region Decode Template
                if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
                {
                    return this.View("Excel", new RdtViewModel
                    {
                        Error = "No existe plantilla para el filtro seleccionado"
                    });
                }

                long NoPrueba = reportInfo.NextTestNumber;

                byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
                Stream stream = new MemoryStream(bytes);

                Workbook workbook = Workbook.Load(stream, ".xlsx");

                IEnumerable<GeneralPropertiesDTO> displacement = await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetEquivalentsAngularDisplacement, (int)MicroservicesEnum.splmasters);

                this._rdtService.PrepareTemplate_RDT(reportInfo, ref workbook, displacement.FirstOrDefault(c => c.Clave.Equals(angular)).Descripcion, clavePrueba, posAT, posBT, posTer, conexion);

                string valueAcceptance = reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptacion"))?.Formato;
                #endregion

                return this.View("Excel", new RdtViewModel
                {
                    ClaveIdioma = claveIdioma,
                    ClavePrueba = clavePrueba,
                    NoPrueba = NoPrueba,
                    NoSerie = noSerie,
                    AngularDisplacement = angular,
                    AngularDisplacementValue = angular,
                    Norm = norm,
                    Connection = conexion.ToString(),
                    ATPosition = posAT,
                    BTPosition = posBT,
                    TerPosition = posTer,
                    RDTReportsDTO = result.Structure,
                    Workbook = workbook,
                    Error = string.Empty,
                    Comments = comment
                });
            }
            catch (Exception e)
            {
                return this.View("Excel", new RdtViewModel
                {
                    Error = e.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] RdtViewModel ViewModel)
        {

            RDTTestsDTO rdtTestDTO = new()
            {
                AcceptanceValue = ViewModel.AcceptanceValue
            };

            List<PlateTensionDTO> plateTensions = (await this._artifactClientService.GetPlateTension(ViewModel.NoSerie, "-1")).Structure;

            GeneralPropertiesDTO generalArtifactAngular = (await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetEquivalentsAngularDisplacement, (int)MicroservicesEnum.splmasters)).FirstOrDefault(ang => ang.Clave == ViewModel.AngularDisplacement);

            this._rdtService.Prepare_RDT_Test(ViewModel.ClavePrueba, ViewModel.ATPosition, ViewModel.BTPosition, ViewModel.TerPosition, generalArtifactAngular, ViewModel.RDTReportsDTO, ViewModel.Workbook, plateTensions, ref rdtTestDTO);

            int rows = rdtTestDTO.Positions.Count;

            if (rdtTestDTO.Columns[0].Values.Count != rows || rdtTestDTO.Columns[1].Values.Count != rows || rdtTestDTO.Columns[2].Values.Count != rows)
            {
                return this.Json(new
                {
                    response = new ApiResponse<RdtViewModel>
                    {
                        Code = -2,
                        Description = "Faltan datos por ingresar en la tabla"
                    }
                });
            }

            ApiResponse<ResultRDTTestsDetailsDTO> resultTestRDT_Response = await this._rdtClientService.CalculateTestRDT(rdtTestDTO);

            if (resultTestRDT_Response.Code.Equals(-1))
            {
                return this.Json(new
                {
                    response = new ApiResponse<RdtViewModel>
                    {
                        Code = -1,
                        Description = string.Empty,
                        Structure = ViewModel
                    }
                });
            }

            Workbook workbook = ViewModel.Workbook;

            this._rdtService.PrepareIndexOfRDT(resultTestRDT_Response.Structure, ViewModel.RDTReportsDTO, ViewModel.ClaveIdioma, ref workbook);

            #region fillColumns
            List<decimal> col1 = rdtTestDTO.Columns[0].Values.ToList();
            List<decimal> col2 = rdtTestDTO.Columns[1].Values.ToList();
            List<decimal> col3 = rdtTestDTO.Columns[2].Values.ToList();

            rdtTestDTO.Columns.Clear();
            rdtTestDTO.Columns.Add(new ColumnDTO() { Name = "Valor Nominal", Values = resultTestRDT_Response.Structure.NominalValue });
            rdtTestDTO.Columns.Add(new ColumnDTO() { Name = "Fase A", Values = col1 });
            rdtTestDTO.Columns.Add(new ColumnDTO() { Name = "Desv Fase A", Values = resultTestRDT_Response.Structure.DeviationPhasesA });
            rdtTestDTO.Columns.Add(new ColumnDTO() { Name = "Fase B", Values = col2 });
            rdtTestDTO.Columns.Add(new ColumnDTO() { Name = "Desv Fase B", Values = resultTestRDT_Response.Structure.DeviationPhasesB });
            rdtTestDTO.Columns.Add(new ColumnDTO() { Name = "Fase C", Values = col3 });
            rdtTestDTO.Columns.Add(new ColumnDTO() { Name = "Desv Fase C", Values = resultTestRDT_Response.Structure.DeviationPhasesC });
            rdtTestDTO.Columns.Add(new ColumnDTO() { Name = "Alta Tension", Values = resultTestRDT_Response.Structure.HVVolts });
            rdtTestDTO.Columns.Add(new ColumnDTO() { Name = "Baja Tension", Values = resultTestRDT_Response.Structure.LVVolts });
            #endregion

            ViewModel.RdtTestDTO = rdtTestDTO;
            ViewModel.Workbook = workbook;
            ViewModel.OfficialWorkbook = workbook;
            bool resultReport = !resultTestRDT_Response.Structure.MessageErrors.Any();
            ViewModel.IsReportAproved = resultReport;

            string errors = string.Empty;
            List<string> errorMessages = resultTestRDT_Response.Structure.MessageErrors.Select(k => k.Message).ToList();
            string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
            return this.Json(new
            {
                response = new ApiResponse<RdtViewModel>
                {
                    Code = 1,
                    Description = allError,
                    Structure = ViewModel
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] RdtViewModel viewModel)
        {
            #region Export Excel
            if (!_rdtService.Verify_RDT_Columns(viewModel.RDTReportsDTO, viewModel.Workbook))
            {
                return this.Json(new
                {
                    response = new { status = -2, description = "Faltan datos por ingresar en la tabla" }
                });
            }

            Workbook workbook = viewModel.OfficialWorkbook;
            this._rdtService.CloneWorkbook(viewModel.Workbook, viewModel.RDTReportsDTO, ref workbook, out DateTime reportDate);

            viewModel.OfficialWorkbook = workbook;

            Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.OfficialWorkbook.ToDocument();

            PdfFormatProvider provider = new()
            {
                ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
            };
            document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.OfficialWorkbook.ActiveSheet);
            FloatingImage image = null;

            int rowCount = 0;
            foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
            {
                rowCount = sheet.UsedCellRange.RowCount;
                for (int j = 15; j < viewModel.RdtTestDTO.Positions.Count + 15; j++)
                {
                    string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(j, 2)].GetValue().Value.RawValue;
                    if (!string.IsNullOrEmpty(val))
                    {
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(j, 2)].SetValueAsText(val);
                    }
                }

                double officialSize = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(15, 3)].GetFontSize().Value;
                Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment allign = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(15, 2)].GetHorizontalAlignment().Value;

                for (int i = 3; i < 10; i++)
                {
                    for (int j = 15; j < viewModel.RdtTestDTO.Positions.Count + 15; j++)
                    {
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(j, i)].SetFontSize(officialSize);
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(j, i)].SetHorizontalAlignment(allign);
                        string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(j, i)].GetValue().Value.RawValue;
                        decimal value = decimal.Parse(val, CultureInfo.InvariantCulture);
                        if(i is 5 or 7 or 9)
                        {
                            value = Math.Round(value, 2);
                        }
                        else
                        {
                            value = Math.Round(value, 4);
                        }
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(j, i)].SetValueAsText(this.FormatStringDecimal(value+"", i is 5 or 7 or 9 ? 2 : 4));
                    }
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
            viewModel.RdtTestDTO.Modificadopor = null;
            viewModel.RdtTestDTO.Fechamodificacion = null;
            viewModel.RdtTestDTO.Creadopor = User.Identity.Name;
            viewModel.RdtTestDTO.Fechacreacion = DateTime.Now;
            #endregion

            #region Save Report
            RDTReportDTO rdtReportDTO = new()
            {
                Capacity = viewModel.RDTReportsDTO.HeadboardReport.Capacity,
                Comment = viewModel.Comments,
                Customer = viewModel.RDTReportsDTO.HeadboardReport.Client,
                LoadDate = reportDate,
                TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                LanguageKey = viewModel.ClaveIdioma,
                SerialNumber = viewModel.NoSerie,
                Result = viewModel.IsReportAproved,
                TypeReport = ReportType.RDT.ToString(),
                NameFile = string.Concat("RDT", viewModel.ClaveIdioma, viewModel.ClavePrueba, viewModel.NoSerie, viewModel.Norm, "-" + rowCount, ".pdf"),
                File = viewModel.Base64PDF,
                KeyTest = viewModel.ClavePrueba,
                AngularDisplacement = viewModel.AngularDisplacement,
                Connection_sp = Convert.ToDecimal(viewModel.Connection),
                DataTests = viewModel.RdtTestDTO,
                Pos_at = viewModel.ATPosition,
                PostTestBt = viewModel.BTPosition,
                Ter = viewModel.TerPosition,
                Rule = viewModel.Norm,
                Date = reportDate,

            };
            #endregion

            try
            {
                ApiResponse<long> result = await this._rdtClientService.SaveReport(rdtReportDTO);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = rdtReportDTO.NameFile, file = rdtReportDTO.File };

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
        public IActionResult Error() => this.View();

        #region PrivateMethods
        public async Task PrepareForm()
        {
            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            this.ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");

            generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetEquivalentsAngularDisplacement, (int)MicroservicesEnum.splmasters));
            this.ViewBag.DesplazamientoAngularItems = new SelectList(generalProperties, "Clave", "Descripcion");

            this.ViewBag.Connections = new SelectList(new List<GeneralPropertiesDTO>()
            {
                new GeneralPropertiesDTO() {
                    Clave = "-1", Descripcion="Seleccione..."
                },
                new GeneralPropertiesDTO() {
                    Clave = "0", Descripcion="Serie/Max"
                }, new GeneralPropertiesDTO() {
                    Clave = "1", Descripcion = "Para/Min"
                }
            }, "Clave", "Descripcion");

            generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetRulesEquivalents, (int)MicroservicesEnum.splmasters));
            this.ViewBag.Norms = new SelectList(generalProperties, "Clave", "Descripcion");

            ApiResponse<IEnumerable<TestsDTO>> reportResult = await this._testClientService.GetTest(ReportType.RDT.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            this.ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");
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

