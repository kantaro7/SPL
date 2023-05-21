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
    public class PimController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IPimClientService _pimClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IPimService _pimService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IResistanceTwentyDegreesClientServices _resistanceTwentyDegreesClientServices;
        private readonly IProfileSecurityService _profileClientService;
        public PimController(
            IMasterHttpClientService masterHttpClientService,
            IPimClientService pimClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IPimService pimService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            ISidcoClientService sidcoClientService,
            IResistanceTwentyDegreesClientServices resistanceTwentyDegreesClientServices,
            IProfileSecurityService profileClientService
            )
        {
            this._masterHttpClientService = masterHttpClientService;
            this._pimClientService = pimClientService;
            this._reportClientService = reportClientService;
            this._artifactClientService = artifactClientService;
            this._testClientService = testClientService;
            this._gatewayClientService = gatewayClientService;
            this._pimService = pimService;
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
                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.ImpulsoporManiobra)))
                {

                    await this.PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return this.View(new PimViewModel { NoSerie = noSerie });
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
            PimViewModel pimViewModel = new();
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
                    response = new ApiResponse<PimViewModel>
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
                    response = new ApiResponse<PimViewModel>
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
                        response = new ApiResponse<PimViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no encontrado",
                            Structure = null
                        }
                    });
                }
                // DG
                if (artifactDesing.GeneralArtifact is null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PimViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee información general",
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
                        response = new ApiResponse<PimViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee garantias",
                            Structure = null
                        }
                    });
                }

                pimViewModel.ConnectionsList = new();

                if(artifactDesing.VoltageKV.TensionKvAltaTension1 is not null and not 0 || artifactDesing.VoltageKV.TensionKvAltaTension3 is not null and not 0)
                {
                    pimViewModel.ConnectionsList.Add("Alta Tensión");

                    if (artifactDesing.VoltageKV.TensionKvAltaTension1 is not null and not 0 && artifactDesing.VoltageKV.TensionKvAltaTension3 is not null and not 0)
                    {
                        pimViewModel.ConnectionsList.Add($"Alta Tensión - {artifactDesing.VoltageKV.TensionKvAltaTension1}");
                        pimViewModel.ConnectionsList.Add($"Alta Tensión - {artifactDesing.VoltageKV.TensionKvAltaTension3}");
                    }
                }

                if (artifactDesing.VoltageKV.TensionKvBajaTension1 is not null and not 0 || artifactDesing.VoltageKV.TensionKvBajaTension3 is not null and not 0)
                {
                    pimViewModel.ConnectionsList.Add("Baja Tensión");
                    if (artifactDesing.VoltageKV.TensionKvBajaTension1 is not null and not 0 && artifactDesing.VoltageKV.TensionKvBajaTension3 is not null and not 0)
                    {
                        pimViewModel.ConnectionsList.Add($"Baja Tensión - {artifactDesing.VoltageKV.TensionKvBajaTension1}");
                        pimViewModel.ConnectionsList.Add($"Baja Tensión - {artifactDesing.VoltageKV.TensionKvBajaTension3}");
                    }
                }
                if(pimViewModel.ConnectionsList.Count > 1)
                {
                    pimViewModel.ConnectionsList.Insert(0, "Todas");
                }
                pimViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await this._pimClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.PIM.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.PIM.ToString() };
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

                pimViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Impulso por Maniobra",
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
                response = new ApiResponse<PimViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = pimViewModel
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

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, string connections, string applyLow, string comment)
        {

            List<string> cList = connections.Split(",").ToList();
            noSerie = noSerie.ToUpper().Trim();

            bool haveAt = cList.Exists(x => x.Contains("Alta"));
            bool haveBt = cList.Exists(x => x.Contains("Baja"));

            string connectionAt = haveAt ? string.Join(',',cList.FindAll(x => x.Contains("Alta"))) ?? "" : "";
            string connectionBt = haveBt ? string.Join(',', cList.FindAll(x => x.Contains("Baja"))) ?? "" : "";

            ApiResponse<SettingsToDisplayPIMReportsDTO> result = await this._gatewayClientService.GetTemplatePIM(noSerie, clavePrueba, claveIdioma, connectionAt, connectionBt, applyLow);

            if (result.Code.Equals(-1))
            {
                return this.View("Excel", new PimViewModel
                {
                    Error = result.Description
                });
            }

            if (result.Structure.Terminals.Count() is 0)
            {
                return this.View("Excel", new PimViewModel
                {
                    Error = "No hay terminales disponibles para el aparato seleccionado"
                });
            }

            SettingsToDisplayPIMReportsDTO reportInfo = result.Structure;

            #region Decode Template
            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return this.View("Excel", new PimViewModel
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
                if (haveAt)
                {
                    int posH0 = reportInfo.Terminals.IndexOf("H0");
                    if (posH0 > -1)
                    {
                        reportInfo.Terminals.RemoveAt(posH0);
                    }
                }
                if (haveBt)
                {
                    int posX0 = reportInfo.Terminals.IndexOf("X0");
                    if (posX0 > -1)
                    {
                        reportInfo.Terminals.RemoveAt(posX0);
                    }    
                }
      
             
                this._pimService.PrepareTemplate_PIM(reportInfo, ref workbook);
            }
            catch (Exception)
            {
                return this.View("Excel", new PimViewModel
                {
                    Error = "La plantilla esta mal configurada por favor verifiquela de inmediato"
                });
            }
            #endregion

            return this.View("Excel", new PimViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = clavePrueba,
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                ConnectionsSelected = cList,
                PIMReportsDTO = result.Structure,
                Workbook = workbook,
                Error = string.Empty,
                Comments = comment,
                Connection = haveAt && haveBt ? "Todas" : haveAt ? "Alta Tensión" : "Baja Tensión",
                Low = applyLow,
                VoltageLevel = "1000"
            });
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] PimViewModel viewModel)
        {
            #region Export Excel
            int[] filas = new int[2];

            if (!_pimService.Verify_PIM_Columns(viewModel.PIMReportsDTO, viewModel.Workbook))
            {
               

                return this.Json(new
                {
                    response = new { status = -1, description = "Faltan datos por ingresar en la tabla" }
                });
            }
            DateTime date = this._pimService.GetDate(viewModel.Workbook, viewModel.PIMReportsDTO);
            PIMTestsGeneralDTO pIMTestsGeneralDTO = new()
            {
                ApplyLow = viewModel.Low,
                Capacity = viewModel.PIMReportsDTO.HeadboardReport.Capacity,
                Comment = viewModel.Comments,
                Connection = viewModel.Connection,
                Creadopor = User.Identity.Name,
                Customer = viewModel.PIMReportsDTO.HeadboardReport.Client,
                Fechacreacion = DateTime.Now,
                IdLoad = 0,
                Data = new List<PIMTestsDetailsDTO>(),
                KeyTest = viewModel.ClavePrueba,
                LanguageKey = viewModel.ClaveIdioma,
                LoadDate = DateTime.Now,
                Modificadopor = null,
                Fechamodificacion = null,
                Result = true,
                SerialNumber = viewModel.NoSerie,
                TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                TypeReport = ReportType.PIM.ToString(),
                VoltageLevel = viewModel.VoltageLevel == "Todas" ? "Todas" : null,
                Voltage = viewModel.VoltageLevel == "Todas" ? 0 : decimal.Parse(viewModel.VoltageLevel),
            };

            _pimService.Prepare_PIM_Test(viewModel.PIMReportsDTO, viewModel.Workbook, ref pIMTestsGeneralDTO);

            pIMTestsGeneralDTO.Date = this._pimService.GetDate(viewModel.Workbook, viewModel.PIMReportsDTO);

            Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

            PdfFormatProvider provider = new()
            {
                ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
            };
            document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.Workbook.ActiveSheet);
            int rowCount = 0;  foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
            {
                rowCount = sheet.UsedCellRange.RowCount;
                FloatingImage image = new(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(0, 0), 0, 0);
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
                sheet.WorksheetPageSetup.Margins = new Telerik.Windows.Documents.Spreadsheet.Model.Printing.PageMargins(0, 20, 0, 20);
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

            List<FilesDTO> archivos = new();
            List<Stream> filesStreams = new() { pdfStream };
            foreach (ArchivosFrontViewModel item in viewModel.Files)
            {
                archivos.Add(new FilesDTO
                {
                    File = Convert.FromBase64String(item.Base64.Split(',')[1]),
                    Name = item.Name
                });

                filesStreams.Add(new MemoryStream(Convert.FromBase64String(item.Base64.Split(',')[1])));
            }

            PdfDocumentBase resultaqui = PdfDocument.MergeFiles(filesStreams.ToArray());
            resultaqui.Save(pdfStream, Spire.Pdf.FileFormat.PDF);
            pdfFile = pdfStream.ToArray();

            viewModel.Base64PDF = Convert.ToBase64String(pdfFile);

            #endregion

            #region Save Report
            pIMTestsGeneralDTO.File = pdfFile;
            pIMTestsGeneralDTO.Files = archivos;
            pIMTestsGeneralDTO.NameFile = string.Concat("PIM", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf");
            #endregion

            try
            {
                string a = JsonConvert.SerializeObject(pIMTestsGeneralDTO);
                ApiResponse<long> result = await this._pimClientService.SaveReport(pIMTestsGeneralDTO);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = pIMTestsGeneralDTO.NameFile, file = pIMTestsGeneralDTO.File };

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
        public IActionResult Error() => this.View();

        #region PrivateMethods
        public async Task PrepareForm()
        {
            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            // Tipos de prueba
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await this._testClientService.GetTest(ReportType.PIM.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            this.ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");

            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            this.ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");

            // Conexion
            ViewBag.ConnectionItems = new SelectList(new List<GeneralPropertiesDTO>()
            {
                new GeneralPropertiesDTO() { Clave = "AT", Descripcion = "Alta Tensión" } ,
                new GeneralPropertiesDTO() { Clave = "BT", Descripcion = "Baja Tensión" } ,

            }.AsEnumerable(), "Clave", "Descripcion");

            // Aplica Baja
            List<SelectListItem> low = new()
            {
                new SelectListItem()
                {
                    Selected = true,
                    Text = "No",
                    Value = "No"
                },
                new SelectListItem()
                {
                    Selected = false,
                    Text = "Si",
                    Value = "Si"
                }
            };

            this.ViewBag.LowItems = low;

            // Aplica Baja
            List<SelectListItem> voltageLevel = new()
            {
                new SelectListItem()
                {
                    Selected = true,
                    Text = "Seleccione...",
                    Value = string.Empty
                }
            };

            this.ViewBag.VoltageLevelItems = voltageLevel;
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
                        num += ".".PadRight(decimals, '0');
                    }
                }
            }
            return num;
        }
        #endregion
    }
}