import { Component, OnInit } from '@angular/core';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { DatabaseClient, QueryDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-query-list',
  templateUrl: './query-list.component.html',
  styleUrls: ['./query-list.component.css'],
})
export class QueryListComponent implements OnInit {
  queryModels!: QueryDto[];
  pageSize = 9;
  totalPages!: number;

  constructor(
    private databaseClient: DatabaseClient,
    private snackBar: SnackbarService
  ) {}

  ngOnInit(): void {
    this.getQueryHistory(1, this.pageSize);
  }

  getQueryHistory(pageNumber: number, pageSize: number) {
    this.databaseClient.getQueryHistory(pageNumber, pageSize).subscribe({
      next: (response) => {
        this.queryModels = response.value?.items!;
        this.totalPages = response.value?.totalPages!;
      },
      error: ({ message }) => {
        this.snackBar.openSnackBar(message);
      },
    });
  }
}
