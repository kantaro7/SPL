namespace SPL.WebApp.Domain.SecurityApis
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using Telerik.Web.Spreadsheet;

    public interface IValidateAccesApis
    {
        public Task<HttpClient> getTokenSesionAsync(HttpClient httpClient, int microservices);
        public Task<string> getTokenSesionGatewayAsync(int microservices);

    }
}
