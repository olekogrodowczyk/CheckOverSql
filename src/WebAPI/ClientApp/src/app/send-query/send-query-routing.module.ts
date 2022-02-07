import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { QueryResultComponent } from './query-result/query-result.component';

const routes: Routes = [
  {
    path: 'query-result',
    component: QueryResultComponent,
  },
  { path: '', component: HomeComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SendRequestRoutingModule {}
