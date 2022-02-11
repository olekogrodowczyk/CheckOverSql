import { Injectable } from '@angular/core';
import {
  Router,
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
} from '@angular/router';
import { delay, map, Observable, of, pluck } from 'rxjs';
import { DatabaseClient } from '../web-api-client';
import { SendQueryService } from './send-query.service';

@Injectable({
  providedIn: 'root',
})
export class QueryResultResolver implements Resolve<string[][]> {
  constructor(
    private sendQueryService: SendQueryService,
    private databaseClient: DatabaseClient
  ) {}
  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<string[][]> | Observable<string[][]> | string[][] {
    console.log('Dajmosinkapsinka');

    return this.databaseClient.getQueryValue(this.sendQueryService.model).pipe(
      pluck('value'),
      map((data) => (this.sendQueryService.queryResult = data))
    );
  }
}
