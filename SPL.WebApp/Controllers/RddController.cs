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
    using SPL.WebApp.Helpers;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using Microsoft.Identity.Web;

    /// <summary>
    /// Relación de Transformación
    /// </summary>
    public class RddController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IRddClientService _rddClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IRddService _rddService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IProfileSecurityService _profileClientService;
        public RddController(
            IMasterHttpClientService masterHttpClientService,
            IRddClientService rddClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IRddService rddService,
            IWebHostEnvironment hostEnvironment,
            ISidcoClientService sidcoClientService,
            IProfileSecurityService profileClientService
            )
        {
            this._masterHttpClientService = masterHttpClientService;
            this._rddClientService = rddClientService;
            this._reportClientService = reportClientService;
            this._artifactClientService = artifactClientService;
            this._testClientService = testClientService;
            this._gatewayClientService = gatewayClientService;
            this._rddService = rddService;
            this._hostEnvironment = hostEnvironment;
            this._sidcoClientService = sidcoClientService;
            this._profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(User.Identity.Name).Result;


                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.ReactanciadeFuga)))
                {

                    await this.PrepareForm();
                    var noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return this.View(new RddViewModel { NoSerie = noSerie });
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
            RddViewModel rddViewModel = new();
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
                InformationArtifactDTO artifactDesing = await this._artifactClientService.GetArtifact(noSerieSimple);
                if (artifactDesing.GeneralArtifact.OrderCode == null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<RddViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no encontrado",
                            Structure = null
                        }
                    });
                }

                // CAR
                if (artifactDesing.VoltageKV is null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<RddViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee voltages",
                            Structure = null
                        }
                    });
                }
                if (artifactDesing.Derivations is null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<RddViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee conexiones equivalentes",
                            Structure = null
                        }
                    });
                }

                if (artifactDesing.GeneralArtifact.TypeTrafoId is null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<RodViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee información de TypeTrafoId",
                            Structure = null
                        }
                    });
                }

                IEnumerable<CatSidcoDTO> sidcos = await this._sidcoClientService.GetCatSIDCO();

                if (!sidcos.Any())
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<RodViewModel>
                        {
                            Code = -1,
                            Description = "La Base de Datos no posee información de CatSIDCO",
                            Structure = null
                        }
                    });
                }

                CatSidcoDTO catSidco = sidcos.Where(s => s.AttributeId == 3 && s.Id == Convert.ToInt32(artifactDesing.GeneralArtifact.TypeTrafoId)).FirstOrDefault();

                rddViewModel.WindingConfiguration = catSidco is null
                    ? ""
                    : catSidco.ClaveSPL.Equals("AUT")
                    ? "Autotransformador"
                    : $"{(artifactDesing.Derivations.ConexionEquivalente.ToUpper() is "WYE" or "ESTRELLA" ? "Estrella" : "Delta")} - {(artifactDesing.Derivations.ConexionEquivalente_2.ToUpper() is "WYE" or "ESTRELLA" ? "Estrella" : "Delta")}";
                rddViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
            }

            // ARBOL

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await this._rddClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.RDD.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.RDD.ToString() };
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
                                text = item.NombreArchivo.Split('.')[0]+"_"+ item.IdCarga.ToString()+".pdf",
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

                rddViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Reactancia de Fuga",
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
                response = new ApiResponse<RddViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = rddViewModel
                }
            });
        }
        

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, string windingConfiguration, string connection, decimal zPorc, decimal jXPorc, string comment)
        {
            ApiResponse<SettingsToDisplayRDDReportsDTO> result = await this._gatewayClientService.GetTemplateRDD(noSerie, clavePrueba, claveIdioma, windingConfiguration, connection, zPorc, jXPorc);

            if (result.Code.Equals(-1))
            {
                return this.View("Excel", new RddViewModel
                {
                    Error = result.Description
                });
            }
            ApiResponse<PositionsDTO> positions = await this._gatewayClientService.GetPositions(noSerie);
            if (positions.Code.Equals(-1))
            {
                return this.View("Excel", new RddViewModel
                {
                    Error = positions.Description
                });
            }

            if(positions.Structure.AltaTension.Count is 0)
            {
                return this.View("Excel", new RddViewModel
                {
                    Error = "Artefacto no posee posiciones en Alta"
                });
            }

            if (positions.Structure.BajaTension.Count is 0)
            {
                return this.View("Excel", new RddViewModel
                {
                    Error = "Artefacto no posee posiciones en Baja"
                });
            }

            SettingsToDisplayRDDReportsDTO reportInfo = result.Structure;

            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return this.View("Excel", new RddViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = reportInfo.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");

            this._rddService.PrepareTemplate_RDD(reportInfo, ref workbook, claveIdioma, positions.Structure);

            int valorAcep = Convert.ToInt32(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptacion")).Formato);

            return this.View("Excel", new RddViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = clavePrueba,
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                Connection = connection,
                WindingConfiguration = windingConfiguration,
                ZPorc = zPorc,
                JXPorc = jXPorc,
                RDDReportsDTO = result.Structure,
                Workbook = workbook,
                Error = string.Empty,
                Comments = comment,
                ValorAcep = valorAcep
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] RddViewModel viewModel)
        {

            RDDTestsGeneralDTO rddTestsGeneralDTO = new()
            {
                ConfigWinding = viewModel.WindingConfiguration,
                OutRDDTests = new()
                {
                    new OutRDDTestsDTO()
                    {
                        RDDTestsDetails = new List<RDDTestsDetailsDTO>()
                    },
                    new OutRDDTestsDTO()
                    {
                        RDDTestsDetails = new List<RDDTestsDetailsDTO>()
                    }
                },
                PorcJx = viewModel.JXPorc,
                PorcZ = viewModel.ZPorc,
                ValueAcep = viewModel.ValorAcep
            };
            string resulta = this._rddService.Verify_RDD_Columns(viewModel.RDDReportsDTO, viewModel.Workbook);
            if (resulta != string.Empty)
            {
                return this.Json(new
                {
                    response = new ApiResponse<RddViewModel>
                    {
                        Code = -1,
                        Description = "Faltan datos por ingresar en la tabla. "+resulta,
                        Structure = viewModel
                    }
                });
            }

            this._rddService.Prepare_RDD_Test(viewModel.RDDReportsDTO, viewModel.Workbook, ref rddTestsGeneralDTO);

            ApiResponse<ResultRDDTestsDTO> resultTestRDD_Response = await this._rddClientService.CalculateTestRDD(rddTestsGeneralDTO);

            if (resultTestRDD_Response.Code.Equals(-1))
            {
                return this.Json(new
                {
                    response = new ApiResponse<RddViewModel>
                    {
                        Code = -1,
                        Description = resultTestRDD_Response.Description,
                        Structure = viewModel
                    }
                });
            }

            Workbook workbook = viewModel.Workbook;

            this._rddService.PrepareIndexOfRDD(resultTestRDD_Response.Structure, viewModel.RDDReportsDTO, viewModel.ClaveIdioma, ref workbook);

            #region fillColumns
            rddTestsGeneralDTO = resultTestRDD_Response.Structure.RDDTestsGeneral;
            #endregion
             
            viewModel.RDDTestsGeneralDTO = rddTestsGeneralDTO;
            viewModel.Workbook = workbook;
            bool resultReport = !resultTestRDD_Response.Structure.Results.Any();
            viewModel.IsReportAproved = resultReport;

            string errors = string.Empty;
            List<string> errorMessages = resultTestRDD_Response.Structure.Results.Select(k => k.Message).ToList();
            string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
            return this.Json(new
            {
                response = new ApiResponse<RddViewModel>
                {
                    Code = 1,
                    Description = allError,
                    Structure = viewModel
                }
            });
        }

        [HttpPost]
        
        public async Task<IActionResult> SavePDF([FromBody] RddViewModel viewModel)
        {
            #region Export Excel
            string resulta = this._rddService.Verify_RDD_Columns(viewModel.RDDReportsDTO, viewModel.Workbook);
            if (resulta != string.Empty)
            {
                return this.Json(new
                {
                    response = new { status = -1, description = "Faltan datos por ingresar en la tabla. "+resulta }
                });
            }

            viewModel.RDDTestsGeneralDTO.DateTest = this._rddService.GetDate(viewModel.RDDReportsDTO, viewModel.Workbook);

            Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

            PdfFormatProvider provider = new()
            {
                ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
            };
            document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.Workbook.ActiveSheet);
            FloatingImage image = null;
            int rowCount = 0;  foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
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
            viewModel.RDDTestsGeneralDTO.CapacityReport = viewModel.RDDReportsDTO.HeadboardReport.Capacity;
            viewModel.RDDTestsGeneralDTO.Comment = viewModel.Comments;
            viewModel.RDDTestsGeneralDTO.Creadopor = User.Identity.Name;
            viewModel.RDDTestsGeneralDTO.Customer = viewModel.RDDReportsDTO.HeadboardReport.Client;
            viewModel.RDDTestsGeneralDTO.Fechacreacion = DateTime.Now;
            viewModel.RDDTestsGeneralDTO.File = pdfFile;
            viewModel.RDDTestsGeneralDTO.IdLoad = 0;
            viewModel.RDDTestsGeneralDTO.KeyTest = viewModel.ClavePrueba;
            viewModel.RDDTestsGeneralDTO.LanguageKey = viewModel.ClaveIdioma;
            viewModel.RDDTestsGeneralDTO.LoadDate = DateTime.Now;
            viewModel.RDDTestsGeneralDTO.Modificadopor = null;
            viewModel.RDDTestsGeneralDTO.NameFile = string.Concat("RDD", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf");
            viewModel.RDDTestsGeneralDTO.Result = viewModel.IsReportAproved;
            viewModel.RDDTestsGeneralDTO.SerialNumber = viewModel.NoSerie;
            viewModel.RDDTestsGeneralDTO.TestNumber = Convert.ToInt32(viewModel.NoPrueba);
            viewModel.RDDTestsGeneralDTO.TypeReport = ReportType.RDD.ToString();
            viewModel.RDDTestsGeneralDTO.PorcJx = viewModel.JXPorc;
            viewModel.RDDTestsGeneralDTO.PorcZ = viewModel.ZPorc;
            viewModel.RDDTestsGeneralDTO.Connection = viewModel.Connection;
            viewModel.RDDTestsGeneralDTO.ConfigWinding = viewModel.WindingConfiguration;
            #endregion

            try
            {
                ApiResponse<long> result = await this._rddClientService.SaveReport(viewModel.RDDTestsGeneralDTO);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = viewModel.RDDTestsGeneralDTO, file = viewModel.Base64PDF };

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
            try
            {
                IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

                // Tipos de prueba
                ApiResponse<IEnumerable<TestsDTO>> reportResult = await this._testClientService.GetTest(ReportType.RDD.ToString(), "-1");
                IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
                {
                    Clave = item.ClavePrueba,
                    Descripcion = item.Descripcion
                });

                this.ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");

                // Idiomas
                IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
                this.ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");

                this.ViewBag.WindingConfigurationItems = new SelectList(new List<GeneralPropertiesDTO>()
                {
                    new GeneralPropertiesDTO() {
                        Clave = "", Descripcion="Seleccione..."
                    },
                    new GeneralPropertiesDTO() {
                        Clave = "Delta - Estrella", Descripcion="Delta - Estrella"
                    }, new GeneralPropertiesDTO() {
                        Clave = "Estrella - Delta", Descripcion = "Estrella - Delta"
                    }, new GeneralPropertiesDTO() {
                        Clave = "Autotransformador", Descripcion = "Autotransformador"
                    }
                }, "Clave", "Descripcion");

                this.ViewBag.ConnectionItems = new SelectList(new List<GeneralPropertiesDTO>()
                {
                    new GeneralPropertiesDTO() {
                        Clave = "No Aplica", Descripcion="No Aplica"
                    },
                    new GeneralPropertiesDTO() {
                        Clave = "Serie", Descripcion="Serie"
                    }, new GeneralPropertiesDTO() {
                        Clave = "Paralelo", Descripcion = "Paralelo"
                    }
                }, "Clave", "Descripcion");
            }
            catch(Exception e)
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