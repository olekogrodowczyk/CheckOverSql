import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { GetInvitationDto, InvitationClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-invitation-card',
  templateUrl: './invitation-card.component.html',
  styleUrls: ['./invitation-card.component.css'],
})
export class InvitationCardComponent implements OnInit {
  @Output() onChange: EventEmitter<any> = new EventEmitter();
  @Input() model!: GetInvitationDto;
  constructor(
    private invitationClient: InvitationClient,
    private snackBar: SnackbarService
  ) {}

  ngOnInit(): void {}

  accept() {
    this.invitationClient.accept(this.model.id!).subscribe({
      next: (response) => {
        this.snackBar.openSnackBar('Invitation accepted successfully');
        this.onChange.emit();
      },
      error: ({ message }) => {
        this.snackBar.openSnackBar(message);
      },
    });
  }

  reject() {
    this.invitationClient.reject(this.model.id!).subscribe({
      next: (response) => {
        this.snackBar.openSnackBar('Invitation rejected successfully');
        this.onChange.emit();
      },
      error: ({ message }) => {
        this.snackBar.openSnackBar(message);
      },
    });
  }
}
