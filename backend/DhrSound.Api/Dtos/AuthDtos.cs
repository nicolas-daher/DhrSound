namespace DhrSound.Api.Dtos;

public record CriarUsuarioDto(string Nome, string Email, string Senha);

public record LoginDto(string Email, string Senha);

public record LoginRespostaDto(int UsuarioId, string Nome, string Token);
