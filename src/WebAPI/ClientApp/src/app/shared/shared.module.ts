import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { FooterComponent } from './components/footer/footer.component';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { SnackbarService } from './snackbar.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatMenu, MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { InvitationsModule } from '../invitations/invitations.module';
import { MatDialogModule } from '@angular/material/dialog';
import { HeaderService } from './header.service';
import { AuthModule } from '../auth/auth.module';
import { HeaderComponent } from './components/header/header.component';
import { NavbarComponent } from './components/navbar/navbar.component';

@NgModule({
  declarations: [NavbarComponent, FooterComponent, HeaderComponent],
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    RouterModule,
    MatSnackBarModule,
    MatMenuModule,
    MatIconModule,
    InvitationsModule,
    MatDialogModule,
    AuthModule,
  ],
  exports: [NavbarComponent, FooterComponent, HeaderComponent],
  providers: [SnackbarService],
})
export class SharedModule {}
