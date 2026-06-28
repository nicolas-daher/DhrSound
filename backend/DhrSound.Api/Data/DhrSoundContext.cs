using DhrSound.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DhrSound.Api.Data;

public class DhrSoundContext : DbContext
{
    public DhrSoundContext(DbContextOptions<DhrSoundContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Conta> Contas => Set<Conta>();
    public DbSet<Plano> Planos => Set<Plano>();
    public DbSet<Assinatura> Assinaturas => Set<Assinatura>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();
    public DbSet<Notificacao> Notificacoes => Set<Notificacao>();
    public DbSet<Banda> Bandas => Set<Banda>();
    public DbSet<Musica> Musicas => Set<Musica>();
    public DbSet<Favorito> Favoritos => Set<Favorito>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Conta)
            .WithOne(c => c.Usuario!)
            .HasForeignKey<Conta>(c => c.UsuarioId);

        modelBuilder.Entity<Banda>().HasIndex(b => b.Nome);
        modelBuilder.Entity<Musica>().HasIndex(m => m.Titulo);

        modelBuilder.Entity<Conta>().Property(c => c.LimiteDisponivel).HasPrecision(18, 2);
        modelBuilder.Entity<Plano>().Property(p => p.Preco).HasPrecision(18, 2);
        modelBuilder.Entity<Transacao>().Property(t => t.Valor).HasPrecision(18, 2);

        modelBuilder.Entity<Transacao>().HasIndex(t => new { t.ContaId, t.Horario });
    }
}
