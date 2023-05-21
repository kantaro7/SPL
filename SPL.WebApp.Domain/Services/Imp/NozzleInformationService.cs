namespace SPL.WebApp.Domain.Services.Imp
{
    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.SecurityApis;

    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class NozzleInformationService : INozzleInformationService
    {
        private  HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IValidateAccesApis _AAD;
        public NozzleInformationService(HttpClient httpClient, IConfiguration config, IValidateAccesApis aad)
        {
            _configuration = config;
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
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

        public async Task<ApiResponse<NozzlesByDesignDTO>> GetRecordNozzleInformation(string numeroSerie)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetNozzleInformation"];


            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri + "/" + numeroSerie, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<NozzlesByDesignDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<long>> SaveRecordNozzleInformation(NozzlesByDesignDTO viewModel)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
         

            uriBuilder.Path = _configuration["SaveRecordNozzleInformation"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(viewModel, _options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> result = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
            using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            }
            return result;
        }
    }
}
