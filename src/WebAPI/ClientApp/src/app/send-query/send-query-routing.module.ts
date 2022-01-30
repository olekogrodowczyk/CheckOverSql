import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { IsResultEmptyGuard } from './is-result-empty.guard';
import { QueryResultComponent } from './query-result/query-result.component';

const routes: Routes = [
  {
    path: 'query-result',
    canActivate: [IsResultEmptyGuard],
    component: QueryResultComponent,
  },
  { path: '', component: HomeComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [IsResultEmptyGuard],
})
export class SendRequestRoutingModule {}
