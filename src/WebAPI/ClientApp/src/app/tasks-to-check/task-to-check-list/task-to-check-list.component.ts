import { Component, OnInit } from '@angular/core';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { GetSolvingDto, SolvingClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-task-to-check-list',
  templateUrl: './task-to-check-list.component.html',
  styleUrls: ['./task-to-check-list.component.css'],
})
export class TaskToCheckListComponent implements OnInit {
  tasksToCheck!: GetSolvingDto[];
  constructor(
    private solvingClient: SolvingClient,
    private snackBar: SnackbarService
  ) {}

  ngOnInit(): void {
    this.getTasksToCheck();
  }

  getTasksToCheck() {
    this.solvingClient.getAllToCheck(undefined).subscribe({
      next: ({ value }) => {
        this.tasksToCheck = value!;
      },
      error: () => {
        this.snackBar.openSnackBar(
          'Unexpected error has occurred while getting tasks to check'
        );
      },
    });
  }
}
