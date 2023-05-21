namespace SPL.WebApp.Controllers.ETD
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ETD;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.ViewModels.ETD;

    public class RddeController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IETDClientService _etdClientService;

        public RddeController(
            IMasterHttpClientService masterHttpClientService,
            IArtifactClientService artifactClientService,
            ISidcoClientService sidcoClientService,
            IETDClientService etdClientService)
        {
            _masterHttpClientService = masterHttpClientService;
            _artifactClientService = artifactClientService;
            _sidcoClientService = sidcoClientService;
            _etdClientService = etdClientService;
        }
        public async Task<IActionResult> Index() => View(new RddeViewModel());

        public async Task<IActionResult> GetData(string noSerie)
        {

            if (string.IsNullOrEmpty(noSerie))
            {
                return Json(new
                {
                    response = new ApiResponse<RddeViewModel>
                    {
                        Code = -1,
                        Description = "No. Serie inválido.",
                        Structure = null
                    }
                });
            }

            bool isFromSPL = await _artifactClientService.CheckNumberOrder(noSerie);

            if (!isFromSPL)
            {
                return Json(new
                {
                    response = new ApiResponse<RddeViewModel>
                    {
                        Code = -1,
                        Description = "Artefacto no presente en SPL",
                        Structure = null
                    }
                });
            }
            else
            {
                InformationArtifactDTO artifactDesing = await _artifactClientService.GetArtifact(noSerie);
                if (artifactDesing.GeneralArtifact.OrderCode == null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<RddeViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no encontrado",
                            Structure = null
                        }
                    });
                }
                //DG
                if (artifactDesing.GeneralArtifact is null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<RddeViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee información general",
                            Structure = null
                        }
                    });
                }

                // CAR
                if (artifactDesing.VoltageKV is null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<RddeViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee voltages",
                            Structure = null
                        }
                    });
                }

                ApiResponse<StabilizationDesignDataDTO> result = await _etdClientService.GetStabilizationDesignData(noSerie);
                RddeViewModel rddeViewModel = result is null
                    ? (new()
                    {
                        AT = artifactDesing.VoltageKV.TensionKvAltaTension1 is not null and not 0 || artifactDesing.VoltageKV.TensionKvAltaTension3 is not null and not 0,
                        BT = artifactDesing.VoltageKV.TensionKvBajaTension1 is not null and not 0 || artifactDesing.VoltageKV.TensionKvBajaTension3 is not null and not 0,
                        TER = artifactDesing.VoltageKV.TensionKvTerciario1 is not null and not 0 || artifactDesing.VoltageKV.TensionKvTerciario3 is not null and not 0,
                        NoSerie = noSerie,
                        TOILimite = 3,
                        Id = 0
                    })
                    : result.Structure is null
                        ? (new()
                        {
                            AT = artifactDesing.VoltageKV.TensionKvAltaTension1 is not null and not 0 || artifactDesing.VoltageKV.TensionKvAltaTension3 is not null and not 0,
                            BT = artifactDesing.VoltageKV.TensionKvBajaTension1 is not null and not 0 || artifactDesing.VoltageKV.TensionKvBajaTension3 is not null and not 0,
                            TER = artifactDesing.VoltageKV.TensionKvTerciario1 is not null and not 0 || artifactDesing.VoltageKV.TensionKvTerciario3 is not null and not 0,
                            NoSerie = noSerie,
                            TOILimite = 3,
                            Id = 0
                        })
                        : (new()
                        {
                            TORLimite = Convert.ToInt32(result.Structure.TorLimite),
                            TORHoja = result.Structure.TorHenf,
                            AORLimite = Convert.ToInt32(result.Structure.AorLimite),
                            AORHoja = result.Structure.AorHenf,
                            GATLimite = result.Structure.GradienteLimAt ?? 0,
                            GATHoja = result.Structure.GradienteHentAt ?? 0,
                            GBTLimite = result.Structure.GradienteLimBt ?? 0,
                            GBTHoja = result.Structure.GradienteHentBt ?? 0,
                            GTERLimite = result.Structure.GradienteLimTer ?? 0,
                            GTERHoja = result.Structure.GradienteHentTer ?? 0,
                            AATLimite = Convert.ToInt32(result.Structure.AwrLimAt ?? 0),
                            AATHoja = result.Structure.AwrHenfAt ?? 0,
                            ABTLimite = Convert.ToInt32(result.Structure.AwrLimBt ?? 0),
                            ABTHoja = Convert.ToInt32(result.Structure.AwrHenfBt ?? 0),
                            ATERLimite = Convert.ToInt32(result.Structure.AwrLimTer ?? 0),
                            ATERHoja = Convert.ToInt32(result.Structure.AwrHenfTer ?? 0),
                            HATLimite = Convert.ToInt32(result.Structure.HsrLimAt ?? 0),
                            HATHoja = result.Structure.HsrHenfAt ?? 0,
                            HBTLimite = Convert.ToInt32(result.Structure.HsrLimBt ?? 0),
                            HBTHoja = result.Structure.HsrHenfBt ?? 0,
                            HTERLimite = Convert.ToInt32(result.Structure.HsrLimTer ?? 0),
                            HTERHoja = result.Structure.HsrHenfTer ?? 0,
                            CteTiempo = result.Structure.CteTiempoTrafo,
                            CteAmbiente = Convert.ToInt32(result.Structure.AmbienteCte),
                            BOR = result.Structure.Bor,
                            KWDiseno = result.Structure.KwDiseno,
                            TOI = Convert.ToInt32(result.Structure.Toi),
                            TOILimite = Convert.ToInt32(result.Structure.ToiLimite),
                            AT = artifactDesing.VoltageKV.TensionKvAltaTension1 is not null and not 0 || artifactDesing.VoltageKV.TensionKvAltaTension3 is not null and not 0,
                            BT = artifactDesing.VoltageKV.TensionKvBajaTension1 is not null and not 0 || artifactDesing.VoltageKV.TensionKvBajaTension3 is not null and not 0,
                            TER = artifactDesing.VoltageKV.TensionKvTerciario1 is not null and not 0 || artifactDesing.VoltageKV.TensionKvTerciario3 is not null and not 0,
                            NoSerie = noSerie,
                            Id = result.Structure.Id
                        });

                return Json(new
                {
                    response = new ApiResponse<RddeViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = rddeViewModel
                    }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] RddeViewModel viewModel)
        {
            if (viewModel == null)
                return Json(new { response = new { status = -1, description = "ViewModel NULL" } });

            ApiResponse<StabilizationDesignDataDTO> data = await _etdClientService.GetStabilizationDesignData(viewModel.NoSerie);

            StabilizationDesignDataDTO send = new()
            {
                Id = viewModel.Id,
                AmbienteCte = viewModel.CteAmbiente,
                AorHenf = viewModel.AORHoja,
                AorLimite = viewModel.AORLimite,
                AwrHenfAt = viewModel.AATHoja,
                AwrHenfBt = viewModel.ABTHoja,
                AwrHenfTer = viewModel.ATERHoja,
                AwrLimAt = viewModel.AATLimite,
                AwrLimBt = viewModel.ABTLimite,
                AwrLimTer = viewModel.ATERLimite,
                Bor = viewModel.BOR,
                CteTiempoTrafo = viewModel.CteTiempo,
                GradienteHentAt = viewModel.GATHoja,
                GradienteHentBt = viewModel.GBTHoja,
                GradienteHentTer = viewModel.GTERHoja,
                GradienteLimAt = viewModel.GATLimite,
                GradienteLimBt = viewModel.GBTLimite,
                GradienteLimTer = viewModel.GTERLimite,
                HsrHenfAt = viewModel.HATHoja,
                HsrHenfBt = viewModel.HBTHoja,
                HsrHenfTer = viewModel.HTERHoja,
                HsrLimAt = viewModel.HATLimite,
                HsrLimBt = viewModel.HBTLimite,
                HsrLimTer = viewModel.HTERLimite,
                KwDiseno = viewModel.KWDiseno,
                NoSerie = viewModel.NoSerie,
                Toi = viewModel.TOI,
                ToiLimite = viewModel.TOILimite,
                TorHenf = viewModel.TORHoja,
                TorLimite = viewModel.TORLimite,

            };

            if (data.Structure != null)
            {
                send.Fechacreacion = data.Structure.Fechacreacion;
                send.Creadopor = data.Structure.Creadopor;
                send.Fechamodificacion = DateTime.Now;
                send.Modificadopor = User.Identity.Name;
            }
            else
            {
                send.Fechacreacion = DateTime.Now;
                send.Creadopor = User.Identity.Name;
                send.Fechamodificacion = null;
                send.Modificadopor = null;

            }

            try
            {
                ApiResponse<long> result = await _etdClientService.SaveStabilizationDesignData(send);

                if (result.Code == -1)
                {
                    return Json(new { response = new { status = -1, description = result.Description } });
                }

                var resultResponse = new { status = result.Code, description = result.Description };

                return Json(new
                {
                    response = resultResponse
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
