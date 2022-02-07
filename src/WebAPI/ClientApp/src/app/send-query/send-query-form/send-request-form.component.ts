import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { DatabaseClient, GetQueryValueQuery } from 'src/app/web-api-client';
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
    private router: Router,
    private dialogRef: MatDialogRef<SendQueryFormComponent>
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
    this.sendQueryService.sendQuery(<GetQueryValueQuery>{
      databaseName: this.SendQueryAdminForm.get('databaseName')?.value,
      query: this.SendQueryAdminForm.get('query')?.value,
      toQueryHistory: true,
    });
    this.dialogRef.close();
  }
}
