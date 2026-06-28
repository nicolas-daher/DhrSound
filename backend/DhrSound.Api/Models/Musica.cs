namespace DhrSound.Api.Models;

public class Musica
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int DuracaoSegundos { get; set; }

    public int BandaId { get; set; }
    public Banda? Banda { get; set; }
}
