using ControleClientesMvc.Services;
using Newtonsoft.Json;
using ControleClientesMvc.Models;

namespace ControleClientesMvc.ApiClients
{
    public class ClienteCadastrarApiClient
    {
        private readonly AutenticacaoService _autenticacaoService;
        public ClienteCadastrarApiClient(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }
        public async Task<string> CadastrarClienteAsync(ClienteModel cliente)
        {
            try
            {
                var _client = await _autenticacaoService.VerificarAutenticacaoAsync();

                var response = await _client.PostAsJsonAsync($"{ApiSettingsService.BaseUrl}cliente/cadastrar", cliente);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse>(content);

                    return result?.Message ?? "Cliente cadastrado com sucesso.";
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
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
