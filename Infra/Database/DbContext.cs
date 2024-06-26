using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasIndex(usuario => usuario.Email)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasIndex(usuario => usuario.Cpf)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .Property(usuario => usuario.Saldo)
            .HasDefaultValue(0);

    }
}