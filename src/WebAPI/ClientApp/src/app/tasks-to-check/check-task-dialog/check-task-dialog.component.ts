import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-check-task-dialog',
  templateUrl: './check-task-dialog.component.html',
  styleUrls: ['./check-task-dialog.component.css'],
})
export class CheckTaskDialogComponent implements OnInit {
  checkDialogForm = new FormGroup({
    remarks: new FormControl(''),
    points: new FormControl(0, [
      Validators.required,
      Validators.pattern('^[0-9]*$'),
    ]),
  });

  constructor(
    private dialogRef: MatDialogRef<CheckTaskDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { title: string; description: string; answer: string }
  ) {}

  ngOnInit(): void {}

  onSubmit() {}
}
