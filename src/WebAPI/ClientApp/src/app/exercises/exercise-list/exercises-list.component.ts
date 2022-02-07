import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import {
  ExerciseClient,
  GetExerciseDto,
  GetExerciseDtoPaginatedList,
} from 'src/app/web-api-client';
import { TypeOfExercise } from '../home/home.component';

@Component({
  selector: 'app-exercise-list',
  templateUrl: './exercises-list.component.html',
  styleUrls: ['./exercises-list.component.css'],
})
export class ExerciseListComponent implements OnInit {
  @Input() typeOfExercise!: TypeOfExercise;
  data!: GetExerciseDtoPaginatedList;
  pageSize: number = 8;
  isBusy: boolean = false;
  constructor(
    private exerciseClient: ExerciseClient,
    private snackBar: SnackbarService
  ) {}

  ngOnInit(): void {
    this.getExercises(this.typeOfExercise, 1, this.pageSize);
  }

  onPageChange(event: PageEvent) {
    this.getExercises(this.typeOfExercise, event.pageIndex + 1, this.pageSize);
  }

  getExercises(
    typeOfExercise: TypeOfExercise,
    pageNumber: number,
    pageSize: number
  ) {
    switch (typeOfExercise) {
      case TypeOfExercise.Created: {
        this.getAllCreatedExercises(pageNumber, pageSize);
        break;
      }
      case TypeOfExercise.Public: {
        this.getAllPublicExercises(pageNumber, pageSize);
        break;
      }
    }
  }

  getAllCreatedExercises(pageNumber: number, pageSize: number) {
    this.exerciseClient.getAllCreated(pageNumber, pageSize).subscribe({
      next: ({ value }) => {
        this.data = value!;
      },
      error: ({ message }) => {
        this.snackBar.openSnackBar(message);
      },
    });
  }

  getAllPublicExercises(pageNumber: number, pageSize: number) {
    this.exerciseClient.getAllPublic(pageNumber, pageSize).subscribe({
      next: ({ value }) => {
        this.data = value!;
      },
      error: ({ message }) => {
        this.snackBar.openSnackBar(message);
      },
    });
  }
}
