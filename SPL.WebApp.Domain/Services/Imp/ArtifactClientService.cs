namespace SPL.WebApp.Domain.Services.Imp
{
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

    using AutoMapper;

    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.SecurityApis;

    public class ArtifactClientService : IArtifactClientService
    {
        //private readonly ITokenAcquisition _tokenAcquisition;
        private HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IValidateAccesApis _AAD;
        public ArtifactClientService(HttpClient httpClient, IConfiguration configuration, IMapper mapper/*, ITokenAcquisition tokenAcquisition*/, IHttpClientFactory clientFactory, IValidateAccesApis aad)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _mapper = mapper;
            //this._tokenAcquisition = tokenAcquisition;
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

        #region Record Artifact Methods
        public async Task<InformationArtifactDTO> GetArtifact(string nroSerie)
        {
            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["GeneralArtifact"];

                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["nroSerie"] = nroSerie;
                uriBuilder.Query = query.ToString();
                //_httpClient.DefaultRequestHeaders.Authorization =
                //    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<InformationArtifactDTO>(stream, _options);
            }
            catch (Exception)
            {
                throw;
            }
            //var token = await _tokenAcquisition.GetAccessTokenForUserAsync(
            //new string[] { "User.ReadBasic.All", "user.read", "user.read", "api://14f15439-78be-4e6e-a897-c07e9b6a7a76/acces_as_user" }).ConfigureAwait(false);
        }


        public async Task<decimal> CheckBoqTerciary(string nroSerie)
        {
            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["CheckBoqTerciary"] + string.Format("/{0}", nroSerie);

                //_httpClient.DefaultRequestHeaders.Authorization =
                //   new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);

                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<decimal>(stream, _options);
            }
            catch (Exception)
            {
                throw;
            }
            //var token = await _tokenAcquisition.GetAccessTokenForUserAsync(
            //new string[] { "User.ReadBasic.All", "user.read", "user.read", "api://14f15439-78be-4e6e-a897-c07e9b6a7a76/acces_as_user" }).ConfigureAwait(false);
        }

        public async Task<InformationArtifactDTO> GetArtifactSIDCO(string nroSerie)
        {
            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["Sidco"];
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["nroSerie"] = nroSerie;
                uriBuilder.Query = query.ToString();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splsidco).Result;
                using HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<InformationArtifactDTO>(stream, _options);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HttpStatusCode> AddArtifactToSPL(InformationArtifactDTO dto)
        {
            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["GeneralArtifact"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson);
                _ = response.EnsureSuccessStatusCode();
                return response.StatusCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HttpStatusCode> UpdateArtifactToSPL(InformationArtifactDTO dto, ArtifactUpdate tabUpdate, string NameUserM, string NameUserC, DateTime FechaCreacion)
        {
            DateTime fecha = DateTime.Now;
            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                string dtoSerialize;
                switch (tabUpdate)
                {
                    case ArtifactUpdate.GENERAL:
                        {
                            dto.GeneralArtifact.CreadoPor = NameUserC;
                            dto.GeneralArtifact.FechaCreacion = FechaCreacion;
                            dto.GeneralArtifact.ModificadoPor = NameUserM;
                            dto.GeneralArtifact.FechaModificacion = fecha;
                            dtoSerialize = JsonSerializer.Serialize(dto.GeneralArtifact, _options);
                            uriBuilder.Path = _configuration["UpdateGeneralArtifact"];
                            break;
                        }
                    case ArtifactUpdate.CHARACTERISTIC:
                        {
                            foreach (CharacteristicsArtifactDTO item in dto.CharacteristicsArtifact)
                            {
                                item.CreadoPor = NameUserC;
                                item.FechaCreacion = FechaCreacion;
                                item.ModificadoPor = NameUserM;
                                item.FechaModificacion = fecha;
                            }
                            dtoSerialize = JsonSerializer.Serialize(_mapper.Map<AllCharacteristicsArtifactDTO>(dto), _options);
                            uriBuilder.Path = _configuration["UpdateCharacteristicsArtifact"];
                            break;
                        }
                    case ArtifactUpdate.WARANTIES:
                        {
                            dto.WarrantiesArtifact.CreadoPor = NameUserC;
                            dto.WarrantiesArtifact.FechaCreacion = FechaCreacion;
                            dto.WarrantiesArtifact.ModificadoPor = NameUserM;
                            dto.WarrantiesArtifact.FechaModificacion = fecha;
                            dtoSerialize = JsonSerializer.Serialize(dto.WarrantiesArtifact, _options);
                            uriBuilder.Path = _configuration["UpdateWarrantiesArtifact"];
                            break;
                        }
                    case ArtifactUpdate.NOZZLESS:
                        {
                            foreach (NozzlesArtifactDTO item in dto.NozzlesArtifact)
                            {
                                item.CreadoPor = NameUserC;
                                item.FechaCreacion = FechaCreacion;
                                item.ModificadoPor = NameUserM;
                                item.FechaModificacion = fecha;
                            }

                            dtoSerialize = JsonSerializer.Serialize(dto.NozzlesArtifact, _options);
                            uriBuilder.Path = _configuration["UpdateNozzlesArtifact"];
                            break;
                        }
                    case ArtifactUpdate.LIGHTNING_ROD:
                        {
                            foreach (LightningRodArtifactDTO item in dto.LightningRodArtifact)
                            {
                                item.CreadoPor = NameUserC;
                                item.FechaCreacion = FechaCreacion;
                                item.ModificadoPor = NameUserM;
                                item.FechaModificacion = fecha;
                            }
                            dtoSerialize = JsonSerializer.Serialize(dto.LightningRodArtifact, _options);
                            uriBuilder.Path = _configuration["UpdateLightningRodArtifact"];
                            break;
                        }
                    case ArtifactUpdate.CHANGENS:
                        {
                            foreach (ChangingTablesArtifactDTO item in dto.ChangingTablesArtifact)
                            {
                                item.Creadopor = NameUserC;
                                item.FechaCreacion = FechaCreacion;
                                item.ModificadoPor = NameUserM;
                                item.FechaModificacion = fecha;
                            }

                            dtoSerialize = JsonSerializer.Serialize(_mapper.Map<AllChangingTablesArtifactDTO>(dto), _options);
                            uriBuilder.Path = _configuration["UpdateChangingTablesArtifact"];
                            break;
                        }
                    case ArtifactUpdate.TEST_LABS:
                        {
                            dto.LabTestsArtifact.CreadoPor = NameUserC;
                            dto.LabTestsArtifact.FechaCreacion = FechaCreacion;
                            dto.LabTestsArtifact.ModificadoPor = NameUserM;
                            dto.LabTestsArtifact.FechaModificacion = fecha;
                            dtoSerialize = JsonSerializer.Serialize(dto.LabTestsArtifact, _options);
                            uriBuilder.Path = _configuration["UpdateLabTestsArtifact"];
                            break;
                        }
                    case ArtifactUpdate.NORMS:
                        {
                            foreach (RulesArtifactDTO item in dto.RulesArtifact)
                            {
                                item.CreadoPor = NameUserC;
                                item.FechaCreacion = FechaCreacion;
                                item.ModificadoPor = NameUserM;
                                item.FechaModificacion = fecha;
                            }

                            dtoSerialize = JsonSerializer.Serialize(dto.RulesArtifact, _options);
                            uriBuilder.Path = _configuration["UpdateRulesArtifact"];
                            break;
                        }
                    default:
                        {
                            dto.GeneralArtifact.CreadoPor = NameUserC;
                            dto.GeneralArtifact.FechaCreacion = FechaCreacion;
                            dto.GeneralArtifact.ModificadoPor = NameUserM;
                            dto.GeneralArtifact.FechaModificacion = fecha;
                            dtoSerialize = JsonSerializer.Serialize(dto, _options);
                            uriBuilder.Path = _configuration["GeneralArtifact"];
                            break;
                        }
                }

                StringContent dtoJson = new(
                      dtoSerialize,
                      Encoding.UTF8,
                      "application/json");

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.PutAsync(uriBuilder.Uri, dtoJson);
                _ = response.EnsureSuccessStatusCode();
                return response.StatusCode;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckNumberOrder(string noSerie)
        {
            try
            {

                //       var token = await _tokenAcsquisition.GetAccessTokenForUserAsync(
                //new string[] { "User.ReadBasic.All", "user.read","api://14f15439-78be-4e6e-a897-c07e9b6a7a76/acces_as_user"
                //}).ConfigureAwait(false);

                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["CheckNumberOrder"] + string.Format("/{0}", noSerie);

                //_httpClient.DefaultRequestHeaders.Authorization =
                //   new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);

                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<bool>(stream, _options);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Tension Plate Methods

        public async Task<ApiResponse<CharacteristicsPlaneTensionDTO>> GetCharacteristics(string nroSerie)
        {
            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["GetCharacteristicsPlateTension"];
                uriBuilder.Query = "";

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.GetAsync(string.Concat(uriBuilder.Uri, "/", nroSerie), HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<CharacteristicsPlaneTensionDTO>>(stream, _options);
            }
            catch (Exception e)
            {
                return new ApiResponse<CharacteristicsPlaneTensionDTO>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = null
                };
            }
        }

        public async Task<ApiResponse<TapBaanDTO>> GetTapBaanPlateTension(string nroSerie)
        {

            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["GetTapBaanPlateTension"];

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.GetAsync(string.Concat(uriBuilder.Uri, "/", nroSerie), HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<TapBaanDTO>>(stream, _options);
            }
            catch (Exception e)
            {
                return new ApiResponse<TapBaanDTO>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = null
                };
            }
        }

        public async Task<ApiResponse<List<PlateTensionDTO>>> GetPlateTension(string unit, string typeVoltage)
        {

            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["GetPlateTension"];
                uriBuilder.Query = string.Empty;

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.GetAsync(string.Concat(uriBuilder.Uri, "/", unit, "/", typeVoltage), HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<List<PlateTensionDTO>>>(stream, _options);
            }
            catch (Exception e)
            {
                return new ApiResponse<List<PlateTensionDTO>>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = null
                };
            }
        }

        public async Task<ApiResponse<List<PlateTensionDTO>>> GetTensionOriginalPlate(string unit, string typeVoltage)
        {
            try
            {

                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["GetTensionOriginalPlate"];
                uriBuilder.Query = string.Empty;

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.GetAsync(string.Concat(uriBuilder.Uri, "/", unit, "/", typeVoltage), HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<List<PlateTensionDTO>>>(stream, _options);
            }
            catch (Exception e)
            {
                return new ApiResponse<List<PlateTensionDTO>>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = null
                };
            }
        }

        public async Task<ApiResponse<long>> AddTension(List<PlateTensionDTO> dto, bool isNewTension)
        {
            try
            {

                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["PlateTension"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.PostAsync(string.Concat(uriBuilder.Uri, "/", Convert.ToDecimal(isNewTension).ToString()), dtoJson);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            }
            catch (Exception e)
            {
                return new ApiResponse<long>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = 0
                };
            }
        }

        public async Task<ApiResponse<long>> AddResistanceDesign(List<ResistDesignDTO> dto)
        {
            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["SaveResistDesign"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");
                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            }
            catch (Exception e)
            {

                return new ApiResponse<long>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = 0
                };
            }
        }

        #endregion

        #region Base Template

        public async Task<ApiResponse<long>> AddBaseTemplate(BaseTemplateDTO dto)
        {
            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["BaseTemplate"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            }
            catch (Exception e)
            {

                return new ApiResponse<long>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = 0
                };
            }
        }

        public async Task<ApiResponse<long>> AddBaseTemplateConsolidatedReport(BaseTemplateConsolidatedReportDTO dto)
        {
            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["SaveBaseTemplateConsolidatedReport"];
                StringContent dtoJson = new(
                      JsonSerializer.Serialize(dto, _options),
                      Encoding.UTF8,
                      "application/json");

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, dtoJson);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, _options);
            }
            catch (Exception e)
            {
                return new ApiResponse<long>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = 0
                };
            }
        }

        public async Task<ApiResponse<BaseTemplateConsolidatedReportDTO>> GetBaseTemplateConsolidatedReport(string idioma)
        {
            try
            {
                UriBuilder uriBuilder = GetUriBuilder();
                uriBuilder.Path = _configuration["GetBaseTemplateConsolidatedReport"] + "/" + idioma;
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                uriBuilder.Query = query.ToString();

                _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.spldesigninformation).Result;
                using HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<ApiResponse<BaseTemplateConsolidatedReportDTO>>(stream, _options);
            }
            catch (Exception e)
            {
                return new ApiResponse<BaseTemplateConsolidatedReportDTO>
                {
                    Code = -1,
                    Description = e.Message,
                    Structure = null
                };
            }
        }

        #endregion
    }
}
