import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { GetAssignmentDto, GroupClient } from 'src/app/web-api-client';
import { MakeInvitationFormComponent } from '../make-invitation-form/make-invitation-form.component';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.css'],
})
export class GroupComponent implements OnInit {
  groupId!: number;
  assignments!: GetAssignmentDto[];
  groupRole!: string;
  constructor(
    private route: ActivatedRoute,
    private groupClient: GroupClient,
    private snackBar: SnackbarService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.getGroupIdFromRoute();
    this.getUsers();
    this.getUserGroupRole();
    console.log(this.groupRole);
  }

  getUserGroupRole() {
    if (this.groupId && this.groupId > 0) {
      this.groupClient.getUserRoleGroup(this.groupId).subscribe({
        next: ({ value }) => {
          this.groupRole = value!;
        },
        error: () => {
          this.snackBar.openSnackBar(
            'Unexpected error occurred while getting user data'
          );
        },
      });
    }
  }

  openInvitationDialog() {
    let width = window.innerWidth;
    const dialogRef = this.dialog.open(MakeInvitationFormComponent, {
      width: innerWidth > 992 ? '25%' : '50%',
      data: {
        groupId: this.groupId,
      },
    });
  }

  getUsers() {
    if (this.groupId && this.groupId > 0) {
      this.groupClient.getAllAssignments(this.groupId).subscribe({
        next: ({ value }) => {
          this.assignments = value!;
        },
        error: () => {
          this.snackBar.openSnackBar(
            'Unexpected error has occurred while getting users'
          );
        },
      });
    }
  }

  getGroupIdFromRoute() {
    this.groupId = Number(this.route.snapshot.paramMap.get('groupId'));
  }
}
