using ControleClientesMvc.Services;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ControleClientesMvc.ApiClients
{
    public class VerificarValidadeJwt
    {
        private readonly HttpClient _client;

        public VerificarValidadeJwt(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> VerificarAsync(string token)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync($"{ApiSettingsService.BaseUrl}usuario/verificar_token");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse>(content);

                    if (result != null)
                    {
                        return result.Message ?? "";
                    }

                    return "Resposta da API sem mensagem.";
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse>(content);
                    return result?.Message ?? "Erro ao processar a resposta da API.";
                }
            }
            catch (Exception ex)
            {
                return $"Erro: {ex.Message}";
            }
        }
    }
}
