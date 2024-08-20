using ControleClientesMvc.Services;
using Newtonsoft.Json;
using ControleClientesMvc.Models;

namespace ControleClientesMvc.ApiClients
{
	public class ObterRelatorioClientesApiClient
	{
        private readonly AutenticacaoService _autenticacaoService;
        public ObterRelatorioClientesApiClient(AutenticacaoService autenticacaoService)
		{
            _autenticacaoService = autenticacaoService;
		}

		public async Task<List<CobrancaModel>> ObterRelatorioClientesAsync(int id_Usuario)
		{
			try
			{
                // Verifica a autenticação e obtém o HttpClient autenticado
                var _client = await _autenticacaoService.VerificarAutenticacaoAsync();

                HttpResponseMessage response = await _client.GetAsync($"{ApiSettingsService.BaseUrl}cliente/relatorio_clientes?id_Usuario={id_Usuario}");

				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					var listaUsuarioInformacoes = JsonConvert.DeserializeObject<List<CobrancaModel>>(content);

					if(listaUsuarioInformacoes != null)
					{
						return listaUsuarioInformacoes;
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
