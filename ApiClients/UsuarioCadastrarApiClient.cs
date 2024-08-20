using ControleClientesMvc.Services;
using Newtonsoft.Json;

namespace ControleClientesMvc.ApiClients
{
    public class UsuarioCadastrarApiClient
    {
        private readonly HttpClient _client;

        public UsuarioCadastrarApiClient()
        {
            _client = new HttpClient();
        }

        public async Task<string> UsuarioCadastrarAsync(string nome, string email, string senha)
        {
            try
            {
                var cadastroRequest = new { Nome = nome, Email = email, Senha = senha };

                var response = await _client.PostAsJsonAsync($"{ApiSettingsService.BaseUrl}usuario/cadastrar", cadastroRequest);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse>(content);

                    if (result != null)
                    {
                        return result.Message ?? "Cadastro realizado com sucesso.";
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
                return $"Erro desconhecido: {ex.Message}. Tente novamente!";
            }
        }
    }
}
