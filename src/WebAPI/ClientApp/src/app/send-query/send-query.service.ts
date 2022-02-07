import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { SnackbarService } from '../shared/snackbar.service';
import {
  DatabaseClient,
  GetQueryValueQuery as GetQueryValueQuery,
} from '../web-api-client';

@Injectable({
  providedIn: 'root',
})
export class SendQueryService {
  queryResult: string[][] = [];
  constructor(
    private databaseClient: DatabaseClient,
    private snackBar: SnackbarService,
    private router: Router
  ) {}

  sendQuery(model: GetQueryValueQuery) {
    this.databaseClient.getQueryValue(model).subscribe({
      next: (result) => {
        this.queryResult = result.value!;
        this.snackBar.openSnackBar('Query executed successfully');
        this.router.navigateByUrl('send-query/query-result');
      },
      error: ({ message }) => {
        this.snackBar.openSnackBar(message);
      },
    });
  }
}
