import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { QueryResultResolver } from './query-result.resolver';
import { QueryResultComponent } from './query-result/query-result.component';

const routes: Routes = [
  {
    path: 'query-result',
    component: QueryResultComponent,
    resolve: { QueryResultResolver },
  },
  { path: '', component: HomeComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SendRequestRoutingModule {}
