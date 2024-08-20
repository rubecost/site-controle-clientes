using ControleClientesMvc.Services;
using Newtonsoft.Json;

namespace ControleClientesMvc.ApiClients
{
    public class ExcluirClienteApiClient
    {
        private readonly AutenticacaoService _autenticacaoService;
        public ExcluirClienteApiClient(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        public async Task<string> ExcluirClienteAsync(int id_Cliente)
        {
            try
            {
                var _client = await _autenticacaoService.VerificarAutenticacaoAsync();

                // Fazendo a requisição DELETE com o id_Cliente como query string
                var response = await _client.DeleteAsync($"{ApiSettingsService.BaseUrl}cliente/excluir?id_Cliente={id_Cliente}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse>(content);

                    return result?.Message ?? "Cliente excluído com sucesso.";
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