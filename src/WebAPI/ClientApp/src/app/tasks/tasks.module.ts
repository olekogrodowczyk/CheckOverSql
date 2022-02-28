import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TasksListComponent } from './tasks-list/tasks-list.component';
import { TaskCardComponent } from './task-card/task-card.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ExercisesModule } from '../exercises/exercises.module';
import { ExerciseCardComponent } from '../exercises/exercise-card/exercise-card.component';

@NgModule({
  declarations: [TasksListComponent, TaskCardComponent],
  imports: [
    CommonModule,
    MatButtonModule,
    MatCardModule,
    ExercisesModule,
    MatDividerModule,
    FlexLayoutModule,
  ],
})
export class TasksModule {}
