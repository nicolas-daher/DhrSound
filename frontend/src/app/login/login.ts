import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../core/auth.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
})
export class Login {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  email = '';
  senha = '';
  erro = signal('');
  carregando = signal(false);

  entrar(): void {
    this.erro.set('');
    this.carregando.set(true);
    this.auth.login(this.email, this.senha).subscribe({
      next: () => this.router.navigate(['/busca']),
      error: () => {
        this.erro.set('E-mail ou senha inválidos.');
        this.carregando.set(false);
      },
    });
  }
}
