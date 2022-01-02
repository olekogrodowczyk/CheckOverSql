import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { SnackbarService } from '../snackbar.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  constructor(
    public authService: AuthService,
    public snackbar: SnackbarService
  ) {}

  ngOnInit(): void {}

  logout() {
    this.authService.logout();
    this.snackbar.openSnackBar('Logout completed');
  }
}
