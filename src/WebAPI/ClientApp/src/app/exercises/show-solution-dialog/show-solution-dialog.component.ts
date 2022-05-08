import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { delay } from 'rxjs';
import { SendQueryService } from 'src/app/send-query/send-query.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import {
  GetQueryValueQuery,
  GetSolutionDto,
  SolutionClient,
} from 'src/app/web-api-client';
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
    private sendQueryService: SendQueryService,
    private router: Router,
    @Inject(MAT_DIALOG_DATA)
    public data: { exerciseId: number; databaseName: string }
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

  executeQuery() {
    this.sendQueryService.model = <GetQueryValueQuery>{
      databaseName: this.data.databaseName,
      query: this.model.query,
    };
    this.router.navigateByUrl('send-query/query-result');
    this.dialogRef.close();
  }
}
