namespace SPL.WebApp.Domain.Services.ProfileSecurity
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web;

    using Microsoft.Extensions.Configuration;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Enums;
    using SPL.WebApp.Domain.SecurityApis;

    public class ProfileSecurityService : IProfileSecurityService
    {
        private  HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IValidateAccesApis _AAD;
        public ProfileSecurityService(HttpClient httpClient, IConfiguration config, IValidateAccesApis aad)
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


    
  


        public async Task<ApiResponse<List<AssignmentUsersDTO>>> GetAssignmentProfiles(string Profile)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetAssignmentProfiles"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splsecurity).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri + "/" + Profile, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<List<AssignmentUsersDTO>>>(stream, this._options);
        }

        public async Task<ApiResponse<long>> DeleteAssignmentProfilesUsers(AssignmentUsersDTO request)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
           
            uriBuilder.Path = this._configuration["DeleteAssignmentProfilesUsers"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(request, this._options),
                  Encoding.UTF8,
                  "application/json");

            ApiResponse<long> resultSaveFactor = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splsecurity).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                resultSaveFactor = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, this._options);
            }
            return resultSaveFactor;
        }


        public async Task<ApiResponse<long>> DeleteProfiles(UserProfilesDTO request)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();

            uriBuilder.Path = this._configuration["DeleteProfiles"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(request, this._options),
                  Encoding.UTF8,
                  "application/json");


            ApiResponse<long> resultSaveFactor = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splsecurity).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                resultSaveFactor = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, this._options);
            }
            return resultSaveFactor;
        }


        public async Task<ApiResponse<List<UserOptionsDTO>>> GetOptionsMenu()
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetOptionsMenu"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splsecurity).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<List<UserOptionsDTO>>>(stream, this._options);
        }

        public async Task<ApiResponse<List<UserPermissionsDTO>>> GetPermissionsProfile(string Profile)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetPermissionsProfile"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splsecurity).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri + "/" + Profile, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<List<UserPermissionsDTO>>>(stream, this._options);
        }


        public async Task<ApiResponse<List<UserProfilesDTO>>> GetProfiles(string Profile)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
             uriBuilder.Path = this._configuration["GetProfiles"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splsecurity).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri + "/" + Profile, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<List<UserProfilesDTO>>>(stream, this._options);
        }


        public async Task<ApiResponse<List<UsersDTO>>> GetUsers(string Name)
        {
         

            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetUsers"];
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["Name"] = Name;
     
            uriBuilder.Query = query.ToString();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splsecurity).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            ApiResponse<List<UsersDTO>> result = await JsonSerializer.DeserializeAsync<ApiResponse<List<UsersDTO>>>(stream, this._options);
            return result;
        }

        public async Task<ApiResponse<long>> SaveAssignmentProfilesUsers(AssignmentUsersDTO request)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
          

            uriBuilder.Path = this._configuration["SaveAssignmentProfilesUsers"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(request, this._options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> result= new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splsecurity).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, this._options);
            }
            return result;
        }



        public async Task<ApiResponse<long>> SaveProfiles(UserProfilesDTO request)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();


            uriBuilder.Path = this._configuration["SaveProfiles"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(request, this._options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> result = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splsecurity).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, this._options);
            }
            return result;
        }
        public async Task<ApiResponse<long>> SavePermissions(List<UserPermissionsDTO> request)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();


            uriBuilder.Path = this._configuration["SavePermissions"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(request, this._options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> result = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splsecurity).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, this._options);
            }
            return result;
        }


        public async Task<ApiResponse<long>> SaveUsers(List<UsersDTO> request)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();


            uriBuilder.Path = this._configuration["SaveUsers"];
            uriBuilder.Query = string.Empty;
            StringContent dtoJson = new(
                  JsonSerializer.Serialize(request, this._options),
                  Encoding.UTF8,
                  "application/json");
            ApiResponse<long> result = new();

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splsecurity).Result;
            using (HttpResponseMessage response = await this._httpClient.PostAsync(uriBuilder.Uri, dtoJson))
            {
                _ = response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<ApiResponse<long>>(stream, this._options);
            }
            return result;
        }



        public async Task<ApiResponse<List<UserPermissionsDTO>>> GetPermissionUsers(string IdUser)
        {
            UriBuilder uriBuilder = this.GetUriBuilder();
            uriBuilder.Path = this._configuration["GetPermissionUsers"];

            _httpClient = _AAD.getTokenSesionAsync(_httpClient, (int)MicroservicesEnum.splsecurity).Result;
            using HttpResponseMessage response = await this._httpClient.GetAsync(uriBuilder.Uri + "/" + IdUser, HttpCompletionOption.ResponseHeadersRead);
            _ = response.EnsureSuccessStatusCode();
            Stream stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ApiResponse<List<UserPermissionsDTO>>>(stream, this._options);
        }
    }
}
