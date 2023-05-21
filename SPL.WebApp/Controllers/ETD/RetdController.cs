namespace SPL.WebApp.Controllers.ETD
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ETD;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.ViewModels.ETD;

    public class RetdController : Controller
    {
        private readonly IETDClientService _etdClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly IPciClientService _pciClientService;

        public RetdController(
            IETDClientService etdClientService,
            IGatewayClientService gatewayClientService,
            IArtifactClientService artifactClientService,
            IPciClientService pciClientService)
        {
            _etdClientService = etdClientService;
            _gatewayClientService = gatewayClientService;
            _artifactClientService = artifactClientService;
            _pciClientService = pciClientService;
        }

        public IActionResult Index() => View(new RetdViewModel());

        [HttpGet]
        public async Task<IActionResult> GetStabilizationData(string noSerie)
        {

            noSerie = noSerie.ToUpper().Trim();
            string noSerieSimple;
            RetdViewModel viewModel = new();

            try
            {
                if (!string.IsNullOrEmpty(noSerie))
                {
                    noSerieSimple = noSerie.Split("-")[0];
                }
                else
                {
                    return Json(new
                    {
                        response = new ApiResponse<RetdViewModel>
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
                        response = new ApiResponse<RetdViewModel>
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
                            response = new ApiResponse<RetdViewModel>
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
                            response = new ApiResponse<RetdViewModel>
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
                            response = new ApiResponse<RetdViewModel>
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
                            response = new ApiResponse<RetdViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee voltages",
                                Structure = null
                            }
                        });
                    }

                    // Tap
                    if (artifactDesing.TapBaan is null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<RetdViewModel>
                            {
                                Code = -1,
                                Description = "Artefacto no posee garantias",
                                Structure = null
                            }
                        });
                    }

                    viewModel.CoolingTypes = artifactDesing.CharacteristicsArtifact.Select(x => x.CoolingType).ToList();
                    viewModel.OverElevations = artifactDesing.CharacteristicsArtifact.Select(x => x.OverElevation ?? 0).ToList();

                    viewModel.Altitude1 = artifactDesing.GeneralArtifact.AltitudeF1 ?? 0;
                    viewModel.Altitude2 = artifactDesing.GeneralArtifact.AltitudeF2;

                    viewModel.AltitudeFactor = viewModel.Altitude2 is "FASL"
                        ? viewModel.Altitude1 <= 3300m ? 1 : viewModel.Altitude1 / 3300m
                        : viewModel.Altitude2 is "MSNM" ? viewModel.Altitude1 <= 1000m ? 1 : viewModel.Altitude1 / 1000m : 0;
                }

                ApiResponse<PositionsDTO> pos = await _gatewayClientService.GetPositions(noSerie);
                if (pos.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<RetdViewModel>
                        {
                            Code = -1,
                            Description = pos.Description,
                            Structure = null
                        }
                    });
                }
                else
                {
                    viewModel.Positions = pos.Structure;
                }

                ApiResponse<List<StabilizationDataDTO>> result = await _etdClientService.GetStabilizationData(noSerie);
                if (result.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<RetdViewModel>
                        {
                            Code = -1,
                            Description = result.Description,
                            Structure = null
                        }
                    });
                }
                else
                {
                    viewModel.StabilizationDatas = new();
                    foreach (StabilizationDataDTO item in result.Structure)
                    {
                        viewModel.StabilizationDatas.Add(new StabilizationDataViewModel(item));
                    }

                    viewModel.Error = string.Empty;
                    viewModel.NoSerie = noSerie;
                }

                viewModel.StabilizationDatas.ForEach(x => x.NoSerie = noSerie);

                return Json(new
                {
                    response = new ApiResponse<RetdViewModel>
                    {
                        Code = 1,
                        Description = "",
                        Structure = viewModel
                    }
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<RetdViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = viewModel
                    }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Iniciar([FromBody] StabilizationDataViewModel viewModel)
        {

            string warning = string.Empty;
            if (viewModel is null)
                return Json(new { response = new { Code = -1, description = "ViewModel NULL" } });
            try
            {
                string keyTest = viewModel.PosAt is "" ? "BYT" : viewModel.PosBt is "" ? "AYT" : "AYB";

                //ApiResponse<PCITestsGeneralDTO> pciResult = await _pciClientService.GetInfoReportPCI(viewModel.NoSerie, keyTest, true);

                //if (pciResult.Code == -1)
                //{
                //    keyTest = keyTest.Equals("AYB") ? "ABT" : keyTest.Equals("BYT") ? "BTT" : "ATT";
                //    pciResult = await _pciClientService.GetInfoReportPCI(viewModel.NoSerie, keyTest, true);
                //    if (pciResult.Code == -1)
                //    {
                //        return Json(new
                //        {
                //            response = new { Code = -1, Description = $"PCI clave {keyTest} - aparato {viewModel.NoSerie}:   {pciResult.Description}" }
                //        });
                //    }
                //}

                //string[] capacidades = pciResult.Structure.Capacity.Replace("//", "/").Split("/");
                //bool encontrada = false;
                //foreach (string cap in capacidades)
                //{
                //    if (Convert.ToDecimal(cap) == viewModel.Capacidad * 1000)
                //    {
                //        encontrada = true;
                //        break;
                //    }
                //}

                //if (!encontrada)
                //{
                //    return Json(new
                //    {
                //        response = new { Code = -1, Description = $"La capacidad no coincide con alguna de las del reporte PCI (report: {pciResult.Structure.Capacity})" }
                //    });
                //}

                //PCITestsDTO section;
                //PCITestsDetailsDTO detail;
                //PCITestsDetailsDTO top = new() { LossesTotal = 0 };

                string primPosTop = string.Empty;
                if (keyTest is "AYB" or "ABT")
                {
                    //if (pciResult.Structure.PCIOutTests.TapPositionsPrim is "AT")
                    //{
                    //    section = pciResult.Structure.PCIOutTests.PCITests.FirstOrDefault(x => x.ValueTapPositions == viewModel.PosAt);
                    //    if (section is not null)
                    //    {
                    //        detail = section.PCITestsDetails.FirstOrDefault(x => x.Position == viewModel.PosBt);
                    //        if (detail is null)
                    //        {
                    //            return Json(new
                    //            {
                    //                response = new { Code = -1, Description = $"No hay detalle en la seccion de la posicion primaria AT: {viewModel.PosAt} del reporte PCI que tenga como posicion secundaria BT: {viewModel.PosBt}" }
                    //            });
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return Json(new
                    //        {
                    //            response = new { Code = -1, Description = "No hay seccion en el reporte PCI que tenga como posicion primaria AT: " + viewModel.PosAt }
                    //        });
                    //    }
                    //}
                    //else
                    //{
                    //    section = pciResult.Structure.PCIOutTests.PCITests.FirstOrDefault(x => x.ValueTapPositions == viewModel.PosBt);
                    //    if (section is not null)
                    //    {
                    //        detail = section.PCITestsDetails.FirstOrDefault(x => x.Position == viewModel.PosAt);
                    //        if (detail is null)
                    //        {
                    //            return Json(new
                    //            {
                    //                response = new { Code = -1, Description = $"No hay detalle en la seccion de la posicion primaria BT: {viewModel.PosBt} del reporte PCI que tenga como posicion secundaria AT: {viewModel.PosAt}" }
                    //            });
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return Json(new
                    //        {
                    //            response = new { Code = -1, Description = "No hay seccion en el reporte PCI que tenga como posicion primaria BT: " + viewModel.PosBt }
                    //        });
                    //    }
                    //}
                }
                else if (keyTest is "AYT" or "ATT")
                {
                    //if (pciResult.Structure.PCIOutTests.TapPositionsPrim is "AT")
                    //{
                    //    section = pciResult.Structure.PCIOutTests.PCITests.FirstOrDefault(x => x.ValueTapPositions == viewModel.PosAt);
                    //    if (section is not null)
                    //    {
                    //        detail = section.PCITestsDetails.FirstOrDefault(x => x.Position == viewModel.PosTer);
                    //        if (detail is null)
                    //        {
                    //            return Json(new
                    //            {
                    //                response = new { Code = -1, Description = $"No hay detalle en la seccion de la posicion primaria AT: {viewModel.PosAt} del reporte PCI que tenga como posicion secundaria Ter: {viewModel.PosTer}" }
                    //            });
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return Json(new
                    //        {
                    //            response = new { Code = -1, Description = "No hay seccion en el reporte PCI que tenga como posicion primaria AT: " + viewModel.PosAt }
                    //        });
                    //    }
                    //}
                    //else
                    //{
                    //    section = pciResult.Structure.PCIOutTests.PCITests.FirstOrDefault(x => x.ValueTapPositions == viewModel.PosTer);
                    //    if (section is not null)
                    //    {
                    //        detail = section.PCITestsDetails.FirstOrDefault(x => x.Position == viewModel.PosAt);
                    //        if (detail is null)
                    //        {
                    //            return Json(new
                    //            {
                    //                response = new { Code = -1, Description = $"No hay detalle en la seccion de la posicion primaria Ter: {viewModel.PosTer} del reporte PCI que tenga como posicion secundaria AT: {viewModel.PosAt}" }
                    //            });
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return Json(new
                    //        {
                    //            response = new { Code = -1, Description = "No hay seccion en el reporte PCI que tenga como posicion primaria Ter: " + viewModel.PosTer }
                    //        });
                    //    }
                    //}
                }
                else
                {
                    //if (pciResult.Structure.PCIOutTests.TapPositionsPrim is "BT")
                    //{
                    //    section = pciResult.Structure.PCIOutTests.PCITests.FirstOrDefault(x => x.ValueTapPositions == viewModel.PosBt);
                    //    if (section is not null)
                    //    {
                    //        detail = section.PCITestsDetails.FirstOrDefault(x => x.Position == viewModel.PosTer);
                    //        if (detail is null)
                    //        {
                    //            return Json(new
                    //            {
                    //                response = new { Code = -1, Description = $"No hay detalle en la seccion de la posicion primaria BT: {viewModel.PosBt} del reporte PCI que tenga como posicion secundaria Ter: {viewModel.PosTer}" }
                    //            });
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return Json(new
                    //        {
                    //            response = new { Code = -1, Description = "No hay seccion en el reporte PCI que tenga como posicion primaria BT: " + viewModel.PosBt }
                    //        });
                    //    }
                    //}
                    //else
                    //{
                    //    section = pciResult.Structure.PCIOutTests.PCITests.FirstOrDefault(x => x.ValueTapPositions == viewModel.PosTer);
                    //    if (section is not null)
                    //    {
                    //        detail = section.PCITestsDetails.FirstOrDefault(x => x.Position == viewModel.PosBt);
                    //        if (detail is null)
                    //        {
                    //            return Json(new
                    //            {
                    //                response = new { Code = -1, Description = $"No hay detalle en la seccion de la posicion primaria Ter: {viewModel.PosTer} del reporte PCI que tenga como posicion secundaria BT: {viewModel.PosBt}" }
                    //            });
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return Json(new
                    //        {
                    //            response = new { Code = -1, Description = "No hay seccion en el reporte PCI que tenga como posicion primaria Ter: " + viewModel.PosTer }
                    //        });
                    //    }
                    //}
                }

                //pciResult.Structure.PCIOutTests.PCITests.ForEach(section => section.PCITestsDetails.ForEach(det =>
                //{
                //    if (det.LossesTotal > top.LossesTotal)
                //    {
                //        top = det;
                //        primPosTop = section.ValueTapPositions;
                //    }
                //}));

                //if (top.LossesTotal != detail.LossesTotal)
                //{
                //    warning += $"Las posiciones {primPosTop} y {top.Position} tienen las mayores pérdidas que son {top.LossesTotal}";
                //}

                //viewModel.Perdidas = detail.LossesTotal;

                viewModel.StabilizationDataDetails = new();

                viewModel.StabilizationDataDetails.FechaHora.Add((DateTime)viewModel.FechaDatos);

                for (int i = 1; i < 52; i++)
                    viewModel.StabilizationDataDetails.FechaHora.Add(viewModel.StabilizationDataDetails.FechaHora[i - 1]?.AddMinutes(viewModel.Intervalo == 1 ? 60 : 30));

                return warning != string.Empty
                    ? Json(new
                    {
                        response = new { Code = -2, Description = warning, Structure = viewModel }
                    })
                    : (IActionResult)Json(new
                    {
                        response = new { Code = 1, Description = "", Structure = viewModel }
                    });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    response = new { Code = -1, Description = ex.Message }
                });
            }
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
        public async Task<IActionResult> Validate([FromBody] StabilizationDataViewModel viewModel)
        {
            if (viewModel is null)
                return Json(new { response = new { Code = -1, Description = "ViewModel NULL" } });
            try
            {
                ApiResponse<ResultStabilizationDataTestsDTO> result = await _etdClientService.CalculeTestStabilizationData(viewModel.GetStabilizationDataDTO());
                return result.Code == -1
                    ? Json(new
                    {
                        response = new { Code = -1, result.Description }
                    })
                    : (IActionResult)Json(new
                    {
                        response = new { Code = 1, Structure = new StabilizationDataViewModel(result.Structure.Results) }
                    });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    response = new { Code = -1, Description = ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] StabilizationDataViewModel viewModel)
        {
            if (viewModel is null)
                return Json(new { response = new { status = -1, description = "ViewModel NULL" } });
            try
            {
                ApiResponse<long> result = await _etdClientService.SaveStabilizationData(viewModel.GetStabilizationDataDTO());
                if (result.Code == -1)
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
                        response = new { Code = 1, Description = "" }
                    });
                }
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

    /*public class prueba
    {
        public int valor { get; set; }

        public decimal? IdReg { get; set; }
        public string NoSerie { get; set; }
        public DateTime? FechaDatos { get; set; }
        public string CoolingType { get; set; }
        public decimal? OverElevation { get; set; }
        public decimal? FactEnf { get; set; }
        public decimal? Intervalo { get; set; }
        public string UmIntervalo { get; set; }
        public string PosAt { get; set; }
        public string PosBt { get; set; }
        public string PosTer { get; set; }
        public decimal? Capacidad { get; set; }
        public string DevanadoSplit { get; set; }
        public decimal? AltitudeF1 { get; set; }
        public string AltitudeF2 { get; set; }
        public decimal? FactAlt { get; set; }

        public decimal? PorcCarga { get; set; }
        public string Sobrecarga { get; set; }
        public decimal? CantTermoPares { get; set; }
        public decimal? Perdidas { get; set; }
        public decimal? Corriente { get; set; }
        public bool? Estatus { get; set; }
        public int? CantEstables { get; set; }
        public int? CantInestables { get; set; }
        public string Creadopor { get; set; }
        public DateTime? Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }

        public x StabilizationDataDetails { get; set; }
    }

    public class x {
        public List<decimal?> IdReg { get; set; }
        public List<DateTime?> FechaHora { get; set; }
        public List<decimal?> KwMedidos { get; set; }
        public List<decimal?> AmpMedidos { get; set; }
        public List<decimal?> CabSupRadBco1 { get; set; }
        public decimal? CanalSup1 { get; set; }
        public List<decimal?> CabSupRadBco2 { get; set; }
        public decimal? CanalSup2 { get; set; }
        public List<decimal?> CabSupRadBco3 { get; set; }
        public decimal? CanalSup3 { get; set; }
        public List<decimal?> CabSupRadBco4 { get; set; }
        public decimal? CanalSup4 { get; set; }
        public List<decimal?> CabSupRadBco5 { get; set; }
        public decimal? CanalSup5 { get; set; }
        public List<decimal?> CabSupRadBco6 { get; set; }
        public decimal? CanalSup6 { get; set; }
        public List<decimal?> CabSupRadBco7 { get; set; }
        public decimal? CanalSup7 { get; set; }
        public List<decimal?> CabSupRadBco8 { get; set; }
        public decimal? CanalSup8 { get; set; }
        public List<decimal?> CabSupRadBco9 { get; set; }
        public decimal? CanalSup9 { get; set; }
        public List<decimal?> CabSupRadBco10 { get; set; }
        public decimal? CanalSup10 { get; set; }
        public List<decimal?> PromRadSup { get; set; }
        public List<decimal?> CabInfRadBco1 { get; set; }
        public decimal? CanalInf1 { get; set; }
        public List<decimal?> CabInfRadBco2 { get; set; }
        public decimal? CanalInf2 { get; set; }
        public List<decimal?> CabInfRadBco3 { get; set; }
        public decimal? CanalInf3 { get; set; }
        public List<decimal?> CabInfRadBco4 { get; set; }
        public decimal? CanalInf4 { get; set; }
        public List<decimal?> CabInfRadBco5 { get; set; }
        public decimal? CanalInf5 { get; set; }
        public List<decimal?> CabInfRadBco6 { get; set; }
        public decimal? CanalInf6 { get; set; }
        public List<decimal?> CabInfRadBco7 { get; set; }
        public decimal? CanalInf7 { get; set; }
        public List<decimal?> CabInfRadBco8 { get; set; }
        public decimal? CanalInf8 { get; set; }
        public List<decimal?> CabInfRadBco9 { get; set; }
        public decimal? CanalInf9 { get; set; }
        public List<decimal?> CabInfRadBco10 { get; set; }
        public decimal? CanalInf10 { get; set; }
        public List<decimal?> PromRadInf { get; set; }
        public List<decimal?> Ambiente1 { get; set; }
        public decimal? CanalAmb1 { get; set; }
        public List<decimal?> Ambiente2 { get; set; }
        public decimal? CanalAmb2 { get; set; }
        public List<decimal?> Ambiente3 { get; set; }
        public decimal? CanalAmb3 { get; set; }
        public List<decimal?> AmbienteProm { get; set; }
        public List<decimal?> TempTapa { get; set; }
        public decimal? CanalTtapa { get; set; }
        public List<decimal?> Aor { get; set; }
        public List<decimal?> Tor { get; set; }
        public List<decimal?> Bor { get; set; }
        public List<decimal?> AorCorr { get; set; }
        public List<decimal?> TorCorr { get; set; }
        public List<decimal?> Ao { get; set; }
        public List<decimal?> TempInstr1 { get; set; }
        public decimal? CanalInst1 { get; set; }
        public List<decimal?> TempInstr2 { get; set; }
        public decimal? CanalInst2 { get; set; }
        public List<decimal?> TempInstr3 { get; set; }
        public decimal? CanalInst3 { get; set; }
        public List<bool?> VerifVent1 { get; set; }
        public List<bool?> VerifVent2 { get; set; }
        public List<decimal?> Presion { get; set; }
        public List<bool?> Estable { get; set; }
    }*/

}
