namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
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

    public class NozzleBrandTypeController : Controller
    {

        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly INozzleBrandTypeClientService _nozzleBrandTypeClientService;
        private readonly IProfileSecurityService _profileClientService;
        public NozzleBrandTypeController(
            ICorrectionFactorService correctionFactorService,
            INozzleBrandTypeClientService nozzleBrandTypeClientService,
            IProfileSecurityService profileClientService)
        {
            _correctionFactorService = correctionFactorService;
            _nozzleBrandTypeClientService = nozzleBrandTypeClientService;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.TiposdeBoquillasporMarca)))
                {

                    await PrepareForm();

                    return View(new NozzleBrandTypeViewModel());
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
        public async Task<IActionResult> DeleteNozzlesBrandType([FromBody] NozzleBrandTypeViewModel request)
        {
            try
            {
                Thread.Sleep(5500);
                        ApiResponse<long> result = await _nozzleBrandTypeClientService.DeleteTypeNozzleMarks(
                    new NozzleMarksDTO
                    {
                        Estatus = request.Status.Equals(Status.Activo),
                        IdMarca = Convert.ToInt64(request.BrandId),
                        IdTipo = request.OperationType == (int)CorrectorFactorOperation.create ? -1 : request.Type,
                        Descripcion = request.Description,
                        Fechacreacion = DateTime.Now,
                        Creadopor = "user",
                        Fechamodificacion = null,
                        Modificadopor = string.Empty
                    });

                return Json(new { response = result });
            }
            catch(Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<int>
                    {
                        Description = e.Message,
                        Code = -1,
                        Structure = 0
                    }
                });
            }
    
        }

        [HttpPost]
        public async Task<IActionResult> SaveRegisterNozzleTypeMark([FromBody] NozzleBrandTypeViewModel request)
        {
            try
            {
                ApiResponse<long> result = await _nozzleBrandTypeClientService.SaveRegisterNozzleTypeMark(
                new NozzleMarksDTO
                {
                    Estatus = request.Status.Equals(Status.Activo),
                    IdMarca = Convert.ToInt64(request.BrandId),
                    IdTipo = request.OperationType == (int)CorrectorFactorOperation.create ? -1 : request.Type,
                    Descripcion = request.Description,
                    Fechacreacion = request.OperationType == (int)CorrectorFactorOperation.create ? DateTime.Now : request.FechaCreacion.Value,
                    Creadopor = request.OperationType == (int)CorrectorFactorOperation.create ? User.Identity.Name : request.CreadoPor,
                    Fechamodificacion = request.OperationType == (int)CorrectorFactorOperation.update ? DateTime.Now : null,
                    Modificadopor = request.OperationType == (int)CorrectorFactorOperation.update ? User.Identity.Name : null,
                }
            );

                if (result.Code == 1)
                {
                    result.Description = request.OperationType == (int)CorrectorFactorOperation.create ? "Registro exitoso" : "Actualización exitosa";
                }

                ApiResponse<List<NozzleMarksDTO>> listNozzlesByBrand = await _nozzleBrandTypeClientService.GetNozzleTypesByBrand(Convert.ToInt64(request.BrandId));
                ApiResponse<List<NozzleMarksDTO>> brandNozzles = await _nozzleBrandTypeClientService.GetBrandType(new TypeNozzleMarksDTO { IdMarca = -1, Estatus = true });

                listNozzlesByBrand.Structure.ForEach(s =>
                {
                    s.Marca = brandNozzles.Structure.FirstOrDefault(c => c.IdMarca == s.IdMarca)?.Descripcion;
                    s.EstatusId = s.Estatus ? 0 : 1;
                });

                ApiResponse<NozzleMarksDTO> response = new();

                if (request.OperationType == (int)CorrectorFactorOperation.create)
                {
                    response.Code = 1;
                    response.Description = result.Description;
                    response.Structure = listNozzlesByBrand.Structure.LastOrDefault();
                }
                else
                {
                    response.Code = 1;
                    response.Description = result.Description;
                    response.Structure = listNozzlesByBrand.Structure.FirstOrDefault(c => c.IdTipo.Equals(Convert.ToDecimal(request.Type)));
                }

                return Json(new { response });
            }
            catch(Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<int>
                    {
                        Description = e.Message,
                        Code = -1,
                        Structure = 0
                    }
                });
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> ValidateNewNozzleTypeBrand(decimal brandId, string description)
        {
            try
            {
                ApiResponse<List<NozzleMarksDTO>> result = await _nozzleBrandTypeClientService.GetNozzleTypesByBrand(-1);
                NozzleMarksDTO exists = result.Structure.Where(x => x.IdMarca == Convert.ToInt64(brandId) && x.Descripcion.ToLower().Equals(description.ToLower())).FirstOrDefault();
                return exists == null
                    ? Json(new { response = new ApiResponse<int> { Code = 1, Structure = 0 } })
                    : (IActionResult)Json(new { response = new ApiResponse<int> { Code = 1, Structure = 1 } });
            }
            catch(Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<int>
                    {
                        Description = e.Message,
                        Code = -1,
                        Structure = 0
                    }
                });
            }
            

        }

        public async Task PrepareForm()
        {
            ApiResponse<List<NozzleMarksDTO>> listNozzlesByBrand = await _nozzleBrandTypeClientService.GetNozzleTypesByBrand(-1);
            listNozzlesByBrand.Structure = listNozzlesByBrand.Structure.OrderByDescending(c => c.Fechacreacion).ToList();

            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Description = "Seleccione..." } }.AsEnumerable();

            IList<GeneralPropertiesDTO> generalProperties = new List<GeneralPropertiesDTO>();

            ApiResponse<List<NozzleMarksDTO>> brandNozzles = await _nozzleBrandTypeClientService.GetBrandType(new TypeNozzleMarksDTO { IdMarca = -1, Estatus = true });

            generalProperties.Add(new GeneralPropertiesDTO { Clave = string.Empty, Description = "Seleccione..." });
            foreach (NozzleMarksDTO item in brandNozzles.Structure)
            {
                generalProperties.Add(new GeneralPropertiesDTO { Clave = item.IdMarca.ToString(), Description = item.Descripcion });
            }

            ViewBag.BrandItems = new SelectList(generalProperties, "Clave", "Description");

            generalProperties = new List<GeneralPropertiesDTO>();
            int indexStatus = 0;

            generalProperties.Add(new GeneralPropertiesDTO
            {
                Clave = "",
                Description = "Seleccione..."
            });

            foreach (string item in Enum.GetNames(typeof(Status)))
            {
                generalProperties.Add(new GeneralPropertiesDTO
                {
                    Clave = indexStatus.ToString(),
                    Description = item
                });
                indexStatus++;
            }

            ViewBag.StatusItems = new SelectList(generalProperties, "Clave", "Description");

            listNozzlesByBrand.Structure.ForEach(s =>
            {
                s.Marca = brandNozzles.Structure.FirstOrDefault(c => c.IdMarca == s.IdMarca)?.Descripcion;
                s.EstatusId = s.Estatus ? 0 : 1;
            });

            ViewBag.ListNozzlesByBrand = listNozzlesByBrand.Structure.AsEnumerable().OrderBy(x=>  x.IdMarca ).ThenBy(o=>o.IdTipo).ToList();

        }
    }
}
