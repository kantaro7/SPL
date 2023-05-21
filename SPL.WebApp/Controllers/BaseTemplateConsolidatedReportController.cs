
namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Identity.Web;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.ViewModels;

    public class BaseTemplateConsolidatedReportController : Controller
    {
        private readonly IArtifactClientService _artifactClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly ITestClientService _testClientService;
        private readonly IProfileSecurityService _profileClientService;
        public BaseTemplateConsolidatedReportController(
            IArtifactClientService artifactClientService,
            IReportClientService reportClientService,
            IMasterHttpClientService masterHttpClientService,
            ITestClientService testClientService,
            IProfileSecurityService profileClientService)
        {
            _artifactClientService = artifactClientService;
            _reportClientService = reportClientService;
            _masterHttpClientService = masterHttpClientService;
            _testClientService = testClientService;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.PlantillaBaseReporteConsolidado)))
                {

                    /* var a = await this._artifactClientService.GetBaseTemplateConsolidatedReport("EN");
                     byte[] aa = System.IO.File.ReadAllBytes(@"C:\Users\Barboza\Downloads\img3.jpg");
                     var o = new BaseTemplateConsolidatedReportDTO
                     {
                         NombreArchivo = "adasd",
                         ClaveIdioma = "sd",
                         Creadopor = "SYS",
                         Fechacreacion = DateTime.Now,
                         Plantilla = Convert.ToBase64String(aa)
                     };

                     var asa = await this._artifactClientService.AddBaseTemplateConsolidatedReport(o);*/
                    IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Id = 0, Clave = string.Empty, Descripcion = "Seleccione..." } }.AsEnumerable();

                    IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));

                    ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion", "");

                    return View(new BaseTemplateConsolidatedReportViewModel());
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

        public async Task<IActionResult> Save([FromForm] BaseTemplateConsolidatedReportViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                byte[] imageArray;
                string imageCodeBase64 = string.Empty;

                if (viewModel.File != null)
                {
                    using MemoryStream memoryStream = new();
                    await viewModel.File.CopyToAsync(memoryStream);
                    imageArray = memoryStream.ToArray();

                    IEnumerable<FileWeightDTO> fileConfigModule = new List<FileWeightDTO>();

                    ApiResponse<IEnumerable<FileWeightDTO>> getConfigModulResponse = await _masterHttpClientService.GetConfigurationFiles(9);

                    if (getConfigModulResponse.Code.Equals(-1))
                    {
                        return Json(new
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
                        return Json(new
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
                        return Json(new
                        {
                            response = new ApiResponse<string>
                            {
                                Code = -1,
                                Description = string.Format("Documento muy grande, debe ser menor que {0}mb.", size * 1024 * 1024)
                            }
                        });
                    }

                    BaseTemplateConsolidatedReportDTO dto = new()
                    {
                        ClaveIdioma = viewModel.Language,
                        NombreArchivo = Path.GetFileNameWithoutExtension(viewModel.File.FileName),
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Plantilla = Convert.ToBase64String(imageArray)
                    };
                    #endregion

                    ApiResponse<long> result = await _artifactClientService.AddBaseTemplateConsolidatedReport(dto);

                    return Json(new
                    {
                        response = result
                    });
                }
            }

            return Json(new
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
            ApiResponse<IEnumerable<FileWeightDTO>> getConfigModulResponse = await _masterHttpClientService.GetConfigurationFiles(pIdModule);
            return Json(new
            {
                response = getConfigModulResponse
            });
        }
    }
}

