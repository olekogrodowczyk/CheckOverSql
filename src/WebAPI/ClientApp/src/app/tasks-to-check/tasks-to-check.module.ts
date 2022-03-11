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

@NgModule({
  declarations: [
    TaskToCheckListComponent,
    TaskToCheckCardComponent,
    HomeComponent,
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatCardModule,
    ExercisesModule,
    MatDividerModule,
    FlexLayoutModule,
    SharedModule,
    MatTabsModule,
  ],
})
export class TasksToCheckModule {}
