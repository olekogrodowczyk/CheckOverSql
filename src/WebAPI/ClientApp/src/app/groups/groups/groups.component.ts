import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { HeaderService } from 'src/app/shared/header.service';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { GetGroupDto, GroupClient } from 'src/app/web-api-client';
import { CreateGroupFormComponent } from '../create-group-form/create-group-form.component';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css'],
})
export class GroupsComponent implements OnInit {
  groups!: GetGroupDto[];
  constructor(
    private dialog: MatDialog,
    private groupClient: GroupClient,
    private snackBar: SnackbarService,
    private headerService: HeaderService
  ) {}

  ngOnInit(): void {
    this.headerService.headerTitle$.next('Groups');
    this.getGroups();
  }

  getGroups() {
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
