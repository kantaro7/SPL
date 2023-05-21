namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.SecurityApis;

    public class RanClientService : IRanClientService
    {
        private  HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IValidateAccesApis _AAD;
        public RanClientService(HttpClient httpClient, IConfiguration config, IValidateAccesApis aad)
        {
            this._configuration = config;
            this._httpClient = httpClient;
            this._options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
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

        public async Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string nroSerie)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["ReportsFilter"];
            string url = uriBuilder.Uri + "/" + nroSerie + "/" + ReportType.RAN;

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<List<InfoGeneralTypesReportsDTO>> infoGnralTypesReports = await JsonSerializer.DeserializeAsync<ApiResponse<List<InfoGeneralTypesReportsDTO>>>(stream, this._options);
            return infoGnralTypesReports;
        }

        public async Task<ApiResponse<ResultRANTestsDTO>> CalculateTestRAN(RANTestsDetailsDTO dto)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["CalculateRAN"];
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(new List<RANTestsDetailsDTO>() { dto }, this._options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<ResultRANTestsDTO> resultRanTest = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spltests).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
                resultRanTest = await JsonSerializer.DeserializeAsync<ApiResponse<ResultRANTestsDTO>>(stream, this._options);
            }
            return resultRanTest;
        }

        public async Task<ApiResponse<long>> SaveReport(RANReportDTO dto)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["SaveReportRAN"];
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(dto, this._options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> resultRadTest = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
                resultRadTest = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, this._options);
            }
            return resultRadTest;
        }
    }
}
