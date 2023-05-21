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

    using Newtonsoft.Json;

    using Spire.Pdf;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ETD;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Helpers;
    using SPL.WebApp.ViewModels;

    using Telerik.Web.Spreadsheet;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export;
    using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

    /// <summary>
    /// Relación de Transformación
    /// </summary>
    public class EtdController : Controller
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

        public EtdController(
            IMasterHttpClientService masterHttpClientService,
            IEtdClientService etdClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IEtdService etdService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            ISidcoClientService sidcoClientService,
            IResistanceTwentyDegreesClientServices resistanceTwentyDegreesClientServices
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _etdClientService = etdClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _etdService = etdService;
            _correctionFactorService = correctionFactorService;
            _hostEnvironment = hostEnvironment;
            _sidcoClientService = sidcoClientService;
            _resistanceTwentyDegreesClientServices = resistanceTwentyDegreesClientServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index(decimal idCutting, string noSerie)
        {
            await PrepareForm();
            //string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
            return View(new EtdViewModel { NoSerie = noSerie, IdCuttingData = idCutting });
        }

        public async Task<IActionResult> GetFilter(string noSerie)
        {
            EtdViewModel etdViewModel = new();
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
                    response = new ApiResponse<EtdViewModel>
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
                    response = new ApiResponse<EtdViewModel>
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
                        response = new ApiResponse<EtdViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no encontrado",
                            Structure = null
                        }
                    });
                }

                if (artifactDesing.VoltageKV is null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<EtdViewModel>
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
                    return Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee características",
                            Structure = null
                        }
                    });
                }

                bool mvaf3 = artifactDesing.CharacteristicsArtifact.Any(c => c.Mvaf3.HasValue && c.Mvaf3.Value > 0);
                bool mvaf4 = artifactDesing.CharacteristicsArtifact.Any(c => c.Mvaf4.HasValue && c.Mvaf4.Value > 0);

                etdViewModel.TerB2 = mvaf4 ? "TER" : mvaf3 ? "BT2" : "NA";

                etdViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _etdClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.ETD.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.ETD.ToString() };
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

                etdViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Elevación de Temperatura de los Devanados",
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
                response = new ApiResponse<EtdViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = etdViewModel
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
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, DateTime date, bool typeRegression, string overload, bool lVDifferentCapacity, decimal capacityBt, string tertiary2B, bool terCapRed, decimal capacityTer, string windingSplit, decimal connection, decimal idCuttingData)
        {
            noSerie = noSerie.ToUpper().Trim();
            ApiResponse<SettingsToDisplayETDReportsDTO> result = await _gatewayClientService.GetTemplateETD(noSerie, clavePrueba, claveIdioma, typeRegression, overload, lVDifferentCapacity, capacityBt, tertiary2B, terCapRed, capacityTer, windingSplit, idCuttingData, connection);

            if (result.Code.Equals(-1))
            {
                return View("Excel", new EtdViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayETDReportsDTO reportInfo = result.Structure;

            #region Decode Template
            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return View("Excel", new EtdViewModel
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
                _etdService.PrepareTemplate_ETD(reportInfo, lVDifferentCapacity, capacityBt, terCapRed, capacityTer, claveIdioma, overload, clavePrueba, ref workbook);
            }
            catch (Exception)
            {
                return View("Excel", new EtdViewModel
                {
                    Error = "La plantilla esta mal configurada por favor verifiquela de inmediato"
                });
            }
            #endregion

            List<List<Coord>> coordenadas = clavePrueba.Equals("SAB") ? new List<List<Coord>>() { new(), new() } : new List<List<Coord>>() { new(), new(), new() };

            for (int i = 0; i < coordenadas.Count; i++)
            {
                coordenadas[i] = result.Structure.GraficaT[i].Zip(result.Structure.GraficaR[i], (T, R) => new Coord(T, R)).ToList();
            }

            return View("Excel", new EtdViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = clavePrueba,
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                ETDReportsDTO = result.Structure,
                Workbook = workbook,
                Error = string.Empty,
                Date = date,
                RegressionType = typeRegression,
                Overload = overload,
                LVDifferentCapacity = lVDifferentCapacity,
                TerReducedCapacity = terCapRed,
                Capacity1 = capacityBt,
                Capacity2 = capacityTer,
                TerB2 = tertiary2B,
                SplitWinding = windingSplit,
                Connection = connection,
                IdCuttingData = idCuttingData,
                Coords = coordenadas,
                Base64Graphics = new List<string>()
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] EtdViewModel viewModel)
        {

            ETDTestsGeneralDTO etdTestGeneralDTO = new()
            {
                IdCorte = viewModel.EtdTestDTO.IdCorte,
                IdReg = viewModel.EtdTestDTO.IdReg,
                BtDifCap = viewModel.LVDifferentCapacity,
                CapacidadBt = viewModel.Capacity1,
                CapacidadTer = viewModel.Capacity2,
                Comentario = viewModel.Comments,
                DevanadoSplit = viewModel.SplitWinding,
                TerCapRed = viewModel.TerReducedCapacity,
                Terciario2b = viewModel.TerB2,
                TipoRegresion = viewModel.RegressionType,
                FactorAltitud = viewModel.ETDReportsDTO.FactorAltitud,
                FactorKw = viewModel.ETDReportsDTO.FactorCorrecciónkW,
                Fechacreacion = DateTime.Now,
                UltimaHora = viewModel.ETDReportsDTO.UltimaHora,
                TorLim = viewModel.ETDReportsDTO.TorLimite,
                AwrLim = viewModel.ETDReportsDTO.AwrLim,
                HsrLim = viewModel.ETDReportsDTO.HsrLim,
                GradienteLim = viewModel.ETDReportsDTO.GradienteLim,
            };

            int sections = viewModel.ClavePrueba.Equals("SAB") ? 2 : 3;

            etdTestGeneralDTO.ETDTests = new List<ETDTestsDTO>() { new ETDTestsDTO(), new ETDTestsDTO(), new ETDTestsDTO() };

            if (sections is 3)
                etdTestGeneralDTO.ETDTests.Add(new ETDTestsDTO());

            ETDTestsDTO inicio = new()
            {
                AltitudeF1 = viewModel.ETDReportsDTO.Altitud1,
                AltitudeF2 = viewModel.ETDReportsDTO.Altitud2,
                Capacidad = viewModel.ETDReportsDTO.CapacidadPruebaAT,
                NivelTension = viewModel.ETDReportsDTO.NivelTension,
                Perdidas = viewModel.ETDReportsDTO.Perdidas,
                CoolingType = viewModel.ETDReportsDTO.Enfriamiento,
                PosAt = viewModel.ETDReportsDTO.PosAT,
                PosBt = viewModel.ETDReportsDTO.PosBT,
                ETDTestsDetails = new List<ETDTestsDetailsDTO>()
            };

            for (int i = 0; i < viewModel.ETDReportsDTO.Tiempo.Count; i++)
            {
                if (viewModel.ETDReportsDTO.PorcentajeRepresentanPerdidas > 3)
                {
                    inicio.ETDTestsDetails.Add(new ETDTestsDetailsDTO()
                    {
                        FechaHora = Convert.ToDateTime(viewModel.ETDReportsDTO.Tiempo[i]),
                        PromRadSup = viewModel.ETDReportsDTO.RadSuperior[i],
                        PromRadInf = viewModel.ETDReportsDTO.RadInferior[i],
                        AmbienteProm = viewModel.ETDReportsDTO.AmbProm[i],
                        TempTapa = viewModel.ETDReportsDTO.TempTapa[i],
                        Tor = viewModel.ETDReportsDTO.TOR[i],
                        Aor = viewModel.ETDReportsDTO.AOR[i],
                        Bor = viewModel.ETDReportsDTO.BOR[i],
                        ElevAceiteSup = viewModel.ETDReportsDTO.ElevAceiteSup[i],
                        ElevAceiteProm = viewModel.ETDReportsDTO.ElevAceiteProm[i],
                        ElevAceiteInf = viewModel.ETDReportsDTO.ElevAceiteInf[i],
                    });
                }
                else
                {
                    inicio.ETDTestsDetails.Add(new ETDTestsDetailsDTO()
                    {
                        Tiempo = Convert.ToDecimal(viewModel.ETDReportsDTO.Tiempo[i]),
                        PromRadSup = viewModel.ETDReportsDTO.RadSuperior[i],
                        PromRadInf = viewModel.ETDReportsDTO.RadInferior[i],
                        AmbienteProm = viewModel.ETDReportsDTO.AmbProm[i],
                        TempTapa = viewModel.ETDReportsDTO.TempTapa[i],
                        Tor = viewModel.ETDReportsDTO.TOR[i],
                        Aor = viewModel.ETDReportsDTO.AOR[i],
                        Bor = viewModel.ETDReportsDTO.BOR[i],
                    });
                }
            }

            etdTestGeneralDTO.ETDTests[0] = inicio;

            for (int i = 0; i < sections; i++)
            {
                ETDTestsDTO page = new()
                {
                    AltitudeF1 = viewModel.ETDReportsDTO.Altitud1,
                    AltitudeF2 = viewModel.ETDReportsDTO.Altitud2,
                    Capacidad = viewModel.ETDReportsDTO.CapacidadPruebaAT,
                    CoolingType = viewModel.ETDReportsDTO.Enfriamiento,
                    PosAt = viewModel.ETDReportsDTO.PosAT,
                    PosBt = viewModel.ETDReportsDTO.PosBT,
                    ResistCorte = viewModel.ETDReportsDTO.ResistCorte[i],
                    TempResistCorte = viewModel.ETDReportsDTO.TempResistCorte[i],
                    FactorK = viewModel.ETDReportsDTO.FactorK[i],
                    ResistTcero = viewModel.ETDReportsDTO.ResistTCero[i],
                    TempDev = viewModel.ETDReportsDTO.TempDev[i],
                    GradienteDev = viewModel.ETDReportsDTO.GradienteDev[i],
                    ElevPromDev = viewModel.ETDReportsDTO.ElevPromDev[i],
                    ElevPtoMasCal = viewModel.ETDReportsDTO.ElevPtoMasCal[i],
                    TempPromAceite = viewModel.ETDReportsDTO.TempPromAceite[i],
                    Terminal = viewModel.ETDReportsDTO.Terminal[i],
                    ETDTestsDetails = new List<ETDTestsDetailsDTO>(),
                    GraphicETDTests = new List<GraphicETDTestsDTO>(),
                };

                for (int j = 0; j < viewModel.ETDReportsDTO.TiempoResist[i].Count; j++)
                {
                    page.ETDTestsDetails.Add(new ETDTestsDetailsDTO()
                    {
                        Tiempo = viewModel.ETDReportsDTO.TiempoResist[i][j],
                        Resistencia = viewModel.ETDReportsDTO.Resistencia[i][j]
                    });
                }

                for (int j = 0; j < viewModel.ETDReportsDTO.GraficaT[i].Count; j++)
                {
                    page.GraphicETDTests.Add(new()
                    {
                        Renglon = j + 1,
                        Seccion = i + 1,
                        ValorX = viewModel.ETDReportsDTO.GraficaT[i][j],
                        ValorY = viewModel.ETDReportsDTO.GraficaR[i][j]
                    });
                }
                etdTestGeneralDTO.ETDTests[i + 1] = page;
            }

            // Prepare data
            ApiResponse<ResultETDTestsDTO> resultTestETD_Response = await _etdClientService.CalculateTestETD(etdTestGeneralDTO);

            if (resultTestETD_Response.Code.Equals(-1))
            {
                return Json(new
                {
                    response = new ApiResponse<EtdViewModel>
                    {
                        Code = -1,
                        Description = string.Empty,
                        Structure = viewModel
                    }
                });
            }

            Workbook workbook = viewModel.Workbook;

            _etdService.PrepareIndexOfETD(resultTestETD_Response.Structure, viewModel.ETDReportsDTO, ref workbook, viewModel.ClaveIdioma);

            #region fillColumns
            etdTestGeneralDTO = resultTestETD_Response.Structure.ETDTestsGeneral;
            #endregion

            viewModel.EtdTestDTO = etdTestGeneralDTO;
            viewModel.Workbook = workbook;
            bool resultReport = !resultTestETD_Response.Structure.ETDTestsGeneral.ETDTests.Any(x => x.Resultado is false);
            viewModel.IsReportAproved = resultReport;

            string errors = string.Empty;
            List<string> errorMessages = resultTestETD_Response.Structure.Results.Select(k => k.Message).ToList();
            string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
            bool advertencia = resultReport && errorMessages.Any();
            return Json(new
            {
                response = new ApiResponse<EtdViewModel>
                {
                    Code = advertencia ? -2 : 1,
                    Description = allError,
                    Structure = viewModel
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] EtdViewModel viewModel)
        {
            #region Export Excel
            int[] filas = new int[2];

            DateTime date = _etdService.GetDate(viewModel.Workbook, viewModel.ETDReportsDTO);

            Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

            PdfFormatProvider provider = new()
            {
                ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
            };
            document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.Workbook.ActiveSheet);
            FloatingImage image = null;
            int rowCount = 0;
            foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
            {
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

            List<Stream> filesStreams = new()
            {
                pdfStream,
                pdfStream,
                pdfStream,
                pdfStream
            };

            PdfDocumentBase resultaqui = PdfDocument.MergeFiles(filesStreams.ToArray());
            resultaqui.Save(pdfStream, Spire.Pdf.FileFormat.PDF);
            pdfFile = pdfStream.ToArray();
            #endregion

            #region Save Report
            ETDReportDTO etdTestsGeneralDTO = new()
            {
                IdRep = 0,
                FechaRep = date,
                NoSerie = viewModel.NoSerie,
                NoPrueba = viewModel.NoPrueba,
                ClaveIdioma = viewModel.ClaveIdioma,
                Cliente = viewModel.ETDReportsDTO.HeadboardReport.Client,
                Capacidad = viewModel.ETDReportsDTO.HeadboardReport.Capacity,
                Resultado = viewModel.IsReportAproved,
                NombreArchivo = string.Concat("ETD", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                Archivo = pdfFile,
                TipoReporte = ReportType.ETD.ToString(),
                ClavePrueba = viewModel.ClavePrueba,
                IdCorte = viewModel.ETDReportsDTO.IdCorte,
                IdReg = viewModel.ETDReportsDTO.IdReg,
                BtDifCap = viewModel.LVDifferentCapacity,
                CapacidadBt = viewModel.Capacity1,
                Terciario2b = viewModel.TerB2,
                TerCapRed = viewModel.TerReducedCapacity,
                CapacidadTer = viewModel.Capacity2,
                DevanadoSplit = viewModel.SplitWinding,
                UltimaHora = viewModel.ETDReportsDTO.UltimaHora,
                FactorKw = viewModel.ETDReportsDTO.FactorCorrecciónkW,
                FactorAltitud = viewModel.ETDReportsDTO.FactorAltitud,
                TipoRegresion = viewModel.RegressionType,
                Comentario = viewModel.Comments,
                Creadopor = User.Identity.Name,
                Fechacreacion = DateTime.Now,
                Modificadopor = null,
                Fechamodificacion = null,
                ETDTestsGeneral = viewModel.EtdTestDTO,

            };
            #endregion

            try
            {
                string a = JsonConvert.SerializeObject(etdTestsGeneralDTO);
                ApiResponse<long> result = await _etdClientService.SaveReport(etdTestsGeneralDTO);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = etdTestsGeneralDTO.NombreArchivo, file = etdTestsGeneralDTO.Archivo };

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
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.ETD.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");

            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");
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