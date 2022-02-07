import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SendQueryFormComponent } from '../send-query-form/send-request-form.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  constructor(private dialog: MatDialog) {}

  ngOnInit(): void {}

  openQueryDialog() {
    let innerWidth = window.innerWidth;
    const dialogRef = this.dialog.open(SendQueryFormComponent, {
      width: innerWidth > 992 ? '75%' : '100%',
    });
  }
}
