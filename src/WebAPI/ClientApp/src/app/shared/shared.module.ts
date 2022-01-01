import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { FooterComponent } from './footer/footer.component';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { SnackbarService } from './snackbar.service';

@NgModule({
  declarations: [NavbarComponent, FooterComponent],
  imports: [CommonModule, MatToolbarModule, MatButtonModule, RouterModule],
  exports: [NavbarComponent, FooterComponent],
  providers: [SnackbarService],
})
export class SharedModule {}
