namespace DhrSound.Api.Models;

public class Transacao
{
    public int Id { get; set; }

    public int ContaId { get; set; }
    public Conta? Conta { get; set; }

    public string Comerciante { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime Horario { get; set; }

    public bool Aprovada { get; set; }
    public string Motivo { get; set; } = string.Empty;
}
