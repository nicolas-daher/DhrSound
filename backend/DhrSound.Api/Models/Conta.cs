namespace DhrSound.Api.Models;

public class Conta
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public bool Ativa { get; set; } = true;
    public decimal LimiteDisponivel { get; set; }

    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}
