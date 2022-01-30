import { Component, OnInit } from '@angular/core';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { ExerciseClient, GetExerciseDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-created',
  templateUrl: './created.component.html',
  styleUrls: ['./created.component.css'],
})
export class CreatedComponent implements OnInit {
  exercises: GetExerciseDto[] = [];
  constructor(
    private exerciseClient: ExerciseClient,
    private snackBar: SnackbarService
  ) {}

  ngOnInit(): void {
    this.getExercises();
  }

  getExercises() {
    this.exerciseClient.getallcreated().subscribe({
      next: ({ value }) => {
        this.exercises = value!;
      },
      error: ({ message }) => {
        this.snackBar.openSnackBar(message);
      },
    });
  }
}
