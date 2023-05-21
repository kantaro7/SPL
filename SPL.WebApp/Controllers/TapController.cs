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
    public class TapController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly ITapClientService _tapClientService;
        private readonly IFpcClientService _fpcClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly ITapService _tapService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IResistanceTwentyDegreesClientServices _resistanceTwentyDegreesClientServices;
        private readonly IProfileSecurityService _profileClientService;

        private readonly Dictionary<int, string> diccionario = new();
        private readonly Dictionary<int, string> dicDevanados = new();

        public TapController(
            IMasterHttpClientService masterHttpClientService,
            ITapClientService tapClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            ITapService tapService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            ISidcoClientService sidcoClientService,
            IResistanceTwentyDegreesClientServices resistanceTwentyDegreesClientServices,
            IFpcClientService fpcClientService,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _tapClientService = tapClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _tapService = tapService;
            _correctionFactorService = correctionFactorService;
            _hostEnvironment = hostEnvironment;
            _sidcoClientService = sidcoClientService;
            _resistanceTwentyDegreesClientServices = resistanceTwentyDegreesClientServices;
            _fpcClientService = fpcClientService;
            _profileClientService = profileClientService;

            diccionario.Add(1, "H");
            diccionario.Add(2, "HX");
            diccionario.Add(3, "X");
            diccionario.Add(4, "Y");
            diccionario.Add(5, "X+Y");
            diccionario.Add(6, "H+Y");
            diccionario.Add(7, "H+X");

            diccionario.Add(8, "CH");
            diccionario.Add(9, "CH+CHX");
            diccionario.Add(10, "CX+CHX");
            diccionario.Add(11, "CHX+CHXY");
            diccionario.Add(12, "CY+CHXY");
            diccionario.Add(13, "CY+CHY");
            diccionario.Add(14, "CHX");

            dicDevanados.Add(2, "2DE");
            dicDevanados.Add(4, "3DE");
            dicDevanados.Add(3, "ACT");
            dicDevanados.Add(5, "AST");
            dicDevanados.Add(1, "REA");

        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.TensiónAplicada)))
                {

                    await PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return View(new TapViewModel { NoSerie = noSerie });
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
            TapViewModel tapViewModel = new();
            noSerie = noSerie.ToUpper().Trim();
            string noSerieSimple = string.Empty;

            EstructuraGeneral estructuraG = new();
            List<EstructuraReporte> reporte = new()
            {
                new EstructuraReporte
                {
                    PrimeraColumna = "HV",
                    Columna = 1
                }
            };

            int[,] arratDevanadoEnergizado = new int[5, 3]
            {
                {1, 0, 0 },
                {1, 3, 0 },
                {2, 4, 0 },
                {1, 3, 4 },
                { 2, 0, 0 }
            };

            int[,] arratDevanadoAterrizado = new int[5, 3]
            {
                {0, 0, 0 },
                {3, 1, 0 },
                {4, 2, 0 },
                {5, 6, 7 },
                { 0, 0, 0 }
            };

            int[,] idCapacitancia = new int[5, 3]
            {
                {8, 0, 0 },
                {9, 10, 0 },
                {11, 12, 0 },
                {9, 10, 13 },
                { 14, 0, 0 }
            };

            if (!string.IsNullOrEmpty(noSerie))
            {
                noSerieSimple = noSerie.Split("-")[0];
            }
            else
            {
                return Json(new
                {
                    response = new ApiResponse<TapViewModel>
                    {
                        Code = -1,
                        Description = "No. Serie inválido.",
                        Structure = null
                    }
                });
            }

            bool isFromSPL = await _artifactClientService.CheckNumberOrder(noSerieSimple);
            int fila = 0;
            if (!isFromSPL)
            {
                return Json(new
                {
                    response = new ApiResponse<TapViewModel>
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
                        response = new ApiResponse<TapViewModel>
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
                        response = new ApiResponse<TapViewModel>
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
                        response = new ApiResponse<PceViewModel>
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
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee voltages",
                            Structure = null
                        }
                    });
                }

                tapViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;

                tapViewModel.ATConnectionsAmount = artifactDesing.VoltageKV.TensionKvAltaTension1 is not null and not 0
                    ? artifactDesing.VoltageKV.TensionKvAltaTension3 is not null and not 0 ? 2 : 1
                    : 0;
                tapViewModel.BTConnectionsAmount = artifactDesing.VoltageKV.TensionKvBajaTension1 is not null and not 0
                    ? artifactDesing.VoltageKV.TensionKvBajaTension3 is not null and not 0 ? 2 : 1
                    : 0;
                tapViewModel.TerConnectionsAmount = artifactDesing.VoltageKV.TensionKvTerciario1 is not null and not 0
                    ? artifactDesing.VoltageKV.TensionKvTerciario3 is not null and not 0 ? 2 : 1
                    : 0;

                tapViewModel.UnitType = artifactDesing.GeneralArtifact.TipoUnidad;

                if (tapViewModel.UnitType == "2DE" || tapViewModel.UnitType == "AST")
                {
                    if (artifactDesing.VoltageKV.TensionKvTerciario1 is not null and not 0 || artifactDesing.VoltageKV.TensionKvTerciario3 is not null and not 0)
                    {
                        //verificar que no cuente con boq en terciario
                        var boqTer = await this._artifactClientService.CheckBoqTerciary(noSerieSimple);

                        if(boqTer == -1)
                        {
                            return Json(new
                            {
                                response = new ApiResponse<TapViewModel>
                                {
                                    Code = -1,
                                    Description = "Error al consultar boquilla para terciario",
                                    Structure = tapViewModel
                                }
                            });
                        }
                        else
                        {
                            if(boqTer == 0)
                            {
                                tapViewModel.EnableCapTer = true;
                                fila = tapViewModel.UnitType == "2DE" ? 4 : 5;
                                tapViewModel.TipoDevanadoNuevo = tapViewModel.UnitType == "2DE" ? "3DE" : "ACT";

                                tapViewModel.ViejaFila = dicDevanados.FirstOrDefault(x => x.Value == tapViewModel.UnitType).Key;
                                tapViewModel.ViejoDevanado = tapViewModel.UnitType;
                                tapViewModel.NuevaFila = fila;
                                tapViewModel.NuevoDevanado = tapViewModel.TipoDevanadoNuevo;
                            }
                            else
                            {
                                fila = dicDevanados.FirstOrDefault(x => x.Value == tapViewModel.UnitType).Key;
                                tapViewModel.TipoDevanadoNuevo = tapViewModel.UnitType;
                                tapViewModel.EnableCapTer = false;
                                tapViewModel.ViejaFila = fila;
                                tapViewModel.ViejoDevanado = tapViewModel.UnitType;
                            }
                        }
                    }
                    else
                    {
                        fila = dicDevanados.FirstOrDefault(x => x.Value == tapViewModel.UnitType).Key;
                        tapViewModel.TipoDevanadoNuevo = tapViewModel.UnitType;
                        tapViewModel.EnableCapTer = false;
                        tapViewModel.ViejaFila = fila; 
                        tapViewModel.ViejoDevanado = tapViewModel.UnitType;
                    }
                }
                else
                {
                    fila = dicDevanados.FirstOrDefault(x => x.Value == tapViewModel.UnitType).Key;
                    tapViewModel.TipoDevanadoNuevo = tapViewModel.UnitType;
                    tapViewModel.EnableCapTer = false;
                    tapViewModel.ViejaFila = fila;
                    tapViewModel.ViejoDevanado = tapViewModel.UnitType;
                }


            }

            if (fila == 0)
            {
                return Json(new
                {
                    response = new ApiResponse<PceViewModel>
                    {
                        Code = -1,
                        Description = "No ha sido encontrado una coincidencia en la tabla de devanados para " + tapViewModel.UnitType,
                        Structure = null
                    }
                });
            }



            estructuraG.Reporte = LlenarFilasReporte(fila,idCapacitancia,arratDevanadoEnergizado,arratDevanadoAterrizado,tapViewModel.ATConnectionsAmount, tapViewModel.BTConnectionsAmount,
                tapViewModel.TerConnectionsAmount, estructuraG,reporte);

            tapViewModel.ATCapacitanceId = estructuraG.IdCampacitanciaAt;
            tapViewModel.BTCapacitanceId = estructuraG.IdCampacitanciaBt;
            tapViewModel.TerCapacitanceId = estructuraG.IdCampacitanciaTer;

            //ApiResponse<FPCTestsGeneralDTO> fpcReport = await this._fpcClientService.GetInfoReportFPC(noSerie, TestType.SAN.ToString(), tapViewModel.ClaveIdioma, tapViewModel.UnitType, (decimal)0, true);
            //if (fpcReport.Code == -1)
            //{
            //    return this.Json(new
            //    {
            //        response = new { Code = -1, Description = fpcReport.Description, Structure = tapViewModel }
            //    });
            //}
            //else if(fpcReport.Structure is null)
            //{
            //    return this.Json(new
            //    {
            //        response = new { Code = -2, Description = fpcReport.Description , Structure  = tapViewModel}
            //    });
            //}

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await _tapClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.TAP.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.TAP.ToString() };
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

                tapViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Tensión Aplicada",
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
                response = new ApiResponse<TapViewModel>
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = tapViewModel
                }
            });
        }

      

        [HttpGet]
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, int aTConnectionsAmount, int bTConnectionsAmount, int terConnectionsAmount, string aTCapacitanceId, string bTCapacitanceId, string terCapacitanceId, string unitType, string comment,
            string capacidadTer, string nuevoDevanado , string viejoDevanado, int nuevaFila , int viejaFila)
        {

            EstructuraGeneral estructuraG = new();
            List<EstructuraReporte> reporte = new()
            {
                new EstructuraReporte
                {
                    PrimeraColumna = "HV",
                    Columna = 1
                }
            };

            int[,] arratDevanadoEnergizado = new int[5, 3]
            {
                {1, 0, 0 },
                {1, 3, 0 },
                {2, 4, 0 },
                {1, 3, 4 },
                { 2, 0, 0 }
            };

            int[,] arratDevanadoAterrizado = new int[5, 3]
            {
                {0, 0, 0 },
                {3, 1, 0 },
                {4, 2, 0 },
                {5, 6, 7 },
                { 0, 0, 0 }
            };

            int[,] idCapacitancia = new int[5, 3]
            {
                {8, 0, 0 },
                {9, 10, 0 },
                {11, 12, 0 },
                {9, 10, 13 },
                { 14, 0, 0 }
            };

            noSerie = noSerie.ToUpper().Trim();

            ApiResponse<SettingsToDisplayTAPReportsDTO> result = await _gatewayClientService.GetTemplateTAP(noSerie, clavePrueba, claveIdioma, unitType, aTConnectionsAmount, bTConnectionsAmount, terConnectionsAmount, aTCapacitanceId, bTCapacitanceId, terCapacitanceId);

            if (result.Code.Equals(-1))
            {
                return View("Excel", new TapViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayTAPReportsDTO reportInfo = result.Structure;

            #region Decode Template
            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return View("Excel", new TapViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }
            ApiResponse<PositionsDTO> resultPositions = await _gatewayClientService.GetPositions(noSerie);

            if (resultPositions.Code.Equals(-1))
            {
                return View("Excel", new TapViewModel
                {
                    Error = resultPositions.Description
                });
            }

            long NoPrueba = reportInfo.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Workbook workbook = Workbook.Load(stream, ".xlsx");

            try
            {
                var nuevoReporte = LlenarFilasReporte(!string.IsNullOrEmpty(capacidadTer) ? nuevaFila : viejaFila, idCapacitancia, arratDevanadoEnergizado, arratDevanadoAterrizado, aTConnectionsAmount, bTConnectionsAmount, terConnectionsAmount, estructuraG, reporte);
                reportInfo.EstructuraReportes = nuevoReporte;
                _tapService.PrepareTemplate_TAP(reportInfo, nuevoReporte, ref workbook);
            }
            catch (Exception)
            {
                return View("Excel", new TapViewModel
                {
                    Error = "La plantilla esta mal configurada por favor verifiquela de inmediato"
                });
            }

            int acceptanceValue = Convert.ToInt32(reportInfo.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Valor_Acep"))?.Formato);

            #endregion

            return View("Excel", new TapViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = clavePrueba,
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                TAPReportsDTO = result.Structure,
                Workbook = workbook,
                ATConnectionsAmount = aTConnectionsAmount,
                ATCapacitanceId = aTCapacitanceId,
                UnitType = unitType,
                BTConnectionsAmount = bTConnectionsAmount,
                BTCapacitanceId = bTCapacitanceId,
                TerConnectionsAmount = terConnectionsAmount,
                TerCapacitanceId = terCapacitanceId,
                AcceptanceValue = acceptanceValue,
                Comments = comment,
                Error = string.Empty,
                NuevoDevanado = nuevoDevanado,
                NuevaFila = nuevaFila,
                ViejoDevanado = viejoDevanado,
                ViejaFila = viejaFila,
                CapacitanciaTer = capacidadTer
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] TapViewModel viewModel)
        {

            TAPTestsDTO tapTestDTO = new()
            {
                TAPTestsDetails = new List<TAPTestsDetailsDTO>()
            };

            if (!_tapService.Verify_TAP_ColumnsToCalculate(viewModel.TAPReportsDTO, viewModel.Workbook))
            {
                return Json(new
                {
                    response = new { Code = -2, Description = "Faltan datos por ingresar en la tabla" }
                });
            }

            _tapService.Prepare_TAP_Test(viewModel.TAPReportsDTO, viewModel.Workbook, ref tapTestDTO);
            tapTestDTO.Freacuency = viewModel.TAPReportsDTO.FrequencyTest ?? 0;
            List<string> errores = new();


            ApiResponse<FPCTestsGeneralDTO> fpcReport = await _fpcClientService.GetInfoReportFPC(viewModel.NoSerie, TestType.SAN.ToString(), viewModel.ClaveIdioma, viewModel.ViejoDevanado,  tapTestDTO.Freacuency, true);
            if (fpcReport.Structure is null)
            {
                fpcReport = await _fpcClientService.GetInfoReportFPC(viewModel.NoSerie, TestType.AYD.ToString(), viewModel.ClaveIdioma, viewModel.ViejoDevanado, tapTestDTO.Freacuency, true);
                if (fpcReport.Structure is null)
                {
                    return Json(new
                    {
                        response = new { Code = -2, Description = "La capacitancia para el aparato no fue encontrada, no se puede efectuar una validación, no se encuentra registrado un reporte FPC con las siguientes coincidencias: " + viewModel.NoSerie + "  " + TestType.SAN.ToString() + "/" + TestType.AYD.ToString() + "   " + viewModel.ClaveIdioma + "  " + viewModel.UnitType + "  " + tapTestDTO.Freacuency + "  " + "Resultado=Aceptado" }
                    });
                }
            }
            foreach(TAPTestsDetailsDTO detailsDTO in tapTestDTO.TAPTestsDetails)
            {
                if(detailsDTO.WindingEnergized.Length > 10)
                {
                    return Json(new
                    {
                        response = new { Code = -2, Description = "Devanado evergizado no puede exceder 10 caracteres" }
                    });
                }
                else if (detailsDTO.WindingGrounded.Length > 10)
                {
                    return Json(new
                    {
                        response = new { Code = -2, Description = "Devanado aterrizado no puede exceder 10 caracteres" }
                    });
                }
            }

            viewModel.FPCId = fpcReport.Structure.IdLoad;

            int index = 0;
            for (int i = 0; i < viewModel.ATConnectionsAmount; i++)
            {
                FPCTestsDetailsDTO capsAT = fpcReport.Structure.Data.FirstOrDefault().FPCTestsDetails.FirstOrDefault(x => x.Id.Equals(viewModel.ATCapacitanceId));
                if (capsAT != null)
                {
                    tapTestDTO.TAPTestsDetails[index].Capacitance = capsAT.Capacitance;
                    index++;
                }
            }

            for (int i = 0; i < viewModel.BTConnectionsAmount; i++)
            {
                FPCTestsDetailsDTO capsBT = fpcReport.Structure.Data.FirstOrDefault().FPCTestsDetails.FirstOrDefault(x => x.Id.Equals(viewModel.BTCapacitanceId));
                if (capsBT != null)
                {
                    tapTestDTO.TAPTestsDetails[index].Capacitance = capsBT.Capacitance;
                    index++;
                }
            }

            if (string.IsNullOrEmpty(viewModel.CapacitanciaTer))
            {
                for (int i = 0; i < viewModel.TerConnectionsAmount; i++)
                {
                    FPCTestsDetailsDTO capsTer = fpcReport.Structure.Data.FirstOrDefault().FPCTestsDetails.FirstOrDefault(x => x.Id.Equals(viewModel.TerCapacitanceId));
                    if (capsTer != null)
                    {
                        tapTestDTO.TAPTestsDetails[index].Capacitance = capsTer.Capacitance;
                        index++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < viewModel.TerConnectionsAmount; i++)
                {
                  
                        tapTestDTO.TAPTestsDetails[index].Capacitance = decimal.Parse(viewModel.CapacitanciaTer);
                        index++;
                    
                }
            }

            tapTestDTO.ValueAcep = viewModel.AcceptanceValue;

            ApiResponse<ResultTAPTestsDTO> resultTestTAP_Response = await _tapClientService.CalculateTestTAP(tapTestDTO);


            if (resultTestTAP_Response.Code.Equals(-1))
            {
                errores.Add(resultTestTAP_Response.Description);

                return this.Json(new
                {
                    response = new ApiResponse<TapViewModel>
                    {
                        Code = -1,
                        Description = resultTestTAP_Response.Description,
                        Structure = viewModel
                    }
                });
            }
            else if (resultTestTAP_Response.Code.Equals(5))
            {
                return this.Json(new
                {
                    response = new ApiResponse<TapViewModel>
                    {
                        Code = 5,
                        Description = resultTestTAP_Response.Description,
                        Structure = viewModel
                    }
                });
            }

            if(resultTestTAP_Response.Structure != null)
            {
                resultTestTAP_Response.Structure.Results.ForEach(x => errores.Add(x.Message));
            }
            Workbook workbook = viewModel.Workbook;

            _tapService.PrepareIndexOfTAP(resultTestTAP_Response.Structure, viewModel.TAPReportsDTO, ref workbook, viewModel.ClaveIdioma);

            #region fillColumns
            tapTestDTO = resultTestTAP_Response.Structure.TAPTests;
            #endregion

            viewModel.TapTestDTO = tapTestDTO;
            viewModel.Workbook = workbook;

            string errors = string.Empty;
            List<string> errorMessages = errores.Select(k => k).ToList();
            string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
            return Json(new
            {
                response = new ApiResponse<TapViewModel>
                {
                    Code = 1,
                    Description = allError,
                    Structure = viewModel
                }
            });
        }

        [HttpPost]

        public async Task<IActionResult> SavePDF([FromBody] TapViewModel viewModel)
        {
            try
            {
                #region Export Excel
                int[] filas = new int[2];
                int[] _positionWB;
                string reportResult = string.Empty;


                _positionWB = this.GetRowColOfWorbook(viewModel.TAPReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
                reportResult = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();


                if(reportResult == "Accepted" || reportResult == "Aceptado")
                {
                    viewModel.IsReportAproved = true;
                }
                else
                {
                    viewModel.IsReportAproved = false;
                }


                if (!_tapService.Verify_TAP_Columns(viewModel.TAPReportsDTO, viewModel.Workbook))
                {
                    return Json(new
                    {
                        response = new { status = -2, description = "Faltan datos por ingresar en la tabla" }
                    });
                }

                DateTime date = _tapService.GetDate(viewModel.Workbook, viewModel.TAPReportsDTO);

                Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

                PdfFormatProvider provider = new()
                {
                    ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
                };
                document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.Workbook.ActiveSheet);
                FloatingImage image = null;
                string val;
                int rowCount = 0; foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
                {
                    #region FormatPDF

                    // Frecuencia
                    _positionWB = GetRowColOfWorbook("D10");
                    val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                    sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(FormatStringDecimal(val, 0));

                    // PorcReg
                    _positionWB = GetRowColOfWorbook(viewModel.TAPReportsDTO.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelTension")).Celda);
                    int row = 0;
                    foreach (TAPTestsDetailsDTO item in viewModel.TapTestDTO.TAPTestsDetails)
                    {
                        val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1])].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1])].SetValueAsText(FormatStringDecimal(val, 3));

                        val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + 1)].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + 1)].SetValueAsText(FormatStringDecimal(val, 3));

                        val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + 2)].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + 2)].SetValueAsText(FormatStringDecimal(val, 3));

                        val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + 3)].GetValue().Value.RawValue;
                        sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0] + row, _positionWB[1] + 3)].SetValueAsText(FormatStringDecimal(val, 0));
                        row++;
                    }
                    #endregion
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
                    sheet.WorksheetPageSetup.CenterHorizontally = true;
                    sheet.WorksheetPageSetup.PrintOptions.PrintGridlines = false;
                    sheet.WorksheetPageSetup.ScaleFactor = new Telerik.Documents.Primitives.Size(0.9, 0.9);
                    sheet.WorksheetPageSetup.Margins = new Telerik.Windows.Documents.Spreadsheet.Model.Printing.PageMargins(0, 20, 0, 20);
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
                #endregion

                #region Save Report
                TAPTestsGeneralDTO tapTestsGeneralDTO = new()
                {
                    Capacity = viewModel.TAPReportsDTO.HeadboardReport.Capacity,
                    Comment = viewModel.Comments,
                    Creadopor = User.Identity.Name,
                    Customer = viewModel.TAPReportsDTO.HeadboardReport.Client,
                    TAPTests = viewModel.TapTestDTO,
                    Date = date,
                    Fechacreacion = DateTime.Now,
                    File = pdfFile,
                    IdLoad = 0,
                    KeyTest = viewModel.ClavePrueba,
                    LanguageKey = viewModel.ClaveIdioma,
                    LoadDate = DateTime.Now,
                    Modificadopor = null,
                    Fechamodificacion = null,
                    NameFile = string.Concat("TAP", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                    Result = viewModel.IsReportAproved,
                    SerialNumber = viewModel.NoSerie,
                    TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                    TypeReport = ReportType.TAP.ToString(),
                    UnitType = viewModel.UnitType,
                    NoConnectionsAT = viewModel.ATConnectionsAmount,
                    NoConnectionsBT = viewModel.BTConnectionsAmount,
                    NoConnectionsTER = viewModel.TerConnectionsAmount,
                    IdCapAT = viewModel.ATCapacitanceId,
                    IdCapBT = viewModel.BTCapacitanceId,
                    IdCapTER = viewModel.TerCapacitanceId,
                    IdRepFPC = viewModel.FPCId
                };
                #endregion

                try
                {
                    string a = JsonConvert.SerializeObject(tapTestsGeneralDTO);
                    ApiResponse<long> result = await _tapClientService.SaveReport(tapTestsGeneralDTO);

                    var resultResponse = new { status = result.Code, description = result.Description, nameFile = tapTestsGeneralDTO.NameFile, file = tapTestsGeneralDTO.File };

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
            catch(Exception ex)
            {
                return Json(new
                {
                    response = new ApiResponse<TapViewModel>
                    {
                        Code = -1,
                        Description = ex.Message,
                        Structure = null
                    }
                });
            }
        }
        public IActionResult Error() => View();


        private List<EstructuraReporte> LlenarFilasReporte(int fila, int[,] idCapacitancia, int[,] arratDevanadoEnergizado, int[,] arratDevanadoAterrizado, 
            int ATConnectionsAmount , int BTConnectionsAmount,int TerConnectionsAmount , EstructuraGeneral estructuraG, List<EstructuraReporte> reporte)
        {
            #region llenado de filas 
            /**************PRIMERA FILA DEL REPORTE************************/


            int idEncontrado = idCapacitancia[fila - 1, 0];
            estructuraG.IdCampacitanciaAt = diccionario.FirstOrDefault(x => x.Key == idEncontrado).Value;

            int idDevanadoEnergizado = arratDevanadoEnergizado[fila - 1, 0];
            reporte[0].DevanadoEnergizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoEnergizado).Value;

            int idDevanadoAterrizado = arratDevanadoAterrizado[fila - 1, 0];
            reporte[0].DevanadoAterrizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoAterrizado).Value;

            /****************************************************************************/

            /********************SEGUNDA FILA ********************************/

            if (ATConnectionsAmount > 1) // es HV
            {
                reporte.Add(
                new EstructuraReporte
                {
                    PrimeraColumna = "HV",
                    Columna = 1
                });

                idEncontrado = idCapacitancia[fila - 1, reporte.LastOrDefault().Columna - 1];
                estructuraG.IdCampacitanciaAt = diccionario.FirstOrDefault(x => x.Key == idEncontrado).Value;

                idDevanadoEnergizado = arratDevanadoEnergizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                reporte.LastOrDefault().DevanadoEnergizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoEnergizado).Value;

                idDevanadoAterrizado = arratDevanadoAterrizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                reporte.LastOrDefault().DevanadoAterrizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoAterrizado).Value;

            }
            else// ES LV
            {
                reporte.Add(
                new EstructuraReporte
                {
                    PrimeraColumna = "LV",
                    Columna = 2
                });

                idEncontrado = idCapacitancia[fila - 1, reporte.LastOrDefault().Columna - 1];
                estructuraG.IdCampacitanciaBt = diccionario.FirstOrDefault(x => x.Key == idEncontrado).Value;

                idDevanadoEnergizado = arratDevanadoEnergizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                reporte.LastOrDefault().DevanadoEnergizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoEnergizado).Value;

                idDevanadoAterrizado = arratDevanadoAterrizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                reporte.LastOrDefault().DevanadoAterrizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoAterrizado).Value;
            }
            /**********************************************************************/

            /********************Tercera  FILA ********************************/
            if ((ATConnectionsAmount > 1 && BTConnectionsAmount == 1) || BTConnectionsAmount == 2) // es LV
            {
                reporte.Add(
                new EstructuraReporte
                {
                    PrimeraColumna = "LV",
                    Columna = 2
                });

                idEncontrado = idCapacitancia[fila - 1, reporte.LastOrDefault().Columna - 1];
                estructuraG.IdCampacitanciaBt = diccionario.FirstOrDefault(x => x.Key == idEncontrado).Value;

                idDevanadoEnergizado = arratDevanadoEnergizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                reporte.LastOrDefault().DevanadoEnergizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoEnergizado).Value;

                idDevanadoAterrizado = arratDevanadoAterrizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                reporte.LastOrDefault().DevanadoAterrizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoAterrizado).Value;

            }
            else
            {
                if (TerConnectionsAmount == 0)// no pongo nada 
                {

                }
                else // pongo TV
                {
                    reporte.Add(
                       new EstructuraReporte
                       {
                           PrimeraColumna = "TV",
                           Columna = 3
                       });

                    idEncontrado = idCapacitancia[fila - 1, reporte.LastOrDefault().Columna - 1];
                    estructuraG.IdCampacitanciaTer = diccionario.FirstOrDefault(x => x.Key == idEncontrado).Value;

                    idDevanadoEnergizado = arratDevanadoEnergizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                    reporte.LastOrDefault().DevanadoEnergizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoEnergizado).Value;

                    idDevanadoAterrizado = arratDevanadoAterrizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                    reporte.LastOrDefault().DevanadoAterrizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoAterrizado).Value;
                }
            }
            /******************************************************************/

            /********************Cuarta  FILA ********************************/
            if (ATConnectionsAmount > 1 && BTConnectionsAmount > 1)//es LV
            {
                reporte.Add(
               new EstructuraReporte
               {
                   PrimeraColumna = "LV",
                   Columna = 2
               });

                idEncontrado = idCapacitancia[fila - 1, reporte.LastOrDefault().Columna - 1];
                estructuraG.IdCampacitanciaBt = diccionario.FirstOrDefault(x => x.Key == idEncontrado).Value;

                idDevanadoEnergizado = arratDevanadoEnergizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                reporte.LastOrDefault().DevanadoEnergizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoEnergizado).Value;

                idDevanadoAterrizado = arratDevanadoAterrizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                reporte.LastOrDefault().DevanadoAterrizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoAterrizado).Value;
            }
            else
            {
                if (TerConnectionsAmount == 0)// no hago nada
                {

                }
                else
                {
                    if (reporte.LastOrDefault().PrimeraColumna == "TV" && TerConnectionsAmount == 1)// no hago nada
                    {

                    }
                    else
                    {
                        reporte.Add(
                        new EstructuraReporte
                        {
                            PrimeraColumna = "TV",
                            Columna = 3
                        });

                        idEncontrado = idCapacitancia[fila - 1, reporte.LastOrDefault().Columna - 1];
                        estructuraG.IdCampacitanciaTer = diccionario.FirstOrDefault(x => x.Key == idEncontrado).Value;

                        idDevanadoEnergizado = arratDevanadoEnergizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                        reporte.LastOrDefault().DevanadoEnergizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoEnergizado).Value;

                        idDevanadoAterrizado = arratDevanadoAterrizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                        reporte.LastOrDefault().DevanadoAterrizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoAterrizado).Value;
                    }
                }
            }
            /******************************************************************/

            /********************Quinta  FILA ********************************/
            if (ATConnectionsAmount > 1 && BTConnectionsAmount > 1 && TerConnectionsAmount >= 1)
            {
                reporte.Add(
                       new EstructuraReporte
                       {
                           PrimeraColumna = "TV",
                           Columna = 3
                       });

                idEncontrado = idCapacitancia[fila - 1, reporte.LastOrDefault().Columna - 1];
                estructuraG.IdCampacitanciaTer = diccionario.FirstOrDefault(x => x.Key == idEncontrado).Value;

                idDevanadoEnergizado = arratDevanadoEnergizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                reporte.LastOrDefault().DevanadoEnergizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoEnergizado).Value;

                idDevanadoAterrizado = arratDevanadoAterrizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                reporte.LastOrDefault().DevanadoAterrizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoAterrizado).Value;
            }
            else
            {
                if (reporte.LastOrDefault().PrimeraColumna == "TV" && TerConnectionsAmount == 2)
                {
                    reporte.Add(
                      new EstructuraReporte
                      {
                          PrimeraColumna = "TV",
                          Columna = 3
                      });

                    idEncontrado = idCapacitancia[fila - 1, reporte.LastOrDefault().Columna - 1];
                    estructuraG.IdCampacitanciaTer = diccionario.FirstOrDefault(x => x.Key == idEncontrado).Value;

                    idDevanadoEnergizado = arratDevanadoEnergizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                    reporte.LastOrDefault().DevanadoEnergizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoEnergizado).Value;

                    idDevanadoAterrizado = arratDevanadoAterrizado[fila - 1, reporte.LastOrDefault().Columna - 1];
                    reporte.LastOrDefault().DevanadoAterrizado = diccionario.FirstOrDefault(x => x.Key == idDevanadoAterrizado).Value;
                }
                else // no hago nada
                {

                }
            }
            /******************************************************************/

            #endregion 
            return reporte;
        }


        [HttpGet]
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


        #region PrivateMethods
        public async Task PrepareForm()
        {
            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            // Tipos de prueba
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await _testClientService.GetTest(ReportType.TAP.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            ViewBag.UnitTypeItems = new SelectList(origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetUnitTypes, (int)MicroservicesEnum.splmasters)), "Clave", "Descripcion");

            ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");

            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await _masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");
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
                    num = decimals is 0 ? $"{entero}" : $"{entero}.{decima}";
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
        #endregion
    }



    public class EstructuraGeneral
    {
        public string IdCampacitanciaAt { get; set; }
        public string IdCampacitanciaBt { get; set; }
        public string IdCampacitanciaTer { get; set; }

        public List<EstructuraReporte> Reporte { get; set; }
    }
}