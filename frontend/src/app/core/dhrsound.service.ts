import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Plano,
  Assinatura,
  ResultadoBusca,
  Favoritos,
  Notificacao,
  ResultadoAutorizacao,
} from './models';

@Injectable({ providedIn: 'root' })
export class DhrSoundService {
  private readonly api = 'http://localhost:5000/api';

  constructor(private http: HttpClient) {}

  listarPlanos(): Observable<Plano[]> {
    return this.http.get<Plano[]>(`${this.api}/planos`);
  }

  assinar(planoId: number): Observable<Assinatura> {
    return this.http.post<Assinatura>(`${this.api}/assinaturas`, { planoId });
  }

  assinaturaAtual(): Observable<Assinatura> {
    return this.http.get<Assinatura>(`${this.api}/assinaturas/atual`);
  }

  buscar(termo: string): Observable<ResultadoBusca> {
    let params = new HttpParams();
    if (termo.trim()) params = params.set('termo', termo.trim());
    return this.http.get<ResultadoBusca>(`${this.api}/busca`, { params });
  }

  listarFavoritos(): Observable<Favoritos> {
    return this.http.get<Favoritos>(`${this.api}/favoritos`);
  }

  favoritarMusica(id: number): Observable<unknown> {
    return this.http.post(`${this.api}/favoritos/musicas/${id}`, {});
  }

  desfavoritarMusica(id: number): Observable<unknown> {
    return this.http.delete(`${this.api}/favoritos/musicas/${id}`);
  }

  favoritarBanda(id: number): Observable<unknown> {
    return this.http.post(`${this.api}/favoritos/bandas/${id}`, {});
  }

  desfavoritarBanda(id: number): Observable<unknown> {
    return this.http.delete(`${this.api}/favoritos/bandas/${id}`);
  }

  listarNotificacoes(): Observable<Notificacao[]> {
    return this.http.get<Notificacao[]>(`${this.api}/notificacoes`);
  }

  autorizarTransacao(
    contaId: number,
    comerciante: string,
    valor: number,
    horario: string
  ): Observable<ResultadoAutorizacao> {
    return this.http.post<ResultadoAutorizacao>(`${this.api}/transacoes/autorizar`, {
      contaId,
      comerciante,
      valor,
      horario,
    });
  }
}
