namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Azure.Core;

    using Microsoft.AspNetCore.Hosting;
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

    using Telerik.Web.Spreadsheet;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export;
    using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
    using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

    /// <summary>
    /// Relación de Transformación
    /// </summary>
    public class PciController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IPciClientService _pciClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IPciService _pciService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IRodClientService _rodClientService;
        private readonly IPceClientService _pceService;
        private readonly IProfileSecurityService _profileClientService;
        public PciController(
            IMasterHttpClientService masterHttpClientService,
            IPciClientService pciClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IPciService pciService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            IRodClientService rodClientService,
            IPceClientService pceService,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _pciClientService = pciClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _pciService = pciService;
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

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.PérdidasDebidasalaCargaeImpedancia)))
                {

                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new PciViewModel { NoSerie = noSerie });
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
            /* List<int> capacidades = new List<int>();

             var capa = "1_10,180_2".Split(",");

             foreach(var item in capa)
             {
                 var secondSplit = item.Split("_")[0];

                 capacidades.Add(int.Parse(secondSplit) * 1000);
             }

             var y = string.Join(",", capacidades);
             var checkPCE = await this._reportClientService.CheckInfoPce("G4673-01", y,"A,B,C","NOM","0",true,true,false);*/

        }

        [HttpGet]
        public async Task<IActionResult> CheckValidationsRodPci(string nroSerie, string capacity, string windingMaterial, string posAt, string posBt, string posTer, bool isAT, bool isBT, bool isTer)
        {

            if (!string.IsNullOrEmpty(posAt))
            {
                if (posAt[0] == ',')
                {
                    posAt = posAt.Remove(0, 1);
                }
            }

            if (!string.IsNullOrEmpty(posBt))
            {
                if (posBt[0] == ',')
                {
                    posBt = posBt.Remove(0, 1);

                }
            }

            if (!string.IsNullOrEmpty(posTer))
            {
                if (posTer[0] == ',')
                {
                    posTer = posTer.Remove(0, 1);
                }
            }

            List<int> capacidades = new();

            string[] capa = capacity.Split(",");

            foreach (string item in capa)
            {
                string secondSplit = item.Split("_")[0];

                capacidades.Add(int.Parse(secondSplit) * 1000);
            }

            string salida = string.Join(",", capacidades);

            //ApiResponse<PositionsDTO> dataSelect = await this._gatewayClientService.GetPositions("G4673-01");
            ApiResponse<CheckInfoRODDTO> checkROD = await _reportClientService.CheckInfoRod(nroSerie, windingMaterial, string.IsNullOrEmpty(posAt) ? "0" : posAt,
               string.IsNullOrEmpty(posBt) ? "0" : posBt, string.IsNullOrEmpty(posTer) ? "0" : posTer, isAT, isBT, isTer);

            if (checkROD.Code == -1)
            {
                return Json(new
                {
                    response = new ApiResponse<PciViewModel>
                    {
                        Code = -1,
                        Description = checkROD.Description,
                        Structure = null
                    }
                });
            }
            else if (checkROD.Structure == null)
            {
                return Json(new
                {
                    response = new ApiResponse<PciViewModel>
                    {
                        Code = -1,
                        Description = "Error en Estructura de datos al momento de validar la información de ROD",
                        Structure = null
                    }
                });
            }
            else if (string.IsNullOrEmpty(checkROD.Structure.ClavePrueba) || string.IsNullOrEmpty(checkROD.Structure.ConexionPrueba))
            {
                return Json(new
                {
                    response = new ApiResponse<PciViewModel>
                    {
                        Code = -1,
                        Description = "Error al obtener la clave de prueba o la conexion de prueba de ROD",
                        Structure = null
                    }
                });
            }

            ApiResponse<CheckInfoRODDTO> checkPCE = await _reportClientService.CheckInfoPce(nroSerie, salida, string.IsNullOrEmpty(posAt) ? "0" : posAt,
               string.IsNullOrEmpty(posBt) ? "0" : posBt, string.IsNullOrEmpty(posTer) ? "0" : posTer, isAT, isBT, isTer);

            if (checkPCE.Code == -1)
            {
                return Json(new
                {
                    response = new ApiResponse<PciViewModel>
                    {
                        Code = -1,
                        Description = checkPCE.Description,
                        Structure = null
                    }
                });
            }

            return Json(new
            {
                response = new ApiResponse<PciViewModel>
                {
                    Code = 1,
                    Description = "Todo correcto",
                    Structure = new PciViewModel { ClavePruebaFind = checkROD.Structure.ClavePrueba, ConexionPruebaFind = checkROD.Structure.ConexionPrueba }
                }
            });

        }

        public async Task<IActionResult> GetFilter(string noSerie)
        {
            InformationArtifactDTO artifactDesing = new();
            PciViewModel pciViewModel = new();
            noSerie = noSerie.ToUpper().Trim();
            string noSerieSimple = string.Empty;
            try
            {
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
                        response = new ApiResponse<RdtViewModel>
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
                        response = new ApiResponse<RadViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no presente en SPL",
                            Structure = null
                        }
                    });
                }
                else
                {
                    artifactDesing = await _artifactClientService.GetArtifact(noSerieSimple);
                    if (artifactDesing.GeneralArtifact.OrderCode == null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<FpcViewModel>
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
                            response = new ApiResponse<PciViewModel>
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
                    }

                    pciViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
                }

                ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _pciClientService.GetFilter(noSerie);
                List<GeneralPropertiesDTO> tipoUnidad = await _masterHttpClientService.GetUnitTypes();

                GeneralPropertiesDTO match = tipoUnidad.FirstOrDefault(x => x.Clave == artifactDesing.GeneralArtifact.TipoUnidad);
                if (match != null)
                {
                    if (match.Descripcion.ToLower().Contains("auto"))
                    {
                        pciViewModel.EsAutotransformador = true;
                    }
                }

                if (artifactDesing.GeneralArtifact.Phases == 1)
                {
                    pciViewModel.EsMonofasico = true;
                }

                if (resultFilter.Code.Equals(1))
                {
                    InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.PCI.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.PCI.ToString() };
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
                        text = "Reporte Pérdidas debidas a la Carga e Impedancia",
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
                    response = new ApiResponse<PciViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = pciViewModel
                    }
                });
            }
            catch (Exception e)
            {

                return Json(new
                {
                    response = new ApiResponse<PciViewModel>
                    {
                        Code = -1,
                        Description = e.Message,
                        Structure = pciViewModel
                    }
                });
            }
        }

        public async Task<IActionResult> ValidateFilter(string noSerie)
        {
            ApiResponse<PositionsDTO> resultP = await _gatewayClientService.GetPositions(noSerie);

            return resultP.Code == -1
                ? Json(new
                {
                    response = new ApiResponse<bool>
                    {
                        Code = -1,
                        Description = "Error al obtener las posiciones para el numero de Serie " + noSerie,
                        Structure = false
                    }
                })
                : (IActionResult)Json(new
                {
                    response = new ApiResponse<bool>
                    {
                        Code = 1,
                        Description = "",
                        Structure = true
                    }
                });
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string nroSerie,
            string lenguage,
            string windingMaterial,
            bool capRedBaja,
            bool autotransformer,
            bool monofasico,
            decimal overElevation,
            string testCapacity,
            int cantPosPri,
            int cantPosSec,
            string posPi,
            string posSec,
            string posicionesPrimarias,
            string posicionesSecundarias,
            string comment, string keyTestSp, string testConnex)
        {
            string[] testCapArr = testCapacity.Split(",");
            List<string> fixedArray = testCapArr.Select(x =>
            {
                string fixedVal = x.Split("_")[0];
                return fixedVal;
            }).ToList();
            string fixedTestCap = string.Join(",", fixedArray);
            string keyTest = string.Empty;
            if (cantPosSec <= 7)
            {
                if (fixedArray.Count == 1)
                {
                    if ((posPi.ToUpper() == "AT" && posSec.ToUpper() == "BT") || (posPi.ToUpper() == "BT" && posSec.ToUpper() == "AT"))
                        keyTest = "AYB";
                    else if ((posPi.ToUpper() == "AT" && posSec.ToUpper() == "TER") || (posPi.ToUpper() == "TER" && posSec.ToUpper() == "AT"))
                        keyTest = "AYT";
                    else if ((posPi.ToUpper() == "BT" && posSec.ToUpper() == "TER") || (posPi.ToUpper() == "TER" && posSec.ToUpper() == "BT"))
                        keyTest = "BYT";
                }
                else if (fixedArray.Count > 1)
                {
                    if ((posPi.ToUpper() == "AT" && posSec.ToUpper() == "BT") || (posPi.ToUpper() == "BT" && posSec.ToUpper() == "AT"))
                        keyTest = "ABT";
                    else if ((posPi.ToUpper() == "AT" && posSec.ToUpper() == "TER") || (posPi.ToUpper() == "TER" && posSec.ToUpper() == "AT"))
                        keyTest = "ATT";
                    else if ((posPi.ToUpper() == "BT" && posSec.ToUpper() == "TER") || (posPi.ToUpper() == "TER" && posSec.ToUpper() == "BT"))
                        keyTest = "BTT";
                }
            }
            else if (cantPosSec > 7)
            {
                if ((posPi.ToUpper() == "AT" && posSec.ToUpper() == "BT") || (posPi.ToUpper() == "BT" && posSec.ToUpper() == "AT"))
                    keyTest = "AYB";
                else if ((posPi.ToUpper() == "AT" && posSec.ToUpper() == "TER") || (posPi.ToUpper() == "TER" && posSec.ToUpper() == "AT"))
                    keyTest = "AYT";
                else if ((posPi.ToUpper() == "BT" && posSec.ToUpper() == "TER") || (posPi.ToUpper() == "TER" && posSec.ToUpper() == "BT"))
                    keyTest = "BYT";
            }

            ApiResponse<PositionsDTO> resultP = await _gatewayClientService.GetPositions(nroSerie);
            ApiResponse<SettingsToDisplayPCIReportsDTO> result = await _gatewayClientService.GetTemplate(nroSerie, keyTest, lenguage, windingMaterial, capRedBaja, autotransformer, monofasico, overElevation, fixedTestCap, cantPosPri, cantPosSec, posPi, posSec);

            if (result.Code.Equals(-1))
            {
                return View("Excel", new PciViewModel
                {
                    Error = result.Description
                });
            }

            if (string.IsNullOrEmpty(result.Structure.BaseTemplate.Plantilla))
            {
                return View("Excel", new PciViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = result.Structure.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(result.Structure.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");

            _pciService.PrepareTemplate_PCI(result.Structure, ref workbook, keyTest, cantPosPri, cantPosSec, posicionesPrimarias, posicionesSecundarias);

            return View("Excel", new PciViewModel
            {
                ClaveIdioma = lenguage,
                ClavePrueba = keyTest,
                NoPrueba = NoPrueba,
                NoSerie = nroSerie,
                MaterialDevanado = windingMaterial,
                CapacidadReducidaBaja = capRedBaja ? "1" : "0",
                AutoTransformador = autotransformer ? "1" : "0",
                Monofasico = monofasico ? "1" : "0",
                CapacidadPrueba = fixedTestCap,
                CantidadPosicionPrimaria = cantPosPri,
                CantidadPosicionSecundaria = cantPosSec,
                PosicionPrimaria = posPi,
                PosicionSecundaria = posSec,
                SettingsPCI = result.Structure,
                ReigstrosPosicionesPrimarias = posicionesPrimarias.Split(','),
                ReigstrosPosicionesSecundarias = posicionesSecundarias.Split(','),
                Workbook = workbook,
                Error = string.Empty,
                Comments = comment,
                Positions = resultP.Structure,
                ConexionPruebaFind = testConnex,
                ClavePruebaFind = keyTestSp
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] PciViewModel viewModel)
        {
            try
            {
                string error = _pciService.ValidateTemplatePCI(viewModel.SettingsPCI, viewModel.Workbook, viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.CapacidadPrueba, viewModel.CantidadPosicionPrimaria, viewModel.CantidadPosicionSecundaria);

                if (error != string.Empty)
                {
                    return Json(new
                    {
                        response = new ApiResponse<PciViewModel>
                        {
                            Code = -1,
                            Description = error
                        }
                    });
                }

                if (string.IsNullOrEmpty(viewModel.MaterialDevanado))
                {
                    return Json(new
                    {
                        response = new ApiResponse<PciViewModel>
                        {
                            Code = -1,
                            Description = "El material de devanado no puede estar vacio"
                        }
                    });
                }


                string fecha = _pciService.GetDatePCI(viewModel.SettingsPCI, viewModel.Workbook);

                #region Consultas necesarias                

                ApiResponse<List<PlateTensionDTO>> plateTension = await _artifactClientService.GetPlateTension(viewModel.NoSerie, "-1");
                if (plateTension.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<PciViewModel>
                        {
                            Code = -1,
                            Description = plateTension.Description
                        }
                    });
                }
                else
                {
                    if (plateTension.Structure is null)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<PciViewModel>
                            {
                                Code = -1,
                                Description = "No ha sido encontrado tensiones"
                            }
                        });
                    }
                }

                string atPositions = "";
                string btPositions = "";
                string terPositions = "";

                if (viewModel.PosicionPrimaria == "AT")
                {
                    atPositions = string.Join(',', viewModel.ReigstrosPosicionesPrimarias);
                }
                else if (viewModel.PosicionSecundaria == "AT")
                {
                    atPositions = string.Join(',', viewModel.ReigstrosPosicionesSecundarias);
                }

                if (viewModel.PosicionPrimaria == "BT")
                {
                    btPositions = string.Join(',', viewModel.ReigstrosPosicionesPrimarias);
                }
                else if (viewModel.PosicionSecundaria == "BT")
                {
                    btPositions = string.Join(',', viewModel.ReigstrosPosicionesSecundarias);
                }

                if (viewModel.PosicionPrimaria == "Ter")
                {
                    terPositions = string.Join(',', viewModel.ReigstrosPosicionesPrimarias);
                }
                else if (viewModel.PosicionSecundaria == "Ter")
                {
                    terPositions = string.Join(',', viewModel.ReigstrosPosicionesSecundarias);
                }

                ApiResponse<IEnumerable<PCIParameters>> parameters = await _reportClientService.GetAAsync(
                     viewModel.NoSerie,
                     viewModel.MaterialDevanado,
                     string.Join(",", viewModel.CapacidadPrueba),
                     string.IsNullOrEmpty(atPositions) ? "0" : atPositions,
                     string.IsNullOrEmpty(btPositions) ? "0" : btPositions,
                     string.IsNullOrEmpty(terPositions) ? "0" : terPositions,
                     isAT: !string.IsNullOrEmpty(atPositions),
                     isBT: !string.IsNullOrEmpty(btPositions),
                     isTer: !string.IsNullOrEmpty(terPositions));

                #endregion

                #region llenando variable para calculo

                PCIInputTestDTO testOut = new()
                {
                    ReducedCapacityAtLowVoltage = viewModel.CapacidadReducidaBaja != "0",
                    Autotransformer = viewModel.AutoTransformador != "0",
                    Monofasico = viewModel.Monofasico != "0",
                    WindingMaterial = viewModel.MaterialDevanado,
                    Kwcu = viewModel.SettingsPCI.InfotmationArtifact.WarrantiesArtifact.Kwcu ?? 0m,
                    KwcuMva = viewModel.SettingsPCI.InfotmationArtifact.WarrantiesArtifact.KwcuMva ?? 0m,
                    Kwtot100 = viewModel.SettingsPCI.InfotmationArtifact.WarrantiesArtifact.Kwtot100 ?? 0m,
                    TolerancyKwtot = viewModel.SettingsPCI.InfotmationArtifact.WarrantiesArtifact.TolerancyKwtot ?? 0m,
                    NominalSecondaryPosition = viewModel.PosicionSecundaria.ToUpper() == "AT"
                        ? viewModel.Positions.ATNom
                        : viewModel.PosicionSecundaria.ToUpper() == "BT" ? viewModel.Positions.BTNom : viewModel.Positions.TerNom
                };

                error = _pciService.Prepare_PCI_Test(
                    viewModel.SettingsPCI,
                    viewModel.Workbook,
                    viewModel.ClaveIdioma,
                    viewModel.CantidadPosicionPrimaria,
                    viewModel.CantidadPosicionSecundaria,
                    viewModel.CapacidadPrueba,
                    viewModel.PosicionPrimaria,
                    viewModel.ReigstrosPosicionesPrimarias,
                    viewModel.PosicionSecundaria,
                    viewModel.ReigstrosPosicionesSecundarias,
                    plateTension.Structure,
                    parameters.Structure,
                    ref testOut);

                if (error != string.Empty)
                {
                    return Json(new
                    {
                        response = new ApiResponse<PciViewModel>
                        {
                            Code = -1,
                            Description = error
                        }
                    });
                }

                #endregion

                ApiResponse<PCITestResponseDTO> calculateResult = await _pciClientService.CalculateTestPCI(testOut);
                List<string> errores = new();
                if (calculateResult.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<PciViewModel>
                        {
                            Code = -1,
                            Description = "Error en calculo de pruebas PCI: " + calculateResult.Description
                        }
                    });
                }

                Workbook workbook = viewModel.Workbook;

                _pciService.PrepareIndexOfPCI(calculateResult.Structure, viewModel.SettingsPCI, viewModel.ClaveIdioma, viewModel.CantidadPosicionPrimaria, viewModel.CantidadPosicionSecundaria, ref workbook);

                viewModel.Workbook = workbook;

                errores.AddRange(calculateResult.Structure.Messages.Select(k => k.Message).ToList());
                string allErrosPrepare = errores.Any() ? errores.Aggregate((c, n) => c + "\n*" + n) : string.Empty;

                viewModel.IsReportAproved = calculateResult.Structure.IsApproved;
                viewModel.Workbook = workbook;
                viewModel.OfficialWorkbook = workbook;
                viewModel.Result = calculateResult.Structure;

                return Json(new
                {
                    response = new ApiResponse<PciViewModel>
                    {
                        Code = 1,
                        Description = allErrosPrepare,
                        Structure = viewModel
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    response = new ApiResponse<PciViewModel>
                    {
                        Code = -1,
                        Description = ex.Message,
                        Structure = null
                    }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] PciViewModel viewModel)
        {

            /* if (!_pciService.Verify_FPB_Columns(viewModel.Workbook, viewModel.FPBReportsDTO, viewModel.ClavePrueba))
             {
                 return this.Json(new
                 {
                     response = new { status = -1, description = "Faltan datos por ingresar en la tabla" }
                 });
             }
            */

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
                #region LogoTwoPagesAndSaltoPage



                string celda = viewModel.SettingsPCI.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("LogoProlec") && c.Seccion == 1).Celda;
                int posicion = Convert.ToInt32(celda.Remove(0, 1)) - 1;
                image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(posicion, 0), 0, 0);
                string path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
                FileStream stream = new(path, FileMode.Open);
                using (stream)
                {
                    image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                }

                image.Width = 215;
                image.Height = 38;

                sheet.Shapes.Add(image);

                string celdapage = viewModel.SettingsPCI.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado") && c.Seccion == 1).Celda;
                int posicionpage = Convert.ToInt32(celdapage.Remove(0, 1)) - 1;

                PageBreaks pageBreaks = sheet.WorksheetPageSetup.PageBreaks;

                _ = pageBreaks.TryInsertHorizontalPageBreak(posicionpage + 9, 0);


                sheet.WorksheetPageSetup.PaperType = Telerik.Windows.Documents.Model.PaperTypes.A4;
                sheet.WorksheetPageSetup.PageOrientation = Telerik.Windows.Documents.Model.PageOrientation.Portrait;
                //sheet.WorksheetPageSetup.CenterVertically = true;
                sheet.WorksheetPageSetup.CenterHorizontally = true;
                sheet.WorksheetPageSetup.PrintOptions.PrintGridlines = false;
                sheet.WorksheetPageSetup.ScaleFactor = new Telerik.Documents.Primitives.Size(0.9, 1);
                sheet.WorksheetPageSetup.Margins =
                    new Telerik.Windows.Documents.Spreadsheet.Model.Printing.PageMargins(0
                    , 20, 0, 0);
                #endregion

                //image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(0, 0), 0, 0);
                //string path = Path.Combine(this._hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
                //FileStream stream = new(path, FileMode.Open);
                //using (stream)
                //{
                //    image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                //}

                //image.Width = 215;
                //image.Height = 38;

                //sheet.Shapes.Add(image);
                //sheet.WorksheetPageSetup.PaperType = Telerik.Windows.Documents.Model.PaperTypes.A4;
                //sheet.WorksheetPageSetup.PageOrientation = Telerik.Windows.Documents.Model.PageOrientation.Portrait;
                ////sheet.WorksheetPageSetup.CenterVertically = true;
                //sheet.WorksheetPageSetup.CenterHorizontally = true;
                //sheet.WorksheetPageSetup.PrintOptions.PrintGridlines = false;
                //sheet.WorksheetPageSetup.ScaleFactor = new Telerik.Documents.Primitives.Size(0.9, 0.9);
                //sheet.WorksheetPageSetup.Margins =
                //     new Telerik.Windows.Documents.Spreadsheet.Model.Printing.PageMargins(0
                //     , 20, 0, 20);
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
            int[] _positionWB = GetRowColOfWorbook(viewModel.SettingsPCI.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Fecha")).Celda);
            string fecha = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value.ToString();

            fecha = fecha.Contains("/") ? (DateTime.Now.Date - basedate.Date).TotalDays.ToString() : fecha;
            viewModel.Date = fecha;

            try
            {
                PCITestGeneralDTO pciTestGeneral = new()
                {
                    IdLoad = 0,
                    Date = basedate.AddDays(int.Parse(viewModel.Date)),
                    TypeReport = ReportType.PCI.ToString(),
                    KeyTest = viewModel.ClavePrueba,
                    LanguageKey = viewModel.ClaveIdioma,
                    TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                    Customer = viewModel.SettingsPCI.HeadboardReport.Client,
                    Capacity = viewModel.SettingsPCI.HeadboardReport.Capacity,
                    SerialNumber = viewModel.NoSerie,
                    ReducedCapacityAtLowVoltage = viewModel.CapacidadReducidaBaja != "0",
                    Autotransformer = viewModel.AutoTransformador != "0",
                    Monofasico = viewModel.Monofasico != "0",
                    WindingMaterial = viewModel.MaterialDevanado,
                    Kwcu = viewModel.SettingsPCI.InfotmationArtifact.WarrantiesArtifact.Kwcu ?? 0m,
                    KwcuMva = viewModel.SettingsPCI.InfotmationArtifact.WarrantiesArtifact.KwcuMva ?? 0m,
                    Kwtot100 = viewModel.SettingsPCI.InfotmationArtifact.WarrantiesArtifact.Kwtot100 ?? 0m,
                    Kwtot100M = viewModel.Result.Kwtot100M,
                    Comment = viewModel.Comments,
                    Result = viewModel.IsReportAproved,
                    NameFile = string.Concat("PCI", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                    File = pdfFile,
                    Creadopor = User.Identity.Name,
                    Fechacreacion = DateTime.Now,
                    Modificadopor = string.Empty,
                    Ratings = viewModel.Result?.Results
                };

                ApiResponse<long> result = await _pciClientService.SaveReport(pciTestGeneral);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = pciTestGeneral.NameFile, file = viewModel.Base64PDF };

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

            /// var a = await this._gatewayClientService.GetPositions("G4135-01");

            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> materialProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "Cobre", Descripcion = "Cobre" }, new GeneralPropertiesDTO { Clave = "Aluminio", Descripcion = "Aluminio" } }.AsEnumerable();
            IEnumerable<GeneralPropertiesDTO> capacidadBajaProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "0", Descripcion = "No" }, new GeneralPropertiesDTO { Clave = "1", Descripcion = "Si" } }.AsEnumerable();
            IEnumerable<GeneralPropertiesDTO> autotransformadorProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "0", Descripcion = "No" }, new GeneralPropertiesDTO { Clave = "1", Descripcion = "Si" } }.AsEnumerable();
            IEnumerable<GeneralPropertiesDTO> monofasicoProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "0", Descripcion = "No" }, new GeneralPropertiesDTO { Clave = "1", Descripcion = "Si" } }.AsEnumerable();

            // Tipos de prueba
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.PCI.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");
            ViewBag.MaterialDevanado = new SelectList(materialProperties, "Clave", "Descripcion");
            ViewBag.CapacidadRbaja = new SelectList(capacidadBajaProperties, "Clave", "Descripcion");
            ViewBag.Autotrasformador = new SelectList(autotransformadorProperties, "Clave", "Descripcion");
            ViewBag.Monofasico = new SelectList(monofasicoProperties, "Clave", "Descripcion");

            ViewBag.Select1 = new SelectList(new List<GeneralPropertiesDTO>()
            {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" } ,

            }.AsEnumerable(), "Clave", "Descripcion");
            ViewBag.Select2 = new SelectList(new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" },

            }.AsEnumerable(), "Clave", "Descripcion");
            ViewBag.Select3 = new SelectList(new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Todas" } }.AsEnumerable(), "Clave", "Descripcion");

            ViewBag.CapacidadPrueba = new SelectList(new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccionar..." } }.AsEnumerable(), "Clave", "Descripcion");

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

