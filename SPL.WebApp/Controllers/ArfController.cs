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

    using Spire.Pdf;

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
    public class ArfController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IArfClientService _arfClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IArfService _arfService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IRodClientService _rodClientService;
        private readonly IPceClientService _pceService;
        private readonly IProfileSecurityService _profileClientService;

        public ArfController(
            IMasterHttpClientService masterHttpClientService,
            IArfClientService arfClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IArfService arfService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            IRodClientService rodClientService,
            IPceClientService pceService,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _arfClientService = arfClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _arfService = arfService;
            _correctionFactorService = correctionFactorService;
            _rodClientService = rodClientService;
            _hostEnvironment = hostEnvironment;
            _pceService = pceService;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.AnálisisdelaRespuestaalBarridodelaFrecuencia)))
                {
                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new ArfViewModel { NoSerie = noSerie });
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
            try
            {
                ArfViewModel pciViewModel = new();
                noSerie = noSerie.ToUpper().Trim();
                string noSerieSimple = string.Empty;

                ApiResponse<PositionsDTO> dataSelect = await _gatewayClientService.GetPositions(noSerie);

                pciViewModel.Positions = dataSelect.Structure;

                if (!string.IsNullOrEmpty(noSerie))
                {
                    noSerieSimple = noSerie.Split("-")[0];
                }
                else
                {
                    return Json(new
                    {
                        response = new ApiResponse<ArfViewModel>
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
                        response = new ApiResponse<ArfViewModel>
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
                            response = new ApiResponse<ArfViewModel>
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
                            response = new ApiResponse<ArfViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee características",
                                Structure = null
                            }
                        });
                    }
                    else
                    {
                        pciViewModel.CharacteristicsArtifact = artifactDesing.CharacteristicsArtifact;
                        List<decimal?> mavaf3 = artifactDesing.CharacteristicsArtifact.Select(x => x.Mvaf3).ToList();
                        List<decimal?> mavaf4 = artifactDesing.CharacteristicsArtifact.Select(x => x.Mvaf4).ToList();

                        List<GeneralPropertiesDTO> selectTer = new();
                        if (artifactDesing.CharacteristicsArtifact.Where(x => x.Mvaf3 > 0).Count() > 0)
                        {
                            selectTer.Add(new GeneralPropertiesDTO() { Clave = "2B", Descripcion = "2da. Baja" });
                        }

                        if (artifactDesing.CharacteristicsArtifact.Where(x => x.Mvaf4 > 0).Count() > 0)
                        {
                            selectTer.Add(new GeneralPropertiesDTO() { Clave = "CT", Descripcion = "Con Terciario" });
                        }

                        if (artifactDesing.CharacteristicsArtifact.Where(x => x.Mvaf3 > 0).Count() == 0 && artifactDesing.CharacteristicsArtifact.Where(x => x.Mvaf4 > 0).Count() == 0)
                        {
                            selectTer.Add(new GeneralPropertiesDTO() { Clave = "ST", Descripcion = "Sin Terciario" });
                        }

                        /*List<GeneralPropertiesDTO> selectTer = new()
                            new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." },
                        };*/

                        pciViewModel.Terciarios = new(selectTer, "Clave", "Descripcion");
                    }

                    pciViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
                    VoltageKVDTO voltage = artifactDesing.VoltageKV;
                    //List<GeneralPropertiesDTO> selectV = new()
                    //{
                    //    new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}" },
                    //    new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}" }
                    //};






                    List<GeneralPropertiesDTO> selectV = new List<GeneralPropertiesDTO>();

                    selectV.Add(new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." });

                    if (voltage.TensionKvAltaTension1 > 0 && voltage.TensionKvBajaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}" });

                    }

                    if (voltage.TensionKvAltaTension1 > 0 && voltage.TensionKvBajaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}" });
                    }

                    if (voltage.TensionKvAltaTension1 > 0 && voltage.TensionKvSegundaBaja1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}" });
                    }

                    if (voltage.TensionKvAltaTension1 > 0 && voltage.TensionKvTerciario1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}" });

                    }

                    if (voltage.TensionKvBajaTension1 > 0 && voltage.TensionKvTerciario1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}" });
                    }

                    if (voltage.TensionKvSegundaBaja1 > 0 && voltage.TensionKvTerciario1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvTerciario1 ?? -778899}" });
                    }

                    if (voltage.TensionKvAltaTension3 > 0 && voltage.TensionKvBajaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}" });
                    }

                    if (voltage.TensionKvAltaTension3 > 0 && voltage.TensionKvBajaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}" });
                    }

                    if (voltage.TensionKvAltaTension3 > 0 && voltage.TensionKvSegundaBaja3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}" });
                    }

                    if (voltage.TensionKvAltaTension3 > 0 && voltage.TensionKvTerciario3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}", Descripcion = $"{voltage.TensionKvAltaTension3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}" });
                    }

                    if (voltage.TensionKvBajaTension3 > 0 && voltage.TensionKvTerciario3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}" });

                    }

                    if (voltage.TensionKvSegundaBaja3 > 0 && voltage.TensionKvTerciario3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvTerciario3 ?? -778899}" });
                    }

                    if (voltage.TensionKvBajaTension1 > 0 && voltage.TensionKvAltaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}" });
                    }

                    if (voltage.TensionKvBajaTension1 > 0 && voltage.TensionKvAltaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension1 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}" });
                    }

                    if (voltage.TensionKvSegundaBaja1 > 0 && voltage.TensionKvAltaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}" });
                    }

                    if (voltage.TensionKvTerciario1 > 0 && voltage.TensionKvAltaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}" });
                    }

                    if (voltage.TensionKvTerciario1 > 0 && voltage.TensionKvBajaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvBajaTension1 ?? -778899}" });
                    }

                    if (voltage.TensionKvTerciario1 > 0 && voltage.TensionKvSegundaBaja1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario1 ?? -778899} - {voltage.TensionKvSegundaBaja1 ?? -778899}" });
                    }

                    if (voltage.TensionKvBajaTension3 > 0 && voltage.TensionKvAltaTension1 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvAltaTension1 ?? -778899}" });
                    }

                    if (voltage.TensionKvBajaTension3 > 0 && voltage.TensionKvAltaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvBajaTension3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}" });
                    }

                    if (voltage.TensionKvSegundaBaja3 > 0 && voltage.TensionKvAltaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvSegundaBaja3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}" });
                    }

                    if (voltage.TensionKvTerciario3 > 0 && voltage.TensionKvAltaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvAltaTension3 ?? -778899}" });
                    }

                    if (voltage.TensionKvTerciario3 > 0 && voltage.TensionKvBajaTension3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvBajaTension3 ?? -778899}" });
                    }

                    if (voltage.TensionKvTerciario3 > 0 && voltage.TensionKvSegundaBaja3 > 0)
                    {
                        selectV.Add(new GeneralPropertiesDTO() { Clave = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}", Descripcion = $"{voltage.TensionKvTerciario3 ?? -778899} - {voltage.TensionKvSegundaBaja3 ?? -778899}" });
                    }




                    selectV = selectV.FindAll(x => !x.Clave.Contains("-778899"));
                    pciViewModel.VoltageLevels = new(selectV, "Clave", "Descripcion");

                }

                ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _arfClientService.GetFilter(noSerie);

                if (resultFilter.Code.Equals(1))
                {
                    InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.ARF.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.ARF.ToString() };
                    IEnumerable<IGrouping<bool?, InfoGeneralReportsDTO>> reportRdtGroupStatus = reportRdt.ListDetails.GroupBy(c => c.Resultado);

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

                    pciViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                    {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Tensión Inducida con Medición de Descargas Parciales",
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
                    response = new ApiResponse<ArfViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = pciViewModel
                    }
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    response = new ApiResponse<TdpViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = null
                    }
                });
            }
        }





        [HttpGet]
        public async Task<IActionResult> GetTemplate(string nroSerie, string keyTest, string lenguage, string team, string tertiary2Low, string tertiaryDisp, string levelsVoltage, string comments,
            string nivelAceiteLab, string nivelAceitePla, string boquillasLab, string boquillasPla, string nucleoLab, string nucleoPla, string terLab, string terPla)
        {
            ApiResponse<PositionsDTO> resultP = await _gatewayClientService.GetPositions(nroSerie);

            if (resultP.Code.Equals(-1))
            {
                return View("Excel", new ArfViewModel
                {
                    Error = resultP.Description
                });
            }
            else
            {
                if (string.IsNullOrEmpty(resultP.Structure.ATNom) || string.IsNullOrEmpty(resultP.Structure.BTNom))
                {
                    return View("Excel", new ArfViewModel
                    {
                        Error = "Debe registrar posiciones en tensión de placa para el aparato " + nroSerie
                    });
                }
            }

            ApiResponse<SettingsToDisplayARFReportsDTO> result = await _gatewayClientService.GetTemplateARF(nroSerie, keyTest, lenguage, team, tertiary2Low, tertiaryDisp, levelsVoltage);

            if (result.Code.Equals(-1))
            {
                return View("Excel", new ArfViewModel
                {
                    Error = result.Description
                });
            }

            if (result.Structure.BaseTemplate == null)
            {
                return View("Excel", new ArfViewModel
                {
                    Error = "No se ha encontrado plantilla para reporte ARF"
                });
            }

            if (string.IsNullOrEmpty(result.Structure.BaseTemplate.Plantilla))
            {
                return View("Excel", new ArfViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = result.Structure.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(result.Structure.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");
            SettingsToDisplayARFReportsDTO settings = result.Structure;

            _arfService.PrepareTemplate(ref settings, ref workbook, keyTest, lenguage, levelsVoltage, team, (int)result.Structure.BaseTemplate.ColumnasConfigurables, tertiary2Low,
                 ref nivelAceiteLab, ref nivelAceitePla, ref boquillasLab, ref boquillasPla, ref nucleoLab, ref nucleoPla, ref terLab, ref terPla);

            return View("Excel", new ArfViewModel
            {
                ClaveIdioma = lenguage,
                Pruebas = keyTest,
                NoPrueba = NoPrueba.ToString(),
                NoSerie = nroSerie,
                Workbook = workbook,
                Error = string.Empty,
                Comments = comments,
                Positions = resultP.Structure,
                VoltageLevel = levelsVoltage,
                BoquillasLab = boquillasLab,
                BoquillasEmp = boquillasPla,
                NucleosEmp = nucleoPla,
                NucleosLab = nucleoLab,
                NivelAceiteEmp = nivelAceitePla,
                NivelAceiteLab = nivelAceiteLab,
                TerciarioLab = terLab,
                TerciarioEmp = terPla,
                TerciarioOSegunda = tertiary2Low,
                SettingsARF = settings,
                Columnas = (int)settings.BaseTemplate.ColumnasConfigurables,
                Equipo = team,
                TerciarioDisponible = tertiaryDisp

            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] TdpViewModel viewModel)
        {
            try
            {
                int[] _positionWB;
                string error = _arfService.ValidateTemplateTDP(viewModel.SettingsTDP, viewModel.Workbook);
                if (error != string.Empty)
                {
                    return Json(new
                    {
                        response = new ApiResponse<TdpViewModel>
                        {
                            Code = -1,
                            Description = error
                        }
                    });
                }

                List<TDPTerminalsDTO> terminales = new();
                TDPTerminalsDTO terminal = new();

                List<TDPTestsDetailsDTO> details = new();
                TDPTestsDetailsDTO detail = new();

                for (int i = 0; i < viewModel.SettingsTDP.Times.Count; i++)
                {
                    if (viewModel.SettingsTDP.BaseTemplate.ColumnasConfigurables == 3)
                    {
                        detail = new TDPTestsDetailsDTO();
                        terminal = new TDPTerminalsDTO();
                        terminales = new List<TDPTerminalsDTO>();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal1")).Celda);
                        string valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsTDP.TitTerminal1;
                        terminal.pC = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new TDPTerminalsDTO();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal2")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsTDP.TitTerminal2;
                        terminal.pC = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new TDPTerminalsDTO();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal3")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsTDP.TitTerminal3;
                        terminal.pC = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new TDPTerminalsDTO();

                        detail.Time = viewModel.SettingsTDP.Times[i];
                        detail.Voltage = decimal.Parse(viewModel.SettingsTDP.Voltages[i]);
                        detail.TDPTerminals = terminales;

                        details.Add(detail);
                    }
                    else
                    {
                        detail = new TDPTestsDetailsDTO();
                        terminal = new TDPTerminalsDTO();
                        terminales = new List<TDPTerminalsDTO>();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal1")).Celda);
                        string valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsTDP.TitTerminal1;
                        terminal.pC = int.Parse(valor);

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal2")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();
                        terminal.µV = int.Parse(valor);

                        terminales.Add(terminal);
                        terminal = new TDPTerminalsDTO();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal3")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsTDP.TitTerminal2;
                        terminal.pC = int.Parse(valor);

                        detail.Time = viewModel.SettingsTDP.Times[i];
                        detail.Voltage = decimal.Parse(viewModel.SettingsTDP.Voltages[i]);
                        detail.TDPTerminals = terminales;

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal4")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.µV = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new TDPTerminalsDTO();

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal5")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.Terminal = viewModel.SettingsTDP.TitTerminal3;
                        terminal.pC = int.Parse(valor);

                        _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Terminal6")).Celda);
                        valor = viewModel.Workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]]?.Value?.ToString();

                        terminal.µV = int.Parse(valor);
                        terminales.Add(terminal);
                        terminal = new TDPTerminalsDTO();

                        detail.Time = viewModel.SettingsTDP.Times[i];
                        detail.Voltage = decimal.Parse(viewModel.SettingsTDP.Voltages[i]);
                        detail.TDPTerminals = terminales;

                        details.Add(detail);
                    }
                }

                TDPCalibrationMeasurementDTO calibraciones = new();

                if (viewModel.SettingsTDP.BaseTemplate.ColumnasConfigurables == 3)
                {
                    string valor1;
                    string valor2;
                    string valor3;

                    _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelCalibracion")).Celda);
                    valor1 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();
                    valor2 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1]?.Value?.ToString();
                    valor3 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2]?.Value?.ToString();

                    calibraciones.Calibration1 = int.Parse(valor1);
                    calibraciones.Calibration2 = int.Parse(valor2);
                    calibraciones.Calibration3 = int.Parse(valor3);

                    _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelMedido")).Celda);
                    valor1 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();
                    valor2 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1]?.Value?.ToString();
                    valor3 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2]?.Value?.ToString();

                    calibraciones.Measured1 = int.Parse(valor1);
                    calibraciones.Measured2 = int.Parse(valor2);
                    calibraciones.Measured3 = int.Parse(valor3);

                    calibraciones.Grades = viewModel.Notes?.Trim();

                }
                else
                {
                    string valor1;
                    string valor2;
                    string valor3;
                    string valor4;
                    string valor5;
                    string valor6;

                    _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelCalibracion")).Celda);
                    valor1 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();
                    valor2 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1]?.Value?.ToString();
                    valor3 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2]?.Value?.ToString();
                    valor4 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3]?.Value?.ToString();
                    valor5 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4]?.Value?.ToString();
                    valor6 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5]?.Value?.ToString();

                    calibraciones.Calibration1 = int.Parse(valor1);
                    calibraciones.Calibration2 = int.Parse(valor2);
                    calibraciones.Calibration3 = int.Parse(valor3);
                    calibraciones.Calibration4 = int.Parse(valor4);
                    calibraciones.Calibration5 = int.Parse(valor5);
                    calibraciones.Calibration6 = int.Parse(valor6);

                    _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelMedido")).Celda);
                    valor1 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();
                    valor2 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 1]?.Value?.ToString();
                    valor3 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 2]?.Value?.ToString();
                    valor4 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 3]?.Value?.ToString();
                    valor5 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 4]?.Value?.ToString();
                    valor6 = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1] + 5]?.Value?.ToString();

                    calibraciones.Measured1 = int.Parse(valor1);
                    calibraciones.Measured2 = int.Parse(valor2);
                    calibraciones.Measured3 = int.Parse(valor3);
                    calibraciones.Measured4 = int.Parse(valor4);
                    calibraciones.Measured5 = int.Parse(valor5);
                    calibraciones.Measured6 = int.Parse(valor6);

                    calibraciones.Grades = viewModel.Notes?.Trim();
                }

                TDPTestsDTO tDPTestsDTO = new()
                {
                    TDPTestsDetails = details,
                    TDPTestsDetailsCalibration = calibraciones
                };

                TDPTestsGeneralDTO test = new()
                {
                    Interval = int.Parse(viewModel.TiempoIntervalo),
                    TotalTime = int.Parse(viewModel.TiempoTotal),
                    DescMayMv = int.Parse(viewModel.DescargaUV),
                    DescMayPc = int.Parse(viewModel.DescargaPC),
                    IncMaxPc = int.Parse(viewModel.IncrementoMaxPC),
                    Tolerance = 3,
                    TDPTest = tDPTestsDTO
                };

                ApiResponse<ResultTDPTestsDTO> calculateResult = await _arfClientService.CalculateTestTdp(test);
                List<string> errores = new();
                if (calculateResult.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<TdpViewModel>
                        {
                            Code = -1,
                            Description = "Error en validaciones TDP " + calculateResult.Description
                        }
                    });
                }

                Workbook workbook = viewModel.Workbook;

                viewModel.Workbook = workbook;

                errores.AddRange(calculateResult.Structure.Results.Select(k => k.Message).ToList());
                string allErrosPrepare = errores.Any() ? errores.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
                bool resultReport = !calculateResult.Structure.Results.Any();

                viewModel.IsReportAproved = resultReport;
                viewModel.Workbook = workbook;
                viewModel.OfficialWorkbook = workbook;
                viewModel.TestTDP = test;

                _positionWB = GetRowColOfWorbook(viewModel.SettingsTDP.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = viewModel.ClaveIdioma.ToUpper() == "ES"
                    ? resultReport ? "Aceptado" : (object)"Rechazado"
                    : resultReport ? "Accepted" : (object)"Rejected";

                return Json(new
                {
                    response = new ApiResponse<TdpViewModel>
                    {
                        Code = 1,
                        Description = allErrosPrepare,
                        Structure = viewModel
                    }
                });

            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] ArfViewModel viewModel)
        {
            try
            {
                viewModel.Workbook.Sheets[0].Rows.ForEach(c => c.Cells?.ForEach(k => k.Validation = null));
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

                List<FilesDTO> archivos = new();
                List<Stream> filesStreams = new()
                {
                    pdfStream
                };
                foreach (ArchivosFrontViewModel item in viewModel.Archivos)
                {
                    archivos.Add(new FilesDTO
                    {
                        File = Convert.FromBase64String(item.Base64.Split(',')[1]),
                        Name = item.Name
                    });

                    filesStreams.Add(new MemoryStream(Convert.FromBase64String(item.Base64.Split(',')[1])));
                }

                //PRIMER ARCHIVO BASE64 CARGADO DESDE EL MODAL
                PdfDocumentBase resultaqui = PdfDocument.MergeFiles(filesStreams.ToArray());
                resultaqui.Save(pdfStream, Spire.Pdf.FileFormat.PDF);
                pdfFile = pdfStream.ToArray();

                viewModel.Base64PDF = Convert.ToBase64String(pdfFile);

                DateTime basedate = new(1899, 12, 30);
                int[] _positionWB = GetRowColOfWorbook(viewModel.SettingsARF.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
                string fecha = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

                fecha = fecha.Contains("/") ? (DateTime.Now.Date - basedate.Date).TotalDays.ToString() : fecha;
                viewModel.Date = fecha;
                decimal TempAceite1 = 0;
                decimal TempAceite2 = 0;

                List<ARFTestsDTO> secciones = new();
                if (viewModel.Pruebas == "LYP")
                {
                    ARFTestsDTO seccion1 = new()
                    {
                        PosAt = viewModel.AT1,
                        PosBt = viewModel.BT1,
                        PosTer = viewModel.Ter1,
                        LevelOil = viewModel.NivelAceiteLab,
                        CoreHardware = viewModel.NucleosLab
                    };
                    _ = decimal.TryParse(viewModel.TempAceite1, out TempAceite1);
                    seccion1.TempOil = decimal.Parse(viewModel.TempAceite1);
                    seccion1.Tertiary = viewModel.Ter1;
                    seccion1.Nozzles = viewModel.BoquillasLab;

                    ARFTestsDTO seccion2 = new()
                    {
                        PosAt = viewModel.AT2,
                        PosBt = viewModel.BT2,
                        PosTer = viewModel.Ter2,
                        LevelOil = viewModel.NivelAceiteEmp,
                        CoreHardware = viewModel.NucleosEmp
                    };
                    _ = decimal.TryParse(viewModel.TempAceite2, out TempAceite2);
                    seccion2.TempOil = TempAceite2;
                    seccion2.Tertiary = viewModel.Ter2;
                    seccion2.Nozzles = viewModel.BoquillasEmp;

                    secciones.Add(seccion1);
                    secciones.Add(seccion2);
                }
                else
                {
                    if (viewModel.Pruebas == "LAB")
                    {
                        ARFTestsDTO seccion1 = new()
                        {
                            PosAt = viewModel.AT1,
                            PosBt = viewModel.BT1,
                            PosTer = viewModel.Ter1,
                            LevelOil = viewModel.NivelAceiteLab,
                            CoreHardware = viewModel.NucleosLab
                        };
                        _ = decimal.TryParse(viewModel.TempAceite1, out TempAceite1);
                        seccion1.TempOil = TempAceite1;
                        seccion1.Tertiary = viewModel.Ter1;
                        seccion1.Nozzles = viewModel.BoquillasLab;
                        secciones.Add(seccion1);
                    }
                    else
                    {
                        ARFTestsDTO seccion1 = new()
                        {
                            PosAt = viewModel.AT2,
                            PosBt = viewModel.BT2,
                            PosTer = viewModel.Ter2,
                            LevelOil = viewModel.NivelAceiteEmp,
                            CoreHardware = viewModel.NucleosEmp
                        };
                        _ = decimal.TryParse(viewModel.TempAceite2, out TempAceite2);
                        seccion1.TempOil = TempAceite2;
                        seccion1.Tertiary = viewModel.Ter2;
                        seccion1.Nozzles = viewModel.BoquillasEmp;
                        secciones.Add(seccion1);
                    }
                }

                // return null;

                ARFTestsGeneralDTO pirGeneralTest = new()
                {
                    Capacity = viewModel.SettingsARF.HeadboardReport.Capacity,
                    Comment = viewModel.Comments,

                    Customer = viewModel.SettingsARF.HeadboardReport.Client,
                    Creadopor = User.Identity.Name,
                    Fechacreacion = DateTime.Now,
                    File = pdfFile,
                    IdLoad = 0,
                    KeyTest = viewModel.Pruebas,
                    LanguageKey = viewModel.ClaveIdioma,
                    Date = basedate.AddDays(int.Parse(viewModel.Date)),
                    LoadDate = basedate.AddDays(int.Parse(viewModel.Date)),
                    Modificadopor = null,
                    NameFile = string.Concat("ARF", viewModel.Pruebas, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                    Result = viewModel.Resultado is "Aceptado" or "Accepted",
                    SerialNumber = viewModel.NoSerie,
                    TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                    TypeReport = ReportType.ARF.ToString(),
                    Files = archivos,
                    ARFTests = secciones,
                    Team = viewModel.Equipo,
                    Tertiary_2Low = viewModel.TerciarioOSegunda,
                    TotalPags = int.Parse(viewModel.Paginas),
                    TertiaryDisp = viewModel.TerciarioDisponible,
                    LevelsVoltage = viewModel.VoltageLevel,

                };

                try
                {
                    ApiResponse<long> result = await _arfClientService.SaveReport(pirGeneralTest);

                    var resultResponse = new { status = result.Code, description = result.Description, nameFile = pirGeneralTest.NameFile, file = viewModel.Base64PDF };

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
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IActionResult> GetPDFReport(long code, string typeReport, string reportName)
        {
            _ = reportName.Split('-').LastOrDefault().Split("_")[0];
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

        [HttpGet]
        public async Task<IActionResult> GetConfigurationFiles(int pIdModule)
        {
            ApiResponse<IEnumerable<FileWeightDTO>> getConfigModulResponse = await _masterHttpClientService.GetConfigurationFiles(pIdModule);
            object resultado = new();
            if (getConfigModulResponse.Code == 1)
            {
                resultado = getConfigModulResponse.Structure.Select(x => new
                {
                    PesoMaximo = int.Parse(x.MaximoPeso),
                    x.ExtensionArchivoNavigation.Extension
                }).ToList();

                return Json(new
                {
                    response = new ApiResponse<object>
                    {
                        Code = 1,
                        Description = "",
                        Structure = resultado
                    }
                });
            }
            else
            {

                return Json(new
                {
                    response = new ApiResponse<object>
                    {
                        Code = getConfigModulResponse.Code,
                        Description = getConfigModulResponse.Description,
                        Structure = null
                    }
                });
            }
        }

        public IActionResult Error() => View();

        #region PrivateMethods
        public async Task PrepareForm()
        {

            /// var a = await this._gatewayClientService.GetPositions("G4135-01");

            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> terciarioSegunda = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." },
                new GeneralPropertiesDTO { Clave = "ST", Descripcion = "Sin Terciario" } ,
                new GeneralPropertiesDTO { Clave = "CT", Descripcion = "Con Terciario" } ,
                new GeneralPropertiesDTO { Clave = "2B", Descripcion = "2da. Baja" } ,
            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> equipoItems = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." },
                new GeneralPropertiesDTO { Clave = "M5100", Descripcion = "M5100" } ,
                new GeneralPropertiesDTO { Clave = "M5200", Descripcion = "M5200" } ,
                new GeneralPropertiesDTO { Clave = "M5400", Descripcion = "M5400" } ,
            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> terciarioDisponible = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "Si", Descripcion = "Si" },
                new GeneralPropertiesDTO { Clave = "No", Descripcion = "No" } ,

            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> nivelAceite = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "Lleno", Descripcion = "Lleno" },
                new GeneralPropertiesDTO { Clave = "Vacío", Descripcion = "Vacío" } ,

            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> boquillas = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "Operación", Descripcion = "Operación" },
                new GeneralPropertiesDTO { Clave = "Tipo", Descripcion = "Tipo Pozo" } ,

            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> nucleos = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "Aterrizado", Descripcion = "Aterrizado" },
                new GeneralPropertiesDTO { Clave = "Flotado", Descripcion = "Flotado" } ,

            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> terciario = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "Aterrizado", Descripcion = "Aterrizado" },
                new GeneralPropertiesDTO { Clave = "Flotado", Descripcion = "Flotado" } ,

            }.AsEnumerable();

            // Tipos de prueba
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.ARF.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");
            ViewBag.TerciarioSegunda = new SelectList(terciarioSegunda, "Clave", "Descripcion");
            ViewBag.EquipoItems = new SelectList(equipoItems, "Clave", "Descripcion");
            ViewBag.NivelTension = new SelectList(origingeneralProperties, "Clave", "Descripcion");
            ViewBag.TerciarioDisponible = new SelectList(terciarioDisponible, "Clave", "Descripcion");

            ViewBag.NivelAceite = new SelectList(nivelAceite, "Clave", "Descripcion");
            ViewBag.Boquillas = new SelectList(boquillas, "Clave", "Descripcion");
            ViewBag.Nucleos = new SelectList(nucleos, "Clave", "Descripcion");
            ViewBag.Terciario = new SelectList(terciario, "Clave", "Descripcion");
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

