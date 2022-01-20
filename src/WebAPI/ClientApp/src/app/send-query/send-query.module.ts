import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SendRequestRoutingModule } from './send-query-routing.module';
import { HomeComponent } from './home/home.component';
import { SendQueryFormComponent } from './send-query-form/send-request-form.component';

@NgModule({
  declarations: [HomeComponent, SendQueryFormComponent],
  imports: [CommonModule, SendRequestRoutingModule],
})
export class SendQueryModule {}
