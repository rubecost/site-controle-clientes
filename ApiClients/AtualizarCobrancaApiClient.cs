using ControleClientesMvc.Services;
using Newtonsoft.Json;
using ControleClientesMvc.Models;

namespace ControleClientesMvc.ApiClients
{
    public class AtualizarCobrancaApiClient
    {
        private readonly AutenticacaoService _autenticacaoService;
        public AtualizarCobrancaApiClient(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        public async Task<string> AtualizarCobrancaAsync(CobrancaModel cobranca)
        {
            try
            {
                var _client = await _autenticacaoService.VerificarAutenticacaoAsync();

                var response = await _client.PostAsJsonAsync($"{ApiSettingsService.BaseUrl}cliente/atualizar_debito", cobranca);

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
