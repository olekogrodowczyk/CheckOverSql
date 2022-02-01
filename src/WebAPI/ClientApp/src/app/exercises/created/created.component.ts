import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import {
  ExerciseClient,
  GetExerciseDto,
  GetExerciseDtoPaginatedList,
} from 'src/app/web-api-client';

@Component({
  selector: 'app-created',
  templateUrl: './created.component.html',
  styleUrls: ['./created.component.css'],
})
export class CreatedComponent implements OnInit {
  data!: GetExerciseDtoPaginatedList;
  pageSize: number = 8;
  isBusy: boolean = false;
  constructor(
    private exerciseClient: ExerciseClient,
    private snackBar: SnackbarService
  ) {}

  ngOnInit(): void {
    this.getExercises(1, this.pageSize);
  }

  onPageChange(event: PageEvent) {
    console.log(event.pageIndex);
    this.getExercises(event.pageIndex + 1, this.pageSize);
  }

  getExercises(pageNumber: number, pageSize: number) {
    this.isBusy = true;
    this.exerciseClient.getAllCreated(pageNumber, pageSize).subscribe({
      next: ({ value }) => {
        this.data = value!;
      },
      error: ({ message }) => {
        this.snackBar.openSnackBar(message);
      },
    });
    this.isBusy = false;
  }
}
