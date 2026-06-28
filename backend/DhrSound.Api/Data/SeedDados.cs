using DhrSound.Api.Models;

namespace DhrSound.Api.Data;

public static class SeedDados
{
    public static void Popular(DhrSoundContext contexto)
    {
        if (!contexto.Planos.Any())
        {
            contexto.Planos.AddRange(
                new Plano { Nome = "Free", Descricao = "Acesso basico com anuncios", Preco = 0m },
                new Plano { Nome = "Premium", Descricao = "Sem anuncios, qualidade alta", Preco = 19.90m },
                new Plano { Nome = "Familia", Descricao = "Ate 5 contas", Preco = 34.90m });
        }

        if (!contexto.Bandas.Any())
        {
            var metallica = new Banda { Nome = "Metallica", Descricao = "Heavy metal" };
            var radiohead = new Banda { Nome = "Radiohead", Descricao = "Rock alternativo" };
            var djavan = new Banda { Nome = "Djavan", Descricao = "MPB" };

            metallica.Musicas.Add(new Musica { Titulo = "Enter Sandman", DuracaoSegundos = 331 });
            metallica.Musicas.Add(new Musica { Titulo = "Nothing Else Matters", DuracaoSegundos = 388 });
            radiohead.Musicas.Add(new Musica { Titulo = "Creep", DuracaoSegundos = 238 });
            radiohead.Musicas.Add(new Musica { Titulo = "Karma Police", DuracaoSegundos = 264 });
            djavan.Musicas.Add(new Musica { Titulo = "Oceano", DuracaoSegundos = 255 });

            contexto.Bandas.AddRange(metallica, radiohead, djavan);
        }

        contexto.SaveChanges();
    }
}
