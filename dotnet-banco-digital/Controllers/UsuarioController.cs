using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;


namespace dotnet_banco_digital.Controllers;
[ApiController]
[Route("v1/usuario")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
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

}
