import { Component, OnInit } from '@angular/core';
import { HeaderService } from 'src/app/shared/header.service';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { GetSolvingDto, SolvingClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-tasks-list',
  templateUrl: './tasks-list.component.html',
  styleUrls: ['./tasks-list.component.css'],
})
export class TasksListComponent implements OnInit {
  tasks!: GetSolvingDto[];
  constructor(
    private solvingClient: SolvingClient,
    private snackBar: SnackbarService,
    private headerService: HeaderService
  ) {}

  ngOnInit(): void {
    this.headerService.headerTitle$.next('Tasks');
    this.getAllTasks();
  }

  getAllTasks() {
    this.solvingClient.getAll().subscribe({
      next: ({ value }) => {
        this.tasks = value!;
      },
      error: (response) => {
        this.snackBar.openSnackBar(
          'An unexpected error has occurred while getting tasks'
        );
      },
    });
  }
}
