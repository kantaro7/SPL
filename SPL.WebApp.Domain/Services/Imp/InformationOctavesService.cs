namespace SPL.WebApp.Domain.Services.Imp
{
    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.Helpers;
    using SPL.WebApp.Domain.SecurityApis;

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web;

    public class InformationOctavesService : IInformationOctavesService
    {
        private HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IValidateAccesApis _AAD;
        public InformationOctavesService(HttpClient httpClient, IConfiguration config, IValidateAccesApis aad)
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


        public async Task<ApiResponse<List<InformationOctavesDTO>>> GetInfoOctavas(string NroSerie, string TypeInformation, string DateData)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["GetInformationOctaves"];
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);

                query["NroSerie"] = NroSerie;
                query["TypeInformation"] = TypeInformation;
                query["DateData"] = DateData;
                uriBuilder.Query = query.ToString();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                ApiResponse<List<InformationOctavesDTO>> factorTypesMark = await JsonSerializer.DeserializeAsync<ApiResponse<List<InformationOctavesDTO>>>(stream, this._options);
                return factorTypesMark;
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<InformationOctavesDTO>>
                {
                    Code = -1,
                    Description = ex.Message,
                    Structure = null
                };
            }
        }

        public async Task<ApiResponse<long>> SaveOctaves(List<InformationOctavesDTO> dto, bool isImport)
        {
            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = isImport ? _configuration["ImportInformationOctaves"] : _configuration["UpdateInformationOctaves"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");

                ApiResponse<long> resultRdtTest = new();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
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
    }
}
