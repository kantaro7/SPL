namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Kendo.Mvc.Extensions;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Identity.Web;

    using Newtonsoft.Json;

    using Serilog;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.ViewModels;

    using Telerik.Windows.Documents.Spreadsheet.FormatProviders;

    /// <summary>
    /// Relación de Transformación
    /// </summary>
    public class OnafController : Controller
    {
        private readonly IMasterHttpClientService _masterHttpClientService;
        private readonly IReportClientService _reportClientService;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ITestClientService _testClientService;
        private readonly IGatewayClientService _gatewayClientService;
        private readonly IArfService _arfService;
        private readonly ICorrectionFactorService _correctionFactorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IInformationOctavesService _octavas;
        private readonly ILogger _logger;
        private readonly IProfileSecurityService _profileClientService;
        public OnafController(
            IMasterHttpClientService masterHttpClientService,
            IReportClientService reportClientService,
            IArtifactClientService artifactClientService,
            ITestClientService testClientService,
            IGatewayClientService gatewayClientService,
            IArfService arfService,
            ICorrectionFactorService correctionFactorService,
            IWebHostEnvironment hostEnvironment,
            IInformationOctavesService octavas, ILogger logger,
            IProfileSecurityService profileClientService
            )
        {
            _masterHttpClientService = masterHttpClientService;
            _reportClientService = reportClientService;
            _artifactClientService = artifactClientService;
            _testClientService = testClientService;
            _gatewayClientService = gatewayClientService;
            _arfService = arfService;
            _correctionFactorService = correctionFactorService;
            _hostEnvironment = hostEnvironment;
            _octavas = octavas;
            _logger = logger;
            _profileClientService = profileClientService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;


                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.InformacióndeOctavas)))
                {


                    await PrepareForm();
                    return View("OnafInformation", new OnafViewModel());
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

        public async Task<IActionResult> LoadPartial()
        {
            await PrepareForm();
            return View("Index", new OnafViewModel());
        }


        public async Task<IActionResult> GetInfo(string noSerie)
        {
            _logger.Information("/Accesando a controlador Onaf/");
            try
            {
                OnafViewModel data = new OnafViewModel();
                string noSerieSimple = string.Empty;

                if (!string.IsNullOrEmpty(noSerie))
                {
                    noSerieSimple = noSerie.Split("-")[0];
                }
                else
                {
                    return Json(new
                    {
                        response = new ApiResponse<OnafViewModel>
                        {
                            Code = -1,
                            Description = "No. Serie inválido.",
                            Structure = null
                        }
                    });
                }
                IEnumerable<GeneralPropertiesDTO> tipoInformacion = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO { Clave = "ANT", Descripcion = "Antes" } ,
                new GeneralPropertiesDTO { Clave = "DES", Descripcion = "Después" } ,
            }.AsEnumerable();

                InformationArtifactDTO artifactDesing = await _artifactClientService.GetArtifact(noSerieSimple);
                var tipoEnfriamiento = artifactDesing.CharacteristicsArtifact.Select(x => x.CoolingType).Distinct().ToList().Select(x => new GeneralPropertiesDTO { Descripcion = x, Clave = x }).Distinct().ToList();
                data.TiposInformacion = new SelectList(tipoInformacion.Concat(tipoEnfriamiento), "Clave", "Descripcion");
                //var a = await this._octavas.GetInfoOctavas(noSerie, null, null);
                return Json(new
                {
                    response = new ApiResponse<OnafViewModel>
                    {
                        Code = 1,
                        Description = "Información Procesada",
                        Structure = data

                    }
                });

            }
            catch (Exception ex)
            {
                var lineNumber = 0;
                const string lineSearch = ":line ";
                var index = ex.StackTrace.LastIndexOf(lineSearch);
                if (index != -1)
                {
                    var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                    if (int.TryParse(lineNumberText, out lineNumber))
                    {
                        _logger.Information("ERROR EN LINEA: " + lineNumber);
                    }
                }

                return Json(new
                {
                    response = new ApiResponse<OnafViewModel>
                    {
                        Code = -1,
                        Description = string.Empty,
                        Structure = null
                    }
                });
            }


        }

        public async Task<IActionResult> GetAlturas(string noSerie, DateTime fecha, string tipo)
        {
            try
            {
                var result = await _octavas.GetInfoOctavas(noSerie, tipo, fecha.ToString("yyyy-MM-dd"));

                if (result.Code == -1)
                {
                    return Json(new
                    {
                        response = new ApiResponse<OnafViewModel>
                        {
                            Code = -1,
                            Description = result.Description,
                            Structure = null

                        }
                    });
                }
                else
                {
                    if (result.Structure.Count() == 0)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<OnafViewModel>
                            {
                                Code = -1,
                                Description = "No se encontraron resultados para los filtros seleccionados ",
                                Structure = null
                            }
                        });
                    }
                }

                var alturas = result.Structure.Select(x => x.Altura).Distinct().ToList();

                return Json(new
                {
                    response = new ApiResponse<OnafViewModel>
                    {
                        Code = 1,
                        Description = "Información Procesada",
                        Structure = new OnafViewModel { ListaAlturas = alturas }

                    }
                });

            }
            catch (Exception ex)
            {
                var lineNumber = 0;
                const string lineSearch = ":line ";
                var index = ex.StackTrace.LastIndexOf(lineSearch);
                if (index != -1)
                {
                    var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                    if (int.TryParse(lineNumberText, out lineNumber))
                    {
                        _logger.Information("ERROR EN LINEA: " + lineNumber);
                    }
                }

                return Json(new
                {
                    response = new ApiResponse<OnafViewModel>
                    {
                        Code = -1,
                        Description = string.Empty,
                        Structure = null
                    }
                });
            }


        }


        public async Task<IActionResult> GetConfigurationFiles(int pIdModule)
        {
            ApiResponse<IEnumerable<FileWeightDTO>> getConfigModulResponse = await _masterHttpClientService.GetConfigurationFiles(pIdModule);
            return Json(new
            {
                response = getConfigModulResponse
            });
        }

        public async Task<IActionResult> GetInformationOctaves(string noSerie, DateTime fecha, string altura, string tipo)
        {
            try
            {
                OnafViewModel data = new OnafViewModel();
                _logger.Information("Consultand metodo información de octavas en metodo GetInformationOctaves");
                var result = await _octavas.GetInfoOctavas(noSerie, tipo, fecha.ToString("yyyy-MM-dd"));
                _logger.Information("Resultado devuelto por metodo " + JsonConvert.SerializeObject(result));
                if (result.Structure.Count == 0)
                {
                    return Json(new
                    {
                        response = new ApiResponse<OnafViewModel>
                        {
                            Code = -1,
                            Description = "No se ha encontrado información de octavas para los filtros seleccionados",
                            Structure = null
                        }
                    });
                }
                else
                {
                    var dataResult = result.Structure.Where(x => x.Altura.ToUpper() == altura.ToUpper()).ToList();

                    if (dataResult.Count() == 0)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<OnafViewModel>
                            {
                                Code = -1,
                                Description = "No se ha encontrado información de octavas para los filtros seleccionados",
                                Structure = null
                            }
                        });
                    }
                    else
                    {
                        OnafViewModel response = new OnafViewModel
                        {
                            Lista = dataResult.OrderByDescending(x => x.Hora).ToList()
                        };

                        return Json(new
                        {
                            response = new ApiResponse<OnafViewModel>
                            {
                                Code = 1,
                                Description = "",
                                Structure = response
                            }
                        });
                    }
                }



            }
            catch (Exception e)
            {
                return Json(new
                {
                    response = new ApiResponse<OnafViewModel>
                    {
                        Code = -1,
                        Description = string.Empty,
                        Structure = null
                    }
                });
            }


        }

        public async Task<IActionResult> Save([FromForm] OnafViewModel viewModel)
        {
            try
            {


                _logger.Information("/Accesando a controlador Onaf metodo SAVE/");
                List<InformationOctavesDTO> lista = new List<InformationOctavesDTO>();
                List<InformationOctavesDTO> alturasFaltantes = new List<InformationOctavesDTO>();

                if (viewModel.IsFromFile)
                {
                    string noSerieSimple = string.Empty;
                    List<string> tipoEnfriamientos = new List<string>();

                    if (!string.IsNullOrEmpty(viewModel.NoSerie))
                    {
                        noSerieSimple = viewModel.NoSerie.Split("-")[0];
                        InformationArtifactDTO artifactDesing = await _artifactClientService.GetArtifact(noSerieSimple);
                        tipoEnfriamientos = artifactDesing.CharacteristicsArtifact
                            .Select(x => x.CoolingType.ToUpper())
                            .Distinct()
                            .ToList();

                        tipoEnfriamientos.Insert(0, "ANT");
                        tipoEnfriamientos.Add("DES");
                    }
                    else
                    {
                        return Json(new
                        {
                            response = new ApiResponse<OnafViewModel>
                            {
                                Code = -1,
                                Description = "No. Serie inválido.",
                                Structure = null
                            }
                        });
                    }

                    if (tipoEnfriamientos.Count == 0)
                    {
                        return Json(new
                        {
                            response = new ApiResponse<OnafViewModel>
                            {
                                Code = -1,
                                Description = $"La serie {noSerieSimple} no tiene configurado tipos de enfriamiento.",
                                Structure = null
                            }
                        });
                    }

                    byte[] imageArray;
                    string imageCodeBase64 = string.Empty;

                    if (viewModel.File != null)
                    {
                        _logger.Information("procesando archivo");
                        using MemoryStream memoryStream = new();
                        await viewModel.File.CopyToAsync(memoryStream);
                        imageArray = memoryStream.ToArray();

                        IEnumerable<FileWeightDTO> fileConfigModule = new List<FileWeightDTO>();

                        ApiResponse<IEnumerable<FileWeightDTO>> getConfigModulResponse = await _masterHttpClientService.GetConfigurationFiles(3);

                        if (getConfigModulResponse.Code.Equals(-1))
                        {
                            return Json(new
                            {
                                response = getConfigModulResponse
                            });
                        }
                        else
                        {
                            fileConfigModule = getConfigModulResponse.Structure;
                        }

                        #region Validate Document

                        string extension = Path.GetExtension(viewModel.File.FileName);

                        FileWeightDTO configModule = fileConfigModule.FirstOrDefault(c => c.ExtensionArchivoNavigation.Extension.Equals(extension));

                        if (configModule is null)
                        {
                            return Json(new
                            {
                                response = new ApiResponse<string>
                                {
                                    Code = -1,
                                    Description = "Archivo no permitido."
                                }
                            });
                        }

                        int size = int.Parse(configModule.MaximoPeso);

                        if (imageArray.Length > size * 1024 * 1024)
                        {
                            return Json(new
                            {
                                response = new ApiResponse<string>
                                {
                                    Code = -1,
                                    Description = string.Format("Documento muy grande, debe ser menor que {0}mb.", size * 1024 * 1024)
                                }
                            });
                        }

                        Stream output = new MemoryStream();
                        if (extension == ".xls")
                        {
                            Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook3;
                            IWorkbookFormatProvider formatProvider3 = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.Xls.XlsFormatProvider();
                            Stream stream2 = new MemoryStream(imageArray);
                            workbook3 = formatProvider3.Import(stream2);

                            Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider4 = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();

                            formatProvider4.Export(workbook3, output);

                        }
                        else
                        {
                            IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();
                            output = new MemoryStream(imageArray);
                        }

                        var workbook2 = Telerik.Web.Spreadsheet.Workbook.Load(output, "xlsx");

                        int indexColumn = 0;
                        int indexRow = 0;
                        string[] altura = new string[] { };
                        DateTime fecha;
                        string hora;
                        DateTime basedate = new(1899, 12, 30);
                        DateTime firstDate = new DateTime();
                        bool encontrado = false;

                        foreach (var sheet in workbook2.Sheets)
                        {
                            string tipoInformacion = sheet.Name.ToUpper();

                            foreach (var row in sheet.Rows)//busqueda de Signal
                            {
                                foreach (var col in row.Cells)
                                {
                                    if (col.Value?.ToString()?.Trim() == "Signal:")
                                    {
                                        encontrado = true;
                                        indexColumn = (int)col.Index;
                                        indexRow = (int)row.Index;
                                        altura = row.Cells[indexColumn + 1]?.Value?.ToString()?.Split(' ');
                                        if (altura.Count() == 2)
                                        {
                                            lista.Add(new InformationOctavesDTO { Altura = altura[1].Trim(), NoSerie = viewModel.NoSerie, Creadopor = "SYS", Fechacreacion = DateTime.Now, TipoInfo = tipoInformacion });
                                        }
                                        else
                                        {
                                            return Json(new
                                            {
                                                response = new ApiResponse<OnafViewModel>
                                                {
                                                    Code = -1,
                                                    Description = "Error en información de Signal para la fila: " + indexRow + " y columna: " + indexColumn,
                                                    Structure = new OnafViewModel()
                                                }
                                            });
                                        }

                                    }
                                }

                                if (encontrado)
                                {
                                    break;
                                }
                            }

                            indexColumn = 0;
                            indexRow = 0;
                            altura = new string[] { };
                            encontrado = false;
                        }
                        bool fisrtRow = true;

                        _logger.Information("Label Signal del excel validado");
                        foreach (var sheet in workbook2.Sheets)
                        {
                            var listaTipo = lista.Where(e => e.TipoInfo == sheet.Name.ToUpper()).ToList();

                            encontrado = false;
                            var cont = 0;

                            foreach (var row in sheet.Rows)//busqueda de DAte
                            {
                                foreach (var col in row.Cells.Select(x => new { x.Index, x.Value }))
                                {
                                    if (col.Value?.ToString()?.Trim() == "Date:")
                                    {
                                        encontrado = true;
                                        var ii = col.Index;
                                        indexColumn = (int)col.Index;
                                        indexRow = (int)row.Index;
                                        fecha = basedate.AddDays(int.Parse(row.Cells[indexColumn + 2]?.Value?.ToString()));
                                        hora = sheet.Rows[indexRow + 1].Cells[indexColumn + 2]?.Value.ToString();
                                        int posi = hora.LastIndexOf(":", System.StringComparison.Ordinal);
                                        hora = hora.Remove(posi, 1);
                                        hora = hora.Insert(posi, ".");

                                        if (fisrtRow)
                                        {
                                            firstDate = fecha.Date;
                                            fisrtRow = false;
                                        }

                                        if (firstDate.Date != fecha.Date)
                                        {
                                            return Json(new
                                            {
                                                response = new ApiResponse<OnafViewModel>
                                                {
                                                    Structure = null,
                                                    Code = -1,
                                                    Description = "Se encontraron diferentes fechas en el archivo, " + firstDate.Date.ToString("yyyy/MM/dd") + " - " + fecha.ToString("yyyy/MM/dd")

                                                }
                                            });
                                        }

                                        listaTipo.ElementAt(cont).Hora = hora.ToString();
                                        listaTipo.ElementAt(cont).FechaDatos = fecha.Date;

                                        System.Diagnostics.Debug.WriteLine($"{col} row = {indexRow}; col = {indexColumn};");
                                        listaTipo.ElementAt(cont).D16 = GetCellValue(sheet, indexRow + 4, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D20 = GetCellValue(sheet, indexRow + 5, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D25 = GetCellValue(sheet, indexRow + 6, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D315 = GetCellValue(sheet, indexRow + 7, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D40 = GetCellValue(sheet, indexRow + 8, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D50 = GetCellValue(sheet, indexRow + 9, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D63 = GetCellValue(sheet, indexRow + 10, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D80 = GetCellValue(sheet, indexRow + 11, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D100 = GetCellValue(sheet, indexRow + 12, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D125 = GetCellValue(sheet, indexRow + 13, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D160 = GetCellValue(sheet, indexRow + 14, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D200 = GetCellValue(sheet, indexRow + 15, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D250 = GetCellValue(sheet, indexRow + 16, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D3151 = GetCellValue(sheet, indexRow + 17, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D400 = GetCellValue(sheet, indexRow + 18, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D500 = GetCellValue(sheet, indexRow + 19, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D630 = GetCellValue(sheet, indexRow + 20, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D800 = GetCellValue(sheet, indexRow + 21, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D1000 = GetCellValue(sheet, indexRow + 22, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D1250 = GetCellValue(sheet, indexRow + 23, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D1600 = GetCellValue(sheet, indexRow + 24, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D2000 = GetCellValue(sheet, indexRow + 25, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D2500 = GetCellValue(sheet, indexRow + 26, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D3150 = GetCellValue(sheet, indexRow + 27, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D4000 = GetCellValue(sheet, indexRow + 28, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D5000 = GetCellValue(sheet, indexRow + 29, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D6300 = GetCellValue(sheet, indexRow + 30, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D8000 = GetCellValue(sheet, indexRow + 31, indexColumn + 2);
                                        listaTipo.ElementAt(cont).D10000 = GetCellValue(sheet, indexRow + 32, indexColumn + 2);
                                        listaTipo.ElementAt(cont).Creadopor = User.Identity.Name;

                                        cont++;
                                    }
                                }

                                if (encontrado)
                                {
                                    break;
                                }
                            }
                            indexColumn = 0;
                            indexRow = 0;
                            hora = string.Empty;
                            fecha = new DateTime();
                            encontrado = false;
                        }

                        _logger.Information("Label Date del excel validado");

                        foreach (var item in lista)
                        {
                            var newCoolingType = tipoEnfriamientos.FirstOrDefault(type => item.TipoInfo.StartsWith(type));

                            if (!string.IsNullOrEmpty(newCoolingType))
                            {
                                item.TipoInfo = newCoolingType;
                            }
                        }

                        lista.RemoveAll(o => !tipoEnfriamientos.Contains(o.TipoInfo));

                        /*Cuando las mediciones son en tercios entonces
                         Los primeros valores son 1/3 y la siguiente columna es 2/3 y así sucesivamente
                         La hora no necesariamente será igual para cada par
                        */
                        var thirds = lista
                            .Where(e => e.Altura == "1/3" || e.Altura == "2/3")
                            .GroupBy(e => e.TipoInfo)
                            .ToArray();

                        foreach (var group in thirds)
                        {
                            int groupCount = group.Count();

                            foreach (InformationOctavesDTO item in group)
                            {
                                int octaveIndex = group.IndexOf(item);

                                bool isLastOctave = octaveIndex == groupCount - 1;

                                bool isEven = octaveIndex % 2 == 0;

                                item.Altura = isEven
                                    ? isLastOctave ? "Ignore" : "1/3"
                                    : "2/3";
                            }
                        }

                        thirds = null;

                        /*Se eliminan valores de tercios que no tienen par*/
                        lista.RemoveAll(e => e.Altura == "Ignore");

                        viewModel.Lista = new List<InformationOctavesDTO>();

                        if (lista.Count > 0 && !viewModel.AcceptaReemplazarData)
                        {
                            List<string> messages = new();

                            foreach (var typeGroup in lista.GroupBy(e => e.TipoInfo))
                            {
                                var tipoAbuscar = typeGroup.Key;

                                var fechaBuscar = typeGroup.FirstOrDefault()?.FechaDatos.ToString("yyyy-MM-dd");
                                var serieBuscar = typeGroup.FirstOrDefault()?.NoSerie;
                                var resultBuscar = await _octavas.GetInfoOctavas(serieBuscar, tipoAbuscar, fechaBuscar);
                                _logger.Information("Resultado del metodo GetInfoOctavas " + serieBuscar + " " + tipoAbuscar + " " + fechaBuscar + " => " + JsonConvert.SerializeObject(resultBuscar));
                                //1/3 =>  2/3
                                //1/2 
                                var alturasArchivo = typeGroup.Select(e => e.Altura).Distinct().ToArray();

                                viewModel.Lista = resultBuscar.Structure.Where(e => alturasArchivo.Contains(e.Altura)).ToList();

                                if (viewModel.Lista.Count > 0)
                                {
                                    string[] alturas = viewModel.Lista.Select(x => x.Altura).Distinct().ToArray();
                                    string union = string.Join(',', alturas);
                                    messages.Add($"Tipo:{tipoAbuscar} Fecha:{fechaBuscar} Altura(s): {union}");
                                }
                            }
                            if (messages.Count > 0)
                            {
                                System.Text.StringBuilder messageBuilder = new("Ya existe información en la fecha de datos.\n");
                                messageBuilder.AppendLine("¿Desea reemplazarla? se eliminara lo que existe y se registrará lo del archivo.");

                                foreach (var message in messages)
                                {
                                    messageBuilder.AppendLine(message);
                                }

                                return Json(new
                                {
                                    response = new ApiResponse<OnafViewModel>
                                    {
                                        Structure = viewModel,
                                        Code = 1,
                                        Description = $"<pre>{messageBuilder.ToString()}</pre>"
                                    }
                                }); ;
                            }
                        }

                        #endregion
                    }
                    else
                    {

                        return Json(new
                        {
                            response = new ApiResponse<OnafViewModel>
                            {
                                Structure = viewModel,
                                Code = -1,
                                Description = "Error al leer el archivo"

                            }
                        });
                    }
                }
                else
                {
                    _logger.Information("Es guardado desde la tabla ");

                    /* if(viewModel.Altura =="1/3" || viewModel.Altura == "2/3")
                     {
                         alturasFaltantes = viewModel.Lista.Where(x => x.Altura != viewModel.Altura).ToList();
                     }*/

                    lista = JsonConvert.DeserializeObject<List<InformationOctavesDTO>>(viewModel.DataSource);
                    lista.AddRange(alturasFaltantes);
                    foreach (InformationOctavesDTO element in lista)
                    {
                        element.Modificadopor = User.Identity.Name;
                        element.Fechamodificacion = DateTime.Now;
                    }
                }

                if (lista.Count > 0)
                {
                    var checkdata = lista.GroupBy(x => new { x.NoSerie, x.Altura, x.TipoInfo, x.Hora })
                        .Select(f => new { f.Key.Altura, f.Key.NoSerie, f.Key.Hora, f.Key.TipoInfo, Count = f.Count() }).Where(g => g.Count > 1).Select(u => u.Hora).FirstOrDefault();

                    var p = JsonConvert.SerializeObject(lista);
                    _logger.Information("Información a guardar " + p);
                    lista.ForEach(x => x.Fechacreacion = DateTime.Now);
                    ApiResponse<long> result = await _octavas.SaveOctaves(lista, viewModel.IsFromFile);
                    _logger.Information("Resultado del API " + JsonConvert.SerializeObject(result));

                    string mensaje = "Se cargaron para la fecha {0} la cantidad de mediciones {1} {2} ";
                    string alt = lista.Select(x => x.Altura).Distinct().Count() == 1 ? "para la altura 1/2" : "para las alturas 1/3 y 2/3";
                    mensaje = string.Format(mensaje, lista.FirstOrDefault().FechaDatos.ToString("yyyy/MM/dd"), lista.Count, alt);

                    if (!string.IsNullOrEmpty(checkdata))
                    {
                        mensaje += $"Se encontró mas de un registro para la hora: {checkdata} ";
                    }

                    return Json(new
                    {
                        response = new ApiResponse<OnafViewModel>
                        {
                            Structure = viewModel,
                            Code = result.Code,
                            Description = result.Code == 1 ? mensaje : result.Description

                        }
                    });
                }
                else
                {
                    return Json(new
                    {
                        response = new ApiResponse<OnafViewModel>
                        {
                            Structure = viewModel,
                            Code = -1,
                            Description = "El archivo no contiene información para guardar. En el caso de las alturas en tercios requieren las medidas de 1/3 y 2/3."
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                var lineNumber = 0;
                const string lineSearch = ":line ";
                var index = ex.StackTrace.LastIndexOf(lineSearch);
                if (index != -1)
                {
                    var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                    if (int.TryParse(lineNumberText, out lineNumber))
                    {
                        _logger.Information("ERROR EN LINEA: " + lineNumber);
                    }
                }

                return Json(new
                {
                    response = new ApiResponse<OnafViewModel>
                    {
                        Code = -1,
                        Description = $"Hubo un problema al procesar el archivo.\n{ex.Message}",
                        Structure = null
                    }
                });
            }
        }

        private static decimal GetCellValue(Telerik.Web.Spreadsheet.Worksheet sheet, int rowIndex, int columnIndex)
        {
            string cellName = "";
            string sheetName = sheet.Name;

            try
            {
                cellName = Telerik.Windows.Documents.Spreadsheet.Utilities.NameConverter.ConvertCellIndexToName(rowIndex, columnIndex);
            }
            finally
            {
                if ((rowIndex > (sheet.Rows.Count - 1)) || (columnIndex > (sheet.Rows[rowIndex].Cells.Count - 1)))
                {
                    throw new Exception(string.IsNullOrEmpty(cellName) ? $"No existe el renglón {rowIndex} y columna {columnIndex} en la pestaña {sheetName}."
                        : $"No existe la celda {cellName} en la pestaña {sheetName}.");
                }
            }

            try
            {
                return decimal.Parse(sheet.Rows[rowIndex].Cells[columnIndex]?.Value?.ToString());
            }
            catch (Exception)
            {
                throw new Exception($"No se puede leer el valor de la celda {cellName} en la pestaña {sheetName}.");
            }
        }

        public IActionResult Error() => View();

        #region PrivateMethods
        public async Task PrepareForm()
        {
            /// var a = await this._gatewayClientService.GetPositions("G4135-01");


            IEnumerable<GeneralPropertiesDTO> tipoInformacion = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." },
                new GeneralPropertiesDTO { Clave = "ANT", Descripcion = "Antes" } ,
                new GeneralPropertiesDTO { Clave = "DES", Descripcion = "Después" } ,
            }.AsEnumerable();

            IEnumerable<GeneralPropertiesDTO> altura = new List<GeneralPropertiesDTO>() {
                new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." },
            }.AsEnumerable();



            // Tipos de prueba



            ViewBag.TipoInformacion = new SelectList(tipoInformacion, "Clave", "Descripcion");
            ViewBag.Altura = new SelectList(altura, "Clave", "Descripcion");
            // Idiomas
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
        #endregion
    }

}

