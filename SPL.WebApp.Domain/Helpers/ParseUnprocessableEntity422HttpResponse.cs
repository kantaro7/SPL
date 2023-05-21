using SPL.Domain;
using System.Net.Http;
using System.Threading.Tasks;

namespace SPL.WebApp.Domain.Helpers
{
    /// <summary>
    /// Utilitario para parsear una API response con codigo 422
    /// </summary>
    public static class ParseUnprocessableEntity422HttpResponse<T>
    {
        public static async Task<ApiResponse<T>> UnprocessableEntityApiResponse(HttpResponseMessage httpResponse)
        {
            var errors = await httpResponse.Content.ReadAsStringAsync();

            return new ApiResponse<T>
            {
                Code = (int)ResponsesID.fallido,
                Description = errors
            };
        }
    }
}