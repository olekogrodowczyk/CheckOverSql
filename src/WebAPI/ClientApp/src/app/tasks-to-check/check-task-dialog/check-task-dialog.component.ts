import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { CheckSolvingCommand, SolvingClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-check-task-dialog',
  templateUrl: './check-task-dialog.component.html',
  styleUrls: ['./check-task-dialog.component.css'],
})
export class CheckTaskDialogComponent implements OnInit {
  checkDialogForm = new FormGroup({
    remarks: new FormControl(''),
    points: new FormControl(0, [
      Validators.required,
      Validators.pattern('^[0-9]*$'),
      Validators.max(this.data.maxPoints),
    ]),
  });

  constructor(
    private solvingClient: SolvingClient,
    private snackBar: SnackbarService,
    private dialogRef: MatDialogRef<CheckTaskDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: {
      title: string;
      description: string;
      answer: string;
      maxPoints: number;
      solvingId: number;
    }
  ) {}

  ngOnInit(): void {}

  onSubmit() {
    if (this.checkDialogForm.invalid) {
      return;
    }

    this.solvingClient
      .checkSolving(<CheckSolvingCommand>{
        solvingId: this.data.solvingId,
        points: this.checkDialogForm.get('points')?.value,
        remarks: this.checkDialogForm.get('remarks')?.value,
      })
      .subscribe({
        next: () => {
          this.snackBar.openSnackBar('The task has been checked successfully');
        },
        error: () => {
          this.snackBar.openSnackBar(
            'An unexpected error has occurred while sending check request'
          );
        },
      });
    this.dialogRef.close();
  }
}
