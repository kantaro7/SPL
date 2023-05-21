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
    using Microsoft.Identity.Web;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.SecurityApis;

    public class CorrectionFactorService : ICorrectionFactorService
    {
        private HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IValidateAccesApis _AAD;

        public CorrectionFactorService(HttpClient httpClient, IConfiguration config, IValidateAccesApis aad)
        {
            _configuration = config;
            _httpClient = httpClient;
            _AAD = aad;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _AAD = aad;

        }

        private UriBuilder GetUriBuilder()
        {
            UriBuilder uriBuilder = new()
            {
                Host = _configuration["GateWayAPIDomain"],
                Scheme = _configuration["SchemeGateWayAPI"]
            };
            string port = _configuration["GateWayAPIPort"];
            if (!string.IsNullOrEmpty(port))
                uriBuilder.Port = int.Parse(port);
            return uriBuilder;
        }

        public async Task<ApiResponse<List<CorrectionFactorDTO>>> GetAllDataFactor(string Specification, decimal Temperature, decimal CorrectionFactor)
        {
            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["GetCorrectioFactor"];

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;

                using HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri + "/" + Specification + "/" + Temperature + "/" + CorrectionFactor, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<ApiResponse<List<CorrectionFactorDTO>>>(stream, _options);
            }
            catch (MicrosoftIdentityWebChallengeUserException e)
            {
                throw new ArgumentNullException(paramName: "Error", message: $"{e.GetType()}: {e.Message}{Environment.NewLine}Stacktrace:{Environment.NewLine}{e.StackTrace}{Environment.NewLine}{Environment.NewLine}Cierre sesión y vuelva a iniciar sesión para solucionar el problema");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ApiResponse<long>> SaveCorrectionFactorSpecification(CorrectionFactorDTO request)
        {
            UriBuilder uriBuilder = GetUriBuilder();
            uriBuilder.Path = _configuration["SaveCorrectionFactor"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(request, _options),
                  Encoding.UTF8,
                  "application/json");

            ApiResponse<long> resultSaveFactor = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;

            using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                resultSaveFactor = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            }
            return resultSaveFactor;
        }

        public async Task<ApiResponse<long>> SaveCorrectionFactorsXMarksXTypes(CorrectionFactorsXMarksXTypesDTO request)
        {
            UriBuilder uriBuilder = GetUriBuilder();

            uriBuilder.Path = _configuration["SaveCorrectionFactorsXMarksXTypes"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(request, _options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> resultSaveFactor = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                resultSaveFactor = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            }
            return resultSaveFactor;
        }

        public async Task<ApiResponse<long>> DeleteCorrectionFactorSpecification(CorrectionFactorDTO request)
        {
            UriBuilder uriBuilder = GetUriBuilder();
            uriBuilder.Path = _configuration["DeleteCorrectionFactor"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(request, _options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> resultSaveFactor = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                resultSaveFactor = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            }
            return resultSaveFactor;
        }

        public async Task<ApiResponse<List<CorrectionFactorsXMarksXTypesDTO>>> GetCorrectionFactorsXMarksXTypes()
        {
            UriBuilder uriBuilder = GetUriBuilder();
            uriBuilder.Path = _configuration["GetCorrectionFactorsXMarksXTypes"];
            string url = uriBuilder.Uri.ToString();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using HttpResponseMessage response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<List<CorrectionFactorsXMarksXTypesDTO>>>(stream, _options);
        }

        public async Task<ApiResponse<List<NozzleMarksDTO>>> GetNozzleMarks(NozzleMarksDTO request)
        {
            UriBuilder uriBuilder = GetUriBuilder();
            uriBuilder.Path = _configuration["GetNozzleMarks"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri + "/" + request.IdMarca + "/" + request.Estatus, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<List<NozzleMarksDTO>>>(stream, _options);
        }

        public async Task<ApiResponse<List<TypeNozzleMarksDTO>>> GetTypeXMarksNozzle(TypeNozzleMarksDTO request)
        {
            UriBuilder uriBuilder = GetUriBuilder();
            uriBuilder.Path = _configuration["GetTypeXMarksNozzle"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri + "/" + request.IdMarca + "/" + request.Estatus, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<List<TypeNozzleMarksDTO>>>(stream, _options);
        }

        public async Task<ApiResponse<long>> DeleteRegisterNozzleTypeMark(CorrectionFactorsXMarksXTypesDTO request)
        {
            UriBuilder uriBuilder = GetUriBuilder();
            request.Fechacreacion = DateTime.Now;
            uriBuilder.Path = _configuration["DeleteCorrectionFactorsXMarksXTypes"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(request, _options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> resultSaveFactor = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                resultSaveFactor = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            }
            return resultSaveFactor;
        }

        public async Task<ApiResponse<List<ValidationTestsIszDTO>>> GetValidationTestsISZ()
        {
            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["GetValidationTestsISZ"];
                uriBuilder.Query = string.Empty;
                ApiResponse<List<ValidationTestsIszDTO>> resultSaveFactor = new();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
                using (HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri))
                {
                    _ = response.EnsureSuccessStatusCode();

                    Stream stream = await response.Content.ReadAsStreamAsync();
                    resultSaveFactor = await JsonSerializer.DeserializeAsync<ApiResponse<List<ValidationTestsIszDTO>>>(stream, _options);
                }
                return resultSaveFactor;
            }
            catch (Exception e)
            {
                return new ApiResponse<List<ValidationTestsIszDTO>>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = null
                };
            }
        }
    }
}
