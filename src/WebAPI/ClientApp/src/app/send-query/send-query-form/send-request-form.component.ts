import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { DialogData } from 'src/app/exercises/solve-exercise-form/solve-exercise-form.component';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { DatabaseClient, GetQueryValueQuery } from 'src/app/web-api-client';
import { SendQueryService } from '../send-query.service';

@Component({
  selector: 'app-send-query-form',
  templateUrl: './send-request-form.component.html',
  styleUrls: ['./send-request-form.component.css'],
})
export class SendQueryFormComponent implements OnInit {
  disableForm: boolean = false;
  databaseNames!: string[];
  SendQueryAdminForm = new FormGroup({
    databaseName: new FormControl(
      {
        value: this.data ? this.data.databaseName : '',
        disabled: this.data ? true : false,
      },
      [Validators.required]
    ),
    query: new FormControl(
      {
        value: this.data ? this.data.query : '',
        disabled: this.data ? true : false,
      },
      [Validators.required]
    ),
  });
  constructor(
    private databaseClient: DatabaseClient,
    private snackBar: SnackbarService,
    private sendQueryService: SendQueryService,
    private router: Router,
    private dialogRef: MatDialogRef<SendQueryFormComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: {
      databaseName: string;
      query: string;
    }
  ) {}

  ngOnInit(): void {
    this.getDatabaseNames();
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
    this.sendQueryService.model = <GetQueryValueQuery>{
      databaseName: this.SendQueryAdminForm.get('databaseName')?.value,
      query: this.SendQueryAdminForm.get('query')?.value,
      toQueryHistory: this.data ? false : true,
    };
    this.router.navigateByUrl('send-query/query-result');

    this.dialogRef.close();
  }
}
