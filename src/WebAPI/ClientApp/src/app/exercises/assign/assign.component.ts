import { Component, OnInit } from '@angular/core';
import { HeaderService } from 'src/app/shared/header.service';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { GetGroupDto, GroupClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-assign',
  templateUrl: './assign.component.html',
  styleUrls: ['./assign.component.css'],
})
export class AssignComponent implements OnInit {
  groups!: GetGroupDto[];
  constructor(
    private groupClient: GroupClient,
    private snackBar: SnackbarService,
    private headerService: HeaderService
  ) {}

  ngOnInit(): void {
    this.headerService.headerTitle$.next('Assign Exercise');
    this.getGroups();
  }

  getGroups() {
    this.groupClient.getUserGroups().subscribe({
      next: ({ value }) => {
        this.groups = value!;
      },
      error: () => {
        this.snackBar.openSnackBar(
          'Unexpected error occurred while getting groups'
        );
      },
    });
  }
}
