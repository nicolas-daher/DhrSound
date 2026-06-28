using DhrSound.Api.Data;
using DhrSound.Api.Dtos;
using DhrSound.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DhrSound.Api.Services;

public class TransacaoService
{
    private readonly DhrSoundContext _contexto;
    private readonly NotificacaoService _notificacoes;

    public TransacaoService(DhrSoundContext contexto, NotificacaoService notificacoes)
    {
        _contexto = contexto;
        _notificacoes = notificacoes;
    }

    public async Task<ResultadoAutorizacaoDto> AutorizarAsync(AutorizarTransacaoDto dto)
    {
        var conta = await _contexto.Contas.FirstOrDefaultAsync(c => c.Id == dto.ContaId);
        if (conta is null)
            return new ResultadoAutorizacaoDto(false, "Conta nao encontrada", null);

        var inicioJanela = dto.Horario - RegrasAutorizacao.JanelaFrequencia;
        var historico = await _contexto.Transacoes
            .Where(t => t.ContaId == conta.Id && t.Horario > inicioJanela && t.Horario <= dto.Horario)
            .ToListAsync();

        var motivo = RegrasAutorizacao.Avaliar(conta, historico, dto.Comerciante, dto.Valor, dto.Horario);
        var aprovada = motivo is null;

        var transacao = new Transacao
        {
            ContaId = conta.Id,
            Comerciante = dto.Comerciante,
            Valor = dto.Valor,
            Horario = dto.Horario,
            Aprovada = aprovada,
            Motivo = aprovada ? "Aprovada" : motivo!
        };

        if (aprovada)
            conta.LimiteDisponivel -= dto.Valor;

        _contexto.Transacoes.Add(transacao);
        await _contexto.SaveChangesAsync();

        await _notificacoes.GerarParaTransacaoAsync(transacao, conta.UsuarioId);

        return new ResultadoAutorizacaoDto(aprovada, transacao.Motivo, transacao.Id);
    }
}
