import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { AccountClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css', '../form-shared.css'],
})
export class RegisterComponent implements OnInit {
  registerForm = new FormGroup({
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    email: new FormControl('', [
      Validators.required,
      Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$'),
    ]),
    password: new FormControl('', [Validators.required]),
    confirmPassword: new FormControl('', [Validators.required]),
  });

  constructor(private client: AccountClient) {}

  ngOnInit(): void {}

  onSubmit() {
    if (this.registerForm.invalid) {
      return;
    }
    this.client.register(this.registerForm.value).subscribe(
      (result) => {
        console.log(result);
        console.log(result.message);
      },
      (error) => {
        console.log(error.message);
        console.log(error);
      }
    );
  }
}
