import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExercisesRoutingModule } from './exercises-routing.module';
import { HomeComponent } from './home/home.component';
import { SharedModule } from '../shared/shared.module';
import { CreateExerciseFormComponent } from './create-exercise-form/create-exercise-form.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { ExerciseCardComponent } from './exercise-card/exercise-card.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ExerciseListComponent } from './exercise-list/exercises-list.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule } from '@angular/material/dialog';
import { SolveExerciseFormComponent } from './solve-exercise-form/solve-exercise-form.component';
import { ShowSolutionDialogComponent } from './show-solution-dialog/show-solution-dialog.component';

@NgModule({
  declarations: [
    HomeComponent,
    CreateExerciseFormComponent,
    ExerciseCardComponent,
    ExerciseListComponent,
    SolveExerciseFormComponent,
    ShowSolutionDialogComponent,
  ],
  imports: [
    CommonModule,
    MatTabsModule,
    ExercisesRoutingModule,
    SharedModule,
    MatCardModule,
    MatDividerModule,
    MatButtonModule,
    MatGridListModule,
    FlexLayoutModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatCheckboxModule,
    MatDialogModule,
  ],
})
export class ExercisesModule {}
