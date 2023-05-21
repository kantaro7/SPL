namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
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

    public class ArtifactRecordController : Controller
    {
        private readonly IArtifactClientService _artifactClientService;
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IArtifactRecordService _artifactRecordService;
        private readonly IMapper _mapper;
        private readonly IProfileSecurityService _profileClientService;
        // private readonly ITokenAcquisition _tokenAcquisition;
        public ArtifactRecordController(
            IArtifactClientService artifactClientService,
            IMasterHttpClientService masterHttpClientService,
            ISidcoClientService sidcoClientService,
            IArtifactRecordService artifactRecordService,
            IMapper mapper/*, ITokenAcquisition tokenAcquisition*/,
            IProfileSecurityService profileClientService)
        {
            _artifactClientService = artifactClientService;
            _mapper = mapper;
            _masterHttpClientService = masterHttpClientService;
            _sidcoClientService = sidcoClientService;
            _artifactRecordService = artifactRecordService;
            //this._tokenAcquisition = tokenAcquisition;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.DiseñodelAparato)))
                {
                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    if (!string.IsNullOrWhiteSpace(noSerie))
                    {
                        string[] splited = noSerie.Split("-");
                        if (splited != null && splited.Any())
                            noSerie = splited[0];
                    }
                    ArtifactRecordViewModel viewModel = new()
                    {
                        NoSerie = noSerie
                    };
                    return View(viewModel);
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
        public async Task<IActionResult> GetFromSidco(string noSerie)
        {
            noSerie = noSerie.ToUpper().Trim();

            InformationArtifactDTO result = await _artifactClientService.GetArtifactSIDCO(noSerie);
            if (result.GeneralArtifact.OrderCode == null)
            {
                return NoContent();
            }
            if (!string.IsNullOrEmpty(result.GeneralArtifact.ClaveIdioma) && result.GeneralArtifact.Applicationid.HasValue)
            {
                string languageKey = GetAcronym(result.GeneralArtifact.ClaveIdioma);
                IEnumerable<RulesArtifactDTO> ruleIenumable = await _masterHttpClientService.GetRuleFromSidco(GetZeroLeft(result.GeneralArtifact.Applicationid.Value), languageKey);
                result.RulesArtifact = ruleIenumable.ToList();
            }
            result.GeneralArtifact.Descripcion = "TRANSFORMADOR";

            IEnumerable<CatSidcoOthersDTO> catSidcoOthers = await _masterHttpClientService.GetCatSidcoOthers();

            _artifactRecordService.FixConnections(catSidcoOthers, ref result);

            _artifactRecordService.FixDerivations(ref result);

            ArtifactRecordViewModel viewModel = _mapper.Map<ArtifactRecordViewModel>(result);

            if (viewModel is null)
            {
                return NoContent();
            }

            viewModel.NozzlesArtifacts = getNozzles(viewModel.NozzlesArtifacts, viewModel.NbaiAltaTension, viewModel.NbaiSegundaBaja, viewModel.NbaiBajaTension, viewModel.NabaiTercera);

            FixDataFromSidco(ref viewModel);

            IEnumerable<GeneralPropertiesDTO> getOperativeConditions = await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetOperativeConditions, (int)MicroservicesEnum.splsidco);

            viewModel.IsFromSPL = true;
            viewModel.OperationsItems = new SelectList(getOperativeConditions, "Id", "Description");

            viewModel.TipoUnidad = await ObtenerTipoAparato(result);
            viewModel.TipoUnidad = GetAcronymTransformer(viewModel.TipoUnidad);

            IEnumerable<CatSidcoDTO> catSidco = await _sidcoClientService.GetCatSIDCO();

            //Norm Equivalent
            foreach (CatSidcoDTO obj in catSidco)
            {
                if (viewModel.StandardId.Equals(obj.Id.ToString()))
                {
                    viewModel.NormaEquivalente = obj.ClaveSPL;
                }
            }

            //Desplazamiento Angular
            foreach (CatSidcoDTO obj in catSidco)
            {
                if (viewModel.PolarityId.Equals(obj.Id.ToString()))
                {
                    viewModel.DesplazamientoAngular = obj.ClaveSPL;
                }
            }
            return Json(new
            {
                response = viewModel
            });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string noSerie)
        {
            try
            {
                noSerie = noSerie.ToUpper().Trim();

                bool isFromSPL = await _artifactClientService.CheckNumberOrder(noSerie);

                InformationArtifactDTO result = new();
                ArtifactRecordViewModel viewModel;
                if (isFromSPL)
                {
                    result = await _artifactClientService.GetArtifact(noSerie);
                    if (result.GeneralArtifact.OrderCode == null)
                    {
                        return NoContent();
                    }

                    IEnumerable<CatSidcoOthersDTO> catSidcoOthers = await _masterHttpClientService.GetCatSidcoOthers();

                    _artifactRecordService.FixConnections(catSidcoOthers, ref result);

                    viewModel = _mapper.Map<ArtifactRecordViewModel>(result);
                    _ = decimal.TryParse(viewModel.Norma, out decimal normaDecimal);
                    viewModel.NormaEquivalente = GetZeroLeft(normaDecimal);

                    viewModel.NozzlesArtifacts = getNozzles(viewModel.NozzlesArtifacts, viewModel.NbaiAltaTension, viewModel.NbaiSegundaBaja, viewModel.NbaiBajaTension, viewModel.NabaiTercera);

                }
                else
                {
                    result = await _artifactClientService.GetArtifactSIDCO(noSerie);
                    if (result.GeneralArtifact.OrderCode == null)
                    {
                        return NoContent();
                    }
                    result.GeneralArtifact.Descripcion = "TRANSFORMADOR";

                    _artifactRecordService.FixDerivations(ref result);

                    IEnumerable<CatSidcoDTO> catSidco = await _sidcoClientService.GetCatSIDCO();
                    IEnumerable<CatSidcoOthersDTO> catSidcoOthers = await _masterHttpClientService.GetCatSidcoOthers();

                    _artifactRecordService.FixConnections(catSidcoOthers, ref result);

                    if (!string.IsNullOrEmpty(result.GeneralArtifact.ClaveIdioma) && result.GeneralArtifact.Applicationid.HasValue)
                    {
                        string languageKey = GetAcronym(result.GeneralArtifact.ClaveIdioma);
                        IEnumerable<RulesArtifactDTO> ruleIenumable = await _masterHttpClientService.GetRuleFromSidco(GetZeroLeft(result.GeneralArtifact.Applicationid.Value), languageKey);
                        result.RulesArtifact = ruleIenumable.ToList();
                    }

                    viewModel = _mapper.Map<ArtifactRecordViewModel>(result);

                    viewModel.NozzlesArtifacts = getNozzles(viewModel.NozzlesArtifacts, viewModel.NbaiAltaTension, viewModel.NbaiSegundaBaja, viewModel.NbaiBajaTension, viewModel.NabaiTercera);

                    foreach (CatSidcoDTO obj in catSidco)
                    {
                        if (viewModel.StandardId.Equals(obj.Id.ToString()))
                        {
                            viewModel.NormaEquivalente = obj.ClaveSPL;
                        }
                    }
                    foreach (CatSidcoDTO obj in catSidco)
                    {
                        if (viewModel.PolarityId.Equals(obj.Id.ToString()))
                        {
                            viewModel.DesplazamientoAngular = obj.ClaveSPL;
                        }
                    }
                }

                if (viewModel is null)
                {
                    return NoContent();
                }

                viewModel.TipoUnidad = await ObtenerTipoAparato(result);
                viewModel.TipoUnidad = GetAcronymTransformer(viewModel.TipoUnidad);

                FixDataFromSidco(ref viewModel);

                IEnumerable<GeneralPropertiesDTO> getOperativeConditions = await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetOperativeConditions, (int)MicroservicesEnum.splsidco);

                viewModel.IsFromSPL = isFromSPL;
                viewModel.OperationsItems = new SelectList(getOperativeConditions, "Id", "Description");

                return Json(new
                {
                    response = viewModel
                });
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        public List<NozzlesArtifactViewModel> getNozzles(List<NozzlesArtifactViewModel> pList, string value1, string value2, string value3, string value4)
        {
            foreach (NozzlesArtifactViewModel item in pList)
            {

                item.ResultBilUnidad = item.ColumnTitle is "Alta Tensión" or "Dev Serie"
                    ? value1.ToString()
                    : item.ColumnTitle == "Baja Tensión 2"
                        ? value2.ToString()
                        : item.ColumnTitle is "Baja Tensión" or "Dev Común" ? value3.ToString() : item.ColumnTitle == "Terc" ? value4.ToString() : "0";
            };
            return pList;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ArtifactRecordViewModel viewModel)
        {

            if (viewModel is not null)
            {
                if (viewModel is not null)
                {
                    HttpStatusCode response;
                    try
                    {
                        FixRuleArtifacts(ref viewModel);
                        InformationArtifactDTO dto = null;

                        dto = _mapper.Map<InformationArtifactDTO>(viewModel);
                        dto.TapBaan = _mapper.Map<TapBaanDTO>(viewModel);

                        string NameUserC = User.Identity.Name;
                        DateTime FechaCreacion = DateTime.Now;

                        InformationArtifactDTO exist = null;
                        if (viewModel.Update.Equals(ArtifactUpdate.ALL))
                        {

                            dto.GeneralArtifact.CreadoPor = NameUserC;
                            dto.GeneralArtifact.FechaCreacion = viewModel.Fechacreacion;
                            dto.GeneralArtifact.ModificadoPor = NameUserC;
                            dto.GeneralArtifact.FechaModificacion = FechaCreacion;

                            foreach (CharacteristicsArtifactDTO item in dto.CharacteristicsArtifact)
                            {
                                item.CreadoPor = viewModel.Creadopor; 
                                item.FechaCreacion = viewModel.Fechacreacion;
                                item.ModificadoPor = NameUserC; 
                                item.FechaModificacion = FechaCreacion;
                            }

                            dto.WarrantiesArtifact.CreadoPor = NameUserC;
                            dto.WarrantiesArtifact.FechaCreacion = FechaCreacion;
                            dto.WarrantiesArtifact.ModificadoPor = NameUserC;
                            dto.WarrantiesArtifact.FechaModificacion = FechaCreacion;

                            foreach (NozzlesArtifactDTO item in dto.NozzlesArtifact)
                            {
                                item.CreadoPor = viewModel.Creadopor; ;
                                item.FechaCreacion = viewModel.Fechacreacion;
                                item.ModificadoPor = NameUserC;
                                item.FechaModificacion = FechaCreacion;
                            }

                            foreach (LightningRodArtifactDTO item in dto.LightningRodArtifact)
                            {
                                item.CreadoPor = viewModel.Creadopor; ;
                                item.FechaCreacion = viewModel.Fechacreacion;
                                item.ModificadoPor = NameUserC;
                                item.FechaModificacion = FechaCreacion;
                            }

                            foreach (ChangingTablesArtifactDTO item in dto.ChangingTablesArtifact)
                            {
                                item.Creadopor = viewModel.Creadopor; ;
                                item.FechaCreacion = viewModel.Fechacreacion;
                                item.ModificadoPor = NameUserC;
                                item.FechaModificacion = FechaCreacion;
                            }

                            dto.LabTestsArtifact.CreadoPor = viewModel.Creadopor; ;
                            dto.LabTestsArtifact.FechaCreacion = viewModel.Fechacreacion;
                            dto.LabTestsArtifact.ModificadoPor = NameUserC;
                            dto.LabTestsArtifact.FechaModificacion = null;

                            foreach (RulesArtifactDTO item in dto.RulesArtifact)
                            {
                                item.CreadoPor = viewModel.Creadopor; ;
                                item.FechaCreacion = viewModel.Fechacreacion;
                                item.ModificadoPor = NameUserC;
                                item.FechaModificacion = FechaCreacion;
                            }
                        }
                        else
                        {

                            exist = await _artifactClientService.GetArtifact(dto.GeneralArtifact.OrderCode);
                        }

                        response = viewModel.Update.Equals(ArtifactUpdate.ALL)
                            ? await _artifactClientService.AddArtifactToSPL(dto)
                            : await _artifactClientService.UpdateArtifactToSPL(dto, viewModel.Update, User.Identity.Name, exist.GeneralArtifact.CreadoPor, exist.GeneralArtifact.FechaCreacion);
                    }
                    catch (Exception)
                    {
                        return BadRequest();
                    }

                    return Json(new
                    {
                        response
                    });
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                string errors = ModelState.Values.SelectMany(v => v.Errors).Select(c => c.ErrorMessage).Aggregate((c, n) => c + "*" + n);
                foreach (string error in errors.Split("*"))
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return BadRequest(ModelState);

            }
        }

        public IActionResult GetChangers(string cantPos, string cantNeg, string nominal, IdentificationChanger identificationId, bool reversed)
        {
            ChangesPositionDTO changesPositionDTO = new()
            {
                Position = nominal,
                Reversed = reversed
            };

            _ = int.TryParse(cantPos, out int positionSup);
            _ = int.TryParse(cantNeg, out int positionInf);
            _ = int.TryParse(nominal, out int nominalNumber);

            switch (identificationId)
            {

                case IdentificationChanger.LETTERS:
                    changesPositionDTO.Values = _artifactRecordService.GenerateLetters(positionSup + positionInf + 1, nominalNumber, reversed);
                    changesPositionDTO.Identification = "Letras";
                    break;
                case IdentificationChanger.NUNBERS:
                    changesPositionDTO.Values = _artifactRecordService.GenerateArrayNum(positionSup + positionInf + 1, nominalNumber, reversed);
                    changesPositionDTO.Identification = "Números";
                    break;
                case IdentificationChanger.R_L_POS_NEG:
                    changesPositionDTO.Values = _artifactRecordService.GenerateArrayRandL("R", "L", positionSup, positionInf, reversed);
                    changesPositionDTO.Identification = "R&L +/-";
                    break;
                case IdentificationChanger.R_L_NEG_POS:
                    changesPositionDTO.Values = _artifactRecordService.GenerateArrayRandL("L", "R", positionSup, positionInf, reversed);
                    changesPositionDTO.Identification = "R&L -/+";
                    break;
                default:
                    break;
            }
            if (changesPositionDTO.Values.Count == 1)
            {
                changesPositionDTO.Values = new List<ValuesChanges>() { new ValuesChanges() { Value = "NOM", Selected = true } };

            }
            return Json(new
            {
                response = changesPositionDTO
            });
        }

        public IActionResult Error() => View();

        public async Task<IActionResult> GetTypeAfroid([FromBody] ArtifactRecordViewModel viewModel)
        {
            if (viewModel is not null)
            {
                string response;
                try
                {

                    InformationArtifactDTO dto = _mapper.Map<InformationArtifactDTO>(viewModel);
                    dto.TapBaan = _mapper.Map<TapBaanDTO>(viewModel);

                    response = await ObtenerTipoAparato(dto);
                    response = GetAcronymTransformer(response);
                }
                catch (Exception)
                {
                    return BadRequest();
                }

                return Json(new
                {
                    response
                });
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> GetNorm([FromBody] ArtifactRecordViewModel viewModel)
        {
            if (viewModel is not null)
            {
                string response = string.Empty;

                IEnumerable<CatSidcoDTO> catSidco = await _sidcoClientService.GetCatSIDCO();

                foreach (CatSidcoDTO obj in catSidco)
                {
                    if (viewModel.StandardId.Equals(obj.Id.ToString()))
                    {
                        response = obj.ClaveSPL;
                    }
                }

                return Json(new
                {
                    response
                });
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> GetDisplacementEquivalent([FromBody] ArtifactRecordViewModel viewModel)
        {
            if (viewModel is not null)
            {
                string response = string.Empty;

                IEnumerable<CatSidcoDTO> catSidco = await _sidcoClientService.GetCatSIDCO();

                foreach (CatSidcoDTO obj in catSidco)
                {
                    if (viewModel.PolarityId.Equals(obj.Id.ToString()))
                    {
                        response = obj.ClaveSPL;
                    }
                }

                return Json(new
                {
                    response
                });
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> GetLanguageEquivalent([FromBody] ArtifactRecordViewModel viewModel)
        {
            if (viewModel is not null)
            {
                string response = string.Empty;

                IEnumerable<CatSidcoDTO> catSidco = await _sidcoClientService.GetCatSIDCO();

                foreach (CatSidcoDTO obj in catSidco)
                {
                    if (viewModel.StandardId.Equals(obj.Id.ToString()))
                    {
                        response = obj.ClaveSPL;
                    }
                }

                return Json(new
                {
                    response
                });
            }
            else
            {
                return BadRequest();
            }
        }

        #region PrivateMethods
        public async Task PrepareForm()
        {
            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Id = 0, Description = "Seleccione..." } }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetTypeTransformers, (int)MicroservicesEnum.splsidco));

            ViewBag.TypetrafoItems = new SelectList(generalProperties, "Id", "Description");

            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetApplications, (int)MicroservicesEnum.splsidco));
            ViewBag.ApplicationidItems = new SelectList(generalProperties, "Id", "Description");

            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetApplicableRule, (int)MicroservicesEnum.splsidco));
            ViewBag.NormaItems = new SelectList(generalProperties, "Id", "Description");

            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLenguages, (int)MicroservicesEnum.splsidco));
            ViewBag.LanguageItems = new SelectList(generalProperties, "Id", "Description");

            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetUnitTypes, (int)MicroservicesEnum.splmasters));
            ViewBag.TipoUnidadItems = new SelectList(generalProperties, "Clave", "Descripcion");

            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");

            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetRulesEquivalents, (int)MicroservicesEnum.splmasters));
            ViewBag.NormaEquivalenteItems = new SelectList(generalProperties, "Clave", "Descripcion");

            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetAngularDisplacement, (int)MicroservicesEnum.splsidco));
            ViewBag.PolarityItems = new SelectList(generalProperties, "Id", "Description");

            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetEquivalentsAngularDisplacement, (int)MicroservicesEnum.splmasters));

            ViewBag.DesplazamientoAngularItems = new SelectList(generalProperties, "Clave", "Descripcion");

            generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetConnectionTypes, (int)MicroservicesEnum.splsidco));

            ViewBag.ConexionItems = new SelectList(generalProperties, "Id", "Description");
            ViewBag.TabBaanIdentificationItems = new SelectList(generalProperties, "Id", "Description");
            ViewBag.TabBaanItems = new SelectList(generalProperties, "Id", "Description");
            ViewBag.TipoDerivacionItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Seleccione...",
                    Value = ""
                },
                new SelectListItem
                {
                    Text = "RCBN",
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = "FCBN",
                    Value = "1"
                }
            };

            ViewBag.OilTypesItems = new List<SelectListItem>
            {
                 new SelectListItem
                {
                    Text = "Seleccione...",
                    Value = ""
                },
                new SelectListItem
                {
                    Text = "Sintético",
                    Value = "Sintético"
                },
                new SelectListItem
                {
                    Text = "Mineral",
                    Value = "Mineral",
                    Selected = true
                },
                new SelectListItem
                {
                    Text = "Vegetal",
                    Value = "Vegetal"
                }
            };

            ViewBag.ConditionOperativeItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Seleccione...",
                    Value = ""
                },
                new SelectListItem
                {
                    Text = "RCBN",
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = "FCBN",
                    Value = "1"
                }
            };

            ViewBag.IdentificationItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Seleccione...",
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = "Letras",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "Números",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "R&L +/-",
                    Value = "3"
                },
                new SelectListItem
                {
                    Text = "R&L -/+",
                    Value = "4"
                }
            };

            ViewBag.Ubicacion = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Seleccione...",
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = "Ter",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "Bt",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "At",
                    Value = "3"
                },
                new SelectListItem
                {
                    Text = "Dev Com",
                    Value = "4"
                },
                new SelectListItem
                {
                    Text = "Dev Ser",
                    Value = "5"
                },
                new SelectListItem
                {
                    Text = "No",
                    Value = "6"
                }
            };
        }

        private void FixDataFromSidco(ref ArtifactRecordViewModel viewModel)
        {

            if (viewModel.Fechacreacion <= DateTime.MinValue)
                viewModel.Fechacreacion = DateTime.UtcNow;
            foreach (NozzlesArtifactViewModel item in viewModel.NozzlesArtifacts)
            {
                if (item.CreadoPor is null || string.IsNullOrEmpty(item.CreadoPor) || string.IsNullOrWhiteSpace(item.CreadoPor))
                    item.CreadoPor = viewModel.Creadopor;
                if (item.ModificadoPor is null || string.IsNullOrEmpty(item.ModificadoPor) || string.IsNullOrWhiteSpace(item.ModificadoPor))
                    item.ModificadoPor = viewModel.Modificadopor;
                item.FechaModificacion ??= viewModel.Fechamodificacion;
                if (item.FechaCreacion <= DateTime.MinValue)
                    item.FechaCreacion = DateTime.UtcNow;

                item.OrderCode = viewModel.OrderCode;
            }

            viewModel.LabTestsArtifact.OrderCode = viewModel.OrderCode;
            viewModel.LabTestsArtifact.CreadoPor = viewModel.Creadopor;
            viewModel.LabTestsArtifact.ModificadoPor = viewModel.Modificadopor;
            viewModel.LabTestsArtifact.FechaCreacion = viewModel.Fechacreacion;
            viewModel.LabTestsArtifact.FechaModificacion = viewModel.Fechamodificacion;

            foreach (ChangingTablesArtifacViewModel item in viewModel.ChangingTablesArtifacs)
            {

                if (item.Creadopor is null || string.IsNullOrEmpty(item.Creadopor) || string.IsNullOrWhiteSpace(item.Creadopor))
                    item.Creadopor = viewModel.Creadopor;
                if (item.ModificadoPor is null || string.IsNullOrEmpty(item.ModificadoPor) || string.IsNullOrWhiteSpace(item.ModificadoPor))
                    item.ModificadoPor = viewModel.Modificadopor;
                item.FechaModificacion ??= viewModel.Fechamodificacion;
                if (item.FechaCreacion <= DateTime.MinValue)
                    item.FechaCreacion = DateTime.UtcNow;

                item.OrderCode = viewModel.OrderCode;
            }
            foreach (LightningRodArtifactViewModel item in viewModel.LightningRodArtifacts)
            {
                if (item.CreadoPor is null || string.IsNullOrEmpty(item.CreadoPor) || string.IsNullOrWhiteSpace(item.CreadoPor))
                    item.CreadoPor = viewModel.Creadopor;
                if (item.ModificadoPor is null || string.IsNullOrEmpty(item.ModificadoPor) || string.IsNullOrWhiteSpace(item.ModificadoPor))
                    item.ModificadoPor = viewModel.Modificadopor;
                item.FechaModificacion ??= viewModel.Fechamodificacion;
                if (item.FechaCreacion <= DateTime.MinValue)
                    item.FechaCreacion = DateTime.UtcNow;

                item.OrderCode = viewModel.OrderCode;
            }
            int iterator = 0;
            foreach (CharacteristicsArtifactViewModel item in viewModel.CharacteristicsArtifacts)
            {
                if (item.CreadoPor is null || string.IsNullOrEmpty(item.CreadoPor) || string.IsNullOrWhiteSpace(item.CreadoPor))
                    item.CreadoPor = viewModel.Creadopor;
                if (item.ModificadoPor is null || string.IsNullOrEmpty(item.ModificadoPor) || string.IsNullOrWhiteSpace(item.ModificadoPor))
                    item.ModificadoPor = viewModel.Modificadopor;
                item.FechaModificacion ??= viewModel.Fechamodificacion;
                if (item.FechaCreacion <= DateTime.MinValue)
                    item.FechaCreacion = DateTime.UtcNow;

                item.Secuencia = iterator.ToString();
                iterator++;
                item.OrderCode = viewModel.OrderCode;
            }

            viewModel.ClaveIdioma = GetAcronym(viewModel.ClaveIdioma);

            iterator = 0;
            foreach (RulesArtifactViewModel item in viewModel.RulesArtifacts)
            {
                if (item.CreadoPor is null || string.IsNullOrEmpty(item.CreadoPor) || string.IsNullOrWhiteSpace(item.CreadoPor))
                    item.CreadoPor = viewModel.Creadopor;
                if (item.ModificadoPor is null || string.IsNullOrEmpty(item.ModificadoPor) || string.IsNullOrWhiteSpace(item.ModificadoPor))
                    item.ModificadoPor = viewModel.Modificadopor;
                item.FechaModificacion ??= viewModel.Fechamodificacion;
                if (item.FechaCreacion <= DateTime.MinValue)
                    item.FechaCreacion = DateTime.UtcNow;

                item.Secuencia = iterator.ToString();
                iterator++;
                item.OrderCode = viewModel.OrderCode;
            }
        }

        private void FixRuleArtifacts(ref ArtifactRecordViewModel viewModel)
        {
            int iterator = 0;
            foreach (RulesArtifactViewModel item in viewModel.RulesArtifacts)
            {
                if (item.CreadoPor is null || string.IsNullOrEmpty(item.CreadoPor) || string.IsNullOrWhiteSpace(item.CreadoPor))
                    item.CreadoPor = viewModel.Creadopor;
                if (item.ModificadoPor is null || string.IsNullOrEmpty(item.ModificadoPor) || string.IsNullOrWhiteSpace(item.ModificadoPor))
                    item.ModificadoPor = viewModel.Modificadopor;
                item.FechaModificacion ??= viewModel.Fechamodificacion;
                if (item.FechaCreacion <= DateTime.MinValue)
                    item.FechaCreacion = DateTime.UtcNow;

                item.Secuencia = iterator.ToString();
                iterator++;
                item.OrderCode = viewModel.OrderCode;
            }
        }

        private string GetAcronym(string language)
        {
            string result = language.ToLower() switch
            {
                "inglés" => "EN",
                "español" => "ES",
                "bilingüe" => "BI",
                _ => "EN",
            };
            return result;

        }

        private string GetZeroLeft(decimal value)
        {
            string valueString = value.ToString();

            return valueString.PadLeft(3, '0');
        }

        private async Task<string> ObtenerTipoAparato(InformationArtifactDTO dto)
        {
            string result = "";
            int? cantidadBoq = 0;
            foreach (NozzlesArtifactDTO boq in dto.NozzlesArtifact)
            {
                cantidadBoq = boq.ColumnTitle == "Terc" ? Convert.ToInt32(boq.Qty) : 0;
            }

            CharacteristicsArtifactDTO Characteristic = dto.CharacteristicsArtifact.FirstOrDefault();

            double tensionTerciario = Convert.ToDouble(Characteristic.Mvaf4);

            IEnumerable<CatSidcoDTO> catSidco = await _sidcoClientService.GetCatSIDCO();

            foreach (CatSidcoDTO obj in catSidco)
            {
                if (dto.GeneralArtifact.TypeTrafoId.ToString().Equals(obj.Id.ToString()))
                {
                    result = obj.ClaveSPL;
                }
                switch (result)
                {
                    case "TRA":
                        if (dto.VoltageKV.TensionKvAltaTension1 > 0 && dto.VoltageKV.TensionKvBajaTension1 > 0 && (dto.VoltageKV.TensionKvSegundaBaja1 == 0 || dto.VoltageKV.TensionKvSegundaBaja1 == null) && dto.VoltageKV.TensionKvTerciario1 > 0 && cantidadBoq > 0)
                        {
                            result = "3 Devanados";
                        }
                        else if (dto.VoltageKV.TensionKvAltaTension1 > 0 && dto.VoltageKV.TensionKvBajaTension1 > 0 && (dto.VoltageKV.TensionKvSegundaBaja1 == 0 || dto.VoltageKV.TensionKvSegundaBaja1 == null) && dto.VoltageKV.TensionKvTerciario1 > 0 && (cantidadBoq == 0 || cantidadBoq == null))
                        {
                            result = "2 Devanados";
                        }
                        else if (dto.VoltageKV.TensionKvAltaTension1 > 0 && dto.VoltageKV.TensionKvBajaTension1 > 0 && (dto.VoltageKV.TensionKvSegundaBaja1 == 0 || dto.VoltageKV.TensionKvSegundaBaja1 == null) && (dto.VoltageKV.TensionKvTerciario1 == 0 || dto.VoltageKV.TensionKvTerciario1 == null))
                        {
                            result = "2 Devanados";
                        }
                        else if (dto.VoltageKV.TensionKvAltaTension1 > 0 && dto.VoltageKV.TensionKvBajaTension1 > 0 && dto.VoltageKV.TensionKvSegundaBaja1 > 0 && (dto.VoltageKV.TensionKvTerciario1 == 0 || dto.VoltageKV.TensionKvTerciario1 == null))
                        {
                            result = "3 Devanados";
                        }

                        break;
                    case "REA":
                        result = "Reactor";
                        break;
                    case "AUT":

                        result = tensionTerciario > 0 && cantidadBoq > 0 ? "Auto con Terciario" : "Auto sin Terciario";
                        break;
                }
            }
            return result;
        }
        private string GetAcronymTransformer(string value) => value.ToLower() switch
        {
            "2 devanados" => "2DE",
            "3 devanados" => "3DE",
            "reactor" => "REA",
            "auto con terciario" => "ACT",
            "auto sin terciario" => "AST",
            _ => "",
        };

        public async Task<IActionResult> GetRulesRep(string pRule, string pLanguage)
        {
            IEnumerable<RulesArtifactDTO> ruleIenumable = await _masterHttpClientService.GetRuleFromSidco(pRule, pLanguage);

            return Json(new
            {
                response = ruleIenumable
            });

        }

        public async Task<IActionResult> GetEquivalencia(int pTipoConexion, string pclaveIdioma)
        {

            IEnumerable<CatSidcoOthersDTO> catSidcoOthers = await _masterHttpClientService.GetCatSidcoOthers();
            string result = "";

            foreach (CatSidcoOthersDTO other in catSidcoOthers)
            {
                if (pTipoConexion == Convert.ToInt16(other.Id) && pclaveIdioma == other.ClaveIdioma)
                {
                    result = other.Descripcion;
                    break;
                }
                else if (pTipoConexion == 0)
                {
                    result = "";
                    break;
                }
            }

            return Json(new
            {
                response = result
            });

        }
        #endregion

    }
}
