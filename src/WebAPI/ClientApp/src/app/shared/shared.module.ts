import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { FooterComponent } from './footer/footer.component';

@NgModule({
  declarations: [NavbarComponent, FooterComponent],
  imports: [CommonModule, MatToolbarModule],
  exports: [NavbarComponent, FooterComponent],
})
export class SharedModule {}
