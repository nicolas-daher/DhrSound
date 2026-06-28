using DhrSound.Api.Models;

namespace DhrSound.Api.Services;

public static class RegrasAutorizacao
{
    public const int LimiteTransacoesPorJanela = 3;
    public static readonly TimeSpan JanelaFrequencia = TimeSpan.FromMinutes(2);
    public static readonly TimeSpan JanelaDuplicidade = TimeSpan.FromMinutes(2);

    public static string? CartaoInativo(Conta conta)
        => conta.Ativa ? null : "Cartao inativo";

    public static string? LimiteInsuficiente(Conta conta, decimal valor)
        => valor <= conta.LimiteDisponivel ? null : "Limite insuficiente";

    public static string? AltaFrequencia(IEnumerable<Transacao> historico, DateTime horario)
    {
        var inicioJanela = horario - JanelaFrequencia;
        var recentes = historico.Count(t => t.Horario > inicioJanela && t.Horario <= horario);
        return recentes >= LimiteTransacoesPorJanela
            ? "Alta frequencia de transacoes em intervalo curto"
            : null;
    }

    public static string? TransacaoDuplicada(
        IEnumerable<Transacao> historico, string comerciante, decimal valor, DateTime horario)
    {
        var inicioJanela = horario - JanelaDuplicidade;
        var duplicada = historico.Any(t =>
            t.Comerciante == comerciante &&
            t.Valor == valor &&
            t.Horario > inicioJanela &&
            t.Horario <= horario);
        return duplicada ? "Transacao duplicada" : null;
    }

    public static string? Avaliar(Conta conta, IEnumerable<Transacao> historico,
        string comerciante, decimal valor, DateTime horario)
    {
        return CartaoInativo(conta)
            ?? LimiteInsuficiente(conta, valor)
            ?? AltaFrequencia(historico, horario)
            ?? TransacaoDuplicada(historico, comerciante, valor, horario);
    }
}
