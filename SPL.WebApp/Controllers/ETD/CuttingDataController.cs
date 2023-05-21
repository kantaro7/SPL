namespace SPL.WebApp.Controllers.ETD
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ETD;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.ViewModels.ETD;

    public class CuttingDataController : Controller
    {
        private readonly IETDClientService _etdClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IArtifactClientService _artifactClientService;

        private readonly IProfileSecurityService _profileClientService;
        private readonly IRctClientService _rctClientService;
        public CuttingDataController(
            IETDClientService etdClientService,
            IGatewayClientService gatewayClientService,
            IArtifactClientService artifactClientService,
            IConfigurationClientService configClientService,
            IProfileSecurityService profileClientService, IRctClientService rctClientService)
        {
            _etdClientService = etdClientService;
            _gatewayClientService = gatewayClientService;
            _artifactClientService = artifactClientService;

            _profileClientService = profileClientService;
            _rctClientService = rctClientService;
        }

        public Task<IActionResult> Index()
        {

            ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

            if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.DatosdeCortedeElevacióndeTemperaturaETD)))
            {
                CuttingDataViewModel view = new();
                IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();
                ViewBag.PositionsAT = new SelectList(origingeneralProperties, "Clave", "Descripcion");
                ViewBag.PositionsBT = new SelectList(origingeneralProperties, "Clave", "Descripcion");
                ViewBag.PositionsTER = new SelectList(origingeneralProperties, "Clave", "Descripcion");
                ViewBag.CoolingTypes = new SelectList(origingeneralProperties, "Clave", "Descripcion");
                ViewBag.UltHora = new SelectList(origingeneralProperties, "Clave", "Descripcion");
                ViewBag.FirstCuts = new SelectList(origingeneralProperties, "Clave", "Descripcion");
                ViewBag.SecondCuts = new SelectList(origingeneralProperties, "Clave", "Descripcion");
                ViewBag.ThirdCuts = new SelectList(origingeneralProperties, "Clave", "Descripcion");

                return Task.FromResult((IActionResult)View(view));
            }
            else
            {
                return Task.FromResult((IActionResult)View("~/Views/PageConstruction/PermissionDenied.cshtml"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(string noSerie)
        {
            noSerie = noSerie.ToUpper().Trim();
            string noSerieSimple;
            CuttingDataViewModel viewModel = new();
            if (!string.IsNullOrEmpty(noSerie))
            {
                noSerieSimple = noSerie.Split("-")[0];
            }
            else
            {
                return Json(new
                {
                    response = new ApiResponse<CuttingDataViewModel>
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
                    response = new ApiResponse<CuttingDataViewModel>
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
                        response = new ApiResponse<CuttingDataViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no encontrado",
                            Structure = null
                        }
                    });
                }

                if (artifactDesing.GeneralArtifact is null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<CuttingDataViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee información general",
                            Structure = null
                        }
                    });
                }

                // CAP
                if (artifactDesing.CharacteristicsArtifact.Count == 0)
                {
                    return Json(new
                    {
                        response = new ApiResponse<CuttingDataViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee características",
                            Structure = null
                        }
                    });
                }

                // CAR
                if (artifactDesing.VoltageKV is null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<CuttingDataViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee voltajes",
                            Structure = null
                        }
                    });
                }

                // TAPS
                if (artifactDesing.TapBaan is null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<CuttingDataViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee información de cambiadores",
                            Structure = null
                        }
                    });
                }

                ApiResponse<List<HeaderCuttingDataDTO>> listCuttingData = await _etdClientService.GetCuttingDatas(noSerie);
                ApiResponse<List<StabilizationDataDTO>> listStabilizationCutingData = await _etdClientService.GetStabilizationData(noSerie, true, true);

                if (listStabilizationCutingData.Structure.Count == 0)
                {
                    return Json(new
                    {
                        response = new ApiResponse<CuttingDataViewModel>
                        {
                            Code = -1,
                            Description = "No existen datos de estabilizacion estabilizados para el artefacto ingresado",
                            Structure = viewModel
                        }
                    });
                }

                listCuttingData.Structure.ForEach(cutting => cutting.StabilizationData = listStabilizationCutingData.Structure.FirstOrDefault(x => x.IdReg == cutting.IdReg));

                if (listCuttingData.Structure.Exists(x => x.StabilizationData is null))
                {
                    return Json(new
                    {
                        response = new ApiResponse<CuttingDataViewModel>
                        {
                            Code = -1,
                            Description = "Error de Data. Existen registros que no poseen datos de estabilizacion asociados validos",
                            Structure = viewModel
                        }
                    });
                }

                viewModel.HeaderCuttingDatas = listCuttingData.Structure;
                viewModel.NoSerie = noSerie;
            }

            return Json(new
            {
                response = new ApiResponse<CuttingDataViewModel>
                {
                    Code = 1,
                    Description = "",
                    Structure = viewModel
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidDataCuttingNew([FromBody] CuttingDataViewModel viewModel)
        {
            string noSerieSimple;

            List<string> list = new() { "Selecciones..." };

            if (!string.IsNullOrEmpty(viewModel.NoSerie))
            {
                noSerieSimple = viewModel.NoSerie.Split("-")[0];
            }
            else
            {
                return Json(new
                {
                    response = new ApiResponse<CuttingDataViewModel>
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
                    response = new ApiResponse<CuttingDataViewModel>
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
                        response = new ApiResponse<CuttingDataViewModel>
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
                        response = new ApiResponse<CuttingDataViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee características",
                            Structure = null
                        }
                    });
                }

                // CAR
                if (artifactDesing.VoltageKV is null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<CuttingDataViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee voltajes",
                            Structure = null
                        }
                    });
                }

                // TAPS
                if (artifactDesing.TapBaan is null)
                {
                    return Json(new
                    {
                        response = new ApiResponse<CuttingDataViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee información de cambiadores",
                            Structure = null
                        }
                    });
                }

                ApiResponse<List<StabilizationDataDTO>> establizacion = await _etdClientService.GetStabilizationData(viewModel.NoSerie, true, true);

                if (establizacion.Structure.Count == 0)
                {
                    return Json(new
                    {
                        response = new ApiResponse<CuttingDataViewModel>
                        {
                            Code = -1,
                            Description = "Para el número de serie no existen datos de estabilización",
                            Structure = null
                        }
                    });
                }

                viewModel.StabilizationDatas = establizacion.Structure;

                viewModel.CoolingTypes = artifactDesing.CharacteristicsArtifact.ConvertAll(item => new OptionsViewModel() { Value = Convert.ToDecimal(item.OverElevation), Description = item.CoolingType });

                viewModel.Dates = viewModel.StabilizationDatas.ConvertAll(item => new OptionsViewModel() { Value = Convert.ToDecimal(item.IdReg), Description = item.FechaDatos.ToString() });
                ApiResponse<PositionsDTO> dataSelect = await _gatewayClientService.GetPositions(viewModel.NoSerie);

                if (dataSelect.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<CuttingDataViewModel>
                        {
                            Code = -1,
                            Description = dataSelect.Description,
                            Structure = null
                        }
                    });
                }

                viewModel.PositionsAT = dataSelect.Structure.AltaTension.Count() > 1 ? list.Concat(dataSelect.Structure.AltaTension).ToList() : dataSelect.Structure.AltaTension;
                viewModel.PositionsBT = dataSelect.Structure.BajaTension.Count() > 1 ? list.Concat(dataSelect.Structure.BajaTension).ToList() : dataSelect.Structure.BajaTension;
                viewModel.PositionsTER = dataSelect.Structure.Terciario.Count() > 1 ? list.Concat(dataSelect.Structure.Terciario).ToList() : dataSelect.Structure.Terciario;

            }

            return Json(new
            {
                response = new ApiResponse<CuttingDataViewModel>
                {
                    Code = 1,
                    Description = "",
                    Structure = viewModel
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidDataCuttingFecha([FromBody] CuttingDataViewModel viewModel)
        {
            viewModel.CoolingType = viewModel.StabilizationData.CoolingType;
            viewModel.OverElevation = viewModel.StabilizationData.OverElevation;
            viewModel.PosAT = viewModel.StabilizationData.PosAt;
            viewModel.PosBT = viewModel.StabilizationData.PosBt;
            viewModel.PosTer = viewModel.StabilizationData.PosTer;
            viewModel.Capacidad = viewModel.StabilizationData.Capacidad.ToString();
            viewModel.Perdidas = viewModel.StabilizationData.Perdidas.ToString();
            viewModel.Corriente = viewModel.StabilizationData.Corriente.ToString();

            List<OptionsViewModel> ops = viewModel.StabilizationData.StabilizationDataDetails.ConvertAll(x => new OptionsViewModel() { Value = Convert.ToDecimal(x.IdReg), Description = x.FechaHora.ToString() });
            viewModel.LastHours = ops;
            viewModel.FirstCuts = ops;
            viewModel.SecondCuts = ops;
            viewModel.ThirdCuts = ops;

            List<string> list = new();

            string noSerieSimple;

            if (!string.IsNullOrEmpty(viewModel.NoSerie))
            {
                noSerieSimple = viewModel.NoSerie.Split("-")[0];
            }
            else
            {
                return Json(new
                {
                    response = new ApiResponse<CuttingDataViewModel>
                    {
                        Code = -1,
                        Description = "No. Serie inválido.",
                        Structure = null
                    }
                });
            }

            InformationArtifactDTO artifactDesing = await _artifactClientService.GetArtifact(noSerieSimple);

            List<decimal?> mavaf3 = artifactDesing.CharacteristicsArtifact.ConvertAll(x => x.Mvaf3);
            List<decimal?> mavaf4 = artifactDesing.CharacteristicsArtifact.ConvertAll(x => x.Mvaf4);

            List<GeneralPropertiesDTO> selectTer = new();
            bool segundabaja = false;
            bool terciario = false;
            if (artifactDesing.CharacteristicsArtifact.Where(x => x.Mvaf3 > 0).Count() > 0)
            {
                selectTer.Add(new GeneralPropertiesDTO() { Clave = "2B", Descripcion = "2da. Baja" });
                segundabaja = true;
            }

            if (artifactDesing.CharacteristicsArtifact.Where(x => x.Mvaf4 > 0).Count() > 0)
            {
                selectTer.Add(new GeneralPropertiesDTO() { Clave = "CT", Descripcion = "Con Terciario" });
                terciario = true;
            }

            if (artifactDesing.CharacteristicsArtifact.Count(x => x.Mvaf3 > 0) == 0 && artifactDesing.CharacteristicsArtifact.Count(x => x.Mvaf4 > 0) == 0)
            {
                ApiResponse<RCTTestsGeneralDTO> dataRCT = await _rctClientService.GetInfoReport(viewModel.NoSerie, "ATI", true);
                if (dataRCT.Structure is null)
                {
                    dataRCT = await _rctClientService.GetInfoReport(viewModel.NoSerie, "ABS", true);
                    if (dataRCT.Structure is null)
                    {
                        dataRCT = await _rctClientService.GetInfoReport(viewModel.NoSerie, "ABI", true);
                    }
                }

                List<RCTTestsDetailsDTO> listDetalles = new();
                if (dataRCT.Structure != null)
                {
                    foreach (RCTTestsDTO item in dataRCT.Structure.RCTTests)
                    {
                        listDetalles.AddRange(item.RCTTestsDetails.Where(itemdeta => viewModel.StabilizationData.PosAt.Equals(itemdeta.Position) || viewModel.StabilizationData.PosBt.Equals(itemdeta.Position)));
                    }
                }
            }
            else
            {
                if (terciario && !segundabaja)
                {
                    ApiResponse<RCTTestsGeneralDTO> dataRCT = await _rctClientService.GetInfoReport(viewModel.NoSerie, "ATI", true);
                    if (dataRCT.Structure is null)
                    {
                        dataRCT = await _rctClientService.GetInfoReport(viewModel.NoSerie, "ABS", true);
                        if (dataRCT.Structure is null)
                        {
                            dataRCT = await _rctClientService.GetInfoReport(viewModel.NoSerie, "ABI", true);
                            if (dataRCT.Structure is null)
                            {
                                dataRCT = await _rctClientService.GetInfoReport(viewModel.NoSerie, "TSI", true);
                                if (dataRCT.Structure is null)
                                {
                                    dataRCT = await _rctClientService.GetInfoReport(viewModel.NoSerie, "TIS", true);
                                }
                            }
                        }
                    }

                    List<RCTTestsDetailsDTO> listDetalles = new();

                    foreach (RCTTestsDTO item in dataRCT.Structure.RCTTests)
                    {
                        listDetalles.AddRange(item.RCTTestsDetails.Where(itemdeta => viewModel.StabilizationData.PosAt.Equals(itemdeta.Position) || viewModel.StabilizationData.PosBt.Equals(itemdeta.Position)));
                    }
                }
                else
                {
                    if (!terciario && segundabaja)
                    {
                        ApiResponse<RCTTestsGeneralDTO> dataRCT = await _rctClientService.GetInfoReport(viewModel.NoSerie, "-1", true);
                        List<RCTTestsDetailsDTO> listDetalles = new();
                        foreach (RCTTestsDTO item in dataRCT.Structure.RCTTests)
                        {
                            listDetalles.AddRange(item.RCTTestsDetails.Where(itemdeta => viewModel.StabilizationData.PosAt.Equals(itemdeta.Position) || viewModel.StabilizationData.PosBt.Equals(itemdeta.Position)));
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            response = new ApiResponse<CuttingDataViewModel>
                            {
                                Code = -1,
                                Description = "Para la fecha indicada no existen datos de estabilización",
                                Structure = null
                            }
                        });

                    }
                }
            }

            DataTable dtValorNom = new();
            try
            {
                _ = dtValorNom.Columns.Add("TIEMPO");
                _ = dtValorNom.Columns.Add("RESISTENCIA");
                _ = dtValorNom.Columns.Add("TEMPERATURA");

            }
            catch (Exception)
            {
                throw new Exception("Ocurrio un error al crear data table.");
            }

            for (decimal i = 3; i <= 10; i += 0.15m)
            {
                DataRow dtRow = dtValorNom.NewRow();
                dtRow["TIEMPO"] = i;
                dtRow["RESISTENCIA"] = "";
                dtRow["TEMPERATURA"] = "";
                dtValorNom.Rows.Add(dtRow);
            }

            viewModel.TableAT = dtValorNom;
            viewModel.TableBT = dtValorNom;
            viewModel.TableTER = dtValorNom;
            viewModel.SectionCuttingData1 = new();
            viewModel.SectionCuttingData2 = new();
            viewModel.SectionCuttingData3 = new();

            ApiResponse<PositionsDTO> dataSelect = await _gatewayClientService.GetPositions(viewModel.NoSerie);

            return dataSelect.Code == -1
                ? Json(new
                {
                    response = new ApiResponse<CuttingDataViewModel>
                    {
                        Code = -1,
                        Description = dataSelect.Description,
                        Structure = null
                    }
                })
                : (IActionResult)Json(new
                {
                    response = new ApiResponse<CuttingDataViewModel>
                    {
                        Code = 1,
                        Description = "",
                        Structure = viewModel
                    }
                });
        }

        [HttpPost]
        public IActionResult ValidDataCuttingUltimaHora([FromBody] CuttingDataViewModel viewModel)
        {
            StabilizationDetailsDataDTO dataUltimaHora = viewModel.StabilizationData.StabilizationDataDetails.FirstOrDefault(x => x.FechaHora == Convert.ToDateTime(viewModel.LastHour));
            viewModel.Kw_Prueba = dataUltimaHora.KwMedidos.ToString();
            viewModel.Corriente = dataUltimaHora.AmpMedidos.ToString();
            viewModel.Ambiente_prom = dataUltimaHora.AmbienteProm.ToString();
            viewModel.TORxAltitud = dataUltimaHora.Tor.ToString();
            viewModel.AORxAltitud = dataUltimaHora.Aor.ToString();
            return Json(new
            {
                response = new ApiResponse<CuttingDataViewModel>
                {
                    Code = 1,
                    Description = "",
                    Structure = viewModel
                }
            });
        }

        [HttpPost]
        public IActionResult ValidDataCuttingPrimerCorte([FromBody] CuttingDataViewModel viewModel)
        {
            StabilizationDetailsDataDTO dataUltimaHora = viewModel.StabilizationData.StabilizationDataDetails.FirstOrDefault(x => x.FechaHora == Convert.ToDateTime(viewModel.FirstCut));
            viewModel.SectionCuttingData1 = new SectionCuttingDataDTO
            {
                TempAceiteProm = dataUltimaHora.AmbienteProm + dataUltimaHora.AorCorr
            };
            return Json(new
            {
                response = new ApiResponse<CuttingDataViewModel>
                {
                    Code = 1,
                    Description = "",
                    Structure = viewModel
                }
            });
        }

        [HttpPost]
        public IActionResult ValidDataCuttingSegundoCorte([FromBody] CuttingDataViewModel viewModel)
        {
            StabilizationDetailsDataDTO dataUltimaHora = viewModel.StabilizationData.StabilizationDataDetails.FirstOrDefault(x => x.FechaHora == Convert.ToDateTime(viewModel.SecondCut));
            viewModel.SectionCuttingData2 = new SectionCuttingDataDTO
            {
                TempAceiteProm = dataUltimaHora.AmbienteProm + dataUltimaHora.AorCorr
            };
            return Json(new
            {
                response = new ApiResponse<CuttingDataViewModel>
                {
                    Code = 1,
                    Description = "",
                    Structure = viewModel
                }
            });
        }

        [HttpPost]
        public IActionResult ValidDataCuttingTercerCorte([FromBody] CuttingDataViewModel viewModel)
        {
            StabilizationDetailsDataDTO dataUltimaHora = viewModel.StabilizationData.StabilizationDataDetails.FirstOrDefault(x => x.FechaHora == Convert.ToDateTime(viewModel.ThirdCut));
            viewModel.SectionCuttingData3 = new SectionCuttingDataDTO
            {
                TempAceiteProm = dataUltimaHora.AmbienteProm + dataUltimaHora.AorCorr
            };

            return Json(new
            {
                response = new ApiResponse<CuttingDataViewModel>
                {
                    Code = 1,
                    Description = "",
                    Structure = viewModel
                }
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetCuttingDataEspecificoAsync(decimal IdCuttiingData, string NoSerie)
        {
            ApiResponse<HeaderCuttingDataDTO> CuttingData = await _etdClientService.GetInfoHeaderCuttingData(IdCuttiingData);
            ApiResponse<List<StabilizationDataDTO>> listStabilizationCutingData = await _etdClientService.GetStabilizationData(NoSerie, true, true);

            CuttingData.Structure.StabilizationData = listStabilizationCutingData.Structure.FirstOrDefault(x => x.IdReg == CuttingData.Structure.StabilizationData.IdReg);
            return Json(new
            {
                response = CuttingData.Structure
            });
        }

        [HttpGet]
        public async Task<IActionResult> Close(string noSerie, int id)
        {
            if (string.IsNullOrEmpty(noSerie))
                return Json(new { response = new { status = -1, description = "noSerie NULL" } });
            try
            {
                ApiResponse<long> result = await _etdClientService.CloseStabilizationData(noSerie, id);
                return result.Code == -1
                    ? Json(new
                    {
                        response = new { status = -1, description = result.Description }
                    })
                    : (IActionResult)Json(new
                    {
                        response = new { status = 1, structure = result.Structure }
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

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] CuttingDataViewModel viewModel)
        {
            if (viewModel is null)
                return Json(new { response = new { status = -1, description = "ViewModel NULL" } });
            try
            {
                ApiResponse<long> result = await _etdClientService.SaveCuttingData(viewModel.TransforInHeader());
                if(result.Code == -1)
                {
                    return Json(new
                    {
                        response = new { Code = -1, result.Description }
                    });
                }
                else
                {
                    return Json(new
                    {
                        response = new { Code = 1, result.Description, Id = result.Structure}
                    });
                }
                
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    response = new { Code = -1, description = ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ValidateDataTables([FromBody] CuttingDataViewModel viewModel)
        {
            if (viewModel is null)
            {
                return Json(new
                {
                    response = new ApiResponse<CuttingDataViewModel>
                    {
                        Code = -1,
                        Description = "ViewModel NULL",
                        Structure = null
                    }
                });
            }

            try
            {
                if (viewModel.TableAT.AsEnumerable().Where(c => Convert.ToDecimal(c.ItemArray[1]) != 0).Any())
                {
                    foreach (object[] item in viewModel.TableAT.AsEnumerable().Select(c => c.ItemArray))
                    {
                        item[2] = (Convert.ToDecimal(item[1]) * ((Convert.ToDecimal(viewModel.Constante) + viewModel.SectionCuttingData1.TempResistencia) / viewModel.SectionCuttingData1.Resistencia)) - Convert.ToDecimal(viewModel.Constante);
                    }
                }

                if (viewModel.TableBT.AsEnumerable().Where(c => Convert.ToDecimal(c.ItemArray[1]) != 0).Any())
                {
                    foreach (object[] item in viewModel.TableBT.AsEnumerable().Select(c => c.ItemArray))
                    {
                        item[2] = (Convert.ToDecimal(item[1]) * ((Convert.ToDecimal(viewModel.Constante) + viewModel.SectionCuttingData2.TempResistencia) / viewModel.SectionCuttingData2.Resistencia)) - Convert.ToDecimal(viewModel.Constante);
                    }
                }

                if (viewModel.TableTER.AsEnumerable().Where(c => Convert.ToDecimal(c.ItemArray[1]) != 0).Any())
                {
                    foreach (object[] item in viewModel.TableTER.AsEnumerable().Select(c => c.ItemArray))
                    {
                        item[2] = (Convert.ToDecimal(item[1]) * ((Convert.ToDecimal(viewModel.Constante) + viewModel.SectionCuttingData3.TempResistencia) / viewModel.SectionCuttingData3.Resistencia)) - Convert.ToDecimal(viewModel.Constante);
                    }
                }

                //Calcular
                ApiResponse<ResultCuttingDataTestsDTO> result = await _etdClientService.CalculateCuttingData(viewModel.TransforInHeader());
                if (result.Code.Equals(-1))
                {
                    return Json(new
                    {
                        response = new ApiResponse<CuttingDataViewModel>
                        {
                            Code = -1,
                            Description = result.Description,
                            Structure = viewModel
                        }
                    });
                }
                bool resultReport = !result.Structure.MessageErrors.Any();
                string errors = string.Empty;
                List<string> errorMessages = result.Structure.MessageErrors.Select(k => k.Message).ToList();
                viewModel.Error = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
                if (resultReport)
                {
                    viewModel.InsertResult(result.Structure.Data);
                }
                return Json(new
                {
                    response = new ApiResponse<CuttingDataViewModel>
                    {
                        Code = 1,
                        Description = "",
                        Structure = viewModel
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    response = new ApiResponse<CuttingDataViewModel>
                    {
                        Code = -1,
                        Description = ex.Message,
                        Structure = viewModel
                    }
                });
            }
        }
    }
}
