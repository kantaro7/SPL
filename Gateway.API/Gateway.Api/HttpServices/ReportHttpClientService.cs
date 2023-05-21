namespace Gateway.Api.HttpServices
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web;

    using Gateway.Api.Dtos;
    using Gateway.Api.Dtos.ArtifactDesing;
    using Gateway.Api.Dtos.Nozzle;
    using Gateway.Api.Dtos.Reports.CGD;

    using Microsoft.Extensions.Configuration;

    using SPL.Domain;

    public class ReportHttpClientService : IReportHttpClientService
    {

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;

        private UriBuilder GetUriBuilder()
        {
            UriBuilder uriBuilder = new()
            {
                Host = _configuration["GateWayAPIDomain"],
                Scheme = _configuration["SchemeGateWayAPI"]
            };

            string port = _configuration["GateWayAPIPort"];
            if (!string.IsNullOrEmpty(port))
                uriBuilder.Port = int.Parse(port);

            return uriBuilder;
        }

        public ReportHttpClientService(HttpClient httpClient, IConfiguration config)
        {
            _configuration = config;
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<ApiResponse<List<FilterReportsDto>>> GetFilterReports(string TypeReport, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetFilterReports"];

            string uri = string.Concat(_uriBuilder.Path, "/", TypeReport.ToString());

            _httpClient.DefaultRequestHeaders.Authorization =
                   new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<List<FilterReportsDto>> result = await JsonSerializer.DeserializeAsync<ApiResponse<List<FilterReportsDto>>>(stream, _options);

            return result;
        }

        public async Task<ApiResponse<List<TestsDto>>> GetTests(string typeReport, string keyTest, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetTest"];

            string uri = string.Concat(_uriBuilder.Path, "/", typeReport.ToString(), "/", keyTest.ToString());

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<List<TestsDto>> result = await JsonSerializer.DeserializeAsync<ApiResponse<List<TestsDto>>>(stream, _options);

            return result;
        }

        public async Task<List<GeneralPropertiesDto>> GetTypeUnit(string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetUnitTypes"];

            _httpClient.DefaultRequestHeaders.Authorization =
              new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Path, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            List<GeneralPropertiesDto> result = await JsonSerializer.DeserializeAsync<List<GeneralPropertiesDto>>(stream, _options);

            return result;
        }

        public async Task<ApiResponse<List<GeneralPropertiesDto>>> GetThirdWinding(string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetThirdWinding"];

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);

            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Path, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<List<GeneralPropertiesDto>> result = await JsonSerializer.DeserializeAsync<ApiResponse<List<GeneralPropertiesDto>>>(stream, _options);

            return result;
        }

        public async Task<List<GeneralPropertiesDto>> GetAngularDisplacement(string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetEquivalentsAngularDisplacement"];

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Path, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            List<GeneralPropertiesDto> result = await JsonSerializer.DeserializeAsync<List<GeneralPropertiesDto>>(stream, _options);

            return result;
        }

        public async Task<List<GeneralPropertiesDto>> GetRules(string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetRulesEquivalents"];

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Path, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            List<GeneralPropertiesDto> result = await JsonSerializer.DeserializeAsync<List<GeneralPropertiesDto>>(stream, _options);

            return result;
        }

        public async Task<InformationArtifactDto> GetArtifact(string nroSerie, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GeneralArtifact"];
            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);
            query["nroSerie"] = nroSerie;
            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            InformationArtifactDto artifact = await JsonSerializer.DeserializeAsync<InformationArtifactDto>(stream, _options);

            return artifact;
        }

        public async Task<List<CatSidcoInformationDto>> GetCatSIDCO(string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetCatSIDCO"];

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            List<CatSidcoInformationDto> generalProperties = await JsonSerializer.DeserializeAsync<List<CatSidcoInformationDto>>(stream, _options);

            return generalProperties;
        }

        public async Task<ApiResponse<List<ConfigurationReportsDto>>> GetConfigurationReports(string typeReport, string keyTest, long numberColumns, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetConfigurationReports"];

            string uri = string.Concat(_uriBuilder.Path, "/", typeReport.ToString(), "/", keyTest.ToString(), "/", numberColumns.ToString());

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<List<ConfigurationReportsDto>> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<List<ConfigurationReportsDto>>>(stream, _options);

            return generalProperties;
        }

        public async Task<ApiResponse<BaseTemplateDto>> GetBaseTemplate(string typeReport, string keyTest, string keyLanguage, long numberColumns, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetBaseTemplate"];

            string uri = string.Concat(_uriBuilder.Path, "/", typeReport.ToString(), "/", keyTest.ToString(), "/", keyLanguage, "/" + numberColumns);

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<BaseTemplateDto> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<BaseTemplateDto>>(stream, _options);

            return generalProperties;
        }

        public async Task<ApiResponse<List<ColumnTitleRADReportsDto>>> GetTitleColumnsRAD(string typeUnit, string thirdWinding, string lenguage, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetTitleColumnsRAD"];

            string uri = string.Concat(_uriBuilder.Path, "/", typeUnit.ToString(), "/", thirdWinding.ToString(), "/", lenguage);

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<List<ColumnTitleRADReportsDto>> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<List<ColumnTitleRADReportsDto>>>(stream, _options);

            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextRAD(string nroSerie, string keyTest, string typeUnit, string thirdWinding, string lenguage, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextRAD"];

            string uri = string.Concat(_uriBuilder.Path, "/", nroSerie.ToString(), "/", keyTest.ToString(), "/", typeUnit, "/", thirdWinding, "/" + lenguage);

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);

            return generalProperties;
        }

        public async Task<ApiResponse<List<ColumnTitleRDTReportsDto>>> GetTitleColumnsRDT(string keyTest, string dAngular, string rule, string lenguage, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetTitleColumnsRDT"];

            string uri = string.Concat(_uriBuilder.Path, "/", keyTest, "/", dAngular, "/", rule, "/", lenguage);

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<List<ColumnTitleRDTReportsDto>> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<List<ColumnTitleRDTReportsDto>>>(stream, _options);

            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextRDT(string NroSerie, string KeyTest, string DAngular, string Rule, string Lenguage, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextRDT"];

            string uri = string.Concat(_uriBuilder.Path, "/", NroSerie.ToString(), "/", KeyTest.ToString(), "/", DAngular, "/", Rule, "/" + Lenguage);

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);

            return generalProperties;
        }

        public async Task<ApiResponse<List<PlateTensionDto>>> GetPlateTension(string nroSerie, string pType, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetPlateTension"];

            string uri = string.Concat(_uriBuilder.Path, "/", nroSerie, "/", pType);

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<List<PlateTensionDto>> result = await JsonSerializer.DeserializeAsync<ApiResponse<List<PlateTensionDto>>>(stream, _options);

            return result;
        }

        public async Task<ApiResponse<long>> GetNroTestNextRAN(string nroSerie, string keyTest, string lenguage, int numberMeasurements, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextRAN"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);
            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["numberMeasurements"] = numberMeasurements.ToString();

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);

            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextFPC(string nroSerie, string keyTest, string typeUnit, string specification, string frequency, string lenguage, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextFPC"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["typeUnit"] = typeUnit;
            query["specification"] = specification;
            query["frequency"] = frequency;
            query["lenguage"] = lenguage;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);

            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);

            return generalProperties;
        }

        public async Task<ApiResponse<List<ColumnTitleFPCReportsDto>>> GetTitleColumnsFPC(string typeUnit, string lenguage, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetTitleColumnsFPC"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["typeUnit"] = typeUnit;

            query["lenguage"] = lenguage;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<List<ColumnTitleFPCReportsDto>> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<List<ColumnTitleFPCReportsDto>>>(stream, _options);

            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextROD(string noSerie, string keyTest, string lenguage, string connection, string unitType, string material, string unitOfMeasurement, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextROD"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["noSerie"] = noSerie;
            query["keyTest"] = keyTest;
            query["connection"] = connection;
            query["unitType"] = unitType;
            query["material"] = material;
            query["unitOfMeasurement"] = unitOfMeasurement;
            query["lenguage"] = lenguage;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<CorrectionFactorsDescDto>> GetCorrectionFactorsDesc(string pSpecification, string pKeyLenguage, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetCorrectionFactorsDesc"];
            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);
            query["pSpecification"] = pSpecification;
            query["pKeyLenguage"] = pKeyLenguage;
            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<CorrectionFactorsDescDto> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<CorrectionFactorsDescDto>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextFPB(string nroSerie, string keyTest, string lenguage, string tangentDelta, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextFPB"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);
            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["tangentDelta"] = tangentDelta;
            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextRCT(string nroSerie, string keyTest, string lenguage, string unitOfMeasurement, string tertiary, decimal testvoltage, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextRCT"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["unitOfMeasurement"] = unitOfMeasurement;
            query["tertiary"] = tertiary;
            query["testvoltage"] = testvoltage.ToString().Replace(',', '.');
            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<NozzlesByDesignDto>> GetInformationBoqDet(string nroSerie, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetInformationBoqDet"];
            string uri = string.Concat(_uriBuilder.Path, "/", nroSerie);

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<NozzlesByDesignDto> result = await JsonSerializer.DeserializeAsync<ApiResponse<NozzlesByDesignDto>>(stream, _options);
            return result;
        }

        public async Task<ApiResponse<long>> GetNroTestNextPCE(string nroSerie, string keyTest, string lenguage, string energizedWinding, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextPCE"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["energizedWinding"] = energizedWinding;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextPCI(string nroSerie, string keyTest, string lenguage, string windingMaterial, bool capRedBaja, bool autotransformer, bool monofasico, decimal overElevation, string posPi, string posSec, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextPCI"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["windingMaterial"] = windingMaterial;
            query["capRedBaja"] = capRedBaja.ToString();
            query["autotransformer"] = autotransformer.ToString();
            query["monofasico"] = monofasico.ToString();
            query["overElevation"] = overElevation.ToString();
            query["posPi"] = posPi;
            query["posSec"] = posSec;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextPLR(string nroSerie, string keyTest, string lenguage, decimal rldnt, decimal nominalVoltage, int amountOfTensions, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextPLR"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["rldnt"] = rldnt.ToString();
            query["nominalVoltage"] = nominalVoltage.ToString();
            query["amountOfTensions"] = amountOfTensions.ToString();

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextPRD(string nroSerie, string keyTest, string lenguage, decimal nominalVoltage, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextPRD"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["nominalVoltage"] = nominalVoltage.ToString();

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextPEE(string nroSerie, string keyTest, string lenguage, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextPEE"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextISZ(string nroSerie, string keyTest, string lenguage,
           decimal degreesCor, string posAT, string posBT, string posTER, string windingEnergized, int qtyNeutral, decimal impedanceGar, string materialWinding, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextISZ"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["degreesCor"] = degreesCor.ToString();
            query["posAT"] = posAT;
            query["posBT"] = posBT;
            query["posTER"] = posTER;
            query["windingEnergized"] = windingEnergized;
            query["qtyNeutral"] = qtyNeutral.ToString();
            query["impedanceGar"] = impedanceGar.ToString();
            query["materialWinding"] = materialWinding;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextRYE(string nroSerie, string keyTest, string lenguage,
            int noConnectionsAT, int noConnectionsBT, int noConnectionsTER, decimal voltageAT, decimal voltageBT, decimal voltageTER, string coolingType, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextRYE"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["noConnectionsAT"] = noConnectionsAT.ToString();
            query["noConnectionsBT"] = noConnectionsBT.ToString();
            query["noConnectionsTER"] = noConnectionsTER.ToString();
            query["voltageAT"] = voltageAT.ToString();
            query["voltageBT"] = voltageBT.ToString();
            query["voltageTER"] = voltageTER.ToString();
            query["coolingType"] = coolingType.ToString();

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextPIM(string nroSerie, string keyTest, string lenguage,
          string connection, string applyLow, string voltageLevel, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextPIM"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["connection"] = connection;
            query["applyLow"] = applyLow;
            query["voltageLevel"] = voltageLevel;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextPIR(string nroSerie, string keyTest, string lenguage,
          string connection, string includesTertiary, string voltageLevel, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextPIR"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["connection"] = connection;
            query["includesTertiary"] = includesTertiary;
            query["voltageLevel"] = voltageLevel;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextTAP(string nroSerie, string keyTest, string lenguage,
      string unitType, int noConnectionAT, int noConnectionBT, int noConnectionTER, string idCapAT, string idCapBT, string idCapTer, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextTAP"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["unitType"] = unitType;
            query["noConnectionAT"] = noConnectionAT.ToString();
            query["noConnectionBT"] = noConnectionBT.ToString();
            query["noConnectionTER"] = noConnectionTER.ToString();
            query["idCapAT"] = idCapAT;
            query["idCapBT"] = idCapBT;
            query["idCapTer"] = idCapTer;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextTIN(string nroSerie, string keyTest, string lenguage,
        string connection, decimal voltage, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextTIN"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["connection"] = connection;
            query["voltage"] = voltage.ToString();

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextCEM(string nroSerie, string keyTest, string lenguage,
       string idPosPrimary, string posPrimary, string idPosSecundary, string posSecundary, decimal testsVoltage, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextCEM"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["idPosPrimary"] = idPosPrimary;
            query["posPrimary"] = posPrimary.ToString();
            query["idPosSecundary"] = idPosSecundary;
            query["posSecundary"] = posSecundary.ToString();
            query["testsVoltage"] = testsVoltage.ToString();

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<List<ColumnTitleCEMReportsDto>>> GetTitleColumnsCEM(decimal typeTrafoId, string keyLenguage, string pos, string posSecundary, string noSerieNormal, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetTitleColumnsCEM"];

            string uri = string.Concat(_uriBuilder.Path, "/", typeTrafoId.ToString(), "/", keyLenguage.ToString(), "/", pos,"/",posSecundary,"/",noSerieNormal);

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<List<ColumnTitleCEMReportsDto>> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<List<ColumnTitleCEMReportsDto>>>(stream, _options);

            return new ApiResponse<List<ColumnTitleCEMReportsDto>>
            {
                Code = generalProperties.Code,
                Description = generalProperties.Description,
                Structure = generalProperties.Structure
            };
        }

        public async Task<ApiResponse<long>> GetNroTestNextCGD(string nroSerie, string keyTest, string lenguage, string typeOil, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextCGD"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["typeOil"] = typeOil;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextTDP(string nroSerie, string keyTest, string lenguage,
int noCycles, int totalTime, int interval, decimal timeLevel, decimal outputLevel, int descMayPc, int descMayMv, int incMaxPc, string voltageLevels, string measurementType, string terminalsTest, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextTDP"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["noCycles"] = noCycles.ToString();
            query["totalTime"] = totalTime.ToString();
            query["interval"] = interval.ToString();
            query["timeLevel"] = timeLevel.ToString();
            query["outputLevel"] = outputLevel.ToString();
            query["descMayPc"] = descMayPc.ToString();
            query["descMayMv"] = descMayMv.ToString();
            query["incMaxPc"] = incMaxPc.ToString();
            query["voltageLevels"] = voltageLevels;
            query["measurementType"] = measurementType;
            query["terminalsTest"] = terminalsTest;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextRDD(string nroSerie, string keyTest, string lenguage,
string config_Winding, string connection, decimal porc_Z, decimal porc_Jx, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextRDD"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["config_Winding"] = config_Winding;
            query["connection"] = connection;
            query["porc_Z"] = porc_Z.ToString();
            query["porc_Jx"] = porc_Jx.ToString();

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextARF(string nroSerie, string keyTest, string lenguage,
string team, string tertiary2Low, string tertiaryDisp, string levelsVoltage, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextARF"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["team"] = team;
            query["tertiary2Low"] = tertiary2Low;
            query["tertiaryDisp"] = tertiaryDisp;
            query["levelsVoltage"] = levelsVoltage;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextIND(string nroSerie, string keyTest, string lenguage,
string tcPurchased, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextIND"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["tcPurchased"] = tcPurchased;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextFPA(string nroSerie, string keyTest, string lenguage, string typeOil, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextFPA"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["typeOil"] = typeOil;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextBPC(string nroSerie, string keyTest, string lenguage, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextBPC"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<List<ContGasCGDDto>>> GetInfoContGasCGD(string IdReport, string KeyTests, string TypeOil, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetInfoContGasCGD"];

            string uri = string.Concat(_uriBuilder.Path, "/", IdReport, "/", KeyTests, "/", TypeOil);

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<List<ContGasCGDDto>> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<List<ContGasCGDDto>>>(stream, _options);

            return new ApiResponse<List<ContGasCGDDto>>
            {
                Code = generalProperties.Code,
                Description = generalProperties.Description,
                Structure = generalProperties.Structure
            };
        }

        public async Task<ApiResponse<CGDTestsGeneralDto>> GetInfoCGD(string NroSerie, string KeyTests, bool Result, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetInfoCGD"];

            string uri = string.Concat(_uriBuilder.Path, "/", NroSerie, "/", KeyTests, "/", Result);

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<CGDTestsGeneralDto> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<CGDTestsGeneralDto>>(stream, _options);

            return new ApiResponse<CGDTestsGeneralDto>
            {
                Code = generalProperties.Code,
                Description = generalProperties.Description,
                Structure = generalProperties.Structure
            };
        }

        public async Task<ApiResponse<long>> GetNroTestNextNRA(string nroSerie, string keyTest, string language, string laboratory, string rule, string feeding, string coolingType, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextNRA"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["language"] = language;
            query["laboratory"] = laboratory;
            query["rule"] = rule;
            query["feeding"] = feeding;
            query["coolingType"] = coolingType;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextETD(string nroSerie, string keyTest, string language, bool typeRegression, bool btDifCap, decimal capacityBt, string tertiary2B, bool terCapRed, decimal capacityTer, string windingSplit, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextETD"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["language"] = language;
            query["typeRegression"] = typeRegression.ToString();
            query["btDifCap"] = btDifCap.ToString();
            query["capacityBt"] = capacityBt.ToString();
            query["tertiary2B"] = tertiary2B.ToString();
            query["terCapRed"] = terCapRed.ToString();
            query["capacityTer"] = capacityTer.ToString();
            query["windingSplit"] = windingSplit;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<long>> GetNroTestNextDPR(string nroSerie, string keyTest, string lenguage, int noCycles,
 int totalTime, int interval, decimal timeLevel, decimal outputLevel, int descMayPc, int descMayMv, int incMaxPc, string measurementType, string terminalsTest, string tokenSesion)
        {

            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetNumNextDPR"];

            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            query["nroSerie"] = nroSerie;
            query["keyTest"] = keyTest;
            query["lenguage"] = lenguage;
            query["noCycles"] = noCycles.ToString();

            query["totalTime"] = totalTime.ToString();
            query["interval"] = interval.ToString();
            query["timeLevel"] = timeLevel.ToString();
            query["outputLevel"] = outputLevel.ToString();
            query["descMayPc"] = descMayPc.ToString();
            query["descMayMv"] = descMayMv.ToString();
            query["incMaxPc"] = incMaxPc.ToString();

            query["measurementType"] = measurementType;
            query["terminalsTest"] = terminalsTest;

            _uriBuilder.Query = query.ToString();

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<long> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            return generalProperties;
        }

        public async Task<ApiResponse<List<InformationOctavesDto>>> GetInformationOctaves(string NroSerie, string TypeInformation, string DateData, string tokenSesion)
        {
            try
            {
                UriBuilder _uriBuilder = GetUriBuilder();
                _uriBuilder.Path = _configuration["GetInformationOctaves"];

                System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

                query["NroSerie"] = NroSerie;
                query["TypeInformation"] = TypeInformation;
                query["DateData"] = DateData;

                _uriBuilder.Query = query.ToString();

                _httpClient.DefaultRequestHeaders.Authorization =
                     new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
                using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
                ApiResponse<List<InformationOctavesDto>> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<List<InformationOctavesDto>>>(stream, _options);
                return generalProperties;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ApiResponse<HeaderCuttingDataDto>> GetInfoHeaderCuttingData(decimal id, string tokenSesion)
        {
            try
            {
                UriBuilder _uriBuilder = GetUriBuilder();
                _uriBuilder.Path = _configuration["GetInfoHeaderCuttingData"] + $"/{id}";

                System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);
                _httpClient.DefaultRequestHeaders.Authorization =
                     new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
                using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
                ApiResponse<HeaderCuttingDataDto> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<HeaderCuttingDataDto>>(stream, _options);
                return generalProperties;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ApiResponse<HeaderCuttingDataDto>> GetInfoHeaderCuttingData(string NroSerie, string tokenSesion)
        {
            try
            {
                UriBuilder _uriBuilder = GetUriBuilder();
                _uriBuilder.Path = _configuration["GetInfoHeaderCuttingData"];

                System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

                query["NroSerie"] = NroSerie;

                _uriBuilder.Query = query.ToString();

                _httpClient.DefaultRequestHeaders.Authorization =
                     new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
                using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
                ApiResponse<HeaderCuttingDataDto> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<HeaderCuttingDataDto>>(stream, _options);
                return generalProperties;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ApiResponse<CGDTestsGeneralDto>> GetInfoHeaderCuttingData(string NroSerie, string KeyTests, bool Result, string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetInfoHeaderCuttingData"];

            string uri = string.Concat(_uriBuilder.Path, "/", NroSerie, "/", KeyTests, "/", Result);

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<CGDTestsGeneralDto> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<CGDTestsGeneralDto>>(stream, _options);

            return new ApiResponse<CGDTestsGeneralDto>
            {
                Code = generalProperties.Code,
                Description = generalProperties.Description,
                Structure = generalProperties.Structure
            };
        }

        public async Task<ApiResponse<List<StabilizationDataDto>>> GetStabilizationData(string NroSerie, bool? Status, bool? Stabilized, string tokenSesion)
        {
            try
            {
                UriBuilder _uriBuilder = GetUriBuilder();
                _uriBuilder.Path = _configuration["GetStabilizationData"];

                System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(_uriBuilder.Query);

                query["NroSerie"] = NroSerie;
                query["Status"] = Status == null ? "null" : Status.ToString();
                query["Stabilized"] = Stabilized == null ? "null" : Stabilized.ToString();

                _uriBuilder.Query = query.ToString();

                _httpClient.DefaultRequestHeaders.Authorization =
                     new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
                using HttpResponseMessage response = await _httpClient.GetAsync(_uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
                ApiResponse<List<StabilizationDataDto>> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<List<StabilizationDataDto>>>(stream, _options);
                return generalProperties;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ApiResponse<StabilizationDesignDataDto>> GetStabilizationDesignData(string NroSerie, string tokenSesion)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["GetStabilizationDesignData"] + string.Format("/{0}", NroSerie);

                _httpClient.DefaultRequestHeaders.Authorization =
                     new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
                ApiResponse<StabilizationDesignDataDto> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<StabilizationDesignDataDto>>(stream, _options);
                return generalProperties;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<List<CorrectionFactorkWTypeCoolingDto>>> GetCorrectionFactorkWTypeCooling(string tokenSesion)
        {
            UriBuilder _uriBuilder = GetUriBuilder();
            _uriBuilder.Path = _configuration["GetCorrectionFactorkWTypeCooling"];

            string uri = string.Concat(_uriBuilder.Path);

            _httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<List<CorrectionFactorkWTypeCoolingDto>> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<List<CorrectionFactorkWTypeCoolingDto>>>(stream, _options);

            return new ApiResponse<List<CorrectionFactorkWTypeCoolingDto>>
            {
                Code = generalProperties.Code,
                Description = generalProperties.Description,
                Structure = generalProperties.Structure
            };
        }

        public async Task<ApiResponse<List<ETDConfigFileReportDto>>> GetConfigurationETDDownload(string tokenSesion)
        {
            UriBuilder _uriBuilder = this.GetUriBuilder();
            _uriBuilder.Path = this._configuration["GetConfigFileETD"];
            this._httpClient.DefaultRequestHeaders.Authorization =
                 new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenSesion);
            using HttpResponseMessage response = await this._httpClient.GetAsync(_uriBuilder.Path, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();

            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();

            ApiResponse<List<ETDConfigFileReportDto>> generalProperties = await JsonSerializer.DeserializeAsync<ApiResponse<List<ETDConfigFileReportDto>>>(stream, this._options);

            return generalProperties;
        }
    }
}

