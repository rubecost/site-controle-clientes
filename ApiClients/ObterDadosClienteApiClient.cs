using ControleClientesMvc.Services;
using Newtonsoft.Json;
using ControleClientesMvc.Models;

namespace ControleClientesMvc.ApiClients
{
    public class ObterDadosClienteApiClient
    {
        private readonly AutenticacaoService _autenticacaoService;

        public ObterDadosClienteApiClient(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        public async Task<ClienteModel?> ObterDadosClienteAsync(int idCliente)
        {
            try
            {
                var client = await _autenticacaoService.VerificarAutenticacaoAsync();

                HttpResponseMessage response = await client.GetAsync($"{ApiSettingsService.BaseUrl}cliente/obter_dados_cliente?id_cliente={idCliente}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var cliente = JsonConvert.DeserializeObject<ClienteModel>(content);

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
