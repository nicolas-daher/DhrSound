import { Component, inject, signal, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DhrSoundService } from '../core/dhrsound.service';
import { AuthService } from '../core/auth.service';
import { ResultadoBusca } from '../core/models';

@Component({
  selector: 'app-busca',
  imports: [FormsModule],
  templateUrl: './busca.html',
  styleUrl: './busca.css',
})
export class Busca implements OnInit {
  private readonly service = inject(DhrSoundService);
  protected readonly auth = inject(AuthService);

  termo = '';
  resultado = signal<ResultadoBusca | null>(null);
  buscou = signal(false);

  favMusicas = signal<Set<number>>(new Set());
  favBandas = signal<Set<number>>(new Set());

  ngOnInit(): void {
    this.buscar();
    if (this.auth.autenticado()) this.carregarFavoritos();
  }

  buscar(): void {
    this.service.buscar(this.termo).subscribe((r) => {
      this.resultado.set(r);
      this.buscou.set(true);
    });
  }

  limparFiltro(): void {
    this.termo = '';
    this.buscar();
  }

  private carregarFavoritos(): void {
    this.service.listarFavoritos().subscribe((f) => {
      this.favMusicas.set(new Set(f.musicas.map((m) => m.id)));
      this.favBandas.set(new Set(f.bandas.map((b) => b.id)));
    });
  }

  musicaFavoritada(id: number): boolean {
    return this.favMusicas().has(id);
  }

  bandaFavoritada(id: number): boolean {
    return this.favBandas().has(id);
  }

  alternarMusica(id: number): void {
    if (this.musicaFavoritada(id)) {
      this.service.desfavoritarMusica(id).subscribe(() => this.atualizar(this.favMusicas, id, false));
    } else {
      this.service.favoritarMusica(id).subscribe(() => this.atualizar(this.favMusicas, id, true));
    }
  }

  alternarBanda(id: number): void {
    if (this.bandaFavoritada(id)) {
      this.service.desfavoritarBanda(id).subscribe(() => this.atualizar(this.favBandas, id, false));
    } else {
      this.service.favoritarBanda(id).subscribe(() => this.atualizar(this.favBandas, id, true));
    }
  }

  private atualizar(alvo: typeof this.favMusicas, id: number, incluir: boolean): void {
    const proximo = new Set(alvo());
    if (incluir) proximo.add(id);
    else proximo.delete(id);
    alvo.set(proximo);
  }
}
