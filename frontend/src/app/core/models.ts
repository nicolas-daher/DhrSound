export interface LoginResposta {
  usuarioId: number;
  nome: string;
  token: string;
}

export interface Plano {
  id: number;
  nome: string;
  descricao: string;
  preco: number;
}

export interface Banda {
  id: number;
  nome: string;
  descricao: string;
}

export interface Musica {
  id: number;
  titulo: string;
  duracaoSegundos: number;
  bandaId: number;
  bandaNome: string;
}

export interface ResultadoBusca {
  bandas: Banda[];
  musicas: Musica[];
}

export interface Favoritos {
  musicas: Musica[];
  bandas: Banda[];
}

export interface Assinatura {
  id: number;
  planoId: number;
  planoNome: string;
  dataInicio: string;
  dataFim: string | null;
  ativa: boolean;
}

export interface Notificacao {
  id: number;
  destinatario: string;
  conteudo: string;
  data: string;
}

export interface ResultadoAutorizacao {
  aprovada: boolean;
  motivo: string;
  transacaoId: number | null;
}
