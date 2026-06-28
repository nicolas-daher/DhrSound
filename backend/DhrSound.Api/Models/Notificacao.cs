namespace DhrSound.Api.Models;

public class Notificacao
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public int? TransacaoId { get; set; }
    public Transacao? Transacao { get; set; }

    public string Destinatario { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public DateTime Data { get; set; }
}
