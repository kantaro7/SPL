namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web;

    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.SecurityApis;

    public class ResistanceTwentyDegreesClientServices : IResistanceTwentyDegreesClientServices
    {
        private  HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IValidateAccesApis _AAD;
        public ResistanceTwentyDegreesClientServices(HttpClient httpClient, IConfiguration config, IValidateAccesApis aad)
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

        public async Task<ApiResponse<List<ResistDesignDTO>>> GetResistDesignDTO(string noSerie, string unitMeasurement, string TestConnection, decimal temperature, string idSection = "-1", decimal order = -1)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetResistDesign"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["NroSerie"] = noSerie;
            query["UnitOfMeasurement"] = unitMeasurement.ToUpper();
            query["TestConnection"] = TestConnection;
            query["Temperature"] = temperature.ToString("G", CultureInfo.InvariantCulture);
            query["IdSection"] = idSection;
            query["Order"] = order.ToString();
            uriBuilder.Query = query.ToString();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<List<ResistDesignDTO>>>(stream, this._options);
        }

        public async Task<ApiResponse<List<ResistDesignDTO>>> GetResistDesignCustom(string noSerie, string unitMeasurement, string TestConnection, decimal temperature, string idSection, decimal order = -1)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["GetResistDesignCustom"];
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["NroSerie"] = noSerie;
                query["UnitOfMeasurement"] = unitMeasurement.ToUpper();
                query["TestConnection"] = TestConnection;
                query["Temperature"] = temperature.ToString("G", CultureInfo.InvariantCulture);
                query["IdSection"] = idSection;
                query["Order"] = order.ToString();
                uriBuilder.Query = query.ToString();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<List<ResistDesignDTO>>>(stream, this._options);
            }
            catch(Exception e)
            {
                return new ApiResponse<List<ResistDesignDTO>>
                {

                    Structure = null, 
                    Code = -1,
                    Description = e.Message
                };
            }

        }
    }
}
