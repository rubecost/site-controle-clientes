using Microsoft.AspNetCore.Mvc;
using ControleClientesMvc.Models;
using ControleClientesMvc.ApiClients;
using ControleClientesMvc.Services;

public class CobrancasController : Controller
{
    private readonly CadastrarCobrancaApiClient _cadastrarCobrancasApiClient;
    private readonly ObterRelatorioDividasApiClient _obterRelatorioDividasApiClient;
    private readonly ExcluirCobrancaApiClient _excluirCobrancaApiClient;
    private readonly ObterDetalhesCobrancaApiClient _obterDetalhesCobrancaApiClient;
    private readonly AtualizarCobrancaApiClient _atualizarCobrancaApiClient;

    public CobrancasController(AutenticacaoService _autenticacaoService)
    {
        _cadastrarCobrancasApiClient = new CadastrarCobrancaApiClient(_autenticacaoService);
        _obterRelatorioDividasApiClient = new ObterRelatorioDividasApiClient(_autenticacaoService);
        _excluirCobrancaApiClient = new ExcluirCobrancaApiClient(_autenticacaoService);
        _obterDetalhesCobrancaApiClient = new ObterDetalhesCobrancaApiClient(_autenticacaoService);
        _atualizarCobrancaApiClient = new AtualizarCobrancaApiClient(_autenticacaoService);
    }

    [HttpGet]
    public async Task<IActionResult> Index(int id, string nome)
    {
        if (id <= 0)
        {
            return RedirectToAction("Index", "Clientes");
        }

        ViewData["NomeCliente"] = nome;

        TempData["ClienteId"] = id;

        var cobrancas = await _obterRelatorioDividasApiClient.ObterRelatorioDividasAsync(id);

        if (cobrancas == null || !cobrancas.Any())
        {
            TempData["AvisoMessage"] = "Nenhuma cobrança encontrada para o cliente.";
        }

        return View("Index", cobrancas);
    }
    [HttpGet]
    public IActionResult Criar()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CobrancaModel cobranca)
    {

        if (cobranca.Descricao == null || cobranca.Valor == 0 || cobranca.Data == DateTime.MinValue || cobranca._Status == 0)
        {
            return View(cobranca);
        }

        int tempID = Convert.ToInt32(TempData["ClienteId"]);
        cobranca.Id = tempID;

        switch (cobranca._Status)
        {
            case 1:
                cobranca.Status = true;
                break;
            case 2:
                cobranca.Status = false;
                break;
            default:
                break;
        }

        var mensagem = await _cadastrarCobrancasApiClient.CadastrarCobrancasAsync(cobranca);

        if (mensagem.Contains("sucesso"))
        {
            ViewData["SucessoMessage"] = mensagem;
            return View("Criar", cobranca);
        }
        else
        {
            ViewData["ErroMessage"] = mensagem;
            return View("Criar", cobranca);
        }
    }
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new { Message = "ID da cobrança inválido." });
        }

        TempData["CobrancaId"] = id;

        var cobranca = await _obterDetalhesCobrancaApiClient.ObterDetalhesCobrancaAsync(id);

        if (cobranca != null)
        {
            var model = new CobrancaModel
            {
                Descricao = cobranca.Descricao,
                Valor = cobranca.Valor,
                _Data = cobranca.Data.ToString("yyyy-MM-dd"),
                _Status = cobranca.Status ? 1 : 2
            };

            return View("Editar",model);
        }
        else
        {
            return NotFound(new { Message = "Cobrança não encontrada." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Editar(CobrancaModel cobranca)
    {
        int tempID = Convert.ToInt32(TempData["CobrancaId"]);
        cobranca.Id = tempID;

        switch (cobranca._Status)
        {
            case 1:
                cobranca.Status = true;
                break;
            case 2:
                cobranca.Status = false;
                break;
            default:
                break;
        }

        var mensagem = await _atualizarCobrancaApiClient.AtualizarCobrancaAsync(cobranca);

        if (mensagem.Contains("sucesso"))
        {
            ViewData["SucessoMessage"] = mensagem;
            return View("Editar", cobranca);
        }
        else
        {
            ViewData["ErroMessage"] = mensagem;
            return View("Editar", cobranca);
        }

    }
    [HttpPost]
    public async Task<IActionResult> Excluir(int id)
    {
        if (id > 0)
        {
            var mensagem = await _excluirCobrancaApiClient.ExcluirCobrancaAsync(id);

            if (mensagem.Contains("sucesso"))
            {
                TempData["SucessoMessage"] = mensagem;
                return Json(new { success = true });
            }
            else
            {
                TempData["ErroMessage"] = mensagem;
                return Json(new { success = false });
            }
        }
        else
        {
            TempData["ErroMessage"] = "O ID do cliente deve ser maior que zero.";
            return Json(new { success = false });
        }
    }

}
