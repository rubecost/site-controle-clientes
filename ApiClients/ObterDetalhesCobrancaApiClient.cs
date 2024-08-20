using ControleClientesMvc.Services;
using Newtonsoft.Json;
using ControleClientesMvc.Models;

namespace ControleClientesMvc.ApiClients
{
    public class ObterDetalhesCobrancaApiClient
    {
        private readonly AutenticacaoService _autenticacaoService;

        public ObterDetalhesCobrancaApiClient(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        public async Task<CobrancaModel?> ObterDetalhesCobrancaAsync(int id_Cobranca)
        {
            try
            {
                var client = await _autenticacaoService.VerificarAutenticacaoAsync();

                HttpResponseMessage response = await client.GetAsync($"{ApiSettingsService.BaseUrl}cliente/obter_detalhes_cobranca?id_cobranca={id_Cobranca}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var cliente = JsonConvert.DeserializeObject<CobrancaModel>(content);

                    return cliente;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
