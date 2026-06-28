using DhrSound.Api.Models;
using DhrSound.Api.Services;
using Xunit;

namespace DhrSound.Tests;

public class RegrasAutorizacaoTests
{
    private static Conta ContaAtiva(decimal limite = 1000m) =>
        new() { Id = 1, Ativa = true, LimiteDisponivel = limite };

    private static readonly DateTime Agora = new(2026, 6, 22, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void CartaoInativo_Recusa()
    {
        var conta = new Conta { Ativa = false, LimiteDisponivel = 1000m };
        Assert.Equal("Cartao inativo", RegrasAutorizacao.CartaoInativo(conta));
    }

    [Fact]
    public void CartaoAtivo_NaoRecusa()
    {
        Assert.Null(RegrasAutorizacao.CartaoInativo(ContaAtiva()));
    }

    [Fact]
    public void LimiteInsuficiente_Recusa()
    {
        Assert.Equal("Limite insuficiente",
            RegrasAutorizacao.LimiteInsuficiente(ContaAtiva(100m), 150m));
    }

    [Fact]
    public void LimiteSuficiente_NaoRecusa()
    {
        Assert.Null(RegrasAutorizacao.LimiteInsuficiente(ContaAtiva(100m), 100m));
    }

    [Fact]
    public void AltaFrequencia_TresEmDoisMinutos_Recusa()
    {
        var historico = new List<Transacao>
        {
            new() { Horario = Agora.AddSeconds(-100) },
            new() { Horario = Agora.AddSeconds(-50) },
        };
        var resultado = RegrasAutorizacao.AltaFrequencia(historico, Agora.AddSeconds(-10));
        Assert.Null(resultado);

        historico.Add(new Transacao { Horario = Agora.AddSeconds(-10) });
        Assert.Equal("Alta frequencia de transacoes em intervalo curto",
            RegrasAutorizacao.AltaFrequencia(historico, Agora));
    }

    [Fact]
    public void AltaFrequencia_ForaDaJanela_NaoRecusa()
    {
        var historico = new List<Transacao>
        {
            new() { Horario = Agora.AddMinutes(-10) },
            new() { Horario = Agora.AddMinutes(-9) },
            new() { Horario = Agora.AddMinutes(-8) },
        };
        Assert.Null(RegrasAutorizacao.AltaFrequencia(historico, Agora));
    }

    [Fact]
    public void TransacaoDuplicada_MesmoValorComerciante_Recusa()
    {
        var historico = new List<Transacao>
        {
            new() { Comerciante = "Loja X", Valor = 50m, Horario = Agora.AddSeconds(-30) },
        };
        Assert.Equal("Transacao duplicada",
            RegrasAutorizacao.TransacaoDuplicada(historico, "Loja X", 50m, Agora));
    }

    [Fact]
    public void TransacaoDuplicada_ComercianteDiferente_NaoRecusa()
    {
        var historico = new List<Transacao>
        {
            new() { Comerciante = "Loja X", Valor = 50m, Horario = Agora.AddSeconds(-30) },
        };
        Assert.Null(RegrasAutorizacao.TransacaoDuplicada(historico, "Loja Y", 50m, Agora));
    }

    [Fact]
    public void Avaliar_ContaInativa_TemPrioridadeSobreLimite()
    {
        var conta = new Conta { Ativa = false, LimiteDisponivel = 0m };
        var motivo = RegrasAutorizacao.Avaliar(conta, new List<Transacao>(), "Loja", 9999m, Agora);
        Assert.Equal("Cartao inativo", motivo);
    }

    [Fact]
    public void Avaliar_TudoOk_Aprova()
    {
        var motivo = RegrasAutorizacao.Avaliar(ContaAtiva(), new List<Transacao>(), "Loja", 50m, Agora);
        Assert.Null(motivo);
    }
}
