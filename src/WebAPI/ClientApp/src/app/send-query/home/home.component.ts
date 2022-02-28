import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { HeaderService } from 'src/app/shared/header.service';
import { SendQueryFormComponent } from '../send-query-form/send-request-form.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  constructor(
    private dialog: MatDialog,
    private headerService: HeaderService
  ) {}

  ngOnInit(): void {
    this.headerService.headerTitle$.next('Execute Query');
  }

  openQueryDialog() {
    let innerWidth = window.innerWidth;
    const dialogRef = this.dialog.open(SendQueryFormComponent, {
      width: innerWidth > 992 ? '75%' : '100%',
    });
  }
}
