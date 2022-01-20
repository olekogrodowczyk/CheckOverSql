import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IsLoggedInGuard } from './auth/is-logged-in.guard';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';

const routes: Routes = [
  {
    canActivate: [IsLoggedInGuard],
    path: 'login',
    component: LoginComponent,
  },
  {
    canActivate: [IsLoggedInGuard],
    path: 'register',
    component: RegisterComponent,
  },
  {
    path: 'send-query',
    loadChildren: () =>
      import('./send-query/send-query.module').then(
        (mod) => mod.SendQueryModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
