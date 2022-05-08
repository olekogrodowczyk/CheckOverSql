import { Component, Input, OnInit } from '@angular/core';
import { HeaderService } from 'src/app/shared/services/header.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { GetSolvingDto, SolvingClient } from 'src/app/web-api-client';

export enum TaskStatus {
  ToDo = 1,
  Overdue,
  Done,
  DoneButOverdue,
  Checked,
}

@Component({
  selector: 'app-tasks-list',
  templateUrl: './tasks-list.component.html',
  styleUrls: ['./tasks-list.component.css'],
})
export class TasksListComponent implements OnInit {
  @Input() taskStatus!: TaskStatus;
  tasks!: GetSolvingDto[];
  constructor(
    private solvingClient: SolvingClient,
    private snackBar: SnackbarService,
    private headerService: HeaderService
  ) {}

  ngOnInit(): void {
    this.headerService.headerTitle$.next('Tasks');
    this.getTasksByStatus();
  }

  getTasksByStatus() {
    this.solvingClient.getAllByStatus(TaskStatus[this.taskStatus]).subscribe({
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
