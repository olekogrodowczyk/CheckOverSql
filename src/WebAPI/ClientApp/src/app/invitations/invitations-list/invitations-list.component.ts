import { Component, OnInit } from '@angular/core';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import {
  AccountClient,
  GetInvitationDto,
  InvitationClient,
} from 'src/app/web-api-client';

@Component({
  selector: 'app-invitations-list',
  templateUrl: './invitations-list.component.html',
  styleUrls: ['./invitations-list.component.css'],
})
export class InvitationsListComponent implements OnInit {
  invitations!: GetInvitationDto[];
  loggedUserId!: number;
  constructor(
    private invitationClient: InvitationClient,
    private snackBar: SnackbarService,
    private accountClient: AccountClient
  ) {}

  ngOnInit(): void {
    this.getAllInvitations();
    this.getLoggedUserId();
  }

  refresh() {
    this.ngOnInit();
  }

  getLoggedUserId() {
    this.accountClient.getLoggedUserId().subscribe({
      next: ({ value }) => {
        this.loggedUserId = value!;
      },
      error: (response) => {
        console.log('Unexpected error occurred');
      },
    });
  }

  getAllInvitations() {
    this.invitationClient.getAll('All').subscribe({
      next: ({ value }) => {
        this.invitations = value!;
      },
      error: (response) => {
        this.snackBar.openSnackBar(
          'Unexpected error has occurred while getting invitations'
        );
      },
    });
  }
}
