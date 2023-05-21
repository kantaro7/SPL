namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Identity.Web;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.ViewModels;

    public class LimitCgdController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly ITestClientService _testClientService;
        private readonly IProfileSecurityService _profileClientService;
        public LimitCgdController(IMasterHttpClientService masterHttpClientService,
          IMapper mapper, ITestClientService testClientService,
            IProfileSecurityService profileClientService)
        {
            _mapper = mapper;
            _masterHttpClientService = masterHttpClientService;
            _testClientService = testClientService;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            ApiResponse<List<UserPermissionsDTO>> listPermissions;
            try
            {
                 listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

            }
            catch (MicrosoftIdentityWebChallengeUserException e)
            {
                return View("~/Views/PageConstruction/Error.cshtml");


            }
            catch (Exception ex)
            {

                return View("~/Views/PageConstruction/Error.cshtml");

            }
          
            if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.LímiteMáximodeContenidodeGas)))
            {

                ApiResponse<IEnumerable<ContGasCGDDTO>> response = await _masterHttpClientService.GetContGasCGD("-1", "-1", "-1");

                List<GeneralPropertiesDTO> listTests = new();
                List<GeneralPropertiesDTO> data = new()
            {
                new GeneralPropertiesDTO { Clave = "", Descripcion = "Seleccione..." },
                new GeneralPropertiesDTO { Clave = "CGD", Descripcion = "Cromatografía de Gases Disueltos en Aceite" },
                new GeneralPropertiesDTO { Clave = "FPA", Descripcion = "Factor de Potencia en Aceite Aislantes" }
            };

                ViewBag.Reports = new SelectList(data.AsEnumerable(), "Clave", "Descripcion", "1");

                List<GeneralPropertiesDTO> dataOildType = new()
            {
                new GeneralPropertiesDTO { Clave = "", Descripcion = "Seleccione..." },
                new GeneralPropertiesDTO { Clave = "Sintético", Descripcion = "Sintético" },
                new GeneralPropertiesDTO { Clave = "Mineral", Descripcion = "Mineral" },
                new GeneralPropertiesDTO { Clave = "Vegetal", Descripcion = "Vegetal" }
            };

                ViewBag.OildType = new SelectList(dataOildType.AsEnumerable(), "Clave", "Descripcion", "1");

                ApiResponse<IEnumerable<TestsDTO>> dataaca = GetTests("-1").Result;

                foreach (TestsDTO test in dataaca.Structure)
                {
                    listTests.Add(new GeneralPropertiesDTO() { Clave = test.ClavePrueba, Descripcion = test.Descripcion, Description = test.TipoReporte });
                }

                ViewBag.Tests = new SelectList(listTests.AsEnumerable(), "Clave", "Descripcion", "1");

                foreach (ContGasCGDDTO item in response.Structure)
                {
                    item.DesPrueba = listTests.FirstOrDefault(x => x.Clave.Equals(item.ClavePrueba)).Descripcion;
                    item.DesTipoReporte = data.FirstOrDefault(x => x.Clave.Equals(item.TipoReporte)).Descripcion;
                }

                ViewBag.LimitGas = response.Structure;

                return View(new LimitCgdViewModel());
            }
            else
            {
                return View("~/Views/PageConstruction/PermissionDenied.cshtml");

            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveLimitCgd([FromBody] ContGasCGDDTO request)
        {

            try
            {
                ApiResponse<IEnumerable<ContGasCGDDTO>> response = await _masterHttpClientService.GetContGasCGD(request.TipoReporte, request.ClavePrueba, request.TipoAceite);
                ApiResponse<long> result;
                if (response.Structure.Count() > 0)
                {
                    if (!request.IsSave)
                    {
                        request.Creadopor = response.Structure.FirstOrDefault().Creadopor;
                        request.Fechacreacion = response.Structure.FirstOrDefault().Fechacreacion;
                        request.Fechamodificacion = DateTime.Now;
                        request.Modificadopor = User.Identity.Name;
                        request.Id = response.Structure.FirstOrDefault().Id;
                    }
                    else
                    {
                        result = new ApiResponse<long>() { Code = -1, Description = "El límite máximo para el reporte, prueba y tipo de aceite ya existe", Structure = 0 };
                        return Json(new { response = result });
                    }
                }
                else
                {
                    request.Creadopor = User.Identity.Name;
                    request.Fechacreacion = DateTime.Now;
                }

                result = await _masterHttpClientService.SaveContGasCGD(request);

                return Json(new { response = new ApiResponse<LimitCgdViewModel>
                {
                    Code = result.Code,
                    Description = result.Description,
                    Structure = new LimitCgdViewModel
                    {
                        Fechacreacion = request.Fechacreacion,
                        Fechamodificacion =request.Fechamodificacion,
                        Creadopor =request.Creadopor,
                        Modificadopor = request.Modificadopor
                    }    
                }
                });
            }
            catch(Exception e)
            {
                return Json(new { response = new ApiResponse<int>{
                
                Code = -1,
                Description = e.Message,
                Structure = 0} });
            }
        
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLimitCgd([FromBody] ContGasCGDDTO request)
        {
            try
            {
                ApiResponse<long> result = await _masterHttpClientService.DeleteContGasCGD(request);

                return Json(new { response = result });
            }
            catch(Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<int>
                    {

                        Code = -1,
                        Description = e.Message,
                        Structure = 0
                    }
                });
            }
      
        }

        public async Task<ApiResponse<IEnumerable<TestsDTO>>> GetTests(string pTypeReport)
        {
            try
            {
                ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(pTypeReport.ToUpper(), "-1");

                return reportResult;
            }
            catch(Exception e)
            {
                return new ApiResponse<IEnumerable<TestsDTO>>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = null
                };
            }

        }
    }
}
