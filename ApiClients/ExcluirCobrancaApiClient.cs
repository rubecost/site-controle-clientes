using ControleClientesMvc.Services;
using Newtonsoft.Json;

namespace ControleClientesMvc.ApiClients
{
    public class ExcluirCobrancaApiClient
    {
        private readonly AutenticacaoService _autenticacaoService;
        public ExcluirCobrancaApiClient(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        public async Task<string> ExcluirCobrancaAsync(int id_Cobranca)
        {
            try
            {
                var _client = await _autenticacaoService.VerificarAutenticacaoAsync();

                var response = await _client.DeleteAsync($"{ApiSettingsService.BaseUrl}cliente/exluir_debito?id_cobranca={id_Cobranca}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse>(content);

                    return result?.Message ?? "Cobrança excluído com sucesso.";
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
