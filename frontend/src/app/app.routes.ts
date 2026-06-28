import { Routes } from '@angular/router';
import { authGuard } from './core/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'busca', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./login/login').then((m) => m.Login) },
  { path: 'cadastro', loadComponent: () => import('./cadastro/cadastro').then((m) => m.Cadastro) },
  { path: 'busca', loadComponent: () => import('./busca/busca').then((m) => m.Busca) },
  {
    path: 'planos',
    canActivate: [authGuard],
    loadComponent: () => import('./planos/planos').then((m) => m.Planos),
  },
  {
    path: 'favoritos',
    canActivate: [authGuard],
    loadComponent: () => import('./favoritos/favoritos').then((m) => m.Favoritos),
  },
  { path: '**', redirectTo: 'busca' },
];
