namespace DhrSound.Api.Models;

public class Banda
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;

    public ICollection<Musica> Musicas { get; set; } = new List<Musica>();
}
