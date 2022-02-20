import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreateGroupFormComponent } from '../create-group-form/create-group-form.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  constructor(private dialog: MatDialog) {}

  ngOnInit(): void {}

  openCreateGroupDialog() {
    let innerWidth = window.innerWidth;
    const dialogRef = this.dialog.open(CreateGroupFormComponent, {
      width: innerWidth > 992 ? '25%' : '50%',
    });
  }
}
