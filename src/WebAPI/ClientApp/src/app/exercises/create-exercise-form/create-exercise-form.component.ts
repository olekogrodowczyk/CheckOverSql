import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { DatabaseClient, ExerciseClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-create-exercise-form',
  templateUrl: './create-exercise-form.component.html',
  styleUrls: ['./create-exercise-form.component.css'],
})
export class CreateExerciseFormComponent implements OnInit {
  createExerciseForm = new FormGroup({
    title: new FormControl('', [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(60),
    ]),
    description: new FormControl('', [
      Validators.required,
      Validators.minLength(10),
      Validators.maxLength(3000),
    ]),
    databaseName: new FormControl('', [Validators.required]),
    validAnswer: new FormControl('', [Validators.required]),
    isPrivate: new FormControl(true, [Validators.required]),
  });
  databaseNames: string[] = [];
  constructor(
    private databaseClient: DatabaseClient,
    private snackBar: SnackbarService,
    private exerciseClient: ExerciseClient
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
    console.log(this.createExerciseForm.value);
    if (this.createExerciseForm.invalid) {
      return;
    }
    console.log(this.createExerciseForm.value);
    this.exerciseClient
      .createExercise(this.createExerciseForm.value)
      .subscribe({
        next: (result) => {
          this.snackBar.openSnackBar('Exercise created successfully');
        },
        error: ({ message }) => {
          this.snackBar.openSnackBar(message);
        },
      });
  }
}
