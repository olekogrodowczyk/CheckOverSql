import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { DatabaseClient } from 'src/app/web-api-client';

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
    private snackBar: SnackbarService
  ) {}

  ngOnInit(): void {
    this.getDatabaseNames();
    console.log(this.databaseNames);
  }

  getDatabaseNames() {
    this.databaseClient.getdatabasenames().subscribe({
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
  }
}
