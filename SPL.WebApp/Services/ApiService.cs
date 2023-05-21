using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SPL.WebApp.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IConfiguration _configuration;

        public ApiService(IHttpClientFactory clientFactory,
            ITokenAcquisition tokenAcquisition,
            IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _tokenAcquisition = tokenAcquisition;
            _configuration = configuration;
        }

        public async Task<(bool Success, string ErrorMessage, JArray Data)> GetApiDataAsync()
        {
            var success = false;
            string errorMessage = null;
            JArray data = null;

            try
            {
                var client = _clientFactory.CreateClient("HttpClientWithSSLUntrusted");

                var scope = _configuration["CallApi:ScopeForAccessToken"];
                var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { scope });

                client.BaseAddress = new Uri(_configuration["CallApi:ApiBaseAddress"]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("WeatherForecast");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    data = JArray.Parse(responseContent);
                    success = true;
                }
                else
                {
                    errorMessage = $"Status code: {response.StatusCode}, Error: {response.ReasonPhrase}";
                }
            }
            catch (MicrosoftIdentityWebChallengeUserException e)
            {
                errorMessage = $"{e.GetType()}: {e.Message}{Environment.NewLine}Stacktrace:{Environment.NewLine}{e.StackTrace}{Environment.NewLine}{Environment.NewLine}Logout then login again to fix the issue.";
            }
            catch (Exception e)
            {
                errorMessage = $"{e.GetType()}: {e.Message}{Environment.NewLine}Stacktrace:{e.StackTrace}";
            }

            return (success, errorMessage, data);
        }
    }
}
