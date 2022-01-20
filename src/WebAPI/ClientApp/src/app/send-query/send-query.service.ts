import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SendQueryService {
  queryResult: string[][] = [];
  constructor() {}
}
