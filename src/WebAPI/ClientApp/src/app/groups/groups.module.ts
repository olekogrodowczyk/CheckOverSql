import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GroupsRoutingModule } from './groups-routing.module';
import { HomeComponent } from './home/home.component';
import { CreateGroupFormComponent } from './create-group-form/create-group-form.component';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FlexLayoutModule } from '@angular/flex-layout';
import { SharedModule } from '../shared/shared.module';
import { GroupsListComponent } from './groups-list/groups-list.component';
import { GroupCardComponent } from './group-card/group-card.component';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { GroupComponent } from './group/group.component';

@NgModule({
  declarations: [
    HomeComponent,
    CreateGroupFormComponent,
    GroupsListComponent,
    GroupCardComponent,
    GroupComponent,
  ],
  imports: [
    CommonModule,
    GroupsRoutingModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    FlexLayoutModule,
    SharedModule,
    MatCardModule,
    MatProgressSpinnerModule,
  ],
})
export class GroupsModule {}
