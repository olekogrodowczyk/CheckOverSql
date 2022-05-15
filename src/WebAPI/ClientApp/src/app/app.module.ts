import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './shared/shared.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import {
  AccountClient,
  DatabaseClient,
  ExerciseClient,
  GroupClient,
  GroupRoleClient,
  InvitationClient,
  SolutionClient,
  SolvingClient,
} from './web-api-client';
import { TokenInterceptor } from './auth/token.interceptor';
import { AuthModule } from './auth/auth.module';
import { BaseUrlInterceptor } from './base-url.interceptor';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { HomePageComponent } from './home-page/home-page.component';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { exerciseReducer } from './shared/root-store/store/exercises/reducer';
import { ExerciseEffects } from './shared/root-store/store/exercises/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from 'src/environments/environment';

@NgModule({
  declarations: [AppComponent, HomePageComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SharedModule,
    HttpClientModule,
    AuthModule,
    MatProgressBarModule,
    StoreModule.forRoot({ exercise: exerciseReducer }),
    EffectsModule.forRoot([ExerciseEffects]),
    StoreDevtoolsModule.instrument({
      maxAge: 25,
      logOnly: environment.production,
      autoPause: true,
    }),
  ],
  providers: [
    AccountClient,
    DatabaseClient,
    ExerciseClient,
    SolutionClient,
    GroupClient,
    GroupRoleClient,
    InvitationClient,
    SolvingClient,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: BaseUrlInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
  exports: [],
})
export class AppModule {}
