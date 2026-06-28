namespace DhrSound.Api.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;

    public Conta? Conta { get; set; }
    public ICollection<Assinatura> Assinaturas { get; set; } = new List<Assinatura>();
    public ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
    public ICollection<Notificacao> Notificacoes { get; set; } = new List<Notificacao>();
}
