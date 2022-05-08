import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { LoginComponent } from 'src/app/auth/login/login.component';
import { RegisterComponent } from 'src/app/auth/register/register.component';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  constructor(
    public authService: AuthService,
    public snackbar: SnackbarService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {}

  openSignInDialog() {
    const dialogRef = this.dialog.open(LoginComponent);
  }

  openSignUpDialog() {
    const dialogRef = this.dialog.open(RegisterComponent);
  }

  logout() {
    this.authService.logout();
    this.snackbar.openSnackBar('Logout completed');
  }
}
