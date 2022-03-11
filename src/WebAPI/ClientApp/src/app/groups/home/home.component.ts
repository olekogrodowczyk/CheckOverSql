import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import {
  ExerciseClient,
  GetGroupDto,
  GroupClient,
  GroupRoleClient,
  SolvingClient,
} from 'src/app/web-api-client';
import { CreateGroupFormComponent } from '../create-group-form/create-group-form.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  checkingTasksPermissionResult!: boolean;
  constructor(
    private router: Router,
    private groupRoleClient: GroupRoleClient,
    private snackBar: SnackbarService
  ) {
    this.router.navigateByUrl('groups/(side:groups)');
  }
  refreshSubject: Subject<boolean> = new Subject<boolean>();

  ngOnInit(): void {
    this.checkCheckingTasksPermission();
  }

  checkCheckingTasksPermission() {
    this.groupRoleClient
      .checkPermission('CheckingExercises', undefined)
      .subscribe({
        next: ({ value }) => {
          this.checkingTasksPermissionResult = value!;
        },
        error: () => {
          this.snackBar.openSnackBar('Unexpected error has occurred');
        },
      });
  }
}
