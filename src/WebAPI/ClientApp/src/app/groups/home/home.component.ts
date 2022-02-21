import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import {
  ExerciseClient,
  GetGroupDto,
  GroupClient,
} from 'src/app/web-api-client';
import { CreateGroupFormComponent } from '../create-group-form/create-group-form.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  groups!: GetGroupDto[];
  refreshSubject: Subject<boolean> = new Subject<boolean>();

  constructor(
    private dialog: MatDialog,
    private groupClient: GroupClient,
    private snackBar: SnackbarService
  ) {}

  ngOnInit(): void {
    this.getImages();
  }

  getImages() {
    this.groupClient.getUserGroups().subscribe({
      next: ({ value }) => {
        this.groups = value!;
      },
      error: () => {
        this.snackBar.openSnackBar(
          'Unexpected error occurred while getting groups'
        );
      },
    });
  }

  openCreateGroupDialog() {
    let innerWidth = window.innerWidth;
    const dialogRef = this.dialog.open(CreateGroupFormComponent, {
      width: innerWidth > 992 ? '25%' : '50%',
    });
    dialogRef.afterClosed().subscribe(() => {
      this.ngOnInit();
    });
  }
}
