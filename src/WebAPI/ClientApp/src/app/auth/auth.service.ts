import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, Observable } from 'rxjs';
import { SnackbarService } from '../shared/snackbar.service';
import { AccountClient, LoginUserCommand } from '../web-api-client';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  loggedUser$ = new BehaviorSubject<string | null>('');
  signedIn$ = new BehaviorSubject<boolean>(false);
  private readonly JWT_TOKEN = 'JWT_TOKEN';

  constructor(
    private client: AccountClient,
    private snackbar: SnackbarService,
    private router: Router,
    private jwtHelper: JwtHelperService
  ) {
    const token = localStorage.getItem(this.JWT_TOKEN);
    this.signedIn$.next(!!token);
    if (this.getJwtToken()) {
      const tokenDecoded = this.getTokenDecoded(this.getJwtToken()!);
      this.loggedUser$.next(tokenDecoded.login);
    }
  }

  getTokenDecoded(token: string) {
    if (this.getJwtToken()) {
      return this.jwtHelper.decodeToken(this.getJwtToken()!);
    }
  }

  getJwtToken() {
    return localStorage.getItem(this.JWT_TOKEN);
  }

  logout() {
    this.loggedUser$.next(null);
    this.removeJwtToken();
    this.signedIn$.next(false);
    this.router.navigateByUrl('/');
  }

  storeJwtToken(jwt: string) {
    localStorage.setItem(this.JWT_TOKEN, jwt);
  }

  removeJwtToken() {
    localStorage.removeItem(this.JWT_TOKEN);
  }
}
