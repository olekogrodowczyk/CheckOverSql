import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import {
  CreateInvitationCommand,
  GroupRoleClient,
  GroupRoleDto,
  InvitationClient,
} from 'src/app/web-api-client';

@Component({
  selector: 'app-make-invitation-form',
  templateUrl: './make-invitation-form.component.html',
  styleUrls: ['./make-invitation-form.component.css'],
})
export class MakeInvitationFormComponent implements OnInit {
  groupRoles!: GroupRoleDto[];
  selectedGroupRole!: string;
  invitationForm = new FormGroup({
    receiverEmail: new FormControl('', [Validators.required]),
    roleName: new FormControl('', [Validators.required]),
  });
  constructor(
    private groupRoleClient: GroupRoleClient,
    private snackBar: SnackbarService,
    private invitationClient: InvitationClient,
    private dialogRef: MatDialogRef<MakeInvitationFormComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { groupId: number }
  ) {}

  ngOnInit(): void {
    this.getAllGroupRoles();
  }

  getAllGroupRoles() {
    this.groupRoleClient.getAll().subscribe({
      next: ({ value }) => {
        this.groupRoles = value!;
      },
      error: () => {
        this.snackBar.openSnackBar(
          'Unexpected error occurred while getting roles'
        );
      },
    });
  }

  onSubmit() {
    if (this.invitationForm.invalid) {
      return;
    }
    console.log(this.invitationForm.value);

    this.sendInvitation();
    this.dialogRef.close();
  }

  sendInvitation() {
    this.invitationClient
      .createInvitation(<CreateInvitationCommand>{
        receiverEmail: this.invitationForm.get('receiverEmail')?.value,
        roleName: this.invitationForm.get('roleName')?.value,
        groupId: this.data.groupId,
      })
      .subscribe({
        next: (response) => {
          this.snackBar.openSnackBar(
            'The invitation has been sent successfully'
          );
        },
        error: ({ errors }) => {
          this.snackBar.openSnackBar(errors[Object.keys(errors)[0]]);
        },
      });
  }
}
