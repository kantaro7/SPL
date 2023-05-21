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
    public class CemController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly ICemClientService _cemClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly ICemService _cemService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProfileSecurityService _profileClientService;
        public CemController(
            IMasterHttpClientService masterHttpClientService,
            ICemClientService cemClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            ICemService cemService,
            IWebHostEnvironment hostEnvironment,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _cemClientService = cemClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _cemService = cemService;
            _hostEnvironment = hostEnvironment;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.CorrientedeExcitaciónMonofásica)))
                {

                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new CemViewModel { NoSerie = noSerie });
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

            CemViewModel cemViewModel = new();
            noSerie = noSerie.ToUpper().Trim();
            string noSerieSimple = string.Empty;

            ApiResponse<PositionsDTO> dataSelect = await _gatewayClientService.GetPositions(noSerie);

            if (dataSelect.Code == -1)
            {
                return Json(new
                {
                    response = new ApiResponse<RdtViewModel>
                    {
                        Code = -1,
                        Description = "Error al obtener las posiciones para el numero de Serie " + noSerie,
                        Structure = null
                    }
                });
            }
            else
            {

                if (dataSelect.Structure.AltaTension.Count() == 0 && dataSelect.Structure.BajaTension.Count() == 0 && dataSelect.Structure.Terciario.Count() == 0)
                {
                    return Json(new
                    {
                        response = new ApiResponse<RdtViewModel>
                        {
                            Code = -1,
                            Description = "No se encuentran registradas posiciones en el módulo Tensión de la placa  para el numero de Serie " + noSerie,
                            Structure = null
                        }
                    });
                }
                cemViewModel.Positions = dataSelect.Structure;
            }

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
                InformationArtifactDTO artifactDesing = await _artifactClientService.GetArtifact(noSerieSimple);
                if (artifactDesing.GeneralArtifact.OrderCode == null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<FpcViewModel>
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
                        response = new ApiResponse<CemViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee características",
                            Structure = null
                        }
                    });
                }

                cemViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _cemClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportCem = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.CEM.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.CEM.ToString() };
                IEnumerable<IGrouping<bool?, InfoGeneralReportsDTO>> reportRdtGroupStatus = reportCem.ListDetails.GroupBy(c => c.Resultado);

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

                cemViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Corriente de Excitación Monofásica",
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
                response = new ApiResponse<CemViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = cemViewModel
                }
            });
        }

        public async Task<IActionResult> ValidateFilter(string nroSerie, string keyTest, string lenguage,
                string idPosPrimary, string idPosSecundary, int cantPosPri, int cantPosSec, string posicionesPrimarias, string posicionesSecundarias, string testsVoltage, string comment, bool statusAllPosSec)
        {
            ApiResponse<PositionsDTO> resultP = await _gatewayClientService.GetPositions(nroSerie);

            return resultP.Code == -1
                ? Json(new
                {
                    response = new ApiResponse<bool>
                    {
                        Code = -1,
                        Description = "Error al obtener las posiciones para el numero de Serie " + nroSerie,
                        Structure = false
                    }
                })
                : (IActionResult)Json(new
                {
                    response = new ApiResponse<bool>
                    {
                        Code = 1,
                        Description = "",
                        Structure = true
                    }
                });
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string nroSerie, string keyTest, string lenguage,
                string idPosPrimary, string idPosSecundary, int cantPosPri, int cantPosSec, string posicionesPrimarias, string posicionesSecundarias, string testsVoltage, string comment, bool statusAllPosSec)
        {
            ApiResponse<PositionsDTO> resultP = await _gatewayClientService.GetPositions(nroSerie);

            ApiResponse<SettingsToDisplayCEMReportsDTO> result = await _gatewayClientService.GetTemplateCEM(nroSerie, keyTest, lenguage, idPosPrimary, posicionesPrimarias, idPosSecundary, posicionesSecundarias, decimal.Parse(testsVoltage));

            if (result.Code.Equals(-1))
            {
                return View("Excel", new CemViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayCEMReportsDTO reportInfo = result.Structure;

            if (reportInfo.SecondaryPositions.Count() == 0)
            {
                return View("Excel", new CemViewModel
                {
                    Error = reportInfo.MessageInformation
                });
            }

            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return View("Excel", new CemViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = reportInfo.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");

            _cemService.PrepareTemplate_CEM(reportInfo, ref workbook, idPosPrimary, idPosSecundary, posicionesPrimarias, posicionesSecundarias, testsVoltage,lenguage);

            return View("Excel", new CemViewModel
            {
                ClaveIdioma = lenguage,
                ClavePrueba = keyTest,
                NoPrueba = NoPrueba,
                NoSerie = nroSerie,

                VoltajePrueba = testsVoltage,

                PosicionPrimaria = idPosPrimary,
                PosicionSecundaria = idPosSecundary,
                SettingsCEM = result.Structure,
                ReigstrosPosicionesPrimarias = posicionesPrimarias.Split(','),
                ReigstrosPosicionesSecundarias = posicionesSecundarias.Split(','),
                Workbook = workbook,
                Error = string.Empty,
                Comments = comment,
                Positions = resultP.Structure,
                CantidadPosicionesSecundarias = posicionesSecundarias.Split(",").Length,
                StatusAllPosSec = statusAllPosSec
            });
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] CemViewModel viewModel)
        {
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

            pdfFile = pdfStream.ToArray();
            viewModel.Base64PDF = Convert.ToBase64String(pdfFile);

            DateTime basedate = new(1899, 12, 30);

            List<CEMTestsDetailsDTO> test = new();

            int[] _positionWB = GetRowColOfWorbook(viewModel.SettingsCEM.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PosSec")).Celda);
            bool pasar = true;
            for (int i = 0; i < viewModel.CantidadPosicionesSecundarias; i++)
            {
                _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 1]?.Value?.ToString(), out decimal current);
                _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 2]?.Value?.ToString(), out decimal current2);
                _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1] + 3]?.Value?.ToString(), out decimal current3);

                if (current == 0 || current2 == 0 || current3 == 0)
                {
                    pasar = false;
                    break;
                }

                //if (current == 0 && current2 == 0 && current3 == 0)
                //{
                    test.Add(new CEMTestsDetailsDTO
                    {
                        PosSec = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString(),
                        CurrentTerm1 = current,
                        CurrentTerm2 = current2,
                        CurrentTerm3 = current3
                    });
                //}
            }

            _positionWB = GetRowColOfWorbook(viewModel.SettingsCEM.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
            string resultPru = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

            if (resultPru == "Seleccione...")
            {
                return Json(new
                {
                
                         response = new { status = -1, description = "Debe seleccionar el resultado de la prueba", nameFile = "", file = "" }

                });
            }

            bool aprovado = resultPru.ToUpper() is "ACCEPTED" or "ACEPTADO";

            if (!pasar)
            {
                return Json(new
                {
                    response = new { status = -1, description = "Debe llenar todos los valores asociados a la corriente", nameFile = "", file = "" }

                  
                }); ;


            }

            _positionWB = GetRowColOfWorbook(viewModel.SettingsCEM.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitColumna1")).Celda);
            string titcol1 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

            _positionWB = GetRowColOfWorbook(viewModel.SettingsCEM.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitColumna2")).Celda);
            string titcol2 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

            _positionWB = GetRowColOfWorbook(viewModel.SettingsCEM.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TitColumna3")).Celda);
            string titcol3 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

            _positionWB = GetRowColOfWorbook(viewModel.SettingsCEM.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
            string fecha = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

            fecha = fecha.Contains("/") ? (DateTime.Now.Date - basedate.Date).TotalDays.ToString() : fecha;
            viewModel.Date = fecha;

            CEMTestsGeneralDTO cemTestGeneral = new()
            {
                Capacity = viewModel.SettingsCEM.HeadboardReport.Capacity,
                Comment = viewModel.Comments,
                Creadopor = User.Identity.Name,
                Customer = viewModel.SettingsCEM.HeadboardReport.Client,
                Fechacreacion = DateTime.Now,
                File = pdfFile,
                IdLoad = 0,
                KeyTest = viewModel.ClavePrueba,
                LanguageKey = viewModel.ClaveIdioma,
                LoadDate = basedate.AddDays(int.Parse(viewModel.Date)),
                Modificadopor = null,
                Fechamodificacion = null,
                NameFile = string.Concat("CEM", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                Result = aprovado,
                SerialNumber = viewModel.NoSerie,
                TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                TypeReport = ReportType.CEM.ToString(),
                CEMTestsDetails = test,
                StatusAllPosSec = viewModel.StatusAllPosSec,
                IdPosPrimary = viewModel.PosicionPrimaria,
                IdPosSecundary = viewModel.PosicionSecundaria,
                PosPrimary = string.Join(",", viewModel.ReigstrosPosicionesPrimarias),
                PosSecundary = string.Join(",", viewModel.ReigstrosPosicionesSecundarias),
                TestsVoltage = decimal.Parse(viewModel.VoltajePrueba),
                TitTerm1 = titcol1,
                TitTerm2 = titcol2,
                TitTerm3 = titcol3,

            };

            try
            {
                ApiResponse<long> result = await _cemClientService.SaveReport(cemTestGeneral);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = cemTestGeneral.NameFile, file = viewModel.Base64PDF };

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

            /// var a = await this._gatewayClientService.GetPositions("G4135-01");

            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            // Tipos de prueba
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.CEM.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");

            ViewBag.Select1 = new SelectList(new List<GeneralPropertiesDTO>()
            {
                new GeneralPropertiesDTO() { Clave = "", Description = "Todas" } ,

            }.AsEnumerable(), "Clave", "Descripcion");

            ViewBag.Select2 = new SelectList(new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" },

            }.AsEnumerable(), "Clave", "Descripcion");

            ViewBag.Select3 = new SelectList(new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" } }.AsEnumerable(), "Clave", "Descripcion");

            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");

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
        #endregion
    }
}

