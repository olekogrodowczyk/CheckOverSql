import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { Subject } from 'rxjs';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import {
  ExerciseClient,
  GetGroupDto,
  GroupClient,
} from 'src/app/web-api-client';

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.css'],
})
export class GroupsListComponent implements OnInit, OnChanges {
  @Input() groups!: GetGroupDto[];
  @Input() toAssign!: boolean;
  constructor(
    private groupClient: GroupClient,
    private snackBar: SnackbarService
  ) {}

  ngOnInit(): void {}

  ngOnChanges() {
    this.ngOnInit();
  }
}
