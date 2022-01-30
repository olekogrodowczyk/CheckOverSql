import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';
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
    path: 'exercises',
    canLoad: [AuthGuard],
    loadChildren: () =>
      import('./exercises/exercises.module').then((mod) => mod.ExercisesModule),
  },
  {
    path: 'send-query',
    canLoad: [AuthGuard],
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
