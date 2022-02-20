import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { FileParameter, GroupClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-create-group-form',
  templateUrl: './create-group-form.component.html',
  styleUrls: ['./create-group-form.component.css'],
})
export class CreateGroupFormComponent implements OnInit {
  srcResult: any;
  image!: File;
  createGroupForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
  });
  constructor(
    private groupClient: GroupClient,
    private snackBar: SnackbarService,
    private dialogRef: MatDialogRef<CreateGroupFormComponent>,
    private http: HttpClient
  ) {}

  ngOnInit(): void {}

  onFileSelected(event: any) {
    this.image = event.target.files[0];
  }

  onSubmit() {
    if (this.createGroupForm.invalid) {
      return;
    }
    this.createGroup();
    this.dialogRef.close();
  }

  createGroup() {
    let fileParameter: FileParameter = {
      data: this.image,
      fileName: this.image.name,
    };
    this.groupClient
      .createGroup(this.createGroupForm.get('name')!.value, fileParameter)
      .subscribe({
        next: () => {
          this.snackBar.openSnackBar('The group has been created successfully');
        },
        error: () => {
          this.snackBar.openSnackBar(
            'Error has occured while creating the group'
          );
        },
      });
  }
}
