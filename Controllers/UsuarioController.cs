using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;


namespace dotnet_banco_digital.Controllers;
[ApiController]
[Route("v1/usuario")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IAutenticacaoService _autenticacaoService;


    public UsuariosController(AppDbContext context, IAutenticacaoService autenticacaoService)
    {
        _context = context;
        _autenticacaoService = autenticacaoService;
    }

    [HttpPost("cadastro")]
    public async Task<ActionResult<Usuario>> CadastraUsuario(Usuario novoUsuario)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (novoUsuario.Id != 0)
        {
            return BadRequest("O ID deve ser 0 para novos usuários.");
        }

        // var emailExistente = await _context.Usuarios.AnyAsync(usuario => usuario.Email == novoUsuario.Email);
        // if (emailExistente)
        // {
        //     return Conflict("Email já cadastrado.");
        // }

        // var cpfExistente = await _context.Usuarios.AnyAsync(usuario => usuario.Cpf == novoUsuario.Cpf);
        // if (cpfExistente)
        // {
        //     return Conflict("CPF já cadastrado.");
        // }

        novoUsuario.Senha = BCrypt.Net.BCrypt.HashPassword(novoUsuario.Senha);

        try
        {
            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when ((ex.InnerException as PostgresException)?.SqlState == "23505")
        {
            var errorMessage = ex.InnerException.Message;
            if (errorMessage.Contains("IX_Usuarios_Email"))
            {
                return Conflict("Email já cadastrado.");
            }

            if (errorMessage.Contains("IX_Usuarios_Cpf"))
            {
                return Conflict("CPF já cadastrado.");
            }
        }
        catch (DbUpdateException ex)
        {

            return StatusCode(500, "Erro interno do servidor.");
        }

        return CreatedAtAction("GetUsuario", new { id = novoUsuario.Id }, novoUsuario);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        return usuario;
    }


    [HttpPut("atualizar/{id}")]
    public async Task<IActionResult> AtualizarUsuario(int id, [FromBody] Usuario usuarioAtualizado)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        usuario.Nome = usuarioAtualizado.Nome;
        usuario.Email = usuarioAtualizado.Email;
        usuario.Cpf = usuarioAtualizado.Cpf;
        usuario.Senha = usuarioAtualizado.Senha;
        usuario.Saldo = usuarioAtualizado.Saldo;

        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Usuário atualizado com sucesso." });
    }


    [HttpGet("saldo/{id}")]
    public async Task<IActionResult> ConsultarSaldo(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        return Ok(new { saldo = usuario.Saldo });
    }


    [HttpPatch("deposito/{id}")]
    public async Task<IActionResult> RealizarDeposito(int id, [FromBody] decimal valorDeposito)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        usuario.Saldo += valorDeposito;

        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Depósito realizado com sucesso.", novoSaldo = usuario.Saldo });
    }


    [HttpPatch("saque/{id}")]
    public async Task<IActionResult> RealizarSaque(int id, [FromBody] decimal valorSaque)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        if (usuario.Saldo < valorSaque)
        {
            return BadRequest("Saldo insuficiente para realizar o saque.");
        }

        usuario.Saldo -= valorSaque;

        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Saque realizado com sucesso.", novoSaldo = usuario.Saldo });
    }

}
