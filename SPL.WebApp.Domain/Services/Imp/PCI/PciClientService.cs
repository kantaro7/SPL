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

    public class PciClientService : IPciClientService
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

        public PciClientService(HttpClient httpClient, IConfiguration config, IValidateAccesApis aad)
        {
            this._configuration = config;
            this._httpClient = httpClient;
            this._options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            this._AAD = aad;
        }

        public async Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string nroSerie)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["ReportsFilter"];
                string url = uriBuilder.Uri + "/" + nroSerie + "/" + ReportType.PCI;
                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                ApiResponse<List<InfoGeneralTypesReportsDTO>> infoGnralTypesReports = await JsonSerializer.DeserializeAsync<ApiResponse<List<InfoGeneralTypesReportsDTO>>>(stream, this._options);
                return infoGnralTypesReports;
            }
            catch (Exception ex) { return null; }
        }

        public async Task<ApiResponse<PCITestResponseDTO>> CalculateTestPCI(PCIInputTestDTO dtos)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = _configuration["CalculatePCI"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dtos, _options),
                      Encoding.UTF8,
                      "application/json");
                ApiResponse<PCITestResponseDTO> resultRdtTest = new();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spltests).Result;
                using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
                {
                    _ = response.EnsureSuccessStatusCode();
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    resultRdtTest = await JsonSerializer.DeserializeAsync<ApiResponse<PCITestResponseDTO>>(stream, _options);
                }
                return resultRdtTest;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<long>> SaveReport(PCITestGeneralDTO dto)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = _configuration["SaveReportPCI"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");
                ApiResponse<long> resultRdtTest = new();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
                using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
                {
                    if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                        return await ParseUnprocessableEntity422HttpResponse<long>.UnprocessableEntityApiResponse(response);
                    _ = response.EnsureSuccessStatusCode();
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    resultRdtTest = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
                }
                return resultRdtTest;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<PCITestGeneralDTO>> GetInfoReportPCI(string nroSerie, string keyTest, bool result)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetInfoReportPCI"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
            string url = uriBuilder.Uri + "/" + nroSerie + "/" + keyTest + "/" + result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<PCITestGeneralDTO> infoGnralTypesReports = await JsonSerializer.DeserializeAsync<ApiResponse<PCITestGeneralDTO>>(stream, this._options);
            return infoGnralTypesReports;
        }
    }
}
