import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { SendQueryService } from './send-query.service';

@Injectable({
  providedIn: 'root',
})
export class IsResultEmptyGuard implements CanActivate {
  constructor(private sendQueryService: SendQueryService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    let queryResult = this.sendQueryService.queryResult;
    if (queryResult.length == 0) {
      return false;
    }
    return true;
  }
}
