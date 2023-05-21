namespace SPL.WebApp.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Identity.Web;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.Helpers;
    using SPL.WebApp.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class PlateTensionController : Controller
    {
        private readonly IArtifactClientService _artifactClientService;
        private readonly IPlateTensionService _plateTensionService;

        private readonly IMapper _mapper;

        private decimal valTension, tensionAT, tensionBT, tensionTER;

        private int? invertidoGrid;
        private DataTable dtValorNom = new();
        private readonly char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        readonly int? idIdentificacion = 0;
        private readonly IProfileSecurityService _profileClientService;
        public PlateTensionController(
            IArtifactClientService artifactClientService,
            IPlateTensionService plateTensionService,
            IMapper mapper,
            IProfileSecurityService profileClientService)
        {
            this._mapper = mapper;
            this._artifactClientService = artifactClientService;
            this._plateTensionService = plateTensionService;
            this._profileClientService = profileClientService;
        }

        public IActionResult Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(User.Identity.Name).Result;


                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.TensiondelaPlaca)))
                {

                    var noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return this.View(new PlateTensionViewModel { NoSerie = noSerie });
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
        public async Task<IActionResult> Get(string noSerie, bool? newTensonResponse)
        {
            PlateTensionViewModel plateTensionViewModel = new();

            string[] noSerieWo_ = noSerie.Trim().Split('-');

            ApiResponse<CharacteristicsPlaneTensionDTO> characteristicPlateTensionResponse = await this._artifactClientService.GetCharacteristics(noSerieWo_[0]);

            #region Validate if exist any characteristics or other error
            if (characteristicPlateTensionResponse.Code.Equals(-1))
            {
                return this.Json(new { response = characteristicPlateTensionResponse });
            }
            else
            {
                plateTensionViewModel.CharacteristicsPlaneTension = characteristicPlateTensionResponse.Structure;
            }
            #endregion

            ApiResponse<TapBaanDTO> tapbaanPlateTensionResponse = await this._artifactClientService.GetTapBaanPlateTension(noSerieWo_[0]);

            #region Validate if exist any tapbaan or other error
            if (tapbaanPlateTensionResponse.Code.Equals(-1))
            {
                return this.Json(new { response = tapbaanPlateTensionResponse });
            }
            else
            {
                plateTensionViewModel.TapBaan = tapbaanPlateTensionResponse.Structure;
            }
            #endregion

            #region GetPositions

            decimal? position = 1;
            #region Position with out charge

            plateTensionViewModel.PositionTapBaan = new PositionTensionPlateDTO
            {
                NominalAT = "1",
                PositionAT = "1",
                NominalBT = "1",
                PositionBT = "1",
                NominalTER = "1",
                PositionTER = "1"
            };

            position = plateTensionViewModel.TapBaan.CantidadSupSc + plateTensionViewModel.TapBaan.CantidadInfSc + 1;
            this.ShowPositionValues( position, 0, false, ref plateTensionViewModel);

            #endregion

            #region Position with charge

            position = plateTensionViewModel.TapBaan.CantidadSupBc + plateTensionViewModel.TapBaan.CantidadInfBc + 1;
            this.ShowPositionValues( position, 1, true, ref plateTensionViewModel);

            #endregion

            #endregion

            #region Validate positions

            ApiResponse<List<PlateTensionDTO>> tensionPlateAllVoltageResponse = await this._artifactClientService.GetTensionOriginalPlate(noSerie, "-1");

            //#region Validate Result
            //if (tensionPlateAllVoltageResponse.Code.Equals(-1))
            //{
            //    tensionPlateAllVoltageResponse.Code = 2;
            //    return Json(new
            //    {
            //        response = tensionPlateAllVoltageResponse
            //    });
            //}
            //#endregion

            List<PlateTensionDTO> tensionPlateAllVoltageResult = tensionPlateAllVoltageResponse.Structure;

            if (tensionPlateAllVoltageResult.Any())
            {
                plateTensionViewModel.TensionAT = tensionPlateAllVoltageResult.FindAll(c => c.TipoTension.ToUpper().Equals("AT"));
                plateTensionViewModel.TensionBT = tensionPlateAllVoltageResult.FindAll(c => c.TipoTension.ToUpper().Equals("BT"));
                plateTensionViewModel.TensionTER = tensionPlateAllVoltageResult.FindAll(c => c.TipoTension.ToUpper().Equals("TER"));
            }

            _ = decimal.TryParse(plateTensionViewModel.PositionTapBaan?.PositionAT, out decimal positionAT);
            _ = decimal.TryParse(plateTensionViewModel.PositionTapBaan?.PositionBT, out decimal positionBT);
            _ = decimal.TryParse(plateTensionViewModel.PositionTapBaan?.PositionTER, out decimal positionTER);

            plateTensionViewModel.PositionValidate = this._plateTensionService.ValidatePositions(tensionPlateAllVoltageResult, positionAT, positionBT, positionTER, out bool isNewTension);

            this.habilitarGrids( ref plateTensionViewModel);

            if (isNewTension && !newTensonResponse.HasValue)
            {
                ApiResponse<bool> response = new()
                {
                    Code = 2,
                    Description = "NEW_TENSION",
                    Structure = isNewTension
                };

                return this.Json(new { response });
            }
            #endregion
            if(newTensonResponse.HasValue && !newTensonResponse.Value)
            {
                plateTensionViewModel.PositionValidate = false;
            }

            if (plateTensionViewModel.PositionValidate)
            {
                plateTensionViewModel.LoadNewTension = true;
                this.CargarInfoGrid(isNewTension, ref plateTensionViewModel);
            }
            else
            {
                plateTensionViewModel.LoadNewTension = false;

                #region Update positions
                plateTensionViewModel.TensionAT = tensionPlateAllVoltageResult.FindAll(c => c.TipoTension.ToUpper().Trim().Equals("AT"));
                plateTensionViewModel.TensionBT = tensionPlateAllVoltageResult.FindAll(c => c.TipoTension.ToUpper().Trim().Equals("BT"));
                plateTensionViewModel.TensionTER = tensionPlateAllVoltageResult.FindAll(c => c.TipoTension.ToUpper().Trim().Equals("TER"));
                plateTensionViewModel.Positions = new PositionTensionPlateDTO
                {
                    PositionAT = plateTensionViewModel.TensionAT.Count.Equals(0) ? "1" : plateTensionViewModel.TensionAT.Count.ToString(),
                    PositionBT = plateTensionViewModel.TensionBT.Count.Equals(0) ? "1" : plateTensionViewModel.TensionBT.Count.ToString(),
                    PositionTER = plateTensionViewModel.TensionTER.Count.Equals(0) ? "1" : plateTensionViewModel.TensionTER.Count.ToString()
                };

                #endregion

                this.CargarInfoTension( ref plateTensionViewModel);

            }
            try
            {
                if (plateTensionViewModel.PositionValidate)
                {

                    decimal antSumAT = Convert.ToDecimal(plateTensionViewModel.TableAT.Rows[Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalAT) - 1]["TENSION"]);

                    decimal antRestAT = Convert.ToDecimal(plateTensionViewModel.TableAT.Rows[Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalAT) - 1]["TENSION"]);

                    if (plateTensionViewModel.TapBaan.ComboNumericBc is 3 or 5)
                    {
                        for (int i = Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalAT) - 2; i >= 0; i--)
                        {
                            antSumAT = Convert.ToDecimal(plateTensionViewModel.TableAT.Rows[i + 1]["TENSION"]);
                            plateTensionViewModel.TableAT.Rows[i]["TENSION"] = Math.Abs(Math.Round((antSumAT * Convert.ToDecimal(plateTensionViewModel.TapBaan.PorcentajeSupBc) / 100) + antSumAT, 3));

                            
                        }

                        for (int j = Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalAT); j < plateTensionViewModel.TableAT.Rows.Count; j++)
                        {
                            antRestAT = Convert.ToDecimal(plateTensionViewModel.TableAT.Rows[j - 1]["TENSION"]);
                            plateTensionViewModel.TableAT.Rows[j]["TENSION"] = Math.Abs(Math.Round((antRestAT * Convert.ToDecimal(plateTensionViewModel.TapBaan.PorcentajeInfBc) / 100) - antRestAT, 3));
                        }
                    }
                    else if (plateTensionViewModel.TapBaan.ComboNumericSc is 3 or 5)
                    {
                        for (int i = Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalAT) - 2; i >= 0; i--)
                        {
                            antSumAT = Convert.ToDecimal(plateTensionViewModel.TableAT.Rows[i + 1]["TENSION"]);
                            plateTensionViewModel.TableAT.Rows[i]["TENSION"] = Math.Abs(Math.Round((antSumAT * Convert.ToDecimal(plateTensionViewModel.TapBaan.PorcentajeSupSc) / 100) + antSumAT, 3));

                          
                        }

                        for (int j = Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalAT); j < plateTensionViewModel.TableAT.Rows.Count; j++)
                        {
                            antRestAT = Convert.ToDecimal(plateTensionViewModel.TableAT.Rows[j - 1]["TENSION"]);
                            plateTensionViewModel.TableAT.Rows[j]["TENSION"] = Math.Abs(Math.Round((antRestAT * Convert.ToDecimal(plateTensionViewModel.TapBaan.PorcentajeInfSc) / 100) - antRestAT, 3));
                        }
                    }

                    decimal antSumBT = Convert.ToDecimal(plateTensionViewModel.TableBT.Rows[Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalBT) - 1]["TENSION"]);

                    decimal antRestBT = Convert.ToDecimal(plateTensionViewModel.TableBT.Rows[Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalBT) - 1]["TENSION"]);

                    if (plateTensionViewModel.TapBaan.ComboNumericBc is 2 or 4)
                    {
                        for (int i = Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalBT) - 2; i >= 0; i--)
                        {

                            antSumBT = Convert.ToDecimal(plateTensionViewModel.TableBT.Rows[i + 1]["TENSION"]);
                            plateTensionViewModel.TableBT.Rows[i]["TENSION"] = Math.Abs(Math.Round((antSumBT * Convert.ToDecimal(plateTensionViewModel.TapBaan.PorcentajeSupBc) / 100) + antSumBT, 3));

                        }

                        for (int j = Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalBT); j < plateTensionViewModel.TableBT.Rows.Count; j++)
                        {
                            antRestBT = Convert.ToDecimal(plateTensionViewModel.TableBT.Rows[j - 1]["TENSION"]);
                            plateTensionViewModel.TableBT.Rows[j]["TENSION"] = Math.Abs(Math.Round((antRestBT * Convert.ToDecimal(plateTensionViewModel.TapBaan.PorcentajeInfBc) / 100) - antRestBT, 3));
                        }
                    }
                    else if (plateTensionViewModel.TapBaan.ComboNumericSc is 2 or 4)
                    {

                        for (int i = Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalBT) - 2; i >= 0; i--)
                        {
                            antSumBT = Convert.ToDecimal(plateTensionViewModel.TableBT.Rows[i + 1]["TENSION"]);
                            plateTensionViewModel.TableBT.Rows[i]["TENSION"] = Math.Abs(Math.Round((antSumBT * Convert.ToDecimal(plateTensionViewModel.TapBaan.PorcentajeSupSc) / 100) + antSumBT, 3));

                          
                        }

                        for (int j = Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalBT); j < plateTensionViewModel.TableBT.Rows.Count; j++)
                        {
                            antRestBT = Convert.ToDecimal(plateTensionViewModel.TableBT.Rows[j - 1]["TENSION"]);
                            plateTensionViewModel.TableBT.Rows[j]["TENSION"] = Math.Abs(Math.Round((antRestBT * Convert.ToDecimal(plateTensionViewModel.TapBaan.PorcentajeInfSc) / 100) - antRestBT, 3));
                        }
                    }

                    decimal antSumTER = Convert.ToDecimal(plateTensionViewModel.TableTER.Rows[Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalTER) - 1]["TENSION"]);

                    decimal antRestTER = Convert.ToDecimal(plateTensionViewModel.TableTER.Rows[Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalTER) - 1]["TENSION"]);

                    if (plateTensionViewModel.TapBaan.ComboNumericBc is 2 or 4)
                    {
                        for (int i = Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalTER) - 2; i >= 0; i--)
                        {

                            antSumTER = Convert.ToDecimal(plateTensionViewModel.TableTER.Rows[i + 1]["TENSION"]);
                            plateTensionViewModel.TableTER.Rows[i]["TENSION"] = Math.Abs(Math.Round((antSumTER * Convert.ToDecimal(plateTensionViewModel.TapBaan.PorcentajeSupBc) / 100) + antSumTER, 3));
                        }

                        for (int j = Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalTER); j < plateTensionViewModel.TableTER.Rows.Count; j++)
                        {
                            antRestTER = Convert.ToDecimal(plateTensionViewModel.TableTER.Rows[j - 1]["TENSION"]);
                            plateTensionViewModel.TableTER.Rows[j]["TENSION"] = Math.Abs(Math.Round((antRestTER * Convert.ToDecimal(plateTensionViewModel.TapBaan.PorcentajeInfBc) / 100) - antRestTER, 3));
                        }
                    }
                    else if (plateTensionViewModel.TapBaan.ComboNumericSc is 2 or 4)
                    {

                        for (int i = Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalTER) - 2; i >= 0; i--)
                        {

                            antSumTER = Convert.ToDecimal(plateTensionViewModel.TableTER.Rows[i + 1]["TENSION"]);
                            plateTensionViewModel.TableTER.Rows[i]["TENSION"] = Math.Abs(Math.Round((antSumTER * Convert.ToDecimal(plateTensionViewModel.TapBaan.PorcentajeSupSc) / 100) + antSumTER, 3));
                        }

                        for (int j = Convert.ToInt32(plateTensionViewModel.PositionTapBaan.NominalTER); j < plateTensionViewModel.TableTER.Rows.Count; j++)
                        {
                            antRestTER = Convert.ToDecimal(plateTensionViewModel.TableTER.Rows[j - 1]["TENSION"]);
                            plateTensionViewModel.TableTER.Rows[j]["TENSION"] = Math.Abs(Math.Round((antRestTER * Convert.ToDecimal(plateTensionViewModel.TapBaan.PorcentajeInfSc) / 100) - antRestTER, 3));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
     

            return this.Json(new { 
            
                response = new ApiResponse<PlateTensionViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = plateTensionViewModel
                }
            });

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlateTensionViewModel viewModel)
        {

            DataTable tableAt = viewModel.TableAT;
            List<PlateTensionDTO> tensions = new();

            

            if (viewModel.TableAT.AsEnumerable().Where(c => Convert.ToDecimal(c.ItemArray[1]) != 0).Any())
            {
                PlateTensionDTO tensionAT = viewModel.TensionAT.FirstOrDefault();
                int order = 1;
                decimal? posNom = viewModel.TapBaan.ComboNumericBc is 3 or 5
                    ? viewModel.TapBaan.NominalBc
                    : viewModel.TapBaan.ComboNumericSc is 3 or 5 ? viewModel.TapBaan.NominalSc : (decimal?)1;
                foreach (object[] item in viewModel.TableAT.AsEnumerable().Select(c => c.ItemArray))
                {
                    tensions.Add(new PlateTensionDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion =DateTime.Now,
                        Fechamodificacion = DateTime.Now,
                        Modificadopor = User.Identity.Name,
                        Orden = order,
                        N = order==posNom,
                        TipoTension ="AT",
                        Unidad = viewModel.NoSerie,
                        Posicion = Convert.ToString(item[0]),
                        Tension = Convert.ToDecimal(item[1])
                    });

                    order++;
                }
            }

            if (viewModel.TableBT.AsEnumerable().Where(c => Convert.ToDecimal(c.ItemArray[1]) != 0).Any())
            {
                PlateTensionDTO tensionBT = viewModel.TensionAT.FirstOrDefault();
                int order = 1;

                decimal? posNom = viewModel.TapBaan.ComboNumericBc is 2 or 4
                    ? viewModel.TapBaan.NominalBc
                    : viewModel.TapBaan.ComboNumericSc is 2 or 4 ? viewModel.TapBaan.NominalSc : (decimal?)1;
                foreach (object[] item in viewModel.TableBT.AsEnumerable().Select(c => c.ItemArray))
                {
                    tensions.Add(new PlateTensionDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = DateTime.Now,
                        Modificadopor = User.Identity.Name,
                        Orden = order,
                        N = order == posNom,
                        TipoTension = "BT",
                        Unidad = viewModel.NoSerie,
                        Posicion = Convert.ToString(item[0]),
                        Tension = Convert.ToDecimal(item[1])
                    });

                    order++;
                }
            }

            if (viewModel.TableTER.AsEnumerable().Where(c => Convert.ToDecimal(c.ItemArray[1]) != 0).Any())
            {
                PlateTensionDTO tensionTER = viewModel.TensionTER.FirstOrDefault();
                int order = 1;

                decimal? posNom = viewModel.TapBaan.ComboNumericBc ==1
                    ? viewModel.TapBaan.NominalBc
                    : viewModel.TapBaan.ComboNumericSc == 1 ? viewModel.TapBaan.NominalSc : (decimal?)1;
                foreach (object[] item in viewModel.TableTER.AsEnumerable().Select(c => c.ItemArray))
                {
                    tensions.Add(new PlateTensionDTO
                    {
                        Creadopor = User.Identity.Name,
                        Fechacreacion = DateTime.Now,
                        Fechamodificacion = DateTime.Now,
                        Modificadopor = User.Identity.Name,
                        Orden = order,
                        N = order == posNom,
                        TipoTension = "TER",
                        Unidad = viewModel.NoSerie,
                        Posicion = Convert.ToString(item[0]),
                        Tension = Convert.ToDecimal(item[1])
                    });

                    order++;
                }
            }

            ApiResponse<long> result = await this._artifactClientService.AddTension(tensions, viewModel.PositionValidate);

            return this.Json(new
            {
                response = result
            });
        }

        #region Private Methods
        public void ShowPositionValues(decimal? position, decimal conditionOpId, bool charge, ref PlateTensionViewModel viewModel)
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

        public void CargarInfoGrid( bool isNewTension, ref PlateTensionViewModel viewModel)
        {
            //CARGAR INFO SIN CARGA
            this.CargarPosiciones("AT", isNewTension, ref viewModel);
            this.CargarPosiciones("BT", isNewTension, ref viewModel);
            this.CargarPosiciones("TER", isNewTension, ref viewModel);

        }

        public void CargarInfoTension(ref PlateTensionViewModel viewModel)
        {
            this.CargarTensionGrid("AT", ref viewModel);
            this.CargarTensionGrid("BT", ref viewModel);
            this.CargarTensionGrid("TER", ref viewModel);
        }

        public void CargarTensionGrid(string nomGrid, ref PlateTensionViewModel viewModel)
        {
            List<PlateTensionDTO> lstTensionPlaca;

            switch (nomGrid)
            {
                case "AT":
                    this.valTension = this.tensionAT;
                    lstTensionPlaca = viewModel.TensionAT;
                    if (lstTensionPlaca.Count > 0)
                    {
                        this.LlenarDataTable(lstTensionPlaca);
                    }
                    else
                    {
                        viewModel.PositionTapBaan.NominalAT = "1";
                        viewModel.PositionTapBaan.IdentificationAT = -1;
                        _ = int.TryParse(viewModel.PositionTapBaan.PositionAT, out int positionAT);
                        _ = int.TryParse(viewModel.PositionTapBaan.NominalAT, out int nominalAT);
                        this.ObtenerPosicionesGrid(viewModel.PositionTapBaan.IdentificationAT, viewModel.PositionTapBaan.IdConditionOpAT, positionAT, nominalAT, ref viewModel);
                    }
                    this.CargarGrid(0, "AT", ref viewModel);
                    break;
                case "BT":
                    this.valTension = this.tensionBT;
                    lstTensionPlaca = viewModel.TensionBT;
                    if (lstTensionPlaca.Count > 0)
                    {
                        this.LlenarDataTable(lstTensionPlaca);
                    }
                    else
                    {
                        viewModel.PositionTapBaan.NominalBT = "1";
                        viewModel.PositionTapBaan.IdentificationBT = -1;
                        _ = int.TryParse(viewModel.PositionTapBaan.PositionAT, out int positionAT);
                        _ = int.TryParse(viewModel.PositionTapBaan.NominalAT, out int nominalAT);
                        this.ObtenerPosicionesGrid(viewModel.PositionTapBaan.IdentificationBT, viewModel.PositionTapBaan.IdConditionOpAT, positionAT, nominalAT, ref viewModel);
                    }
                    this.CargarGrid(0, "BT", ref viewModel);
                    break;
                case "TER":
                    this.valTension = this.tensionTER;
                    lstTensionPlaca = viewModel.TensionTER;
                    if (lstTensionPlaca.Count > 0)
                    {
                        this.LlenarDataTable(lstTensionPlaca);
                    }
                    else
                    {

                        viewModel.PositionTapBaan.NominalTER = "1";
                        viewModel.PositionTapBaan.IdentificationTER = -1;
                        _ = int.TryParse(viewModel.PositionTapBaan.PositionAT, out int positionAT);
                        _ = int.TryParse(viewModel.PositionTapBaan.NominalAT, out int nominalAT);
                        this.ObtenerPosicionesGrid(viewModel.PositionTapBaan.IdentificationTER, viewModel.PositionTapBaan.IdConditionOpAT, positionAT, nominalAT, ref viewModel);
                    }

                    this.CargarGrid(0, "TER", ref viewModel);
                    break;
            }
        }

        public void CargarPosiciones(string nomGrid, bool isNewTension, ref PlateTensionViewModel viewModel)
        {
            List<PlateTensionDTO> lstTensionPlaca;

            switch (nomGrid)
            {
                case "AT":
                    this.valTension = this.tensionAT;
                    _ = int.TryParse(viewModel.PositionTapBaan.PositionAT, out int atPosition);
                    _ = int.TryParse(viewModel.PositionTapBaan.NominalAT, out int atNominal);
                    this.obtenerPosicionesGrid(viewModel.PositionTapBaan.IdentificationAT, viewModel.PositionTapBaan.IdConditionOpAT, atPosition, atNominal, ref viewModel);
                    //lstTensionPlaca = obtenerTensionKV(noSerie, "AT");
                    lstTensionPlaca = viewModel.TensionAT;
                    if (lstTensionPlaca != null && lstTensionPlaca.Any())
                        this.llenarDataTableMerge(lstTensionPlaca, isNewTension);
                    this.CargarGrid(this.invertidoGrid, "AT", ref viewModel);
                    break;
                case "BT":
                    this.valTension = this.tensionBT;
                    _ = int.TryParse(viewModel.PositionTapBaan.PositionBT, out int btPosition);
                    _ = int.TryParse(viewModel.PositionTapBaan.NominalBT, out int btNominal);
                    this.obtenerPosicionesGrid(viewModel.PositionTapBaan.IdentificationBT, viewModel.PositionTapBaan.IdConditionOpBT, btPosition, btNominal, ref viewModel);
                    //lstTensionPlaca = obtenerTensionKV(noSerie, "BT");
                    lstTensionPlaca = viewModel.TensionBT;
                    if (lstTensionPlaca != null && lstTensionPlaca.Any())
                        this.llenarDataTableMerge(lstTensionPlaca, isNewTension);
                    this.CargarGrid(this.invertidoGrid, "BT", ref viewModel);
                    break;
                case "TER":
                    this.valTension = this.tensionTER;
                    _ = int.TryParse(viewModel.PositionTapBaan.PositionTER, out int terPosition);
                    _ = int.TryParse(viewModel.PositionTapBaan.NominalBT, out int terNominal);
                    this.obtenerPosicionesGrid(viewModel.PositionTapBaan.IdentificationTER, viewModel.PositionTapBaan.IdConditionOpTER, terPosition, terNominal, ref viewModel);
                    //lstTensionPlaca = obtenerTensionKV(noSerie, "TER");
                    lstTensionPlaca = viewModel.TensionTER; 
                    if (lstTensionPlaca != null && lstTensionPlaca.Any())
                        this.llenarDataTableMerge(lstTensionPlaca, isNewTension);
                    this.CargarGrid(this.invertidoGrid, "TER", ref viewModel);
                    break;
            }
        }

        public void ObtenerPosicionesGrid(decimal? idIdentificacion, decimal idCondicionOP, int posiciones, int posNominal, ref PlateTensionViewModel viewModel)
        {
            try
            {
                this.invertidoGrid = (int)(idCondicionOP == 0 ? viewModel.TapBaan.InvertidoSc : idCondicionOP == 1 ? viewModel.TapBaan.InvertidoBc : 0);
                this.dtValorNom = new DataTable();
                this.CrearDataTable();
                switch (idIdentificacion)
                {
                    case 1:
                        this.obtenerLetras(posiciones, posNominal);
                        break;
                    case 2:
                        this.obtenerNumero(posiciones, posNominal);
                        break;
                    case 3:
                        this.obtenerRandLsup(idCondicionOP, posNominal, viewModel);
                        break;
                    case 4:
                        this.obtenerRandLdown(idCondicionOP, posNominal, viewModel);
                        break;
                    default:
                        this.obtenerPosicionN(idCondicionOP);
                        break;
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio u error al obtener las posiciones del grid.");
            }
        }
        public void LlenarDataTable(List<PlateTensionDTO> lstTensionPlaca)
        {
            try
            {
                this.dtValorNom = new DataTable();
                this.CrearDataTable();

                foreach (PlateTensionDTO tens in lstTensionPlaca.OrderBy(x => x.Orden))
                {
                    DataRow dtRow = this.dtValorNom.NewRow();
                    dtRow["POSICION"] = tens.Posicion;
                    dtRow["TENSION"] = tens.Tension;
                    dtRow["Existente"] = true;
                    this.dtValorNom.Rows.Add(dtRow);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al cargar la información del grid.");
            }
        }

        public void obtenerPosicionesGrid(decimal? idIdentificacion, decimal idCondicionOP, int posiciones, int posNominal, ref PlateTensionViewModel viewModel)
        {
            try
            {
                this.invertidoGrid = (int)(idCondicionOP == 0 ? viewModel.TapBaan.InvertidoSc : idCondicionOP == 1 ? viewModel.TapBaan.InvertidoBc : 0);
                this.dtValorNom = new DataTable();
                this.CrearDataTable();
                switch (idIdentificacion)
                {
                    case 1:
                        this.obtenerLetras(posiciones, posNominal);
                        break;
                    case 2:
                        this.obtenerNumero(posiciones, posNominal);
                        break;
                    case 3:
                        this.obtenerRandLsup(idCondicionOP, posNominal, viewModel);
                        break;
                    case 4:
                        this.obtenerRandLdown(idCondicionOP, posNominal, viewModel);
                        break;
                    default:
                        this.obtenerPosicionN(idCondicionOP);
                        break;
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio u error al obtener las posiciones del grid.");
            }
        }

        public void CargarGrid(int? invertido, string idGrid, ref PlateTensionViewModel viewModel)
        {
            try
            {
                if (invertido == 1)
                {
                    IEnumerable<DataRow> orderedRows = from row in this.dtValorNom.AsEnumerable().Reverse() select row;
                    this.dtValorNom = orderedRows.CopyToDataTable();
                }
                idGrid = idGrid.ToUpper();
                switch (idGrid)
                {
                    case "AT":
                        viewModel.TableAT = this.dtValorNom;                        
                        break;
                    case "BT":
                        viewModel.TableBT = this.dtValorNom;
                        break;
                    case "TER":
                        viewModel.TableTER = this.dtValorNom;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al cargar la información del grid.");
            }
        }

        public void llenarDataTableMerge(List<PlateTensionDTO> lstTensionPlaca, bool isNewTension)
        {
            try
            {
                PlateTensionDTO tensionPlaca;
                int i = this.invertidoGrid == 1 ? lstTensionPlaca.Count : 1;
                foreach (DataRow dt in this.dtValorNom.Rows)
                {
                    int numOrden = i;
                    tensionPlaca = new PlateTensionDTO();
                    tensionPlaca = lstTensionPlaca.Find(x => x.Orden == numOrden);
                    if (tensionPlaca != null)
                    {
                        dt["TENSION"] = tensionPlaca.Tension;
                        dt["Existente"] = isNewTension;
                    }
                    i = this.invertidoGrid == 1 ? i - 1 : i + 1;
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
                _ = this.dtValorNom.Columns.Add("POSICION");
                _ = this.dtValorNom.Columns.Add("TENSION");
                _ = this.dtValorNom.Columns.Add("Existente");
                _ = this.dtValorNom.Columns.Add("idIdentificador");
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al crear data table.");
            }
        }
        public void obtenerNumero(int posiciones, int posNominal)
        {
            try
            {
                for (int iterador = 0; iterador < posiciones; iterador++)
                {
                    DataRow dtRow = this.dtValorNom.NewRow();
                    dtRow["POSICION"] = iterador + 1;
                    dtRow["idIdentificador"] = this.idIdentificacion;
                    if (iterador + 1 == posNominal)
                    {
                        dtRow["TENSION"] = string.Format("{0:0.###}", this.valTension);
                    }
                    this.dtValorNom.Rows.Add(dtRow);
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al obtener las posiciones");
            }
        }

        public void obtenerLetras(int posiciones, int posNominal)
        {
            try
            {
                for (int iterador = 0; iterador < posiciones; iterador++)
                {
                    DataRow dtRow = this.dtValorNom.NewRow();
                    dtRow["POSICION"] = this.alpha[iterador].ToString();
                    dtRow["idIdentificador"] = this.idIdentificacion;
                    if (iterador + 1 == posNominal)
                    {
                        dtRow["TENSION"] = string.Format("{0:0.###}", this.valTension);
                    }
                    this.dtValorNom.Rows.Add(dtRow);
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al obtener las posiciones");
            }
        }

        public void obtenerRandLsup(decimal idCondicionOP, decimal posNominal, PlateTensionViewModel viewModel)
        {
            try
            {
                int capasUp = idCondicionOP == 0 ? int.Parse(viewModel.TapBaan.CantidadSupSc.ToString()) : int.Parse(viewModel.TapBaan.CantidadSupBc.ToString());
                int capasDown = idCondicionOP == 0 ? int.Parse(viewModel.TapBaan.CantidadInfSc.ToString()) : int.Parse(viewModel.TapBaan.CantidadInfBc.ToString());
                int i = 1;
                for (int iterador = capasUp; iterador > 0; iterador--)
                {
                    DataRow dtRow = this.dtValorNom.NewRow();
                    dtRow["POSICION"] = iterador + "R";
                    dtRow["idIdentificador"] = this.idIdentificacion;
                    if (i == posNominal)
                    {
                        dtRow["TENSION"] = string.Format("{0:0.###}", this.valTension);
                    }
                    this.dtValorNom.Rows.Add(dtRow);
                    i++;
                }

                DataRow dtNom = this.dtValorNom.NewRow();
                dtNom["POSICION"] = "NOM";
                dtNom["idIdentificador"] = this.idIdentificacion;
                if (i == posNominal)
                {
                    dtNom["TENSION"] = string.Format("{0:0.###}", this.valTension);
                }
                this.dtValorNom.Rows.Add(dtNom);
                i++;

                for (int iterador = 0; iterador < capasDown; iterador++)
                {
                    DataRow dtRow = this.dtValorNom.NewRow();
                    dtRow["POSICION"] = iterador + 1 + "L";
                    dtRow["idIdentificador"] = this.idIdentificacion;
                    if (i == posNominal)
                    {
                        dtRow["TENSION"] = string.Format("{0:0.###}", this.valTension);
                    }
                    this.dtValorNom.Rows.Add(dtRow);
                    i++;
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al obtener las posiciones");
            }
        }

        public void obtenerRandLdown(decimal idCondicionOP, int posNominal, PlateTensionViewModel viewModel)
        {
            try
            {

                int capasUp = idCondicionOP == 0 ? int.Parse(viewModel.TapBaan.CantidadSupSc.ToString()) : int.Parse(viewModel.TapBaan.CantidadSupBc.ToString());
                int capasDown = idCondicionOP == 0 ? int.Parse(viewModel.TapBaan.CantidadInfSc.ToString()) : int.Parse(viewModel.TapBaan.CantidadInfBc.ToString());
                int i = 1;
                for (int iterador = capasUp; iterador > 0; iterador--)
                {
                    DataRow dtRow = this.dtValorNom.NewRow();
                    dtRow["POSICION"] = iterador + "L";
                    dtRow["idIdentificador"] = this.idIdentificacion;
                    if (i == posNominal)
                    {
                        dtRow["TENSION"] = string.Format("{0:0.###}", this.valTension);
                    }
                    this.dtValorNom.Rows.Add(dtRow);
                    i++;
                }

                DataRow dtNom = this.dtValorNom.NewRow();
                dtNom["POSICION"] = "NOM";
                dtNom["idIdentificador"] = this.idIdentificacion;
                if (i == posNominal)
                {
                    dtNom["TENSION"] = string.Format("{0:0.###}", this.valTension);
                }
                this.dtValorNom.Rows.Add(dtNom);
                i++;

                for (int iterador = 0; iterador < capasDown; iterador++)
                {
                    DataRow dtRow = this.dtValorNom.NewRow();
                    dtRow["POSICION"] = iterador + 1 + "R";
                    dtRow["idIdentificador"] = this.idIdentificacion;
                    if (i == posNominal)
                    {
                        dtRow["TENSION"] = string.Format("{0:0.###}", this.valTension);
                    }
                    i++;
                    this.dtValorNom.Rows.Add(dtRow);
                }
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al obtener las posiciones");
            }
        }

        public void obtenerPosicionN(decimal idCondicionOP)
        {
            try
            {
                DataRow dtRow = this.dtValorNom.NewRow();
                dtRow["POSICION"] = "NOM";
                dtRow["TENSION"] = string.Format("{0:0.###}", this.valTension);
                dtRow["idIdentificador"] = this.idIdentificacion;
                this.dtValorNom.Rows.Add(dtRow);
            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al obtener las posiciones");
            }
        }

        private void habilitarGrids(ref PlateTensionViewModel viewModel)
        {

            if (viewModel.CharacteristicsPlaneTension.Mvaf1Voltage1.HasValue)
            {
                this.tensionAT = viewModel.CharacteristicsPlaneTension.Mvaf1Voltage1.Value;
            }

            if (viewModel.CharacteristicsPlaneTension.Mvaf2Voltage1.HasValue)
            {
                this.tensionBT = viewModel.CharacteristicsPlaneTension.Mvaf2Voltage1.Value;
            }

            if (viewModel.CharacteristicsPlaneTension.Mvaf4Voltage1.HasValue)
            {
                this.tensionTER = viewModel.CharacteristicsPlaneTension.Mvaf4Voltage1.Value;
            }
        }
        #endregion

    }
}
