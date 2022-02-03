import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { DatabaseClient } from 'src/app/web-api-client';
import { SendQueryService } from '../send-query.service';

@Component({
  selector: 'app-send-query-form',
  templateUrl: './send-request-form.component.html',
  styleUrls: ['./send-request-form.component.css'],
})
export class SendQueryFormComponent implements OnInit {
  SendQueryAdminForm = new FormGroup({
    databaseName: new FormControl('', [Validators.required]),
    query: new FormControl('', [Validators.required]),
  });
  databaseNames: string[] = [];
  constructor(
    private databaseClient: DatabaseClient,
    private snackBar: SnackbarService,
    private sendQueryService: SendQueryService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getDatabaseNames();
    console.log(this.databaseNames);
  }

  getDatabaseNames() {
    this.databaseClient.getDatabaseNames().subscribe({
      next: (result) => {
        this.databaseNames = result.value!;
      },
      error: () => {
        this.snackBar.openSnackBar(
          'Error occurred while getting database names'
        );
      },
    });
  }

  onSubmit() {
    if (this.SendQueryAdminForm.invalid) {
      return;
    }
    console.log(this.SendQueryAdminForm.value);
    this.databaseClient.getQueryValue(this.SendQueryAdminForm.value).subscribe({
      next: (result) => {
        this.sendQueryService.queryResult = result.value!;
        this.snackBar.openSnackBar('Query executed successfully');
        this.router.navigateByUrl('send-query/query-result');
      },
      error: ({ message }) => {
        this.snackBar.openSnackBar(message);
      },
    });
  }
}
