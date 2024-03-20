using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


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

        var emailExistente = await _context.Usuarios.AnyAsync(usuario => usuario.Email == novoUsuario.Email);
        if (emailExistente)
        {
            return Conflict("Email já cadastrado.");
        }

        var cpfExistente = await _context.Usuarios.AnyAsync(usuario => usuario.Cpf == novoUsuario.Cpf);
        if (cpfExistente)
        {
            return Conflict("CPF já cadastrado.");
        }

        _context.Usuarios.Add(novoUsuario);
        await _context.SaveChangesAsync();

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
