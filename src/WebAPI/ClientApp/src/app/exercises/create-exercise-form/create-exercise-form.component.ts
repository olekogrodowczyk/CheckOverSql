import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { DatabaseClient, ExerciseClient } from 'src/app/web-api-client';
import { addExercise } from '../../shared/root-store/store/exercises/store/actions';

@Component({
  selector: 'app-create-exercise-form',
  templateUrl: './create-exercise-form.component.html',
  styleUrls: ['./create-exercise-form.component.css'],
})
export class CreateExerciseFormComponent implements OnInit {
  title!: string;
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
    private store: Store
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
    if (this.createExerciseForm.invalid) {
      return;
    }

    this.store.dispatch(addExercise(this.createExerciseForm.value));
  }
}
