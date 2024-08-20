using ControleClientesMvc.Services;
using Newtonsoft.Json;
using ControleClientesMvc.Models;

namespace ControleClientesMvc.ApiClients
{
    public class CadastrarCobrancaApiClient
    {
        private readonly AutenticacaoService _autenticacaoService;
        public CadastrarCobrancaApiClient(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        public async Task<string> CadastrarCobrancasAsync(CobrancaModel cobranca)
        {
            try
            {
                var _client = await _autenticacaoService.VerificarAutenticacaoAsync();

                var response = await _client.PostAsJsonAsync($"{ApiSettingsService.BaseUrl}cliente/cadastrar_debito", cobranca);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<dynamic>(result);

                    return responseObject?.Message ?? "Cobrança cadastrada com sucesso!";
                }
                else
                {
                    return $"Erro: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                return $"Erro: {ex.Message}";
            }
        }
    }

}
