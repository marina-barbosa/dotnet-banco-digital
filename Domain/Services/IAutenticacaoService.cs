
using System.Threading.Tasks;

public interface IAutenticacaoService
{
    Task<Usuario> AutenticarUsuario(string cpf, string senha);
}
