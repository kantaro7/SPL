// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gateway.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using Gateway.Api.Common;
    using Gateway.Api.Dtos;
    using Gateway.Api.Dtos.ArtifactDesing;
    using Gateway.Api.Dtos.Nozzle;
    using Gateway.Api.Dtos.Reports.CGD;
    using Gateway.Api.Dtos.Reports.TAP;
    using Gateway.Api.Dtos.Reports.TIN;
    using Gateway.Api.HttpServices;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using SPL.Domain;

    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        #region Fields
        private readonly IReportHttpClientService _reportHttpClientService;
        private readonly ILogger<ReportsController> _logger;

        #endregion

        public ReportsController(ILogger<ReportsController> logger, IReportHttpClientService reportHttpClientService)
        {
            _logger = logger;
            _reportHttpClientService = reportHttpClientService;

        }
        /// <summary>
        /// Funcion que obtiene la configuracion de RAD
        /// </summary>
        /// <param name="typeReport">Clave tipo de Reporte.</param>
        /// <param name="nroSerie">Numero de serie del artefacto</param>
        /// <param name="keyTest">Clave de tipo de prueba</param>
        /// <param name="typeUnit">Tipo unidad</param>
        /// <param name="thirdWinding">Tercer Devanado</param>
        /// <param name="keyLanguage">Clave lenguaje</param>
        /// <returns>Toda la configuracion necesaria para el reporte</returns>
        [HttpGet("getConfigurationReportRAD")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayRADReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportRAD([FromQuery] string typeReport, string nroSerie, string keyTest, string typeUnit, string thirdWinding
             , string keyLanguage, string tokenSesion)
        {

            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            SettingsToDisplayRADReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(nroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayRADReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(keyLanguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayRADReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            if (string.IsNullOrEmpty(keyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayRADReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(typeUnit))
                return new JsonResult(new ApiResponse<SettingsToDisplayRADReportsDto>((int)ResponsesID.fallido, "Tipo de unidad es requerido", null));

            if (string.IsNullOrEmpty(thirdWinding))
                thirdWinding = "SEL";

            string[] newnoserie = { "" };
            long numberColumns = 0;
            if (!string.IsNullOrEmpty(nroSerie))
                newnoserie = nroSerie.Split('-');
            try
            {

                Task<ApiResponse<List<ColumnTitleRADReportsDto>>> getTitleColumnsRAD() => _reportHttpClientService.GetTitleColumnsRAD(typeUnit, thirdWinding, keyLanguage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<long>> getNroTestNextRAD() => _reportHttpClientService.GetNroTestNextRAD(nroSerie, keyTest, typeUnit, thirdWinding, keyLanguage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports(typeReport, keyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));
                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate(typeReport, keyTest, keyLanguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<ColumnTitleRADReportsDto>>> Task1 = getTitleColumnsRAD();

                Task<ApiResponse<long>> Task3 = getNroTestNextRAD();

                Task<InformationArtifactDto> Task5 = getInformationGeneral(newnoserie[0]);

                List<Task> lote1 = new() { Task1, Task3, Task5 };

                await Task.WhenAny(lote1).Result;

                if (Task5.Result.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRADReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (Task5.Result.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRADReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (Task5.Result.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRADReportsDto>((int)ResponsesID.fallido, "“El número de serie no cuenta con información de características", null));

                bool valLongitud = Validations.validacion55Caracteres(nroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(nroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRADReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRADReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                numberColumns = Task1.Result.Structure.Count;

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task2 = getConfigurationReports();
                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                List<Task> lote2 = new() { Task2, Task4 };

                await Task.WhenAny(lote2).Result;

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task5.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task5.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (!obj.Mvaf1.ToString().Contains('.'))
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = !stringSplit[i].Contains('.') ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }

                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayRADReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task2.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task5.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = nroSerie }, NextTestNumber = Task3.Result.Structure, TitleOfColumns = Task1.Result.Structure };

                return new JsonResult(new ApiResponse<SettingsToDisplayRADReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportRAD));
                return new JsonResult(new ApiResponse<SettingsToDisplayRADReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("getConfigurationReportRDT")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayRDTReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportRDT([FromQuery] string typeReport, string nroSerie, string KeyTest, string dAngular, string rule, string lenguage, int conexion, string posAT, string posBT, string posTer, string tokenSesion)
        {

            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            SettingsToDisplayRDTReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(nroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(dAngular))
                return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Desplazamiento angular es requerido", null));

            if (string.IsNullOrEmpty(rule))
                return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Norma es requerido", null));

            if (string.IsNullOrEmpty(posAT))
                return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Posición Alta tensión es requerido", null));

            if (string.IsNullOrEmpty(posBT))
                return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Posición Baja tensión es requerido", null));

            if (string.IsNullOrEmpty(posTer))
                return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Posición Terciaria es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 0;
            if (!string.IsNullOrEmpty(nroSerie))
                newnoserie = nroSerie.Split('-');
            try
            {

                Task<ApiResponse<List<ColumnTitleRDTReportsDto>>> getTitleColumnsRDT() => _reportHttpClientService.GetTitleColumnsRDT(KeyTest, dAngular, rule, lenguage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<long>> getNroTestNextRDT() => _reportHttpClientService.GetNroTestNextRDT(nroSerie, KeyTest, dAngular, rule, lenguage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports(typeReport, KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));
                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate(typeReport, KeyTest, lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<ColumnTitleRDTReportsDto>>> Task1 = getTitleColumnsRDT();

                Task<ApiResponse<long>> Task3 = getNroTestNextRDT();

                Task<InformationArtifactDto> Task5 = getInformationGeneral(newnoserie[0]);

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(nroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task5, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(nroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(nroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (Task5.Result.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (Task5.Result.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (Task5.Result.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "“El número de serie no cuenta con información de características", null));

                if (Task6.Result.Structure == null || Task6.Result.Structure.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de las tensiones de la placa", null));

                if (Task5.Result.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                List<PlateTensionDto> listAt = Task6.Result.Structure.Where(x => x.TipoTension.ToUpper().Equals("AT")).ToList();
                List<PlateTensionDto> listBt = Task6.Result.Structure.Where(x => x.TipoTension.ToUpper().Equals("BT")).ToList();
                List<PlateTensionDto> listTer = Task6.Result.Structure.Where(x => x.TipoTension.ToUpper().Equals("TER")).ToList();

                List<string> positions = new();
                switch (KeyTest)
                {
                    case "ABT":
                        if (listAt.Count > 1 || listBt.Count > 1)
                        {
                            if (!posAT.Equals("Todas") && !posBT.Equals("Todas"))
                            {
                                return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Se debe seleccionar todas las posiciones en AT o BT", null));

                            }
                            else
                            {
                                if (posAT.Equals("Todas") && !posBT.Equals("Todas"))
                                {
                                    foreach (PlateTensionDto item in listAt)
                                    {
                                        positions.Add(item.Posicion);
                                    }
                                }
                                else
                                if (posBT.Equals("Todas") && !posAT.Equals("Todas"))
                                {
                                    foreach (PlateTensionDto item in listBt)
                                    {
                                        positions.Add(item.Posicion);
                                    }
                                }
                                else
                                {
                                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Solo puede seleccionar todas las posiciones en AT o BT pero no en ambas", null));

                                }
                            }
                        }
                        else
                        {
                            foreach (PlateTensionDto item in listAt)
                            {
                                positions.Add(item.Posicion);
                            }
                        }
                        break;
                    case "ATT":
                        if (listAt.Count > 1 || listTer.Count > 1)
                        {
                            if (!posAT.Equals("Todas") && !posTer.Equals("Todas"))
                            {
                                return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Se debe seleccionar todas las posiciones en AT o Ter", null));

                            }
                            else
                            {
                                if (posAT.Equals("Todas") && !posTer.Equals("Todas"))
                                {
                                    foreach (PlateTensionDto item in listAt)
                                    {
                                        positions.Add(item.Posicion);
                                    }
                                }
                                else
                            if (posTer.Equals("Todas") && !posAT.Equals("Todas"))
                                {
                                    foreach (PlateTensionDto item in listTer)
                                    {
                                        positions.Add(item.Posicion);
                                    }
                                }
                                else
                                {
                                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Solo puede seleccionar todas las posiciones en AT o Ter pero no en ambas", null));

                                }
                            }
                        }
                        else
                        {
                            foreach (PlateTensionDto item in listAt)
                            {
                                positions.Add(item.Posicion);
                            }
                        }
                        break;
                    case "BTT":
                        if (listBt.Count > 1 || listTer.Count > 1)
                        {
                            if (!posBT.Equals("Todas") && !posTer.Equals("Todas"))
                            {
                                return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Se debe seleccionar todas las posiciones en BT o Ter", null));

                            }
                            else
                            {
                                if (posBT.Equals("Todas") && !posTer.Equals("Todas"))
                                {
                                    foreach (PlateTensionDto item in listBt)
                                    {
                                        positions.Add(item.Posicion);
                                    }
                                }
                                else
                         if (posTer.Equals("Todas") && !posBT.Equals("Todas"))
                                {
                                    foreach (PlateTensionDto item in listTer)
                                    {
                                        positions.Add(item.Posicion);
                                    }
                                }
                                else
                                {
                                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "Solo puede seleccionar todas las posiciones en BT o Ter pero no en ambas", null));

                                }
                            }
                        }
                        else
                        {
                            foreach (PlateTensionDto item in listBt)
                            {
                                positions.Add(item.Posicion);
                            }
                        }
                        break;
                    default:
                        break;
                }

                numberColumns = 3;

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task2 = getConfigurationReports();
                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                List<Task> lote2 = new() { Task2, Task4 };

                await Task.WhenAny(lote2).Result;

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task5.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task5.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (!obj.Mvaf1.ToString().Contains('.'))
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = !stringSplit[i].Contains('.') ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                string ubicacionKV = "";
                string voltajeKv = "";

                switch (KeyTest)
                {
                    case "ATT":
                        ubicacionKV = "Alta Tension";

                        break;
                    case "ABT":
                        ubicacionKV = "Alta Tension";
                        break;
                    case "BTT":
                        ubicacionKV = "Baja Tension";

                        break;
                }

                switch (ubicacionKV)
                {

                    case "Alta Tension":

                        voltajeKv = (conexion == 0 ? Task5.Result.VoltageKV.TensionKvAltaTension1 : conexion == 1 ? Task5.Result.VoltageKV.TensionKvAltaTension3 : null).ToString() + "KV";
                        break;
                    case "Baja Tension":
                        voltajeKv = (conexion == 0 ? Task5.Result.VoltageKV.TensionKvBajaTension1 : conexion == 1 ? Task5.Result.VoltageKV.TensionKvBajaTension3 : null).ToString() + "KV";

                        break;
                }

                resultConfiReports = new SettingsToDisplayRDTReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task2.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task5.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = nroSerie, NoteFc = voltajeKv }, NextTestNumber = Task3.Result.Structure, TitleOfColumns = Task1.Result.Structure, InfotmationArtifact = Task5.Result, ValuePositions = positions };

                return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportRDT));
                return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportRAN")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayRANReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportRAN([FromQuery] string NroSerie, string KeyTest, int NumberMeasurements, string Lenguage, string tokenSesion)
        {

            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            //initialTokens[0] Designinformation
            //initialTokens[1]Masters
            //initialTokens[2]Reports
            //initialTokens[3]Test
            //initialTokens[4]Configuration

            SettingsToDisplayRANReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (NumberMeasurements == 0)
                return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>((int)ResponsesID.fallido, "Cantidad de mediciones es requerido", null));

            if (NumberMeasurements < 1)
                return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>((int)ResponsesID.fallido, "Cantidad de mediciones debe ser mayor o igual a 1", null));

            if (KeyTest.Equals("APD"))
            {
                if (NumberMeasurements is < 1 or > 14)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>((int)ResponsesID.fallido, "La prueba solo acepta de 1 hasta 14 mediciones, favor de corregirlo", null));
            }

            if (KeyTest.Equals("AYD"))
            {
                if (NumberMeasurements is < 1 or > 9)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>((int)ResponsesID.fallido, "La prueba solo acepta de 1 hasta 9 mediciones, favor de corregirlo", null));
            }

            string[] newnoserie = { "" };
            long numberColumns = 1;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<ApiResponse<long>> getNroTestNextRAN() => _reportHttpClientService.GetNroTestNextRAN(NroSerie, KeyTest, Lenguage, NumberMeasurements, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("RAN", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("RAN", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextRAN();

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                List<Task> lote = new() { Task2, Task4, Task3, Task4 };

                await Task.WhenAny(lote).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (Task2.Result.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (Task2.Result.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (Task2.Result.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>((int)ResponsesID.fallido, "“El número de serie no cuenta con información de características", null));

                if (Task2.Result.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (!obj.Mvaf1.ToString().Contains('.'))
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = !stringSplit[i].Contains('.') ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayRANReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result };

                return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportRAD));
                return new JsonResult(new ApiResponse<SettingsToDisplayRANReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        public static string Base64Decode(string base64EncodedData)
        {
            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream memoryStream = new();
            using (GZipStream gZipStream = new(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            byte[] compressedData = new byte[memoryStream.Length];
            _ = memoryStream.Read(compressedData, 0, compressedData.Length);

            byte[] gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        public static string Decompress(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using MemoryStream memoryStream = new();
            int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
            memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

            byte[] buffer = new byte[dataLength];

            memoryStream.Position = 0;
            using (GZipStream gZipStream = new(memoryStream, CompressionMode.Decompress))
            {
                _ = gZipStream.Read(buffer, 0, buffer.Length);
            }

            return Encoding.UTF8.GetString(buffer);
        }
        //public static string Decompress(string compressedString)
        //{
        //    byte[] bytes = Convert.FromBase64String(compressedString);

        //    using (var memoryStream = new MemoryStream(bytes))
        //    {
        //        using (var outputStream = new MemoryStream())
        //        {
        //            using (var decompressStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
        //            {
        //                decompressStream.CopyTo(outputStream);
        //            }
        //            return Encoding.UTF8.GetString(outputStream.ToArray());
        //        }
        //    }
        //}

        //public static string Decompress(string compressedString)
        //{
        //    byte[] decompressedBytes;

        //    var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

        //    using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
        //    {
        //        using ( var decompressedStream = new MemoryStream())
        //        {
        //            decompressorStream.CopyTo(decompressedStream);

        //            decompressedBytes = decompressedStream.ToArray();
        //        }
        //    }

        //    return Encoding.UTF8.GetString(decompressedBytes);
        //}
        public static string getTokenForMicroservices(string initialTokens, int microservices)
        {

            //initialTokens = Decompress(initialTokens);

            string Token = "";
            _ = new string[] { "" };
            string[] getTokens = initialTokens.Split(' ');

            //getTokens[0] Designinformation
            //getTokens[1]Masters
            //getTokens[2]Reports
            //getTokens[3]Test
            //getTokens[4]Configuration

            if (microservices == (int)Enums.MicroservicesEnum.spldesigninformation)
            {
                Token = getTokens[0];
            }
            else if (microservices == (int)Enums.MicroservicesEnum.splmasters)
            {
                Token = getTokens[1];
            }

            else if (microservices == (int)Enums.MicroservicesEnum.splreports)
            {
                Token = getTokens[2];
            }
            else if (microservices == (int)Enums.MicroservicesEnum.spltests)
            {
                Token = getTokens[3];
            }
            else if (microservices == (int)Enums.MicroservicesEnum.splconfiguration)
            {
                Token = getTokens[4];
            }

            return Token;

        }

        [HttpGet("GetConfigurationReportFPC")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayFPCReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportFPC([FromQuery] string NroSerie, string KeyTest, string TypeUnit, string Specification, string Frequency, string Lenguage, string tokenSesion)
        {

            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            string[] initialTokens = new string[] { "" };
            initialTokens = tokenSesion.Split(' ');

            //initialTokens[0] Designinformation
            //initialTokens[1]Masters
            //initialTokens[2]Reports
            //initialTokens[3]Test
            //initialTokens[4]Configuration

            SettingsToDisplayFPCReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie.Trim()))
                return new JsonResult(new ApiResponse<SettingsToDisplayFPCReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest.Trim()))
                return new JsonResult(new ApiResponse<SettingsToDisplayFPCReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(TypeUnit.Trim()))
                return new JsonResult(new ApiResponse<SettingsToDisplayFPCReportsDto>((int)ResponsesID.fallido, "Tipo unidad es requerido", null));

            if (string.IsNullOrEmpty(Specification.Trim()))
                return new JsonResult(new ApiResponse<SettingsToDisplayFPCReportsDto>((int)ResponsesID.fallido, "Especificación es requerido", null));

            if (string.IsNullOrEmpty(Frequency.Trim()))
                return new JsonResult(new ApiResponse<SettingsToDisplayFPCReportsDto>((int)ResponsesID.fallido, "Frecuencia es requerido", null));

            if (string.IsNullOrEmpty(Lenguage.Trim()))
                return new JsonResult(new ApiResponse<SettingsToDisplayFPCReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 0;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {
                Task<ApiResponse<List<ColumnTitleFPCReportsDto>>> getTitleColumnsFPC() => _reportHttpClientService.GetTitleColumnsFPC(TypeUnit, Lenguage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));
                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));
                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);
                Task<ApiResponse<List<ColumnTitleFPCReportsDto>>> Task5 = getTitleColumnsFPC();
                Task<ApiResponse<CorrectionFactorsDescDto>> Task6 = getCorrectionFactorsDesc();
                List<Task> lote0 = new() { Task5, Task2, Task6 };
                await Task.WhenAny(lote0).Result;
                numberColumns = Task5.Result.Structure.Count;

                Task<ApiResponse<long>> getNroTestNextFPC() => _reportHttpClientService.GetNroTestNextFPC(NroSerie, KeyTest, TypeUnit, Specification, Frequency, Lenguage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));
                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("FPC", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));
                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("FPC", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<CorrectionFactorsDescDto>> getCorrectionFactorsDesc() => _reportHttpClientService.GetCorrectionFactorsDesc(Specification, Lenguage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splconfiguration));
                Task<ApiResponse<long>> Task1 = getNroTestNextFPC();
                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();
                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();
                List<Task> lote1 = new() { Task1, Task3, Task4 };
                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPCReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (Task2.Result.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPCReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (Task2.Result.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPCReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (Task2.Result.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPCReportsDto>((int)ResponsesID.fallido, "“El número de serie no cuenta con información de características", null));

                if (Task2.Result.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPCReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (!obj.Mvaf1.ToString().Contains('.'))
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = !stringSplit[i].Contains('.') ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayFPCReportsDto { BaseTemplate = Task4.Result.Structure, TitleOfColumns = Task5.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = (Task6.Result.Structure != null) ? Task6.Result.Structure.Descripcion : "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result };

                return new JsonResult(new ApiResponse<SettingsToDisplayFPCReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportROD));
                return new JsonResult(new ApiResponse<SettingsToDisplayFPCReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));
            }
        }

        [HttpGet("GetConfigurationReportROD")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayRODReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportROD([FromQuery] string noSerie,
            string keyTest,
            string lenguage,
            string connection,
            string unitType,
            string material,
            string unitOfMeasurement,
            decimal? testVoltage,
            long numberColumns,
            string atPositions,
            string btPositions,
            string terPositions, string tokenSesion)
        {

            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayRODReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(noSerie.Trim()))
                return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(keyTest.Trim()))
                return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(lenguage.Trim()))
                return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            if (string.IsNullOrEmpty(connection.Trim()))
                return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Conexión de prueba es requerido", null));

            if (string.IsNullOrEmpty(unitType.Trim()))
                return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Tipo de unidad es requerido", null));

            if (string.IsNullOrEmpty(material.Trim()))
                return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Material del devanado es requerido", null));

            if (string.IsNullOrEmpty(unitOfMeasurement.Trim()))
                return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Unidad de medida es requerido", null));

            //if (string.IsNullOrEmpty(testVoltage.Trim()))
            //    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Tensión de prueba es requerido", null));

            string[] newnoserie = { "" };
            //long numberColumns = 2;
            if (!string.IsNullOrEmpty(noSerie))
                newnoserie = noSerie.Split('-');
            try
            {
                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                Task<ApiResponse<long>> getNroTestNextROD() => _reportHttpClientService.GetNroTestNextROD(noSerie, keyTest, lenguage, connection, unitType, material, unitOfMeasurement, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("ROD", keyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("ROD", keyTest, lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextROD();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                List<Task> lote1 = new() { Task1, Task3, Task4 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(noSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(noSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (Task2.Result.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (Task2.Result.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (Task2.Result.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "“El número de serie no cuenta con información de características", null));

                if (Task2.Result.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                //List<ResistDesignDTO> resistDesigns = (await this._resistanceTwentyDegreesClientServices.GetResistDesignDTO(viewModel.NoSerie, viewModel.UnitOfMeasurement, viewModel.Connection, rodTestsDTO.Temperature)).Structure;
                //if (resistDesigns.Count == 0)
                //{
                //    return this.Json(new
                //    {
                //        response = new ApiResponse<RodViewModel>
                //        {
                //            Code = -2,
                //            Description = $"No existe resistencia a 20 grados a temperatura {rodTestsDTO.Temperature}"
                //        }
                //    });
                //}

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (!obj.Mvaf1.ToString().Contains('.'))
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = !stringSplit[i].Contains('.') ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                bool[] tipos = { false, false, false };

                switch (keyTest)
                {
                    case "AB1":
                    case "AB2":
                        tipos[0] = true;
                        tipos[1] = true;
                        break;
                    case "TA2":
                    case "TB2":
                    case "TO1":
                    case "TO3":
                        tipos[0] = true;
                        tipos[1] = true;
                        tipos[2] = true;
                        break;
                    case "SAT":
                        tipos[0] = true;
                        break;
                    case "SBT":
                        tipos[1] = true;
                        break;
                    case "STE":
                        tipos[2] = true;
                        break;
                    case "AYB":
                        tipos[0] = true;
                        tipos[1] = true;
                        break;
                    case "AYT":
                    case "AT2":
                        tipos[0] = true;
                        tipos[2] = true;
                        break;
                    case "BYT":
                    case "BT2":
                        tipos[1] = true;
                        tipos[2] = true;
                        break;
                    default:
                        break;
                }

                List<string> conList = connection.Split(',').ToList();

                if (conList.Count == 1)
                {
                    if (tipos.Length > 1 && tipos.Length <= 2)
                    {
                        conList.Add(conList[0]);
                    }
                    if (tipos.Length == 3)
                    {
                        conList.Add(conList[0]);
                        conList.Add(conList[0]);
                    }
                }
                List<string> orderedConnections = new();

                InformationArtifactDto general = Task2.Result;

                List<ColumnTitleRODReportsDto> columnTitleRODReportsDtos = new();

                // Generar las posiciones
                int numSections = tipos.Where(x => x).Count();
                int count = 0;
                //bool section1Null = false, section2Null = false;
                for (int i = 0; i < 3; i++)
                {
                    if (tipos[i])
                    {
                        ColumnTitleRODReportsDto columnTitleRODReportsDto = keyTest == "TB2" ? new()
                        {
                            Positions = new(),
                            Position = i == 0 ? "BT" : i == 1 ? "AT" : "TER"
                        } : new()
                        {
                            Positions = new(),
                            Position = i == 0 ? "AT" : i == 1 ? "BT" : "TER"
                        };

                        if (keyTest == "TB2")
                        {
                            string selectedConnection = null;

                            if (i == 0)
                            {
                                selectedConnection = conList.First(c => c.StartsWith("X"));
                            }
                            else if (i == 1)
                            {
                                selectedConnection = conList.First(c => c.StartsWith("H"));
                            }
                            else if (i == 2)
                            {
                                selectedConnection = conList.First(c => c.StartsWith("Y"));
                            }

                            if (!string.IsNullOrEmpty(selectedConnection))
                            {
                                orderedConnections.Add(selectedConnection);
                            }
                        }
                        if (columnTitleRODReportsDto.Position.Equals("AT"))
                        {
                            if (general.TapBaan.ComboNumericSc is 3 or 5)
                            {
                                columnTitleRODReportsDto.Load = false;
                                columnTitleRODReportsDto.Sup = Convert.ToInt32(general.TapBaan.CantidadSupSc);
                                columnTitleRODReportsDto.Inf = Convert.ToInt32(general.TapBaan.CantidadInfSc);
                                columnTitleRODReportsDto.Nom = Convert.ToInt32(general.TapBaan.NominalSc);
                                columnTitleRODReportsDto.Iden = Convert.ToInt32(general.TapBaan.IdentificacionSc);
                                columnTitleRODReportsDto.Inv = Convert.ToInt32(general.TapBaan.InvertidoSc) == 1;
                                columnTitleRODReportsDto.TotalPositions = columnTitleRODReportsDto.Inf + 1 + columnTitleRODReportsDto.Sup;

                                //if (columnTitleRODReportsDto.TotalPositions > 15 && numSections > 1)
                                //{
                                //    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Las posiciones para alta tensión sobrepasan las 15 posiciones permitidas en la plantilla favor se seleccionar otra prueba", null));
                                //}
                                //if (columnTitleRODReportsDto.TotalPositions > 33 && numSections == 1)
                                //{
                                //    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Las posiciones para alta tensión sobrepasan las 33 posiciones permitidas en la plantilla favor se seleccionar otra prueba", null));
                                //}
                                /*if (columnTitleRODReportsDto.Sup + columnTitleRODReportsDto.Inf == 0)
                                {
                                    columnTitleRODReportsDto.Positions.Add("NOM");
                                }
                                else
                                {
                                    columnTitleRODReportsDto.Positions =
                                        GetPositions(columnTitleRODReportsDto.Iden, columnTitleRODReportsDto.Sup, columnTitleRODReportsDto.Inf, columnTitleRODReportsDto.Inv)
                                        .Where(x => atPositions.Contains(x.Trim())).ToList();
                                }*/

                                columnTitleRODReportsDto.Positions = atPositions.Split(",").ToList();

                            }
                            else if (general.TapBaan.ComboNumericBc is 3 or 5)
                            {
                                columnTitleRODReportsDto.Load = true;
                                columnTitleRODReportsDto.Sup = Convert.ToInt32(general.TapBaan.CantidadSupBc);
                                columnTitleRODReportsDto.Inf = Convert.ToInt32(general.TapBaan.CantidadInfBc);
                                columnTitleRODReportsDto.Nom = Convert.ToInt32(general.TapBaan.NominalBc);
                                columnTitleRODReportsDto.Iden = Convert.ToInt32(general.TapBaan.IdentificacionBc);
                                columnTitleRODReportsDto.Inv = Convert.ToInt32(general.TapBaan.InvertidoBc) == 1;
                                columnTitleRODReportsDto.TotalPositions = columnTitleRODReportsDto.Inf + 1 + columnTitleRODReportsDto.Sup;

                                //if (columnTitleRODReportsDto.TotalPositions > 15 && numSections > 1)
                                //{
                                //    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Las posiciones para alta tensión sobrepasan las 15 posiciones permitidas en la plantilla favor se seleccionar otra prueba", null));
                                //}

                                //if (columnTitleRODReportsDto.TotalPositions > 33 && numSections == 1)
                                //{
                                //    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Las posiciones para alta tensión sobrepasan las 33 posiciones permitidas en la plantilla favor se seleccionar otra prueba", null));
                                //}

                                /* if (columnTitleRODReportsDto.Sup + columnTitleRODReportsDto.Inf == 0)
                                 {
                                     columnTitleRODReportsDto.Positions.Add("NOM");
                                 }
                                 else
                                 {
                                     columnTitleRODReportsDto.Positions = GetPositions(columnTitleRODReportsDto.Iden, columnTitleRODReportsDto.Sup, columnTitleRODReportsDto.Inf, columnTitleRODReportsDto.Inv)
                                     .Where(x => atPositions.Contains(x.Trim())).ToList();
                                 }*/

                                columnTitleRODReportsDto.Positions = atPositions.Split(",").ToList();
                            }
                            else
                            {
                                /*if ((general.VoltageKV.TensionKvAltaTension1 is > 0) || (general.VoltageKV.TensionKvAltaTension3 is > 0))
                                {
                                    columnTitleRODReportsDto.Positions.Add("NOM");
                                }*/

                                columnTitleRODReportsDto.Positions = atPositions.Split(",").ToList();
                            }
                        }
                        else if (columnTitleRODReportsDto.Position.Equals("BT"))
                        {
                            if (general.TapBaan.ComboNumericSc is 2 or 4)
                            {
                                columnTitleRODReportsDto.Load = false;
                                columnTitleRODReportsDto.Sup = Convert.ToInt32(general.TapBaan.CantidadSupSc);
                                columnTitleRODReportsDto.Inf = Convert.ToInt32(general.TapBaan.CantidadInfSc);
                                columnTitleRODReportsDto.Nom = Convert.ToInt32(general.TapBaan.NominalSc);
                                columnTitleRODReportsDto.Iden = Convert.ToInt32(general.TapBaan.IdentificacionSc);
                                columnTitleRODReportsDto.Inv = Convert.ToInt32(general.TapBaan.InvertidoSc) == 1;
                                columnTitleRODReportsDto.TotalPositions = columnTitleRODReportsDto.Inf + 1 + columnTitleRODReportsDto.Sup;

                                //if (columnTitleRODReportsDto.TotalPositions > 15 && numSections > 1)
                                //{
                                //    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Las posiciones para baja tensión sobrepasan las 15 posiciones permitidas en la plantilla favor se seleccionar otra prueba", null));
                                //}

                                //if (columnTitleRODReportsDto.TotalPositions > 33 && numSections == 1)
                                //{
                                //    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Las posiciones para baja tensión sobrepasan las 15 posiciones permitidas en la plantilla favor se seleccionar otra prueba", null));
                                //}

                                /* if (columnTitleRODReportsDto.Sup + columnTitleRODReportsDto.Inf == 0)
                                 {
                                     columnTitleRODReportsDto.Positions.Add("NOM");
                                 }
                                 else
                                 {
                                     columnTitleRODReportsDto.Positions = GetPositions(columnTitleRODReportsDto.Iden, columnTitleRODReportsDto.Sup, columnTitleRODReportsDto.Inf, columnTitleRODReportsDto.Inv)
                                         .Where(x => btPositions.Contains(x.Trim())).ToList();
                                 }*/

                                columnTitleRODReportsDto.Positions = btPositions.Split(",").ToList();
                            }
                            else if (general.TapBaan.ComboNumericBc is 2 or 4)
                            {
                                columnTitleRODReportsDto.Load = true;
                                columnTitleRODReportsDto.Sup = Convert.ToInt32(general.TapBaan.CantidadSupBc);
                                columnTitleRODReportsDto.Inf = Convert.ToInt32(general.TapBaan.CantidadInfBc);
                                columnTitleRODReportsDto.Nom = Convert.ToInt32(general.TapBaan.NominalBc);
                                columnTitleRODReportsDto.Iden = Convert.ToInt32(general.TapBaan.IdentificacionBc);
                                columnTitleRODReportsDto.Inv = Convert.ToInt32(general.TapBaan.InvertidoBc) == 1;
                                columnTitleRODReportsDto.TotalPositions = columnTitleRODReportsDto.Inf + 1 + columnTitleRODReportsDto.Sup;

                                //if (columnTitleRODReportsDto.TotalPositions > 15 && numSections > 1)
                                //{
                                //    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Las posiciones para baja tensión sobrepasan las 15 posiciones permitidas en la plantilla favor se seleccionar otra prueba", null));
                                //}

                                //if (columnTitleRODReportsDto.TotalPositions > 33 && numSections == 1)
                                //{
                                //    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Las posiciones para baja tensión sobrepasan las 33 posiciones permitidas en la plantilla favor se seleccionar otra prueba", null));
                                //}

                                //AQUI
                                /* if (columnTitleRODReportsDto.Sup + columnTitleRODReportsDto.Inf == 0)
                                 {
                                     columnTitleRODReportsDto.Positions.Add("NOM");
                                 }
                                 else
                                 {
                                     columnTitleRODReportsDto.Positions = GetPositions(columnTitleRODReportsDto.Iden, columnTitleRODReportsDto.Sup, columnTitleRODReportsDto.Inf, columnTitleRODReportsDto.Inv)
                                         .Where(x => btPositions.Contains(x.Trim())).ToList();
                                 }*/

                                columnTitleRODReportsDto.Positions = btPositions.Split(",").ToList();
                            }
                            else
                            {
                                /* if ((general.VoltageKV.TensionKvBajaTension1 is > 0) || (general.VoltageKV.TensionKvBajaTension3 is > 0))
                                 {
                                     columnTitleRODReportsDto.Positions.Add("NOM");
                                 }*/

                                columnTitleRODReportsDto.Positions = btPositions.Split(",").ToList();
                            }
                        }
                        else
                        {
                            if (general.TapBaan.ComboNumericSc is 1)
                            {
                                columnTitleRODReportsDto.Load = false;
                                columnTitleRODReportsDto.Sup = Convert.ToInt32(general.TapBaan.CantidadSupSc);
                                columnTitleRODReportsDto.Inf = Convert.ToInt32(general.TapBaan.CantidadInfSc);
                                columnTitleRODReportsDto.Nom = Convert.ToInt32(general.TapBaan.NominalSc);
                                columnTitleRODReportsDto.Iden = Convert.ToInt32(general.TapBaan.IdentificacionSc);
                                columnTitleRODReportsDto.Inv = Convert.ToInt32(general.TapBaan.InvertidoSc) == 1;
                                columnTitleRODReportsDto.TotalPositions = columnTitleRODReportsDto.Inf + 1 + columnTitleRODReportsDto.Sup;

                                if (columnTitleRODReportsDto.TotalPositions > 15 && numSections > 1)
                                {
                                    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Las posiciones para la tensión terciaria sobrepasan las 15 posiciones permitidas en la plantilla favor se seleccionar otra prueba", null));
                                }

                                if (columnTitleRODReportsDto.TotalPositions > 33 && numSections == 1)
                                {
                                    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Las posiciones para la tensión terciaria sobrepasan las 33 posiciones permitidas en la plantilla favor se seleccionar otra prueba", null));
                                }

                                //AQUI
                                /*if (columnTitleRODReportsDto.Sup + columnTitleRODReportsDto.Inf == 0)
                                {
                                    columnTitleRODReportsDto.Positions.Add("NOM");
                                }
                                else
                                {
                                    columnTitleRODReportsDto.Positions = GetPositions(columnTitleRODReportsDto.Iden, columnTitleRODReportsDto.Sup, columnTitleRODReportsDto.Inf, columnTitleRODReportsDto.Inv)
                                        .Where(x => terPositions.Contains(x.Trim())).ToList();
                                }*/

                                columnTitleRODReportsDto.Positions = terPositions.Split(",").ToList();
                            }
                            else if (general.TapBaan.ComboNumericBc is 1)
                            {
                                columnTitleRODReportsDto.Load = true;
                                columnTitleRODReportsDto.Sup = Convert.ToInt32(general.TapBaan.CantidadSupBc);
                                columnTitleRODReportsDto.Inf = Convert.ToInt32(general.TapBaan.CantidadInfBc);
                                columnTitleRODReportsDto.Nom = Convert.ToInt32(general.TapBaan.NominalBc);
                                columnTitleRODReportsDto.Iden = Convert.ToInt32(general.TapBaan.IdentificacionBc);
                                columnTitleRODReportsDto.Inv = Convert.ToInt32(general.TapBaan.InvertidoBc) == 1;
                                columnTitleRODReportsDto.TotalPositions = columnTitleRODReportsDto.Inf + 1 + columnTitleRODReportsDto.Sup;

                                if (columnTitleRODReportsDto.TotalPositions > 15 && numSections > 1)
                                {
                                    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Las posiciones para la tensión terciaria sobrepasan las 15 posiciones permitidas en la plantilla favor se seleccionar otra prueba", null));
                                }

                                if (columnTitleRODReportsDto.TotalPositions > 33 && numSections == 1)
                                {
                                    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Las posiciones para la tensión terciaria sobrepasan las 33 posiciones permitidas en la plantilla favor se seleccionar otra prueba", null));
                                }

                                //AQUI
                                /* if (columnTitleRODReportsDto.Sup + columnTitleRODReportsDto.Inf == 0)
                                 {
                                     columnTitleRODReportsDto.Positions.Add("NOM");
                                 }
                                 else
                                 {
                                     columnTitleRODReportsDto.Positions = GetPositions(columnTitleRODReportsDto.Iden, columnTitleRODReportsDto.Sup, columnTitleRODReportsDto.Inf, columnTitleRODReportsDto.Inv)
                                         .Where(x => terPositions.Contains(x.Trim())).ToList();
                                 }*/

                                columnTitleRODReportsDto.Positions = terPositions.Split(",").ToList();
                            }
                            else
                            {
                                /*if ((general.VoltageKV.TensionKvTerciario1 is > 0) || (general.VoltageKV.TensionKvTerciario3 is > 0))
                                {
                                    columnTitleRODReportsDto.Positions.Add("NOM");
                                }*/

                                columnTitleRODReportsDto.Positions = terPositions.Split(",").ToList();
                            }
                        }

                        columnTitleRODReportsDtos.Add(columnTitleRODReportsDto);
                        count++;
                    }
                }

                if (orderedConnections.Count > 0)
                {
                    conList = orderedConnections;
                }

                string voltage = testVoltage is null or 0
                    ? string.Empty
                    : lenguage.Equals("ES") ? $"Tensión de Prueba {testVoltage} KV" : $"Test Voltage {testVoltage} KV";

                if (keyTest == "SAT")
                {
                    columnTitleRODReportsDtos[0].Connection = connection;

                    if (connection.Contains("H-H"))
                    {
                        columnTitleRODReportsDtos[0].Terminal1 = "H1-H2";
                        columnTitleRODReportsDtos[0].Terminal2 = "H2-H3";
                        columnTitleRODReportsDtos[0].Terminal3 = "H3-H1";
                    }
                    else if (connection.Contains("L-N"))
                    {
                        columnTitleRODReportsDtos[0].Terminal1 = "H1-H0";
                        columnTitleRODReportsDtos[0].Terminal2 = "H2-H0";
                        columnTitleRODReportsDtos[0].Terminal3 = "H3-H0";
                    }
                    else if (connection.Contains("H-X"))
                    {
                        columnTitleRODReportsDtos[0].Terminal1 = "H1-X1";
                        columnTitleRODReportsDtos[0].Terminal2 = "H2-X2";
                        columnTitleRODReportsDtos[0].Terminal3 = "H3-X3";
                    }
                    else if (connection.Contains("H-N"))
                    {
                        columnTitleRODReportsDtos[0].Terminal1 = "H1-H0X0";
                        columnTitleRODReportsDtos[0].Terminal2 = "H2-H0X0";
                        columnTitleRODReportsDtos[0].Terminal3 = "H3-H0X0";
                    }
                    else
                    {
                        columnTitleRODReportsDtos[0].Terminal1 = "H1-H2";
                        columnTitleRODReportsDtos[0].Terminal2 = "H2-H3";
                        columnTitleRODReportsDtos[0].Terminal3 = "H3-H1";
                    }
                }
                else if (keyTest == "SBT")
                {
                    if (connection.Contains("X-X"))
                    {
                        columnTitleRODReportsDtos[0].Terminal1 = "X1-X2";
                        columnTitleRODReportsDtos[0].Terminal2 = "X2-X3";
                        columnTitleRODReportsDtos[0].Terminal3 = "X3-X1";
                    }
                    else if (connection.Contains("L-N"))
                    {
                        columnTitleRODReportsDtos[0].Terminal1 = "X1-X0";
                        columnTitleRODReportsDtos[0].Terminal2 = "X2-X0";
                        columnTitleRODReportsDtos[0].Terminal3 = "X3-X0";
                    }
                    else if (connection.Contains("X-N"))
                    {
                        columnTitleRODReportsDtos[0].Terminal1 = "X1-H0X0";
                        columnTitleRODReportsDtos[0].Terminal2 = "X2-H0X0";
                        columnTitleRODReportsDtos[0].Terminal3 = "X3-H0X0";
                    }
                    else
                    {
                        columnTitleRODReportsDtos[0].Terminal1 = "X1-X2";
                        columnTitleRODReportsDtos[0].Terminal2 = "X2-X3";
                        columnTitleRODReportsDtos[0].Terminal3 = "X3-X1";
                    }
                }
                else if (keyTest == "STE")
                {
                    if (connection.Contains("Y-Y"))
                    {
                        columnTitleRODReportsDtos[0].Terminal1 = "Y1-Y2";
                        columnTitleRODReportsDtos[0].Terminal2 = "Y2-Y3";
                        columnTitleRODReportsDtos[0].Terminal3 = "Y3-Y1";
                    }
                    else if (connection.Contains("L-N") || connection.Contains("Y-N"))
                    {
                        columnTitleRODReportsDtos[0].Terminal1 = "Y1-Y0";
                        columnTitleRODReportsDtos[0].Terminal2 = "Y2-Y0";
                        columnTitleRODReportsDtos[0].Terminal3 = "Y3-Y0";
                    }
                    else
                    {
                        columnTitleRODReportsDtos[0].Terminal1 = "Y1-Y2";
                        columnTitleRODReportsDtos[0].Terminal2 = "Y2-Y3";
                        columnTitleRODReportsDtos[0].Terminal3 = "Y3-Y1";
                    }
                }
                else if (keyTest is "AB1" or "AB2")
                {
                    //Alta Tension

                    columnTitleRODReportsDtos[0].Connection = conList[0];

                    switch (conList[0])
                    {
                        case "L-L":
                        case "H-H":
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-H2";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-H3";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-H1";
                            break;
                        case "L-N":
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-H0";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-H0";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-H0";
                            break;
                        case "H-X":
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-X1";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-X2";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-X3";
                            break;
                        case "H-N":
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-H0X0";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-H0X0";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-H0X0";
                            break;
                        default:
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-H2";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-H3";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-H1";
                            break;
                    }

                    //Baja Tension

                    if (conList.Count > 1)
                    {
                        columnTitleRODReportsDtos[1].Connection = conList[1];

                        switch (conList[1])
                        {
                            case "L-L":
                            case "X-X":
                                columnTitleRODReportsDtos[1].Terminal1 = "X1-X2";
                                columnTitleRODReportsDtos[1].Terminal2 = "X2-X3";
                                columnTitleRODReportsDtos[1].Terminal3 = "X3-X1";
                                break;
                            case "L-N":
                                columnTitleRODReportsDtos[1].Terminal1 = "X1-X0";
                                columnTitleRODReportsDtos[1].Terminal2 = "X2-X0";
                                columnTitleRODReportsDtos[1].Terminal3 = "X3-X0";
                                break;
                            case "X-N":
                                columnTitleRODReportsDtos[1].Terminal1 = "X1-H0X0";
                                columnTitleRODReportsDtos[1].Terminal2 = "X2-H0X0";
                                columnTitleRODReportsDtos[1].Terminal3 = "X3-H0X0";
                                break;
                            default:
                                columnTitleRODReportsDtos[1].Terminal1 = "X1-X2";
                                columnTitleRODReportsDtos[1].Terminal2 = "X2-X3";
                                columnTitleRODReportsDtos[1].Terminal3 = "X3-X1";
                                break;
                        }
                    }
                    else if (conList.Count == 1 && conList[0].Equals("L-L"))
                    {
                        columnTitleRODReportsDtos[1].Connection = conList[1];
                        columnTitleRODReportsDtos[1].Terminal1 = "X1-X2";
                        columnTitleRODReportsDtos[1].Terminal2 = "X2-X3";
                        columnTitleRODReportsDtos[1].Terminal3 = "X3-X1";
                    }
                }
                else if (keyTest is "AYT" or "AT2")
                {
                    //Alta Tension
                    switch (conList[0])
                    {
                        case "L-L":
                        case "H-H":
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-H2";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-H3";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-H1";
                            break;
                        case "L-N":
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-H0";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-H0";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-H0";
                            break;
                        case "H-X":
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-X1";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-X2";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-X3";
                            break;
                        case "H-N":
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-H0X0";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-H0X0";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-H0X0";
                            break;
                        default:
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-H2";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-H3";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-H1";
                            break;
                    }

                    columnTitleRODReportsDtos[0].Connection = conList[0];

                    //Terciario
                    switch (conList[1])
                    {
                        case "L-L":
                        case "Y-Y":
                            columnTitleRODReportsDtos[1].Terminal1 = "Y1-Y2";
                            columnTitleRODReportsDtos[1].Terminal2 = "Y2-Y3";
                            columnTitleRODReportsDtos[1].Terminal3 = "Y3-Y1";
                            break;
                        case "Y-N":
                        case "L-N":
                            columnTitleRODReportsDtos[1].Terminal1 = "Y1-Y0";
                            columnTitleRODReportsDtos[1].Terminal2 = "Y2-Y0";
                            columnTitleRODReportsDtos[1].Terminal3 = "Y3-Y0";
                            break;
                        default:
                            columnTitleRODReportsDtos[1].Terminal1 = "Y1-Y2";
                            columnTitleRODReportsDtos[1].Terminal2 = "Y2-Y3";
                            columnTitleRODReportsDtos[1].Terminal3 = "Y3-Y1";
                            break;
                    }

                    columnTitleRODReportsDtos[1].Connection = conList[1];
                }
                else if (keyTest is "BYT" or "BT2")
                {
                    //Baja Tension
                    switch (conList[0])
                    {
                        case "L-L":
                        case "X-X":
                            columnTitleRODReportsDtos[0].Terminal1 = "X1-X2";
                            columnTitleRODReportsDtos[0].Terminal2 = "X2-X3";
                            columnTitleRODReportsDtos[0].Terminal3 = "X3-X1";
                            break;
                        case "L-N":
                            columnTitleRODReportsDtos[0].Terminal1 = "X1-X0";
                            columnTitleRODReportsDtos[0].Terminal2 = "X2-X0";
                            columnTitleRODReportsDtos[0].Terminal3 = "X3-X0";
                            break;
                        case "X-N":
                            columnTitleRODReportsDtos[0].Terminal1 = "X1-H0X0";
                            columnTitleRODReportsDtos[0].Terminal2 = "X2-H0X0";
                            columnTitleRODReportsDtos[0].Terminal3 = "X3-H0X0";
                            break;
                        default:
                            columnTitleRODReportsDtos[0].Terminal1 = "X1-X2";
                            columnTitleRODReportsDtos[0].Terminal2 = "X2-X3";
                            columnTitleRODReportsDtos[0].Terminal3 = "X3-X1";
                            break;
                    }

                    columnTitleRODReportsDtos[0].Connection = conList[0];

                    //Terciario
                    switch (conList[1])
                    {
                        case "L-L":
                        case "Y-Y":
                            columnTitleRODReportsDtos[1].Terminal1 = "Y1-Y2";
                            columnTitleRODReportsDtos[1].Terminal2 = "Y2-Y3";
                            columnTitleRODReportsDtos[1].Terminal3 = "Y3-Y1";
                            break;
                        case "Y-N":
                        case "L-N":
                            columnTitleRODReportsDtos[1].Terminal1 = "Y1-Y0";
                            columnTitleRODReportsDtos[1].Terminal2 = "Y2-Y0";
                            columnTitleRODReportsDtos[1].Terminal3 = "Y3-Y0";
                            break;
                        default:
                            columnTitleRODReportsDtos[1].Terminal1 = "Y1-Y2";
                            columnTitleRODReportsDtos[1].Terminal2 = "Y2-Y3";
                            columnTitleRODReportsDtos[1].Terminal3 = "Y3-Y1";
                            break;
                    }

                    columnTitleRODReportsDtos[1].Connection = conList[1];
                }
                else if (keyTest is "TO1" or "TA2" or "TO3")
                {

                    //Alta Tension
                    switch (conList[0])
                    {
                        case "L-L":
                        case "H-H":
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-H2";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-H3";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-H1";
                            break;
                        case "L-N":
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-H0";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-H0";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-H0";
                            break;
                        case "H-X":
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-X1";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-X2";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-X3";
                            break;
                        case "H-N":
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-H0X0";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-H0X0";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-H0X0";
                            break;
                        default:
                            columnTitleRODReportsDtos[0].Terminal1 = "H1-H2";
                            columnTitleRODReportsDtos[0].Terminal2 = "H2-H3";
                            columnTitleRODReportsDtos[0].Terminal3 = "H3-H1";
                            break;
                    }

                    columnTitleRODReportsDtos[0].Connection = conList[0];

                    //Baja Tension
                    switch (conList[1])
                    {
                        case "L-L":
                        case "X-X":
                            columnTitleRODReportsDtos[1].Terminal1 = "X1-X2";
                            columnTitleRODReportsDtos[1].Terminal2 = "X2-X3";
                            columnTitleRODReportsDtos[1].Terminal3 = "X3-X1";
                            break;
                        case "L-N":
                            columnTitleRODReportsDtos[1].Terminal1 = "X1-X0";
                            columnTitleRODReportsDtos[1].Terminal2 = "X2-X0";
                            columnTitleRODReportsDtos[1].Terminal3 = "X3-X0";
                            break;
                        case "X-N":
                            columnTitleRODReportsDtos[1].Terminal1 = "X1-H0X0";
                            columnTitleRODReportsDtos[1].Terminal2 = "X2-H0X0";
                            columnTitleRODReportsDtos[1].Terminal3 = "X3-H0X0";
                            break;
                        default:
                            columnTitleRODReportsDtos[1].Terminal1 = "X1-X2";
                            columnTitleRODReportsDtos[1].Terminal2 = "X2-X3";
                            columnTitleRODReportsDtos[1].Terminal3 = "X3-X1";
                            break;
                    }

                    columnTitleRODReportsDtos[1].Connection = conList[1];

                    //Terciario
                    switch (conList[2])
                    {
                        case "L-L":
                        case "Y-Y":
                            columnTitleRODReportsDtos[2].Terminal1 = "Y1-Y2";
                            columnTitleRODReportsDtos[2].Terminal2 = "Y2-Y3";
                            columnTitleRODReportsDtos[2].Terminal3 = "Y3-Y1";
                            break;
                        case "Y-N":
                        case "L-N":
                            columnTitleRODReportsDtos[2].Terminal1 = "Y1-Y0";
                            columnTitleRODReportsDtos[2].Terminal2 = "Y2-Y0";
                            columnTitleRODReportsDtos[2].Terminal3 = "Y3-Y0";
                            break;
                        default:
                            columnTitleRODReportsDtos[2].Terminal1 = "Y1-Y2";
                            columnTitleRODReportsDtos[2].Terminal2 = "Y2-Y3";
                            columnTitleRODReportsDtos[2].Terminal3 = "Y3-Y1";
                            break;
                    }

                    columnTitleRODReportsDtos[2].Connection = conList[2];
                }
                else if (keyTest is "TB2")
                {
                    //Baja Tension
                    switch (conList[0])
                    {
                        case "L-L":
                        case "X-X":
                            columnTitleRODReportsDtos[0].Terminal1 = "X1-X2";
                            columnTitleRODReportsDtos[0].Terminal2 = "X2-X3";
                            columnTitleRODReportsDtos[0].Terminal3 = "X3-X1";
                            break;
                        case "L-N":
                            columnTitleRODReportsDtos[0].Terminal1 = "X1-X0";
                            columnTitleRODReportsDtos[0].Terminal2 = "X2-X0";
                            columnTitleRODReportsDtos[0].Terminal3 = "X3-X0";
                            break;
                        case "X-N":
                            columnTitleRODReportsDtos[0].Terminal1 = "X1-H0X0";
                            columnTitleRODReportsDtos[0].Terminal2 = "X2-H0X0";
                            columnTitleRODReportsDtos[0].Terminal3 = "X3-H0X0";
                            break;
                        default:
                            columnTitleRODReportsDtos[0].Terminal1 = "X1-X2";
                            columnTitleRODReportsDtos[0].Terminal2 = "X2-X3";
                            columnTitleRODReportsDtos[0].Terminal3 = "X3-X1";
                            break;
                    }

                    columnTitleRODReportsDtos[0].Connection = conList[0];

                    //Alta Tension
                    switch (conList[1])
                    {
                        case "L-L":
                        case "H-H":
                            columnTitleRODReportsDtos[1].Terminal1 = "H1-H2";
                            columnTitleRODReportsDtos[1].Terminal2 = "H2-H3";
                            columnTitleRODReportsDtos[1].Terminal3 = "H3-H1";
                            break;
                        case "L-N":
                            columnTitleRODReportsDtos[1].Terminal1 = "H1-H0";
                            columnTitleRODReportsDtos[1].Terminal2 = "H2-H0";
                            columnTitleRODReportsDtos[1].Terminal3 = "H3-H0";
                            break;
                        case "H-X":
                            columnTitleRODReportsDtos[1].Terminal1 = "H1-X1";
                            columnTitleRODReportsDtos[1].Terminal2 = "H2-X2";
                            columnTitleRODReportsDtos[1].Terminal3 = "H3-X3";
                            break;
                        case "H-N":
                            columnTitleRODReportsDtos[1].Terminal1 = "H1-H0X0";
                            columnTitleRODReportsDtos[1].Terminal2 = "H2-H0X0";
                            columnTitleRODReportsDtos[1].Terminal3 = "H3-H0X0";
                            break;
                        default:
                            columnTitleRODReportsDtos[1].Terminal1 = "H1-H2";
                            columnTitleRODReportsDtos[1].Terminal2 = "H2-H3";
                            columnTitleRODReportsDtos[1].Terminal3 = "H3-H1";
                            break;
                    }

                    columnTitleRODReportsDtos[1].Connection = conList[1];

                    //Terciario
                    switch (conList[2])
                    {
                        case "L-L":
                        case "Y-Y":
                            columnTitleRODReportsDtos[2].Terminal1 = "Y1-Y2";
                            columnTitleRODReportsDtos[2].Terminal2 = "Y2-Y3";
                            columnTitleRODReportsDtos[2].Terminal3 = "Y3-Y1";
                            break;
                        case "Y-N":
                        case "L-N":
                            columnTitleRODReportsDtos[2].Terminal1 = "Y1-Y0";
                            columnTitleRODReportsDtos[2].Terminal2 = "Y2-Y0";
                            columnTitleRODReportsDtos[2].Terminal3 = "Y3-Y0";
                            break;
                        default:
                            columnTitleRODReportsDtos[2].Terminal1 = "Y1-Y2";
                            columnTitleRODReportsDtos[2].Terminal2 = "Y2-Y3";
                            columnTitleRODReportsDtos[2].Terminal3 = "Y3-Y1";
                            break;
                    }

                    columnTitleRODReportsDtos[2].Connection = conList[2];
                }

                // Otra parte
                decimal overElevation = general.CharacteristicsArtifact.Max(x => x.OverElevation) ?? 0;

                columnTitleRODReportsDtos.ForEach(x => x.TempSE = Convert.ToInt32(overElevation) + 20);

                resultConfiReports = new SettingsToDisplayRODReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = noSerie }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, TitleOfColumns = columnTitleRODReportsDtos, TestVoltage = voltage };

                return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportROD));
                return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));
            }
        }

        /// <summary>
        /// Obtiene las posiciones en una tension en especifica
        /// </summary>
        /// <param name="ind">Indicador de Posiciones</param>
        /// <param name="up">Cantidad de posiciones hacia arriba</param>
        /// <param name="down">Cantidad de posiciones hacia abajo</param>
        /// <param name="inv">Indicador de inevrtido</param>
        /// <returns></returns>
        private static List<string> GetPositions(int ind, int up, int down, bool inv)
        {
            List<string> positions = new();
            int cant = up + 1 + down;
            switch (ind)
            {
                case 1:
                    for (int j = 65; j < 65 + cant; j++)
                    {
                        positions.Add(Convert.ToChar(j).ToString());
                    }
                    break;
                case 2:
                    for (int j = 1; j <= cant; j++)
                    {
                        positions.Add(j.ToString());
                    }
                    break;
                case 3:
                    for (int j = up; j > 0; j--)
                    {
                        positions.Add(j.ToString() + "R");
                    }
                    positions.Add("NOM");
                    for (int j = 1; j <= down; j++)
                    {
                        positions.Add(j.ToString() + "L");
                    }
                    break;
                case 4:
                    for (int j = up; j > 0; j--)
                    {
                        positions.Add(j.ToString() + "L");
                    }
                    positions.Add("NOM");
                    for (int j = 1; j <= down; j++)
                    {
                        positions.Add(j.ToString() + "R");
                    }
                    break;
                default:
                    break;
            }
            if (inv)
            {
                positions.Reverse();
            }
            return positions;
        }

        [HttpGet("GetConfigurationReportFPB")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayFPBReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportFPB(string NroSerie, string KeyTest, string TangentDelta, string Lenguage, string tokenSesion)
        {

            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayFPBReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(TangentDelta))
                return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>((int)ResponsesID.fallido, "Tangente delta es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 2;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                Task<ApiResponse<NozzlesByDesignDto>> TaskBoqDet() => _reportHttpClientService.GetInformationBoqDet(NroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));
                Task<ApiResponse<NozzlesByDesignDto>> Task5 = TaskBoqDet();

                List<Task> lote0 = new() { Task5, Task2, Task5 };
                await Task.WhenAny(lote0).Result;

                Task<ApiResponse<long>> getNroTestNextFPB() => _reportHttpClientService.GetNroTestNextFPB(NroSerie, KeyTest, Lenguage, TangentDelta, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("FPB", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("FPB", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextFPB();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                List<Task> lote1 = new() { Task1, Task3, Task4 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (Task2.Result.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (Task2.Result.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (Task2.Result.NozzlesArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>((int)ResponsesID.fallido, "“El número de serie no cuenta con información de boquillas", null));

                if (Task5.Result.Structure.NozzleInformation.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información detallada de las boquillas, favor de corregirlo", null));

                if (Task5.Result.Structure.NozzleInformation.Where(x => x.Prueba != true).ToList().Count != Task5.Result.Structure.TotalQuantity)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información detallada de las boquillas, favor de corregirlo", null));

                if (KeyTest.Equals("AYD"))
                {
                    if (Task5.Result.Structure.TotalQuantity > 15)
                        return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>((int)ResponsesID.fallido, "La cantidad de boquillas del aparato sobre pasa los 15 renglones de la plantilla de captura", null));

                }
                else
                {
                    if (Task5.Result.Structure.TotalQuantity > 36)
                        return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>((int)ResponsesID.fallido, "La cantidad de boquillas del aparato sobre pasa los 36 renglones de la plantilla de captura", null));

                }

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (!obj.Mvaf1.ToString().Contains('.'))
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = !stringSplit[i].Contains('.') ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                NozzlesByDesignDto boqDet = new() { NozzleInformation = Task5.Result.Structure.NozzleInformation.Where(x => x.Prueba != true).ToList(), TotalQuantity = Task5.Result.Structure.TotalQuantity };

                resultConfiReports = new SettingsToDisplayFPBReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, NozzlesByDesignDtos = boqDet };

                return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportFPB));
                return new JsonResult(new ApiResponse<SettingsToDisplayFPBReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportRCT")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayRCTReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportRCT([FromQuery] string NroSerie, string KeyTest, string Lenguage, string UnitOfMeasurement, string Tertiary, decimal Testvoltage, string tokenSesion)
        {

            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            SettingsToDisplayRCTReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(UnitOfMeasurement))
                return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "Unidad de medida es requerido", null));

            //if (string.IsNullOrEmpty(Tertiary))
            //    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "Terciario o segunda baja es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 0;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                switch (KeyTest)
                {
                    case "ATI":
                        numberColumns = 1;
                        break;
                    case "ABS":
                        numberColumns = 2;
                        break;
                    case "ABI":
                        numberColumns = 2;
                        break;
                    case "TSI":
                        numberColumns = 3;
                        break;
                    case "TOS":
                        numberColumns = 3;
                        break;
                    case "TOI":
                        numberColumns = 3;
                        break;
                    case "TIS":
                        numberColumns = 3;
                        break;

                    default:
                        break;
                }

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                Task<ApiResponse<long>> getNroTestNextRCT() => _reportHttpClientService.GetNroTestNextRCT(NroSerie, KeyTest, Lenguage, UnitOfMeasurement, Tertiary, Testvoltage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("RCT", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("RCT", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextRCT();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                List<Task> lote1 = new() { Task1, Task3, Task4 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (Task2.Result.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (Task2.Result.VoltageKV == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (Task2.Result.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                bool aT = Task2.Result.VoltageKV.TensionKvAltaTension1 is not 0 and not null && Task2.Result.VoltageKV.TensionKvAltaTension3 is not 0 and not null;
                bool bT = Task2.Result.VoltageKV.TensionKvBajaTension1 is not 0 and not null && Task2.Result.VoltageKV.TensionKvBajaTension3 is not 0 and not null;
                bool ter = Task2.Result.VoltageKV.TensionKvTerciario1 is not 0 and not null && Task2.Result.VoltageKV.TensionKvTerciario3 is not 0 and not null;

                if (aT && Testvoltage is 0)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "La tension de prueba es requerida", null));
                }

                if (bT && !KeyTest.Equals("ATI") && Testvoltage is 0)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "La tension de prueba es requerida", null));
                }

                if (ter && (KeyTest.Equals("TSI") || KeyTest.Equals("TOS") || KeyTest.Equals("TOI") || KeyTest.Equals("TIS")) && Testvoltage is 0)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "La tension de prueba es requerida", null));
                }

                int[] validationA = CommonMethods.cantDigitsPoint(Convert.ToDouble(Testvoltage));

                if (validationA[0] > 3 || validationA[1] > 3)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "La tensión de prueba debe ser numérica mayor a cero considerando 3 enteros con 3 decimales", null));
                }

                // bool existTestvoltage = false;
                // if (Testvoltage == Task2.Result.VoltageKV.TensionKvAltaTension1)
                //     existTestvoltage = true;
                //else if (Testvoltage == Task2.Result.VoltageKV.TensionKvAltaTension3)
                //     existTestvoltage = true;
                // else if (Testvoltage == Task2.Result.VoltageKV.TensionKvBajaTension1)
                //     existTestvoltage = true;
                // else if (Testvoltage == Task2.Result.VoltageKV.TensionKvBajaTension3)
                //     existTestvoltage = true;
                // else if (Testvoltage == Task2.Result.VoltageKV.TensionKvTerciario3)
                //     existTestvoltage = true;

                //if (!existTestvoltage)
                //{
                //    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "La tensión de prueba no es válida para el aparato, favor de corregirla", null));
                //}

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (!obj.Mvaf1.ToString().Contains('.'))
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = !stringSplit[i].Contains('.') ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayRCTReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result };

                return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportRAD));
                return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportFCE")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayRCTReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportFCE([FromQuery] string NroSerie, string KeyTest, string Lenguage, string UnitOfMeasurement, string Tertiary, decimal Testvoltage, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            SettingsToDisplayRCTReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(UnitOfMeasurement))
                return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "Unidad de medida es requerido", null));

            if (string.IsNullOrEmpty(Tertiary))
                return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "Terciario o segunda baja es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 0;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                switch (KeyTest)
                {
                    case "ATI":
                        numberColumns = 1;
                        break;
                    case "ABS":
                        numberColumns = 2;
                        break;
                    case "ABI":
                        numberColumns = 2;
                        break;
                    case "TSI":
                        numberColumns = 3;
                        break;
                    case "TOS":
                        numberColumns = 3;
                        break;
                    case "TOI":
                        numberColumns = 3;
                        break;
                    case "TIS":
                        numberColumns = 3;
                        break;

                    default:
                        break;
                }

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                Task<ApiResponse<long>> getNroTestNextRCT() => _reportHttpClientService.GetNroTestNextRCT(NroSerie, KeyTest, Lenguage, UnitOfMeasurement, Tertiary, Testvoltage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("RCT", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("RCT", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextRCT();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                List<Task> lote1 = new() { Task1, Task3, Task4 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (Task2.Result.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (Task2.Result.VoltageKV == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (Task2.Result.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                bool aT = Task2.Result.VoltageKV.TensionKvAltaTension1 is not 0 and not null && Task2.Result.VoltageKV.TensionKvAltaTension3 is not 0 and not null;
                bool bT = Task2.Result.VoltageKV.TensionKvBajaTension1 is not 0 and not null && Task2.Result.VoltageKV.TensionKvBajaTension3 is not 0 and not null;
                bool ter = Task2.Result.VoltageKV.TensionKvTerciario1 is not 0 and not null && Task2.Result.VoltageKV.TensionKvTerciario3 is not 0 and not null;

                if (aT && Testvoltage is 0)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "La tension de prueba es requerida", null));
                }

                if (bT && !KeyTest.Equals("ATI") && Testvoltage is 0)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "La tension de prueba es requerida", null));
                }

                if (ter && (KeyTest.Equals("TSI") || KeyTest.Equals("TOS") || KeyTest.Equals("TOI") || KeyTest.Equals("TIS")) && Testvoltage is 0)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "La tension de prueba es requerida", null));
                }

                int[] validationA = CommonMethods.cantDigitsPoint(Convert.ToDouble(Testvoltage));

                if (validationA[0] > 3 || validationA[1] > 3)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "La tensión de prueba debe ser numérica mayor a cero considerando 3 enteros con 3 decimales", null));
                }

                // bool existTestvoltage = false;
                // if (Testvoltage == Task2.Result.VoltageKV.TensionKvAltaTension1)
                //     existTestvoltage = true;
                //else if (Testvoltage == Task2.Result.VoltageKV.TensionKvAltaTension3)
                //     existTestvoltage = true;
                // else if (Testvoltage == Task2.Result.VoltageKV.TensionKvBajaTension1)
                //     existTestvoltage = true;
                // else if (Testvoltage == Task2.Result.VoltageKV.TensionKvBajaTension3)
                //     existTestvoltage = true;
                // else if (Testvoltage == Task2.Result.VoltageKV.TensionKvTerciario3)
                //     existTestvoltage = true;

                //if (!existTestvoltage)
                //{
                //    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "La tensión de prueba no es válida para el aparato, favor de corregirla", null));
                //}

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (!obj.Mvaf1.ToString().Contains('.'))
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = !stringSplit[i].Contains('.') ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayRCTReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result };

                return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportRAD));
                return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetPositionsArtifact")]
        [ProducesResponseType(typeof(ApiResponse<PositionsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetPositionsArtifact([FromQuery] string noSerie, string tokenSesion)
        {
            //if (Request.Headers.TryGetValue("ApiKey", out var testId))
            //{
            //    tokenSesion = testId; //use testId value
            //}
            PositionsDto positionsDto = new()
            {
                AltaTension = new(),
                BajaTension = new(),
                Terciario = new()
            };

            if (string.IsNullOrEmpty(noSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            string[] newnoserie = { "" };
            if (!string.IsNullOrEmpty(noSerie.Trim()))
                newnoserie = noSerie.Split('-');
            try
            {
                InformationArtifactDto general = await _reportHttpClientService.GetArtifact(newnoserie[0], getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                bool valLongitud = Validations.validacion55Caracteres(noSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(noSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                // Generar las posiciones
                int Sup, Inf, Nom, Iden, TotalPositions = 0;
                bool Inv;

                #region Alta Tension
                if (general.TapBaan.ComboNumericSc is 3 or 5)
                {
                    Sup = Convert.ToInt32(general.TapBaan.CantidadSupSc);
                    Inf = Convert.ToInt32(general.TapBaan.CantidadInfSc);
                    Nom = Convert.ToInt32(general.TapBaan.NominalSc);
                    Iden = Convert.ToInt32(general.TapBaan.IdentificacionSc);
                    Inv = Convert.ToInt32(general.TapBaan.InvertidoSc) == 1;
                    TotalPositions = Inf + 1 + Sup;

                    if (Sup + Inf == 0)
                    {
                        positionsDto.AltaTension.Add("NOM");
                    }
                    else
                    {
                        positionsDto.AltaTension = GetPositions(Iden, Sup, Inf, Inv);
                    }
                }
                else if (general.TapBaan.ComboNumericBc is 3 or 5)
                {
                    Sup = Convert.ToInt32(general.TapBaan.CantidadSupBc);
                    Inf = Convert.ToInt32(general.TapBaan.CantidadInfBc);
                    Nom = Convert.ToInt32(general.TapBaan.NominalBc);
                    Iden = Convert.ToInt32(general.TapBaan.IdentificacionBc);
                    Inv = Convert.ToInt32(general.TapBaan.InvertidoBc) == 1;
                    TotalPositions = Inf + 1 + Sup;

                    if (Sup + Inf == 0)
                    {
                        positionsDto.AltaTension.Add("NOM");
                    }
                    else
                    {
                        positionsDto.AltaTension = GetPositions(Iden, Sup, Inf, Inv);
                    }
                }
                else
                {
                    positionsDto.AltaTension = new();
                }
                #endregion

                #region Baja Tension
                if (general.TapBaan.ComboNumericSc is 2 or 4)
                {
                    Sup = Convert.ToInt32(general.TapBaan.CantidadSupSc);
                    Inf = Convert.ToInt32(general.TapBaan.CantidadInfSc);
                    Nom = Convert.ToInt32(general.TapBaan.NominalSc);
                    Iden = Convert.ToInt32(general.TapBaan.IdentificacionSc);
                    Inv = Convert.ToInt32(general.TapBaan.InvertidoSc) == 1;
                    TotalPositions = Inf + 1 + Sup;

                    if (Sup + Inf == 0)
                    {
                        positionsDto.BajaTension.Add("NOM");
                    }
                    else
                    {
                        positionsDto.BajaTension = GetPositions(Iden, Sup, Inf, Inv);
                    }
                }
                else if (general.TapBaan.ComboNumericBc is 2 or 4)
                {
                    Sup = Convert.ToInt32(general.TapBaan.CantidadSupBc);
                    Inf = Convert.ToInt32(general.TapBaan.CantidadInfBc);
                    Nom = Convert.ToInt32(general.TapBaan.NominalBc);
                    Iden = Convert.ToInt32(general.TapBaan.IdentificacionBc);
                    Inv = Convert.ToInt32(general.TapBaan.InvertidoBc) == 1;
                    TotalPositions = Inf + 1 + Sup;

                    if (TotalPositions == 1)
                    {
                        positionsDto.BajaTension.Add("NOM");
                    }
                    else
                    {
                        positionsDto.BajaTension = GetPositions(Iden, Sup, Inf, Inv);
                    }
                }
                else
                {
                    positionsDto.BajaTension = new();
                }
                #endregion

                #region Terciario
                if (general.TapBaan.ComboNumericSc is 1)
                {
                    Sup = Convert.ToInt32(general.TapBaan.CantidadSupSc);
                    Inf = Convert.ToInt32(general.TapBaan.CantidadInfSc);
                    Nom = Convert.ToInt32(general.TapBaan.NominalSc);
                    Iden = Convert.ToInt32(general.TapBaan.IdentificacionSc);
                    Inv = Convert.ToInt32(general.TapBaan.InvertidoSc) == 1;
                    TotalPositions = Inf + 1 + Sup;

                    if (Sup + Inf == 0)
                    {
                        positionsDto.Terciario.Add("NOM");
                    }
                    else
                    {
                        positionsDto.Terciario = GetPositions(Iden, Sup, Inf, Inv);
                    }
                }
                else if (general.TapBaan.ComboNumericBc is 1)
                {
                    Sup = Convert.ToInt32(general.TapBaan.CantidadSupBc);
                    Inf = Convert.ToInt32(general.TapBaan.CantidadInfBc);
                    Nom = Convert.ToInt32(general.TapBaan.NominalBc);
                    Iden = Convert.ToInt32(general.TapBaan.IdentificacionBc);
                    Inv = Convert.ToInt32(general.TapBaan.InvertidoBc) == 1;
                    TotalPositions = Inf + 1 + Sup;

                    if (TotalPositions == 1)
                    {
                        positionsDto.Terciario.Add("NOM");
                    }
                    else
                    {
                        positionsDto.Terciario = GetPositions(Iden, Sup, Inf, Inv);
                    }
                }
                else
                {
                    positionsDto.Terciario = new();
                }
                #endregion

                #region Nominal Cases

                if (positionsDto.AltaTension.Count == 0)
                {
                    if ((general.VoltageKV.TensionKvAltaTension1 is > 0) || (general.VoltageKV.TensionKvAltaTension3 is > 0))
                    {
                        positionsDto.AltaTension.Add("NOM");
                    }
                }

                if (positionsDto.BajaTension.Count == 0)
                {
                    if ((general.VoltageKV.TensionKvBajaTension1 is > 0) || (general.VoltageKV.TensionKvBajaTension3 is > 0))
                    {
                        positionsDto.BajaTension.Add("NOM");
                    }
                }

                if (positionsDto.Terciario.Count == 0)
                {
                    if ((general.VoltageKV.TensionKvTerciario1 is > 0) || (general.VoltageKV.TensionKvTerciario3 is > 0))
                    {
                        positionsDto.Terciario.Add("NOM");
                    }
                }
                #endregion

                return new JsonResult(new ApiResponse<PositionsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", positionsDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetPositions));
                return new JsonResult(new ApiResponse<PositionsDto>(Enums.EnumsGen.Error, ex.Message, positionsDto));
            }
        }

        [HttpGet("GetPositions")]
        [ProducesResponseType(typeof(ApiResponse<PositionsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetPositions([FromQuery] string noSerie, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            PositionsDto positionsDto = new()
            {
                AltaTension = new(),
                BajaTension = new(),
                Terciario = new()
            };

            if (string.IsNullOrEmpty(noSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayRODReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            string noserieSimple = "";
            if (!string.IsNullOrEmpty(noSerie.Trim()))
                noserieSimple = noSerie.Split('-')[0];

            ApiResponse<List<PlateTensionDto>> tensions = await _reportHttpClientService.GetPlateTension(noSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

            if (tensions is null)
            {
                if (tensions.Code != 1 || tensions.Structure.Count == 0)
                {
                    tensions = await _reportHttpClientService.GetPlateTension(noserieSimple, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));
                    if (tensions is null)
                    {
                        if (tensions.Code != 1 || tensions.Structure.Count == 0)
                        {
                            return new JsonResult(new ApiResponse<PositionsDto>((int)ResponsesID.fallido, "Numero de serie no posee tensiones registradas", null));
                        }
                    }
                }
            }
            else
            {
                positionsDto.AltaTension = tensions.Structure.Where(x => x.TipoTension.ToUpper().Equals("AT")).Select(x => x.Posicion).ToList();
                positionsDto.BajaTension = tensions.Structure.Where(x => x.TipoTension.ToUpper().Equals("BT")).Select(x => x.Posicion).ToList();
                positionsDto.Terciario = tensions.Structure.Where(x => x.TipoTension.ToUpper().Equals("TER")).Select(x => x.Posicion).ToList();

                if (positionsDto.AltaTension.Count() != 0)
                {
                    if (!tensions.Structure.Exists(x => x.N && x.TipoTension.ToUpper().Equals("AT")))
                    {
                        return new JsonResult(new ApiResponse<PositionsDto>((int)ResponsesID.fallido, "No hay posicion Nominal registrada en Alta Tension en el modulo Tension de la Placa", null));
                    }
                    else
                    {
                        positionsDto.ATNom = positionsDto.AltaTension.Count() == 1
                        ? positionsDto.AltaTension.FirstOrDefault()
                        : tensions.Structure.FirstOrDefault(x => x.TipoTension.ToUpper().Equals("AT") && x.N).Posicion;
                    }
                }

                if (positionsDto.BajaTension.Count() != 0)
                {
                    if (!tensions.Structure.Exists(x => x.N && x.TipoTension.ToUpper().Equals("BT")))
                    {
                        return new JsonResult(new ApiResponse<PositionsDto>((int)ResponsesID.fallido, "No hay posicion Nominal registrada en Baja Tension en el modulo Tension de la Placa", null));
                    }
                    else
                    {
                        positionsDto.BTNom = positionsDto.BajaTension.Count() == 1
                        ? positionsDto.BajaTension.FirstOrDefault()
                        : tensions.Structure.FirstOrDefault(x => x.TipoTension.ToUpper().Equals("BT") && x.N).Posicion;
                    }
                }

                if (positionsDto.Terciario.Count() != 0)
                {
                    if (!tensions.Structure.Exists(x => x.N && x.TipoTension.ToUpper().Equals("TER")))
                    {
                        return new JsonResult(new ApiResponse<PositionsDto>((int)ResponsesID.fallido, "No hay posicion Nominal registrada en Terciaria en el modulo Tension de la Placa", null));
                    }
                    else
                    {
                        positionsDto.TerNom = positionsDto.Terciario.Count() == 1
                        ? positionsDto.Terciario.FirstOrDefault()
                        : tensions.Structure.FirstOrDefault(x => x.TipoTension.ToUpper().Equals("TER") && x.N).Posicion;
                    }
                }
            }

            return new JsonResult(new ApiResponse<PositionsDto>((int)ResponsesID.exitoso, "Resultado exitoso", positionsDto));
        }

        [HttpGet("GetConfigurationReportPCI")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayPCIReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportPCI(string NroSerie, string KeyTest, string Lenguage, string WindingMaterial, bool CapRedBaja, bool Autotransformer, bool Monofasico, decimal OverElevation, string TestCapacity, int CantPosPri, int CantPosSec, string PosPi, string PosSec, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayPCIReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            //if (string.IsNullOrEmpty(KeyTest))
            //    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            //long numberColumns = 0;
            long numberColumns = TestCapacity.Split(",").Length;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                if (CantPosPri is not 1)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "Solo se permite seleccionar 1 posicion como primaria", null));

                int numberPositionsSec = CantPosSec;

                if (numberColumns == 3 && numberPositionsSec > 8)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "Solo se permite seleccionar hasta 8 posiciones como secundaria", null));
                }

                if (numberColumns is 1 && numberPositionsSec is > 33)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "Cantidad de posiciones en secundaria no permitidas, solo se permiten como máximo 33", null));

                if (PosSec.ToUpper().Equals(PosPi.ToUpper()))
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "Solo se permite una posición como primaria y otra posición como secundaria", null));
                }

                Task<ApiResponse<long>> getNroTestNextRCT() => _reportHttpClientService.GetNroTestNextPCI(NroSerie, KeyTest, Lenguage, WindingMaterial, CapRedBaja, Autotransformer, Monofasico, OverElevation, PosPi, PosSec, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("PCI", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("PCI", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextRCT();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (Task2.Result.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (Task2.Result.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (Task2.Result.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (Task2.Result.WarrantiesArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de garantías", null));

                if (Task2.Result.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de tapas", null));

                if (Task2.Result.VoltageKV == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (Task2.Result.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                string CapacidadP = TestCapacity;
                string TitPosPrim = "";

                if (Lenguage.ToUpper().Equals("EN"))
                {
                    if (PosPi.ToUpper().Equals("AT"))
                    {
                        TitPosPrim = "HV";
                    }
                    else
                    if (PosPi.ToUpper().Equals("BT"))
                    {
                        TitPosPrim = "LV";
                    }
                    else
                    if (PosPi.ToUpper().Equals("TER"))
                    {
                        TitPosPrim = "TV";
                    }
                    else { }
                }
                else
                {
                    TitPosPrim = PosPi.ToUpper();

                }

                decimal? Frecuency = Task2.Result.GeneralArtifact.Frecuency;

                decimal TitPerdCorr = OverElevation + 20;
                decimal TitPerdTot = OverElevation + 20;
                string TitPosSec = "";

                if (Lenguage.ToUpper().Equals("EN"))
                {
                    if (PosSec.ToUpper().Equals("AT"))
                    {
                        TitPosSec = "HV";
                    }
                    else
                    if (PosSec.ToUpper().Equals("BT"))
                    {
                        TitPosSec = "LV";
                    }
                    else
                    if (PosSec.ToUpper().Equals("TER"))
                    {
                        TitPosSec = "TV";
                    }
                    else { }
                }
                else
                {
                    TitPosSec = PosSec.ToUpper();

                }

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayPCIReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, CapacidadP = CapacidadP, Frecuency = Frecuency, TitPerdCorr = TitPerdCorr, TitPerdTot = TitPerdTot, TitPosPrim = TitPosPrim, TitPosSec = TitPosSec };

                return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportRAD));
                return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportPCE")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayPCEReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportPCE([FromQuery] string NroSerie, string KeyTest, string Lenguage, string PosAT, string PosBT, string PosTER, decimal Beginning, decimal End, decimal Interval, bool Graph, string EnergizedWinding, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayPCEReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 1;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextPCE() => _reportHttpClientService.GetNroTestNextPCE(NroSerie, KeyTest, Lenguage, EnergizedWinding, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("PCE", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("PCE", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextPCE();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.WarrantiesArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de garantías", null));

                if (general.WarrantiesArtifact.Iexc100 is null or 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El número de serie  del aparato no cuenta con valor en el campo llamado: Lexc @ 100 / 110 % Vn(%) en Diseño del Aparato, en la pestaña: Garantías", null));

                if (general.WarrantiesArtifact.Kwfe100 is null or 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El número de serie  del aparato no cuenta con valor en el campo llamado: Pérd. Núcleo @ 100/110% Vn en Diseño del Aparato, en la pestaña: Garantías", null));

                if (general.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de tapas", null));

                if (general.VoltageKV == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                decimal lowest_capacities = Convert.ToDecimal(Task2.Result.CharacteristicsArtifact.Min(car => car.Mvaf1));

                decimal CapMinima = lowest_capacities * 1000;
                string Pos_AT = PosAT;
                string Pos_BT = PosBT;
                string Pos_TER = PosTER;
                PlateTensionDto tension = Task6.Result.Structure.FirstOrDefault(x => x.TipoTension.ToUpper().Equals(EnergizedWinding.ToUpper()) && x.Posicion.Equals(EnergizedWinding is "AT" ? Pos_AT : EnergizedWinding is "BT" ? Pos_BT : Pos_TER));
                if (tension is null)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con tension registrada en la posicion del devanado energizado", null));
                }
                decimal VoltajeBase = tension.Tension;
                decimal? Frecuencia = general.GeneralArtifact.Frecuency;
                decimal? Gar_Perdidas = general.WarrantiesArtifact.Kwfe100;
                decimal? Tol_Gar_Perdidas = general.WarrantiesArtifact.TolerancyKwfe;
                decimal? Gar_Cexcitacion = general.WarrantiesArtifact.Iexc100;

                resultConfiReports = new SettingsToDisplayPCEReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, CapMinima = CapMinima, Frecuencia = Frecuencia, Gar_Cexcitacion = Gar_Cexcitacion, Gar_Perdidas = Gar_Perdidas, Pos_AT = Pos_AT, Pos_BT = Pos_BT, Pos_TER = PosTER, Tol_Gar_Perdidas = Tol_Gar_Perdidas, VoltajeBase = VoltajeBase };

                return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportRAD));
                return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportPLR")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayPLRReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportPLR(string NroSerie, string KeyTest, string Lenguage, decimal Rldnt, decimal NominalVoltage, int AmountOfTensions, int AmountOfTime, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayPLRReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayPLRReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayPLRReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayPLRReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            if (KeyTest.ToUpper().Equals("TIE"))
            {
                if (AmountOfTime < 1)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPLRReportsDto>((int)ResponsesID.fallido, "Favor de proporcionar la cantidad de tiempos a capturar", null));

                if (AmountOfTime.ToString().Length > 31)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "La cantidad de tiempos a capturar debe ser entera mayor a cero y menor o igual a 31", null));
                }
            }
            else
            {

                if (Rldnt < 1)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPLRReportsDto>((int)ResponsesID.fallido, "Favor de proporcionar la reactancia lineal de diseño a tensión nominal", null));

                int[] validationRldnt = CommonMethods.cantDigitsPoint(Convert.ToDouble(Rldnt));

                if (validationRldnt[0] > 6 || validationRldnt[1] > 3)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "La reactancia lineal de diseño a tensión nominal debe ser numérica mayor a cero considerando 6 enteros con 3 decimales", null));
                }

                if (NominalVoltage < 1)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPLRReportsDto>((int)ResponsesID.fallido, "Favor de proporcionar la tensión nominal", null));

                int[] validationNominalVoltage = CommonMethods.cantDigitsPoint(Convert.ToDouble(NominalVoltage));

                if (validationNominalVoltage[0] > 6 || validationNominalVoltage[1] > 3)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "Favor de proporcionar la tensión nominal", null));
                }

                if (AmountOfTensions < 1)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPLRReportsDto>((int)ResponsesID.fallido, "Favor de proporcionar la cantidad de tensiones a capturar", null));

                if (AmountOfTensions.ToString().Length > 12)
                {
                    return new JsonResult(new ApiResponse<SettingsToDisplayRCTReportsDto>((int)ResponsesID.fallido, "La cantidad de tensiones a capturar debe ser entera mayor a cero y menor o igual a 12", null));
                }
            }

            string[] newnoserie = { "" };
            long numberColumns = 1;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));
                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);
                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                Task<ApiResponse<long>> getNroTestNextPLR() => _reportHttpClientService.GetNroTestNextPLR(NroSerie, KeyTest, Lenguage, Rldnt, NominalVoltage, AmountOfTensions, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("PLR", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("PLR", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextPLR();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPLRReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPLRReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = (Task2.Result.CharacteristicsArtifact != null) ? Task2.Result.CharacteristicsArtifact.Count : 0;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayPLRReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result };

                return new JsonResult(new ApiResponse<SettingsToDisplayPLRReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportRAD));
                return new JsonResult(new ApiResponse<SettingsToDisplayPLRReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportPRD")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayPRDReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportPRD([FromQuery] string NroSerie, string KeyTest, string Lenguage, decimal NominalVoltage, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            SettingsToDisplayPRDReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayPRDReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayPRDReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayPRDReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 1;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextPRD() => _reportHttpClientService.GetNroTestNextPRD(NroSerie, KeyTest, Lenguage, NominalVoltage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("PRD", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("PRD", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextPRD();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPRDReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPRDReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = (Task2.Result.CharacteristicsArtifact != null) ? Task2.Result.CharacteristicsArtifact.Count : 0;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayPRDReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result };

                return new JsonResult(new ApiResponse<SettingsToDisplayPRDReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportRAD));
                return new JsonResult(new ApiResponse<SettingsToDisplayPRDReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportPEE")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayPEEReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportPEE([FromQuery] string NroSerie, string KeyTest, string Lenguage, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            SettingsToDisplayPEEReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayPEEReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayPEEReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayPEEReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 1;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextPEE() => _reportHttpClientService.GetNroTestNextPEE(NroSerie, KeyTest, Lenguage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("PEE", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("PEE", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextPEE();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPEEReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPEEReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));
                if (general.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.WarrantiesArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de garantías", null));

                if (general.VoltageKV == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCEReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = (Task2.Result.CharacteristicsArtifact != null) ? Task2.Result.CharacteristicsArtifact.Count : 0;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayPEEReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result };

                return new JsonResult(new ApiResponse<SettingsToDisplayPEEReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportRAD));
                return new JsonResult(new ApiResponse<SettingsToDisplayPEEReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportISZ")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayISZReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportISZ([FromQuery] string NroSerie, string KeyTest, string Lenguage,
           decimal DegreesCor, string PosAT, string PosBT, string PosTER, int QtyNeutral, string MaterialWinding, string tokenSesion, string ImpedanceGar, string WindingEnergized)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayISZReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = QtyNeutral;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;
                bool esImpedancia = decimal.TryParse(ImpedanceGar, out decimal impedancia);
                Task<ApiResponse<long>> getNroTestNextISZ() => _reportHttpClientService.GetNroTestNextISZ(NroSerie, KeyTest, Lenguage, DegreesCor, PosAT, PosBT, PosTER, WindingEnergized, QtyNeutral, esImpedancia ? impedancia : 0, MaterialWinding, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("ISZ", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("ISZ", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextISZ();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { /*Task1,*/ Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.WarrantiesArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de garantías", null));

                if (general.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de tapas", null));

                if (general.Derivations == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                if (Task6.Result.Structure == null || Task6.Result.Structure.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de las tensiones de la placa", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                decimal lowest_capacities = Convert.ToDecimal(Task2.Result.CharacteristicsArtifact.Min(car => car.Mvaf1));

                string heaadersDevanados = string.Empty;
                if (KeyTest != "ABT")
                {

                    if (WindingEnergized.ToUpper().Equals("AT"))
                    {
                        if (Lenguage.ToUpper().Equals("EN"))
                        {
                            WindingEnergized = "HV";
                        }
                    }
                    else if (WindingEnergized.ToUpper().Equals("BT"))
                    {

                        if (Lenguage.ToUpper().Equals("EN"))
                        {
                            WindingEnergized = "LV";
                        }
                    }

                    else if (WindingEnergized.ToUpper().Equals("TER"))
                    {

                        if (Lenguage.ToUpper().Equals("EN"))
                        {
                            WindingEnergized = "TV";
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    string[] devanados = WindingEnergized.Split(",");

                    foreach (string item in devanados)
                    {
                        if (item.ToUpper().Equals("AT"))
                        {
                            if (Lenguage.ToUpper().Equals("EN"))
                            {
                                heaadersDevanados += ",HV";
                            }
                            else
                            {
                                heaadersDevanados += ",AT";
                            }
                        }
                        else if (item.ToUpper().Equals("BT"))
                        {

                            if (Lenguage.ToUpper().Equals("EN"))
                            {
                                heaadersDevanados += ",LV";
                            }
                            else
                            {
                                heaadersDevanados += ",BT";
                            }
                        }

                        else if (item.ToUpper().Equals("TER"))
                        {

                            if (Lenguage.ToUpper().Equals("EN"))
                            {
                                heaadersDevanados += ",TV";
                            }
                            else
                            {
                                heaadersDevanados += ",Ter";
                            }
                        }
                    }

                    heaadersDevanados = heaadersDevanados.Remove(0, 1);
                }

                resultConfiReports = new SettingsToDisplayISZReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, BaseCapacity = lowest_capacities, XXDegreesCorrection = DegreesCor, XXWindingEnergized = KeyTest == "ABT" ? heaadersDevanados : WindingEnergized };

                return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportISZ));
                return new JsonResult(new ApiResponse<SettingsToDisplayISZReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportRYE")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayRYEReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportRYE([FromQuery] string NroSerie, string KeyTest, string Lenguage,
            int NoConnectionsAT, int NoConnectionsBT, int NoConnectionsTER, decimal VoltageAT, decimal VoltageBT, decimal VoltageTER, string CoolingType, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayRYEReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayRYEReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayRYEReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayRYEReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 1;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextRYE() => _reportHttpClientService.GetNroTestNextRYE(NroSerie, KeyTest, Lenguage, NoConnectionsAT, NoConnectionsBT, NoConnectionsTER, VoltageAT, VoltageBT, VoltageTER, CoolingType, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("RYE", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("RYE", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextRYE();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                List<Task> lote1 = new() { Task1, Task3, Task4 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRYEReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRYEReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRYEReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRYEReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRYEReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.WarrantiesArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRYEReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de garantías", null));

                if (general.VoltageKV == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRYEReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;
                string CoolingCapacity = "";

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {

                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                string[] TypeEnfr = { "" };

                if (!string.IsNullOrEmpty(CoolingType))
                    TypeEnfr = CoolingType.Split('-');

                CoolingCapacity = Math.Round((Task2.Result.CharacteristicsArtifact
                    .Where(x => x.CoolingType.ToUpper().Equals(TypeEnfr[0])
                        && x.OverElevation == Convert.ToDecimal(TypeEnfr[1]))
                    .FirstOrDefault()?.Mvaf1 * 1000m) ?? 0m, 0).ToString();

                resultConfiReports = new SettingsToDisplayRYEReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, CoolingType = CoolingType, CoolingCapacity = CoolingCapacity };

                return new JsonResult(new ApiResponse<SettingsToDisplayRYEReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportISZ));
                return new JsonResult(new ApiResponse<SettingsToDisplayRYEReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportPIM")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayPIMReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportPIM([FromQuery] string NroSerie, string KeyTest, string Lenguage,
          string ConnectionAt, string ConnectionBt, string ApplyLow, string tokenSesion)
        {

            if (string.IsNullOrEmpty(ConnectionAt))
            {
                ConnectionAt = "";
            }
            if (string.IsNullOrEmpty(ConnectionBt))
            {
                ConnectionBt = "";
            }

            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            SettingsToDisplayPIMReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayPIMReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayPIMReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayPIMReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 2;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {
                string connection = ConnectionAt != string.Empty && ConnectionBt != string.Empty ? "Todas" :
                    ConnectionAt != string.Empty ? "Alta Tensión" : "Baja Tensión";

                string voltageLevel = "1000";

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextPIM() => _reportHttpClientService.GetNroTestNextPIM(NroSerie, KeyTest, Lenguage, connection, ApplyLow, voltageLevel, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("PIM", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("PIM", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextPIM();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                List<Task> lote1 = new() { Task1, Task3, Task4 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPIMReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPIMReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPIMReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPIMReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.VoltageKV == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPIMReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                List<string> terminalsAT = new();
                List<string> terminalsBT = new();
                List<string> terminals = new();

                if (Task2.Result.Derivations.IdConexionEquivalente is not 0)
                {
                    if (Task2.Result.Derivations.ConexionEquivalente.ToUpper().Contains("DELTA"))
                    {
                        terminalsAT.Add("H1");
                        terminalsAT.Add("H2");
                        terminalsAT.Add("H3");
                    }

                    if (Task2.Result.Derivations.ConexionEquivalente.ToUpper().Contains("ESTRELLA") || Task2.Result.Derivations.ConexionEquivalente.ToUpper().Contains("WYE"))
                    {
                        terminalsAT.Add("H1");
                        terminalsAT.Add("H2");
                        terminalsAT.Add("H3");
                        //terminalsAT.Add("H0");
                    }
                }

                if (Task2.Result.Derivations.IdConexionEquivalente2 is not 0)
                {
                    if (Task2.Result.Derivations.ConexionEquivalente_2.ToUpper().Contains("DELTA"))
                    {
                        terminalsBT.Add("X1");
                        terminalsBT.Add("X2");
                        terminalsBT.Add("X3");
                    }

                    if (Task2.Result.Derivations.ConexionEquivalente_2.ToUpper().Contains("ESTRELLA") || Task2.Result.Derivations.ConexionEquivalente_2.ToUpper().Contains("WYE"))
                    {
                        terminalsBT.Add("X1");
                        terminalsBT.Add("X2");
                        terminalsBT.Add("X3");
                        //terminalsBT.Add("X0");
                    }
                }

                if (ConnectionAt != string.Empty)
                {
                    foreach (string conAt in ConnectionAt.Split(','))
                    {
                        if (conAt.Split('-').Length <= 1)
                        {
                            terminals.AddRange(terminalsAT.Select(x => $"{x}"));
                        }
                        else
                        {
                            terminals.AddRange(terminalsAT.Select(x => $"{x} - {Convert.ToDecimal(conAt.Split('-')[1]):##0.000}"));
                        }
                    }
                }

                if (ConnectionBt != string.Empty)
                {
                    foreach (string conBt in ConnectionBt.Split(','))
                    {
                        if (conBt.Split('-').Length <= 1)
                        {
                            terminals.AddRange(terminalsBT.Select(x => $"{x}"));
                        }
                        else
                        {
                            terminals.AddRange(terminalsBT.Select(x => $"{x} - {Convert.ToDecimal(conBt.Split('-')[1]):##0.000}"));
                        }
                    }
                }

                resultConfiReports = new SettingsToDisplayPIMReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, Terminals = terminals };

                return new JsonResult(new ApiResponse<SettingsToDisplayPIMReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportPIM));
                return new JsonResult(new ApiResponse<SettingsToDisplayPIMReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportPIR")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayPIRReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportPIR([FromQuery] string NroSerie, string KeyTest, string Lenguage,
        string ConnectionAt, string ConnectionBt, string ConnectionTer, string IncludesTertiary, string tokenSesion)
        {
            if (string.IsNullOrEmpty(ConnectionAt))
            {
                ConnectionAt = "";
            }
            if (string.IsNullOrEmpty(ConnectionBt))
            {
                ConnectionBt = "";
            }
            if (string.IsNullOrEmpty(ConnectionTer))
            {
                ConnectionTer = "";
            }

            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayPIRReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayPIRReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayPIRReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayPIRReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 2;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {
                string connection = ConnectionAt != string.Empty && ConnectionBt != string.Empty && ConnectionTer != string.Empty ? "Todas" :
                    ConnectionAt != string.Empty && ConnectionBt != string.Empty ? "AT&BT" :
                    ConnectionAt != string.Empty && ConnectionTer != string.Empty ? "AT&TER" :
                    ConnectionBt != string.Empty && ConnectionTer != string.Empty ? "BT&TER" :
                    ConnectionAt != string.Empty ? "AT" :
                    ConnectionBt != string.Empty ? "BT" : "TER";

                string voltageLevel = "1000";

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextPIR() => _reportHttpClientService.GetNroTestNextPIR(NroSerie, KeyTest, Lenguage, connection, IncludesTertiary, voltageLevel, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("PIR", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("PIR", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextPIR();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                List<Task> lote1 = new() { Task1, Task3, Task4 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPIRReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPIRReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPIRReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPIRReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.VoltageKV == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPIRReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayPIRReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result };

                return new JsonResult(new ApiResponse<SettingsToDisplayPIRReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportPIM));
                return new JsonResult(new ApiResponse<SettingsToDisplayPIRReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportTAP")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayTAPReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportTAP([FromQuery] string NroSerie, string KeyTest, string Lenguage,
        string UnitType, int NoConnectionAT, int NoConnectionBT, int NoConnectionTER, string IdCapAT, string IdCapBT, string IdCapTer, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            SettingsToDisplayTAPReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayTAPReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayTAPReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayTAPReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 1;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextTAP() => _reportHttpClientService.GetNroTestNextTAP(NroSerie, KeyTest, Lenguage, UnitType, NoConnectionAT, NoConnectionBT, NoConnectionTER, IdCapAT, IdCapBT, IdCapTer, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("TAP", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("TAP", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextTAP();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTAPReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTAPReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTAPReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTAPReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTAPReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.Derivations == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTAPReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                TAPBodyDto Body = new();
                List<TAPBodyDto> Bodys = new();

                // AT y BT
                if (NoConnectionAT > 0 && NoConnectionBT > 0)
                {
                    Bodys.Add(new TAPBodyDto() { WindingEnergized = "H", WindingGrounded = "X", LevelVoltage = Convert.ToDecimal(Task2.Result.VoltageKV.TensionKvAltaTension1) });
                    if (NoConnectionAT == 2)
                    {
                        Bodys.Add(new TAPBodyDto() { WindingEnergized = "H", WindingGrounded = "X", LevelVoltage = Convert.ToDecimal(Task2.Result.VoltageKV.TensionKvAltaTension3) });
                    }
                }

                // AT y TER
                if (NoConnectionAT > 0 && NoConnectionTER > 0)
                {
                    Bodys.Add(new TAPBodyDto() { WindingEnergized = "H", WindingGrounded = "Y", LevelVoltage = Convert.ToDecimal(Task2.Result.VoltageKV.TensionKvAltaTension1) });
                    if (NoConnectionAT == 2)
                    {
                        Bodys.Add(new TAPBodyDto() { WindingEnergized = "H", WindingGrounded = "Y", LevelVoltage = Convert.ToDecimal(Task2.Result.VoltageKV.TensionKvAltaTension3) });
                    }
                }

                // BT y AT
                /*if (NoConnectionBT > 0 && NoConnectionAT > 0)
                {
                    Bodys.Add(new TAPBodyDto() { WindingEnergized = "X", WindingGrounded = "H", LevelVoltage = Convert.ToDecimal(Task2.Result.VoltageKV.TensionKvBajaTension1) });
                    if (NoConnectionBT == 2)
                    {
                        Bodys.Add(new TAPBodyDto() { WindingEnergized = "X", WindingGrounded = "H", LevelVoltage = Convert.ToDecimal(Task2.Result.VoltageKV.TensionKvBajaTension3) });
                    }
                }*/

                // BT y TER
                if (NoConnectionBT > 0 && NoConnectionTER > 0)
                {
                    Bodys.Add(new TAPBodyDto() { WindingEnergized = "X", WindingGrounded = "Y", LevelVoltage = Convert.ToDecimal(Task2.Result.VoltageKV.TensionKvBajaTension1) });
                    if (NoConnectionBT == 2)
                    {
                        Bodys.Add(new TAPBodyDto() { WindingEnergized = "X", WindingGrounded = "Y", LevelVoltage = Convert.ToDecimal(Task2.Result.VoltageKV.TensionKvBajaTension3) });
                    }
                }

                // TER y AT
                if (NoConnectionTER > 0 && NoConnectionAT > 0)
                {
                    Bodys.Add(new TAPBodyDto() { WindingEnergized = "Y", WindingGrounded = "H", LevelVoltage = Convert.ToDecimal(Task2.Result.VoltageKV.TensionKvTerciario1) });
                    if (NoConnectionTER == 2)
                    {
                        Bodys.Add(new TAPBodyDto() { WindingEnergized = "Y", WindingGrounded = "H", LevelVoltage = Convert.ToDecimal(Task2.Result.VoltageKV.TensionKvTerciario3) });
                    }
                }

                // TER y BT
                if (NoConnectionTER > 0 && NoConnectionBT > 0)
                {
                    Bodys.Add(new TAPBodyDto() { WindingEnergized = "Y", WindingGrounded = "X", LevelVoltage = Convert.ToDecimal(Task2.Result.VoltageKV.TensionKvTerciario1) });
                    if (NoConnectionTER == 2)
                    {
                        Bodys.Add(new TAPBodyDto() { WindingEnergized = "Y", WindingGrounded = "X", LevelVoltage = Convert.ToDecimal(Task2.Result.VoltageKV.TensionKvTerciario3) });
                    }
                }

                resultConfiReports = new SettingsToDisplayTAPReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, FrequencyTest = Task2.Result.GeneralArtifact.Frecuency, TAPBodys = Bodys };

                return new JsonResult(new ApiResponse<SettingsToDisplayTAPReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportTAP));
                return new JsonResult(new ApiResponse<SettingsToDisplayTAPReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportTIN")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayTINReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportTIN([FromQuery] string NroSerie, string KeyTest, string Lenguage,
        string Connection, decimal Voltage, string tokenSesion)
        {

            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayTINReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayTINReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayTINReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayTAPReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 3;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextTIN() => _reportHttpClientService.GetNroTestNextTIN(NroSerie, KeyTest, Lenguage, Connection, Voltage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("TIN", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("TIN", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextTIN();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTINReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTINReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTINReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTINReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTINReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.Derivations == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTINReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                string TitConexion = "";

                if (Connection.ToUpper().Equals("SERIE"))
                {
                    TitConexion = KeyTest.ToUpper().Equals("ES") ? "Conexión Serie" : "Series Connection";
                }
                else if (Connection.ToUpper().Equals("PARALELO"))
                {
                    TitConexion = KeyTest.ToUpper().Equals("ES") ? "Conexión Paralelo" : "Parallel Connection";
                }
                else if (!Connection.ToUpper().Equals("NO APLICA"))
                {
                    TitConexion = KeyTest.ToUpper().Equals("ES") ? "Conexión de " + Voltage + " KV" : Voltage + " kV connection";
                }

                resultConfiReports = new SettingsToDisplayTINReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, TitConexion = TitConexion };

                return new JsonResult(new ApiResponse<SettingsToDisplayTINReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportTIN));
                return new JsonResult(new ApiResponse<SettingsToDisplayTINReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportCEM")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayCEMReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportCEM([FromQuery] string NroSerie, string KeyTest, string Lenguage,
       string IdPosPrimary, string PosPrimary, string IdPosSecundary, string PosSecundary, bool StatusAllPosSec, decimal TestsVoltage, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            SettingsToDisplayCEMReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayCEMReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayCEMReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayCEMReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 3;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                if (StatusAllPosSec)
                {
                    PosSecundary = "Todas";
                }

                Task<ApiResponse<long>> getNroTestNextCEM() => _reportHttpClientService.GetNroTestNextCEM(NroSerie, KeyTest, Lenguage, IdPosPrimary, PosPrimary, IdPosSecundary, PosSecundary, TestsVoltage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("CEM", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("CEM", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextCEM();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayCEMReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayCEMReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayCEMReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayCEMReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayCEMReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.Derivations == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayCEMReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));
                if (general.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                Task<ApiResponse<List<ColumnTitleCEMReportsDto>>> getTitleColumnsCEMPosSec() => _reportHttpClientService.GetTitleColumnsCEM(Convert.ToDecimal(general.GeneralArtifact.TypeTrafoId), Lenguage, IdPosPrimary, IdPosSecundary, newnoserie.Length > 1 ? newnoserie[0] : NroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<List<ColumnTitleCEMReportsDto>>> TaskCemPosSec = getTitleColumnsCEMPosSec();

                List<Task> TasksColumnsTitleCEM = new() { TaskCemPosSec };
                await Task.WhenAny(TasksColumnsTitleCEM).Result;

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayCEMReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, SecondaryPositions = TaskCemPosSec.Result.Structure, MessageInformation = TaskCemPosSec.Result.Description, CodeInformation = TaskCemPosSec.Result.Code };

                return new JsonResult(new ApiResponse<SettingsToDisplayCEMReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportCEM));
                return new JsonResult(new ApiResponse<SettingsToDisplayCEMReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportCGD")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayCGDReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportCGD([FromQuery] string NroSerie, string KeyTest, string Lenguage, string TypeOil, int hour1, int hour2, int hour3, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            SettingsToDisplayCGDReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayCGDReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayCGDReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayCGDReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = KeyTest is "DDT" ? new List<int> { hour1, hour2, hour3 }.Count(x => x > 0) : 1;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {
                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextCGD() => _reportHttpClientService.GetNroTestNextCGD(NroSerie, KeyTest, Lenguage, TypeOil, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("CGD", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("CGD", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextCGD();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                if (numberColumns == 1)
                {
                    Task3.Result.Structure = Task3.Result.Structure.Where(item => item.Seccion == 1).ToList();
                }
                else if (numberColumns == 2) { Task3.Result.Structure = Task3.Result.Structure.Where(item => item.Seccion is 1 or 2).ToList(); }

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayCGDReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayCGDReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayCGDReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayCGDReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayCGDReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayCGDReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                Task<ApiResponse<List<ContGasCGDDto>>> getInfoContGasCGD() => _reportHttpClientService.GetInfoContGasCGD("CGD", KeyTest, TypeOil, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splconfiguration));

                Task<ApiResponse<List<ContGasCGDDto>>> TaskInfoContGasCGD = getInfoContGasCGD();

                List<Task> InfoContGasCGD = new() { TaskInfoContGasCGD };
                await Task.WhenAny(InfoContGasCGD).Result;

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayCGDReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result };

                return new JsonResult(new ApiResponse<SettingsToDisplayCGDReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportCGD));
                return new JsonResult(new ApiResponse<SettingsToDisplayCGDReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportTDP")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayTDPReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportTDP([FromQuery] string NroSerie, string KeyTest, string Lenguage, int NoCycles, int TotalTime, int Interval, decimal TimeLevel, decimal OutputLevel, int DescMayPc, int DescMayMv, int IncMaxPc, string VoltageLevels, string MeasurementType, string TerminalsTest, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayTDPReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayTDPReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayTDPReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayTDPReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 0;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {
                if (MeasurementType.ToUpper().Equals("MICROVOLTS") || MeasurementType.ToUpper().Equals("PICOLUMNS"))
                {
                    numberColumns = 3;
                }
                else
                {
                    if (MeasurementType.ToUpper().Equals("AMBOS")) { numberColumns = 6; }
                }

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextTDP() => _reportHttpClientService.GetNroTestNextTDP(NroSerie, KeyTest, Lenguage, NoCycles, TotalTime, Interval, TimeLevel, OutputLevel, DescMayPc, DescMayMv, IncMaxPc, VoltageLevels, MeasurementType, TerminalsTest, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("TDP", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("TDP", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextTDP();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTDPReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTDPReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTDPReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTDPReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayTDPReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                string TitTerminal1 = string.Empty;
                string TitTerminal2 = string.Empty;
                string TitTerminal3 = string.Empty;
                string TitTerminal4 = string.Empty;
                string TitTerminal5 = string.Empty;
                string TitTerminal6 = string.Empty;

                string UMed1 = string.Empty;
                string UMed2 = string.Empty;
                string UMed3 = string.Empty;
                string UMed4 = string.Empty;
                string UMed5 = string.Empty;
                string UMed6 = string.Empty;

                string[] terminales = TerminalsTest.Split(",");

                foreach (string item in terminales)
                {
                    int pos = terminales.ToList().IndexOf(item) + 1;

                    if (pos == 1)
                    {
                        TitTerminal1 = item;

                        getUnits(MeasurementType, ref UMed1);

                    }
                    else if (pos == 2)
                    {
                        TitTerminal2 = item;
                        getUnits(MeasurementType, ref UMed2);
                    }
                    else if (pos == 3)
                    {
                        TitTerminal3 = item;
                        getUnits(MeasurementType, ref UMed3);

                    }
                    else if (pos == 4)
                    {
                        TitTerminal4 = item;
                        getUnits(MeasurementType, ref UMed4);
                    }
                    else if (pos == 5)
                    {
                        TitTerminal5 = item;

                        getUnits(MeasurementType, ref UMed5);
                    }
                    else if (pos == 6)
                    {
                        TitTerminal6 = item;
                        getUnits(MeasurementType, ref UMed6);
                    }
                }

                List<string> Times = new();
                List<string> Voltages = new();

                Times.Add("0");
                Times.Add(Interval.ToString());
                decimal value3 = NoCycles / Convert.ToDecimal(general.GeneralArtifact.Frecuency);
                Times.Add(Math.Round(value3, 0) + " Sec.");

                for (int i = Interval; i <= TotalTime; i += Interval)
                {
                    Times.Add(i + " Min.");
                }

                Voltages.Add("0");
                Voltages.Add(TimeLevel.ToString());
                Voltages.Add(OutputLevel.ToString());

                for (int i = 3; i < Times.Count; i++)
                {
                    Voltages.Add(TimeLevel.ToString());
                }

                resultConfiReports = new SettingsToDisplayTDPReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, VoltageLevel = VoltageLevels, Frequency = general.GeneralArtifact.Frecuency, TitTerminal1 = TitTerminal1, TitTerminal2 = TitTerminal2, TitTerminal3 = TitTerminal3, TitTerminal4 = TitTerminal4, TitTerminal5 = TitTerminal5, TitTerminal6 = TitTerminal6, UMed1 = UMed1, UMed2 = UMed2, UMed3 = UMed3, UMed4 = UMed4, UMed5 = UMed5, UMed6 = UMed6, Times = Times, Voltages = Voltages };

                return new JsonResult(new ApiResponse<SettingsToDisplayTDPReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportTDP));
                return new JsonResult(new ApiResponse<SettingsToDisplayTDPReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportARF")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayARFReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportARF([FromQuery] string NroSerie, string KeyTest, string Lenguage, string Team, string Tertiary2Low, string TertiaryDisp, string LevelsVoltage, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayARFReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayARFReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayARFReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayARFReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 0;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextARF() => _reportHttpClientService.GetNroTestNextARF(NroSerie, KeyTest, Lenguage, Team, Tertiary2Low, TertiaryDisp, LevelsVoltage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("ARF", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                numberColumns = Tertiary2Low.ToUpper().Equals("2B".ToUpper()) || Tertiary2Low.ToUpper().Equals("CT".ToUpper()) ? 3 : 2;

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("ARF", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextARF();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayARFReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayARFReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayARFReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayARFReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayARFReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.Derivations == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayARFReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayARFReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayARFReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, VoltageLevel = LevelsVoltage, Team = Team, };

                return new JsonResult(new ApiResponse<SettingsToDisplayARFReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportARF));
                return new JsonResult(new ApiResponse<SettingsToDisplayARFReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportRDD")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayRDDReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportRDD([FromQuery] string NroSerie, string KeyTest, string Lenguage, string ConfigWinding, string Connection, decimal PorcZ, decimal PorcJx, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            SettingsToDisplayRDDReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayRDDReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayRDDReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayRDDReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 3;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextRDD() => _reportHttpClientService.GetNroTestNextRDD(NroSerie, KeyTest, Lenguage, ConfigWinding, Connection, PorcZ, PorcJx, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("RDD", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("RDD", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextRDD();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDDReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDDReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDDReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDDReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDDReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.Derivations == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDDReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDDReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }
                string TitConexion = Connection.ToUpper().Equals("SERIE")
                    ? KeyTest.ToUpper().Equals("ES") ? "Conexión Serie" : "Series Connection"
                    : Connection.ToUpper().Equals("PARALELO") ? KeyTest.ToUpper().Equals("ES") ? "Conexión Paralelo" : "Parallel Connection" : "";
                string Phase = ConfigWinding.ToUpper().Equals("DELTA - ESTRELLA")
                    ? "H1 - H3,H2 - H1,H3 - H2"
                    : ConfigWinding.ToUpper().Equals("ESTRELLA - DELTA") ? "H1 - H0,H2 - H0,H3 - H0" : "H1 - H0X0,H2 - H0X0,H3 - H0X0";
                resultConfiReports = new SettingsToDisplayRDDReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, TitConexion = TitConexion, Phase = Phase };

                return new JsonResult(new ApiResponse<SettingsToDisplayRDDReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportRDD));
                return new JsonResult(new ApiResponse<SettingsToDisplayRDDReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportIND")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayINDReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportIND([FromQuery] string NroSerie, string KeyTest, string Lenguage, string TCBuyers, string tokenSesion)
        {

            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayINDReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayINDReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayINDReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayINDReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 0;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextIND() => _reportHttpClientService.GetNroTestNextIND(NroSerie, KeyTest, Lenguage, TCBuyers, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                numberColumns = KeyTest.ToUpper().Equals("CTC") ? TCBuyers.ToUpper().Equals("NO") ? 1 : 2 : 1;

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("IND", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("IND", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextIND();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                List<Task> lote1 = new() { Task1, Task3, Task4 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayINDReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayINDReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayINDReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayINDReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayINDReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result };

                return new JsonResult(new ApiResponse<SettingsToDisplayINDReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportIND));
                return new JsonResult(new ApiResponse<SettingsToDisplayINDReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportFPA")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayFPAReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportFPA([FromQuery] string NroSerie, string KeyTest, string Lenguage, string OilType, int nroCol, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            SettingsToDisplayFPAReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayFPAReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayFPAReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayFPAReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextFPA() => _reportHttpClientService.GetNroTestNextFPA(NroSerie, KeyTest, Lenguage, OilType, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<CGDTestsGeneralDto>> getInfoCGD() => _reportHttpClientService.GetInfoCGD(NroSerie, KeyTest, true, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<List<ContGasCGDDto>>> getInfoContGasCGD() => _reportHttpClientService.GetInfoContGasCGD("CGD", KeyTest, Task2.Result.GeneralArtifact.OilType, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splconfiguration));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("FPA", KeyTest, nroCol, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("FPA", KeyTest, Lenguage, nroCol, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextFPA();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<CGDTestsGeneralDto>> TaskInfoCGD = getInfoCGD();

                Task<ApiResponse<List<ContGasCGDDto>>> TaskInfoContGasCGD = getInfoContGasCGD();

                List<Task> lote1 = new() { Task1, Task3, Task4, TaskInfoCGD, TaskInfoContGasCGD };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());
                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPAReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPAReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPAReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayFPAReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                decimal? medida = TaskInfoCGD.Result.Structure is null
                    ? 0
                    : TaskInfoCGD.Result.Structure.CGDTests?.FirstOrDefault()?.CGDTestsDetails?.FirstOrDefault(x => x.Key.ToUpper() == Enums.CGDKeys.PorcGasContent)?.Value1;

                decimal limite = TaskInfoContGasCGD.Result.Structure.Count == 0
                    ? 0
                    : TaskInfoContGasCGD.Result.Structure.FirstOrDefault().LimiteMax;

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayFPAReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, OilType = Task2.Result.GeneralArtifact.OilBrand + " (" + Task2.Result.GeneralArtifact.OilType + ")", Measurement = medida, PorcMaximumLimit = limite };

                return new JsonResult(new ApiResponse<SettingsToDisplayFPAReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportFPA));
                return new JsonResult(new ApiResponse<SettingsToDisplayFPAReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportBPC")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayBPCReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportBPC([FromQuery] string NroSerie, string KeyTest, string Lenguage, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayBPCReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayBPCReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayBPCReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayBPCReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 1;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextBPC() => _reportHttpClientService.GetNroTestNextBPC(NroSerie, KeyTest, Lenguage, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("BPC", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("BPC", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextBPC();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                List<Task> lote1 = new() { Task1, Task3, Task4 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayBPCReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayBPCReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayBPCReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayBPCReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                StringBuilder newCapacitiesSB = new();
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        _ = newCapacitiesSB.AppendFormat(string.Format("{0:0.000}", obj.Mvaf1));
                    }
                    else
                    {
                        _ = newCapacitiesSB.AppendFormat("/" + string.Format("{0:0.000}", obj.Mvaf1));
                    }
                }

                string newCapacities = FormatFieldCapacity(newCapacitiesSB.ToString());

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        if (!stringSplit[i].Contains('.'))
                        {
                            stringSplit[i] = i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/";
                        }
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                resultConfiReports = new SettingsToDisplayBPCReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result };

                return new JsonResult(new ApiResponse<SettingsToDisplayBPCReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportBPC));
                return new JsonResult(new ApiResponse<SettingsToDisplayBPCReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportNRA")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayNRAReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportNRA([FromQuery] string NroSerie, string KeyTest, string Language, bool loadData, string selectloadData, string Laboratory, string Rule, string Feeding, decimal FeedingKVValue, string CoolingType, int AmountMeasureExistsOctaveInfo, string DataDate, int cantidadMaximadeMediciones, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayNRAReportsDto resultConfiReports = new();
            List<MatrixOneDto> matrixBaseDto = new();
            List<MatrixOneDto> matrixOneDto = new();
            List<MatrixTwoDto> matrixTwoDto = new();
            List<MatrixThreeDto> matrixThreeDto = new();
            List<MatrixThree1323HDto> MatrixThree12HDto = new();
            List<MatrixThree1323HDto> MatrixThree13HDto = new();
            List<MatrixThree1323HDto> MatrixThree23HDto = new();
            List<MatrixThreeDto> matrixThreeAnt = new();
            List<MatrixThreeDto> matrixThreeDes = new();
            List<MatrixThreeDto> matrixThreeCoolingType = new();

            MatrixThreeDto AmbTrans = null;
            MatrixThreeDto AmbProm = null;

            MatrixTwoPromDto objetoAmbienteAntDesp = null;
            MatrixTwoPromDto objetoAmbienteSelect = null;

            decimal diferencia = 0;
            decimal factorCoreccion = 0;

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayNRAReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayNRAReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Language))
                return new JsonResult(new ApiResponse<SettingsToDisplayNRAReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 0;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                Task<ApiResponse<long>> getNroTestNextNRA() => _reportHttpClientService.GetNroTestNextNRA(NroSerie, KeyTest, Language, Laboratory, Rule, Feeding, CoolingType, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                if (loadData)
                {
                    numberColumns = AmountMeasureExistsOctaveInfo;
                }
                else
                {
                    if (selectloadData.ToUpper().Equals("4 MEDICIONES ANTES Y DESPUÉS DE AMBIENTE"))
                    {
                        numberColumns = 4;
                    }
                    else if (selectloadData.ToUpper().Equals("10 MEDICIONES ANTES Y DESPUÉS DE AMBIENTE"))
                    {
                        numberColumns = 10;
                    }
                }

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("NRA", KeyTest, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("NRA", KeyTest, Language, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextNRA();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                List<Task> lote1 = new() { Task1, Task3, Task4 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayRDTReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (Task2.Result.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.WarrantiesArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de garantías", null));

                if (general.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de tapas", null));

                if (general.VoltageKV == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                if (general.TapBaan == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "El aparato no cuenta con la información de tapas que es requerida, favor de corregirlo", null));

                int count = 1;

                if (loadData)
                {

                    ApiResponse<List<InformationOctavesDto>> infoOctaves = await _reportHttpClientService.GetInformationOctaves(NroSerie, null, DataDate, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splconfiguration));

                    string[] requiredTypes = new string[] { "ANT", "DES", CoolingType };

                    infoOctaves.Structure.RemoveAll(e => !requiredTypes.Contains(e.TipoInfo));

                    if (infoOctaves.Code == -1)
                    {
                        return new JsonResult(new ApiResponse<SettingsToDisplayPCIReportsDto>((int)ResponsesID.fallido, "Falló obteniendo información de octavas " + infoOctaves.Description, null));
                    }

                    foreach (InformationOctavesDto item in infoOctaves.Structure.OrderBy(x => TimeSpan.Parse(x.Hora)))
                    {
                        int pos = infoOctaves.Structure.IndexOf(item) + 1;

                        List<double> list = new()
                        {
                            (double)Math.Round(item.D16, 7),
                            (double)Math.Round(item.D20, 7),
                            (double)Math.Round(item.D25, 7),
                            (double)Math.Round(item.D315, 7),
                            (double)Math.Round(item.D40, 7),
                            (double)Math.Round(item.D50, 7),
                            (double)Math.Round(item.D63, 7),
                            (double)Math.Round(item.D80, 7),
                            (double)Math.Round(item.D100, 7),
                            (double)Math.Round(item.D125, 7),
                            (double)Math.Round(item.D160, 7),
                            (double)Math.Round(item.D200, 7),
                            (double)Math.Round(item.D250, 7),
                            (double)Math.Round(item.D3151, 7),
                            (double)Math.Round(item.D400, 7),
                            (double)Math.Round(item.D500, 7),
                            (double)Math.Round(item.D630, 7),
                            (double)Math.Round(item.D800, 7),
                            (double)Math.Round(item.D1000, 7),
                            (double)Math.Round(item.D1250, 7),
                            (double)Math.Round(item.D1600, 7),
                            (double)Math.Round(item.D2000, 7),
                            (double)Math.Round(item.D2500, 7),
                            (double)Math.Round(item.D3150, 7),
                            (double)Math.Round(item.D4000, 7),
                            (double)Math.Round(item.D5000, 7),
                            (double)Math.Round(item.D6300, 7),
                            (double)Math.Round(item.D8000, 7),
                            (double)Math.Round(item.D10000, 7),
                        };

                        matrixBaseDto.Add(new MatrixOneDto() { Position = pos, TypeInformation = item.TipoInfo, Height = item.Altura, Decibels = list });
                    }

                    //////Matriz 1
                    foreach (InformationOctavesDto item in infoOctaves.Structure.OrderBy(x => TimeSpan.Parse(x.Hora)).Where(x => (x.TipoInfo is not "ANT" and not "DES") || ((x.TipoInfo is "ANT" or "DES") && x.Altura is "1/3")).OrderBy(x => TimeSpan.Parse(x.Hora)))
                    {
                        int pos = infoOctaves.Structure.IndexOf(item) + 1;

                        List<double> list = new()
                        {
                            (double)Math.Round(item.D16, 7),
                            (double)Math.Round(item.D20, 7),
                            (double)Math.Round(item.D25, 7),
                            (double)Math.Round(item.D315, 7),
                            (double)Math.Round(item.D40, 7),
                            (double)Math.Round(item.D50, 7),
                            (double)Math.Round(item.D63, 7),
                            (double)Math.Round(item.D80, 7),
                            (double)Math.Round(item.D100, 7),
                            (double)Math.Round(item.D125, 7),
                            (double)Math.Round(item.D160, 7),
                            (double)Math.Round(item.D200, 7),
                            (double)Math.Round(item.D250, 7),
                            (double)Math.Round(item.D3151, 7),
                            (double)Math.Round(item.D400, 7),
                            (double)Math.Round(item.D500, 7),
                            (double)Math.Round(item.D630, 7),
                            (double)Math.Round(item.D800, 7),
                            (double)Math.Round(item.D1000, 7),
                            (double)Math.Round(item.D1250, 7),
                            (double)Math.Round(item.D1600, 7),
                            (double)Math.Round(item.D2000, 7),
                            (double)Math.Round(item.D2500, 7),
                            (double)Math.Round(item.D3150, 7),
                            (double)Math.Round(item.D4000, 7),
                            (double)Math.Round(item.D5000, 7),
                            (double)Math.Round(item.D6300, 7),
                            (double)Math.Round(item.D8000, 7),
                            (double)Math.Round(item.D10000, 7),
                        };

                        matrixOneDto.Add(new MatrixOneDto() { Position = pos, TypeInformation = item.TipoInfo, Height = item.Altura, Decibels = list });
                    }

                    //////Matriz 2 datos reales
                    foreach (InformationOctavesDto item in infoOctaves.Structure.OrderBy(x => TimeSpan.Parse(x.Hora)))
                    {
                        int pos = infoOctaves.Structure.IndexOf(item) + 1;

                        List<double> list = new()
                        {
                            Math.Round(Math.Pow(10d, (double)item.D16 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D20 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D25 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D315 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D40 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D50 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D63 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D80 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D100 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D125 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D160 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D200 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D250 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D3151 / 10d), 7),

                            Math.Round(Math.Pow(10d, (double)item.D400 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D500 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D630 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D800 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D1000 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D1250 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D1600 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D2000 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D2500 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D3150 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D4000 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D5000 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D6300 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D8000 / 10d), 7),
                            Math.Round(Math.Pow(10d, (double)item.D10000 / 10d), 7),
                        };

                        matrixTwoDto.Add(new MatrixTwoDto() { Position = pos, TypeInformation = item.TipoInfo, Height = item.Altura, Decibels = list, SumRealDecibels = list.Sum() });
                    }

                    //////Matriz 3 •	A la información de esta matriz la llamaremos mostrar.
                    int increment = 0;
                    foreach (InformationOctavesDto item in infoOctaves.Structure.OrderBy(x => TimeSpan.Parse(x.Hora)))
                    {
                        int pos = infoOctaves.Structure.IndexOf(item) + 1;

                        MatrixTwoDto orig = matrixTwoDto.FirstOrDefault(x => x.Position == pos);

                        matrixThreeDto.Add(new MatrixThreeDto()
                        {
                            Position = pos,
                            TypeInformation = item.TipoInfo,
                            Height = item.Altura,
                            Dba = (decimal)Math.Round(10 * Math.Log10(matrixTwoDto.FirstOrDefault(x => x.Position == pos).SumRealDecibels), 7),
                            Decibels315 = (decimal)Math.Round(10 * Math.Log10(orig.Decibels[2] + orig.Decibels[3] + orig.Decibels[4]), 7),
                            Decibels63 = (decimal)Math.Round(10 * Math.Log10(orig.Decibels[5] + orig.Decibels[6] + orig.Decibels[7]), 7),
                            Decibels125 = (decimal)Math.Round(10 * Math.Log10(orig.Decibels[8] + orig.Decibels[9] + orig.Decibels[10]), 7),
                            Decibels250 = (decimal)Math.Round(10 * Math.Log10(orig.Decibels[11] + orig.Decibels[12] + orig.Decibels[13]), 7),
                            Decibels500 = (decimal)Math.Round(10 * Math.Log10(orig.Decibels[14] + orig.Decibels[15] + orig.Decibels[16]), 7),
                            Decibels1000 = (decimal)Math.Round(10 * Math.Log10(orig.Decibels[17] + orig.Decibels[18] + orig.Decibels[19]), 7),
                            Decibels2000 = (decimal)Math.Round(10 * Math.Log10(orig.Decibels[20] + orig.Decibels[21] + orig.Decibels[22]), 7),
                            Decibels4000 = (decimal)Math.Round(10 * Math.Log10(orig.Decibels[23] + orig.Decibels[24] + orig.Decibels[25]), 7),
                            Decibels8000 = (decimal)Math.Round(10 * Math.Log10(orig.Decibels[26] + orig.Decibels[27] + orig.Decibels[28]), 7),
                            Decibels10000 = (decimal)Math.Round(matrixBaseDto[increment].Decibels[28], 7)
                        });

                        increment++;
                    }
                }

                count = 1;

                //////Matriz 3  ANT
                foreach (MatrixThreeDto item in matrixThreeDto.Where(x => x.TypeInformation.ToUpper().Equals("ANT") && x.Height != "2/3"))
                {
                    matrixThreeAnt.Add(new MatrixThreeDto()
                    {
                        Position = count,
                        TypeInformation = item.TypeInformation,
                        Height = item.Height,
                        Dba = item.Dba,

                        Decibels315 = Math.Round(item.Decibels315, 7),
                        Decibels63 = Math.Round(item.Decibels63, 7),
                        Decibels125 = Math.Round(item.Decibels125, 7),
                        Decibels250 = Math.Round(item.Decibels250, 7),
                        Decibels500 = Math.Round(item.Decibels500, 7),
                        Decibels1000 = Math.Round(item.Decibels1000, 7),
                        Decibels2000 = Math.Round(item.Decibels2000, 7),
                        Decibels4000 = Math.Round(item.Decibels4000, 7),
                        Decibels8000 = Math.Round(item.Decibels8000, 7),
                        Decibels10000 = Math.Round(item.Decibels10000, 7),
                    });

                    count++;
                }

                count = 1;

                //////Matriz 3  DES
                foreach (MatrixThreeDto item in matrixThreeDto.Where(x => x.TypeInformation.ToUpper().Equals("DES") && x.Height != "2/3").OrderBy(x => x.Height))
                {
                    matrixThreeDes.Add(new MatrixThreeDto()
                    {
                        Position = count,
                        TypeInformation = item.TypeInformation,
                        Height = item.Height,
                        Dba = item.Dba,

                        Decibels315 = item.Decibels315,
                        Decibels63 = item.Decibels63,
                        Decibels125 = item.Decibels125,
                        Decibels250 = item.Decibels250,
                        Decibels500 = item.Decibels500,
                        Decibels1000 = item.Decibels1000,
                        Decibels2000 = item.Decibels2000,
                        Decibels4000 = item.Decibels4000,
                        Decibels8000 = item.Decibels8000,
                        Decibels10000 = item.Decibels10000
                    });

                    count++;
                }

                if (loadData)
                {
                    // Objeto que vamos a usar para guardar los promedios y eso
                    objetoAmbienteAntDesp = new MatrixTwoPromDto() { Decibels = new List<double>(), SumRealDecibels = 0 };

                    // Cantidad de decibeles por posicion (Los conte arriba)
                    decimal cantidadDecibeles = 29;

                    // Inicializando la lista de decibeles en 0 para empezar a sumar
                    for (int i = 0; i < cantidadDecibeles; i++)
                    {
                        objetoAmbienteAntDesp.Decibels.Add(0);
                    }

                    // Empezamos a recorrer la matriz 2
                    foreach (MatrixTwoDto renglon in matrixTwoDto.Where(e => (e.TypeInformation is "ANT" or "DES") && e.Height != "2/3"))
                    {
                        // Aqui recorremos todos los decibeles de cada renglon
                        for (int i = 0; i < cantidadDecibeles; i++)
                        {
                            // Aqui sumamos todos los valores de cada decibel por separado en cada posicion de la lista de decibeles, aqui se suman 
                            // por ejemplo todos los D16 en la posicion 0, todos los D20 en la posicion 1 y asi
                            objetoAmbienteAntDesp.Decibels[i] += Math.Round(renglon.Decibels[i], 7);
                        }
                        // Sumamos todas las sumas de cada renglon
                        objetoAmbienteAntDesp.SumRealDecibels += Math.Round(renglon.SumRealDecibels, 7);
                    }

                    // Ahora toca aplicar la formula a todos los totales que conseguimos
                    for (int i = 0; i < cantidadDecibeles; i++)
                    {
                        objetoAmbienteAntDesp.Decibels[i] = Math.Round(10d * Math.Log10(objetoAmbienteAntDesp.Decibels[i] / matrixTwoDto.Where(e => (e.TypeInformation is "ANT" or "DES") && e.Height != "2/3").Count()), 7);
                    }

                    objetoAmbienteAntDesp.SumRealDecibels = Math.Round(10d * Math.Log10(objetoAmbienteAntDesp.SumRealDecibels / matrixTwoDto.Where(e => (e.TypeInformation is "ANT" or "DES") && e.Height != "2/3").Count()), 7);

                    // Y listo ahora la segunda es un copia y pega pero solo cambia el Where del foreach ya que la anterior era solo a los antes y despues y ahora esta es para el tipo seleccionado

                    // Objeto que vamos a usar para guardar los promedios y eso
                    objetoAmbienteSelect = new MatrixTwoPromDto() { Decibels = new List<double>(), SumRealDecibels = 0 };
                    // Cantidad de decibeles por posicion (Los conte arriba)

                    // Inicializando la lista de decibeles en 0 para empezar a sumar
                    for (int i = 0; i < cantidadDecibeles; i++)
                    {
                        objetoAmbienteSelect.Decibels.Add(0);
                    }

                    // Empezamos a recorrer la matriz 2
                    foreach (IGrouping<string, MatrixTwoDto> group in matrixTwoDto
                        .Where(x => x.TypeInformation.ToUpper().Equals(CoolingType.ToUpper()))
                        .GroupBy(e => e.Height)
                        .OrderBy(e => e.Key))
                    {
                        foreach (MatrixTwoDto renglon in group.Take(cantidadMaximadeMediciones))
                        {
                            // Aqui recorremos todos los decibeles de cada renglon
                            for (int i = 0; i < cantidadDecibeles; i++)
                            {
                                // Aqui sumamos todos los valores de cada decibel por separado en cada posicion de la lista de decibeles, aqui se suman 
                                // por ejemplo todos los D16 en la posicion 0, todos los D20 en la posicion 1 y asi
                                objetoAmbienteSelect.Decibels[i] += Math.Round(renglon.Decibels[i], 7);
                            }
                            // Sumamos todas las sumas de cada renglon
                            objetoAmbienteSelect.SumRealDecibels += Math.Round(renglon.SumRealDecibels, 7);
                        }
                    }

                    // Ahora toca aplicar la formula a todos los totales que conseguimos
                    for (int i = 0; i < cantidadDecibeles; i++)
                    {
                        objetoAmbienteSelect.Decibels[i] = Math.Round(10d * Math.Log10(objetoAmbienteSelect.Decibels[i] / (cantidadMaximadeMediciones * 2d)), 7);
                    }

                    objetoAmbienteSelect.SumRealDecibels = Math.Round(10d * Math.Log10(objetoAmbienteSelect.SumRealDecibels / (cantidadMaximadeMediciones * 2d)), 7);

                    // Y listo la segunda calculo

                    double value = 0.1d;

                    AmbProm = new MatrixThreeDto()
                    {
                        Dba = (decimal)Math.Round(objetoAmbienteAntDesp.SumRealDecibels, 0),
                        Decibels315 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[2]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[3]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[4])), 2),
                        Decibels63 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[5]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[6]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[7])), 2),
                        Decibels125 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[8]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[9]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[10])), 2),
                        Decibels250 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[11]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[12]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[13])), 2),
                        Decibels500 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[14]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[15]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[16])), 2),
                        Decibels1000 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[17]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[18]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[19])), 2),
                        Decibels2000 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[20]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[21]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[22])), 2),
                        Decibels4000 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[23]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[24]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[25])), 2),
                        Decibels8000 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[26]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[27]) + Math.Pow(10d, value * objetoAmbienteAntDesp.Decibels[28])), 2),
                        Decibels10000 = (decimal)Math.Round(objetoAmbienteAntDesp.Decibels[28], 2)
                    };

                    AmbTrans = new MatrixThreeDto()
                    {
                        Dba = (decimal)Math.Round(objetoAmbienteSelect.SumRealDecibels, 0),
                        Decibels315 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10, value * objetoAmbienteSelect.Decibels[2]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[3]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[4])), 2),
                        Decibels63 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10, value * objetoAmbienteSelect.Decibels[5]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[6]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[7])), 2),
                        Decibels125 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10, value * objetoAmbienteSelect.Decibels[8]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[9]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[10])), 2),
                        Decibels250 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10, value * objetoAmbienteSelect.Decibels[11]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[12]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[13])), 2),
                        Decibels500 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10, value * objetoAmbienteSelect.Decibels[14]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[15]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[16])), 2),
                        Decibels1000 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10, value * objetoAmbienteSelect.Decibels[17]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[18]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[19])), 2),
                        Decibels2000 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10, value * objetoAmbienteSelect.Decibels[20]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[21]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[22])), 2),
                        Decibels4000 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10, value * objetoAmbienteSelect.Decibels[23]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[24]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[25])), 2),
                        Decibels8000 = (decimal)Math.Round(10d * Math.Log10(Math.Pow(10, value * objetoAmbienteSelect.Decibels[26]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[27]) + Math.Pow(10, value * objetoAmbienteSelect.Decibels[28])), 2),
                        Decibels10000 = (decimal)Math.Round(objetoAmbienteSelect.Decibels[28], 2)
                    };

                    diferencia = Math.Round(AmbTrans.Dba - AmbProm.Dba, 0);

                    if (diferencia is < 0 or >= 11)
                    {
                        factorCoreccion = 0;
                    }
                    else if (diferencia is 0 or 1 or 2 or 3 or 4 or 5)
                    {
                        factorCoreccion = Convert.ToDecimal(-1.6);
                    }
                    else if (diferencia == 6)
                    {
                        factorCoreccion = Convert.ToDecimal(-1.3);
                    }
                    else if (diferencia == 7)
                    {
                        factorCoreccion = Convert.ToDecimal(-1.0);
                    }
                    else if (diferencia == 8)
                    {
                        factorCoreccion = Convert.ToDecimal(-0.8);
                    }
                    else if (diferencia == 9)
                    {
                        factorCoreccion = Convert.ToDecimal(-0.6);
                    }
                    else if (diferencia == 10)
                    {
                        factorCoreccion = Convert.ToDecimal(-0.4);
                    }

                    if (KeyTest.ToUpper().Equals("RUI"))
                    {

                        count = 1;
                        //////Matriz 3 •alturas 1/2 test ruido.
                        foreach (MatrixThreeDto item in matrixThreeDto.Where(x => (x.TypeInformation is not "ANT" and not "DES") && x.Height.Equals("1/2")))
                        {

                            MatrixThree12HDto.Add(new MatrixThree1323HDto()
                            {
                                Position = count,
                                TypeInformation = item.TypeInformation,
                                Height = item.Height,
                                Dba = item.Dba,

                                CorDba = item.Dba + factorCoreccion,

                                Decibels315 = item.Decibels315,
                                Decibels63 = item.Decibels63,
                                Decibels125 = item.Decibels125,
                                Decibels250 = item.Decibels250
                                ,
                                Decibels500 = item.Decibels500
                                 ,
                                Decibels1000 = item.Decibels1000

                                  ,
                                Decibels2000 = item.Decibels2000

                                  ,
                                Decibels4000 = item.Decibels4000

                                   ,
                                Decibels8000 = item.Decibels8000

                                    ,
                                Decibels10000 = item.Decibels10000

                            });
                            count++;
                        }

                        count = 1;
                        //////Matriz 3 •alturas 1/3 test ruido.
                        foreach (MatrixThreeDto item in matrixThreeDto.Where(x => (x.TypeInformation is not "ANT" and not "DES") && x.Height.Equals("1/3")))
                        {

                            MatrixThree13HDto.Add(new MatrixThree1323HDto()
                            {
                                Position = count,
                                TypeInformation = item.TypeInformation,
                                Height = item.Height,
                                Dba = item.Dba,

                                CorDba = item.Dba + factorCoreccion,

                                Decibels315 = item.Decibels315,
                                Decibels63 = item.Decibels63,
                                Decibels125 = item.Decibels125,
                                Decibels250 = item.Decibels250
                                ,
                                Decibels500 = item.Decibels500
                                 ,
                                Decibels1000 = item.Decibels1000

                                  ,
                                Decibels2000 = item.Decibels2000

                                  ,
                                Decibels4000 = item.Decibels4000

                                   ,
                                Decibels8000 = item.Decibels8000

                                    ,
                                Decibels10000 = item.Decibels10000

                            });

                            count++;
                        }

                        count = 1;
                        //////Matriz 3 •alturas  2/3 test ruido.
                        foreach (MatrixThreeDto item in matrixThreeDto.Where(x => (x.TypeInformation is not "ANT" and not "DES") && x.Height.Equals("2/3")))
                        {
                            MatrixThree23HDto.Add(new MatrixThree1323HDto()
                            {
                                Position = count,
                                TypeInformation = item.TypeInformation,
                                Height = item.Height,
                                Dba = item.Dba,
                                CorDba = item.Dba + factorCoreccion,
                                Decibels315 = item.Decibels315,
                                Decibels63 = item.Decibels63,
                                Decibels125 = item.Decibels125,
                                Decibels250 = item.Decibels250,
                                Decibels500 = item.Decibels500,
                                Decibels1000 = item.Decibels1000,
                                Decibels2000 = item.Decibels2000,
                                Decibels4000 = item.Decibels4000,
                                Decibels8000 = item.Decibels8000,
                                Decibels10000 = item.Decibels10000
                            });
                            count++;
                        }
                    }
                    else
                    {
                        string altura = "";
                        count = 1;

                        //////Matriz 3 OCTAVAS Tipos de enfriamiento
                        foreach (MatrixThreeDto item in matrixThreeDto.Where(x => x.TypeInformation.ToUpper() is not "DES" and not "ANT").OrderBy(x => x.Height))
                        {
                            if (string.IsNullOrEmpty(altura))
                            {
                                altura = item.Height;
                            }

                            if (!altura.Equals(item.Height))
                            {
                                count = 1;
                            }

                            altura = item.Height;
                            matrixThreeCoolingType.Add(new MatrixThreeDto()
                            {

                                Position = count,
                                TypeInformation = item.TypeInformation,
                                Height = item.Height,
                                Dba = item.Dba,
                                Decibels315 = item.Decibels315,
                                Decibels63 = item.Decibels63,
                                Decibels125 = item.Decibels125,
                                Decibels250 = item.Decibels250,
                                Decibels500 = item.Decibels500,
                                Decibels1000 = item.Decibels1000,
                                Decibels2000 = item.Decibels2000,
                                Decibels4000 = item.Decibels4000,
                                Decibels8000 = item.Decibels8000,
                                Decibels10000 = item.Decibels10000
                            });
                            count++;
                        }
                    }
                }
                else
                {
                    count = 1;
                    if (KeyTest.ToUpper().Equals("RUI"))
                    {

                        //////Matriz 3 •alturas 1/2 test ruido.
                        foreach (MatrixThreeDto item in matrixThreeDto.Where(x => (x.TypeInformation is not "ANT" and not "DES") && x.Height.Equals("1/2")))
                        {

                            MatrixThree12HDto.Add(new MatrixThree1323HDto()
                            {
                                Position = count,
                                TypeInformation = item.TypeInformation,
                                Height = item.Height,

                            });
                            count++;
                        }

                        count = 1;

                        foreach (MatrixThreeDto item in matrixThreeDto.Where(x => (x.TypeInformation is not "ANT" and not "DES") && x.Height.Equals("1/3")))

                        {

                            MatrixThree13HDto.Add(new MatrixThree1323HDto()
                            {
                                Position = count,
                                TypeInformation = item.TypeInformation,
                                Height = item.Height,

                            });

                            count++;
                        }

                        count = 1;
                        //////Matriz 3 •alturas  2/3 test ruido.
                        foreach (MatrixThreeDto item in matrixThreeDto.Where(x => (x.TypeInformation is not "ANT" and not "DES") && x.Height.Equals("2/3")))

                        {

                            MatrixThree23HDto.Add(new MatrixThree1323HDto()
                            {
                                Position = count,
                                TypeInformation = item.TypeInformation,
                                Height = item.Height,

                            });

                            count++;
                        }
                    }
                    else
                    {

                        string altura = "";
                        count = 1;

                        //////Matriz 3 OCTAVAS Tipos de enfriamiento
                        foreach (MatrixThreeDto item in matrixThreeDto.Where(x => x.TypeInformation.ToUpper() is not "DES" and not "ANT").OrderBy(X => X.Height))
                        {

                            if (string.IsNullOrEmpty(altura))
                            {
                                altura = item.Height;
                            }

                            if (!altura.Equals(item.Height))
                            {
                                count = 1;
                            }

                            altura = item.Height;
                            matrixThreeCoolingType.Add(new MatrixThreeDto()
                            {

                                Position = count,
                                TypeInformation = item.TypeInformation,
                                Height = item.Height,

                            });
                            count++;
                        }
                    }
                }
                StringBuilder sb = new();
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (decimal? obj in Task2.Result.CharacteristicsArtifact.Select(x => x.Mvaf1))
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        _ = sb.Append(string.Format("{0:0.000}", obj));
                    }
                    else
                    {
                        _ = sb.Append("/" + string.Format("{0:0.000}", obj));
                    }
                }

                string newCapacities = FormatFieldCapacity(sb.ToString());

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                string TypeCooling = CoolingType;
                decimal? Garantia = null;
                string FoodTitle = "";
                string FoodUM = "";
                string FoodValue = "";
                string RuleTitle = "";

                foreach (CharacteristicsArtifactDto item in Task2.Result.CharacteristicsArtifact)
                {
                    if (CoolingType.ToUpper().Equals(item.CoolingType.ToUpper()))
                    {
                        if (item.Secuencia == 0)
                        {
                            Garantia = general.WarrantiesArtifact.NoiseOa;
                        }
                        else if (item.Secuencia == 1)
                        {
                            Garantia = general.WarrantiesArtifact.NoiseFa1;
                        }
                        else if (item.Secuencia == 2)
                        {
                            Garantia = general.WarrantiesArtifact.NoiseFa2;
                        }
                        break;
                    }
                }

                if (Language.ToUpper().Equals("ES"))
                {
                    if (Rule.ToUpper().Equals("IEEE"))
                    {
                        RuleTitle = "Prueba realizada de acuerdo a la IEEE C57.12.90";
                    }
                    else if (Rule.ToUpper().Equals("IEC"))
                    {
                        RuleTitle = "Prueba realizada de acuerdo a la IEC 60076-10";
                    }

                    if (Feeding.ToUpper().Equals("TENSIÓN"))
                    {
                        FoodTitle = "Tensión de Prueba:";
                        FoodValue = FeedingKVValue.ToString();
                        FoodUM = "kV";
                    }
                    else if (Feeding.ToUpper().Equals("CORRIENTE"))
                    {
                        FoodTitle = Feeding + ":";
                        FoodValue = FeedingKVValue.ToString();
                        FoodUM = "Amps";
                    }
                    else if (Feeding.ToUpper().Equals("PÉRDIDAS"))
                    {
                        FoodTitle = Feeding + ":";
                        FoodValue = FeedingKVValue.ToString();
                        FoodUM = "kW";
                    }
                }
                else
                {

                    if (Rule.ToUpper().Equals("IEEE"))
                    {
                        RuleTitle = "Test Performed according to IEEE C57.12.90";
                    }
                    else if (Rule.ToUpper().Equals("IEC"))
                    {
                        RuleTitle = "Test Performed according to IEC 60076-10";
                    }

                    if (Feeding.ToUpper().Equals("TENSIÓN"))
                    {
                        FoodTitle = "Test Voltage:";
                        FoodValue = FeedingKVValue.ToString();
                        FoodUM = "kV";
                    }
                    else if (Feeding.ToUpper().Equals("CORRIENTE"))
                    {
                        FoodTitle = "Current:";
                        FoodValue = FeedingKVValue.ToString();
                        FoodUM = "Amps";
                    }
                    else if (Feeding.ToUpper().Equals("PÉRDIDAS"))
                    {
                        FoodTitle = "Losses:";
                        FoodValue = FeedingKVValue.ToString();
                        FoodUM = "kW";
                    }
                }

                resultConfiReports = new SettingsToDisplayNRAReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, TypeCooling = TypeCooling, FoodTitle = FoodTitle, FoodUM = FoodUM, FoodValue = FoodValue, RuleTitle = RuleTitle, Warranty = Garantia.ToString(), MatrixBaseDto = matrixBaseDto, MatrixOneDto = matrixOneDto, MatrixTwoDto = matrixTwoDto, MatrixTwoPromAntDespDto = objetoAmbienteAntDesp, MatrixTwoPromCoolingTypeDto = objetoAmbienteSelect, MatrixThreeDto = matrixThreeDto, AmbProm = AmbProm, AmbTrans = AmbTrans, Diferencia = diferencia, FactorCoreccion = factorCoreccion, MatrixHeight12 = MatrixThree12HDto, MatrixHeight13 = MatrixThree13HDto, MatrixHeight23 = MatrixThree23HDto, matrixThreeAnt = matrixThreeAnt, matrixThreeDes = matrixThreeDes, matrixThreeCoolingType = matrixThreeCoolingType };

                return new JsonResult(new ApiResponse<SettingsToDisplayNRAReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportBPC));
                return new JsonResult(new ApiResponse<SettingsToDisplayNRAReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportETD")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayETDReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportETD([FromQuery] string NroSerie, string KeyTest, string Lenguage, bool TypeRegression, string Overload, bool BtDifCap, decimal CapacityBt, string Tertiary2B, bool TerCapRed, decimal CapacityTer, string WindingSplit, decimal IdCuttingData, string Connection, string tokenSesion)
        {

            if (this.Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }
            SettingsToDisplayETDReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayETDReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            //if (string.IsNullOrEmpty(KeyTest))
            //    return new JsonResult(new ApiResponse<SettingsToDisplayETDReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            //if (string.IsNullOrEmpty(Lenguage))
            //    return new JsonResult(new ApiResponse<SettingsToDisplayETDReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 1;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => this._reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                //Task<ApiResponse<long>> getNroTestNextETD() => this._reportHttpClientService.GetNroTestNextETD(NroSerie, KeyTest, Lenguage, TypeRegression, BtDifCap, CapacityBt, Tertiary2B, TerCapRed, CapacityTer, WindingSplit, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ETDConfigFileReportDto>>> getConfigurationReports() => this._reportHttpClientService.GetConfigurationETDDownload(tokenSesion: getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => this._reportHttpClientService.GetBaseTemplate("ETD", "UNI", Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                //Task<ApiResponse<HeaderCuttingDataDto>> getInfoHeaderCuttingData() => this._reportHttpClientService.GetInfoHeaderCuttingData(IdCuttingData, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splconfiguration));

                //Task<ApiResponse<List<CorrectionFactorkWTypeCoolingDto>>> getCorrectionFactorkWTypeCooling() => this._reportHttpClientService.GetCorrectionFactorkWTypeCooling(getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splconfiguration));

                //Task<ApiResponse<List<StabilizationDataDto>>> getStabilizationData() => this._reportHttpClientService.GetStabilizationData(NroSerie, null, null, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splconfiguration));

                //Task<ApiResponse<StabilizationDesignDataDto>> getStabilizationDesignData() => this._reportHttpClientService.GetStabilizationDesignData(NroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splconfiguration));

                //Task<ApiResponse<long>> Task1 = getNroTestNextETD();

                Task<ApiResponse<List<ETDConfigFileReportDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                //Task<ApiResponse<HeaderCuttingDataDto>> TaskHeaderCuttingData = getInfoHeaderCuttingData();

                //Task<ApiResponse<List<StabilizationDataDto>>> TaskStabilizationData = getStabilizationData();
                //Task<ApiResponse<StabilizationDesignDataDto>> TaskStabilizationDesignData = getStabilizationDesignData();

                //Task<ApiResponse<List<CorrectionFactorkWTypeCoolingDto>>> TaskCorrectionFactorkWTypeCooling = getCorrectionFactorkWTypeCooling();

                List<Task> lote1 = new() { Task3, Task4 };

                await Task.WhenAll(lote1);

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayETDReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayETDReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayETDReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayETDReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                SettingsToDisplayETDReportsDto config = new();

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                //config.HeadboardReport.Capacity = FormatFieldCapacity(newCapacities);

                //string FormatFieldCapacity(string str)
                //{
                //    string finalFormat = "";
                //    string[] stringSplit = str.Split('/');

                //    for (int i = 0; i < stringSplit.Length; i++)
                //    {
                //        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                //    }
                //    finalFormat = string.Join("/", stringSplit);

                //    return finalFormat;
                //}

                //StabilizationDataDto stabilizationData = TaskStabilizationData.Result.Structure.FirstOrDefault(x => x.IdReg == TaskHeaderCuttingData.Result.Structure.IdReg);
                //config.IdCorte = TaskHeaderCuttingData.Result.Structure.IdCorte;
                //config.IdReg = TaskHeaderCuttingData.Result.Structure.IdReg;

                //config.TorLim = TaskStabilizationDesignData.Result.Structure.TorLimite;
                //config.AwrLim = new List<decimal?>() { TaskStabilizationDesignData.Result.Structure.AwrLimAt, TaskStabilizationDesignData.Result.Structure.AwrLimBt, TaskStabilizationDesignData.Result.Structure.AwrLimTer };
                //config.HsrLim = new List<decimal?>() { TaskStabilizationDesignData.Result.Structure.HsrLimAt, TaskStabilizationDesignData.Result.Structure.HsrLimBt, TaskStabilizationDesignData.Result.Structure.HsrLimTer };
                //config.GradienteLim = new List<decimal?>() { TaskStabilizationDesignData.Result.Structure.GradienteLimAt, TaskStabilizationDesignData.Result.Structure.GradienteLimBt, TaskStabilizationDesignData.Result.Structure.GradienteLimTer };

                // Grafica
                config.GraficaT = new List<List<decimal>>() { new List<decimal>(), new List<decimal>(), new List<decimal>() };
                config.GraficaR = new List<List<decimal>>() { new List<decimal>(), new List<decimal>(), new List<decimal>() };

                //for (int i = 0; i < 4; i++)
                //{
                //    List<decimal> listTimeConvert = TaskHeaderCuttingData.Result.Structure.SectionCuttingData[i].DetailCuttingData.ConvertAll(x =>
                //        ((decimal.Round(x.Tiempo, 0, MidpointRounding.ToNegativeInfinity) * 60) + (Math.Abs(decimal.Round(x.Tiempo, 0, MidpointRounding.ToNegativeInfinity) - 2) * 100)) / 10
                //    );
                //    if (!listTimeConvert.Exists(x => x == 0m))
                //    {
                //        config.GraficaT[i].Add(0);
                //        config.GraficaR[i].Add(0);
                //    }
                //    if (!listTimeConvert.Exists(x => x == 3m))
                //    {
                //        config.GraficaT[i].Add(3);
                //        config.GraficaR[i].Add(0);
                //    }
                //    if (!listTimeConvert.Exists(x => x == 6m))
                //    {
                //        config.GraficaT[i].Add(6);
                //        config.GraficaR[i].Add(0);
                //    }
                //    if (!listTimeConvert.Exists(x => x == 9m))
                //    {
                //        config.GraficaT[i].Add(9);
                //        config.GraficaR[i].Add(0);
                //    }
                //    if (!listTimeConvert.Exists(x => x == 12m))
                //    {
                //        config.GraficaT[i].Add(12);
                //        config.GraficaR[i].Add(0);
                //    }
                //    if (!listTimeConvert.Exists(x => x == 15m))
                //    {
                //        config.GraficaT[i].Add(15);
                //        config.GraficaR[i].Add(0);
                //    }
                //    if (!listTimeConvert.Exists(x => x == 18m))
                //    {
                //        config.GraficaT[i].Add(18);
                //        config.GraficaR[i].Add(0);
                //    }

                //    config.GraficaT[i].AddRange(listTimeConvert.ConvertAll(x => x));
                //    config.GraficaR[i].AddRange(TaskHeaderCuttingData.Result.Structure.SectionCuttingData[i].DetailCuttingData.ConvertAll(x => x.Resistencia));
                //}

                //config.UltimaHora = TaskHeaderCuttingData.Result.Structure.UltimaHora;

                //config.NivelTension = Convert.ToDecimal(Connection);
                //config.CapacidadPruebaAT = stabilizationData.Capacidad * 1000;

                //config.CapacidadPruebaBT = (!BtDifCap) ? stabilizationData.Capacidad * 1000 : CapacityBt;
                //config.CapacidadPruebaTer = (!TerCapRed) ? stabilizationData.Capacidad * 1000 : CapacityTer;
                //config.TitPerdCorr = Overload.ToUpper().Equals("PÉRDIDAS") ? KeyTest.ToUpper().Equals("ES") ? "Pérdidas Totales" : "Total Losses" : KeyTest.ToUpper().Equals("ES") ? "Corriente" : "Current";

                //config.DatoPerdCorr = Overload.ToUpper().Equals("PÉRDIDAS") ? stabilizationData.Perdidas ?? 0 : stabilizationData.Corriente ?? 0;

                //config.Enfriamiento = stabilizationData.CoolingType;

                //config.Perdidas = stabilizationData.Perdidas ?? 0;

                //config.Elevacion = stabilizationData.OverElevation;

                //config.Altitud1 = stabilizationData.AltitudeF1;

                //config.Altitud2 = stabilizationData.AltitudeF2;

                //if (config.Altitud2.ToUpper() == "FASL" && KeyTest.ToUpper().Equals("ES"))
                //{
                //    config.Altitud2 = "PSNM";
                //}

                //if (config.Altitud2.ToUpper() == "MASL" && KeyTest.ToUpper().Equals("ES"))
                //{
                //    config.Altitud2 = "MSNM";
                //}

                //config.PosAT = stabilizationData.PosAt;
                //config.PosBT = stabilizationData.PosBt;

                //// Tiempo
                //int index = stabilizationData.StabilizationDataDetails.FindIndex(x => x.FechaHora == Convert.ToDateTime(TaskHeaderCuttingData.Result.Structure.UltimaHora));
                //StabilizationDetailsDataDto lastTimeObj = stabilizationData.StabilizationDataDetails[index];

                //for (int i = index; i > index - 4; i--)
                //{
                //    StabilizationDetailsDataDto obj = stabilizationData.StabilizationDataDetails[i];
                //    config.Tiempo.Add(obj.FechaHora.ToString());
                //    config.RadSuperior.Add(obj.PromRadSup);
                //    config.RadInferior.Add(obj.PromRadInf);
                //    config.AmbProm.Add(obj.AmbienteProm);
                //    config.TempTapa.Add(obj.TempTapa);
                //    config.TOR.Add(obj.Tor);
                //    config.AOR.Add(obj.Aor);
                //    config.BOR.Add(obj.Bor);
                //}

                //if (stabilizationData.AltitudeF2.ToUpper() == "FASL")
                //{
                //    config.FactorAltitud = stabilizationData.AltitudeF1 <= 3300
                //        ? 1
                //        : stabilizationData.AltitudeF1 / 3300;
                //}
                //else if (stabilizationData.AltitudeF2.ToUpper() == "MSNM")
                //{

                //    config.FactorAltitud = stabilizationData.AltitudeF1 <= 1000
                //        ? 1
                //        : stabilizationData.AltitudeF1 / 1000;

                //}

                //config.FactorCorrecciónkW = config.Enfriamiento.ToUpper().Equals("OL") && Overload.ToUpper().Equals("CORRIENTE")
                //    ? 0
                //    : (config.DatoPerdCorr / Convert.ToDecimal(Math.Pow(Convert.ToDouble(TaskHeaderCuttingData.Result.Structure.KwPrueba), Convert.ToDouble(TaskCorrectionFactorkWTypeCooling.Result.Structure.FirstOrDefault(x => x.CoolingType.ToUpper().Equals(config.Enfriamiento.ToUpper())).FactorCorr)))) - 1;

                //config.PorcentajeRepresentanPerdidas = Math.Abs((Math.Max(stabilizationData.Perdidas ?? 0, config.DatoPerdCorr) / Math.Min(stabilizationData.Perdidas ?? 0, config.DatoPerdCorr)) - 1);

                config.ElevAceiteSup = new();
                config.ElevAceiteProm = new();
                config.ElevAceiteInf = new();

                if (config.PorcentajeRepresentanPerdidas > 3)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        config.ElevAceiteSup.Add((config.TOR[i] * config.FactorCorrecciónkW) + config.TOR[i]);
                        config.ElevAceiteProm.Add((config.AOR[i] * config.FactorCorrecciónkW) + config.AOR[i]);
                        config.ElevAceiteInf.Add((config.BOR[i] * config.FactorCorrecciónkW) + config.BOR[i]);
                    }
                }

                //// Resistencia de corte
                //config.ResistCorte = new()
                //{
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[0].Resistencia,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[1].Resistencia,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[2].Resistencia,
                //};

                //// Temperatura resistecia de corte
                //config.TempResistCorte = new()
                //{
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[0].TempResistencia,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[1].TempResistencia,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[2].TempResistencia,
                //};

                //// Factor K
                //config.FactorK = new()
                //{
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[0].FactorK,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[1].FactorK,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[2].FactorK,
                //};

                //// Resistencia zero
                //config.ResistTCero = new()
                //{
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[0].ResistZeroC,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[1].ResistZeroC,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[2].ResistZeroC,
                //};

                //// Temperature devanado
                //config.TempDev = new()
                //{
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[0].TempDevC,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[1].TempDevC,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[2].TempDevC,
                //};

                //// Gradiente del devanado
                //config.GradienteDev = new()
                //{
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[0].GradienteCaC,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[1].GradienteCaC,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[2].GradienteCaC,
                //};

                //// Matriz
                //config.AORVKwA = new() { lastTimeObj.Aor, (lastTimeObj.Aor * config.FactorCorrecciónkW) + lastTimeObj.Aor };
                //config.TORVKwA = new() { lastTimeObj.Tor, (lastTimeObj.Tor * config.FactorCorrecciónkW) + lastTimeObj.Tor };
                //config.BORVKwA = new() { lastTimeObj.Bor, (lastTimeObj.Bor * config.FactorCorrecciónkW) + lastTimeObj.Bor };

                //config.AORVKwA.Add((config.AORVKwA[1] * config.FactorAltitud) + config.AORVKwA[1]);
                //config.TORVKwA.Add((config.TORVKwA[1] * config.FactorAltitud) + config.TORVKwA[1]);
                //config.BORVKwA.Add((config.BORVKwA[1] * config.FactorAltitud) + config.BORVKwA[1]);

                //// ElevPromDev
                //config.ElevPromDev = new() { config.GradienteDev[0] + config.AORVKwA[1] };

                //decimal AOR = 0;
                //if (WindingSplit.Equals("NA"))
                //{
                //    AOR = config.AORVKwA[2];
                //}
                //else if (WindingSplit.Equals(value: "X Arriba"))
                //{
                //    AOR = (config.TORVKwA[2] + config.AORVKwA[2]) / 2;
                //}
                //else if (WindingSplit.Equals(value: "Y Arriba"))
                //{
                //    AOR = (config.BORVKwA[2] + config.AORVKwA[2]) / 2;
                //}

                //config.ElevPromDev.Add(config.GradienteDev[1] + AOR);

                //if (WindingSplit.Equals("NA"))
                //{
                //    AOR = config.AORVKwA[2];
                //}
                //else if (WindingSplit.Equals(value: "X Arriba"))
                //{
                //    AOR = (config.AORVKwA[2] + config.BORVKwA[2]) / 2;
                //}
                //else if (WindingSplit.Equals("Y Arriba"))
                //{
                //    AOR = (config.TORVKwA[2] + config.AORVKwA[2]) / 2;
                //}

                //config.ElevPromDev.Add(config.GradienteDev[2] + AOR);

                //// ElevPtoMasCal
                //config.ElevPtoMasCal = new() { (config.GradienteDev[0] * config.FactorK[0]) + config.TORVKwA[2] };

                //decimal TOR = 0;
                //if (WindingSplit.Equals("NA") || WindingSplit.Equals("X Arriba"))
                //{
                //    TOR = config.TORVKwA[2];
                //}
                //else if (WindingSplit.Equals("Y Arriba"))
                //{
                //    TOR = config.AORVKwA[2];
                //}

                //config.ElevPromDev.Add((config.GradienteDev[1] * config.FactorK[0]) + TOR);

                //if (WindingSplit.Equals("NA") || WindingSplit.Equals("Y Arriba"))
                //{
                //    TOR = config.TORVKwA[2];
                //}
                //else if (WindingSplit.Equals("X Arriba"))
                //{
                //    TOR = config.AORVKwA[2];
                //}

                //config.ElevPromDev.Add((config.GradienteDev[2] * config.FactorK[0]) + TOR);

                ////Temp prom aceite
                //config.TempPromAceite = new() {
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[0].TempAceiteProm,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[1].TempAceiteProm,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[2].TempAceiteProm,
                //};

                ////Terminal
                //config.Terminal = new() {
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[0].Terminal,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[1].Terminal,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[2].Terminal,
                //};

                ////UM Resist
                //config.UMResist = new() {
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[0].UmResistencia,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[1].UmResistencia,
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[2].UmResistencia,
                //};

                ////Tiemp Resist
                //config.TiempoResist = new() {
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[0].DetailCuttingData.ConvertAll(x => x.Tiempo),
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[1].DetailCuttingData.ConvertAll(x => x.Tiempo),
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[2].DetailCuttingData.ConvertAll(x => x.Tiempo),
                //};

                //// Resistencia
                //config.Resistencia = new() {
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[0].DetailCuttingData.ConvertAll(x => x.Resistencia),
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[1].DetailCuttingData.ConvertAll(x => x.Resistencia),
                //    TaskHeaderCuttingData.Result.Structure.SectionCuttingData[2].DetailCuttingData.ConvertAll(x => x.Resistencia),
                //};

                //config.TitGrafica = TypeRegression;

                resultConfiReports = new SettingsToDisplayETDReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = 0, InfotmationArtifact = Task2.Result };

                return new JsonResult(new ApiResponse<SettingsToDisplayETDReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportBPC));
                return new JsonResult(new ApiResponse<SettingsToDisplayETDReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        [HttpGet("GetConfigurationReportDPR")]
        [ProducesResponseType(typeof(ApiResponse<SettingsToDisplayDPRReportsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetConfigurationReportDPR([FromQuery] string NroSerie, string KeyTest, string Lenguage, int NoCycles, int TotalTime, int Interval, decimal TimeLevel, decimal OutputLevel, int DescMayPc, int DescMayMv, int IncMaxPc, string MeasurementType, string TerminalsTest, string tokenSesion)
        {
            if (Request.Headers.TryGetValue("ApiKey", out Microsoft.Extensions.Primitives.StringValues testId))
            {
                tokenSesion = testId; //use testId value
            }

            SettingsToDisplayDPRReportsDto resultConfiReports = new();

            if (string.IsNullOrEmpty(NroSerie))
                return new JsonResult(new ApiResponse<SettingsToDisplayDPRReportsDto>((int)ResponsesID.fallido, "Número de serie es requerido", null));

            if (string.IsNullOrEmpty(KeyTest))
                return new JsonResult(new ApiResponse<SettingsToDisplayDPRReportsDto>((int)ResponsesID.fallido, "Prueba es requerido", null));

            if (string.IsNullOrEmpty(Lenguage))
                return new JsonResult(new ApiResponse<SettingsToDisplayDPRReportsDto>((int)ResponsesID.fallido, "Idioma es requerido", null));

            string[] newnoserie = { "" };
            long numberColumns = 0;
            if (!string.IsNullOrEmpty(NroSerie))
                newnoserie = NroSerie.Split('-');
            try
            {
                if (MeasurementType.ToUpper().Equals("MICROVOLTS") || MeasurementType.ToUpper().Equals("PICOLUMNS"))
                {
                    numberColumns = 3;
                }
                else
                {
                    if (MeasurementType.ToUpper().Equals("AMBOS")) { numberColumns = 6; }
                }

                Task<InformationArtifactDto> getInformationGeneral(string nroSerie) => _reportHttpClientService.GetArtifact(nroSerie, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<InformationArtifactDto> Task2 = getInformationGeneral(newnoserie[0]);

                List<Task> lote0 = new() { Task2 };
                await Task.WhenAny(lote0).Result;

                InformationArtifactDto general = Task2.Result;

                string KeyTestpre = (numberColumns == 3) ? "UN3" : "UN6";

                Task<ApiResponse<long>> getNroTestNextDPR() => _reportHttpClientService.GetNroTestNextDPR(NroSerie, KeyTest, Lenguage, NoCycles, TotalTime, Interval, TimeLevel, OutputLevel, DescMayPc, DescMayMv, IncMaxPc, MeasurementType, TerminalsTest, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spltests));

                Task<ApiResponse<List<ConfigurationReportsDto>>> getConfigurationReports() => _reportHttpClientService.GetConfigurationReports("DPR", KeyTestpre, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.splreports));

                Task<ApiResponse<BaseTemplateDto>> getBaseTemplate() => _reportHttpClientService.GetBaseTemplate("DPR", KeyTest, Lenguage, numberColumns, getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<List<PlateTensionDto>>> getPlateTension(string nroSerie) => _reportHttpClientService.GetPlateTension(nroSerie, "-1", getTokenForMicroservices(tokenSesion, (int)Enums.MicroservicesEnum.spldesigninformation));

                Task<ApiResponse<long>> Task1 = getNroTestNextDPR();

                Task<ApiResponse<List<ConfigurationReportsDto>>> Task3 = getConfigurationReports();

                Task<ApiResponse<BaseTemplateDto>> Task4 = getBaseTemplate();

                Task<ApiResponse<List<PlateTensionDto>>> Task6 = getPlateTension(NroSerie);

                List<Task> lote1 = new() { Task1, Task3, Task4, Task6 };

                await Task.WhenAny(lote1).Result;

                bool valLongitud = Validations.validacion55Caracteres(NroSerie.Trim());
                bool valFormat = Validations.validacionCaracteresNoSeriePruebasConsutla(NroSerie.Trim());

                if (valLongitud)
                    return new JsonResult(new ApiResponse<SettingsToDisplayDPRReportsDto>((int)ResponsesID.fallido, "El número de serie no puede excederse de 55 caracteres", null));
                if (valFormat)
                    return new JsonResult(new ApiResponse<SettingsToDisplayDPRReportsDto>((int)ResponsesID.fallido, "El número de serie solo debe contener letras, números y guión medio", null));

                if (general.GeneralArtifact == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayDPRReportsDto>((int)ResponsesID.fallido, "El número de serie no se encuentra registrado", null));

                if (general.GeneralArtifact.OrderCode == null)
                    return new JsonResult(new ApiResponse<SettingsToDisplayDPRReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información general de diseño", null));

                if (general.CharacteristicsArtifact.Count == 0)
                    return new JsonResult(new ApiResponse<SettingsToDisplayDPRReportsDto>((int)ResponsesID.fallido, "El número de serie no cuenta con información de características", null));

                string newCapacities = "";
                bool firstvalue = true;
                int numCapacidad = Task2.Result.CharacteristicsArtifact.Count;

                foreach (CharacteristicsArtifactDto obj in Task2.Result.CharacteristicsArtifact)
                {
                    if (firstvalue)
                    {
                        firstvalue = false;
                        newCapacities += string.Format("{0:0.000}", obj.Mvaf1);
                    }
                    else
                    {
                        if (obj.Mvaf1.ToString().IndexOf('.') == -1)
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                        else
                        {
                            newCapacities += "/" + string.Format("{0:0.000}", obj.Mvaf1);
                        }
                    }
                }

                newCapacities = FormatFieldCapacity(newCapacities);

                string FormatFieldCapacity(string str)
                {
                    string finalFormat = "";
                    string[] stringSplit = str.Split('/');

                    for (int i = 0; i < stringSplit.Length; i++)
                    {
                        stringSplit[i] = stringSplit[i].IndexOf('.') == -1 ? i == (stringSplit.Length - 1) ? stringSplit[i] : stringSplit[i] + "/" : stringSplit[i];
                    }
                    finalFormat = string.Join("/", stringSplit);

                    return finalFormat;
                }

                string TitTerminal1 = string.Empty;
                string TitTerminal2 = string.Empty;
                string TitTerminal3 = string.Empty;
                string TitTerminal4 = string.Empty;
                string TitTerminal5 = string.Empty;
                string TitTerminal6 = string.Empty;

                string UMed1 = string.Empty;
                string UMed2 = string.Empty;
                string UMed3 = string.Empty;
                string UMed4 = string.Empty;
                string UMed5 = string.Empty;
                string UMed6 = string.Empty;

                string[] terminales = TerminalsTest.Split(",");

                foreach (string item in terminales)
                {
                    int pos = terminales.ToList().IndexOf(item) + 1;

                    if (pos == 1)
                    {
                        TitTerminal1 = item;

                        getUnits(MeasurementType, ref UMed1);

                    }
                    else if (pos == 2)
                    {
                        TitTerminal2 = item;
                        getUnits(MeasurementType, ref UMed2);
                    }
                    else if (pos == 3)
                    {
                        TitTerminal3 = item;
                        getUnits(MeasurementType, ref UMed3);

                    }
                    else if (pos == 4)
                    {
                        TitTerminal4 = item;
                        getUnits(MeasurementType, ref UMed4);
                    }
                    else if (pos == 5)
                    {
                        TitTerminal5 = item;

                        getUnits(MeasurementType, ref UMed5);
                    }
                    else if (pos == 6)
                    {
                        TitTerminal6 = item;
                        getUnits(MeasurementType, ref UMed6);
                    }
                }

                List<string> Times = new();
                List<string> Voltages = new();

                Times.Add("0");
                Times.Add(Interval.ToString());
                decimal value3 = NoCycles / Convert.ToDecimal(general.GeneralArtifact.Frecuency);
                Times.Add(Math.Round(value3, 0) + " Sec.");

                for (int i = Interval; i <= TotalTime; i += Interval)
                {
                    Times.Add(i + " Min.");
                }

                Voltages.Add("0");
                Voltages.Add(TimeLevel.ToString());
                Voltages.Add(OutputLevel.ToString());

                for (int i = 3; i < Times.Count; i++)
                {
                    Voltages.Add(TimeLevel.ToString());
                }

                resultConfiReports = new SettingsToDisplayDPRReportsDto { BaseTemplate = Task4.Result.Structure, ConfigurationReports = Task3.Result.Structure, HeadboardReport = new HeadboardReportsDto { Client = Task2.Result.GeneralArtifact.CustomerName, Capacity = newCapacities, NoSerie = NroSerie, NoteFc = "" }, NextTestNumber = Task1.Result.Structure, InfotmationArtifact = Task2.Result, Frequency = general.GeneralArtifact.Frecuency, TitTerminal1 = TitTerminal1, TitTerminal2 = TitTerminal2, TitTerminal3 = TitTerminal3, TitTerminal4 = TitTerminal4, TitTerminal5 = TitTerminal5, TitTerminal6 = TitTerminal6, UMed1 = UMed1, UMed2 = UMed2, UMed3 = UMed3, UMed4 = UMed4, UMed5 = UMed5, UMed6 = UMed6, Times = Times, Voltages = Voltages };

                return new JsonResult(new ApiResponse<SettingsToDisplayDPRReportsDto>(Enums.EnumsGen.Succes, "Resultado obtenido exitosamente", resultConfiReports));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en '{0}'", nameof(GetConfigurationReportTDP));
                return new JsonResult(new ApiResponse<SettingsToDisplayDPRReportsDto>(Enums.EnumsGen.Error, ex.Message, resultConfiReports));

            }
        }

        private static void getUnits(string MeasurementType, ref string UMed)
        {
            if (MeasurementType.ToUpper().Equals("MICROVOLTS"))
            {
                UMed = "µV";
            }
            else
             if (MeasurementType.ToUpper().Equals("PICOLUMNS"))
            {
                UMed = "pC";
            }
            else
            {
                //if (MeasurementType.ToUpper().Equals("AMBOS"))
                //{
                UMed = "pC,µV";

                //}
            }
        }
    }
}