

using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AutenticacaoService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Usuario> AutenticarUsuario(string cpf, string senha)
    {
        var usuarioLogado = await _context.Usuarios.SingleOrDefaultAsync(usuario => usuario.Cpf == cpf);

        if (usuarioLogado == null)
        {
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(senha, usuarioLogado.Senha))
        {
            return null;
        }

        return usuarioLogado;
    }

    public async Task<Usuario> ObterUsuarioAutenticado()
    {

        var usuarioId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(usuarioId))
        {
            return null;
        }

        var usuario = await _context.Usuarios.FindAsync(usuarioId);
        return usuario;
    }
}
