using Microsoft.AspNetCore.Mvc;
using ControleClientesMvc.Models;
using ControleClientesMvc.ApiClients;

public class UsuariosController : Controller
{
	private readonly UsuarioLoginApiClient _usuarioLoginApiClient;
	private readonly UsuarioCadastrarApiClient _usuarioCadastrarApiClient;
	private readonly VerificarValidadeJwt _verificarValidadeJwt;

	public UsuariosController(VerificarValidadeJwt verificarValidadeJwt)
	{
		_usuarioLoginApiClient = new UsuarioLoginApiClient();
		_usuarioCadastrarApiClient = new UsuarioCadastrarApiClient();
		_verificarValidadeJwt = verificarValidadeJwt;
	}

	[HttpGet]
	public async Task<IActionResult> Login()
	{
		var token = HttpContext.Request.Cookies["jwt"];

		if (!string.IsNullOrEmpty(token))
		{
			var resultado = await _verificarValidadeJwt.VerificarAsync(token);

			if (resultado == "Token válido.")
			{
				return RedirectToAction("Index", "Clientes");
			}
		}
        return View(new LoginModel());
	}

	[HttpPost]
	public async Task<IActionResult> Login(LoginModel model)
	{
		if (!ModelState.IsValid)
		{
			return View(model);
		}

		if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Senha))
		{
			var resultado = await _usuarioLoginApiClient.UsuarioLoginAsync(model.Email, model.Senha);

			if (resultado.Message == "Login bem-sucedido." && resultado.UserId > 0)
			{
				var cookieOptions = new CookieOptions
				{
					HttpOnly = true,
					Secure = true,  // Temporariamente desativado para depuração local
					SameSite = SameSiteMode.Strict,
					Path = "/"
				};

				Response.Cookies.Append("jwt", resultado.Token.Token, cookieOptions);
				Response.Cookies.Append("UserId", resultado.UserId.ToString(), cookieOptions);

				return RedirectToAction("Index", "Clientes");
			}
			else
			{
				TempData["ErroLogin"] = resultado.Message;

				LoginModel novoLogin = new();

				return View(novoLogin);
			}
		}

		return View(model);
	}
    [HttpPost]
    public IActionResult Logout()
    {
        // Remove o JWT do cookie
        Response.Cookies.Delete("jwt");

        return RedirectToAction("Login", "Usuarios");
    }
    [HttpGet]
	public IActionResult Cadastrar()
	{
		return View(new UsuarioModel());
	}

	[HttpPost]
	public async Task<IActionResult> Cadastrar(UsuarioModel usuario)
	{
		if (!ModelState.IsValid)
		{
			return View(usuario);
		}

		if (!string.IsNullOrEmpty(usuario.Nome) && !string.IsNullOrEmpty(usuario.Email) && !string.IsNullOrEmpty(usuario.Senha))
		{
			var mensagem = await _usuarioCadastrarApiClient.UsuarioCadastrarAsync(usuario.Nome, usuario.Email, usuario.Senha);

			if (mensagem.Contains("sucesso"))
			{
				TempData["SucessoCadastro"] = mensagem;

				UsuarioModel novoUsuario = new();

				return View(novoUsuario);
			}
			else
			{
				TempData["ErroCadastro"] = mensagem;
				return View(usuario);
			}
		}
		return View(usuario);
	}
}
