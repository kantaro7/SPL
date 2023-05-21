namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Data;
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.ViewModels;
    using Telerik.Documents;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf;
    using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export;
    using Telerik.Web.Spreadsheet;
    using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
    using Microsoft.AspNetCore.Hosting;
    using Newtonsoft.Json;
    using Telerik.Windows.Documents.Spreadsheet.Model;
    using Telerik.Windows.Documents.Spreadsheet;
using SPL.WebApp.Helpers;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using Microsoft.Identity.Web;

    /// <summary>
    /// Relación de Transformación
    /// </summary>
    public class PceController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IPceClientService _pceClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IPceService _pceService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IResistanceTwentyDegreesClientServices _resistanceTwentyDegreesClientServices;
        private readonly IProfileSecurityService _profileClientService;
        public PceController(
            IMasterHttpClientService masterHttpClientService,
            IPceClientService pceClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IPceService pceService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            ISidcoClientService sidcoClientService,
            IResistanceTwentyDegreesClientServices resistanceTwentyDegreesClientServices,
            IProfileSecurityService profileClientService
            )
        {
            this._masterHttpClientService = masterHttpClientService;
            this._pceClientService = pceClientService;
            this._reportClientService = reportClientService;
            this._artifactClientService = artifactClientService;
            this._testClientService = testClientService;
            this._gatewayClientService = gatewayClientService;
            this._pceService = pceService;
            this._correctionFactorService = correctionFactorService;
            this._hostEnvironment = hostEnvironment;
            this._sidcoClientService = sidcoClientService;
            this._resistanceTwentyDegreesClientServices = resistanceTwentyDegreesClientServices;
            this._profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(this.User.Identity.Name).Result;

                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.PérdidasenVacíoyCorrientedeExcitación)))
                {

                    await this.PrepareForm();
                    string noSerie = ParserHelper.GetNoSerieFromUrlQuery(this.Request.Query);
                    return this.View(new PceViewModel { NoSerie = noSerie });
                }
                else
                {
                    return this.View("~/Views/PageConstruction/PermissionDenied.cshtml");

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
            PceViewModel pceViewModel = new();
            noSerie = noSerie.ToUpper().Trim();
            string noSerieSimple = string.Empty;

            if (!string.IsNullOrEmpty(noSerie))
            {
                noSerieSimple = noSerie.Split("-")[0];
            }
            else
            {
                return this.Json(new
                {
                    response = new ApiResponse<PceViewModel>
                    {
                        Code = -1,
                        Description = "No. Serie inválido.",
                        Structure = null
                    }
                });
            }

            bool isFromSPL = await this._artifactClientService.CheckNumberOrder(noSerieSimple);

            if (!isFromSPL)
            {
                return this.Json(new
                {
                    response = new ApiResponse<PceViewModel>
                    {
                        Code = -1,
                        Description = "Artefacto no presente en SPL",
                        Structure = null
                    }
                });
            }
            else
            {
                InformationArtifactDTO artifactDesing = await this._artifactClientService.GetArtifact(noSerieSimple);
                if (artifactDesing.GeneralArtifact.OrderCode == null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no encontrado",
                            Structure = null
                        }
                    });
                }

                if (artifactDesing.GeneralArtifact is null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
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
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee características",
                            Structure = null
                        }
                    });
                }

                // TAPS
                if (artifactDesing.TapBaan is null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee información de cambiadores",
                            Structure = null
                        }
                    });
                }

                // CAR
                if (artifactDesing.VoltageKV is null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee voltages",
                            Structure = null
                        }
                    });
                }

                // GAR
                if (artifactDesing.WarrantiesArtifact is null)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee garantias",
                            Structure = null
                        }
                    });
                }

                List<PlateTensionDTO> tensions = (await this._artifactClientService.GetPlateTension(noSerie, "-1")).Structure;

                if (tensions.Count is 0)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee tensiones de placa",
                            Structure = null
                        }
                    });
                }
                ApiResponse<PositionsDTO> resultP = await this._gatewayClientService.GetPositions(noSerie);

                if (resultP.Code == -1)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = resultP.Description,
                            Structure = null
                        }
                    });
                }

                PositionsDTO positions = (await this._gatewayClientService.GetPositions(noSerie)).Structure;

                pceViewModel.AT = positions.AltaTension.Any();
                pceViewModel.BT = positions.BajaTension.Any();
                pceViewModel.Ter = positions.Terciario.Any();

                IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

                pceViewModel.TestPositionsAT = new SelectList(origingeneralProperties.Concat(positions.AltaTension.Select(x => new GeneralPropertiesDTO()
                {
                    Clave = x,
                    Descripcion = x
                })), "Clave", "Descripcion");

                pceViewModel.TestPositionsBT = new SelectList(origingeneralProperties.Concat(positions.BajaTension.Select(x => new GeneralPropertiesDTO()
                {
                    Clave = x,
                    Descripcion = x
                })), "Clave", "Descripcion");

                pceViewModel.TestPositionsTer = new SelectList(origingeneralProperties.Concat(positions.Terciario.Select(x => new GeneralPropertiesDTO()
                {
                    Clave = x,
                    Descripcion = x
                })), "Clave", "Descripcion");

                List<(string, decimal)> tensiones = new();

                if ((artifactDesing.VoltageKV.TensionKvAltaTension1 is null or 0) && (artifactDesing.VoltageKV.TensionKvBajaTension1 is null or 0) && (artifactDesing.VoltageKV.TensionKvTerciario1 is null or 0))
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "Artefacto no posee valores de tensiones KV",
                            Structure = null
                        }
                    });
                }

                if (artifactDesing.VoltageKV.TensionKvAltaTension1 is not null and > 0)
                {
                    tensiones.Add(("AT", artifactDesing.VoltageKV.TensionKvAltaTension1 ?? 0));
                }

                if (artifactDesing.VoltageKV.TensionKvBajaTension1 is not null and > 0)
                {
                    tensiones.Add(("BT", artifactDesing.VoltageKV.TensionKvBajaTension1 ?? 0));
                }

                if (artifactDesing.VoltageKV.TensionKvTerciario1 is not null and > 0)
                {
                    tensiones.Add(("Ter", artifactDesing.VoltageKV.TensionKvTerciario1 ?? 0));
                }

                pceViewModel.LessWindingEnergized = tensiones.OrderBy(x => x.Item2).FirstOrDefault().Item1;

                pceViewModel.WindingsEnergized = new SelectList(tensiones.Select(x => new GeneralPropertiesDTO()
                {
                    Clave = x.Item1,
                    Descripcion = x.Item1
                }), "Clave", "Descripcion");

                pceViewModel.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;
            }

            ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await this._pceClientService.GetFilter(noSerie);

            if (resultFilter.Code.Equals(1))
            {
                InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.PCE.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.PCE.ToString() };
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

                pceViewModel.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Pérdidas en Vacío y Corriente de Excitación",
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
           /* List<Coord>coordenadas = new List<Coord>()
            {

                new Coord
                {
                    x = 0,
                    y = 86.0145M
                },
                new Coord
                {
                    x = 3,
                   y = 85.7145M
                },
                new Coord
                {
                    x = 6,
                    y = 85.4145M
                },
                new Coord
                {
                    x = 9,
                    y = 85.2145M
                },
                new Coord
                {
                    x = 12,
                    y = 85.0145M
                },
                new Coord
                {
                    x = 15,
                    y = 84.9457M
                },
                new Coord
                {
                    x = 18,
                    y = 84.79457M
                },
                new Coord
                {
                    x = 19,
                    y = 84.59789M
                },
                new Coord
                {
                    x = 21,
                    y = 84.44787M
                },
                new Coord
                {
                    x = 22,
                    y = 84.3997M
                },
                new Coord
                {
                    x = 24,
                    y = 84.3479M
                },
                new Coord
                {
                    x = 25,
                    y = 84.31797M
                },
                new Coord
                {
                    x = 27,
                    y = 84.26797M
                },
                new Coord
                {
                    x = 28,
                    y =84.227979M
                },
                new Coord
                {
                    x = 30,
                    y = 84.18799M
                },
                new Coord
                {
                    x = 31,
                    y = 84.15797M
                },
                new Coord
                {
                    x =33,
                    y = 84.1199M
                },
                new Coord
                {
                    x = 34,
                    y = 84.0777M
                },
                new Coord
                {
                    x = 35,
                    y = 84.0479M
                },
                new Coord
                {
                    x = 36,
                    y = 84.01M
                },
                new Coord
                {
                    x = 37,
                    y = 83.98998M
                },
                new Coord
                {
                    x = 39,
                    y =  83.947979M
                },
                new Coord
                {
                    x = 40,
                    y = 83.92898M
                },
                  new Coord
                {
                    x = 42,
                    y =  83.898989M
                },
                  new Coord
                {
                    x = 43,
                    y =  83.8678M
                },
                    new Coord
                {
                    x = 45,
                    y =  83.83998M
                },
                     new Coord
                {
                    x = 46,
                    y =  83.80888M
                },
                  new Coord
                {
                    x = 48,
                    y =  83.78899M
                },
                  new Coord
                {
                    x = 49,
                    y =  83.768989M
                },
                    new Coord
                {
                    x = 51,
                    y =  83.738989M
                },
                     new Coord
                {
                    x = 52,
                    y =  83.7147M
                },
                  new Coord
                {
                    x = 54,
                    y =  83.6975M
                },
                  new Coord
                {
                    x = 55,
                    y = 83.6577M
                },
                    new Coord
                {
                    x = 57,
                    y =  83.6479M
                },
     new Coord
                {
                    x = 59,
                    y =  83.6579M
                }
            };
            var xx = coordenadas.Select(x => x.x).ToArray();
            var yy = coordenadas.Select(x => x.y).ToArray();*/
            return this.Json(new
            {
                response = new 
                {
                    Code = 1,
                    Description = string.Empty,
                    Structure = pceViewModel,
                   // DataX= xx,
                    //DataY = yy,
                   // Totales = coordenadas
                }
            });
        }

        public IActionResult ValidateFilter(string noSerie, string clavePrueba, string claveIdioma, string TestPositionAT, string TestPositionBT, string TestPositionTer, int VNStart, int VNEnd, int VNInterval, string WindingEnergized, bool? Grafic, string comment)
        {
            if (VNStart > VNEnd)
            {
                return this.Json(new
                {
                    response = new ApiResponse<bool>
                    {
                        Code = -1,
                        Description = "Porcentaje inicial de Vn debe ser menor al final, favor de corregirlo",
                        Structure = false
                    }
                });
            }

            int renglones = ((VNEnd - VNStart) / VNInterval) + 1;

            if (TestType.AYD.ToString().Equals(clavePrueba))
            {
                if (renglones > 12)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<bool>
                        {
                            Code = -1,
                            Description = "Porcentajes de Vn no permitidos excede de 12 valores que soporta la plantilla, favor de corregirlos",
                            Structure = false
                        }
                    });
                }
                else
                {
                    if (Grafic ?? false)
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<bool>
                            {
                                Code = -1,
                                Description = "Este tipo de prueba no puede incluir grafica, por favor deshabilitarla",
                                Structure = false
                            }
                        });
                    }
                }
            }
            else
            {
                if (renglones > 24)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<bool>
                        {
                            Code = -1,
                            Description = "Porcentajes de Vn no permitidos excede de 24 valores que soporta la plantilla, favor de corregirlos",
                            Structure = false
                        }
                    });
                }
                else
                {
                    if (renglones < 6 && (Grafic ?? false))
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<bool>
                            {
                                Code = -1,
                                Description = "Porcentajes de Vn no suficientes para la grafica, favor de corregirlos o deshabilitar el grafico",
                                Structure = false
                            }
                        });
                    }
                }
            }

            return this.Json(new
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
        public async Task<IActionResult> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, string TestPositionAT, string TestPositionBT, string TestPositionTer, int VNStart, int VNEnd, int VNInterval, string WindingEnergized, bool? Grafic, string comment)
        {
            ApiResponse<SettingsToDisplayPCEReportsDTO> result = await this._gatewayClientService.GetTemplate(noSerie, clavePrueba, claveIdioma, TestPositionAT, TestPositionBT, TestPositionTer, VNStart, VNEnd, VNInterval, Grafic ?? false, WindingEnergized);

            if (result.Code.Equals(-1))
            {
                return this.View("Excel", new PceViewModel
                {
                    Error = result.Description
                });
            }

            SettingsToDisplayPCEReportsDTO reportInfo = result.Structure;

            #region Decode Template
            if (string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
            {
                return this.View("Excel", new PceViewModel
                {
                    Error = "No existe plantilla para el filtro seleccionado"
                });
            }

            long NoPrueba = reportInfo.NextTestNumber;

            byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
            Stream stream = new MemoryStream(bytes);

            Telerik.Web.Spreadsheet.Workbook workbook = Telerik.Web.Spreadsheet.Workbook.Load(stream, ".xlsx");
            try
            {
                this._pceService.PrepareTemplate_PCE(reportInfo, ref workbook, claveIdioma, VNStart, VNEnd, VNInterval,clavePrueba);
            }
            catch (Exception)
            {
                return this.View("Excel", new PceViewModel
                {
                    Error = "La plantilla esta mal configurada por favor verifiquela de inmediato"
                });
            }
            #endregion

            return this.View("Excel", new PceViewModel
            {
                ClaveIdioma = claveIdioma,
                ClavePrueba = clavePrueba,
                NoPrueba = NoPrueba,
                NoSerie = noSerie,
                PCEReportsDTO = result.Structure,
                Workbook = workbook,
                TestPositionAT = TestPositionAT,
                TestPositionBT = TestPositionBT,
                TestPositionTer = TestPositionTer,
                VNStart = VNStart,
                VNEnd = VNEnd,
                VNInterval = VNInterval,
                WindingEnergized = WindingEnergized,
                Grafic = Grafic ?? false,
                Error = string.Empty,
                Comments = comment
            });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] PceViewModel viewModel)
        {
            int renglones = ((viewModel.VNEnd - viewModel.VNStart) / viewModel.VNInterval) + 1;
            List<PCETestsDTO> pceTestDTOs = new()
            {
                new PCETestsDTO()
                {
                    GarCExcitacion = viewModel.PCEReportsDTO.Gar_Cexcitacion ?? 0,
                    GarPerdidas = viewModel.PCEReportsDTO.Gar_Perdidas ?? 0,
                    KeyTest = viewModel.ClavePrueba,
                    PorGarPerdidasTolerancy = viewModel.PCEReportsDTO.Tol_Gar_Perdidas ?? 0,
                    IInicio = viewModel.VNStart,
                    IFin = viewModel.VNEnd,
                    Intervalo = viewModel.VNInterval,
                    PosPruebaAT = viewModel.TestPositionAT,
                    PosPruebaBT = viewModel.TestPositionBT,
                    PosPruebaTER = viewModel.TestPositionTer,
                    VoltajeBase = viewModel.PCEReportsDTO.VoltajeBase,
                    Frecuencia = viewModel.PCEReportsDTO.Frecuencia ?? 0,
                    PCETestsDetails = new List<PCETestsDetailsDTO>(),
                }
            };

            if (viewModel.ClavePrueba == TestType.AYD.ToString())
            {
                pceTestDTOs.Add(new PCETestsDTO()
                {
                    GarCExcitacion = viewModel.PCEReportsDTO.Gar_Cexcitacion ?? 0,
                    KeyTest = viewModel.ClavePrueba,
                    GarPerdidas = viewModel.PCEReportsDTO.Gar_Perdidas ?? 0,
                    PorGarPerdidasTolerancy = viewModel.PCEReportsDTO.Tol_Gar_Perdidas ?? 0,
                    IInicio = viewModel.VNStart,
                    IFin = viewModel.VNEnd,
                    Intervalo = viewModel.VNInterval,
                    PosPruebaAT = viewModel.TestPositionAT,
                    PosPruebaBT = viewModel.TestPositionBT,
                    PosPruebaTER = viewModel.TestPositionTer,
                    VoltajeBase = viewModel.PCEReportsDTO.VoltajeBase,
                    Frecuencia = viewModel.PCEReportsDTO.Frecuencia ?? 0,
                    PCETestsDetails = new List<PCETestsDetailsDTO>()
                });
            }

            if (!this._pceService.Verify_PCE_Columns(viewModel.PCEReportsDTO, viewModel.Workbook, renglones))
            {
                return this.Json(new
                {
                    response = new ApiResponse<PceViewModel>
                    {
                        Code = -1,
                        Description = "Faltan datos por ingresar para poder calcular.",
                        Structure = viewModel
                    }
                });
            }

            decimal tolPer = 0;
            decimal tolExec = 0;
            if (viewModel.ClavePrueba == "AYD")
            {

                if (!decimal.TryParse(viewModel.PCEReportsDTO.ConfigurationReports.Where(x => x.Dato == "ToleranciaIexc").FirstOrDefault()?.Formato, out  tolExec))
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "No se ha podido encontrar el dato 'ToleranciaIexc' en la configuracion del reporte",
                            Structure = viewModel
                        }
                    });
                }
            }

          
            if (viewModel.ClavePrueba == "AYD")
            {

                if (!decimal.TryParse(viewModel.PCEReportsDTO.ConfigurationReports.Where(x => x.Dato == "ToleranciaPerdidas").FirstOrDefault()?.Formato, out  tolPer))
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<PceViewModel>
                        {
                            Code = -1,
                            Description = "No se ha podido encontrar el dato 'ToleranciaPerdidas' en la configuracion del reporte",
                            Structure = viewModel
                        }
                    });
                }
            }

            this._pceService.Prepare_PCE_Test(viewModel.PCEReportsDTO, viewModel.Workbook, ref pceTestDTOs);
            pceTestDTOs.ForEach(x => x.ToleranciaExec = tolExec);
            pceTestDTOs.ForEach(x => x.ToleranciaPer = tolPer);
            ApiResponse<ResultPCETestsDTO> resultTestPCE_Response = await this._pceClientService.CalculateTestPCE(pceTestDTOs);

            if (resultTestPCE_Response.Code.Equals(-1))
            {
                return this.Json(new
                {
                    response = new ApiResponse<PceViewModel>
                    {
                        Code = -1,
                        Description = string.Empty,
                        Structure = viewModel
                    }
                });
            }

            Telerik.Web.Spreadsheet.Workbook workbook = viewModel.Workbook;

            this._pceService.PrepareIndexOfPCE(resultTestPCE_Response.Structure, viewModel.PCEReportsDTO, viewModel.ClaveIdioma, ref workbook);

            #region fillColumns
            pceTestDTOs = resultTestPCE_Response.Structure.PCETests.ToList();
            #endregion

            viewModel.PceTestDTOs = pceTestDTOs.ToList();
            viewModel.Workbook = workbook;
            viewModel.OfficialWorkbook = workbook;
            bool resultReport = !resultTestPCE_Response.Structure.Results.Any(x => x.Fila != 77 && x.Column != 77);
            viewModel.IsReportAproved = resultReport;

            string errors = string.Empty;
            List<string> errorMessages = resultTestPCE_Response.Structure.Results.Select(k => k.Message).ToList();
            string allError = errorMessages.Any() ? errorMessages.Aggregate((c, n) => c + "\n*" + n) : string.Empty;
            List<Coord> coordenadas = new();
            if (viewModel.Grafic)
            {
                coordenadas = viewModel.PceTestDTOs.FirstOrDefault().PCETestsDetails.Select(row => new Coord { x = row.CorrienteIRMS, y = row.TensionKVRMS }).ToList();
            }


            
            return this.Json(new
            {
                response = new
                {
                    Code = 1,
                    Description = allError,
                    Structure = viewModel,
                    Coords = coordenadas,
                }
            }); ;
        }

        [HttpGet]
        public async Task<IActionResult> GetPDFReport(long code, string typeReport)
        {
            ApiResponse<ReportPDFDto> result = await this._reportClientService.GetPDFReport(code, typeReport);

            if (result.Code.Equals(-1))
            {
                return this.Json(new
                {
                    response = result
                });
            }

            byte[] bytes = Convert.FromBase64String(result.Structure.File);
            _ = new MemoryStream(bytes);

            return this.Json(new
            {
                data = bytes,
            });
        }

        [HttpPost]

        public async Task<IActionResult> SavePDF([FromBody] PceViewModel viewModel)
        {
            #region Export Excel
            int[] filas = new int[2];
            int[] _positionWB;
            int section;
            int renglones = ((viewModel.VNEnd - viewModel.VNStart) / viewModel.VNInterval) + 1;

            if (!_pceService.Verify_PCE_Columns(viewModel.PCEReportsDTO, viewModel.Workbook, renglones))
            {
                return this.Json(new
                {
                    response = new { status = -2, description = "Faltan datos por ingresar en la tabla" }
                });
            }

            List<DateTime> dates = this._pceService.GetDate(viewModel.Workbook, viewModel.PCEReportsDTO);

            foreach (PCETestsDTO item in viewModel.PceTestDTOs)
            {
                item.Date = dates[viewModel.PceTestDTOs.IndexOf(item)];
            }

            Telerik.Windows.Documents.Spreadsheet.Model.Workbook document = viewModel.Workbook.ToDocument();

           


            PdfFormatProvider provider = new()
            {
                ExportSettings = new PdfExportSettings(Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet, true)
            };
            document.ActiveSheet = document.Worksheets.First(sheet => sheet.Name == viewModel.Workbook.ActiveSheet);
            int rowCount = 0;  foreach (Telerik.Windows.Documents.Spreadsheet.Model.Worksheet sheet in document.Worksheets)
            {
                #region FormatPDF

                // Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment allign = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(15, 2)].GetHorizontalAlignment().Value;

                IEnumerable<ConfigurationReportsDTO> starts = viewModel.PCEReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Temperatura"));
                foreach (ConfigurationReportsDTO item in starts)
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    string val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                    sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 1));
                }

                section = 0;
                starts = viewModel.PCEReportsDTO.ConfigurationReports.Where(c => c.Dato.Equals("Porc_Vn"));
                foreach (ConfigurationReportsDTO item in starts)
                {
                    _positionWB = this.GetRowColOfWorbook(item.Celda);
                    for (int i = 0; i < renglones; i++)
                    {
                        // %%
                        string val = sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1])].GetValue().Value.RawValue;
                        sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1])].SetValueAsText(this.FormatStringDecimal((Convert.ToDecimal(val)*100).ToString(), 0) + " %");

                        // Losses KW
                        val = sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 2)].GetValue().Value.RawValue;
                        sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 2)].SetValueAsText(this.FormatStringDecimal(val, 3));

                        // Terminal 3
                        val = sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 3)].GetValue().Value.RawValue;
                        sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 3)].SetValueAsText(this.FormatStringDecimal(val, 3));

                        // Current RMS
                        val = sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 4)].GetValue().Value.RawValue;
                        sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 4)].SetValueAsText(this.FormatStringDecimal(val, 3));

                        // Voltage KV RMS
                        val = sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 5)].GetValue().Value.RawValue;
                        sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 5)].SetValueAsText(this.FormatStringDecimal(val, 3));

                        // Voltage KV AVG
                        val = sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 6)].GetValue().Value.RawValue;
                        sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 6)].SetValueAsText(this.FormatStringDecimal(val, 3));

                        // Corrected Losses	Wf. kW
                        val = sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 7)].GetValue().Value.RawValue;
                        sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 7)].SetValueAsText(this.FormatStringDecimal(val, 3));

                        // Corrected Losses	20°C kW
                        val = sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 8)].GetValue().Value.RawValue;
                        sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 8)].SetValueAsText(this.FormatStringDecimal(val, 3));

                        // Lexc %
                        val = sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 9)].GetValue().Value.RawValue;
                        sheet.Cells[new CellIndex(_positionWB[0] + i, _positionWB[1] + 9)].SetValueAsText(this.FormatStringDecimal(val, 3));
                    }
                }
                #endregion
                rowCount = sheet.UsedCellRange.RowCount;
                FloatingImage image = new(sheet, new CellIndex(0, 0), 0, 0);
                string path = Path.Combine(_hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
                FileStream stream = new(path, FileMode.Open);
                using (stream)
                {
                    image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                }

                image.Width = 215;
                image.Height = 38;

               

                sheet.Shapes.Add(image);

                if (viewModel.Grafic)
                {
                    byte[] imageBytes = Convert.FromBase64String(viewModel.Base64Graphic);
                    FloatingImage graphic = new(sheet, new CellIndex(39, 2), 0, 0);
                    MemoryStream stream2 = new(imageBytes);

                    using (stream2)
                    {
                        graphic.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream2, "jpg");
                    }

                    graphic.Width = 500;
                    graphic.Height = 160;
                    sheet.Shapes.Add(graphic);
                }

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

            pdfFile = pdfStream.ToArray();
            viewModel.Base64PDF = Convert.ToBase64String(pdfFile);
            #endregion

            #region Save Report
            PCETestsGeneralDTO pceTestsGeneralDTO = new()
            {
                Capacity = viewModel.PCEReportsDTO.HeadboardReport.Capacity,
                Comment = viewModel.Comments,
                Creadopor = this.User.Identity.Name,
                Customer = viewModel.PCEReportsDTO.HeadboardReport.Client,
                Data = viewModel.PceTestDTOs,
                Fechacreacion = DateTime.Now,
                File = Convert.FromBase64String(viewModel.Base64PDF),
                IdLoad = 0,
                KeyTest = viewModel.ClavePrueba,
                LanguageKey = viewModel.ClaveIdioma,
                LoadDate = DateTime.Now,
                Modificadopor = null,
                Fechamodificacion = null,
                NameFile = string.Concat("PCE", viewModel.ClavePrueba, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                Result = viewModel.IsReportAproved,
                SerialNumber = viewModel.NoSerie,
                TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                TypeReport = ReportType.PCE.ToString(),
                EnergizedWinding = viewModel.WindingEnergized
            };
            #endregion

            try
            {
                string a = JsonConvert.SerializeObject(pceTestsGeneralDTO);
                ApiResponse<long> result = await this._pceClientService.SaveReport(pceTestsGeneralDTO);

                var resultResponse = new { status = result.Code, description = result.Description, nameFile = pceTestsGeneralDTO.NameFile, file = pceTestsGeneralDTO.File };

                return this.Json(new
                {
                    response = resultResponse
                });
            }
            catch (Exception ex)
            {
                return this.Json(new
                {
                    response = new { status = -1, description = ex.Message, nameFile = "", file = "" }
                });
            }
        }
        public IActionResult Error() => this.View();

        partial class Coord
        {
            public decimal x { get; set; }
            public decimal y { get; set; }
        }

        #region PrivateMethods
        public async Task PrepareForm()
        {
            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            // Tipos de prueba
            ApiResponse<IEnumerable<TestsDTO>> reportResult = await this._testClientService.GetTest(ReportType.PCE.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            this.ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");

            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            this.ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");

            this.ViewBag.WindingEnergizeds = new SelectList(new List<GeneralPropertiesDTO>()
            {
                new GeneralPropertiesDTO() {
                    Clave = "", Descripcion="Seleccione..."
                },
                new GeneralPropertiesDTO() {
                    Clave = "AT", Descripcion="AT"
                },
                new GeneralPropertiesDTO() {
                    Clave = "BT", Descripcion="BT"
                },
                new GeneralPropertiesDTO() {
                    Clave = "Ter", Descripcion="Ter"
                }
            }, "Clave", "Descripcion");
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
                    num = decimals == 0 ? $"{entero}" : $"{entero}.{decima}";
                }
                else
                {
                    if (decimals != 0)
                    {
                        num += ".".PadRight(decimals+1, '0');
                    }
                }
            }
            return num;
        }
        #endregion
    }
}

