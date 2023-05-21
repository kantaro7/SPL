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

    using Newtonsoft.Json;

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
    public class IszController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IIszClientService _iszClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IIszService _iszService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProfileSecurityService _profileClientService;
        public IszController(
            IMasterHttpClientService masterHttpClientService,
            IIszClientService iszClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IIszService iszService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            IRodClientService rodClientService,
            IPceClientService pceService,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _iszClientService = iszClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _iszService = iszService;
            _correctionFactorService = correctionFactorService;
            _hostEnvironment = hostEnvironment;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                /*string texto2 = System.IO.File.ReadAllText(@"C:\Users\Barboza\Desktop\calculeTestIsZ.json");
           var testOut2 = JsonConvert.DeserializeObject<OutISZTestsDTO>(texto2);

           ApiResponse<ResultISZTestsDTO> calculateResult2 = await _iszClientService.CalculateTestIsz(testOut2);*/
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.ImpedanciadeSecuenciaCero)))
                {
                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new IszViewModel { NoSerie = noSerie });
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
                IszViewModel iszViewModel = new();
                noSerie = noSerie.ToUpper().Trim();
                string noSerieSimple = string.Empty;

                ApiResponse<PositionsDTO> dataSelect = await _gatewayClientService.GetPositions(noSerie);

                ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.ISZ.ToString(), "-1");
                //iszViewModel.ListaPruebas = reportResult.Structure.ToList();

                if (dataSelect.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<IszViewModel>
                        {
                            Code = -1,
                            Description = "Error al obtener las posiciones para el numero de Serie " + noSerie,
                            Structure = null
                        }
                    });
                }
                else
                {
                    iszViewModel.Positions = dataSelect.Structure;
                }

                if (!string.IsNullOrEmpty(noSerie))
                {
                    noSerieSimple = noSerie.Split("-")[0];
                }
                else
                {
                    return Json(new
                    {
                        response = new ApiResponse<IszViewModel>
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
                        response = new ApiResponse<IszViewModel>
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
                            response = new ApiResponse<IszViewModel>
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
                            response = new ApiResponse<IszViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee características",
                                Structure = null
                            }
                        });
                    }
                    else
                    {
                        iszViewModel.CharacteristicsArtifact = artifactDesing.CharacteristicsArtifact;
                    }

                    if (artifactDesing.VoltageKV == null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<IszViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no tensiones",
                                Structure = null
                            }
                        });
                    }
                    else
                    {
                        iszViewModel.VoltageKV = artifactDesing.VoltageKV;
                    }

                    if (artifactDesing.Derivations == null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<IszViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no tiene devrivaciones",
                                Structure = null
                            }
                        });
                    }
                    bool traelasTres = false;
                    iszViewModel.WarrantiesArtifact = artifactDesing.WarrantiesArtifact;

                    iszViewModel.WarrantiesArtifact.ZPositiveXy = iszViewModel.WarrantiesArtifact.ZPositiveXy ?? 0;
                    iszViewModel.WarrantiesArtifact.ZPositiveHx = iszViewModel.WarrantiesArtifact.ZPositiveHx ?? 0;
                    iszViewModel.WarrantiesArtifact.ZPositiveHy = iszViewModel.WarrantiesArtifact.ZPositiveHy ?? 0;


                    iszViewModel.DevanadosAgregar = new();
                    iszViewModel.SelectedIndexDevanadoEnergizado = 0;
                    if (iszViewModel.VoltageKV.TensionKvAltaTension1 != null && iszViewModel.VoltageKV.TensionKvAltaTension1!= 0 && iszViewModel.VoltageKV.TensionKvBajaTension1 != null 
                        && iszViewModel.VoltageKV.TensionKvBajaTension1 != 0 && iszViewModel.VoltageKV.TensionKvTerciario1 != null && iszViewModel.VoltageKV.TensionKvTerciario1 != 0)
                    {
                        iszViewModel.ListaPruebas = reportResult.Structure.ToList();
                        traelasTres = true;


                        if (artifactDesing.VoltageKV.TensionKvAltaTension1 != null && artifactDesing.VoltageKV.TensionKvAltaTension1 != 0)
                        {
                            if (artifactDesing.Derivations.ConexionEquivalente is "ESTRELLA" or "WYE")
                            {
                                iszViewModel.DevanadosAgregar.DevanadosAT.Add(new DevanadoEnergizadoViewModel() { Devanado = "AT", Select = false });
                            }
                        }

                        if (artifactDesing.VoltageKV.TensionKvBajaTension1 != null && artifactDesing.VoltageKV.TensionKvBajaTension1 != 0)
                        {
                            if (artifactDesing.Derivations.ConexionEquivalente_2 is "ESTRELLA" or "WYE")
                            {
                                iszViewModel.DevanadosAgregar.DevanadosAT.Add(new DevanadoEnergizadoViewModel() { Devanado = "BT", Select = false });
                            }
                        }

                        if (artifactDesing.VoltageKV.TensionKvAltaTension1 != null && artifactDesing.VoltageKV.TensionKvAltaTension1 != 0)
                        {
                            if (artifactDesing.Derivations.ConexionEquivalente is "ESTRELLA" or "WYE")
                            {
                                iszViewModel.DevanadosAgregar.DevanadosBT.Add(new DevanadoEnergizadoViewModel() { Devanado = "AT", Select = false });
                            }
                        }

                        if (artifactDesing.VoltageKV.TensionKvTerciario1 != null && artifactDesing.VoltageKV.TensionKvTerciario1 != 0)
                        {
                            if (artifactDesing.Derivations.ConexionEquivalente_4 is "ESTRELLA" or "WYE")
                            {
                                iszViewModel.DevanadosAgregar.DevanadosBT.Add(new DevanadoEnergizadoViewModel() { Devanado = "TER", Select = false });
                            }
                        }



                        if (artifactDesing.VoltageKV.TensionKvBajaTension1 != null && artifactDesing.VoltageKV.TensionKvBajaTension1 != 0)
                        {
                            if (artifactDesing.Derivations.ConexionEquivalente is "ESTRELLA" or "WYE")
                            {
                                iszViewModel.DevanadosAgregar.DevanadosTer.Add(new DevanadoEnergizadoViewModel() { Devanado = "BT", Select = false });
                            }
                        }

                        if (artifactDesing.VoltageKV.TensionKvTerciario1 != null && artifactDesing.VoltageKV.TensionKvTerciario1 != 0)
                        {
                            if (artifactDesing.Derivations.ConexionEquivalente_4 is "ESTRELLA" or "WYE")
                            {
                                iszViewModel.DevanadosAgregar.DevanadosTer.Add(new DevanadoEnergizadoViewModel() { Devanado = "TER", Select = false });
                            }
                        }

                        if (iszViewModel.DevanadosAgregar.DevanadosAT.Count() == 1)
                            iszViewModel.DevanadosAgregar.SeleccionadoAT = 1;

                        if (iszViewModel.DevanadosAgregar.DevanadosBT.Count() == 1)
                            iszViewModel.DevanadosAgregar.SeleccionadoBT = 1;

                        if (iszViewModel.DevanadosAgregar.DevanadosTer.Count() == 1)
                            iszViewModel.DevanadosAgregar.SeleccionadoTer = 1;

                    }
                    else
                    {
                        if (iszViewModel.VoltageKV.TensionKvAltaTension1 != null && iszViewModel.VoltageKV.TensionKvAltaTension1 != 0 &&
                            iszViewModel.VoltageKV.TensionKvBajaTension1 != null && iszViewModel.VoltageKV.TensionKvBajaTension1 != 0)
                        {
                            iszViewModel.ListaPruebas = reportResult.Structure.Where(x => x.ClavePrueba == "AYB").ToList();

                            if (artifactDesing.VoltageKV.TensionKvAltaTension1 != null && artifactDesing.VoltageKV.TensionKvAltaTension1 != 0)
                            {
                                if (artifactDesing.Derivations.ConexionEquivalente is "ESTRELLA" or "WYE")
                                {
                                    iszViewModel.DevanadosAgregar.DevanadosAT.Add(new DevanadoEnergizadoViewModel() { Devanado = "AT", Select = false });
                                }
                            }

                            if (artifactDesing.VoltageKV.TensionKvBajaTension1 != null && artifactDesing.VoltageKV.TensionKvBajaTension1 != 0)
                            {
                                if (artifactDesing.Derivations.ConexionEquivalente_2 is "ESTRELLA" or "WYE")
                                {
                                    iszViewModel.DevanadosAgregar.DevanadosAT.Add(new DevanadoEnergizadoViewModel() { Devanado = "BT", Select = false });
                                }
                            }


                            if (iszViewModel.DevanadosAgregar.DevanadosAT.Count() == 1)
                                iszViewModel.DevanadosAgregar.SeleccionadoAT = 1;
                        }
                        else if (iszViewModel.VoltageKV.TensionKvAltaTension1 != null && iszViewModel.VoltageKV.TensionKvAltaTension1 != 0
                            && iszViewModel.VoltageKV.TensionKvTerciario1 != null && iszViewModel.VoltageKV.TensionKvTerciario1 != 0)
                        {
                            iszViewModel.ListaPruebas = reportResult.Structure.Where(x => x.ClavePrueba == "AYT").ToList();


                            if (artifactDesing.VoltageKV.TensionKvAltaTension1 != null && artifactDesing.VoltageKV.TensionKvAltaTension1 != 0)
                            {
                                if (artifactDesing.Derivations.ConexionEquivalente is "ESTRELLA" or "WYE")
                                {
                                    iszViewModel.DevanadosAgregar.DevanadosBT.Add(new DevanadoEnergizadoViewModel() { Devanado = "AT", Select = false });
                                }
                            }

                            if (artifactDesing.VoltageKV.TensionKvTerciario1 != null && artifactDesing.VoltageKV.TensionKvTerciario1 != 0)
                            {
                                if (artifactDesing.Derivations.ConexionEquivalente_4 is "ESTRELLA" or "WYE")
                                {
                                    iszViewModel.DevanadosAgregar.DevanadosBT.Add(new DevanadoEnergizadoViewModel() { Devanado = "TER", Select = false });
                                }
                            }

                            if (iszViewModel.DevanadosAgregar.DevanadosBT.Count() == 1)
                                iszViewModel.DevanadosAgregar.SeleccionadoBT = 1;

                        }
                        else if (iszViewModel.VoltageKV.TensionKvBajaTension1 != null && iszViewModel.VoltageKV.TensionKvBajaTension1 != 0 
                            && iszViewModel.VoltageKV.TensionKvTerciario1 != null && iszViewModel.VoltageKV.TensionKvTerciario1 != 0)
                        {
                            iszViewModel.ListaPruebas = reportResult.Structure.Where(x => x.ClavePrueba == "BYT").ToList();


                            if (artifactDesing.VoltageKV.TensionKvBajaTension1 != null && artifactDesing.VoltageKV.TensionKvBajaTension1 != 0)
                            {
                                if (artifactDesing.Derivations.ConexionEquivalente is "ESTRELLA" or "WYE")
                                {
                                    iszViewModel.DevanadosAgregar.DevanadosTer.Add(new DevanadoEnergizadoViewModel() { Devanado = "BT", Select = false });
                                }
                            }

                            if (artifactDesing.VoltageKV.TensionKvTerciario1 != null && artifactDesing.VoltageKV.TensionKvTerciario1 != 0)
                            {
                                if (artifactDesing.Derivations.ConexionEquivalente_4 is "ESTRELLA" or "WYE")
                                {
                                    iszViewModel.DevanadosAgregar.DevanadosTer.Add(new DevanadoEnergizadoViewModel() { Devanado = "TER", Select = false });
                                }
                            }

                            if (iszViewModel.DevanadosAgregar.DevanadosTer.Count() == 1)
                                iszViewModel.DevanadosAgregar.SeleccionadoTer = 1;
                        }
                        else
                        {
                            return Json(new
                            {
                                response = new ApiResponse<IszViewModel>
                                {
                                    Code = -1,
                                    Description = "El aparato "+noSerie +" posee un solo tipo de conexión",
                                    Structure = iszViewModel
                                }
                            });
                        }

                    }

                    iszViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
                    iszViewModel.CantidadConexionesEstrellas = 0;



                    if (artifactDesing.Derivations.ConexionEquivalente is "WYE" or "ESTRELLA")
                    {
                        iszViewModel.CantidadConexionesEstrellas++;
                    }

                    if (artifactDesing.Derivations.ConexionEquivalente_2 is "WYE" or "ESTRELLA")
                    {
                        iszViewModel.CantidadConexionesEstrellas++;
                    }

                    if (artifactDesing.Derivations.ConexionEquivalente_4 is "WYE" or "ESTRELLA")
                    {
                        iszViewModel.CantidadConexionesEstrellas++;
                    }
                    if(traelasTres && iszViewModel.CantidadConexionesEstrellas > 1)
                    {
                        iszViewModel.SeleccionameATB = true;
                    }
                    else
                    {
                        iszViewModel.SeleccionameATB = false;
                    }

                    iszViewModel.ImpedanciaGarantizada = string.IsNullOrEmpty(artifactDesing.WarrantiesArtifact.ZPositiveHx.ToString()) ? "0" : artifactDesing.WarrantiesArtifact.ZPositiveHx.ToString();
                }

                ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _iszClientService.GetFilter(noSerie);

                if (resultFilter.Code.Equals(1))
                {
                    InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.ISZ.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.ISZ.ToString() };
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

                    iszViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Impedancia Zero",
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
                    response = new ApiResponse<IszViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = iszViewModel
                    }
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<IszViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetDataDevenadoEnergizadoAsync(string noSerieSimple, string keyTests)
        {
            ListaDevanados DevanadosAgregar = new();
            InformationArtifactDTO artifactDesing = await _artifactClientService.GetArtifact(noSerieSimple.Split("-")[0]);

            if (keyTests.ToUpper().Equals("AYB"))
            {

                //DevanadosAgregar.Add(new DevanadoEnergizadoViewModel() { Devanado = "AT", Select = false });
                //DevanadosAgregar.Add(new DevanadoEnergizadoViewModel() { Devanado = "BT", Select = false });

                if (artifactDesing.Derivations.ConexionEquivalente is "ESTRELLA" or "WYE")
                {
                    DevanadosAgregar.DevanadosAT.Add(new DevanadoEnergizadoViewModel() { Devanado = "AT", Select = false });
                }

                if (artifactDesing.Derivations.ConexionEquivalente_2 is "ESTRELLA" or "WYE")
                {
                    DevanadosAgregar.DevanadosAT.Add(new DevanadoEnergizadoViewModel() { Devanado = "BT", Select = false });
                }



            }
            else if (keyTests.ToUpper().Equals("AYT"))
            {
                if (artifactDesing.Derivations.ConexionEquivalente is "ESTRELLA" or "WYE")
                {
                    DevanadosAgregar.DevanadosBT.Add(new DevanadoEnergizadoViewModel() { Devanado = "AT", Select = false });
                }

                if (artifactDesing.Derivations.ConexionEquivalente_4 is "ESTRELLA" or "WYE")
                {
                    DevanadosAgregar.DevanadosBT.Add(new DevanadoEnergizadoViewModel() { Devanado = "TER", Select = false });
                }



            }
            else if (keyTests.ToUpper().Equals("BYT"))
            {


                if (artifactDesing.Derivations.ConexionEquivalente_2 is "ESTRELLA" or "WYE")
                {
                    DevanadosAgregar.DevanadosTer.Add(new DevanadoEnergizadoViewModel() { Devanado = "BT", Select = false });
                }

                if (artifactDesing.Derivations.ConexionEquivalente_4 is "ESTRELLA" or "WYE")
                {
                    DevanadosAgregar.DevanadosTer.Add(new DevanadoEnergizadoViewModel() { Devanado = "TER", Select = false });
                }
            }
            else if (keyTests.ToUpper().Equals("ABT"))
            {

                if(artifactDesing.VoltageKV.TensionKvAltaTension1 != null && artifactDesing.VoltageKV.TensionKvAltaTension1 != 0)
                {
                    if (artifactDesing.Derivations.ConexionEquivalente is "ESTRELLA" or "WYE")
                    {
                        DevanadosAgregar.DevanadosAT.Add(new DevanadoEnergizadoViewModel() { Devanado = "AT", Select = false });
                    }
                }

                if (artifactDesing.VoltageKV.TensionKvBajaTension1 != null && artifactDesing.VoltageKV.TensionKvBajaTension1 != 0)
                {
                    if (artifactDesing.Derivations.ConexionEquivalente_2 is "ESTRELLA" or "WYE")
                    {
                        DevanadosAgregar.DevanadosAT.Add(new DevanadoEnergizadoViewModel() { Devanado = "BT", Select = false });
                    }
                }


                if (artifactDesing.VoltageKV.TensionKvAltaTension1 != null && artifactDesing.VoltageKV.TensionKvAltaTension1 != 0)
                {
                    if (artifactDesing.Derivations.ConexionEquivalente is "ESTRELLA" or "WYE")
                    {
                        DevanadosAgregar.DevanadosBT.Add(new DevanadoEnergizadoViewModel() { Devanado = "AT", Select = false });
                    }
                }

                if (artifactDesing.VoltageKV.TensionKvTerciario1 != null && artifactDesing.VoltageKV.TensionKvTerciario1 != 0)
                {
                    if (artifactDesing.Derivations.ConexionEquivalente_4 is "ESTRELLA" or "WYE")
                    {
                        DevanadosAgregar.DevanadosBT.Add(new DevanadoEnergizadoViewModel() { Devanado = "TER", Select = false });
                    }
                }



                if (artifactDesing.VoltageKV.TensionKvBajaTension1 != null && artifactDesing.VoltageKV.TensionKvBajaTension1 != 0)
                {
                    if (artifactDesing.Derivations.ConexionEquivalente is "ESTRELLA" or "WYE")
                    {
                        DevanadosAgregar.DevanadosTer.Add(new DevanadoEnergizadoViewModel() { Devanado = "BT", Select = false });
                    }
                }

                if (artifactDesing.VoltageKV.TensionKvTerciario1 != null && artifactDesing.VoltageKV.TensionKvTerciario1 != 0)
                {
                    if (artifactDesing.Derivations.ConexionEquivalente_4 is "ESTRELLA" or "WYE")
                    {
                        DevanadosAgregar.DevanadosTer.Add(new DevanadoEnergizadoViewModel() { Devanado = "TER", Select = false });
                    }
                }
            }


            return this.Json(new
            {
                response = DevanadosAgregar
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string nroSerie, string keyTest, string lenguage,
           decimal degreesCor, string posAT, string posBT, string posTER,  int qtyNeutral, string materialWinding,
           string ATList, string BTList, string TerList, string devanado , string impedancia,string seleccionadoTodosABT, string comentarios)
        {
            try
            {
                ApiResponse<SettingsToDisplayISZReportsDTO> result = await _gatewayClientService.GetTemplateISZ(nroSerie, keyTest, lenguage, degreesCor, string.IsNullOrEmpty(posAT) ? null : posAT,
                string.IsNullOrEmpty(posBT) ? null : posBT, string.IsNullOrEmpty(posTER) ? null : posTER, qtyNeutral,  materialWinding,devanado,impedancia);

                if (result.Code.Equals(-1))
                {
                    return View("Excel", new IszViewModel
                    {
                        Error = result.Description
                    });
                }

                SettingsToDisplayISZReportsDTO reportInfo = result.Structure;

                if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
                {
                    return View("Excel", new IszViewModel
                    {
                        Error = "No existe plantilla para el filtro seleccionado"
                    });
                }

                long NoPrueba = reportInfo.NextTestNumber;

                byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
                Stream stream = new MemoryStream(bytes);

                Workbook workbook = Workbook.Load(stream, ".xlsx");
                ApiResponse<List<PlateTensionDTO>> platetension = await _artifactClientService.GetPlateTension(nroSerie, "-1");
                int filas = 0;
                string posicionMayor = "";
                _iszService.PrepareTemplate_Isz(reportInfo, ref workbook, keyTest, degreesCor,  platetension.Structure, ref filas,ref posicionMayor, lenguage, ATList?.Split(','), BTList?.Split(','), TerList?.Split(','),result.Structure.XXWindingEnergized, seleccionadoTodosABT);

                return View("Excel", new IszViewModel
                {
                    ClaveIdioma = lenguage,
                    Comments = comentarios,
                    Pruebas = keyTest,
                    Filas = filas,
                    NoPrueba = NoPrueba,
                    NoSerie = nroSerie,
                    Workbook = workbook,
                    Error = string.Empty,
                    Grados = degreesCor.ToString(),
                    //DevanadoEnergizado = windingEnergized,
                    CantidadNeutros = qtyNeutral.ToString(),
                    ATList = ATList,
                    BTList = BTList,
                    TerList = TerList,
                    SettingsISZ = reportInfo,
                    MaterialDevanado = materialWinding,
                    AtSelected = posAT,
                    BtSelected = posBT,
                    TerSelected = posTER,
                    PosicionMayotABT = posicionMayor,
                    ImpedanciaGarantizada = impedancia,
                    DevanadoEnergizado = devanado,
                    SeleccionadoTodosABT = seleccionadoTodosABT
                });
            }
            catch (Exception e)
            {
                return View("Excel", new IszViewModel
                {
                    Error = e.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] IszViewModel viewModel)
        {
            try
            {
              /*  string texto2 = System.IO.File.ReadAllText(@"C:\Users\Barboza\Desktop\calculeTestIsZ.json");
                var testOut2 = JsonConvert.DeserializeObject<OutISZTestsDTO>(texto2);

                ApiResponse<ResultISZTestsDTO> calculateResult2 = await _iszClientService.CalculateTestIsz(testOut2);*/
                SettingsToDisplayISZReportsDTO reportInfo = viewModel.SettingsISZ;
                /******** VALIDAR CAMPOS INPUT del EXCEL ****************/
                bool flag = false;
                int[] positions = null;
                int[] secciones = new int[] { };
                int lastSeccion = 0;

                if (viewModel.Pruebas != "ABT")
                {

                    for (int i = 1; i <= viewModel.Filas; i++)
                    {
                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionVrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        object tension = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]].Value;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        object corriente = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]].Value;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PotenciaKW") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        object pot = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]].Value;

                        if (tension == null || corriente == null || pot == null)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                else
                {
        

                    if (viewModel.PosicionMayotABT == "AT")
                    {
                        secciones = new int[] { 1,2};
                        lastSeccion = 3;
                    }
                    else if (viewModel.PosicionMayotABT == "BT")
                    {
                        secciones = new int[] { 1, 3};
                        lastSeccion = 2;
                    }
                    else if (viewModel.PosicionMayotABT.ToUpper() == "TER")
                    {
                        secciones = new int[] { 2, 3 };
                        lastSeccion = 1;
                    }


                    positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == lastSeccion && c.Dato.Equals("TensionVrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                    object tension = viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value;

                    positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == lastSeccion && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                    object corriente = viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value;

                    positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == lastSeccion && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == viewModel.Pruebas).Celda);
                    object pot = viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value;

                    if (tension == null || corriente == null || pot == null)
                    {
                        flag = true;
                    }

                    if (!flag)
                    {


                        for (int i = 1; i <= viewModel.Filas; i++)
                        {
                            foreach (var sec in secciones)
                            {
                                positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == sec && c.Dato.Equals("TensionVrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                                tension = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]].Value;

                                positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == sec && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                                corriente = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]].Value;

                                positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == sec && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == viewModel.Pruebas).Celda);
                                pot = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]].Value;

                                if (tension == null || corriente == null || pot == null)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                }


                if (flag)
                {
                    return Json(new
                    {
                        response = new ApiResponse<IszViewModel>
                        {
                            Code = -1,
                            Description = "Debe llenar todas las posiciones de corriente, potencia y tension"
                        }
                    });
                }

                /******** VALIDAR CAMPOS del EXCE ****************/
                string fecha;

                positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("Fecha")).Celda);
                fecha = viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

                if (string.IsNullOrEmpty(fecha))
                {
                    return Json(new
                    {
                        response = new ApiResponse<IszViewModel>
                        {
                            Code = -1,
                            Description = "La fecha no puede estar vacia."
                        }
                    });
                }
                DateTime basedate = new(1899, 12, 30);

                fecha = fecha.Contains("/") ? (DateTime.Now.Date - basedate.Date).TotalDays.ToString() : fecha;
                viewModel.Date = fecha;

                positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("CapacidadBase")).Celda);
                _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out decimal baseRating);

                if (baseRating == 0)
                {
                    return Json(new
                    {
                        response = new ApiResponse<IszViewModel>
                        {
                            Code = -1,
                            Description = "El valor de la capacidad base no puede ser 0 o no puede estar vacio"
                        }
                    });
                }

                positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("Temperatura")).Celda);
                _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out decimal tempe);

                if (tempe == 0)
                {
                    return Json(new
                    {
                        response = new ApiResponse<IszViewModel>
                        {
                            Code = -1,
                            Description = "El valor de la temperatura base no puede ser 0 o no puede estar vacio"
                        }
                    });
                }

                InformationArtifactDTO artifact = await _artifactClientService.GetArtifact(viewModel.NoSerie.Split("-")[0]);
                if (artifact == null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<IszViewModel>
                        {
                            Code = -1,
                            Description = "No se ha encontrado Artifact"
                        }
                    });
                }
                else
                {
                    if (artifact.WarrantiesArtifact is null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<IszViewModel>
                            {
                                Code = -1,
                                Description = "El objecto WarrantiesArtifact es nulo"
                            }
                        });
                    }
                }

                string[] arrAt = !string.IsNullOrEmpty(viewModel.ATList) ? viewModel.ATList.Split(',') : null;
                string[] arrBT = !string.IsNullOrEmpty(viewModel.BTList) ? viewModel.BTList.Split(',') : null;
                string[] arrTer = !string.IsNullOrEmpty(viewModel.TerList) ? viewModel.TerList.Split(',') : null;

                string AT = viewModel.AtSelected;
                string BT = viewModel.BtSelected;
                string Ter = viewModel.TerSelected;

                ApiResponse<List<ValidationTestsIszDTO>> validationTest = new ApiResponse<List<ValidationTestsIszDTO>> { Structure = new List<ValidationTestsIszDTO> { } };

                positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("UMCapBase")).Celda);
                string UMCap = viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

                positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("UMTemp")).Celda);
                string UMTemp = viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();
                ApiResponse<PositionsDTO> dataSelect = await _gatewayClientService.GetPositions(viewModel.NoSerie);

                if (dataSelect.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<IszViewModel>
                        {
                            Code = -1,
                            Description = dataSelect.Description
                        }
                    });
                }

                string valueNom = string.Empty;

                if (viewModel.Pruebas == "AYB" && AT!=null && AT.Length>0)
                {
                    valueNom = dataSelect.Structure.ATNom;
                }
                else if (viewModel.Pruebas == "AYB" && BT != null && BT.Length > 0)
                {
                    valueNom = dataSelect.Structure.BTNom;
                }
                else if (viewModel.Pruebas == "AYT" && AT != null && AT.Length > 0)
                {
                    valueNom = dataSelect.Structure.ATNom;
                }
                else if (viewModel.Pruebas == "AYT" && Ter != null && Ter.Length > 0)
                {
                    valueNom = dataSelect.Structure.TerNom;
                }
                else if (viewModel.Pruebas == "BYT" && Ter != null && Ter.Length > 0)
                {
                    valueNom = dataSelect.Structure.TerNom;
                }
                else if (viewModel.Pruebas == "BYT" && BT != null && BT.Length > 0)
                {
                    valueNom = dataSelect.Structure.BTNom;
                }
                else
                {
                    if(viewModel.PosicionMayotABT == "AT")
                    {
                        valueNom = dataSelect.Structure.ATNom;
                    }
                    else if(viewModel.PosicionMayotABT == "BT")
                    {
                        valueNom = dataSelect.Structure.BTNom;
                    }
                    else if (viewModel.PosicionMayotABT.ToUpper() == "TER")
                    {
                        valueNom = dataSelect.Structure.TerNom;
                    }
                }
            

                if (string.IsNullOrEmpty(valueNom))
                {

                    return Json(new
                    {
                        response = new ApiResponse<IszViewModel>
                        {
                            Code = -1,
                            Description = "No se ha podido definir una posicion nominal en base a los filtros seleccionados"
                        }
                    });

                }

                List<SeccionesISZTestDetailsDTO> seccionesISZ = new();
                if (viewModel.Pruebas != "ABT")
                {
                    List < ISZTestsDetailsDTO > seccionUnica= new();

                    for (int i = 1; i <= viewModel.Filas; i++)
                    {
                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        string posi1 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Pos_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        string posi2 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out decimal tension1);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Tension_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out decimal tension2);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("TensionVrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out decimal voltaje);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out decimal corriente);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PotenciaKW") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out decimal potencia);

                        seccionUnica.Add(new ISZTestsDetailsDTO
                        {
                            Position1 = posi1,
                            Position2 = posi2,
                            Voltage1 = tension1,
                            Voltage2 = tension2,
                            VoltsVRMS = voltaje,
                            CurrentsIRMS = corriente,
                            PowerKW = potencia

                        });
                    }
                    seccionesISZ.Add(new SeccionesISZTestDetailsDTO { ISZTestsDetails = seccionUnica });


                }
                else
                {
                    List<ISZTestsDetailsDTO> seccion1 = new();
                    List<ISZTestsDetailsDTO> seccion2 = new();
                    List<ISZTestsDetailsDTO> seccion3 = new();

                    string posi1 = "";
                    string posi2 = "";
                    decimal tension1 = 0;
                    decimal tension2 = 0;
                    decimal voltaje = 0;
                    decimal corriente = 0;
                    decimal potencia = 0;
                    if (viewModel.PosicionMayotABT == "AT")
                    {
                        for (int i = 1; i <= viewModel.Filas; i++)
                        {
                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Pos_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            posi1 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Pos_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            posi2 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Tension_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out  tension1);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Tension_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out  tension2);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("TensionVrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out  voltaje);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out  corriente);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out  potencia);

                            seccion1.Add(new ISZTestsDetailsDTO
                            {
                                Position1 = posi1,
                                Position2 = posi2,
                                Voltage1 = tension1,
                                Voltage2 = tension2,
                                VoltsVRMS = voltaje,
                                CurrentsIRMS = corriente,
                                PowerKW = potencia

                            });


                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Pos_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            posi1 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Pos_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            posi2 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Tension_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out tension1);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Tension_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out tension2);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("TensionVrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out voltaje);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out corriente);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out  potencia);

                            seccion2.Add(new ISZTestsDetailsDTO
                            {
                                Position1 = posi1,
                                Position2 = posi2,
                                Voltage1 = tension1,
                                Voltage2 = tension2,
                                VoltsVRMS = voltaje,
                                CurrentsIRMS = corriente,
                                PowerKW = potencia

                            });

                        }

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Pos_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        posi1 = viewModel.Workbook.Sheets[0].Rows[positions[0] ].Cells[positions[1]]?.Value?.ToString();

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Pos_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        posi2 = viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Tension_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] ].Cells[positions[1]]?.Value?.ToString(), out  tension1);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Tension_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] ].Cells[positions[1]]?.Value?.ToString(), out  tension2);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("TensionVrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out  voltaje);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out  corriente);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out  potencia);

                        seccion3.Add(new ISZTestsDetailsDTO
                        {
                            Position1 = posi1,
                            Position2 = posi2,
                            Voltage1 = tension1,
                            Voltage2 = tension2,
                            VoltsVRMS = voltaje,
                            CurrentsIRMS = corriente,
                            PowerKW = potencia

                        });

                        seccionesISZ.Add(new SeccionesISZTestDetailsDTO { ISZTestsDetails = seccion1 });
                        seccionesISZ.Add(new SeccionesISZTestDetailsDTO { ISZTestsDetails = seccion2 });
                        seccionesISZ.Add(new SeccionesISZTestDetailsDTO { ISZTestsDetails = seccion3 });

                    }
                    else if (viewModel.PosicionMayotABT == "BT")
                    {
                        for (int i = 1; i <= viewModel.Filas; i++)
                        {
                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Pos_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            posi1 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Pos_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            posi2 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Tension_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out tension1);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Tension_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out tension2);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("TensionVrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out voltaje);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out corriente);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out potencia);

                            seccion1.Add(new ISZTestsDetailsDTO
                            {
                                Position1 = posi1,
                                Position2 = posi2,
                                Voltage1 = tension1,
                                Voltage2 = tension2,
                                VoltsVRMS = voltaje,
                                CurrentsIRMS = corriente,
                                PowerKW = potencia

                            });


                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Pos_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            posi1 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Pos_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            posi2 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Tension_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out tension1);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Tension_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out tension2);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("TensionVrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out voltaje);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out corriente);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out potencia);

                            seccion3.Add(new ISZTestsDetailsDTO
                            {
                                Position1 = posi1,
                                Position2 = posi2,
                                Voltage1 = tension1,
                                Voltage2 = tension2,
                                VoltsVRMS = voltaje,
                                CurrentsIRMS = corriente,
                                PowerKW = potencia

                            });

                        }

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Pos_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        posi1 = viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Pos_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        posi2 = viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Tension_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out tension1);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Tension_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out tension2);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("TensionVrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out voltaje);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out corriente);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out potencia);

                        seccion2.Add(new ISZTestsDetailsDTO
                        {
                            Position1 = posi1,
                            Position2 = posi2,
                            Voltage1 = tension1,
                            Voltage2 = tension2,
                            VoltsVRMS = voltaje,
                            CurrentsIRMS = corriente,
                            PowerKW = potencia

                        });

                        seccionesISZ.Add(new SeccionesISZTestDetailsDTO { ISZTestsDetails = seccion1 });
                        seccionesISZ.Add(new SeccionesISZTestDetailsDTO { ISZTestsDetails = seccion2 });
                        seccionesISZ.Add(new SeccionesISZTestDetailsDTO { ISZTestsDetails = seccion3 });
                    }
                    else if (viewModel.PosicionMayotABT.ToUpper() == "TER")
                    {
                        for (int i = 1; i <= viewModel.Filas; i++)
                        {
                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Pos_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            posi1 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Pos_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            posi2 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Tension_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out tension1);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Tension_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out tension2);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("TensionVrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out voltaje);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out corriente);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out potencia);

                            seccion2.Add(new ISZTestsDetailsDTO
                            {
                                Position1 = posi1,
                                Position2 = posi2,
                                Voltage1 = tension1,
                                Voltage2 = tension2,
                                VoltsVRMS = voltaje,
                                CurrentsIRMS = corriente,
                                PowerKW = potencia

                            });


                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Pos_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            posi1 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Pos_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            posi2 = viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString();

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Tension_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out tension1);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Tension_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out tension2);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("TensionVrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out voltaje);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out corriente);

                            positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == viewModel.Pruebas).Celda);
                            _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0] + (i - 1)].Cells[positions[1]]?.Value?.ToString(), out potencia);

                            seccion3.Add(new ISZTestsDetailsDTO
                            {
                                Position1 = posi1,
                                Position2 = posi2,
                                Voltage1 = tension1,
                                Voltage2 = tension2,
                                VoltsVRMS = voltaje,
                                CurrentsIRMS = corriente,
                                PowerKW = potencia

                            });

                        }

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Pos_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        posi1 = viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Pos_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        posi2 = viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Tension_1") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out tension1);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Tension_2") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out tension2);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("TensionVrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out voltaje);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("CorrienteIrms") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out corriente);

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("PotenciaKW") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        _ = decimal.TryParse(viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString(), out potencia);

                        seccion1.Add(new ISZTestsDetailsDTO
                        {
                            Position1 = posi1,
                            Position2 = posi2,
                            Voltage1 = tension1,
                            Voltage2 = tension2,
                            VoltsVRMS = voltaje,
                            CurrentsIRMS = corriente,
                            PowerKW = potencia

                        });

                        seccionesISZ.Add(new SeccionesISZTestDetailsDTO { ISZTestsDetails = seccion1 });
                        seccionesISZ.Add(new SeccionesISZTestDetailsDTO { ISZTestsDetails = seccion2 });
                        seccionesISZ.Add(new SeccionesISZTestDetailsDTO { ISZTestsDetails = seccion3 });
                    }
                }


                var r1 = decimal.TryParse(reportInfo.ConfigurationReports.FirstOrDefault(x => x.Dato == "PorcMaxAcepImp")?.Formato,out decimal porcMax);
                var r2 = decimal.TryParse(reportInfo.ConfigurationReports.FirstOrDefault(x => x.Dato == "PorcMinAcepImp")?.Formato, out decimal porcMin);

                if(r1 && r2)
                {
                }
                else
                {
                    return Json(new
                    {
                        response = new ApiResponse<PciViewModel>
                        {
                            Code = -1,
                            Description = "No se ha podido extraer el porcentaje de aceptacion mínimo o máximo"
                        }
                    });;
                }

                OutISZTestsDTO testOut = new()
                {
                    KeyTest = viewModel.Pruebas,
                    Lenguage = viewModel.ClaveIdioma,
                    DegreesCor = decimal.Parse(viewModel.Grados),
                    WindingEnergized = viewModel.DevanadoEnergizado,
                    QtyNeutral = int.Parse(viewModel.CantidadNeutros),
                    ImpedanceGar = viewModel.ImpedanciaGarantizada,
                    MaterialWinding = viewModel.MaterialDevanado,
                    PosAT = AT,
                    PosBT = BT,
                    PosTER = Ter,
                    GeneralArtifact = artifact,
                    ValidationTestsIsz = validationTest.Structure,
                    CapBaseMin = baseRating,
                    Temperature = tempe,
                    UmCapBaseMin = UMCap,
                    UmTemperature = UMTemp,
                    ValueNomPosAll = valueNom,
                    SeccionesISZTestsDetails = seccionesISZ,
                    PorcMaxAcepImp = porcMax,
                    PorcMinAcepImp = porcMin,
                    PosicionMayotABT = viewModel.PosicionMayotABT,
                    NominalAT = dataSelect.Structure.ATNom,
                    NominalTer = dataSelect.Structure.TerNom,
                    NominalBT = dataSelect.Structure.BTNom,

                };
                string p = JsonConvert.SerializeObject(testOut);

                ApiResponse<ResultISZTestsDTO> calculateResult = await _iszClientService.CalculateTestIsz(testOut);
                //var p = JsonConvert.SerializeObject(testOut);
                List<string> errores = new();
                if (calculateResult.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<PciViewModel>
                        {
                            Code = -1,
                            Description = "Error en calculo de pruebas ISZ: " + calculateResult.Description
                        }
                    });
                }




                if (viewModel.Pruebas == "ABT")
                {




                    var sec1 = calculateResult.Structure.ISZTests.SeccionesISZTestsDetails[0];
                    var sec2 = calculateResult.Structure.ISZTests.SeccionesISZTestsDetails[1];
                    var sec3 = calculateResult.Structure.ISZTests.SeccionesISZTestsDetails[2];


                    positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion ==1 && c.Dato.Equals("Porc_Z")).Celda);
                    viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value = sec1.Porc_Z;
                    viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]+2].Value = sec1.Porc_R;
                    viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]+3].Value = sec1.Porc_X;

                    positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Porc_Z")).Celda);
                    viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value = sec2.Porc_Z;
                    viewModel.Workbook.Sheets[0].Rows[positions[0] ].Cells[positions[1]+2].Value = sec2.Porc_R;
                    viewModel.Workbook.Sheets[0].Rows[positions[0] ].Cells[positions[1]+3].Value = sec2.Porc_X;

                    positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Porc_Z")).Celda);
                    viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value = sec3.Porc_Z;
                    viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1] + 2].Value = sec3.Porc_R;
                    viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1] + 3].Value = sec3.Porc_X;


                    for (int i =0; i<sec1.ISZTestsDetails.Count; i++)
                    {
                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Porc_jXo") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec1.ISZTestsDetails[i].PercentagejXo;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Porc_Ro") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec1.ISZTestsDetails[i].PercentageRo;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Porc_Zo") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec1.ISZTestsDetails[i].PercentageZo;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("PotenciaCorr") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec1.ISZTestsDetails[i].PowerKWVoltage1;
                    }

                    for (int i = 0; i < sec2.ISZTestsDetails.Count; i++)
                    {
                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Porc_jXo") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec2.ISZTestsDetails[i].PercentagejXo;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Porc_Ro") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec2.ISZTestsDetails[i].PercentageRo;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("Porc_Zo") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec2.ISZTestsDetails[i ].PercentageZo;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 2 && c.Dato.Equals("PotenciaCorr") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec2.ISZTestsDetails[i].PowerKWVoltage1;
                    }

                    for (int i = 0; i < sec3.ISZTestsDetails.Count; i++)
                    {
                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Porc_jXo") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec3.ISZTestsDetails[i].PercentagejXo;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Porc_Ro") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec3.ISZTestsDetails[i].PercentageRo;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("Porc_Zo") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec3.ISZTestsDetails[i].PercentageZo;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 3 && c.Dato.Equals("PotenciaCorr") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec3.ISZTestsDetails[i].PowerKWVoltage1;
                    }


                }
                else
                {
                    var sec1 = calculateResult.Structure.ISZTests.SeccionesISZTestsDetails[0];
                    for (int i = 0; i < sec1.ISZTestsDetails.Count; i++)
                    {
                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Porc_jXo") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec1.ISZTestsDetails[i].PercentagejXo;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Porc_Ro") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec1.ISZTestsDetails[i].PercentageRo;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("Porc_Zo") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec1.ISZTestsDetails[i].PercentageZo;

                        positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Seccion == 1 && c.Dato.Equals("PotenciaCorr") && c.ClavePrueba == viewModel.Pruebas).Celda);
                        viewModel.Workbook.Sheets[0].Rows[positions[0] + (i)].Cells[positions[1]].Value = sec1.ISZTestsDetails[i].PowerKWVoltage1;

                    }
         
                }

                string resultadoString = string.Empty;
                resultadoString = calculateResult.Structure.Results.Any()
                    ? viewModel.ClaveIdioma == "EN"
                        ? reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorRechazadoEN")).Formato
                        : reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorRechazadoES")).Formato
                    : viewModel.ClaveIdioma == "EN"
                        ? reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptadoEN")).Formato
                        : reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("ValorAceptadoES")).Formato;

                positions = GetRowColOfWorbook(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
                viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]].Value = resultadoString;

                errores.AddRange(calculateResult.Structure.Results.Select(k => k.Message).ToList());
                string allErrosPrepare = errores.Any() ? errores.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
                Workbook workbook = viewModel.Workbook;
                bool resultReport = !calculateResult.Structure.Results.Any();
                viewModel.ResultadoCalculateISZ = calculateResult.Structure.ISZTests;
                viewModel.IsReportAproved = resultReport;
                viewModel.Workbook = workbook;
                viewModel.OfficialWorkbook = workbook;
                // viewModel.PCIOutDto = calculateResult.Structure.PCIOutTests;

                return Json(new
                {
                    response = new ApiResponse<IszViewModel>
                    {
                        Code = 1,
                        Description = allErrosPrepare,
                        Structure = viewModel
                    }
                });

            }
            catch (Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<IszViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = viewModel
                    }
                });

            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] IszViewModel viewModel)
        {

            Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

            PdfFormatProvider provider = new()
            {
                ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
            };
            document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.OfficialWorkbook.ActiveSheet);
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

                if (viewModel.Pruebas == "ABT")
                {
                    image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(40, 7), 200, 20);
                    path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "imageISZ.jpg");

                    stream = new(path, FileMode.Open);
                    using (stream)
                    {
                        image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                    }

                    image.Width = 210;
                    image.Height = 120;

                    sheet.Shapes.Add(image);
                }

                sheet.WorksheetPageSetup.PaperType = Telerik.Windows.Documents.Model.PaperTypes.A4;
                sheet.WorksheetPageSetup.PageOrientation = Telerik.Windows.Documents.Model.PageOrientation.Portrait;
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

            pdfFile = pdfStream.ToArray();
            viewModel.Base64PDF = Convert.ToBase64String(pdfFile);

            DateTime basedate = new(1899, 12, 30);

            ISZTestsGeneralDTO iszTestGeneral = new()
            {
                Capacity = viewModel.SettingsISZ.HeadboardReport.Capacity,
                Comment = viewModel.Comments,
                Creadopor = User.Identity.Name,
                Customer = viewModel.SettingsISZ.HeadboardReport.Client,
                Fechacreacion = DateTime.Now,
                File = pdfFile,
                IdLoad = 0,
                KeyTest = viewModel.Pruebas,
                LanguageKey = viewModel.ClaveIdioma,
                LoadDate = basedate.AddDays(int.Parse(viewModel.Date)),
                Modificadopor = null,
                NameFile = string.Concat("ISZ", viewModel.Pruebas, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                Result = viewModel.IsReportAproved,
                SerialNumber = viewModel.NoSerie,
                TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                TypeReport = ReportType.ISZ.ToString(),
                Data = viewModel.ResultadoCalculateISZ

            };

            try
            {
                var a = JsonConvert.SerializeObject(iszTestGeneral);
                ApiResponse<long> result = await _iszClientService.SaveReport(iszTestGeneral);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = iszTestGeneral.NameFile, file = viewModel.Base64PDF };

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

        public async Task<IActionResult> GetPDFReport(long code, string typeReport)
        {
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

        public IActionResult Error() => View();

        #region PrivateMethods
        public async Task PrepareForm()
        {

            //var a = await this._gatewayClientService.GetPositions("G4135-01");

            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> gradosCorreccion = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } ,
                new GeneralPropertiesDTO { Clave = "75", Descripcion = "75" } ,
                new GeneralPropertiesDTO { Clave = "85", Descripcion = "85" },
                new GeneralPropertiesDTO { Clave = "Otro", Descripcion = "Otro" }
            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> devanadaoEnergizado = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } ,
                new GeneralPropertiesDTO { Clave = "AT", Descripcion = "AT" } ,
                new GeneralPropertiesDTO { Clave = "BT", Descripcion = "BT" },
                new GeneralPropertiesDTO { Clave = "Ter", Descripcion = "Ter" }
            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> devanadoSelect = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } ,
            }.AsEnumerable();
            ViewBag.DevanadoSelectTotal = new SelectList(devanadoSelect, "Clave", "Descripcion");




            IEnumerable<GeneralPropertiesDTO> materialProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "Cobre", Descripcion = "Cobre" }, new GeneralPropertiesDTO { Clave = "Aluminio", Descripcion = "Aluminio" } }.AsEnumerable();

            // Tipos de prueba
            ViewBag.TestItems = new SelectList(origingeneralProperties, "Clave", "Descripcion");
            ViewBag.MaterialDevanado = new SelectList(materialProperties, "Clave", "Descripcion");

            ViewBag.Select1 = new SelectList(new List<GeneralPropertiesDTO>()
            {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" } ,

            }.AsEnumerable(), "Clave", "Descripcion");
            ViewBag.Select2 = new SelectList(new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" },

            }.AsEnumerable(), "Clave", "Descripcion");
            ViewBag.Select3 = new SelectList(new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" } }.AsEnumerable(), "Clave", "Descripcion");

            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");
            ViewBag.Grados = new SelectList(gradosCorreccion, "Clave", "Descripcion");
            ViewBag.DevanadoEnergizado = new SelectList(devanadaoEnergizado, "Clave", "Descripcion");
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

