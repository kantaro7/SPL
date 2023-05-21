//extern alias FIXEDDOCUMENTS;

//using FIXEDDOCUMENTS::Telerik.Windows.Documents.UI;
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telerik.Documents.Common.Model;
using Telerik.Documents.Core.Fonts;
using Telerik.Documents.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Fixed.Utilities.Rendering;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Txt;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Editing;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Winnovative.PdfToImage;

namespace SPL.WebApp.Controllers
{
    public class ReportePruebasController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly INraClientService _nraClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly INraService _nraService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfigurationClientService _configurationClientService;
        private readonly IProfileSecurityService _profileClientService;
        public ReportePruebasController(
         IMasterHttpClientService masterHttpClientService,
         INraClientService nraClientService,
         IReportClientService reportClientService,
         IArtifactClientService artifactClientService,
         ITestClientService testClientService,
         IGatewayClientService gatewayClientService,
         INraService nraService,
         IWebHostEnvironment hostEnvironment,
         IConfigurationClientService configurationClientService,
            IProfileSecurityService profileClientService
         )
        {
            this._masterHttpClientService = masterHttpClientService;
            this._nraClientService = nraClientService;
            this._reportClientService = reportClientService;
            this._artifactClientService = artifactClientService;
            this._testClientService = testClientService;
            this._gatewayClientService = gatewayClientService;
            this._nraService = nraService;
            this._hostEnvironment = hostEnvironment;
            this._configurationClientService = configurationClientService;
            this._profileClientService = profileClientService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(User.Identity.Name).Result;
                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.ConsultadePruebas)))
                {

                    // await this.GetReports(new ReporteConsolidadoViewModel());
                    await this.PrepareForm();
                    var noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return this.View(new ReportePruebasViewModel { NoSerie = noSerie });
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

        public async Task PrepareForm()
        {
            //ApiResponse<ReportPDFDto> result = await this._reportClientService.GetPDFReport(12,"FPB");
            //var a = this.Convertir(result.Structure.File, 59);7

            //var a = await this._gatewayClientService.GetPositions("G4135-01");
            // var a = await this._gatewayClientService.("G4135-01");

            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

                // Tipos de prueba7



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

            origingeneralProperties = origingeneralProperties.Concat(reports);

            this.ViewBag.ReportItems = new SelectList(origingeneralProperties, "Clave", "Descripcion");

            Thread.Sleep(5000);

        }

        public async Task<IActionResult> CheckInfo(string noSerie, string typeReport)
        {
            ReportePruebasViewModel model = new ReportePruebasViewModel();
            var apiResponse = await this._reportClientService.GetTestedReport(noSerie);

            if(apiResponse.Code == -1)
            {
                return Json(new ApiResponse<ReportePruebasViewModel>
                {
                    Structure = null,
                    Code = -1,
                    Description = apiResponse.Description
                });
            }

            if (apiResponse.Structure.Count() == 0)
            {
                return Json(new ApiResponse<ReportePruebasViewModel>
                {
                    Structure = null,
                    Code = -1,
                    Description = "No se encontraron resultados"
                });
            }
            else
            {
                if (!string.IsNullOrEmpty(typeReport))
                {
                    apiResponse.Structure =  apiResponse.Structure.Where(x => x.TIPO_REPORTE == typeReport).ToList();

                    if (apiResponse.Structure.Count() == 0)
                    {
                        return Json(new ApiResponse<ReportePruebasViewModel>
                        {
                            Structure = null,
                            Code = -1,
                            Description = "No se encontraron resultados"
                        });
                    }
                }
            }


            model.datasource = new List<DataSourcePruebas>();
            var data = apiResponse.Structure.GroupBy(x => new { x.TIPO_REPORTE, x.NOMBRE_REPORTE }).Select(c => new { TIPO_REPORTE = c.Key.TIPO_REPORTE, NOMBRE_REPORTE = c.Key.NOMBRE_REPORTE }).ToList();
            foreach (var item in data)
            {

                var lista = new DataSourcePruebas
                {
                    isParent = true,
                    expanded = false,
                    rowLevel = 1,
                    ID_REP = 0,
                    NOMBRE_REPORTE = item.NOMBRE_REPORTE,
                    TIPO_REPORTE = item.TIPO_REPORTE,
                    items = new List<DataSourcePruebas2>()

                };

                foreach (var item2 in apiResponse.Structure.Where(x => x.TIPO_REPORTE == item.TIPO_REPORTE))
                {
                    lista.items.Add(new DataSourcePruebas2
                    {
                        isParent = false,
                        expanded = false,
                        rowLevel = 2,
                        ID_REP = item2.ID_REP,
                        IDIOMA = item2.IDIOMA,
                        RESULTADO =  item2.RESULTADO ? "Aprobado" : "Rechazado",
                        TIPO_REPORTE = item2.TIPO_REPORTE,
                        NOMBRE_REPORTE = item2.NOMBRE_REPORTE,
                        FILTROS = string.IsNullOrEmpty( item2.FILTROS) ? string.Empty : item2.FILTROS,
                        COMENTARIOS = string.IsNullOrEmpty(item2.COMENTARIOS) ? string.Empty : item2.COMENTARIOS,
                        ID_PRUEBA = item2.ID_PRUEBA,
                        PRUEBA = item2.PRUEBA,
                        DESCRIPCION_EN = item2.DESCRIPCION_EN,
                        AGRUPACION_EN = item2.AGRUPACION_EN,
                        AGRUPACION = item2.AGRUPACION,
                        FECHA = item2.FECHA.Value.ToLocalTime().ToString(),
                        isChecked = ""
                    });
                }

                lista.items = lista.items.OrderByDescending(x => x.ID_REP).ToList();

                model.datasource.Add(lista);

            }


            return Ok(new ApiResponse<ReportePruebasViewModel>
            {
                Structure = model,
                Code = 1,
                Description = ""
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
    }

}
