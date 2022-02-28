import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TasksListComponent } from '../tasks/tasks-list/tasks-list.component';
import { GroupComponent } from './group/group.component';
import { GroupsComponent } from './groups/groups.component';
import { HomeComponent } from './home/home.component';
import { InvitationsComponent } from './invitations/invitations.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    children: [
      { path: 'invitations', component: InvitationsComponent, outlet: 'side' },
      { path: 'groups', component: GroupsComponent, outlet: 'side' },
      { path: 'group/:groupId', component: GroupComponent, outlet: 'side' },
      { path: 'tasks', component: TasksListComponent, outlet: 'side' },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GroupsRoutingModule {}
