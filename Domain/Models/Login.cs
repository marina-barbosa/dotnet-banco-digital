using System.ComponentModel.DataAnnotations.Schema;

[NotMapped]
public class LoginModel
{
    public string Cpf { get; set; }
    public string Senha { get; set; }
}
