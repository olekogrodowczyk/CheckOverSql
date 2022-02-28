import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialog,
} from '@angular/material/dialog';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { CreateSolutionCommand, SolutionClient } from 'src/app/web-api-client';

export interface DialogData {
  exerciseId: number;
  title: string;
  description: string;
}

@Component({
  selector: 'app-solve-exercise-form',
  templateUrl: './solve-exercise-form.component.html',
  styleUrls: ['./solve-exercise-form.component.css'],
})
export class SolveExerciseFormComponent implements OnInit {
  solveExerciseForm = new FormGroup({
    query: new FormControl('', [Validators.required]),
  });

  constructor(
    public dialogRef: MatDialogRef<SolveExerciseFormComponent>,
    private solutionClient: SolutionClient,
    private snackBar: SnackbarService,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  ngOnInit(): void {}

  onSubmit() {
    if (this.solveExerciseForm.invalid) {
      return;
    }
    this.solutionClient
      .createSolution(<CreateSolutionCommand>{
        exerciseId: this.data.exerciseId,
        query: this.solveExerciseForm.get('query')?.value,
      })
      .subscribe({
        next: ({ value }) => {
          if (value?.result) {
            this.snackBar.openSnackBar("You've passed the exercise");
            this.ngOnInit();
          } else {
            this.snackBar.openSnackBar("You haven't passed the exercise");
          }
        },
        error: ({ message }) => {
          this.snackBar.openSnackBar(message);
        },
      });
    this.dialogRef.close(true);
  }
}
