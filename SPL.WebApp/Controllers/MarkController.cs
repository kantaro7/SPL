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

    public class MarkController : Controller
    {

        private readonly IMapper _mapper;
        private readonly INozzleMarkService _nozzleMarkService;
        private readonly IProfileSecurityService _profileClientService;
        public MarkController(INozzleMarkService nozzleMarkService,
          IMapper mapper,
            IProfileSecurityService profileClientService)
        {
            _mapper = mapper;
            _nozzleMarkService = nozzleMarkService;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(Domain.Enums.Menu.MarcasdeBoquillas)))
                {
                    ApiResponse<List<NozzleMarksDTO>> response = await _nozzleMarkService.GetNozzleBrands(-1);
                    List<GeneralPropertiesDTO> data = new()
                {
                    new GeneralPropertiesDTO { Clave = "1", Descripcion = "Activa" },
                    new GeneralPropertiesDTO { Clave = "0", Descripcion = "In-Activa" }
                };
                    ViewBag.Marks = response.Structure.OrderByDescending(x => x.IdMarca);
                    ViewBag.Estatus = new SelectList(data.AsEnumerable(), "Clave", "Descripcion", "1");
                    return View(new NozzleMarkViewModel());
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

        [HttpPost]
        public async Task<IActionResult> SaveMark([FromBody] NozzleMarksDTO request)
        {
            if (request.IdMarca == 0)
            {
                request.Fechacreacion = DateTime.Now;
                request.Creadopor = User.Identity.Name;
                request.Modificadopor = null;
                request.Fechamodificacion = null;
            }
            else
            {
                request.Fechamodificacion = DateTime.Now;
                request.Modificadopor = User.Identity.Name;

            }
            ApiResponse<long> result = await _nozzleMarkService.SaveNozzleBrands(request);

            if (result.Code == 1)
            {
                result.Description = request.IdMarca == 0 ? "Registro exitoso" : "Actualización exitosa";
                if (request.IdMarca == 0)
                {
                    ApiResponse<List<NozzleMarksDTO>> list = await _nozzleMarkService.GetNozzleBrands(-1);
                    result.Code = (int)list.Structure.OrderByDescending(x => x.IdMarca).First().IdMarca;
                }
            }

            return Json(new { response = result });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMark([FromBody] NozzleMarksDTO request)
        {
            ApiResponse<long> result = await _nozzleMarkService.DeleteNozzleBrands(request);

            return Json(new { response = result });
        }
    }
}
