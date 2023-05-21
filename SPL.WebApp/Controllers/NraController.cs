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
    using SPL.WebApp.Domain.DTOs.NRA;
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
    public class NraController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly INraClientService _nraClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly INraService _nraService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IInformationOctavesService _octavas;
        private readonly IConfigurationClientService _configurationClientService;
        private readonly IProfileSecurityService _profileClientService;
        public NraController(
            IMasterHttpClientService masterHttpClientService,
            INraClientService nraClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            INraService nraService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            IInformationOctavesService octavas,
            IConfigurationClientService configurationClientService,
            IProfileSecurityService profileClientService
            )
        {
            this._masterHttpClientService = masterHttpClientService;
            this._nraClientService = nraClientService;
            this._reportClientService = reportClientService;
            this._artifactClientService = artifactClientService;
            this._testClientService = testClientService;
            this._gatewayClientService = gatewayClientService;
            this._nraService = nraService;
            this._correctionFactorService = correctionFactorService;
            this._hostEnvironment = hostEnvironment;
            this._octavas = octavas;
            this._configurationClientService = configurationClientService;
            this._profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(User.Identity.Name).Result;


                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.RuidoAudible)))
                {


                    await this.PrepareForm();
                    var noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    return this.View(new NraViewModel { CheckBox = true, NoSerie = noSerie });
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
        public async Task<IActionResult> GetInfo(string noSerie)
        {
            try
            {
                var g = decimal.MaxValue;
                //g = g + ((decimal)1);

                var l = Math.Log((double)g) / 10;

                decimal r = (decimal)Math.Round(Math.Pow(2.71828182846, l));
                r = r + (decimal)0.04688;
                var ggg = Math.Pow((double)r, 10);

                NraViewModel data = new NraViewModel();
                string noSerieSimple = string.Empty;

                if (!string.IsNullOrEmpty(noSerie))
                {
                    noSerieSimple = noSerie.Split("-")[0];
                }
                else
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<NraViewModel>
                        {
                            Code = -1,
                            Description = "No. Serie inválido.",
                            Structure = null
                        }
                    });
                }
                InformationArtifactDTO artifactDesing = await this._artifactClientService.GetArtifact(noSerieSimple);
                var tipoEnfriamiento = artifactDesing.CharacteristicsArtifact.Select(x => x.CoolingType).Distinct().ToList().Select(x => new GeneralPropertiesDTO { Descripcion = x, Clave = x }).Distinct().ToList();
                data.Norma = artifactDesing.GeneralArtifact.Norma;

                var normas = await this._masterHttpClientService.GetRuleEquivalents();
                var data2 = normas.Select(x => new GeneralPropertiesDTO { Clave = x.Clave, Descripcion = x.Descripcion }).Distinct().ToList();
                data.Normas = new SelectList(data2, "Clave", "Descripcion");
                data.ClaveIdioma = artifactDesing.GeneralArtifact.ClaveIdioma;

                IEnumerable<GeneralPropertiesDTO> tipoInformacion = new List<GeneralPropertiesDTO>()
                {
                }.AsEnumerable();
                data.Enfriamientos = new SelectList(tipoInformacion.Concat(tipoEnfriamiento), "Clave", "Descripcion");

                ApiResponse<List<InfoGeneralTypesReportsDTO>> resultFilter = await this._nraClientService.GetFilter(noSerie);

                if (resultFilter.Code.Equals(1))
                {
                    InfoGeneralTypesReportsDTO reportRdt = resultFilter.Structure.FirstOrDefault(c => c.TipoReporte.Equals(ReportType.NRA.ToString())) ?? new InfoGeneralTypesReportsDTO() { ListDetails = new List<InfoGeneralReportsDTO>(), TipoReporte = ReportType.NRA.ToString() };
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

                    data.TreeViewItem = new List<TreeViewItemDTO>()
                {
                    new TreeViewItemDTO
                    {
                        id = "1",
                        text = "Ruido Audible",
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

                return this.Json(new
                {
                    response = new ApiResponse<NraViewModel>
                    {
                        Code = 1,
                        Description = "Información Procesada",
                        Structure = data

                    }
                });

            }
            catch (Exception e)
            {
                return this.Json(new
                {
                    response = new ApiResponse<NraViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = null
                    }
                });
            }


        }

        [HttpGet]
        public async Task<IActionResult> CheckInfo(string noSerie, string tipoInformacion, DateTime? fecha, string keyTest,
            bool esCargaData, string valorSelectCargaData, string enfriamiento, DateTime dataDate, int cantidadMediciones, string alturaSelected)
        {
            int cantidadColunmas = 0;
            int cantidadDeAntesoDespues = 0;
            int cantidadMaximadeMediciones = 0;
            ApiResponse<List<InformationOctavesDTO>> infoAntes = new ApiResponse<List<InformationOctavesDTO>>();
            ApiResponse<List<InformationOctavesDTO>> infoDespues = new ApiResponse<List<InformationOctavesDTO>>();
            ApiResponse<List<InformationOctavesDTO>> infoCurrentTypeInfo = new ApiResponse<List<InformationOctavesDTO>>();
            string alutraDefault = string.Empty;

            try
            {
                NraViewModel data = new NraViewModel();
                string noSerieSimple = string.Empty;

                if (!string.IsNullOrEmpty(noSerie))
                {
                    noSerieSimple = noSerie.Split("-")[0];
                }
                else
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<NraViewModel>
                        {
                            Code = -1,
                            Description = "No. Serie inválido.",
                            Structure = null
                        }
                    });
                }

                if (esCargaData)
                {
                    infoAntes = await this._octavas.GetInfoOctavas(noSerie, "ANT", fecha?.ToString("yyyy-MM-dd"));
                    infoDespues = await this._octavas.GetInfoOctavas(noSerie, "DES", fecha?.ToString("yyyy-MM-dd"));
                    infoCurrentTypeInfo = await this._octavas.GetInfoOctavas(noSerie, tipoInformacion, fecha?.ToString("yyyy-MM-dd"));



                    if (infoAntes.Structure.Count() == 0 || infoDespues.Structure.Count() == 0 || infoCurrentTypeInfo.Structure.Count() == 0)
                    {
                        return this.Json(new
                        {
                            response = new ApiResponse<NraViewModel>
                            {
                                Code = -1,
                                Description = "La información de octavas para el aparato no está completa se requiere para el tipo de información antes, después y el tipo de enfriamiento seleccionado",
                                Structure = null
                            }
                        });

                    }
                }

                if (esCargaData)
                {
                    alutraDefault = infoAntes.Structure.FirstOrDefault()?.Altura;

                    if (alutraDefault == "1/3" || alutraDefault == "2/3")
                    {
                        cantidadDeAntesoDespues = infoAntes.Structure.Where(x => x.Altura == "1/3" || x.Altura == "2/3").Count();
                    }
                    else
                    {
                        cantidadDeAntesoDespues = infoAntes.Structure.Where(x => x.Altura == "1/2").Count();
                    }


                    if (cantidadDeAntesoDespues == 0)
                    {

                        return this.Json(new
                        {
                            response = new ApiResponse<NraViewModel>
                            {
                                Code = -1,
                                Description = "La cantidad mediciones para antes y despues es igual a 0",
                                Structure = null
                            }
                        });
                    }
                    else
                    {
                        if (cantidadDeAntesoDespues >= 1 && cantidadDeAntesoDespues <= 9)
                        {
                            cantidadColunmas = 4;
                        }
                        else
                        {
                            cantidadColunmas = 10;
                        }
                    }

                    alutraDefault = infoCurrentTypeInfo.Structure.FirstOrDefault()?.Altura;

                    cantidadMaximadeMediciones = alutraDefault == "1/3" || alutraDefault == "2/3" ? infoCurrentTypeInfo.Structure.Where(x => x.Altura == "1/3").Count() :
                        infoCurrentTypeInfo.Structure.Where(x => x.Altura == "1/2").Count();


                    if (cantidadMaximadeMediciones == 0)
                    {

                        return this.Json(new
                        {
                            response = new ApiResponse<NraViewModel>
                            {
                                Code = -1,
                                Description = "La cantidad mediciones para la altura seleccionada, fecha y tipo de enfriamiento no puede ser 0 ",
                                Structure = null
                            }
                        });
                    }

                    if (cantidadMediciones > cantidadMaximadeMediciones)
                    {

                        return this.Json(new
                        {
                            response = new ApiResponse<NraViewModel>
                            {
                                Code = -1,
                                Description = "La cantidad valida de mediciones es hasta un maximo de " + cantidadMaximadeMediciones,
                                Structure = null
                            }
                        });
                    }

                    if (cantidadMaximadeMediciones > cantidadMediciones)
                    {
                        cantidadMaximadeMediciones = cantidadMediciones;
                    }

                    if (keyTest.ToUpper() == "OCT" && cantidadColunmas == 4)
                    {
                        if (alutraDefault == "1/3" || alutraDefault == "2/3")
                        {

                            if (cantidadMediciones > 39)
                            {
                                return this.Json(new
                                {
                                    response = new ApiResponse<NraViewModel>
                                    {
                                        Code = -1,
                                        Description = "La cantidad de mediciones para 1/3 - 2/3 excede las permitidas en la plantilla (solo acepta hasta 39)",
                                        Structure = null
                                    }
                                });
                            }
                        }
                        else
                        {
                            if (cantidadMediciones > 78)
                            {
                                return this.Json(new
                                {
                                    response = new ApiResponse<NraViewModel>
                                    {
                                        Code = -1,
                                        Description = "La cantidad de mediciones para 1/2 excede las permitidas en la plantilla (solo acepta hasta 39)",
                                        Structure = null
                                    }
                                });
                            }
                        }
                    }
                    else if (keyTest.ToUpper() == "OCT" && cantidadColunmas == 10)
                    {
                        if (alutraDefault == "1/3" || alutraDefault == "2/3")
                        {

                            if (cantidadMediciones > 33)
                            {
                                return this.Json(new
                                {
                                    response = new ApiResponse<NraViewModel>
                                    {
                                        Code = -1,
                                        Description = "La cantidad de mediciones para 1/3 - 2/3 excede las permitidas en la plantilla (solo acepta hasta 33)",
                                        Structure = null
                                    }
                                });
                            }
                        }
                        else
                        {
                            if (cantidadMediciones > 66)
                            {
                                return this.Json(new
                                {
                                    response = new ApiResponse<NraViewModel>
                                    {
                                        Code = -1,
                                        Description = "La cantidad de mediciones para 1/2 excede las permitidas en la plantilla (solo acepta hasta 33)",
                                        Structure = null
                                    }
                                });
                            }
                        }
                    }

                    if (keyTest.ToUpper() == "RUI" && cantidadColunmas == 4)
                    {

                        if (cantidadMediciones > 70)
                        {
                            return this.Json(new
                            {
                                response = new ApiResponse<NraViewModel>
                                {
                                    Code = -1,
                                    Description = "La cantidad de mediciones excede las permitidas en la plantilla (solo acepta hasta 70)",
                                    Structure = null
                                }
                            });
                        }
                    }
                    else if (keyTest.ToUpper() == "RUI" && cantidadColunmas == 10)
                    {

                        if (cantidadMediciones > 60)
                        {
                            return this.Json(new
                            {
                                response = new ApiResponse<NraViewModel>
                                {
                                    Code = -1,
                                    Description = "La cantidad de mediciones excede las permitidas en la plantilla (solo acepta hasta 60)",
                                    Structure = null
                                }
                            });
                        }
                    }

                }
                else//entra aca cuando es RUI y la carga es manual
                {
                    cantidadColunmas = valorSelectCargaData.ToUpper() == "4 MEDICIONES ANTES Y DESPUÉS DE AMBIENTE" ? 4 : 10;
                    alutraDefault = alturaSelected;

                    if (keyTest.ToUpper() == "OCT" && cantidadColunmas == 4)
                    {
                        if (alutraDefault == "1/3" || alutraDefault == "2/3")
                        {
                            if (cantidadMediciones > 39)
                            {
                                return this.Json(new
                                {
                                    response = new ApiResponse<NraViewModel>
                                    {
                                        Code = -1,
                                        Description = "La cantidad de mediciones para 1/3 - 2/3 excede las permitidas en la plantilla (solo acepta hasta 39)",
                                        Structure = null
                                    }
                                });
                            }
                        }
                        else
                        {
                            if (cantidadMediciones > 78)
                            {
                                return this.Json(new
                                {
                                    response = new ApiResponse<NraViewModel>
                                    {
                                        Code = -1,
                                        Description = "La cantidad de mediciones para 1/2 excede las permitidas en la plantilla (solo acepta hasta 78)",
                                        Structure = null
                                    }
                                });
                            }
                        }
                    }
                    else if (keyTest.ToUpper() == "OCT" && cantidadColunmas == 10)
                    {
                        if (alutraDefault == "1/3" || alutraDefault == "2/3")
                        {

                            if (cantidadMediciones > 33)
                            {
                                return this.Json(new
                                {
                                    response = new ApiResponse<NraViewModel>
                                    {
                                        Code = -1,
                                        Description = "La cantidad de mediciones para 1/3 - 2/3 excede las permitidas en la plantilla (solo acepta hasta 33)",
                                        Structure = null
                                    }
                                });
                            }
                        }
                        else
                        {
                            if (cantidadMediciones > 66)
                            {
                                return this.Json(new
                                {
                                    response = new ApiResponse<NraViewModel>
                                    {
                                        Code = -1,
                                        Description = "La cantidad de mediciones para 1/2 excede las permitidas en la plantilla (solo acepta hasta 66)",
                                        Structure = null
                                    }
                                });
                            }
                        }
                    }

                    if (keyTest.ToUpper() == "RUI" && cantidadColunmas == 4)
                    {

                        if (cantidadMediciones > 70)
                        {
                            return this.Json(new
                            {
                                response = new ApiResponse<NraViewModel>
                                {
                                    Code = -1,
                                    Description = "La cantidad de mediciones excede las permitidas en la plantilla (solo acepta hasta 70)",
                                    Structure = null
                                }
                            });
                        }
                    }
                    else if (keyTest.ToUpper() == "RUI" && cantidadColunmas == 10)
                    {

                        if (cantidadMediciones > 60)
                        {
                            return this.Json(new
                            {
                                response = new ApiResponse<NraViewModel>
                                {
                                    Code = -1,
                                    Description = "La cantidad de mediciones excede las permitidas en la plantilla (solo acepta hasta 60)",
                                    Structure = null
                                }
                            });
                        }
                    }


                    cantidadMaximadeMediciones = cantidadMediciones;
                    if (cantidadMaximadeMediciones < cantidadMediciones)
                    {
                        cantidadMediciones = cantidadMaximadeMediciones;
                    }


                    if (keyTest.ToUpper() == "RUI")
                    {
                        alutraDefault = alturaSelected;
                    }
                    else
                    {
                        infoCurrentTypeInfo = await this._octavas.GetInfoOctavas(noSerie, enfriamiento, fecha?.ToString("yyyy-MM-dd"));
                        alutraDefault = infoCurrentTypeInfo.Structure.FirstOrDefault().Altura;
                    }


                }

                alutraDefault = (alutraDefault == "2/3") ? "1/3" : alutraDefault; /*Para antes y después solo se muestra la información de la altura de 1/3*/

                data.CantidadColumnas = cantidadColunmas;
                data.Altura = alutraDefault;
                data.CantidadMaximadeMediciones = cantidadMaximadeMediciones;
                data.PruebasAntes = infoAntes.Structure?.ToList();
                data.PruebasDespues = infoDespues.Structure?.ToList();
                data.PruebasEnfriamiento = infoCurrentTypeInfo.Structure?.ToList();

                return this.Json(new
                {
                    response = new ApiResponse<NraViewModel>
                    {
                        Code = 1,
                        Description = "Información Procesada",
                        Structure = data

                    }
                });

            }
            catch (Exception e)
            {
                return this.Json(new
                {
                    response = new ApiResponse<NraViewModel>
                    {
                        Code = 1,
                        Description = string.Empty,
                        Structure = null
                    }
                });
            }


        }



        [HttpGet]
        public async Task<IActionResult> GetTemplate(string nroSerie, string keyTest, string language,
            bool esCargaData, string valorSelectCargaData, string laboratorio, string norma,
            string alimentacion, string alimentacionRespaldo, decimal valorAlimentacion,
            string enfriamiento, DateTime? dataDate, int cantidadColumnas, string alutraDefault,
            int cantidadMaximadeMediciones, string comentarios)
        {
            try
            {
                ApiResponse<SettingsToDisplayNRAReportsDTO> result = await this._gatewayClientService.GetTemplateNRA(nroSerie, keyTest, language, esCargaData, valorSelectCargaData, laboratorio, norma, alimentacion, valorAlimentacion, enfriamiento, cantidadColumnas, !esCargaData ? null : dataDate, cantidadMaximadeMediciones);

                if (result.Code.Equals(-1))
                {
                    return this.View("Excel", new NraViewModel
                    {
                        Error = result.Description
                    });
                }

                SettingsToDisplayNRAReportsDTO reportInfo = result.Structure;
                reportInfo.MatrixHeight12 = !esCargaData ? LlenarLista(cantidadMaximadeMediciones) : reportInfo.MatrixHeight12.Where(y => y.Height == "1/2").ToList();
                reportInfo.MatrixHeight13 = !esCargaData ? LlenarLista(cantidadMaximadeMediciones) : reportInfo.MatrixHeight13.Where(y => y.Height == "1/3").ToList();
                reportInfo.MatrixHeight23 = !esCargaData ? LlenarLista(cantidadMaximadeMediciones) : reportInfo.MatrixHeight23.Where(y => y.Height == "2/3").ToList();
                reportInfo.matrixThreeAnt = !esCargaData ? LlenarLista2(cantidadColumnas) : reportInfo.matrixThreeAnt;
                reportInfo.matrixThreeDes = !esCargaData ? LlenarLista2(cantidadColumnas) : reportInfo.matrixThreeDes;
                reportInfo.Posiciones = (await this._gatewayClientService.GetPositions(nroSerie)).Structure;
                if (reportInfo.BaseTemplate == null || string.IsNullOrEmpty(reportInfo.BaseTemplate.Plantilla))
                {
                    return this.View("Excel", new NraViewModel
                    {
                        Error = "No existe plantilla para el filtro seleccionado"
                    });
                }


                if (esCargaData)
                {
                    if (keyTest == "RUI")
                    {

                        if (reportInfo.matrixThreeAnt == null || reportInfo.matrixThreeAnt.Count() == 0)
                        {
                            return this.View("Excel", new NraViewModel
                            {
                                Error = "La matriz de información 'Antes' esta vacia"
                            });
                        }

                        if (reportInfo.matrixThreeDes == null || reportInfo.matrixThreeDes.Count() == 0)
                        {
                            return this.View("Excel", new NraViewModel
                            {
                                Error = "La matriz de información 'Despues' esta vacia"
                            });
                        }

                        if ((alutraDefault == "1/3" || alutraDefault == "2/3") && ((reportInfo.MatrixHeight13 == null || reportInfo.MatrixHeight13.Count() == 0) || (reportInfo.MatrixHeight23 == null || reportInfo.MatrixHeight23.Count() == 0)))
                        {
                            return this.View("Excel", new NraViewModel
                            {
                                Error = "LA información para la matriz de 2/3 o 1/3 esta vacia"
                            });
                        }

                        if (alutraDefault == "1/2" && (reportInfo.MatrixHeight12 == null || reportInfo.MatrixHeight12.Count() == 0))
                        {
                            return this.View("Excel", new NraViewModel
                            {
                                Error = "LA información para la matriz de 1/2 esta vacia"
                            });
                        }
                    }

                    if (keyTest == "OCT")
                    {
                        if (reportInfo.matrixThreeAnt == null || reportInfo.matrixThreeAnt.Count() == 0)
                        {
                            return this.View("Excel", new NraViewModel
                            {
                                Error = "La matriz de información 'Antes' esta vacia"
                            });
                        }

                        if (reportInfo.matrixThreeDes == null || reportInfo.matrixThreeDes.Count() == 0)
                        {
                            return this.View("Excel", new NraViewModel
                            {
                                Error = "La matriz de información 'Despues' esta vacia"
                            });
                        }

                        if (reportInfo.matrixThreeCoolingType == null || reportInfo.matrixThreeCoolingType.Count() == 0)
                        {
                            return this.View("Excel", new NraViewModel
                            {
                                Error = "La matriz de tipo de enfriamiento esta vacia"
                            });
                        }
                    }
                }

                long NoPrueba = reportInfo.NextTestNumber;
                
                /*   byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\Barboza\Downloads\G4673-01 OL.xlsx");
                   Stream stream2 = new MemoryStream(bytes);
                   Workbook workbook = Workbook.Load(stream2, ".xlsx");*/

                byte[] bytes = Convert.FromBase64String(reportInfo.BaseTemplate.Plantilla);
                Stream stream = new MemoryStream(bytes);

                Workbook workbook = Workbook.Load(stream, ".xlsx");
                int filas = 0;
                int count = 0;
                bool activarSegundoHeader = false;
                List<MatrixThreeDTO> UnionMatrices = new List<MatrixThreeDTO>();
                string medicionCorriente = "";
                string tituloAlimentacion = "";
                this._nraService.PrepareTemplateNRA(reportInfo, ref tituloAlimentacion, ref UnionMatrices,
                    ref activarSegundoHeader, ref workbook, keyTest, cantidadColumnas,
                    cantidadMaximadeMediciones, alutraDefault, esCargaData, language, alimentacion, valorAlimentacion.ToString(), enfriamiento, ref medicionCorriente);

                return this.View("Excel", new NraViewModel
                {
                    SettingsNRA = reportInfo,
                    ClaveIdioma = language,
                    Pruebas = keyTest,
                    NoPrueba = NoPrueba.ToString(),
                    NoSerie = nroSerie,
                    Workbook = workbook,
                    Error = string.Empty,
                    Norma = norma,
                    CantMediciones = cantidadMaximadeMediciones.ToString(),
                    Laboratorio = laboratorio,
                    Enfriamiento = enfriamiento,
                    Alimentacion = tituloAlimentacion,
                    ValorAlimentacion = valorAlimentacion.ToString(),
                    Altura = alutraDefault,
                    EsCargaData = esCargaData,
                    CantidadColumnas = cantidadColumnas,
                    Positions = reportInfo.Posiciones,
                    MatrixHeight12 = !esCargaData ? LlenarLista(cantidadMaximadeMediciones) : result.Structure.MatrixHeight12,
                    MatrixHeight23 = !esCargaData ? LlenarLista(cantidadMaximadeMediciones) : result.Structure.MatrixHeight23,
                    MatrixHeight13 = !esCargaData ? LlenarLista(cantidadMaximadeMediciones) : result.Structure.MatrixHeight13,
                    matrixThreeAnt = !esCargaData ? LlenarLista2(cantidadColumnas) : result.Structure.matrixThreeAnt,
                    matrixThreeDes = !esCargaData ? LlenarLista2(cantidadColumnas) : result.Structure.matrixThreeDes,
                    Warranty = result.Structure.Warranty,
                    ActivarSegundoHeader = activarSegundoHeader,
                    UnionMatrices = UnionMatrices,
                    AlimentacionRespaldo = alimentacionRespaldo,
                    Fecha = dataDate,
                    MedicionCorriente = medicionCorriente,
                    Comments = comentarios

                });
            }
            catch (Exception e)
            {
                return this.View("Excel", new NraViewModel
                {
                    Error = e.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePDF([FromBody] NraViewModel viewModel)
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

                var _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Amb+Trans")).Celda);
                var ambtrans = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AmbienteProm")).Celda);
                var ambprom = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();




                if (string.IsNullOrEmpty(ambtrans) || string.IsNullOrEmpty(ambprom))
                {
                    return this.Json(new
                    {
                        response = new { Code = -1, Description = "Debe introducir el AmbTrans y el AmbProm para procesar el reporte", nameFile = "", file = "" }
                    });
                }


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

                    string celdapage = viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado") && c.Seccion == 1).Celda;
                    int posicionpage = Convert.ToInt32(celdapage.Remove(0, 1)) - 1;

                    PageBreaks pageBreaks = sheet.WorksheetPageSetup.PageBreaks;


                    if (viewModel.Pruebas == "RUI")
                    {
                        _ = pageBreaks.TryInsertHorizontalPageBreak(posicionpage + 9, 0);
                        viewModel.Notas = null;
                    }
                    else
                    {
                        _ = pageBreaks.TryInsertHorizontalPageBreak(posicionpage + 11, 0);
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



                    _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelPresionProm")).Celda);
                    var val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                    sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 0));


                    _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PotenciaRuido")).Celda);
                    val = sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].GetValue().Value.RawValue;
                    sheet.Cells[new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(_positionWB[0], _positionWB[1])].SetValueAsText(this.FormatStringDecimal(val, 0));

                }



                //    image = new FloatingImage(sheet, new Telerik.Windows.Documents.Spreadsheet.Model.CellIndex(0, 0), 0, 0);
                //    string path = Path.Combine(this._hostEnvironment.WebRootPath + "\\images\\", "prolecge_excel.jpg");
                //    FileStream stream = new(path, FileMode.Open);
                //    using (stream)
                //    {
                //        image.ImageSource = new Telerik.Windows.Documents.Media.ImageSource(stream, "jpg");
                //    }

                //    image.Width = 215;
                //    image.Height = 38; 

                //    sheet.Shapes.Add(image);
                //    sheet.WorksheetPageSetup.PaperType = Telerik.Windows.Documents.Model.PaperTypes.A4;
                //    sheet.WorksheetPageSetup.PageOrientation = Telerik.Windows.Documents.Model.PageOrientation.Portrait;
                //    sheet.WorksheetPageSetup.PageOrientation = Telerik.Windows.Documents.Model.PageOrientation.Portrait;
                //    sheet.WorksheetPageSetup.CenterHorizontally = true;
                //    sheet.WorksheetPageSetup.PrintOptions.PrintGridlines = false;
                //    sheet.WorksheetPageSetup.ScaleFactor = new Telerik.Documents.Primitives.Size(0.9, 0.9);
                //    sheet.WorksheetPageSetup.Margins =
                //         new Telerik.Windows.Documents.Spreadsheet.Model.Printing.PageMargins(0
                //         , 20, 0, 20);
                //}

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
                string fecha;

                var positions = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Contains("Fecha")).Celda);
                fecha = viewModel.Workbook.Sheets[0].Rows[positions[0]].Cells[positions[1]]?.Value?.ToString();

                fecha = fecha.Contains("/") ? (DateTime.Now.Date - basedate.Date).TotalDays.ToString() : fecha;
                viewModel.Date = fecha;
                viewModel.NRAPruebas.Data.KFactor = viewModel.NRAResults.NRATestsGeneral.Data.KFactor;
                viewModel.NRAPruebas.Data.Surface = viewModel.NRAResults.NRATestsGeneral.Data.Surface;
                viewModel.NRAPruebas.Data.AvgSoundPressureLevel = viewModel.NRAResults.NRATestsGeneral.Data.AvgSoundPressureLevel;
                viewModel.NRAPruebas.Data.SoundPowerLevel = viewModel.NRAResults.NRATestsGeneral.Data.SoundPowerLevel;
                if (viewModel.NRAPruebas.Data.NRATestsDetailsRuis != null && viewModel.NRAResults.NRATestsGeneral.Data.NRATestsDetailsRuis != null)
                {
                    viewModel.NRAPruebas.Data.NRATestsDetailsRuis.MatrixNRAAntTests = viewModel.NRAResults.NRATestsGeneral.Data.NRATestsDetailsRuis.MatrixNRAAntTests;
                    viewModel.NRAPruebas.Data.NRATestsDetailsRuis.MatrixNRADesTests = viewModel.NRAResults.NRATestsGeneral.Data.NRATestsDetailsRuis.MatrixNRADesTests;
                    viewModel.NRAPruebas.Data.NRATestsDetailsRuis.MatrixNRA13Tests = viewModel.NRAResults.NRATestsGeneral.Data.NRATestsDetailsRuis.MatrixNRA13Tests;
                    viewModel.NRAPruebas.Data.NRATestsDetailsRuis.MatrixNRA23Tests = viewModel.NRAResults.NRATestsGeneral.Data.NRATestsDetailsRuis.MatrixNRA23Tests;
                    viewModel.NRAPruebas.Data.NRATestsDetailsRuis.AverageAmb = viewModel.NRAResults.NRATestsGeneral.Data.NRATestsDetailsRuis.AverageAmb;
                    viewModel.NRAPruebas.Data.NRATestsDetailsRuis.AmbTrans = viewModel.NRAResults.NRATestsGeneral.Data.NRATestsDetailsRuis.AmbTrans;
                }

                viewModel.NRAPruebas.Data.Guaranteed = !string.IsNullOrEmpty(viewModel.Warranty) ? decimal.Parse(viewModel.Warranty) : 0;
                NRATestsGeneralDTO NRAGeneralTest = new NRATestsGeneralDTO
                {
                    Capacity = viewModel.SettingsNRA.HeadboardReport.Capacity,
                    Feeding = viewModel.AlimentacionRespaldo,
                    FeedValue = decimal.Parse(viewModel.ValorAlimentacion),
                    UmFeeding = viewModel.MedicionCorriente,
                    Comment = viewModel.Comments,
                    Creadopor = User.Identity.Name,
                    Customer = viewModel.SettingsNRA.HeadboardReport.Client,
                    Fechacreacion = DateTime.Now,
                    File = pdfFile,
                    KeyTest = viewModel.Pruebas,
                    LanguageKey = viewModel.ClaveIdioma,
                    DateData = viewModel.Fecha != null ? viewModel.Fecha.Value : DateTime.Now,
                    LoadDate = DateTime.Now,
                    TestDate = basedate.AddDays(int.Parse(viewModel.Date)),
                    Modificadopor = null,
                    NameFile = string.Concat("NRA", viewModel.Pruebas, viewModel.ClaveIdioma, viewModel.NoSerie, Convert.ToInt32(viewModel.NoPrueba), "-" + rowCount, ".pdf"),
                    Result = viewModel.IsReportAproved,
                    SerialNumber = viewModel.NoSerie,
                    TestNumber = Convert.ToInt32(viewModel.NoPrueba),
                    TypeReport = ReportType.NRA.ToString(),
                    Data = viewModel.NRAPruebas.Data,
                    Laboratory = viewModel.Laboratorio,
                    CoolingType = viewModel.Enfriamiento,
                    LoadInfo = viewModel.EsCargaData,
                    QtyMeasurements = decimal.Parse(viewModel.CantMediciones),
                    MediAyd = viewModel.CantidadColumnas,
                    Rule = viewModel.Norma,
                    Grades = viewModel.Notas,
                };
                string p = JsonConvert.SerializeObject(NRAGeneralTest);

                try
                {
                    ApiResponse<long> result = await this._nraClientService.SaveReport(NRAGeneralTest);

                    var resultResponse = new { Code = result.Code, Description = result.Description, nameFile = NRAGeneralTest.NameFile, file = viewModel.Base64PDF };

                    return this.Json(new
                    {
                        response = resultResponse
                    });
                }
                catch (Exception ex)
                {
                    return this.Json(new
                    {
                        response = new { Code = -1, Description = ex.Message, nameFile = "", file = "" }
                    });
                }
            }
            catch (Exception ex)
            {
                return this.Json(new
                {
                    response = new { Code = -1, Description = ex.Message, nameFile = "", file = "" }
                });
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetConfigurationFiles(int pIdModule)
        {
            ApiResponse<IEnumerable<FileWeightDTO>> getConfigModulResponse = await this._masterHttpClientService.GetConfigurationFiles(pIdModule);
            object resultado = new();
            if (getConfigModulResponse.Code == 1)
            {
                resultado = getConfigModulResponse.Structure.Select(x => new
                {
                    PesoMaximo = int.Parse(x.MaximoPeso),
                    x.ExtensionArchivoNavigation.Extension
                }).ToList();

                return this.Json(new
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

                return this.Json(new
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

        [HttpPost]
        public async Task<IActionResult> ValidateData([FromBody] NraViewModel viewModel)
        {
            try
            {
                NRATestsGeneralDTO salida = new NRATestsGeneralDTO(); ;
                NRATestsGeneralDTO salida2 = new NRATestsGeneralDTO(); ;
                int[] _positionWB = new int[] { };
                var infoLab = await this._configurationClientService.GetInformationLaboratories();
                string error = this._nraService.ValidateTemplate(viewModel.SettingsNRA, viewModel.Workbook, viewModel.EsCargaData, int.Parse(viewModel.CantMediciones), viewModel.Pruebas, viewModel.CantidadColumnas, viewModel.Altura,
                    viewModel.Norma, viewModel.Enfriamiento, infoLab.Structure, viewModel.Laboratorio,
                    viewModel.matrixThreeAnt, viewModel.matrixThreeDes, viewModel.MatrixHeight12, viewModel.MatrixHeight13, viewModel.MatrixHeight23, viewModel.Positions, viewModel.ActivarSegundoHeader, viewModel.UnionMatrices, ref salida);
                if (error != string.Empty)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<NraViewModel>
                        {
                            Code = -1,
                            Description = error
                        }
                    });
                }

                salida.Data.UmLength = "m";
                salida.Data.UmSurface = "m2";
                salida2 = salida;
                salida.LanguageKey = viewModel.ClaveIdioma;
                salida.Data.Guaranteed = decimal.Parse(viewModel.Warranty);
                ApiResponse<ResultNRATestsDTO> calculateResult = await this._nraClientService.CalculateTestNra(salida);
                List<string> errores = new();
                if (calculateResult.Code == -1)
                {
                    return this.Json(new
                    {
                        response = new ApiResponse<NraViewModel>
                        {
                            Code = -1,
                            Description = "Error en validaciones NRA " + calculateResult.Description
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

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
                if (viewModel.ClaveIdioma.ToUpper() == "ES")
                {
                    if (resultReport)
                    {
                        viewModel.Resultado = "Aceptado";
                        viewModel.IsReportAproved = true;
                    }
                    else
                    {
                        viewModel.Resultado = "Rechazado";
                    }
                }
                else
                {
                    if (resultReport)
                    {
                        viewModel.Resultado = "Accepted";
                        viewModel.IsReportAproved = true;
                    }
                    else
                    {
                        viewModel.Resultado = "Rejected";
                    }
                }

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Amb+Trans")).Celda);
                var ambtrans = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AmbienteProm")).Celda);
                var ambprom = viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]]?.Value?.ToString();


                if (string.IsNullOrEmpty(ambtrans) && string.IsNullOrEmpty(ambprom))
                {
                    _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Amb+Trans")).Celda);
                    viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.NRATestsDetailsRuis.AmbTrans.ToString();

                    _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AmbienteProm")).Celda);
                    viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.NRATestsDetailsRuis.AverageAmb.ToString();
                }


                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("FactorK")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.KFactor.ToString("F1");
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Format = "0.0";

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("NivelPresionProm")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.AvgSoundPressureLevel.ToString("F0");

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("PotenciaRuido")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.SoundPowerLevel.ToString("F0");

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AreaS")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.Surface.ToString("F1");

                _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado")).Celda);
                viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = viewModel.Resultado;

                if (viewModel.ActivarSegundoHeader)
                {
                    _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("Resultado") && c.Seccion == 2).Celda);
                    viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = viewModel.Resultado;

                    _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("AreaS") && c.Seccion == 2).Celda);
                    viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.Surface.ToString("F1");

                    _positionWB = this.GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("FactorK") && c.Seccion == 2).Celda);
                    viewModel.Workbook.Sheets[0].Rows[_positionWB[0]].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.KFactor.ToString("F1");
                }

                viewModel.NRAPruebas = salida;
                viewModel.NRAResults = calculateResult.Structure;

                viewModel.NRAPruebas.Data.Guaranteed = decimal.Parse(viewModel.SettingsNRA.Warranty);
                viewModel.NRAPruebas.Data.FactorCoreccion = viewModel.NRAPruebas.Data.FactorCoreccion;
                viewModel.NRAPruebas.Data.UmLength = "m";
                viewModel.NRAPruebas.Data.UmSurface = "m2";
                viewModel.NRAPruebas.Data.UmHeight = "m";


                int seccion = 1;
                int i = 0;
                bool flag = false;
                int valorSeccionComparar = viewModel.CantidadColumnas == 10 ? 30 : 35;
                int breakComparasion = viewModel.CantidadColumnas == 10 ? 60 : 70;
                int seccion1 = viewModel.CantidadColumnas == 4 ? 1 : 3;
                int seccion2 = viewModel.CantidadColumnas == 4 ? 2 : 4;

                if (viewModel.Pruebas == "RUI" && viewModel.Altura == "1/2")
                {
                    object valor;
                    for (int o = 0; o < calculateResult.Structure.NRATestsGeneral.Data.NRATestsDetailsRuis.MatrixNRA13Tests.Count; o++)
                    {
                        if (seccion <= valorSeccionComparar)
                        {

                            _positionWB = GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion1).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.NRATestsDetailsRuis.MatrixNRA13Tests[o].DbaCor;
                        }

                        if (seccion > valorSeccionComparar)
                        {
                            if (!flag)
                            {
                                i = 0;
                                flag = true;
                            }

                            _positionWB = GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion2).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.NRATestsDetailsRuis.MatrixNRA13Tests[o].DbaCor;
                        }

                        if (seccion == viewModel.CantidadMaximadeMediciones)
                        {
                            break;
                        }

                        if (seccion > breakComparasion)
                        {
                            break;
                        }

                        seccion++;
                        i++;
                    }
                }

                if (viewModel.Pruebas == "RUI" && viewModel.Altura == "1/3")
                {
                    for (int o = 0; o < calculateResult.Structure.NRATestsGeneral.Data.NRATestsDetailsRuis.MatrixNRA13Tests.Count; o++)
                    {
                        if (seccion <= valorSeccionComparar)
                        {


                            _positionWB = GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion1).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.NRATestsDetailsRuis.MatrixNRA13Tests[o].DbaCor;

                            _positionWB = GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23Corr") && c.Seccion == seccion1).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.NRATestsDetailsRuis.MatrixNRA23Tests[o].DbaCor;
                        }

                        if (seccion > valorSeccionComparar)
                        {
                            if (!flag)
                            {
                                i = 0;
                                flag = true;
                            }

                            _positionWB = GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA13Corr") && c.Seccion == seccion2).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.NRATestsDetailsRuis.MatrixNRA13Tests[o].DbaCor;

                            _positionWB = GetRowColOfWorbook(viewModel.SettingsNRA.ConfigurationReports.FirstOrDefault(c => c.Dato.Equals("dBA23Corr") && c.Seccion == seccion2).Celda);
                            workbook.Sheets[0].Rows[_positionWB[0] + i].Cells[_positionWB[1]].Value = calculateResult.Structure.NRATestsGeneral.Data.NRATestsDetailsRuis.MatrixNRA23Tests[o].DbaCor;

                        }

                        if (seccion == viewModel.CantidadMaximadeMediciones)
                        {
                            break;
                        }

                        if (seccion > breakComparasion)
                        {
                            break;
                        }

                        seccion++;
                        i++;
                    }
                }

                return this.Json(new
                {
                    response = new ApiResponse<NraViewModel>
                    {
                        Code = 1,
                        Description = allErrosPrepare,
                        Structure = viewModel
                    }
                });

            }
            catch (Exception ex)
            {
                return this.Json(new
                {
                    response = new ApiResponse<NraViewModel>
                    {
                        Code = -1,
                        Description = ex.Message,
                        Structure = viewModel
                    }
                });
            }
        }
        public async Task<IActionResult> GetPDFReport(long code, string typeReport, string reportName)
        {

            var numFilas = reportName.Split('-').LastOrDefault().Split("_")[0];
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

        public IActionResult Error() => this.View();

        #region PrivateMethods
        public async Task PrepareForm()
        {


            //var a = await this._gatewayClientService.GetPositions("G4135-01");

            IEnumerable<GeneralPropertiesDTO> origingeneralProperties = new List<GeneralPropertiesDTO>() { new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." } }.AsEnumerable();

            // Tipos de prueba

            ApiResponse<IEnumerable<TestsDTO>> reportResult = await this._testClientService.GetTest(ReportType.NRA.ToString(), "-1");
            IEnumerable<GeneralPropertiesDTO> reportsGP = reportResult.Structure.Select(item => new GeneralPropertiesDTO
            {
                Clave = item.ClavePrueba,
                Descripcion = item.Descripcion
            });

            IEnumerable<GeneralPropertiesDTO> laboratorio = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO { Clave = "Potencia", Descripcion = "Potencia" } ,
                new GeneralPropertiesDTO { Clave = "Mediana", Descripcion = "Mediana" },
                new GeneralPropertiesDTO { Clave = "EHV", Descripcion = "EHV" }
            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> alimentacion = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO { Clave = "Tensión", Descripcion = "Tensión" } ,
                new GeneralPropertiesDTO { Clave = "Corriente", Descripcion = "Corriente" },
                new GeneralPropertiesDTO { Clave = "Pérdidas", Descripcion = "Pérdidas" }
            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> selectCarga = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO { Clave = "4 MEDICIONES ANTES Y DESPUÉS DE AMBIENTE", Descripcion = "4 mediciones antes y después de ambiente" } ,
                new GeneralPropertiesDTO { Clave = "10 MEDICIONES ANTES Y DESPUÉS DE AMBIENTE", Descripcion = "10 mediciones antes y después de ambiente" },
            }.AsEnumerable();


            IEnumerable<GeneralPropertiesDTO> selectAltura = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO { Clave = "1/2", Descripcion = "1/2" } ,
                new GeneralPropertiesDTO { Clave = "1/3", Descripcion = "1/3 - 2/3" },
            }.AsEnumerable();

            this.ViewBag.TestItems = new SelectList(origingeneralProperties.Concat(reportsGP), "Clave", "Descripcion");
            this.ViewBag.Laboratorio = new SelectList(laboratorio, "Clave", "Descripcion");
            this.ViewBag.Norma = new SelectList(origingeneralProperties, "Clave", "Descripcion");
            this.ViewBag.Alimentacion = new SelectList(alimentacion, "Clave", "Descripcion");
            this.ViewBag.Enfriamiento = new SelectList(origingeneralProperties, "Clave", "Descripcion");
            this.ViewBag.SelectCargaInformacion = new SelectList(selectCarga, "Clave", "Descripcion");
            this.ViewBag.Alturas = new SelectList(selectAltura, "Clave", "Descripcion");


            // Idiomas
            IEnumerable<GeneralPropertiesDTO> generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetLanguageEquivalents, (int)MicroservicesEnum.splmasters));
            this.ViewBag.ClaveIdiomaItems = new SelectList(generalProperties, "Clave", "Descripcion");

            generalProperties = origingeneralProperties.Concat(await this._masterHttpClientService.GetMasterByMethodKey(MethodMasterKeyName.GetRulesEquivalents, (int)MicroservicesEnum.splmasters));
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

        private List<MatrixThree1323HDTO> LlenarLista(int cantidad)
        {
            List<MatrixThree1323HDTO> lista = new List<MatrixThree1323HDTO>();
            int posi = 1;
            for (int i = 0; i < cantidad; i++)
            {
                lista.Add(new MatrixThree1323HDTO { Position = posi });
                posi++;
            }

            return lista;
        }

        private List<MatrixThreeDTO> LlenarLista2(int cantidad)
        {
            List<MatrixThreeDTO> lista = new List<MatrixThreeDTO>();
            int posi = 1;
            for (int i = 0; i < cantidad; i++)
            {
                lista.Add(new MatrixThreeDTO { Position = posi });
                posi++;
            }

            return lista;
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

        #endregion
    }
}

