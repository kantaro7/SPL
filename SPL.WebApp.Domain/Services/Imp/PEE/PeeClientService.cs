namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Helpers;
    using SPL.WebApp.Domain.SecurityApis;

    public class PeeClientService : IPeeClientService
    {
        private  HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IValidateAccesApis _AAD;
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

        public PeeClientService(HttpClient httpClient, IConfiguration config, IValidateAccesApis aad)
        {
            this._configuration = config;
            this._httpClient = httpClient;
            this._options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            this._AAD = aad;
        }

        public async Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string nroSerie)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportsFilter"];
            string url = uriBuilder.Uri + "/" + nroSerie + "/" + ReportType.PEE;

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<List<InfoGeneralTypesReportsDTO>> infoGnralTypesReports = await JsonSerializer.DeserializeAsync<ApiResponse<List<InfoGeneralTypesReportsDTO>>>(stream, this._options);
            return infoGnralTypesReports;
        }

        public async Task<ApiResponse<ResultPEETestsDTO>> CalculateTestPEE(PEETestsDTO dto)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = _configuration["CalculatePEE"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(dto, _options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<ResultPEETestsDTO> resultPeeTest = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spltests).Result;
            using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                resultPeeTest = await JsonSerializer.DeserializeAsync<ApiResponse<ResultPEETestsDTO>>(stream, _options);
            }
            return resultPeeTest;
        }

        public async Task<ApiResponse<long>> SaveReport(PEETestsGeneralDTO dto)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = _configuration["SaveReportPEE"];
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(dto, _options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> resultPeeTest = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
            using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                    return await ParseUnprocessableEntity422HttpResponse<long>.UnprocessableEntityApiResponse(response);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                resultPeeTest = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            }
            return resultPeeTest;
        }

        public async Task<ApiResponse<PEETestsGeneralDTO>> GetInfoReportPEE(string nroSerie, string keyTest, bool result)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetInfoReportPEE"];
            string url = uriBuilder.Uri + "/" + nroSerie + "/" + keyTest + "/" + result;

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<PEETestsGeneralDTO> infoGnralTypesReports = await JsonSerializer.DeserializeAsync<ApiResponse<PEETestsGeneralDTO>>(stream, this._options);
            return infoGnralTypesReports;
        }
    }
}
