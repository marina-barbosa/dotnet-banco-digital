

using Microsoft.EntityFrameworkCore;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly AppDbContext _context;

    public AutenticacaoService(AppDbContext context)
    {
        _context = context;
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
}
