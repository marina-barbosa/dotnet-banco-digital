using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("v1/usuario/login")]
public class LoginController : ControllerBase
{
    // Supondo que você tenha um serviço de autenticação injetado
    private readonly IAutenticacaoService _autenticacaoService;

    public LoginController(IAutenticacaoService autenticacaoService)
    {
        _autenticacaoService = autenticacaoService;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        var usuario = await _autenticacaoService.AutenticarUsuario(loginModel.Cpf, loginModel.Senha);

        if (usuario == null)
        {
            return Unauthorized("CPF e/ou senha incorretos.");
        }

        // lembrar de gerar um token JWT pra retornar na resposta

        return Ok(new { message = "Login bem-sucedido." });
    }
}
