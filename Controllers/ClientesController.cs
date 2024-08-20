using Microsoft.AspNetCore.Mvc;
using ControleClientesMvc.Models;
using System.Collections.Generic;
using ControleClientesMvc.ApiClients;
using ControleClientesMvc.Services;

public class ClientesController : Controller
{
    private readonly ObterRelatorioClientesApiClient _obterRelatorioClientesApi;
    private readonly ExcluirClienteApiClient _excluirClienteApiClient;
    private readonly ClienteCadastrarApiClient _clienteCadastrarApiClient;
    private readonly ObterDadosClienteApiClient _obterDadosClienteApiClient;
    private readonly AtualizarDadosClienteApiClient _atualizarDadosClienteApiClient;

    public ClientesController(AutenticacaoService _autenticacaoService)
    {
        _obterRelatorioClientesApi = new ObterRelatorioClientesApiClient(_autenticacaoService);
        _excluirClienteApiClient = new ExcluirClienteApiClient(_autenticacaoService);
        _clienteCadastrarApiClient = new ClienteCadastrarApiClient(_autenticacaoService);
        _obterDadosClienteApiClient = new ObterDadosClienteApiClient(_autenticacaoService);
        _atualizarDadosClienteApiClient = new AtualizarDadosClienteApiClient(_autenticacaoService);
    }
    private async Task<int> ObterIdUsuario()
    {
        var idString = HttpContext?.Request.Cookies["UserId"];

        if (!string.IsNullOrEmpty(idString))
        {
            if (int.TryParse(idString, out int userId))
            {
                return userId;
            }
        }

        await Task.CompletedTask;

        return 0;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (await ObterIdUsuario() != 0)
        {
            var relatorioClientes = await _obterRelatorioClientesApi.ObterRelatorioClientesAsync(await ObterIdUsuario());

            if (relatorioClientes == null || !relatorioClientes.Any())
            {
                TempData["AvisoBoasVindas"] = "Olá, você ainda não possui nenhum cliente cadastrado. Que tal começar agora?";
            }
            return View("Index", relatorioClientes);
        }

        return RedirectToAction("Login", "Usuarios");
    }

    [HttpGet]
    public IActionResult Criar()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Criar(ClienteModel cliente)
    {

        if (!ModelState.IsValid)
        {
            return View("Criar", cliente);
        }

        cliente.Id = await ObterIdUsuario();

        string mensagem = await _clienteCadastrarApiClient.CadastrarClienteAsync(cliente);

        if (mensagem.Contains("sucesso"))
        {
            TempData["SucessoCriarCliente"] = mensagem;

            var novoCliente = new ClienteModel();
            return View("Criar", novoCliente);
        }
        else
        {
            TempData["ErroCriarCliente"] = mensagem;
            return View("Criar", cliente);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new { Message = "ID do cliente inválido." });
        }

        var cliente = await _obterDadosClienteApiClient.ObterDadosClienteAsync(id);

        if (cliente != null)
        {
            return View("Editar", cliente);
        }
        else
        {
            return RedirectToAction("Index", "Clientes");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Editar(ClienteModel cliente)
    {
        if (!ModelState.IsValid)
        {
            return View("Editar", cliente);
        }

        var mensagem = await _atualizarDadosClienteApiClient.AtualizarDadosClienteAsync(cliente);

        if (mensagem.Contains("sucesso"))
        {
            TempData["SucessoEditarCliente"] = mensagem;

            return View("Editar", cliente);
        }
        else
        {
            TempData["ErroEditarCliente"] = mensagem;

            return View("Editar", cliente);
        }
    }
    [HttpPost]
    public async Task<IActionResult> Excluir(int id)
    {
        if (id > 0)
        {
            var status = await _excluirClienteApiClient.ExcluirClienteAsync(id);

            if (status == "Cliente excluído com sucesso!")
            {
                TempData["SuccessExcluirCliente"] = "Cliente excluído com sucesso!";
            }
            else
            {
                TempData["ErrorExcluirCliente"] = status;
            }
        }
        else
        {
            TempData["ErrorExcluirCliente"] = "O ID do cliente deve ser maior que zero.";
        }

        return RedirectToAction("Index");
    }
}