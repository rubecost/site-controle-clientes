using ControleClientesMvc.Services;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Linq;

namespace ControleClientesMvc.ApiClients
{
    public class UsuarioLoginApiClient
    {
        private readonly HttpClient _client;
        public UsuarioLoginApiClient()
        {
            _client = new HttpClient();
        }

        public async Task<ApiResponse> UsuarioLoginAsync(string email, string senha)
        {
            try
            {
                var loginRequest = new { Email = email, Senha = senha };

                var response = await _client.PostAsJsonAsync($"{ApiSettingsService.BaseUrl}usuario/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse>();
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    return new ApiResponse { Message = errorResponse?.Message ?? "Erro ao processar a resposta." };
                }
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse { Message = $"Erro ao tentar fazer login: {ex.Message}" };
            }
        }
    }
    public class ApiResponse
    {
        public int UserId { get; set; }
        public string? Message { get; set; }
        public TokenResponse? Token { get; set; }
    }
    public class TokenResponse
    {
        public string? Token { get; set; }
    }
}
