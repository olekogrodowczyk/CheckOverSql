import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import {
  ExerciseClient,
  GetGroupDto,
  GroupClient,
} from 'src/app/web-api-client';
import { CreateGroupFormComponent } from '../create-group-form/create-group-form.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  constructor(private router: Router) {
    this.router.navigateByUrl('groups/(side:groups)');
  }
  refreshSubject: Subject<boolean> = new Subject<boolean>();

  ngOnInit(): void {}
}
