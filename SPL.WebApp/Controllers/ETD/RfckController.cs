namespace SPL.WebApp.Controllers.ETD
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs.ETD;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.ViewModels.ETD;

    public class RfckController : Controller
    {
        private readonly IETDClientService _etdClientService;
        private readonly IProfileSecurityService _profileClientService;

        public RfckController(
            IETDClientService etdClientService, IProfileSecurityService profileClientService)
        {
            _etdClientService = etdClientService;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;
            if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.FactordePotenciayCapacitanciadeBoquillas)))
            {
                ApiResponse<List<CorrectionFactorKWTypeCoolingDTO>> result = await _etdClientService.GetCorrectionFactorkWTypeCooling();
                return result.Code == -1
                    ? View(new RfckViewModel() { Error = result.Description, CorrectionFactors = new List<CorrectionFactorKWTypeCoolingDTO>() })
                    : (IActionResult)View(new RfckViewModel() { CorrectionFactors = result.Structure, Error = string.Empty });
            }
            else
            {
                return View("~/Views/PageConstruction/PermissionDenied.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] RfckViewModel viewModel)
        {
            if (viewModel is null)
                return Json(new { response = new { status = -1, description = "ViewModel NULL" } });
            try
            {
                viewModel.CorrectionFactors = viewModel.CorrectionFactors.FindAll(x => x.CoolingType is not "" && x.FactorCorr is not 0);

                ApiResponse<List<CorrectionFactorKWTypeCoolingDTO>> fkw = await _etdClientService.GetCorrectionFactorkWTypeCooling();

                if (fkw.Structure.Count > 0)
                {
                    foreach (CorrectionFactorKWTypeCoolingDTO item in viewModel.CorrectionFactors)
                    {
                        if (item.Fechacreacion == DateTime.MinValue)
                        {
                            item.Fechacreacion = DateTime.Now;
                            item.Creadopor = User.Identity.Name;
                        }
                        else if (fkw.Structure.Exists(x => x.CoolingType == item.CoolingType))
                        {
                            CorrectionFactorKWTypeCoolingDTO aux = fkw.Structure.FirstOrDefault(x => x.CoolingType == item.CoolingType);
                            if (aux.FactorCorr != item.FactorCorr)
                            {
                                item.Fechamodificacion = DateTime.Now;
                                item.Modificadopor = User.Identity.Name;
                            }
                        }
                    }
                }
                else
                {
                    foreach (CorrectionFactorKWTypeCoolingDTO item in viewModel.CorrectionFactors)
                    {
                        if (item.Fechacreacion == DateTime.MinValue)
                        {
                            item.Fechacreacion = DateTime.Now;
                            item.Creadopor = User.Identity.Name;
                        }
                    }
                }

                ApiResponse<long> result = await _etdClientService.SaveCorrectionFactorkWTypeCooling(viewModel.CorrectionFactors);
                return Json(new
                {
                    response = new { status = result.Code, description = result.Description }
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    response = new { status = -1, description = ex.Message }
                });
            }
        }
    }
}
