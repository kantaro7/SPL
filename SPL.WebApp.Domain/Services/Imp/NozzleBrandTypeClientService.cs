namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.SecurityApis;

    public class NozzleBrandTypeClientService : INozzleBrandTypeClientService
    {
        private  HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IValidateAccesApis _AAD;
        public NozzleBrandTypeClientService(
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

        public async Task<ApiResponse<List<NozzleMarksDTO>>> GetNozzleTypesByBrand(long pIdMark)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetNozzleTypesByBrand"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(string.Concat(uriBuilder.Uri, "/", pIdMark), HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<List<NozzleMarksDTO>>>(stream, this._options);
        }

        public async Task<ApiResponse<List<NozzleMarksDTO>>> GetBrandType(TypeNozzleMarksDTO request)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetNozzleMarks"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri + "/" + request.IdMarca + "/" + request.Estatus, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<List<NozzleMarksDTO>>>(stream, this._options);
        }

        public async Task<ApiResponse<long>> SaveRegisterNozzleTypeMark(NozzleMarksDTO dto)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["SaveTypeNozzleMarks"];
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(dto, this._options),
                  Encoding.UTF8,
                  "application/json");

            ApiResponse<long> resultRadTest = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                resultRadTest = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, this._options);
            }
            return resultRadTest;
        }

        public async Task<ApiResponse<long>> DeleteTypeNozzleMarks(NozzleMarksDTO dto)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["DeleteTypeNozzleMarks"];
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(dto, this._options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> resultRadTest = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                resultRadTest = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, this._options);
            }
            return resultRadTest;
        }
    }
}
