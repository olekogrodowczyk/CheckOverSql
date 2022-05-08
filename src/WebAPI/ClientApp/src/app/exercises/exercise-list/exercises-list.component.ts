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
  loadExerciseById,
} from '../../shared/root-store/store/exercises/store/actions';
import {
  selectCanAssignExercises,
  selectCreatedExercisesDataModel,
  selectExercisesDataModel,
  selectPublicExercisesDataModel,
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
    this.store.dispatch(loadCanAssignExercises());
    this.selectExercises();
    this.refreshExercises();
  }

  onPageChange(event: PageEvent) {
    this.loadExercises(this.typeOfExercise, event.pageIndex, event.pageSize);
  }

  refreshExercises(): void {
    let pageSize: number = 8;
    let pageNumber: number = 0;

    this.exercisesData$?.subscribe((x) => (pageSize = x.pageSize));
    this.exercisesData$?.subscribe((x) => (pageNumber = x.pageNumber));

    this.loadExercises(this.typeOfExercise, pageNumber, pageSize);
  }

  selectExercises(): void {
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

  loadExercises(
    typeOfExercise: TypeOfExercise,
    pageNumber: number,
    pageSize: number
  ) {
    switch (typeOfExercise) {
      case TypeOfExercise.Created: {
        this.store.dispatch(
          loadCreatedExercises({ pageNumber: pageNumber, pageSize: pageSize })
        );
        this.exercisesData$ = this.store.select(
          selectCreatedExercisesDataModel
        );
        break;
      }
      case TypeOfExercise.Public: {
        this.store.dispatch(
          loadPublicExercises({ pageNumber: pageNumber, pageSize: pageSize })
        );
        this.exercisesData$ = this.store.select(selectPublicExercisesDataModel);
        break;
      }
    }
  }
}
