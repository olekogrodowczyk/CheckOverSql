import { Component, OnInit } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormGroup, Validators } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { AccountClient } from 'src/app/web-api-client';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css', '../form-shared.css'],
})
export class LoginComponent implements OnInit {
  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  constructor(
    private client: AccountClient,
    private snackbar: SnackbarService,
    private router: Router,
    private authService: AuthService
  ) {}
  ngOnInit(): void {}

  onSubmit() {
    if (this.loginForm.invalid) {
      return;
    }
    this.client.login(this.loginForm.value).subscribe(
      (result) => {
        if (result.message) this.snackbar.openSnackBar(result.message);
        if (result.value) this.authService.storeJwtToken(result.value);
        this.authService.loggedUser$.next(this.loginForm.value.email ?? '');
        this.authService.signedIn$.next(true);
        this.router.navigateByUrl('/');
      },
      (error) => {
        if (error.message) this.snackbar.openSnackBar(error.message);
      }
    );
  }
}
