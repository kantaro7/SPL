namespace SPL.WebApp.Domain.Services.Imp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web;

    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.SecurityApis;

    public class MasterHttpClientService : IMasterHttpClientService
    {
        private  HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IArtifactClientService _artifactClientService;
        private readonly ISidcoClientService _sidcoClientService;
        private readonly IValidateAccesApis _AAD;
        public MasterHttpClientService(
            HttpClient httpClient, 
            IConfiguration config,
            IArtifactClientService artifactClientService,
            ISidcoClientService sidcoClientService, IValidateAccesApis aad)
        {
            this._configuration = config;
            this._httpClient = httpClient;
            this._options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            this._artifactClientService = artifactClientService;
            this._sidcoClientService = sidcoClientService;
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

        public async Task<IEnumerable<GeneralPropertiesDTO>> GetMasterByMethodKey(MethodMasterKeyName masterKeyName, int Microservices)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration[masterKeyName.ToString()];

                this._httpClient.DefaultRequestHeaders.Clear();
                this._httpClient = _AAD.getTokenSesionAsync(_httpClient, Microservices).Result;


                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                IEnumerable<GeneralPropertiesDTO> generalProperties;
                switch (masterKeyName)
                {
                    case MethodMasterKeyName.GetThirdWinding:
                        ApiResponse<IEnumerable<GeneralPropertiesDTO>> result = await JsonSerializer.DeserializeAsync<ApiResponse<IEnumerable<GeneralPropertiesDTO>>>(stream, this._options);
                        generalProperties = result.Structure;
                        break;
                    default:
                        generalProperties = await JsonSerializer.DeserializeAsync<IEnumerable<GeneralPropertiesDTO>>(stream, this._options);
                        break;
                }
                return generalProperties;
            }
            catch(Exception ex) 
            {
                throw ex; 
            }
        }

        public async Task<IEnumerable<RulesArtifactDTO>> GetRuleFromSidco(string rule, string language)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["GetRulesRep"];
                System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["pRule"] = rule;
                query["pLenguage"] = language;
                uriBuilder.Query = query.ToString();

                this._httpClient.DefaultRequestHeaders.Clear();
                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splmasters).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<IEnumerable<RulesArtifactDTO>>(stream, this._options);
            }
            catch(Exception e)
            {
                throw e;
            }
      
        }

        public async Task<IEnumerable<CatSidcoOthersDTO>> GetCatSidcoOthers()
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration[MethodMasterKeyName.GetCatSidcoOthers.ToString()];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splmasters).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<CatSidcoOthersDTO>>(stream, this._options);
        }

        public async Task<ApiResponse<IEnumerable<FileWeightDTO>>> GetConfigurationFiles(int moduleId)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetConfigFile"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splmasters).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(string.Concat(uriBuilder.Uri, "/", moduleId.ToString()), HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<IEnumerable<FileWeightDTO>>>(stream, this._options);
        }

        public async Task<IEnumerable<GeneralPropertiesDTO>> GetConnectionTest(string noSerie)
        {
            InformationArtifactDTO artifactDesing = await this._artifactClientService.GetArtifact(noSerie);
            if (artifactDesing.GeneralArtifact is null)
            {
                throw new Exception("Artefacto no posee información general.");
            }
            if (string.IsNullOrEmpty(artifactDesing.GeneralArtifact.OrderCode))
            {
                throw new Exception("Artefacto no encontrado.");
            }
            if (!artifactDesing.CharacteristicsArtifact.Any())
            {
                throw new Exception("Artefacto no posee características.");
            }
            if( artifactDesing.GeneralArtifact.TypeTrafoId is null)
            {
                throw new Exception("Artefacto no posee información de Tipo de transformación (TypeTrafoId).");
            }
            IEnumerable<CatSidcoDTO> sidcos = await this._sidcoClientService.GetCatSIDCO();
            if (!sidcos.Any())
            {
                throw new Exception("La Base de Datos no posee información de CatSIDCO.");
            }
            CatSidcoDTO catSidco = sidcos.Where(s => s.AttributeId == 3 && s.Id == Convert.ToInt32(artifactDesing.GeneralArtifact.TypeTrafoId)).FirstOrDefault();
            return catSidco is null
                ? throw new Exception("Artefacto no posee información de CatSIDCO.")
                : (IEnumerable<GeneralPropertiesDTO>)(catSidco.ClaveSPL.Equals("TRA")
                ? new List<GeneralPropertiesDTO>()
                {
                    new GeneralPropertiesDTO() { Clave = "L-L", Descripcion = "L-L" },
                    new GeneralPropertiesDTO() { Clave = "L-N", Descripcion = "L-N" }
                }
                : new List<GeneralPropertiesDTO>()
                {
                    new GeneralPropertiesDTO() { Clave = "H-X", Descripcion = "H-X" },
                    new GeneralPropertiesDTO() { Clave = "H-N", Descripcion = "H-N" }
                });
        }

        public async Task<ApiResponse<IEnumerable<ContGasCGDDTO>>>  GetContGasCGD(string idReport, string keyTests, string typeOil)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetContGasCGD"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(string.Concat(uriBuilder.Uri, "/", idReport, "/", keyTests, "/", typeOil), HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<IEnumerable<ContGasCGDDTO>>>(stream, this._options);
        }

        public async Task<ApiResponse<long>> SaveContGasCGD(ContGasCGDDTO dto)
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = _configuration["SaveInfoContGasCGD"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");

                ApiResponse<long> resultCgd = new();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
                using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
                {
                    _ = response.EnsureSuccessStatusCode();
                    Stream stream = await response.Content.ReadAsStreamAsync();

                    resultCgd = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);

                 }
                return resultCgd;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }



        public async Task<ApiResponse<long>> DeleteContGasCGD(ContGasCGDDTO dto)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = _configuration["DeleteInfoContGasCGD"];
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(dto, _options),
                  Encoding.UTF8,
                  "application/json");

            ApiResponse<long> resultCgd = new();


            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splconfiguration).Result;
            using (HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();

                resultCgd = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);

            }
            return resultCgd;
        }



        public async Task<List<GeneralPropertiesDTO>> GetRuleEquivalents()
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["GetRulesEquivalents"];

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splmasters).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<GeneralPropertiesDTO>>(stream, this._options);
            }
            catch (Exception ex) { return null; }
        }

        public async Task<List<GeneralPropertiesDTO>> GetUnitTypes()
        {
            try
            {
                UriBuilder uriBuilder = this.GetUriBuilder();
                uriBuilder.Path = this._configuration["GetUnitTypes"];

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splmasters).Result;
                using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<GeneralPropertiesDTO>>(stream, this._options);
            }
            catch (Exception ex) { return null; }
        }
    }
}
