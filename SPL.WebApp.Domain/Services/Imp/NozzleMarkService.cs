namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.SecurityApis;

    public class NozzleMarkService : INozzleMarkService
    {
        private  HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IValidateAccesApis _AAD;
        public NozzleMarkService(HttpClient httpClient, IConfiguration config, IValidateAccesApis aad)
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

        public async Task<ApiResponse<List<NozzleMarksDTO>>> GetNozzleBrands(long idMark)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetNozzleBrands"];
            string url = uriBuilder.Uri + "/" + idMark;

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<List<NozzleMarksDTO>>>(stream, this._options);
        }

        public async Task<ApiResponse<long>> SaveNozzleBrands(NozzleMarksDTO viewModel)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            if (viewModel.IdMarca is 0)
            {
                viewModel.Fechacreacion = DateTime.Now;
                //viewModel.Creadopor = "none";
            }
            else
            {
                viewModel.Fechamodificacion = DateTime.Now;
                //viewModel.Modificadopor = "none";
            }

            uriBuilder.Path = this._configuration["SaveNozzleBrands"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(viewModel, this._options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> resultSaveFactor = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                resultSaveFactor = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, this._options);
            }
            return resultSaveFactor;
        }

        public async Task<ApiResponse<long>> DeleteNozzleBrands(NozzleMarksDTO viewModel)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            viewModel.Fechacreacion = DateTime.Now;
            uriBuilder.Path = this._configuration["DeleteNozzleBrands"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(viewModel, this._options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> result = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, this._options);
            }
            return result;
        }
    }
}
