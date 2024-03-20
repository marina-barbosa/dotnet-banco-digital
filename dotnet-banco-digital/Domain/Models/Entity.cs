using System.ComponentModel.DataAnnotations;

namespace dotnet_banco_digital.Domain.Models;

public interface IEntity
{
    public int Id { get; set; }
}

public abstract class Entity : IEntity
{
    [Key]
    [Required]
    public int Id { get; set; }
}