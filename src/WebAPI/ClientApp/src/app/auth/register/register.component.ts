import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { AccountClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css', '../form-shared.css'],
})
export class RegisterComponent implements OnInit {
  registerForm = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    login: new FormControl('', [Validators.required]),
    email: new FormControl('', [
      Validators.required,
      Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$'),
    ]),
    password: new FormControl('', [Validators.required]),
    confirmPassword: new FormControl('', [Validators.required]),
  });

  constructor(
    private client: AccountClient,
    private snackBar: SnackbarService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  onSubmit() {
    if (this.registerForm.invalid) {
      return;
    }
    this.client.register(this.registerForm.value).subscribe(
      (result) => {
        if (result.message) this.snackBar.openSnackBar(result.message);
        this.router.navigateByUrl('/login');
      },
      (error) => {
        if (error.message) this.snackBar.openSnackBar(error.message);
      }
    );
  }
}
