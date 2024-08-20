using ControleClientesMvc.Models;
using ControleClientesMvc.Services;
using Newtonsoft.Json;

namespace ControleClientesMvc.ApiClients
{
    public class ObterRelatorioDividasApiClient
    {
        private readonly AutenticacaoService _autenticacaoService;
        public ObterRelatorioDividasApiClient(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }
        public async Task<List<CobrancaModel>> ObterRelatorioDividasAsync(int id_Cliente)
        {
            try
            {
                var _client = await _autenticacaoService.VerificarAutenticacaoAsync();

                HttpResponseMessage response = await _client.GetAsync($"{ApiSettingsService.BaseUrl}cliente/obter_debitos?id_cliente={id_Cliente}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var listaCobrancas = JsonConvert.DeserializeObject<List<CobrancaModel>>(content);

                    if (listaCobrancas != null)
                    {
                        return listaCobrancas;
                    }

                    return new List<CobrancaModel>();
                }
                else
                {
                    return new List<CobrancaModel>();
                }
            }
            catch (Exception)
            {
                return new List<CobrancaModel>();
            }
        }
    }
}
