namespace DhrSound.Api.Dtos;

public record AutorizarTransacaoDto(int ContaId, string Comerciante, decimal Valor, DateTime Horario);

public record ResultadoAutorizacaoDto(bool Aprovada, string Motivo, int? TransacaoId);
