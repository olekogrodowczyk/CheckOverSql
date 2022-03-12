import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskToCheckListComponent } from './task-to-check-list/task-to-check-list.component';
import { TaskToCheckCardComponent } from './task-to-check-card/task-to-check-card.component';
import { HomeComponent } from './home/home.component';
import { ExercisesModule } from '../exercises/exercises.module';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatTabsModule } from '@angular/material/tabs';
import { SharedModule } from '../shared/shared.module';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule } from '@angular/material/dialog';
import { CheckTaskDialogComponent } from './check-task-dialog/check-task-dialog.component';

@NgModule({
  declarations: [
    CheckTaskDialogComponent,
    TaskToCheckListComponent,
    TaskToCheckCardComponent,
    HomeComponent,
  ],
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    ExercisesModule,
    MatDividerModule,
    FlexLayoutModule,
    SharedModule,
    MatTabsModule,
    MatSelectModule,
    ReactiveFormsModule,
    MatDialogModule,
  ],
})
export class TasksToCheckModule {}
