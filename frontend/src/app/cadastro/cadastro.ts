import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../core/auth.service';

@Component({
  selector: 'app-cadastro',
  imports: [FormsModule, RouterLink],
  templateUrl: './cadastro.html',
})
export class Cadastro {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  nome = '';
  email = '';
  senha = '';
  erro = signal('');
  carregando = signal(false);

  cadastrar(): void {
    this.erro.set('');
    this.carregando.set(true);
    this.auth.cadastrar(this.nome, this.email, this.senha).subscribe({
      next: () => {
        this.auth.login(this.email, this.senha).subscribe({
          next: () => this.router.navigate(['/busca']),
          error: () => this.router.navigate(['/login']),
        });
      },
      error: (e) => {
        this.erro.set(e?.error?.erro ?? 'Não foi possível cadastrar.');
        this.carregando.set(false);
      },
    });
  }
}
