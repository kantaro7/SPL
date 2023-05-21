namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web;

    using AutoMapper;

    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ETD;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Helpers;
    using SPL.WebApp.Domain.SecurityApis;

    public class ETDClientService : IETDClientService
    {
        private  HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IValidateAccesApis _AAD;
        public ETDClientService(
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

        public async Task<ApiResponse<StabilizationDesignDataDTO>> GetStabilizationDesignData(string nroSerie)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["GetStabilizationDesignData"] + string.Format("/{0}", nroSerie);

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                ApiResponse<StabilizationDesignDataDTO> result = await JsonSerializer.DeserializeAsync<ApiResponse<StabilizationDesignDataDTO>>(stream, this._options);
                return result;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<long>> SaveStabilizationDesignData(StabilizationDesignDataDTO dto)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = _configuration["SaveStabilizationDesignData"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");
                ApiResponse<long> result = new();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
                using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
                {
                    if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                        return await ParseUnprocessableEntity422HttpResponse<long>.UnprocessableEntityApiResponse(response);
                    _ = response.EnsureSuccessStatusCode();
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    result = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
                }
                return result;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<List<CorrectionFactorKWTypeCoolingDTO>>> GetCorrectionFactorkWTypeCooling()
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["GetCorrectionFactorkWTypeCooling"];

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                ApiResponse<List<CorrectionFactorKWTypeCoolingDTO>> result = await JsonSerializer.DeserializeAsync<ApiResponse<List<CorrectionFactorKWTypeCoolingDTO>>>(stream, this._options);
                return result;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<long>> SaveCorrectionFactorkWTypeCooling(List<CorrectionFactorKWTypeCoolingDTO> dto)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = _configuration["SaveCorrectionFactorkWTypeCooling"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");
                ApiResponse<long> result = new();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
                using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
                {
                    if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                        return await ParseUnprocessableEntity422HttpResponse<long>.UnprocessableEntityApiResponse(response);
                    _ = response.EnsureSuccessStatusCode();
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    result = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
                }
                return result;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<List<StabilizationDataDTO>>> GetStabilizationData(string nroSerie, bool? status = null, bool? stabilized = null)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["GetStabilizationData"];
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["NroSerie"] = nroSerie;
                query["Status"] = status.ToString();
                query["Stabilized"] = stabilized.ToString();
                uriBuilder.Query = query.ToString();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                ApiResponse<List<StabilizationDataDTO>> result = await JsonSerializer.DeserializeAsync<ApiResponse<List<StabilizationDataDTO>>>(stream, this._options);
                return result;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<ResultStabilizationDataTestsDTO>> CalculeTestStabilizationData(StabilizationDataDTO dto)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = _configuration["CalculeTestStabilizationData"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");
                ApiResponse<ResultStabilizationDataTestsDTO> result = new();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spltests).Result;
                using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
                {
                    if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                        return await ParseUnprocessableEntity422HttpResponse<ResultStabilizationDataTestsDTO>.UnprocessableEntityApiResponse(response);
                    _ = response.EnsureSuccessStatusCode();
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    result = await JsonSerializer.DeserializeAsync<ApiResponse<ResultStabilizationDataTestsDTO>>(stream, _options);
                }
                return result;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<long>> CloseStabilizationData(string nroSerie, decimal idReg) {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = _configuration["CloseStabilizationData"];
                var dto = new { NroSerie = nroSerie, IdReg = idReg };
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");
                ApiResponse<long> result = new();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
                using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
                {
                    if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                        return await ParseUnprocessableEntity422HttpResponse<long>.UnprocessableEntityApiResponse(response);
                    _ = response.EnsureSuccessStatusCode();
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    result = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
                }
                return result;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<long>> SaveStabilizationData(StabilizationDataDTO dto)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = _configuration["SaveStabilizationData"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");
                ApiResponse<long> result = new();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
                using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
                {
                    if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                        return await ParseUnprocessableEntity422HttpResponse<long>.UnprocessableEntityApiResponse(response);
                    _ = response.EnsureSuccessStatusCode();
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    result = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
                }
                return result;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<HeaderCuttingDataDTO>> GetInfoHeaderCuttingData(decimal idCut)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["GetInfoHeaderCuttingData"] + string.Format("/{0}", idCut);

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                ApiResponse<HeaderCuttingDataDTO> result = await JsonSerializer.DeserializeAsync<ApiResponse<HeaderCuttingDataDTO>>(stream, this._options);
                return result;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<List<HeaderCuttingDataDTO>>> GetCuttingDatas(string nroSerie)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["GetCuttingDatas"] + string.Format("/{0}", nroSerie);

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                ApiResponse<List<HeaderCuttingDataDTO>> result = await JsonSerializer.DeserializeAsync<ApiResponse<List<HeaderCuttingDataDTO>>>(stream, this._options);
                return result;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<ResultCuttingDataTestsDTO>> CalculateCuttingData(HeaderCuttingDataDTO dto)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = _configuration["CalculateCuttingData"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");
                ApiResponse<ResultCuttingDataTestsDTO> result = new();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spltests).Result;
                using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
                {
                    if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                        return await ParseUnprocessableEntity422HttpResponse<ResultCuttingDataTestsDTO>.UnprocessableEntityApiResponse(response);
                    _ = response.EnsureSuccessStatusCode();
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    result = await JsonSerializer.DeserializeAsync<ApiResponse<ResultCuttingDataTestsDTO>>(stream, _options);
                }
                return result;
            }
            catch (Exception) { return null; }
        }

        public async Task<ApiResponse<long>> SaveCuttingData(HeaderCuttingDataDTO dto)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = _configuration["SaveCuttingData"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");
                ApiResponse<long> result = new();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
                using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
                {
                    if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                        return await ParseUnprocessableEntity422HttpResponse<long>.UnprocessableEntityApiResponse(response);
                    _ = response.EnsureSuccessStatusCode();
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    result = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
                }
                return result;
            }
            catch (Exception) { return null; }
        }
    }
}
