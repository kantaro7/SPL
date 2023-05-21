extern alias DOCUMENTSCORE;
namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Identity.Web;

    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.ViewModels;
    using Telerik.Web.Spreadsheet;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export;
    using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
    using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

    public class BaseTemplateController : Controller
    {
        private readonly IArtifactClientService _artifactClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly ITestClientService _testClientService;
        private readonly IProfileSecurityService _profileClientService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BaseTemplateController(
            IArtifactClientService artifactClientService,
            IReportClientService reportClientService,
            IMasterHttpClientService masterHttpClientService,
            ITestClientService testClientService,
            IProfileSecurityService profileClientService , IWebHostEnvironment hostEnvironment)
        {
            this._artifactClientService = artifactClientService;
            this._reportClientService = reportClientService;
            this._masterHttpClientService = masterHttpClientService;
            this._testClientService = testClientService;
            this._profileClientService = profileClientService;
            this._hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        
        {
          
            try
            {

               /* Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook2;
                IWorkbookFormatProvider formatProvider2 = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();

                using (Stream input2 = new FileStream(@"C:\Users\Barboza\Downloads\ETD.xlsx", FileMode.Open))
                {
                    workbook2 = formatProvider2.Import(input2);

                }*/

               /* byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Barboza\Downloads\ETD.xlsx");
                Stream stream2 = new MemoryStream(bytes);

                Workbook workbook = Workbook.Load(stream2, ".xlsx");
                Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = workbook.ToDocument();

                var y =workbook.Sheets[0].Rows[10].Cells[11]?.Value?.ToString();
               */
               /*
                document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == "Rep.F3");
                var p2 = document.ActiveSheet; 
   
               

                foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
                {

                    if (sheet.Name != "Rep.F3")
                    {
                        sheet.Visibility = Telerik.Windows.Documents.Spreadsheet.Model.SheetVisibility.Hidden;
                        //document.Worksheets.Remove(sheet);
                    }
                    else
                    {

                        var image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(0, 0), 0, 0);
                        string path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
                        FileStream stream = new(path, FileMode.Open);
                        using (stream)
                        {
                            image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                        }

                        image.Width = 215;
                        image.Height = 38;

                        sheet.Shapes.Add(image);

                        image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(59, 0), 0, 0);
                        path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
                        stream = new(path, FileMode.Open);
                        using (stream)
                        {
                            image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                        }

                        image.Width = 215;
                        image.Height = 38;
                        sheet.Shapes.Add(image);

                        image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(118, 0), 0, 0);
                        path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
                        stream = new(path, FileMode.Open);
                        using (stream)
                        {
                            image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                        }

                        image.Width = 215;
                        image.Height = 38;
                       sheet.Shapes.Add(image);


                        /*
                        image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(81, 7), 0, 0);
                        path = Path.Combine(@"C:\Users\Barboza\Downloads\chart (3).png");
                        stream = new(path, FileMode.Open);
                        using (stream)
                        {
                            image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                        }

                        image.Width = 400;
                        image.Height = 400;
                        sheet.Shapes.Add(image);


                        image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(139, 7), 0, 0);
                        path = Path.Combine(@"C:\Users\Barboza\Downloads\chart (3).png");
                        stream = new(path, FileMode.Open);
                        using (stream)
                        {
                            image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                        }

                        image.Width = 400;
                        image.Height = 400;
                        sheet.Shapes.Add(image);
                        

                        PageBreaks pageBreaks = sheet.WorksheetPageSetup.PageBreaks;

                        _ = pageBreaks.TryInsertHorizontalPageBreak(59, 0);
                        _ = pageBreaks.TryInsertHorizontalPageBreak(118, 0);
                        // _ = pageBreaks.TryInsertHorizontalPageBreak(59, 0);
                         //sheet.Rows.Insert(58);
                        sheet.WorksheetPageSetup.PaperType = Telerik.Windows.Documents.Model.PaperTypes.A4;
                        sheet.WorksheetPageSetup.PageOrientation = Telerik.Windows.Documents.Model.PageOrientation.Portrait;
                        sheet.WorksheetPageSetup.CenterHorizontally = true;
                        sheet.WorksheetPageSetup.PrintOptions.PrintGridlines = false;
                        sheet.WorksheetPageSetup.ScaleFactor = new Telerik.Documents.Primitives.Size(0.9, 1);
                        sheet.WorksheetPageSetup.Margins =
                            new Telerik.Windows.Documents.Spreadsheet.Model.Printing.PageMargins(0
                            , 20, 0, 0);
                    }

                }*/
                //var a= workbook.Sheets[0].Rows[10].Cells[7].Value.ToString();
               /* Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsm.XlsmFormatProvider();
                byte[] excelFile;
                byte[] pdfFile;
                string file;
               


                using (MemoryStream stream = new())
                {
                    formatProvider.Export(document, stream);
                    excelFile = stream.ToArray();
                    System.IO.File.WriteAllBytes(@"C:\Users\Barboza\Desktop\s.xlsm", excelFile);
                    file = Convert.ToBase64String(stream.ToArray());
                }

                Spire.Xls.Workbook wbFromStream = new();

                wbFromStream.LoadFromStream(new MemoryStream(excelFile));
                MemoryStream pdfStream = new();
                wbFromStream.SaveToStream(pdfStream, Spire.Xls.FileFormat.PDF);

                pdfFile = pdfStream.ToArray();
                var aefd = Convert.ToBase64String(pdfFile);*/

            }
            catch(Exception e)
            {

            }

            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.PlantillaBaseReportes)))
                {
                    //var a = await this._artifactClientService.GetBaseTemplateConsolidatedReport("EN");
                    //byte[] aa = System.IO.File.ReadAllBytes(@"C:\Users\Barboza\Downloads\img3.jpg");
                    //var o = new BaseTemplateConsolidatedReportDTO
                    //{
                    //    NombreArchivo = "adasd",
                    //    ClaveIdioma = "sd",
                    //    Creadopor = "SYS",
                    //    Fechacreacion = DateTime.Now,
                    //    Plantilla = Convert.ToBase64String(aa)
                    //};

                    //var asa = await this._artifactClientService.AddBaseTemplateConsolidatedReport(o);
                    IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Id = 0, Clave = string.Empty, Descripcion = "Seleccione..." } }.AsEnumerable();

                    IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));

                    this.ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion", "");

                    ApiResponse<IEnumerable<ReportsDTO>> result = await this._reportClientService.GetReportTypes();
                    List<GeneralPropertiesDTO> reports = new();

                    foreach (ReportsDTO item in result.Structure)
                    {
                        reports.Add(new GeneralPropertiesDTO
                        {
                            Clave = item.TipoReporte,
                            Descripcion = item.Descripcion
                        });
                    }

                    generalProperties = origingeneralProperties.Concat(reports);

                    this.ViewBag.ReportItems = new SelectList(generalProperties, "Clave", "Descripcion");

                    this.ViewBag.testItems = new SelectList(origingeneralProperties, "Clave", "Descripcion");
                    var model = new BaseTemplateViewModel();

                    return this.View();
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

        [HttpGet]
        public async Task<IActionResult> GetTest(string typeReport, string keyTest)
        {
            try
            {
                if (string.IsNullOrEmpty(keyTest))
                    keyTest = "-1";

                ApiResponse<IEnumerable<TestsDTO>> result = await this._testClientService.GetTest(typeReport, keyTest);

                return this.Json(new
                {
                    response = result
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

        [HttpGet]
        public async Task<IActionResult> Get(string noSerie, bool? newTensonResponse) => this.Json(new
        {
            response = new ApiResponse<PlateTensionViewModel>
            {
                Code = 1,
                Description = string.Empty,
                Structure = null
            }
        });

        [HttpPost]
        public async Task<IActionResult> Save([FromForm] BaseTemplateViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                byte[] imageArray;
                string imageCodeBase64 = string.Empty;

                if (viewModel.File != null)
                {
                    using MemoryStream memoryStream = new();
                    await viewModel.File.CopyToAsync(memoryStream);
                    imageArray = memoryStream.ToArray();

                    IEnumerable<FileWeightDTO> fileConfigModule = new List<FileWeightDTO>();

                    ApiResponse<IEnumerable<FileWeightDTO>> getConfigModulResponse = await this._masterHttpClientService.GetConfigurationFiles(3);

                    if (getConfigModulResponse.Code.Equals(-1))
                    {
                        return this.Json(new
                        {
                            response = getConfigModulResponse
                        });
                    }
                    else
                    {
                        fileConfigModule = getConfigModulResponse.Structure;
                    }

                    #region Validate Document

                    string extension = Path.GetExtension(viewModel.File.FileName);

                    FileWeightDTO configModule = fileConfigModule.FirstOrDefault(c => c.ExtensionArchivoNavigation.Extension.Equals(extension));

                    if (configModule is null)
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<string>
                            {
                                Code = -1,
                                Description = "Archivo no permitido."
                            }
                        });
                    }

                    int size = int.Parse(configModule.MaximoPeso);

                    if (imageArray.Length > size * 1024 * 1024)
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<string>
                            {
                                Code = -1,
                                Description = string.Format("Documento muy grande, debe ser menor que {0}mb.", size * 1024 * 1024)
                            }
                        });
                    }

                    BaseTemplateDTO dto = new()
                    {
                        ClaveIdioma = viewModel.Language,
                        ClavePrueba = viewModel.TestId,
                        TipoReporte = viewModel.ReportId,
                        ColumnasConfigurables = viewModel.NoColumn,
                        Plantilla = Convert.ToBase64String(imageArray),
                        Creadopor = User.Identity.Name,
                        Modificadopor = User.Identity.Name,
                        Fechamodificacion = DateTime.Now
                    };
                    #endregion

                    ApiResponse<long> result = await this._artifactClientService.AddBaseTemplate(dto);

                    return this.Json(new
                    {
                        response = result
                    });
                }
            }

            return this.Json(new
            {
                response = new ApiResponse<PlateTensionViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = null
                }
            });
        }
        public async Task<IActionResult> GetConfigurationFiles(int pIdModule)
        {
            ApiResponse<IEnumerable<FileWeightDTO>> getConfigModulResponse = await this._masterHttpClientService.GetConfigurationFiles(3);
            return this.Json(new
            {
                response = getConfigModulResponse
            });
        }

        private BaseTemplateViewModel dame()
        {
            List<Coord2> coordenadas = new List<Coord2>()
            {

                new Coord2
                {
                    x = 0,
                    y = 0
                },
                new Coord2
                {
                    x = 3,
                   y = 0
                },
                new Coord2
                {
                    x = 6,
                    y = 0
                },
                new Coord2
                {
                    x = 9,
                    y = 0
                },
                new Coord2
                {
                    x = 12,
                    y = 0
                },
                new Coord2
                {
                    x = 15,
                    y = 0
                },
                new Coord2
                {
                    x = 18,
                    y = 84.53500M
                },
                new Coord2
                {
                    x = 19.5M,
                    y = 84.49000M
                },
                new Coord2
                {
                    x = 21,
                    y = 84.44000M
                },
                new Coord2
                {
                    x = 22.5M,
                    y = 84.395M
                },
                new Coord2
                {
                    x = 24,
                    y = 84.345M
                },
                new Coord2
                {
                    x = 25.5M,
                    y = 84.31007M
                },
                new Coord2
                {
                    x = 27,
                    y = 84.26500M
                },
                new Coord2
                {
                    x = 28.5M,
                    y =84.22500M
                },
                new Coord2
                {
                    x = 30,
                    y = 84.185000M
                },
                new Coord2
                {
                    x = 31.5M,
                    y = 84.15000M
                },
                new Coord2
                {
                    x =33,
                    y = 84.11500M
                },
                new Coord2
                {
                    x = 34.5M,
                    y = 84.07500M
                },
                new Coord2
                {
                    x = 36,
                    y = 84.04000M
                },
                new Coord2
                {
                    x = 37.5M,
                    y = 84.0100M
                },
                new Coord2
                {
                    x = 39,
                    y =  83.99800M
                },
                new Coord2
                {
                    x = 40.5M,
                    y = 83.94500M
                },
                  new Coord2
                {
                    x = 42,
                    y =  83.92000M
                },
                  new Coord2
                {
                    x = 43.5M,
                    y =  83.89000M
                },
                    new Coord2
                {
                    x = 45,
                    y =  83.86000M
                },
                     new Coord2
                {
                    x = 46.5M,
                    y =  83.83500M
                },
                  new Coord2
                {
                    x = 48,
                    y =  83.8050M
                },
                  new Coord2
                {
                    x = 49.5M,
                    y =  83.78500M
                },
                    new Coord2
                {
                    x = 51,
                    y =  83.76000M
                },
                     new Coord2
                {
                    x = 52.5M,
                    y =  83.73500M
                },
                  new Coord2
                {
                    x = 54,
                    y =  83.71000M
                },
                  new Coord2
                {
                    x = 55.5M,
                    y = 83.69000M
                },
                    new Coord2
                {
                    x = 57,
                    y =  83.65500M
                },
     new Coord2
                {
                    x = 58.5M,
                    y =  83.64500M
                },
          new Coord2
                {
                    x = 60,
                    y =  83.65000M
                }
            };



             decimal maxY = coordenadas.Where(p =>p.y!=0).Max(x => x.y);
            decimal minY = coordenadas.Where(p => p.y != 0).Min(x => x.y);
            decimal maxX = coordenadas.Max(x => x.x);
            decimal  minX = coordenadas.Min(x => x.x);

            coordenadas = coordenadas.Where(x => x.y != 0).ToList();


            decimal[][] al = coordenadas.Select(x => new decimal[] { x.x, x.y }).ToArray();


            var ret = new BaseTemplateViewModel { };
            ret.data = al;
            ret.MaxX = maxX+1;
            ret.MinX = minX;
            ret.MaxY = maxY +1.1M;
            ret.MinY = minY - 0.35M;
            return ret;

        }
    }

    partial class Coord2
    {
        public decimal x { get; set; }
        public decimal y { get; set; }
    }
}

