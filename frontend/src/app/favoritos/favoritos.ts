import { Component, inject, signal, OnInit } from '@angular/core';
import { DhrSoundService } from '../core/dhrsound.service';
import { Favoritos as FavoritosModel } from '../core/models';

@Component({
  selector: 'app-favoritos',
  templateUrl: './favoritos.html',
})
export class Favoritos implements OnInit {
  private readonly service = inject(DhrSoundService);

  dados = signal<FavoritosModel | null>(null);

  ngOnInit(): void {
    this.carregar();
  }

  carregar(): void {
    this.service.listarFavoritos().subscribe((d) => this.dados.set(d));
  }

  removerMusica(id: number): void {
    this.service.desfavoritarMusica(id).subscribe(() => this.carregar());
  }

  removerBanda(id: number): void {
    this.service.desfavoritarBanda(id).subscribe(() => this.carregar());
  }
}
