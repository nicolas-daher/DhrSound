using DhrSound.Api.Data;
using DhrSound.Api.Dtos;
using DhrSound.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DhrSound.Api.Services;

public class NotificacaoService
{
    private readonly DhrSoundContext _contexto;

    public NotificacaoService(DhrSoundContext contexto) => _contexto = contexto;

    public async Task GerarParaTransacaoAsync(Transacao transacao, int usuarioId)
    {
        var situacao = transacao.Aprovada ? "APROVADA" : "RECUSADA";
        var resumo = $"Transacao {situacao} - R$ {transacao.Valor:0.00} em {transacao.Comerciante}. " +
                     $"Motivo: {transacao.Motivo}.";

        _contexto.Notificacoes.Add(new Notificacao
        {
            UsuarioId = usuarioId,
            TransacaoId = transacao.Id,
            Destinatario = "DonoCartao",
            Conteudo = resumo,
            Data = DateTime.UtcNow
        });

        _contexto.Notificacoes.Add(new Notificacao
        {
            UsuarioId = null,
            TransacaoId = transacao.Id,
            Destinatario = $"Comerciante:{transacao.Comerciante}",
            Conteudo = resumo,
            Data = DateTime.UtcNow
        });

        await _contexto.SaveChangesAsync();
    }

    public async Task<IEnumerable<NotificacaoDto>> ListarDoUsuarioAsync(int usuarioId)
    {
        return await _contexto.Notificacoes
            .Where(n => n.UsuarioId == usuarioId)
            .OrderByDescending(n => n.Data)
            .Select(n => new NotificacaoDto(n.Id, n.Destinatario, n.Conteudo, n.Data))
            .ToListAsync();
    }
}
