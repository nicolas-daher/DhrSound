namespace DhrSound.Api.Dtos;

public record PlanoDto(int Id, string Nome, string Descricao, decimal Preco);

public record CriarAssinaturaDto(int PlanoId);

public record AssinaturaDto(int Id, int PlanoId, string PlanoNome, DateTime DataInicio, DateTime? DataFim, bool Ativa);

public record NotificacaoDto(int Id, string Destinatario, string Conteudo, DateTime Data);

public record MusicaDto(int Id, string Titulo, int DuracaoSegundos, int BandaId, string BandaNome);

public record BandaDto(int Id, string Nome, string Descricao);

public record ResultadoBuscaDto(IEnumerable<BandaDto> Bandas, IEnumerable<MusicaDto> Musicas);

public record FavoritosDto(IEnumerable<MusicaDto> Musicas, IEnumerable<BandaDto> Bandas);
