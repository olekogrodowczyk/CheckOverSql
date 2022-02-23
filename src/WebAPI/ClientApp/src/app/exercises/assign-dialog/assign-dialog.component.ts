import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import {
  AssignExerciseToUsersCommand,
  ExerciseClient,
} from 'src/app/web-api-client';

@Component({
  selector: 'app-assign-dialog',
  templateUrl: './assign-dialog.component.html',
  styleUrls: ['./assign-dialog.component.css'],
})
export class AssignDialogComponent implements OnInit {
  deadLineForm = new FormGroup({
    date: new FormControl(new Date(), [Validators.required]),
    time: new FormControl(new String(), [Validators.required]),
  });
  constructor(
    private dialogRef: MatDialogRef<AssignDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { groupId: number; groupName: string; exerciseId: number },
    private exerciseClient: ExerciseClient,
    private snackBar: SnackbarService
  ) {}

  ngOnInit(): void {}

  closeDialog() {
    this.dialogRef.close();
  }

  assignExercise(deadLine: Date) {
    this.exerciseClient
      .assignExercise(<AssignExerciseToUsersCommand>{
        exerciseId: this.data.exerciseId,
        groupId: this.data.groupId,
        deadLine: deadLine,
      })
      .subscribe({
        next: (response) => {
          this.snackBar.openSnackBar('The exercise assigned successfully');
        },
        error: () => {
          this.snackBar.openSnackBar(
            'Unexpected error has occurred whie assigning the exercise'
          );
        },
      });
    this.closeDialog();
  }

  submit() {
    if (this.deadLineForm.invalid) {
      return;
    }
    let deadLine = this.injectTimeIntoDate();
    this.assignExercise(deadLine);
  }

  injectTimeIntoDate() {
    let date = this.deadLineForm.get('date')?.value;
    let time = this.deadLineForm.get('time')?.value;
    date.setHours(time.slice(0, 2));
    date.setMinutes(time.slice(3, 5));
    date.setSeconds(0);
    return date;
  }
}
