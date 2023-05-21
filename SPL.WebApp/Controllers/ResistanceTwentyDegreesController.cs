namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Text.Json;
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
    using SPL.WebApp.Helpers;
    using SPL.WebApp.ViewModels;

    public class ResistanceTwentyDegreesController : Controller
    {
        private readonly IArtifactClientService _artifactClientService;
        private readonly IPlateTensionService _plateTensionService;
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IResistanceTwentyDegreesClientServices _resistanceTwentyDegressClientService;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IMapper _mapper;

        private decimal valTension, tensionAT, tensionBT, tensionTER;

        private int? invertidoGrid;
        private DataTable dtValorNom = new();
        private readonly char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private readonly int? idIdentificacion = 0;
        private readonly IProfileSecurityService _profileClientService;
        public ResistanceTwentyDegreesController(
            IArtifactClientService artifactClientService,
            IPlateTensionService plateTensionService,
            IMapper mapper,
            ISidcoClientService sidcoClientService,
            IGatewayClientService gatewayClientService,
            IMasterHttpClientService masterHttpClientService,
            IResistanceTwentyDegreesClientServices resistanceTwentyDegreesClientServices,
            IProfileSecurityService profileClientService)
        {
            _mapper = mapper;
            _artifactClientService = artifactClientService;
            _plateTensionService = plateTensionService;
            _masterHttpClientService = masterHttpClientService;
            _gatewayClientService = gatewayClientService;
            _resistanceTwentyDegressClientService = resistanceTwentyDegreesClientServices;
            _profileClientService = profileClientService;
            _sidcoClientService = sidcoClientService;
        }

        public IActionResult Index()
        {
            try
            {
                ResistanceTwentyDegreesViewModel model = new() { };

                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;
                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.Resistenciaa20C)))
                {

                    PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(model);
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

        [HttpGet]
        public async Task<IActionResult> GetConnectionTest(string noSerie)
        {
            try
            {
                if (!string.IsNullOrEmpty(noSerie))
                {
                    string[] noSerieWo_ = noSerie.Trim().Split('-');
                    IEnumerable<GeneralPropertiesDTO> result = await _masterHttpClientService.GetConnectionTest(noSerieWo_[0]);

                    return Json(new
                    {

                        response = new ApiResponse<IEnumerable<GeneralPropertiesDTO>>
                        {
                            Code = 1,
                            Description = string.Empty,
                            Structure = result
                        }
                    });
                }
                else
                {
                    return Json(new
                    {

                        response = new ApiResponse<string>
                        {
                            Code = -1,
                            Description = "Ingrese un Artefacto válido.",
                            Structure = string.Empty
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {

                    response = new ApiResponse<string>
                    {
                        Code = -1,
                        Description = ex.Message,
                        Structure = string.Empty
                    }
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            string noSerie, string measuring, decimal temperature, bool? newTensonResponse, bool requestInicial)
        {
            try
            {
                ResistanceTwentyDegreesViewModel model = new() { };
                ApiResponse<PositionsDTO> positions = _gatewayClientService.GetPositions(noSerie).Result;
                model.PositionsDTO = positions.Structure;
                ApiResponse<List<ResistDesignDTO>> resistanceAllVoltageResponse = _resistanceTwentyDegressClientService.GetResistDesignCustom(noSerie, measuring, "-1", temperature, "-1").Result;
                InformationArtifactDTO artifactDesing = _artifactClientService.GetArtifact(noSerie.Split("-")[0]).Result;

                IEnumerable<CatSidcoDTO> sidcos = await _sidcoClientService.GetCatSIDCO();
                CatSidcoDTO catSidco = sidcos.Where(s => s.AttributeId == 3 && s.Id == Convert.ToInt32(artifactDesing.GeneralArtifact.TypeTrafoId)).FirstOrDefault();

                model.PositionsDTO = positions.Structure;
                // positions.Structure.AltaTension = new List<string>() { "P", "K" };

                if (catSidco == null)
                {
                    return Json(new
                    {

                        response = new ApiResponse<ResistanceTwentyDegreesViewModel>
                        {
                            Code = -1,
                            Description = "No se pudo identificar el tipo de transformador para el aparato " + noSerie,
                            Structure = model
                        }
                    });
                }

                List<ResistDesignDTO> resistencias = resistanceAllVoltageResponse.Structure;
                bool isEqualATSidCo = false;
                bool isEqualBTSidCo = false;
                bool isEqualTerSidCo = false;

                if (requestInicial)
                {

                    List<string> posicionesSIDCOAT = positions.Structure.AltaTension;
                    List<string> posicionesSIDCOBT = positions.Structure.BajaTension;
                    List<string> posicionesSIDCOTer = positions.Structure.Terciario;

                    List<string> resisAT = resistencias.Where(x => x.IdSeccion.ToUpper() == "AT").Select(y => y.Posicion).Distinct().ToList();
                    List<string> resisBT = resistencias.Where(x => x.IdSeccion.ToUpper() == "BT").Select(y => y.Posicion).Distinct().ToList();
                    List<string> resisTer = resistencias.Where(x => x.IdSeccion.ToUpper() == "TER").Select(y => y.Posicion).Distinct().ToList();

                    isEqualATSidCo = resisAT.Count() == 0 || Enumerable.SequenceEqual(resisAT, posicionesSIDCOAT != null && posicionesSIDCOAT.Count() > 0 ? posicionesSIDCOAT : resisAT);
                    isEqualBTSidCo = resisBT.Count() == 0 || Enumerable.SequenceEqual(resisBT, posicionesSIDCOBT != null && posicionesSIDCOBT.Count() > 0 ? posicionesSIDCOBT : resisBT);
                    isEqualTerSidCo = resisTer.Count() == 0 || Enumerable.SequenceEqual(resisTer, posicionesSIDCOTer != null && posicionesSIDCOTer.Count() > 0 ? posicionesSIDCOTer : resisTer);

                    if (resisAT.Count == 0 && resisBT.Count == 0 && resisTer.Count == 0)// no hay registros en bd para ese aparato 
                    {
                        //carga las que venga en el GetPositions
                    }
                    else
                    {

                        if (!isEqualATSidCo || !isEqualBTSidCo || !isEqualTerSidCo)// si entra aca es porque en alguno de los devanados por lo menos una posicion cambio com repsecto a lo que hay en bd y lo que hay en plate tension
                        {
                            model.HayDataNueva = true;
                        }
                    }

                    /**************************************************************************************************************/

                    model.RequestInicial = false;

                    if (model.HayDataNueva)
                    {
                        return Json(new
                        {

                            response = new ApiResponse<ResistanceTwentyDegreesViewModel>
                            {
                                Code = 2,
                                Description = "NEW_DATA",
                                Structure = model
                            }
                        });
                    }
                }

                if (newTensonResponse != null)
                {
                    model.AceptaCargarLaDataNueva = (bool)newTensonResponse;

                    if (model.AceptaCargarLaDataNueva)
                    {
                        List<string> resisAT = resistencias.Where(x => x.IdSeccion.ToUpper() == "AT").Select(y => y.Posicion).Distinct().ToList();
                        List<string> resisBT = resistencias.Where(x => x.IdSeccion.ToUpper() == "BT").Select(y => y.Posicion).Distinct().ToList();
                        List<string> resisTer = resistencias.Where(x => x.IdSeccion.ToUpper() == "TER").Select(y => y.Posicion).Distinct().ToList();
                        List<string> posicionesSIDCOAT = positions.Structure.AltaTension;
                        List<string> posicionesSIDCOBT = positions.Structure.BajaTension;
                        List<string> posicionesSIDCOTer = positions.Structure.Terciario;

                        isEqualATSidCo = Enumerable.SequenceEqual(resisAT, posicionesSIDCOAT != null && posicionesSIDCOAT.Count() > 0 ? posicionesSIDCOAT : resisAT);
                        isEqualBTSidCo = Enumerable.SequenceEqual(resisBT, posicionesSIDCOBT != null && posicionesSIDCOBT.Count() > 0 ? posicionesSIDCOBT : resisBT);
                        isEqualTerSidCo = Enumerable.SequenceEqual(resisTer, posicionesSIDCOTer != null && posicionesSIDCOTer.Count() > 0 ? posicionesSIDCOTer : resisTer);

                        if (posicionesSIDCOAT == null || posicionesSIDCOAT.Count == 0)
                        {
                            isEqualATSidCo = true;
                        }

                        if (posicionesSIDCOBT == null || posicionesSIDCOBT.Count == 0)
                        {
                            isEqualBTSidCo = true;
                        }

                        if (posicionesSIDCOTer == null || posicionesSIDCOTer.Count == 0)
                        {
                            isEqualTerSidCo = true;
                        }
                    }
                }

                #region Conversion de posiciones de PLATE TENSION
                List<int> conversionAT = positions.Structure.AltaTension.Select(s => int.TryParse(s, out int i) ? i : -1).ToList();

                positions.Structure.AltaTension = !conversionAT.Any(x => x == -1) ? positions.Structure.AltaTension.ToList() : positions.Structure.AltaTension.ToList();

                List<int> conversionBT = positions.Structure.BajaTension.Select(s => int.TryParse(s, out int i) ? i : -1).ToList();

                positions.Structure.BajaTension = !conversionBT.Any(x => x == -1) ? positions.Structure.BajaTension.ToList() : positions.Structure.BajaTension.ToList();

                List<int> conversionTer = positions.Structure.Terciario.Select(s => int.TryParse(s, out int i) ? i : -1).ToList();

                positions.Structure.Terciario = !conversionTer.Any(x => x == -1) ? positions.Structure.Terciario.ToList() : positions.Structure.Terciario.ToList();

                #endregion

                if (catSidco.ClaveSPL == "AUT")//es autotransformador
                {
                    #region LLenado de las AT
                    model.TipoAparato = "AUT";
                    List<ResistDesignDTO> data = resistencias.Where(x => x.ConexionPrueba == "H-X" && x.IdSeccion == "AT").OrderBy(y => y.Orden).ToList();
                    if (!isEqualATSidCo && model.AceptaCargarLaDataNueva)
                    {
                        data = new List<ResistDesignDTO>();
                    }

                    if (data.Any())//si hay data ponemos lo que viene de base de datos
                    {
                        model.ResistDesignHX.AddRange(data);
                    }
                    else
                    {
                        foreach (var posi in positions.Structure.AltaTension.Select((value, i) => new { i, value }).OrderBy(y => y.i))
                        {
                            model.ResistDesignHX.Add(new ResistDesignDTO
                            {
                                Id = posi.i + 1,
                                Posicion = posi.value,
                                Resistencia = 0
                            });

                        }
                    }

                    data = resistencias.Where(x => x.ConexionPrueba == "H-H" && x.IdSeccion == "AT").OrderBy(y => y.Orden).ToList();
                    if (!isEqualATSidCo && model.AceptaCargarLaDataNueva)
                    {
                        data = new List<ResistDesignDTO>();
                    }
                    if (data.Any())//si hay data ponemos lo que viene de base de datos
                    {
                        model.ResistDesignHH.AddRange(data);
                    }
                    else
                    {
                        foreach (var posi in positions.Structure.AltaTension.Select((value, i) => new { i, value }).OrderBy(y => y.i))
                        {
                            model.ResistDesignHH.Add(new ResistDesignDTO
                            {
                                Id = posi.i + 1,
                                Posicion = posi.value,
                                Resistencia = 0
                            });
                        }
                    }

                    if (artifactDesing.Derivations.ConexionEquivalente is "WYE" or "ESTRELLA")
                    {
                        data = resistencias.Where(x => x.ConexionPrueba == "H-N" && x.IdSeccion == "AT").OrderBy(y => y.Orden).ToList();
                        if (!isEqualATSidCo && model.AceptaCargarLaDataNueva)
                        {
                            data = new List<ResistDesignDTO>();
                        }
                        if (data.Any())//si hay data ponemos lo que viene de base de datos
                        {
                            model.ResistDesignHN.AddRange(data);
                        }
                        else
                        {
                            foreach (var posi in positions.Structure.AltaTension.Select((value, i) => new { i, value }).OrderBy(y => y.i))
                            {
                                model.ResistDesignHN.Add(new ResistDesignDTO
                                {
                                    Id = posi.i + 1,
                                    Posicion = posi.value,
                                    Resistencia = 0
                                });
                            }
                        }

                        model.HN = true;
                    }

                    #endregion

                    #region Llenado de las BT
                    data = resistencias.Where(x => x.ConexionPrueba == "X-X" && x.IdSeccion == "BT").OrderBy(y => y.Orden).ToList();
                    if (!isEqualBTSidCo && model.AceptaCargarLaDataNueva)
                    {
                        data = new List<ResistDesignDTO>();
                    }
                    if (data.Any())//si hay data ponemos lo que viene de base de datos
                    {
                        model.ResistDesignXX.AddRange(data);
                    }
                    else
                    {
                        foreach (var posi in positions.Structure.BajaTension.Select((value, i) => new { i, value }).OrderBy(y => y.i))
                        {
                            model.ResistDesignXX.Add(new ResistDesignDTO
                            {
                                Id = posi.i + 1,
                                Posicion = posi.value,
                                Resistencia = 0
                            });

                        }
                    }

                    if (artifactDesing.Derivations.ConexionEquivalente_2 is "WYE" or "ESTRELLA")
                    {
                        model.XN = true;

                        data = resistencias.Where(x => x.ConexionPrueba == "X-N" && x.IdSeccion == "BT").OrderBy(y => y.Orden).ToList();
                        if (!isEqualBTSidCo && model.AceptaCargarLaDataNueva)
                        {
                            data = new List<ResistDesignDTO>();
                        }
                        if (data.Any())//si hay data ponemos lo que viene de base de datos
                        {
                            model.ResistDesignXN.AddRange(data);
                        }
                        else
                        {
                            foreach (var posi in positions.Structure.BajaTension.Select((value, i) => new { i, value }).OrderBy(y => y.i))
                            {
                                model.ResistDesignXN.Add(new ResistDesignDTO
                                {
                                    Id = posi.i + 1,
                                    Posicion = posi.value,
                                    Resistencia = 0
                                });
                            }
                        }
                    }

                    #endregion

                    #region Llenado de las Ter
                    data = resistencias.Where(x => x.ConexionPrueba == "Y-Y" && x.IdSeccion.ToUpper() == "TER").OrderBy(y => y.Orden).ToList();
                    if (!isEqualTerSidCo && model.AceptaCargarLaDataNueva)
                    {
                        data = new List<ResistDesignDTO>();
                    }
                    if (data.Any())//si hay data ponemos lo que viene de base de datos
                    {
                        model.ResistDesignYY.AddRange(data);
                    }
                    else
                    {
                        foreach (var posi in positions.Structure.Terciario.Select((value, i) => new { i, value }).OrderBy(y => y.i))
                        {
                            model.ResistDesignYY.Add(new ResistDesignDTO
                            {
                                Id = posi.i + 1,
                                Posicion = posi.value,
                                Resistencia = 0
                            });

                        }
                    }

                    if (artifactDesing.Derivations.ConexionEquivalente_4 is "WYE" or "ESTRELLA")
                    {
                        model.YN = true;
                        data = resistencias.Where(x => x.ConexionPrueba == "Y-N" && x.IdSeccion.ToUpper() == "TER").OrderBy(y => y.Orden).ToList();
                        if (!isEqualTerSidCo && model.AceptaCargarLaDataNueva)
                        {
                            data = new List<ResistDesignDTO>();
                        }

                        if (data.Any())//si hay data ponemos lo que viene de base de datos
                        {
                            model.ResistDesignYN.AddRange(data);
                        }
                        else
                        {
                            foreach (var posi in positions.Structure.Terciario.Select((value, i) => new { i, value }).OrderBy(y => y.i))
                            {
                                model.ResistDesignYN.Add(new ResistDesignDTO
                                {
                                    Id = posi.i + 1,
                                    Posicion = posi.value,
                                    Resistencia = 0
                                });

                            }
                        }
                    }

                    #endregion
                }
                else
                {

                    model.TipoAparato = "TRA";
                    #region LLenado de los DS de LL 
                    List<ResistDesignDTO> data = resistencias.Where(x => x.ConexionPrueba == "L-L" && x.IdSeccion == "AT").ToList();
                    if (!isEqualATSidCo && model.AceptaCargarLaDataNueva)
                    {
                        data = new List<ResistDesignDTO>();
                    }

                    if (data.Any())//si hay data ponemos lo que viene de base de datos
                    {
                        model.ResistDesignLLAT.AddRange(data);
                    }
                    else
                    {
                        foreach (var posi in positions.Structure.AltaTension.Select((value, i) => new { i, value }))
                        {
                            model.ResistDesignLLAT.Add(new ResistDesignDTO
                            {
                                Id = posi.i + 1,
                                Posicion = posi.value,
                                Resistencia = 0
                            });

                        }
                    }

                    data = resistencias.Where(x => x.ConexionPrueba == "L-L" && x.IdSeccion == "BT").ToList();
                    if (!isEqualBTSidCo && model.AceptaCargarLaDataNueva)
                    {
                        data = new List<ResistDesignDTO>();
                    }

                    if (data.Any())//si hay data ponemos lo que viene de base de datos
                    {
                        model.ResistDesignLLBT.AddRange(data);
                    }
                    else
                    {
                        foreach (var posi in positions.Structure.BajaTension.Select((value, i) => new { i, value }))
                        {
                            model.ResistDesignLLBT.Add(new ResistDesignDTO
                            {
                                Id = posi.i + 1,
                                Posicion = posi.value,
                                Resistencia = 0
                            });

                        }
                    }

                    data = resistencias.Where(x => x.ConexionPrueba == "L-L" && x.IdSeccion.ToUpper() == "TER").ToList();
                    if (!isEqualTerSidCo && model.AceptaCargarLaDataNueva)
                    {
                        data = new List<ResistDesignDTO>();
                    }
                    if (data.Any())//si hay data ponemos lo que viene de base de datos
                    {
                        model.ResistDesignLLTER.AddRange(data);
                    }
                    else
                    {
                        foreach (var posi in positions.Structure.Terciario.Select((value, i) => new { i, value }))
                        {
                            model.ResistDesignLLTER.Add(new ResistDesignDTO
                            {
                                Id = posi.i + 1,
                                Posicion = posi.value,
                                Resistencia = 0
                            });

                        }
                    }
                    #endregion

                    #region LLENADO DE LA ESTRELLA LN

                    if (artifactDesing.Derivations.ConexionEquivalente is "WYE" or "ESTRELLA")
                    {
                        data = resistencias.Where(x => x.ConexionPrueba == "L-N" && x.IdSeccion == "AT").ToList();
                        if (!isEqualATSidCo && model.AceptaCargarLaDataNueva)
                        {
                            data = new List<ResistDesignDTO>();
                        }

                        if (data.Any())//si hay data ponemos lo que viene de base de datos
                        {
                            model.ResistDesignLNAT.AddRange(data);
                        }
                        else
                        {
                            foreach (var posi in positions.Structure.AltaTension.Select((value, i) => new { i, value }))
                            {
                                model.ResistDesignLNAT.Add(new ResistDesignDTO
                                {
                                    Id = posi.i + 1,
                                    Posicion = posi.value,
                                    Resistencia = 0
                                });

                            }
                        }
                    }
                    if (artifactDesing.Derivations.ConexionEquivalente_2 is "WYE" or "ESTRELLA")
                    {

                        data = resistencias.Where(x => x.ConexionPrueba == "L-N" && x.IdSeccion == "BT").ToList();
                        if (!isEqualBTSidCo && model.AceptaCargarLaDataNueva)
                        {
                            data = new List<ResistDesignDTO>();
                        }

                        if (data.Any())//si hay data ponemos lo que viene de base de datos
                        {
                            model.ResistDesignLNBT.AddRange(data);
                        }
                        else
                        {
                            foreach (var posi in positions.Structure.BajaTension.Select((value, i) => new { i, value }))
                            {
                                model.ResistDesignLNBT.Add(new ResistDesignDTO
                                {
                                    Id = posi.i + 1,
                                    Posicion = posi.value,
                                    Resistencia = 0
                                });

                            }
                        }
                    }
                    if (artifactDesing.Derivations.ConexionEquivalente_4 is "WYE" or "ESTRELLA")
                    {

                        data = resistencias.Where(x => x.ConexionPrueba == "L-N" && x.IdSeccion.ToUpper() == "TER").ToList();
                        if (!isEqualTerSidCo && model.AceptaCargarLaDataNueva)
                        {
                            data = new List<ResistDesignDTO>();
                        }
                        if (data.Any())//si hay data ponemos lo que viene de base de datos
                        {
                            model.ResistDesignLNTER.AddRange(data);
                        }
                        else
                        {
                            foreach (var posi in positions.Structure.Terciario.Select((value, i) => new { i, value }))
                            {
                                model.ResistDesignLNTER.Add(new ResistDesignDTO
                                {
                                    Id = posi.i + 1,
                                    Posicion = posi.value,
                                    Resistencia = 0
                                });

                            }
                        }
                    }

                    #endregion
                }

                #region funcionalidad vieja
                //ResistanceTwentyDegreesViewModel resistanceTwentyDegreesViewModel = new();

                //string[] noSerieWo_ = noSerie.Trim().Split('-');

                //ApiResponse<PositionsDTO> dataPositionsPlateTension = await this._gatewayClientService.GetPositions(noSerie);

                //ApiResponse<List<ResistDesignDTO>> resistanceAllVoltageResponse = await this._resistanceTwentyDegressClientService.GetResistDesignDTO(noSerie, measuring, testConnection, temperature);

                //List<ResistDesignDTO> resistanceAllVoltageResult = resistanceAllVoltageResponse.Structure;

                //if (resistanceAllVoltageResult.Any())
                //{
                //    resistanceTwentyDegreesViewModel.ResistanceAT = resistanceAllVoltageResult.FindAll(c => c.IdSeccion.ToUpper().Equals("AT"));
                //    resistanceTwentyDegreesViewModel.ResistanceBT = resistanceAllVoltageResult.FindAll(c => c.IdSeccion.ToUpper().Equals("BT"));
                //    resistanceTwentyDegreesViewModel.ResistanceTER = resistanceAllVoltageResult.FindAll(c => c.IdSeccion.ToUpper().Equals("TER"));
                //}

                ////ApiResponse<CharacteristicsPlaneTensionDTO> characteristicPlateTensionResponse = await this._artifactClientService.GetCharacteristics(noSerieWo_[0]);

                ////#region Validate if exist any characteristics or other error
                ////if (characteristicPlateTensionResponse.Code.Equals(-1))
                ////{
                ////    return this.Json(new { response = characteristicPlateTensionResponse });
                ////}
                ////else
                ////{
                ////    resistanceTwentyDegreesViewModel.CharacteristicsPlaneTension = characteristicPlateTensionResponse.Structure;
                ////}
                ////#endregion

                ////ApiResponse<TapBaanDTO> tapbaanPlateTensionResponse = await this._artifactClientService.GetTapBaanPlateTension(noSerieWo_[0]);

                ////#region Validate if exist any tapbaan or other error
                ////if (tapbaanPlateTensionResponse.Code.Equals(-1))
                ////{
                ////    return this.Json(new { response = tapbaanPlateTensionResponse });
                ////}
                ////else
                ////{
                ////    resistanceTwentyDegreesViewModel.TapBaan = tapbaanPlateTensionResponse.Structure;
                ////}
                ////#endregion

                ////#region GetPositions

                ////decimal? position = 1;
                ////#region Position with out charge

                ////resistanceTwentyDegreesViewModel.PositionTapBaan = new PositionTensionPlateDTO
                ////{
                ////    NominalAT = "1",
                ////    PositionAT = "1",
                ////    NominalBT = "1",
                ////    PositionBT = "1",
                ////    NominalTER = "1",
                ////    PositionTER = "1"
                ////};

                ////position = resistanceTwentyDegreesViewModel.TapBaan.CantidadSupSc + resistanceTwentyDegreesViewModel.TapBaan.CantidadInfSc + 1;
                ////this.ShowPositionValues(position, 0, false, ref resistanceTwentyDegreesViewModel);

                ////#endregion

                ////#region Position with charge

                ////position = resistanceTwentyDegreesViewModel.TapBaan.CantidadSupBc + resistanceTwentyDegreesViewModel.TapBaan.CantidadInfBc + 1;
                ////this.ShowPositionValues(position, 1, true, ref resistanceTwentyDegreesViewModel);

                ////#endregion

                ////#endregion

                //#region Validate positions
                //if (dataPositionsPlateTension.Structure==null  )
                //{
                //    return this.Json(new
                //    {

                //        response = new ApiResponse<ResistanceTwentyDegreesViewModel>
                //        {
                //            Code = dataPositionsPlateTension.Code,
                //            Description = dataPositionsPlateTension.Description,
                //            Structure = resistanceTwentyDegreesViewModel
                //        }
                //    });
                //}
                //decimal PosAtPlateTension = dataPositionsPlateTension.Structure.AltaTension.Count();
                //decimal PosBtPlateTension = dataPositionsPlateTension.Structure.BajaTension.Count();
                //decimal PosTerPlateTension = dataPositionsPlateTension.Structure.Terciario.Count();

                //resistanceTwentyDegreesViewModel.PositionValidate = this._plateTensionService.ValidatePositions(resistanceAllVoltageResult, PosAtPlateTension, PosBtPlateTension, PosTerPlateTension, out bool isNewTension);

                ////this.habilitarGrids(ref resistanceTwentyDegreesViewModel);

                //if (isNewTension && !newTensonResponse.HasValue)
                //{
                //    ApiResponse<bool> response = new()
                //    {
                //        Code = 2,
                //        Description = "NEW_TENSION",
                //        Structure = isNewTension
                //    };

                //    return this.Json(new { response });
                //}
                //#endregion
                //if (newTensonResponse.HasValue && !newTensonResponse.Value)
                //{
                //    resistanceTwentyDegreesViewModel.PositionValidate = false;
                //}

                //if (resistanceTwentyDegreesViewModel.PositionValidate)
                //{
                //    resistanceTwentyDegreesViewModel.LoadNewTension = true;
                //    //this.CargarInfoGrid(isNewTension, ref resistanceTwentyDegreesViewModel);
                //    #region Update positions

                //    resistanceTwentyDegreesViewModel.ResistanceAT = new List<ResistDesignDTO>();
                //    resistanceTwentyDegreesViewModel.ResistanceBT = new List<ResistDesignDTO>();
                //    resistanceTwentyDegreesViewModel.ResistanceTER = new List<ResistDesignDTO>();
                //    foreach (string item in dataPositionsPlateTension.Structure.AltaTension)
                //    {
                //        ResistDesignDTO resist = new() { Posicion = item, Resistencia = 0, IdSeccion = "AT", N = item.Equals(dataPositionsPlateTension.Structure.ATNom)};
                //        resistanceTwentyDegreesViewModel.ResistanceAT.Add(resist);
                //    }

                //    foreach (string item in dataPositionsPlateTension.Structure.BajaTension)
                //    {
                //        ResistDesignDTO resist = new() { Posicion = item, Resistencia = 0, IdSeccion = "BT", N = item.Equals(dataPositionsPlateTension.Structure.BTNom) };
                //        resistanceTwentyDegreesViewModel.ResistanceBT.Add(resist);
                //    }

                //    foreach (string item in dataPositionsPlateTension.Structure.Terciario)
                //    {
                //        ResistDesignDTO resist = new() { Posicion = item, Resistencia = 0, IdSeccion = "TER", N = item.Equals(dataPositionsPlateTension.Structure.TerNom) };
                //        resistanceTwentyDegreesViewModel.ResistanceTER.Add(resist);
                //    }

                //    resistanceTwentyDegreesViewModel.Positions = new PositionTensionPlateDTO
                //    {
                //        PositionAT = resistanceTwentyDegreesViewModel.ResistanceAT.Count.Equals(0) ? "1" : resistanceTwentyDegreesViewModel.ResistanceAT.Count.ToString(),
                //        PositionBT = resistanceTwentyDegreesViewModel.ResistanceBT.Count.Equals(0) ? "1" : resistanceTwentyDegreesViewModel.ResistanceBT.Count.ToString(),
                //        PositionTER = resistanceTwentyDegreesViewModel.ResistanceTER.Count.Equals(0) ? "1" : resistanceTwentyDegreesViewModel.ResistanceTER.Count.ToString()
                //    };

                //    resistanceTwentyDegreesViewModel.PositionTapBaan.NominalAT = dataPositionsPlateTension.Structure.ATNom;
                //    resistanceTwentyDegreesViewModel.PositionTapBaan.NominalBT = dataPositionsPlateTension.Structure.BTNom;
                //    resistanceTwentyDegreesViewModel.PositionTapBaan.NominalTER = dataPositionsPlateTension.Structure.TerNom;

                //    #endregion

                //    this.CargarInfoTension(ref resistanceTwentyDegreesViewModel);
                //}
                //else
                //{
                //    resistanceTwentyDegreesViewModel.LoadNewTension = false;

                //    #region Update positions
                //    resistanceTwentyDegreesViewModel.ResistanceAT = resistanceAllVoltageResult.FindAll(c => c.IdSeccion.ToUpper().Trim().Equals("AT"));
                //    resistanceTwentyDegreesViewModel.ResistanceBT = resistanceAllVoltageResult.FindAll(c => c.IdSeccion.ToUpper().Trim().Equals("BT"));
                //    resistanceTwentyDegreesViewModel.ResistanceTER = resistanceAllVoltageResult.FindAll(c => c.IdSeccion.ToUpper().Trim().Equals("TER"));

                //    resistanceTwentyDegreesViewModel.ResistanceAT.ForEach(x => x.N = x.Posicion.Equals(dataPositionsPlateTension.Structure.ATNom));
                //    resistanceTwentyDegreesViewModel.ResistanceBT.ForEach(x => x.N = x.Posicion.Equals(dataPositionsPlateTension.Structure.BTNom));
                //    resistanceTwentyDegreesViewModel.ResistanceTER.ForEach(x => x.N = x.Posicion.Equals(dataPositionsPlateTension.Structure.TerNom));

                //    resistanceTwentyDegreesViewModel.Positions = new PositionTensionPlateDTO
                //    {
                //        PositionAT = resistanceTwentyDegreesViewModel.ResistanceAT.Count.Equals(0) ? "1" : resistanceTwentyDegreesViewModel.ResistanceAT.Count.ToString(),
                //        PositionBT = resistanceTwentyDegreesViewModel.ResistanceBT.Count.Equals(0) ? "1" : resistanceTwentyDegreesViewModel.ResistanceBT.Count.ToString(),
                //        PositionTER = resistanceTwentyDegreesViewModel.ResistanceTER.Count.Equals(0) ? "1" : resistanceTwentyDegreesViewModel.ResistanceTER.Count.ToString()
                //    };

                //    resistanceTwentyDegreesViewModel.PositionTapBaan.NominalAT = resistanceAllVoltageResult.Exists(c => c.IdSeccion.ToUpper().Trim().Equals("AT") && c.N) ? resistanceAllVoltageResult.FirstOrDefault(c => c.IdSeccion.ToUpper().Trim().Equals("AT") && c.N).Posicion : string.Empty ;
                //    resistanceTwentyDegreesViewModel.PositionTapBaan.NominalBT = resistanceAllVoltageResult.Exists(c => c.IdSeccion.ToUpper().Trim().Equals("BT") && c.N) ? resistanceAllVoltageResult.FirstOrDefault(c => c.IdSeccion.ToUpper().Trim().Equals("BT") && c.N).Posicion : string.Empty;
                //    resistanceTwentyDegreesViewModel.PositionTapBaan.NominalTER = resistanceAllVoltageResult.Exists(c => c.IdSeccion.ToUpper().Trim().Equals("TER") && c.N) ? resistanceAllVoltageResult.FirstOrDefault(c => c.IdSeccion.ToUpper().Trim().Equals("TER") && c.N).Posicion : string.Empty;

                //    #endregion

                //    this.CargarInfoTension(ref resistanceTwentyDegreesViewModel);

                //}

                //resistanceTwentyDegreesViewModel.PositionTapBaan.PositionAT = dataPositionsPlateTension.Structure.AltaTension.Count().ToString();
                //resistanceTwentyDegreesViewModel.PositionTapBaan.PositionBT = dataPositionsPlateTension.Structure.BajaTension.Count().ToString();
                //resistanceTwentyDegreesViewModel.PositionTapBaan.PositionTER = dataPositionsPlateTension.Structure.Terciario.Count().ToString();
                #endregion
                return Json(new
                {

                    response = new ApiResponse<ResistanceTwentyDegreesViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = model
                    }
                });
            }
            catch (Exception e)
            {
                return Json(new
                {

                    response = new ApiResponse<ResistanceTwentyDegreesViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = null
                    }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResistanceTwentyDegreesViewModel viewModel)
        {

            List<ResistDesignDTO> resistances = new();
            List<ResistDesignDTO> resistancesToPost = new();
            DateTime myDate = new(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            if (viewModel.ResistDesignHX.Count > 0)
            {
                foreach (ResistDesignDTO item in viewModel.ResistDesignHX)
                {
                    resistances.Add(new ResistDesignDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = null,
                        Modificadopor = null,
                        Orden = item.Orden,
                        IdSeccion = item.IdSeccion,
                        NoSerie = item.NoSerie,
                        Posicion = item.Posicion,
                        Resistencia = item.Resistencia,
                        ConexionPrueba = item.ConexionPrueba,
                        Temperatura = item.Temperatura,
                        UnidadMedida = item.UnidadMedida
                    });

                }
            }

            if (viewModel.ResistDesignHN.Count > 0)
            {
                foreach (ResistDesignDTO item in viewModel.ResistDesignHN)
                {
                    resistances.Add(new ResistDesignDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = null,
                        Modificadopor = null,
                        Orden = item.Orden,
                        IdSeccion = item.IdSeccion,
                        NoSerie = item.NoSerie,
                        Posicion = item.Posicion,
                        Resistencia = item.Resistencia,
                        ConexionPrueba = item.ConexionPrueba,
                        Temperatura = item.Temperatura,
                        UnidadMedida = item.UnidadMedida
                    });

                }
            }

            if (viewModel.ResistDesignHH.Count > 0)
            {
                foreach (ResistDesignDTO item in viewModel.ResistDesignHH)
                {
                    resistances.Add(new ResistDesignDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = null,
                        Modificadopor = null,
                        Orden = item.Orden,
                        IdSeccion = item.IdSeccion,
                        NoSerie = item.NoSerie,
                        Posicion = item.Posicion,
                        Resistencia = item.Resistencia,
                        ConexionPrueba = item.ConexionPrueba,
                        Temperatura = item.Temperatura,
                        UnidadMedida = item.UnidadMedida
                    });

                }
            }

            if (viewModel.ResistDesignXX.Count > 0)
            {
                foreach (ResistDesignDTO item in viewModel.ResistDesignXX)
                {
                    resistances.Add(new ResistDesignDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = null,
                        Modificadopor = null,
                        Orden = item.Orden,
                        IdSeccion = item.IdSeccion,
                        NoSerie = item.NoSerie,
                        Posicion = item.Posicion,
                        Resistencia = item.Resistencia,
                        ConexionPrueba = item.ConexionPrueba,
                        Temperatura = item.Temperatura,
                        UnidadMedida = item.UnidadMedida
                    });

                }
            }

            if (viewModel.ResistDesignXN.Count > 0)
            {
                foreach (ResistDesignDTO item in viewModel.ResistDesignXN)
                {
                    resistances.Add(new ResistDesignDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = null,
                        Modificadopor = null,
                        Orden = item.Orden,
                        IdSeccion = item.IdSeccion,
                        NoSerie = item.NoSerie,
                        Posicion = item.Posicion,
                        Resistencia = item.Resistencia,
                        ConexionPrueba = item.ConexionPrueba,
                        Temperatura = item.Temperatura,
                        UnidadMedida = item.UnidadMedida
                    });

                }
            }

            if (viewModel.ResistDesignYY.Count > 0)
            {
                foreach (ResistDesignDTO item in viewModel.ResistDesignYY)
                {
                    resistances.Add(new ResistDesignDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = null,
                        Modificadopor = null,
                        Orden = item.Orden,
                        IdSeccion = item.IdSeccion,
                        NoSerie = item.NoSerie,
                        Posicion = item.Posicion,
                        Resistencia = item.Resistencia,
                        ConexionPrueba = item.ConexionPrueba,
                        Temperatura = item.Temperatura,
                        UnidadMedida = item.UnidadMedida
                    });

                }
            }

            if (viewModel.ResistDesignYN.Count > 0)
            {
                foreach (ResistDesignDTO item in viewModel.ResistDesignYN)
                {
                    resistances.Add(new ResistDesignDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = null,
                        Modificadopor = null,
                        Orden = item.Orden,
                        IdSeccion = item.IdSeccion,
                        NoSerie = item.NoSerie,
                        Posicion = item.Posicion,
                        Resistencia = item.Resistencia,
                        ConexionPrueba = item.ConexionPrueba,
                        Temperatura = item.Temperatura,
                        UnidadMedida = item.UnidadMedida
                    });

                }
            }

            if (viewModel.ResistDesignLLAT.Count > 0)
            {
                foreach (ResistDesignDTO item in viewModel.ResistDesignLLAT)
                {
                    resistances.Add(new ResistDesignDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = null,
                        Modificadopor = null,
                        Orden = item.Orden,
                        IdSeccion = item.IdSeccion,
                        NoSerie = item.NoSerie,
                        Posicion = item.Posicion,
                        Resistencia = item.Resistencia,
                        ConexionPrueba = item.ConexionPrueba,
                        Temperatura = item.Temperatura,
                        UnidadMedida = item.UnidadMedida
                    });

                }
            }

            if (viewModel.ResistDesignLLBT.Count > 0)
            {
                foreach (ResistDesignDTO item in viewModel.ResistDesignLLBT)
                {
                    resistances.Add(new ResistDesignDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = null,
                        Modificadopor = null,
                        Orden = item.Orden,
                        IdSeccion = item.IdSeccion,
                        NoSerie = item.NoSerie,
                        Posicion = item.Posicion,
                        Resistencia = item.Resistencia,
                        ConexionPrueba = item.ConexionPrueba,
                        Temperatura = item.Temperatura,
                        UnidadMedida = item.UnidadMedida
                    });

                }
            }

            if (viewModel.ResistDesignLLTER.Count > 0)
            {
                foreach (ResistDesignDTO item in viewModel.ResistDesignLLTER)
                {
                    resistances.Add(new ResistDesignDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = null,
                        Modificadopor = null,
                        Orden = item.Orden,
                        IdSeccion = item.IdSeccion,
                        NoSerie = item.NoSerie,
                        Posicion = item.Posicion,
                        Resistencia = item.Resistencia,
                        ConexionPrueba = item.ConexionPrueba,
                        Temperatura = item.Temperatura,
                        UnidadMedida = item.UnidadMedida
                    });

                }
            }

            if (viewModel.ResistDesignLNAT.Count > 0)
            {
                foreach (ResistDesignDTO item in viewModel.ResistDesignLNAT)
                {
                    resistances.Add(new ResistDesignDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = null,
                        Modificadopor = null,
                        Orden = item.Orden,
                        IdSeccion = item.IdSeccion,
                        NoSerie = item.NoSerie,
                        Posicion = item.Posicion,
                        Resistencia = item.Resistencia,
                        ConexionPrueba = item.ConexionPrueba,
                        Temperatura = item.Temperatura,
                        UnidadMedida = item.UnidadMedida
                    });

                }
            }

            if (viewModel.ResistDesignLNBT.Count > 0)
            {
                foreach (ResistDesignDTO item in viewModel.ResistDesignLNBT)
                {
                    resistances.Add(new ResistDesignDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = null,
                        Modificadopor = null,
                        Orden = item.Orden,
                        IdSeccion = item.IdSeccion,
                        NoSerie = item.NoSerie,
                        Posicion = item.Posicion,
                        Resistencia = item.Resistencia,
                        ConexionPrueba = item.ConexionPrueba,
                        Temperatura = item.Temperatura,
                        UnidadMedida = item.UnidadMedida
                    });

                }
            }

            if (viewModel.ResistDesignLNTER.Count > 0)
            {
                foreach (ResistDesignDTO item in viewModel.ResistDesignLNTER)
                {
                    resistances.Add(new ResistDesignDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = null,
                        Modificadopor = null,
                        Orden = item.Orden,
                        IdSeccion = item.IdSeccion,
                        NoSerie = item.NoSerie,
                        Posicion = item.Posicion,
                        Resistencia = item.Resistencia,
                        ConexionPrueba = item.ConexionPrueba,
                        Temperatura = item.Temperatura,
                        UnidadMedida = item.UnidadMedida
                    });

                }
            }

            _ = resistances.RemoveAll(x => x.Resistencia == 0);

            int globalUnit = 1000;
            CultureInfo culture = new("en-US");

            IEnumerable<ResistDesignDTO> convertedResistances = resistances.Select(x =>
            {
                decimal convertedResult = 0;
                string convertedUnit = string.Empty;
                if (x.UnidadMedida == "Ohms")
                {
                    convertedResult = x.Resistencia * globalUnit;
                    convertedUnit = "Miliohms";
                }
                else if (x.UnidadMedida == "Miliohms")
                {
                    convertedResult = x.Resistencia / globalUnit;
                    if (convertedResult < 0.0001m)
                        convertedResult = 0.0001m;

                    convertedUnit = "Ohms";
                }

                //var splitedValue = CommonMethods.SplitDouble(Convert.ToDouble(convertedResult), 3, decimalPlaces);
                //var intPart = splitedValue[0];
                //var decimalPart = splitedValue[1];
                //var roundedResult = Convert.ToDecimal($"{intPart}.{decimalPart}", culture);

                return new ResistDesignDTO
                {
                    Creadopor = x.Creadopor,
                    Fechacreacion = x.Fechacreacion,
                    Fechamodificacion = x.Fechamodificacion,
                    Modificadopor = x.Modificadopor,
                    Orden = x.Orden,
                    IdSeccion = x.IdSeccion,
                    NoSerie = x.NoSerie,
                    Posicion = x.Posicion,
                    Resistencia = convertedResult,
                    ConexionPrueba = x.ConexionPrueba,
                    Temperatura = x.Temperatura,
                    UnidadMedida = convertedUnit
                };
            });

            if (resistances != null && resistances.Any())
                resistancesToPost.AddRange(resistances);

            if (convertedResistances != null && convertedResistances.Any())
                resistancesToPost.AddRange(convertedResistances);

            JsonSerializerOptions options = new() { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(resistancesToPost, options);

            if (resistancesToPost.Count() == 0)
            {
                return Json(new
                {
                    response = new ApiResponse<long>
                    {
                        Structure = 0,
                        Code = 1,
                        Description = ""

                    }
                });
            }
            else
            {
                resistancesToPost.ForEach(x => x.Modificadopor = User.Identity.Name);
                resistancesToPost.ForEach(x => x.Fechamodificacion = DateTime.Now);
                ApiResponse<long> result = await _artifactClientService.AddResistanceDesign(resistancesToPost);
                return Json(new
                {
                    response = result
                });
            }
        }

        #region Private Methods
        public void ShowPositionValues(decimal? position, decimal conditionOpId, bool charge, ref ResistanceTwentyDegreesViewModel viewModel)
        {

            switch (charge ? viewModel.TapBaan.ComboNumericBc : viewModel.TapBaan.ComboNumericSc)
            {
                case 3:
                case 5:
                    viewModel.PositionTapBaan.PositionAT = position.ToString();
                    viewModel.PositionTapBaan.NominalAT = charge ? viewModel.TapBaan.NominalBc.ToString() : viewModel.TapBaan.NominalSc.ToString();
                    viewModel.PositionTapBaan.IdentificationAT = charge ? viewModel.TapBaan.IdentificacionBc : viewModel.TapBaan.IdentificacionSc;
                    viewModel.PositionTapBaan.IdConditionOpAT = conditionOpId;
                    break;
                case 2:
                case 4:
                    viewModel.PositionTapBaan.PositionBT = position.ToString();
                    viewModel.PositionTapBaan.NominalBT = charge ? viewModel.TapBaan.NominalBc.ToString() : viewModel.TapBaan.NominalSc.ToString();
                    viewModel.PositionTapBaan.IdentificationBT = charge ? viewModel.TapBaan.IdentificacionBc : viewModel.TapBaan.IdentificacionSc; ;
                    viewModel.PositionTapBaan.IdConditionOpBT = conditionOpId;
                    break;
                case 1:
                    viewModel.PositionTapBaan.PositionTER = position.ToString();
                    viewModel.PositionTapBaan.NominalTER = charge ? viewModel.TapBaan.NominalBc.ToString() : viewModel.TapBaan.NominalSc.ToString();
                    viewModel.PositionTapBaan.IdentificationTER = charge ? viewModel.TapBaan.IdentificacionBc : viewModel.TapBaan.IdentificacionSc; ;
                    viewModel.PositionTapBaan.IdConditionOpTER = conditionOpId;
                    break;
                default:
                    break;
            }
        }

        public void CargarInfoGrid(bool isNewTension, ref ResistanceTwentyDegreesViewModel viewModel)
        {
            //CARGAR INFO SIN CARGA
            CargarPosiciones("AT", isNewTension, ref viewModel);
            CargarPosiciones("BT", isNewTension, ref viewModel);
            CargarPosiciones("TER", isNewTension, ref viewModel);

        }

        public void CargarInfoTension(ref ResistanceTwentyDegreesViewModel viewModel)
        {
            CargarTensionGrid("AT", ref viewModel);
            CargarTensionGrid("BT", ref viewModel);
            CargarTensionGrid("TER", ref viewModel);
        }

        public void CargarTensionGrid(string nomGrid, ref ResistanceTwentyDegreesViewModel viewModel)
        {
            List<ResistDesignDTO> lstTensionPlaca;

            switch (nomGrid)
            {
                case "AT":
                    valTension = 0;//this.tensionAT;
                    lstTensionPlaca = viewModel.ResistanceAT;
                    if (lstTensionPlaca.Count > 0)
                    {
                        LlenarDataTable(lstTensionPlaca);
                    }
                    else
                    {
                        //viewModel.PositionTapBaan.NominalAT = "1";
                        //viewModel.PositionTapBaan.IdentificationAT = -1;
                        //_ = int.TryParse(viewModel.PositionTapBaan.PositionAT, out int positionAT);
                        //_ = int.TryParse(viewModel.PositionTapBaan.NominalAT, out int nominalAT);
                        //this.ObtenerPosicionesGrid(viewModel.PositionTapBaan.IdentificationAT, viewModel.PositionTapBaan.IdConditionOpAT, positionAT, nominalAT, ref viewModel);
                    }
                    CargarGrid(0, "AT", ref viewModel);
                    break;
                case "BT":
                    valTension = 0;//this.tensionBT;
                    lstTensionPlaca = viewModel.ResistanceBT;
                    if (lstTensionPlaca.Count > 0)
                    {
                        LlenarDataTable(lstTensionPlaca);
                    }
                    else
                    {
                        //viewModel.PositionTapBaan.NominalBT = "1";
                        //viewModel.PositionTapBaan.IdentificationBT = -1;
                        //_ = int.TryParse(viewModel.PositionTapBaan.PositionAT, out int positionAT);
                        //_ = int.TryParse(viewModel.PositionTapBaan.NominalAT, out int nominalAT);
                        //this.ObtenerPosicionesGrid(viewModel.PositionTapBaan.IdentificationBT, viewModel.PositionTapBaan.IdConditionOpAT, positionAT, nominalAT, ref viewModel);
                    }
                    CargarGrid(0, "BT", ref viewModel);
                    break;
                case "TER":
                    valTension = 0;//this.tensionTER;
                    lstTensionPlaca = viewModel.ResistanceTER;
                    if (lstTensionPlaca.Count > 0)
                    {
                        LlenarDataTable(lstTensionPlaca);
                    }
                    else
                    {

                        //viewModel.PositionTapBaan.NominalTER = "1";
                        //viewModel.PositionTapBaan.IdentificationTER = -1;
                        //_ = int.TryParse(viewModel.PositionTapBaan.PositionAT, out int positionAT);
                        //_ = int.TryParse(viewModel.PositionTapBaan.NominalAT, out int nominalAT);
                        //this.ObtenerPosicionesGrid(viewModel.PositionTapBaan.IdentificationTER, viewModel.PositionTapBaan.IdConditionOpAT, positionAT, nominalAT, ref viewModel);
                    }

                    CargarGrid(0, "TER", ref viewModel);
                    break;
            }
        }

        public void CargarPosiciones(string nomGrid, bool isNewTension, ref ResistanceTwentyDegreesViewModel viewModel)
        {
            List<ResistDesignDTO> lstTensionPlaca;

            switch (nomGrid)
            {
                case "AT":
                    valTension = 0;//this.tensionAT;
                    _ = int.TryParse(viewModel.PositionTapBaan.PositionAT, out int atPosition);
                    _ = int.TryParse(viewModel.PositionTapBaan.NominalAT, out int atNominal);
                    obtenerPosicionesGrid(viewModel.PositionTapBaan.IdentificationAT, viewModel.PositionTapBaan.IdConditionOpAT, atPosition, atNominal, ref viewModel);
                    //lstTensionPlaca = obtenerTensionKV(noSerie, "AT");
                    lstTensionPlaca = viewModel.ResistanceAT;
                    if (lstTensionPlaca != null && lstTensionPlaca.Any())
                        LlenarDataTableMerge(lstTensionPlaca, isNewTension);
                    CargarGrid(invertidoGrid, "AT", ref viewModel);
                    break;
                case "BT":
                    valTension = 0;//this.tensionBT;
                    _ = int.TryParse(viewModel.PositionTapBaan.PositionBT, out int btPosition);
                    _ = int.TryParse(viewModel.PositionTapBaan.NominalBT, out int btNominal);
                    obtenerPosicionesGrid(viewModel.PositionTapBaan.IdentificationBT, viewModel.PositionTapBaan.IdConditionOpBT, btPosition, btNominal, ref viewModel);
                    //lstTensionPlaca = obtenerTensionKV(noSerie, "BT");
                    lstTensionPlaca = viewModel.ResistanceBT;
                    if (lstTensionPlaca != null && lstTensionPlaca.Any())
                        LlenarDataTableMerge(lstTensionPlaca, isNewTension);
                    CargarGrid(invertidoGrid, "BT", ref viewModel);
                    break;
                case "TER":
                    valTension = 0;//this.tensionTER;
                    _ = int.TryParse(viewModel.PositionTapBaan.PositionTER, out int terPosition);
                    _ = int.TryParse(viewModel.PositionTapBaan.NominalBT, out int terNominal);
                    obtenerPosicionesGrid(viewModel.PositionTapBaan.IdentificationTER, viewModel.PositionTapBaan.IdConditionOpTER, terPosition, terNominal, ref viewModel);
                    //lstTensionPlaca = obtenerTensionKV(noSerie, "TER");
                    lstTensionPlaca = viewModel.ResistanceTER;
                    if (lstTensionPlaca != null && lstTensionPlaca.Any())
                        LlenarDataTableMerge(lstTensionPlaca, isNewTension);
                    CargarGrid(invertidoGrid, "TER", ref viewModel);
                    break;
            }
        }

        public void ObtenerPosicionesGrid(decimal? idIdentificacion, decimal idCondicionOP, int posiciones, int posNominal, ref ResistanceTwentyDegreesViewModel viewModel)
        {
            try
            {
                invertidoGrid = (int)(idCondicionOP == 0 ? viewModel.TapBaan.InvertidoSc : idCondicionOP == 1 ? viewModel.TapBaan.InvertidoBc : 0);
                dtValorNom = new DataTable();
                CrearDataTable();
                switch (idIdentificacion)
                {
                    case 1:
                        ObtenerLetras(posiciones, posNominal);
                        break;
                    case 2:
                        ObtenerNumero(posiciones, posNominal);
                        break;
                    case 3:
                        ObtenerRandLsup(idCondicionOP, posNominal, viewModel);
                        break;
                    case 4:
                        ObtenerRandLdown(idCondicionOP, posNominal, viewModel);
                        break;
                    default:
                        ObtenerPosicionN(idCondicionOP);
                        break;
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio u error al obtener las posiciones del grid.");
            }
        }
        public void LlenarDataTable(List<ResistDesignDTO> lstTensionPlaca)
        {
            try
            {
                dtValorNom = new DataTable();
                CrearDataTable();

                foreach (ResistDesignDTO tens in lstTensionPlaca.OrderBy(x => x.Orden))
                {
                    DataRow dtRow = dtValorNom.NewRow();
                    dtRow["POSICION"] = tens.Posicion;
                    dtRow["RESISTANCE"] = tens.Resistencia;
                    dtRow["Existente"] = true;
                    dtValorNom.Rows.Add(dtRow);
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al cargar la información del grid.");
            }
        }

        public void obtenerPosicionesGrid(decimal? idIdentificacion, decimal idCondicionOP, int posiciones, int posNominal, ref ResistanceTwentyDegreesViewModel viewModel)
        {
            try
            {
                invertidoGrid = (int)(idCondicionOP == 0 ? viewModel.TapBaan.InvertidoSc : idCondicionOP == 1 ? viewModel.TapBaan.InvertidoBc : 0);
                dtValorNom = new DataTable();
                CrearDataTable();
                switch (idIdentificacion)
                {
                    case 1:
                        ObtenerLetras(posiciones, posNominal);
                        break;
                    case 2:
                        ObtenerNumero(posiciones, posNominal);
                        break;
                    case 3:
                        ObtenerRandLsup(idCondicionOP, posNominal, viewModel);
                        break;
                    case 4:
                        ObtenerRandLdown(idCondicionOP, posNominal, viewModel);
                        break;
                    default:
                        ObtenerPosicionN(idCondicionOP);
                        break;
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio u error al obtener las posiciones del grid.");
            }
        }

        public void CargarGrid(int? invertido, string idGrid, ref ResistanceTwentyDegreesViewModel viewModel)
        {
            try
            {
                if (invertido == 1)
                {
                    IEnumerable<DataRow> orderedRows = from row in dtValorNom.AsEnumerable().Reverse() select row;
                    dtValorNom = orderedRows.CopyToDataTable();
                }
                idGrid = idGrid.ToUpper();
                switch (idGrid)
                {
                    case "AT":
                        viewModel.TableAT = dtValorNom;
                        break;
                    case "BT":
                        viewModel.TableBT = dtValorNom;
                        break;
                    case "TER":
                        viewModel.TableTER = dtValorNom;
                        break;
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al cargar la información del grid.");
            }
        }

        public void LlenarDataTableMerge(List<ResistDesignDTO> lstTensionPlaca, bool isNewTension)
        {
            try
            {
                ResistDesignDTO tensionPlaca;
                int i = invertidoGrid == 1 ? lstTensionPlaca.Count : 1;
                foreach (DataRow dt in dtValorNom.Rows)
                {
                    int numOrden = i;
                    tensionPlaca = new ResistDesignDTO();
                    tensionPlaca = lstTensionPlaca.Find(x => x.Orden == Convert.ToDecimal(numOrden));
                    if (tensionPlaca != null)
                    {
                        dt["RESISTANCE"] = tensionPlaca.Resistencia;
                        dt["Existente"] = isNewTension;
                    }
                    i = invertidoGrid == 1 ? i - 1 : i + 1;
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al cargar la información del grid.");
            }
        }

        public void CrearDataTable()
        {
            try
            {
                _ = dtValorNom.Columns.Add("POSICION");
                _ = dtValorNom.Columns.Add("RESISTANCE");
                _ = dtValorNom.Columns.Add("Existente");
                _ = dtValorNom.Columns.Add("idIdentificador");
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al crear data table.");
            }
        }
        public void ObtenerNumero(int posiciones, int posNominal)
        {
            try
            {
                for (int iterador = 0; iterador < posiciones; iterador++)
                {
                    DataRow dtRow = dtValorNom.NewRow();
                    dtRow["POSICION"] = iterador + 1;
                    dtRow["idIdentificador"] = idIdentificacion;
                    if (iterador + 1 == posNominal)
                    {
                        dtRow["RESISTANCE"] = string.Format("{0:0.###}", valTension);
                    }
                    dtValorNom.Rows.Add(dtRow);
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al obtener las posiciones");
            }
        }

        public void ObtenerLetras(int posiciones, int posNominal)
        {
            try
            {
                for (int iterador = 0; iterador < posiciones; iterador++)
                {
                    DataRow dtRow = dtValorNom.NewRow();
                    dtRow["POSICION"] = alpha[iterador].ToString();
                    dtRow["idIdentificador"] = idIdentificacion;
                    if (iterador + 1 == posNominal)
                    {
                        dtRow["RESISTANCE"] = string.Format("{0:0.###}", valTension);
                    }
                    dtValorNom.Rows.Add(dtRow);
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al obtener las posiciones");
            }
        }

        public void ObtenerRandLsup(decimal idCondicionOP, decimal posNominal, ResistanceTwentyDegreesViewModel viewModel)
        {
            try
            {
                int capasUp = idCondicionOP == 0 ? int.Parse(viewModel.TapBaan.CantidadSupSc.ToString()) : int.Parse(viewModel.TapBaan.CantidadSupBc.ToString());
                int capasDown = idCondicionOP == 0 ? int.Parse(viewModel.TapBaan.CantidadInfSc.ToString()) : int.Parse(viewModel.TapBaan.CantidadInfBc.ToString());
                int i = 1;
                for (int iterador = capasUp; iterador > 0; iterador--)
                {
                    DataRow dtRow = dtValorNom.NewRow();
                    dtRow["POSICION"] = iterador + "R";
                    dtRow["idIdentificador"] = idIdentificacion;
                    if (i == posNominal)
                    {
                        dtRow["RESISTANCE"] = string.Format("{0:0.###}", valTension);
                    }
                    dtValorNom.Rows.Add(dtRow);
                    i++;
                }

                DataRow dtNom = dtValorNom.NewRow();
                dtNom["POSICION"] = "NOM";
                dtNom["idIdentificador"] = idIdentificacion;
                if (i == posNominal)
                {
                    dtNom["RESISTANCE"] = string.Format("{0:0.###}", valTension);
                }
                dtValorNom.Rows.Add(dtNom);
                i++;

                for (int iterador = 0; iterador < capasDown; iterador++)
                {
                    DataRow dtRow = dtValorNom.NewRow();
                    dtRow["POSICION"] = iterador + 1 + "L";
                    dtRow["idIdentificador"] = idIdentificacion;
                    if (i == posNominal)
                    {
                        dtRow["RESISTANCE"] = string.Format("{0:0.###}", valTension);
                    }
                    dtValorNom.Rows.Add(dtRow);
                    i++;
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al obtener las posiciones");
            }
        }

        public void ObtenerRandLdown(decimal idCondicionOP, int posNominal, ResistanceTwentyDegreesViewModel viewModel)
        {
            try
            {

                int capasUp = idCondicionOP == 0 ? int.Parse(viewModel.TapBaan.CantidadSupSc.ToString()) : int.Parse(viewModel.TapBaan.CantidadSupBc.ToString());
                int capasDown = idCondicionOP == 0 ? int.Parse(viewModel.TapBaan.CantidadInfSc.ToString()) : int.Parse(viewModel.TapBaan.CantidadInfBc.ToString());
                int i = 1;
                for (int iterador = capasUp; iterador > 0; iterador--)
                {
                    DataRow dtRow = dtValorNom.NewRow();
                    dtRow["POSICION"] = iterador + "L";
                    dtRow["idIdentificador"] = idIdentificacion;
                    if (i == posNominal)
                    {
                        dtRow["RESISTANCE"] = string.Format("{0:0.###}", valTension);
                    }
                    dtValorNom.Rows.Add(dtRow);
                    i++;
                }

                DataRow dtNom = dtValorNom.NewRow();
                dtNom["POSICION"] = "NOM";
                dtNom["idIdentificador"] = idIdentificacion;
                if (i == posNominal)
                {
                    dtNom["RESISTANCE"] = string.Format("{0:0.###}", valTension);
                }
                dtValorNom.Rows.Add(dtNom);
                i++;

                for (int iterador = 0; iterador < capasDown; iterador++)
                {
                    DataRow dtRow = dtValorNom.NewRow();
                    dtRow["POSICION"] = iterador + 1 + "R";
                    dtRow["idIdentificador"] = idIdentificacion;
                    if (i == posNominal)
                    {
                        dtRow["RESISTANCE"] = string.Format("{0:0.###}", valTension);
                    }
                    i++;
                    dtValorNom.Rows.Add(dtRow);
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al obtener las posiciones");
            }
        }

        public void ObtenerPosicionN(decimal idCondicionOP)
        {
            try
            {
                DataRow dtRow = dtValorNom.NewRow();
                dtRow["POSICION"] = "NOM";
                dtRow["RESISTANCE"] = string.Format("{0:0.###}", valTension);
                dtRow["idIdentificador"] = idIdentificacion;
                dtValorNom.Rows.Add(dtRow);
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al obtener las posiciones");
            }
        }

        private void HabilitarGrids(ref ResistanceTwentyDegreesViewModel viewModel)
        {

            if (viewModel.CharacteristicsPlaneTension.Mvaf1Voltage1.HasValue)
            {
                tensionAT = viewModel.CharacteristicsPlaneTension.Mvaf1Voltage1.Value;
            }

            if (viewModel.CharacteristicsPlaneTension.Mvaf2Voltage1.HasValue)
            {
                tensionBT = viewModel.CharacteristicsPlaneTension.Mvaf2Voltage1.Value;
            }

            if (viewModel.CharacteristicsPlaneTension.Mvaf4Voltage1.HasValue)
            {
                tensionTER = viewModel.CharacteristicsPlaneTension.Mvaf4Voltage1.Value;
            }
        }

        private void PrepareForm()
        {
            List<SelectListItem> measuringList = new()
            {
                new SelectListItem()
                {
                    Selected = true,
                    Text = "Seleccione...",
                    Value = string.Empty
                }
            };

            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            foreach (int item in Enum.GetValues(typeof(MeasuringResistance)))
            {
                measuringList.Add(new SelectListItem()
                {
                    Text = $"{myTI.ToTitleCase(Enum.GetName(typeof(MeasuringResistance), item).ToLower())}",
                    Value = item.ToString()
                });
            }

            ViewBag.UnitMeasuringItems = measuringList;

            List<SelectListItem> testList = new()
            {
                new SelectListItem()
                {
                    Selected = true,
                    Text = "Seleccione...",
                    Value = string.Empty
                }
            };

            ViewBag.TestConnectionItems = testList;
        }
        #endregion

    }
}
