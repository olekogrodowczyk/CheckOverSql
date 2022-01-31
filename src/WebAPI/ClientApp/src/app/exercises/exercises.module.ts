import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExercisesRoutingModule } from './exercises-routing.module';
import { HomeComponent } from './home/home.component';
import { SharedModule } from '../shared/shared.module';
import { PublicComponent } from './public/public.component';
import { CreatedComponent } from './created/created.component';
import { CreateExerciseFormComponent } from './create-exercise-form/create-exercise-form.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { ExerciseCardComponent } from './exercise-card/exercise-card.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  declarations: [
    HomeComponent,
    PublicComponent,
    CreatedComponent,
    CreateExerciseFormComponent,
    ExerciseCardComponent,
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
  ],
})
export class ExercisesModule {}
