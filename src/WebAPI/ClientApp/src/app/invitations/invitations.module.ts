import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FlexLayoutModule } from '@angular/flex-layout';
import { SharedModule } from '../shared/shared.module';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatListModule } from '@angular/material/list';
import { MatTableModule } from '@angular/material/table';
import { ExercisesModule } from '../exercises/exercises.module';
import { MatSelectModule } from '@angular/material/select';
import { MakeInvitationFormComponent } from './make-invitation-form/make-invitation-form.component';
import { InvitationCardComponent } from './invitation-card/invitation-card.component';
import { InvitationsListComponent } from './invitations-list/invitations-list.component';

@NgModule({
  declarations: [
    InvitationCardComponent,
    InvitationsListComponent,
    MakeInvitationFormComponent,
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    FlexLayoutModule,
    SharedModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatListModule,
    MatTableModule,
    MatSelectModule,
  ],
  exports: [MakeInvitationFormComponent],
})
export class InvitationsModule {}
