namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.SecurityApis;

    public class SidcoClientService : ISidcoClientService
    {
        private  HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IValidateAccesApis _AAD;
        public SidcoClientService(HttpClient httpClient, IConfiguration config, IValidateAccesApis aad)
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

        public async Task<IEnumerable<CatSidcoDTO>> GetCatSIDCO()
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration[MethodMasterKeyName.GetCatSIDCO.ToString()];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splmasters).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<CatSidcoDTO>>(stream, this._options); ;
        }
    }
}
