using Microsoft.OpenApi.Writers;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net.Http.Headers;
using System.Web;

namespace ClamManagement.Data.Services
{
    public interface IUserManagement
    {
        Task<int> GetUserId();
    }
    public  class UserManagement : IUserManagement
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserManagement(IHttpContextAccessor contextAccessor)
        {
            _httpContextAccessor = contextAccessor;
        }
        public async  Task<int> GetUserId()
        {
            var context = _httpContextAccessor.HttpContext;

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:7100/User/CurrentUserId");
            var uriBuilder = new UriBuilder(httpClient.BaseAddress);
            string token = context.Request.Headers["Authorization"];

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (token));
            var res = await (await httpClient.GetAsync(uriBuilder.ToString())).Content.ReadAsStringAsync();
            return int.Parse(res);
        }
    }
}
