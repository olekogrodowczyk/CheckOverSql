import { Injectable } from '@angular/core';
import {
  Router,
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
} from '@angular/router';
import {
  catchError,
  delay,
  map,
  Observable,
  of,
  pluck,
  take,
  tap,
  throwError,
} from 'rxjs';
import { SnackbarService } from '../shared/services/snackbar.service';
import { DatabaseClient } from '../web-api-client';
import { SendQueryService } from './send-query.service';

@Injectable({
  providedIn: 'root',
})
export class QueryResultResolver implements Resolve<string[][] | undefined> {
  constructor(
    private sendQueryService: SendQueryService,
    private databaseClient: DatabaseClient,
    private snackBar: SnackbarService
  ) {}
  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<string[][]> | Observable<string[][]> | string[][] {
    return this.databaseClient.getQueryValue(this.sendQueryService.model).pipe(
      pluck('value'),
      map((data) => (this.sendQueryService.queryResult = data!)),
      catchError((err, caught) => {
        this.snackBar.openSnackBar(err.message);
        return throwError(() => new Error(err));
      })
    );
  }
}
