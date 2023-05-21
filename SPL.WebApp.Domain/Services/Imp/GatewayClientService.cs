namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web;

    using AutoMapper;

    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.DPR;
    using SPL.WebApp.Domain.DTOs.ETD;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.SecurityApis;

    public class GatewayClientService : IGatewayClientService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IValidateAccesApis _AAD;
        public GatewayClientService(
            HttpClient httpClient, IConfiguration configuration, IMapper mapper, IValidateAccesApis aad)
        {
            this._configuration = configuration;
            this._httpClient = httpClient;
            this._options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            this._mapper = mapper;
            this._AAD = aad;
        }

        private UriBuilder GetUriBuilder()
        {
            UriBuilder uriBuilder = new()
            {
                Host = this._configuration["GateWayAPIDomain"],
                Scheme = this._configuration["SchemeGateWayAPI"]
            };
            string port = this._configuration["GateWayAPIPort"];
            if (!string.IsNullOrEmpty(port))
                uriBuilder.Port = int.Parse(port);
            return uriBuilder;
        }

        public async Task<ApiResponse<PositionsDTO>> GetPositions(string nroSerie)
        {
            try
            {

                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["GetPositions"];
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["noSerie"] = nroSerie;

                string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.spldesigninformation).Result;
                query["tokenSesion"] = "";
                uriBuilder.Query = query.ToString();

                this._httpClient.DefaultRequestHeaders.Clear();
                this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<PositionsDTO>>(stream, this._options);
            }
            catch (Exception e)
            {
                return new ApiResponse<PositionsDTO>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = null
                };
            }
        }

        public async Task<ApiResponse<SettingsToDisplayRADReportsDTO>> GetTemplate(string typeReport, string nroSerie, string keyTest, string typeUnit, string thirdWinding, string keyLanguage)
        {

            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportsTemplate"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["typeReport"] = typeReport;
            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["typeUnit"] = typeUnit;
            query["thirdWinding"] = thirdWinding;
            query["keyLanguage"] = keyLanguage;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayRADReportsDTO>>(stream, this._options);

        }

        public async Task<ApiResponse<SettingsToDisplayRDTReportsDTO>> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, string angular, string norm, int conexion, string posAT, string posBT, string posTer)
        {

            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateRDT"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["typeReport"] = "RDT";
            query["nroSerie"] = noSerie;
            query["KeyTest"] = clavePrueba;
            query["dAngular"] = angular;
            query["rule"] = norm;
            query["lenguage"] = claveIdioma;
            query["conexion"] = conexion.ToString();
            query["posAT"] = string.IsNullOrEmpty(posAT) ? "NAN" : posAT;
            query["posBT"] = string.IsNullOrEmpty(posBT) ? "NAN" : posBT;
            query["posTer"] = string.IsNullOrEmpty(posTer) ? "NAN" : posTer;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayRDTReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayRANReportsDTO>> GetTemplate(string typeReport, string nroSerie, string keyTest, int measuring, string keyLanguage)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateRAN"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["KeyTest"] = keyTest;
            query["NumberMeasurements"] = measuring.ToString();
            query["Lenguage"] = keyLanguage;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayRANReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayFPCReportsDTO>> GetTemplate(string noSerie, string clavePrueba, string unitType, string specification, string voltageLevel, decimal frequency, string claveIdioma)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["ReportTemplateFPC"];
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["NroSerie"] = noSerie;
                query["KeyTest"] = clavePrueba;
                query["TypeUnit"] = unitType;
                query["Specification"] = specification;
                query["Frequency"] = frequency.ToString();
                query["Lenguage"] = claveIdioma;

                string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
                query["tokenSesion"] = "";
                uriBuilder.Query = query.ToString();

                this._httpClient.DefaultRequestHeaders.Clear();
                this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayFPCReportsDTO>>(stream, this._options);
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<SettingsToDisplayRODReportsDTO>> GetTemplate(string noSerie,
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
            string terPositions)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateROD"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["noSerie"] = noSerie;
            query["keyTest"] = keyTest;
            query["unitType"] = unitType;
            query["lenguage"] = lenguage;
            query["connection"] = connection;
            query["material"] = material;
            query["unitOfMeasurement"] = unitOfMeasurement;
            query["testVoltage"] = (testVoltage ?? 0).ToString();
            query["numberColumns"] = numberColumns.ToString();
            query["atPositions"] = atPositions;
            query["btPositions"] = btPositions;
            query["terPositions"] = terPositions;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayRODReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayRCTReportsDTO>> GetTemplate(string NroSerie, string KeyTest, string Lenguage, string UnitOfMeasurement, string Tertiary, decimal? Testvoltage)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateRCT"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = NroSerie;
            query["KeyTest"] = KeyTest;
            query["Lenguage"] = Lenguage;
            query["UnitOfMeasurement"] = UnitOfMeasurement;
            query["Tertiary"] = Tertiary;
            query["Testvoltage"] = (Testvoltage ?? 0).ToString();

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayRCTReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayFPBReportsDTO>> GetTemplate(string noSerie, string clavePrueba, string claveIdioma, string tangtDelta)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = _configuration["ReportTemplateFPB"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = noSerie;
            query["KeyTest"] = clavePrueba;
            query["TangentDelta"] = tangtDelta;
            query["Lenguage"] = claveIdioma;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayFPBReportsDTO>>(stream, _options);
        }

        public async Task<ApiResponse<SettingsToDisplayPCEReportsDTO>> GetTemplate(string nroSerie, string keyTest, string lenguage, string posAT, string posBT, string posTER, int beginning, int end, int interval, bool graph, string energizedWinding)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplatePCE"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["energizedWinding"] = energizedWinding;
            query["lenguage"] = lenguage;
            query["posAT"] = posAT;
            query["posBT"] = posBT;
            query["posTER"] = posTER;
            query["beginning"] = beginning.ToString();
            query["end"] = end.ToString();
            query["interval"] = interval.ToString();
            query["graph"] = graph.ToString();

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayPCEReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayPCIReportsDTO>> GetTemplate(string nroSerie, string keyTest, string lenguage, string windingMaterial, bool capRedBaja, bool autotransformer, bool monofasico, decimal overElevation, string testCapacity, int cantPosPri, int cantPosSec, string posPi, string posSec)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplatePCI"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["KeyTest"] = keyTest;
            query["Lenguage"] = lenguage;
            query["WindingMaterial"] = windingMaterial;
            query["CapRedBaja"] = capRedBaja.ToString();
            query["Autotransformer"] = autotransformer.ToString();
            query["Monofasico"] = monofasico.ToString();
            query["OverElevation"] = overElevation.ToString();
            query["TestCapacity"] = testCapacity;
            query["CantPosPri"] = cantPosPri.ToString();
            query["CantPosSec"] = cantPosSec.ToString();
            query["PosPi"] = posPi == null ? "" : posPi.ToString();
            query["PosSec"] = posSec == null ? "" : posSec.ToString();

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayPCIReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayPRDReportsDTO>> GetTemplate(string nroSerie, string keyTest, string lenguage, decimal nominalVoltage)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplatePRD"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["KeyTest"] = keyTest;
            query["Lenguage"] = lenguage;

            query["NominalVoltage"] = nominalVoltage.ToString("G", CultureInfo.InvariantCulture);

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayPRDReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayPLRReportsDTO>> GetTemplate(string nroSerie, string keyTest, string lenguage, decimal rldnt, decimal nominalVoltage, int amountOfTensions, int amountOfTime)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplatePLR"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["KeyTest"] = keyTest;
            query["Lenguage"] = lenguage;
            query["Rldnt"] = rldnt.ToString();
            query["NominalVoltage"] = nominalVoltage.ToString("G", CultureInfo.InvariantCulture);
            query["AmountOfTensions"] = amountOfTensions.ToString();
            query["AmountOfTime"] = amountOfTime.ToString();

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayPLRReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayPEEReportsDTO>> GetTemplate(string nroSerie, string keyTest, string lenguage)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplatePEE"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["KeyTest"] = keyTest;
            query["Lenguage"] = lenguage;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayPEEReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayBPCReportsDTO>> GetTemplateBPC(string nroSerie, string keyTest, string lenguage)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateBPC"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["KeyTest"] = keyTest;
            query["Lenguage"] = lenguage;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayBPCReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayRYEReportsDTO>> GetTemplate(string nroSerie, string keyTest, string lenguage,
            int noConnectionsAT, int noConnectionsBT, int noConnectionsTER, decimal voltageAT, decimal voltageBT, decimal voltageTER, string coolingType)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateRYE"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["Lenguage"] = lenguage;
            query["KeyTest"] = keyTest;
            query["NoConnectionsAT"] = noConnectionsAT.ToString();
            query["NoConnectionsBT"] = noConnectionsBT.ToString();
            query["NoConnectionsTER"] = noConnectionsTER.ToString();
            query["VoltageAT"] = voltageAT.ToString("G", CultureInfo.InvariantCulture);
            query["VoltageBT"] = voltageBT.ToString("G", CultureInfo.InvariantCulture);
            query["VoltageTER"] = voltageTER.ToString("G", CultureInfo.InvariantCulture);
            query["CoolingType"] = coolingType;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayRYEReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayISZReportsDTO>> GetTemplateISZ(string nroSerie, string keyTest, string lenguage, decimal degreesCor, string posAT, string posBT, string posTER, int qtyNeutral,  string materialWinding,
         string devanado, string impedancia)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateISZ"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["Lenguage"] = lenguage;
            query["KeyTest"] = keyTest;
            query["DegreesCor"] = degreesCor.ToString();
            query["PosAT"] = posAT;
            query["PosBT"] = posBT;
            query["PosTER"] = posTER;
            query["QtyNeutral"] = qtyNeutral.ToString();
            query["MaterialWinding"] = materialWinding;
            query["WindingEnergized"] = devanado;
            query["ImpedanceGar"] = impedancia;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayISZReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayPIRReportsDTO>> GetTemplatePIR(string nroSerie, string keyTest, string lenguage, string connectionAt, string connectionBt, string connectionTer, string includesTertiary)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["ReportTemplatePIR"];
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["NroSerie"] = nroSerie;
                query["Lenguage"] = lenguage;
                query["KeyTest"] = keyTest;
                query["ConnectionAt"] = connectionAt.Replace(" ", string.Empty);
                query["ConnectionBt"] = connectionBt.Replace(" ", string.Empty);
                query["ConnectionTer"] = connectionTer.Replace(" ", string.Empty);
                query["IncludesTertiary"] = includesTertiary;

                string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
                query["tokenSesion"] = "";
                uriBuilder.Query = query.ToString();

                this._httpClient.DefaultRequestHeaders.Clear();
                this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayPIRReportsDTO>>(stream, this._options);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResponse<SettingsToDisplayPIMReportsDTO>> GetTemplatePIM(string nroSerie, string keyTest, string lenguage, string connectionAt, string connectionBt, string applyLow)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplatePIM"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["Lenguage"] = lenguage;
            query["KeyTest"] = keyTest;
            query["ConnectionAt"] = connectionAt;
            query["ConnectionBt"] = connectionBt;
            query["ApplyLow"] = applyLow;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayPIMReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayTINReportsDTO>> GetTemplateTIM(string nroSerie, string keyTest, string lenguage, string connection, string tension)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateTIN"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["Lenguage"] = lenguage;
            query["KeyTest"] = keyTest;
            query["Connection"] = connection;
            query["Voltaje"] = tension;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayTINReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayTAPReportsDTO>> GetTemplateTAP(string nroSerie, string keyTest, string lenguage,
        string unitType, int noConnectionAT, int noConnectionBT, int noConnectionTER, string idCapAT, string idCapBT, string idCapTer)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateTAP"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["Lenguage"] = lenguage;
            query["KeyTest"] = keyTest;
            query["NoConnectionAT"] = noConnectionAT.ToString();
            query["NoConnectionBT"] = noConnectionBT.ToString();
            query["NoConnectionTER"] = noConnectionTER.ToString();
            query["IdCapAT"] = idCapAT;
            query["IdCapBT"] = idCapBT;
            query["IdCapTer"] = idCapTer;
            query["UnitType"] = unitType;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayTAPReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayCEMReportsDTO>> GetTemplateCEM(string nroSerie, string keyTest, string lenguage,
       string idPosPrimary, string posPrimary, string idPosSecundary, string posSecundary, decimal testsVoltage)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateCEM"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["Lenguage"] = lenguage;
            query["KeyTest"] = keyTest;
            query["IdPosPrimary"] = idPosPrimary;
            query["PosPrimary"] = posPrimary;
            query["IdPosSecundary"] = idPosSecundary;
            query["PosSecundary"] = posSecundary;
            query["TestsVoltage"] = testsVoltage.ToString();

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayCEMReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayTDPReportsDTO>> GetTemplateTDP(string nroSerie, string keyTest, string lenguage, int noCycles, int totalTime, int interval, decimal timeLevel, decimal outputLevel, int descMayPc, int descMayMv, int incMaxPc, string voltageLevels, string measurementType, string terminalsTest)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateTDP"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["Lenguage"] = lenguage;
            query["KeyTest"] = keyTest;
            query["NoCycles"] = noCycles.ToString();
            query["TotalTime"] = totalTime.ToString();
            query["Interval"] = interval.ToString();
            query["TimeLevel"] = timeLevel.ToString();
            query["OutputLevel"] = outputLevel.ToString();
            query["DescMayPc"] = descMayPc.ToString();
            query["DescMayMv"] = descMayMv.ToString();
            query["IncMaxPc"] = incMaxPc.ToString();
            query["VoltageLevels"] = voltageLevels.ToString();
            query["MeasurementType"] = measurementType.ToString();
            query["TerminalsTest"] = terminalsTest.ToString();

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayTDPReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayDPRReportsDTO>> GetTemplateDPR(string nroSerie, string keyTest, string lenguage, int noCycles, int totalTime, int interval, decimal timeLevel, decimal outputLevel, int descMayPc, int descMayMv, int incMaxPc, string measurementType, string terminalsTest)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateDPR"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["Lenguage"] = lenguage;
            query["KeyTest"] = keyTest;
            query["NoCycles"] = noCycles.ToString();
            query["TotalTime"] = totalTime.ToString();
            query["Interval"] = interval.ToString();
            query["TimeLevel"] = timeLevel.ToString();
            query["OutputLevel"] = outputLevel.ToString();
            query["DescMayPc"] = descMayPc.ToString();
            query["DescMayMv"] = descMayMv.ToString();
            query["IncMaxPc"] = incMaxPc.ToString();
            query["MeasurementType"] = measurementType.ToString();
            query["TerminalsTest"] = terminalsTest.ToString();

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayDPRReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayCGDReportsDTO>> GetTemplateCGD(string nroSerie, string keyTest, string lenguage, int temperatureHour1, int temperatureHour2, int temperatureHour3, string oilType)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateCGD"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["Lenguage"] = lenguage;
            query["KeyTest"] = keyTest;
            query["TypeOil"] = oilType;
            query["hour1"] = temperatureHour1.ToString();
            query["hour2"] = temperatureHour2.ToString();
            query["hour3"] = temperatureHour3.ToString();

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayCGDReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayRDDReportsDTO>> GetTemplateRDD(string nroSerie, string keyTest, string lenguage, string configWinding, string connection, decimal porcZ, decimal porcJx)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateRDD"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["Lenguage"] = lenguage;
            query["KeyTest"] = keyTest;
            query["ConfigWinding"] = configWinding;
            query["Connection"] = connection;
            query["PorcZ"] = porcZ.ToString("G", CultureInfo.InvariantCulture);
            query["PorcJx"] = porcJx.ToString("G", CultureInfo.InvariantCulture);

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayRDDReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayARFReportsDTO>> GetTemplateARF(string nroSerie, string keyTest, string lenguage, string team, string tertiary2Low, string tertiaryDisp, string levelsVoltage)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateARF"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["Lenguage"] = lenguage;
            query["KeyTest"] = keyTest;
            query["Team"] = team;
            query["Tertiary2Low"] = tertiary2Low;
            query["TertiaryDisp"] = tertiaryDisp;
            query["LevelsVoltage"] = levelsVoltage;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayARFReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayFPAReportsDTO>> GetTemplateFPA(string nroSerie, string keyTest, string lenguage, string oilType, int nroCol)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateFPA"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["Lenguage"] = lenguage;
            query["KeyTest"] = keyTest;
            query["OilType"] = oilType;
            query["nroCol"] = nroCol.ToString();

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayFPAReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayINDReportsDTO>> GetTemplateIND(string nroSerie, string keyTest, string lenguage, string tCBuyers)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportTemplateIND"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = nroSerie;
            query["Lenguage"] = lenguage;
            query["KeyTest"] = keyTest;
            query["TCBuyers"] = tCBuyers;

            string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
            query["tokenSesion"] = "";
            uriBuilder.Query = query.ToString();

            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayINDReportsDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<SettingsToDisplayNRAReportsDTO>> GetTemplateNRA(string nroSerie, string keyTest, string language, bool loadData, string selectloadData, string laboratory, string rule, string feeding, decimal feedingKVValue, string coolingType, int amountMeasureExistsOctaveInfo, DateTime? dataDate, int cantidadMaximadeMediciones)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["ReportTemplateNRA"];
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["NroSerie"] = nroSerie;
                query["KeyTest"] = keyTest;
                query["Language"] = language;
                query["loadData"] = loadData.ToString();
                query["selectloadData"] = selectloadData;
                query["Laboratory"] = laboratory;
                query["Rule"] = rule;
                query["Feeding"] = feeding;
                query["FeedingKVValue"] = feedingKVValue.ToString();
                query["CoolingType"] = coolingType;
                query["AmountMeasureExistsOctaveInfo"] = amountMeasureExistsOctaveInfo.ToString();
                query["DataDate"] = dataDate == null ? "" : dataDate.Value.ToString("yyyy-MM-dd");
                query["cantidadMaximadeMediciones"] = cantidadMaximadeMediciones.ToString();

                string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
                query["tokenSesion"] = "";
                uriBuilder.Query = query.ToString();

                this._httpClient.DefaultRequestHeaders.Clear();
                this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayNRAReportsDTO>>(stream, this._options);
            }
            catch (Exception e)
            {
                return new ApiResponse<SettingsToDisplayNRAReportsDTO>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = null
                };
            }
        }

        public async Task<ApiResponse<SettingsToDisplayETDReportsDTO>> GetTemplateETD(string nroSerie, string keyTest, string lenguage, bool typeRegression, string overload, bool btDifCap, decimal capacityBt, string tertiary2B, bool terCapRed, decimal capacityTer, string windingSplit, decimal idCuttingData, decimal connection)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["ReportTemplateNRA"];
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["NroSerie"] = nroSerie;
                query["KeyTest"] = keyTest;
                query["Lenguage"] = lenguage;
                query["TypeRegression"] = typeRegression.ToString();
                query["Overload"] = overload;
                query["BtDifCap"] = btDifCap.ToString();
                query["CapacityBt"] = capacityBt.ToString();
                query["Tertiary2B"] = tertiary2B;
                query["TerCapRed"] = terCapRed.ToString();
                query["CapacityTer"] = capacityTer.ToString();
                query["WindingSplit"] = windingSplit;
                query["IdCuttingData"] = idCuttingData.ToString();
                query["Connection"] = connection.ToString();

                string token = _AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
                query["tokenSesion"] = "";
                uriBuilder.Query = query.ToString();

                this._httpClient.DefaultRequestHeaders.Clear();
                this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayETDReportsDTO>>(stream, this._options);
            }
            catch (Exception e)
            {
                return new ApiResponse<SettingsToDisplayETDReportsDTO>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = null
                };
            }
        }

        public async Task<ApiResponse<SettingsToDisplayETDReportsDTO>> GetUploadConfigurationETD(string nroSerie, string keyTest, string lenguage)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["ReportTemplateETD"];
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["NroSerie"] = nroSerie;
                query["KeyTest"] = keyTest;
                query["Lenguage"] = lenguage;

                string token = this._AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
                query["tokenSesion"] = "";
                uriBuilder.Query = query.ToString();

                this._httpClient.DefaultRequestHeaders.Clear();
                this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayETDReportsDTO>>(stream, this._options);
            }
            catch (Exception e)
            {
                return new ApiResponse<SettingsToDisplayETDReportsDTO>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = null
                };
            }
        }

        public async Task<ApiResponse<SettingsToDisplayETDReportsDTO>> GetDownloadTemplateETD(string nroSerie, string keyTest, string lenguage)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["ReportTemplateETD"];
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["NroSerie"] = nroSerie;
                query["KeyTest"] = keyTest;
                query["Lenguage"] = lenguage;
                string token = this._AAD.getTokenSesionGatewayAsync((int)MicroservicesEnum.splconfiguration).Result;
                query["tokenSesion"] = "";
                uriBuilder.Query = query.ToString();
                this._httpClient.DefaultRequestHeaders.Clear();
                this._httpClient.DefaultRequestHeaders.Add("ApiKey", token);
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<SettingsToDisplayETDReportsDTO>>(stream, this._options);
            }
            catch (Exception e)
            {
                return new ApiResponse<SettingsToDisplayETDReportsDTO>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = null
                };
            }
        }
    }
}
