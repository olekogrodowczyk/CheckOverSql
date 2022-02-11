import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { GetSolutionDto, SolutionClient } from 'src/app/web-api-client';
import {
  DialogData,
  SolveExerciseFormComponent,
} from '../solve-exercise-form/solve-exercise-form.component';

@Component({
  selector: 'app-show-solution-dialog',
  templateUrl: './show-solution-dialog.component.html',
  styleUrls: ['./show-solution-dialog.component.css'],
})
export class ShowSolutionDialogComponent implements OnInit {
  model!: GetSolutionDto;
  constructor(
    private dialogRef: MatDialogRef<ShowSolutionDialogComponent>,
    private solutionClient: SolutionClient,
    private snackBar: SnackbarService,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  ngOnInit(): void {
    this.getSolutionModel();
  }

  getSolutionModel() {
    this.solutionClient
      .getLastSolutionSentIntoExercise(this.data.exerciseId)
      .subscribe({
        next: ({ value }) => {
          this.model = value!;
        },
        error: ({ message }) => {
          this.snackBar.openSnackBar(message);
        },
      });
  }
}
