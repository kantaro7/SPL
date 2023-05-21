namespace Gateway.Api.Handlers
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class RemoveEncodingDelegatingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.AcceptEncoding.Clear();
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
