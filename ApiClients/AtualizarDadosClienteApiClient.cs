using ControleClientesMvc.Services;
using Newtonsoft.Json;
using ControleClientesMvc.Models;

namespace ControleClientesMvc.ApiClients
{
    public class AtualizarDadosClienteApiClient
    {
        private readonly AutenticacaoService _autenticacaoService;
        public AtualizarDadosClienteApiClient(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        public async Task<string> AtualizarDadosClienteAsync(ClienteModel cliente)
        {
            try
            {
                var _client = await _autenticacaoService.VerificarAutenticacaoAsync();

                var response = await _client.PostAsJsonAsync($"{ApiSettingsService.BaseUrl}cliente/atualizar_dados", cliente);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse>(content);

                    return result?.Message ?? "Dados atualizados com sucesso.";
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
