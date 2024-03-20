using dotnet_banco_digital.Domain.Models;
using System.ComponentModel.DataAnnotations;

public class Usuario : Entity
{
    [Required]
    public string Nome { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; }

    [Required]
    [StringLength(11)]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve ter 11 dígitos.")]
    public string Cpf { get; set; }

    [Required]
    public string Senha { get; set; }
}