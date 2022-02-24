import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { AuthGuard } from './auth/auth.guard';
import { IsLoggedInGuard } from './auth/is-logged-in.guard';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { HomePageComponent } from './home-page/home-page.component';

const routes: Routes = [
  {
    path: '',
    component: HomePageComponent,
  },
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
  {
    path: 'groups',
    canLoad: [AuthGuard],
    loadChildren: () =>
      import('./groups/groups.module').then((mod) => mod.GroupsModule),
  },
  {
    path: 'invitations',
    canLoad: [AuthGuard],
    loadChildren: () =>
      import('./invitations/invitations.module').then(
        (mod) => mod.InvitationsModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
