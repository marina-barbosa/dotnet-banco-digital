using dotnet_banco_digital.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Transacao : Entity
{

    [Required]
    public int UsuarioOrigemId { get; set; }

    public int UsuarioDestinoId { get; set; }

    public DateTime DataTransacao { get; set; }

    public decimal Valor { get; set; }

    public TipoTransacao TipoTransacao { get; set; }

    [Column(TypeName = "json")]
    public string DescricaoJson { get; set; }

    [ForeignKey("UsuarioOrigemId")]
    public virtual Usuario UsuarioOrigem { get; set; }

    [ForeignKey("UsuarioDestinoId")]
    public virtual Usuario UsuarioDestino { get; set; }
}

public enum TipoTransacao
{
    Debito,
    Credito,
    TED,
    Pix,
    Transferencia,
    PagamentoBoleto,
    Deposito,
    Saque,
    CartaoDebito,
    CartaoCredito
}