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
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.ViewModels;

    public class CorrectionFactorController : Controller
    {

        private readonly IMapper _mapper;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IProfileSecurityService _profileClientService;
        public CorrectionFactorController(ICorrectionFactorService correctionFactorService,
          IMapper mapper,
            IProfileSecurityService profileClientService)
        {
            _mapper = mapper;
            _correctionFactorService = correctionFactorService;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> FactorPorEspecificacion()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(Menu.FactordeCorrecciónparaFPporEspecificación)))
                {

                    ApiResponse<List<CorrectionFactorDTO>> response = await _correctionFactorService.GetAllDataFactor("-1", -1, -1);

                    List<GeneralPropertiesDTO> data = new() {
                new GeneralPropertiesDTO { Clave ="" , Descripcion ="Seleccione..." },
                new GeneralPropertiesDTO { Clave ="Otros" , Descripcion ="Otros" },
                new GeneralPropertiesDTO { Clave ="Doble" , Descripcion ="Doble" },
                new GeneralPropertiesDTO { Clave ="NMX" , Descripcion ="NMX" }
            };

                    ViewBag.Especificaciones = new SelectList(data.AsEnumerable(), "Clave", "Descripcion", "");
                    ViewBag.DataFactorCorrecion = response.Structure;

                    return await Task.Run(() => View());
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

        public async Task<IActionResult> FactorMarcaTipo()
        {

            ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

            if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(Menu.FactordeCorrecciónparaFPporMarcayTipo)))
            {

                ApiResponse<List<CorrectionFactorsXMarksXTypesDTO>> response = await _correctionFactorService.GetCorrectionFactorsXMarksXTypes();
                response.Structure = response.Structure.OrderByDescending(x => x.Fechacreacion).ToList();
                ApiResponse<List<NozzleMarksDTO>> marcas = await _correctionFactorService.GetNozzleMarks(new NozzleMarksDTO { IdMarca = -1, Estatus = true });

                List<TypeNozzleMarksDTO> tipo = new() {
                    new TypeNozzleMarksDTO { IdTipo =0 , Descripcion ="Seleccione..." }
                };

                List<NozzleMarksDTO> marca = new()
                {
                     new NozzleMarksDTO { IdMarca =0 , Descripcion ="Seleccione..." }
                };

                marca.AddRange(marcas.Structure);

                ViewBag.Marca = new SelectList(marca.AsEnumerable(), "IdMarca", "Descripcion", "");
                ViewBag.Tipo = new SelectList(tipo.AsEnumerable(), "IdTipo", "Descripcion", "");
                ViewBag.FactorPorMarcaYtipo = response.Structure;

                return await Task.Run(() => View());
            }
            else
            {
                return View("~/Views/PageConstruction/PermissionDenied.cshtml");

            }
        }

        [HttpGet]
        public async Task<IActionResult> ValidateNewRegister(decimal Temperatura, string Especificacion)
        {

            ApiResponse<List<CorrectionFactorDTO>> result = await _correctionFactorService.GetAllDataFactor(Especificacion, Temperatura, -1);
            return Json(new { response = result });
        }

        [HttpGet]
        public async Task<IActionResult> ValidateNewNozzleTypeMarkr(decimal MarcaId, decimal TipoId, decimal Temperatura)
        {

            ApiResponse<List<CorrectionFactorsXMarksXTypesDTO>> result = await _correctionFactorService.GetCorrectionFactorsXMarksXTypes();
            CorrectionFactorsXMarksXTypesDTO exists = result.Structure.Where(x => x.IdMarca == MarcaId && x.IdTipo == TipoId && x.Temperatura == Temperatura).FirstOrDefault();
            return exists == null
                ? Json(new { response = new ApiResponse<int> { Code = 1, Structure = 0 } })
                : (IActionResult)Json(new { response = new ApiResponse<int> { Code = 1, Structure = 1 } });

        }
        [HttpPost]
        public async Task<IActionResult> SaveRegister([FromBody] CorrectionFactorViewModel request)
        {
            try
            {
                bool nuevo = request.OperationType == (int)CorrectorFactorOperation.create;
                var newRegister = new CorrectionFactorDTO
                {
                    Temperatura = decimal.Parse(request.TemperatureId),
                    Especificacion = request.EspecificacionId,
                    FactorCorr = decimal.Parse(request.FactorCorreccionId),
                    Creadopor = nuevo ? User.Identity.Name : request.Creadopor,
                    Fechacreacion = nuevo ? DateTime.Now : request.Fechacreacion.Value,
                    Modificadopor = !nuevo ? User.Identity.Name : null,
                    Fechamodificacion = !nuevo ? DateTime.Now : null,
                    OperationType = (int)request.OperationType,
                };

                ApiResponse<long> result = await _correctionFactorService.SaveCorrectionFactorSpecification(newRegister);
                    
                 if (result.Code == 1)
                 { 
                    result.Description = request.OperationType == (int)CorrectorFactorOperation.create ? "Registro exitoso" : "Actualización exitosa";

                    return Json(new
                    {
                        response = new ApiResponse<CorrectionFactorViewModel>
                        {
                            Code =1,
                            Description = result.Description,
                            Structure  = new CorrectionFactorViewModel {Fechacreacion = newRegister.Fechacreacion, Fechamodificacion = newRegister.Fechamodificacion,
                            Creadopor = newRegister.Creadopor, Modificadopor = newRegister.Modificadopor}
                        }
                    });
                }
                else
                {
                    return Json(new
                    {
                        response = new ApiResponse<CorrectionFactorViewModel>
                        {
                            Code = result.Code,
                            Description = result.Description,
                            Structure = new CorrectionFactorViewModel { }
                        }
                    });
                }

               
            }
            catch(Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<int>
                    {
                        Code = 1,
                        Description = e.Message,
                        Structure = 0
                    }
                });
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> SaveRegisterNozzleTypeMark([FromBody] CorrectionFactorViewModel request)
        {
            try
            {
                bool nuevo = request.OperationType == (int)CorrectorFactorOperation.create;
                var newRegister = new CorrectionFactorsXMarksXTypesDTO
                {
                    Temperatura = decimal.Parse(request.TemperatureId),
                    FactorCorr = decimal.Parse(request.FactorCorreccionId),
                    OperationType = (int)request.OperationType,
                    IdMarca = request.MarcaId,
                    IdTipo = request.TipoId,
                    Creadopor = nuevo ? User.Identity.Name : request.Creadopor,
                    Fechacreacion = nuevo ? DateTime.Now : request.Fechacreacion.Value,
                    Modificadopor = !nuevo ? User.Identity.Name : null,
                    Fechamodificacion = !nuevo ? DateTime.Now : null,
                };

                ApiResponse<long> result = await _correctionFactorService.SaveCorrectionFactorsXMarksXTypes(newRegister     );

                if (result.Code == 1)
                {
                    result.Description = request.OperationType == (int)CorrectorFactorOperation.create ? "Registro exitoso" : "Actualización exitosa";

                    return Json(new
                    {
                        response = new ApiResponse<CorrectionFactorViewModel>
                        {
                            Code = 1,
                            Description = result.Description,
                            Structure = new CorrectionFactorViewModel
                            {
                                Fechacreacion = newRegister.Fechacreacion,
                                Fechamodificacion = newRegister.Fechamodificacion,
                                Creadopor = newRegister.Creadopor,
                                Modificadopor = newRegister.Modificadopor
                            }
                        }
                    });
                }
                else
                {
                    return Json(new
                    {
                        response = new ApiResponse<CorrectionFactorViewModel>
                        {
                            Code = result.Code,
                            Description = result.Description,
                            Structure = new CorrectionFactorViewModel { }
                        }
                    });
                }

            }
            catch(Exception e) 
            {
                return Json(new { response = new ApiResponse<int> { Code =-1, Structure = 0 , Description = e.Message} });
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> GetTypesByMark(long IdMarca)
        {
            ApiResponse<List<TypeNozzleMarksDTO>> result = await _correctionFactorService.GetTypeXMarksNozzle(new TypeNozzleMarksDTO { IdMarca = IdMarca, Estatus = true });
            return result.Code == 1 ? Json(new { response = result }) : (IActionResult)Json(new { response = result });

        }

        [HttpPost]
        public async Task<IActionResult> DeleteRegister([FromBody] CorrectionFactorViewModel request)
        {
            ApiResponse<long> result = await _correctionFactorService.DeleteCorrectionFactorSpecification(new CorrectionFactorDTO { Temperatura = decimal.Parse(request.TemperatureId), Especificacion = request.EspecificacionId, FactorCorr = decimal.Parse(request.FactorCorreccionId) });

            return Json(new { response = result });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRegisterNozzleTypeMark([FromBody] CorrectionFactorViewModel request)
        {
            ApiResponse<long> result = await _correctionFactorService.DeleteRegisterNozzleTypeMark(new CorrectionFactorsXMarksXTypesDTO { Temperatura = decimal.Parse(request.TemperatureId), FactorCorr = decimal.Parse(request.FactorCorreccionId), IdTipo = request.TipoId, IdMarca = request.MarcaId });

            return Json(new { response = result });
        }
    }
}
