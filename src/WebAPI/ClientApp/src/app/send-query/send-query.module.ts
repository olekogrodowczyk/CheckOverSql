import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { SendRequestRoutingModule } from './send-query-routing.module';
import { HomeComponent } from './home/home.component';
import { SendQueryFormComponent } from './send-query-form/send-request-form.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { QueryResultComponent } from './query-result/query-result.component';
import { MatTableModule } from '@angular/material/table';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  declarations: [HomeComponent, SendQueryFormComponent, QueryResultComponent],
  imports: [
    CommonModule,
    SendRequestRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    FlexLayoutModule,
  ],
})
export class SendQueryModule {}
