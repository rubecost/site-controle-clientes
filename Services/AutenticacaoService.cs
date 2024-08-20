using ControleClientesMvc.ApiClients;
using System.Net.Http.Headers;

namespace ControleClientesMvc.Services
{
    public class AutenticacaoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _client;
        private readonly VerificarValidadeJwt _verificarValidadeJwt;

        public AutenticacaoService(IHttpContextAccessor httpContextAccessor, HttpClient client, VerificarValidadeJwt verificarValidadeJwt)
        {
            _httpContextAccessor = httpContextAccessor;
            _client = client;
            _verificarValidadeJwt = verificarValidadeJwt;
        }

        public async Task<HttpClient> VerificarAutenticacaoAsync()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwt"];

            if (!string.IsNullOrEmpty(token))
            {
                var resultado = await _verificarValidadeJwt.VerificarAsync(token);

                if (resultado != "Token válido.")
                {
                     _httpContextAccessor.HttpContext?.Response.Redirect("/Usuarios/Login");

                    return _client;
                }
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await Task.CompletedTask;

            return _client;
        }
    }
}
