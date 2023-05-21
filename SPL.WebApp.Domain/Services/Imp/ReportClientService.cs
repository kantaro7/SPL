namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.SecurityApis;

    public class ReportClientService : IReportClientService
    {
        private  HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IValidateAccesApis _AAD;
        public ReportClientService(HttpClient httpClient, IConfiguration configuration, IValidateAccesApis aad)
        {
            this._configuration = configuration;
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

        public async Task<ApiResponse<IEnumerable<ReportsDTO>>> GetReportTypes()
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["Reports"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(string.Concat(uriBuilder.Uri, "/-1"), HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<IEnumerable<ReportsDTO>>>(stream, this._options);
        }

        public async Task<ApiResponse<ReportPDFDto>> GetPDFReport(long code, string typeReport)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["PDFReport"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(string.Concat(uriBuilder.Uri, "/" + code + "/" + typeReport), HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<ReportPDFDto>>(stream, this._options);
        }

        public async Task<ApiResponse<List<ConsolidatedReportDTO>>> GetConsolidatedReport(string noSerie, string languaje)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["ReportConsolidated"];

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri +"/"+ noSerie + "/" + languaje, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<List<ConsolidatedReportDTO>>>(stream, this._options);
            }
            catch(Exception e)
            {
                return new ApiResponse<List<ConsolidatedReportDTO>>
                {
                    Description = e.Message,
                    Structure = null ,
                    Code = -1
                };
            }
  
        }

        public async Task<ApiResponse<List<TypeConsolidatedReportDTO>>> GetTypeSectionConsolidatedReport(string noSerie, string languaje)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["TypeSectionConsolidatedReport"];

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri + "/" + noSerie + "/" + languaje, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<List<TypeConsolidatedReportDTO>>>(stream, this._options);
            }
            catch (Exception e)
            {
                return new ApiResponse<List<TypeConsolidatedReportDTO>>
                {
                    Description = e.Message,
                    Structure = null,
                    Code = -1
                };
            }

        }

        public async Task<ApiResponse<List<ConsolidatedReportDTO>>> GetTestedReport(string noSerie)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["ReportTested"];

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri + "/" + noSerie , HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<List<ConsolidatedReportDTO>>>(stream, this._options);
            }
            catch (Exception e)
            {
                return new ApiResponse<List<ConsolidatedReportDTO>>
                {
                    Description = e.Message,
                    Structure = null,
                    Code = -1
                };
            }

        }

        public async Task<ApiResponse<CheckInfoRODDTO>> CheckInfoRod(string noSerie, string windingMaterial, string atPositions, string btPositions, string terPositions, bool isAT, bool isBT, bool isTer)
        {

            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["CheckInfoRod"];

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri + "/" + noSerie + "/" + windingMaterial + "/" + atPositions + "/" + btPositions + "/" + terPositions
                    + "/" + isAT + "/" + isBT + "/" + isTer,  HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<CheckInfoRODDTO>>(stream, this._options);
            }
            catch (Exception e)
            {
                return new ApiResponse<CheckInfoRODDTO>
                {
                    Description = e.Message,
                    Structure = null,
                    Code = -1
                };
            }
        }

        public async Task<ApiResponse<CheckInfoRODDTO>> CheckInfoPce(string noSerie, string capacity, string atPositions, string btPositions, string terPositions, bool isAT, bool isBT, bool isTer)
        {

            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["CheckInfoPce"];

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri + "/" + noSerie + "/" + capacity + "/" + atPositions + "/" + btPositions + "/" + terPositions
                    + "/" + isAT + "/" + isBT + "/" + isTer, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<CheckInfoRODDTO>>(stream, this._options);
            }
            catch (Exception e)
            {
                return new ApiResponse<CheckInfoRODDTO>
                {
                    Description = e.Message,
                    Structure = null,
                    Code = -1
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<PCIParameters>>> GetAAsync(string noSerie, string windingMaterial, string capacity, string atPositions, string btPositions, string terPositions, bool isAT, bool isBT, bool isTer)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetA"];
            string url = uriBuilder.Uri + "/" + noSerie + "/" + windingMaterial+ "/" + capacity + "/" + atPositions + "/" + btPositions + "/" + terPositions + "/" + isAT + "/" + isBT + "/" + isTer; ;

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splreports).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<IEnumerable<PCIParameters>> res = await JsonSerializer.DeserializeAsync<ApiResponse<IEnumerable<PCIParameters>>>(stream, this._options);
            return res;
        }
    }
}
