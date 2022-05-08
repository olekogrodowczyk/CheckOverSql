import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  Type,
} from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { select, Store } from '@ngrx/store';
import { filter, map, Observable, tap } from 'rxjs';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import {
  ExerciseClient,
  GetExerciseDto,
  GetExerciseDtoPaginatedList,
} from 'src/app/web-api-client';
import { TypeOfExercise } from '../home/home.component';
import {
  loadCreatedExercises,
  loadPublicExercises,
  loadCanAssignExercises,
} from '../../shared/root-store/store/exercises/store/actions';
import {
  selectCanAssignExercises,
  selectCreatedExercises,
  selectCreatedExercisesDataModel,
  selectExercisesDataModel,
  selectPublicExercises,
  selectPublicExercisesDataModel,
  selectPublicExercisesPageNumber,
  selectPublicExercisesPageSize,
} from '../../shared/root-store/store/exercises/store/selectors';

@Component({
  selector: 'app-exercise-list',
  templateUrl: './exercises-list.component.html',
  styleUrls: ['./exercises-list.component.css'],
})
export class ExerciseListComponent implements OnInit {
  @Input() typeOfExercise!: TypeOfExercise;

  canAssign$ = this.store.select(selectCanAssignExercises);
  exercisesData$: Observable<selectExercisesDataModel> | undefined;

  constructor(private store: Store) {}

  ngOnInit(): void {
    console.log(this.typeOfExercise.toString());
    this.store.dispatch(loadCanAssignExercises());
    this.select();
    this.refresh();
  }

  onPageChange(event: PageEvent) {
    this.getExercises(this.typeOfExercise, event.pageIndex, event.pageSize);
  }

  refresh(): void {
    let pageSize: number = 8;
    let pageNumber: number = 0;

    this.exercisesData$?.subscribe((x) => (pageSize = x.pageSize));
    this.exercisesData$?.subscribe((x) => (pageNumber = x.pageNumber));

    this.getExercises(this.typeOfExercise, pageNumber, pageSize);
  }

  select(): void {
    switch (this.typeOfExercise) {
      case TypeOfExercise.Created:
        this.exercisesData$ = this.store.select(
          selectCreatedExercisesDataModel
        );
        break;
      case TypeOfExercise.Public:
        this.exercisesData$ = this.store.select(selectPublicExercisesDataModel);
        break;
    }
  }

  getExercises(
    typeOfExercise: TypeOfExercise,
    pageNumber: number,
    pageSize: number
  ) {
    switch (typeOfExercise) {
      case TypeOfExercise.Created: {
        console.log('Created');

        this.store.dispatch(
          loadCreatedExercises({ pageNumber: pageNumber, pageSize: pageSize })
        );
        this.exercisesData$ = this.store.select(
          selectCreatedExercisesDataModel
        );
        break;
      }
      case TypeOfExercise.Public: {
        console.log('Public');

        this.store.dispatch(
          loadPublicExercises({ pageNumber: pageNumber, pageSize: pageSize })
        );
        this.exercisesData$ = this.store.select(selectPublicExercisesDataModel);
        break;
      }
    }
  }
}
