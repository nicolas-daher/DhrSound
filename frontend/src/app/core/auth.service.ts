import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginResposta } from './models';

const CHAVE_TOKEN = 'dhrsound_token';
const CHAVE_NOME = 'dhrsound_nome';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly api = 'http://localhost:5000/api';

  readonly token = signal<string | null>(localStorage.getItem(CHAVE_TOKEN));
  readonly nome = signal<string | null>(localStorage.getItem(CHAVE_NOME));
  readonly autenticado = computed(() => this.token() !== null);

  constructor(private http: HttpClient) {}

  cadastrar(nome: string, email: string, senha: string): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(`${this.api}/usuarios`, { nome, email, senha });
  }

  login(email: string, senha: string): Observable<LoginResposta> {
    return this.http.post<LoginResposta>(`${this.api}/login`, { email, senha }).pipe(
      tap((r) => {
        localStorage.setItem(CHAVE_TOKEN, r.token);
        localStorage.setItem(CHAVE_NOME, r.nome);
        this.token.set(r.token);
        this.nome.set(r.nome);
      })
    );
  }

  sair(): void {
    localStorage.removeItem(CHAVE_TOKEN);
    localStorage.removeItem(CHAVE_NOME);
    this.token.set(null);
    this.nome.set(null);
  }
}
