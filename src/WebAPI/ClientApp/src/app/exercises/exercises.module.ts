import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExercisesRoutingModule } from './exercises-routing.module';
import { HomeComponent } from './home/home.component';
import { SharedModule } from '../shared/shared.module';
import { PublicComponent } from './public/public.component';
import { CreatedComponent } from './created/created.component';
import { CreateExerciseFormComponent } from './create-exercise-form/create-exercise-form.component';
import { MatTabsModule } from '@angular/material/tabs';

@NgModule({
  declarations: [
    HomeComponent,
    PublicComponent,
    CreatedComponent,
    CreateExerciseFormComponent,
  ],
  imports: [CommonModule, MatTabsModule, ExercisesRoutingModule, SharedModule],
})
export class ExercisesModule {}
