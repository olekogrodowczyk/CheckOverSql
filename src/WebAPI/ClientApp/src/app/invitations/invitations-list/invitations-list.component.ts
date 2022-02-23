import { Component, OnInit } from '@angular/core';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { GetInvitationDto, InvitationClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-invitations-list',
  templateUrl: './invitations-list.component.html',
  styleUrls: ['./invitations-list.component.css'],
})
export class InvitationsListComponent implements OnInit {
  invitations!: GetInvitationDto[];
  constructor(
    private invitationClient: InvitationClient,
    private snackBar: SnackbarService
  ) {}

  ngOnInit(): void {
    this.getAllInvitations();
  }

  refresh() {
    this.ngOnInit();
  }

  getAllInvitations() {
    this.invitationClient.getAll('Received').subscribe({
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
