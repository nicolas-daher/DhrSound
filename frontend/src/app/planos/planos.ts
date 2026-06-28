import { Component, inject, signal, OnInit } from '@angular/core';
import { DhrSoundService } from '../core/dhrsound.service';
import { Plano } from '../core/models';

@Component({
  selector: 'app-planos',
  templateUrl: './planos.html',
})
export class Planos implements OnInit {
  private readonly service = inject(DhrSoundService);

  planos = signal<Plano[]>([]);
  planoAtualId = signal<number | null>(null);
  planoAtualNome = signal<string | null>(null);
  mensagem = signal('');

  ngOnInit(): void {
    this.service.listarPlanos().subscribe((p) => this.planos.set(p));
    this.carregarAtual();
  }

  carregarAtual(): void {
    this.service.assinaturaAtual().subscribe((a) => {
      if (a) {
        this.planoAtualId.set(a.planoId);
        this.planoAtualNome.set(a.planoNome);
      } else {
        this.planoAtualId.set(null);
        this.planoAtualNome.set(null);
      }
    });
  }

  ehAtual(plano: Plano): boolean {
    return this.planoAtualId() === plano.id;
  }

  assinar(plano: Plano): void {
    this.service.assinar(plano.id).subscribe(() => {
      this.mensagem.set(`Assinatura do plano "${plano.nome}" ativada!`);
      this.carregarAtual();
    });
  }
}
