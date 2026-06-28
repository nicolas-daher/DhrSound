namespace DhrSound.Api.Models;

public class Favorito
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public int? MusicaId { get; set; }
    public Musica? Musica { get; set; }

    public int? BandaId { get; set; }
    public Banda? Banda { get; set; }
}
