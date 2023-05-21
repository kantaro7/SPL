namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web;

    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Helpers;
    using SPL.WebApp.Domain.SecurityApis;

    public class RadClientService : IRadClientService
    {
        private HttpClient _httpClient;
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

        public RadClientService(HttpClient httpClient, IConfiguration config, IValidateAccesApis aad)
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
            string url = uriBuilder.Uri + "/" + nroSerie + "/" + ReportType.RAD;

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<List<InfoGeneralTypesReportsDTO>> infoGnralTypesReports = await JsonSerializer.DeserializeAsync<ApiResponse<List<InfoGeneralTypesReportsDTO>>>(stream, this._options);
            return infoGnralTypesReports;
        }

        public async Task<ApiResponse<ResultRADTestsDTO>> CalculateTestRAD(RADTestsDTO dto)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["CalculateRAD"];
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(dto, this._options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<ResultRADTestsDTO> resultRadTest = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spltests).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
                resultRadTest = await JsonSerializer.DeserializeAsync<ApiResponse<ResultRADTestsDTO>>(stream, this._options);
            }
            return resultRadTest;
        }

        public async Task<ApiResponse<long>> SaveReport(RADReportDTO dto)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["SaveReportRAD"];
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(dto, this._options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> resultRadTest = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                    return await ParseUnprocessableEntity422HttpResponse<long>.UnprocessableEntityApiResponse(response);
                _ = response.EnsureSuccessStatusCode();
                System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
                resultRadTest = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, this._options);
            }
            return resultRadTest;
        }
    }
}
