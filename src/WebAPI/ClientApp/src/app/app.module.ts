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
