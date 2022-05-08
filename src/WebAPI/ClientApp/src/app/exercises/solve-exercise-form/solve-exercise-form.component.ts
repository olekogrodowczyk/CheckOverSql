import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialog,
} from '@angular/material/dialog';
import { Store } from '@ngrx/store';
import { solveExercise } from 'src/app/shared/root-store/store/exercises/store/actions';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
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
    private store: Store,
    private snackBar: SnackbarService,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  ngOnInit(): void {}

  onSubmit() {
    if (this.solveExerciseForm.invalid) {
      return;
    }
    this.store.dispatch(
      solveExercise({
        command: <CreateSolutionCommand>{
          exerciseId: this.data.exerciseId,
          query: this.solveExerciseForm.get('query')?.value,
        },
      })
    );
    this.dialogRef.close(true);
  }
}
