import { createFeatureSelector, createSelector } from '@ngrx/store';
import { GetExerciseDto } from 'src/app/web-api-client';
import { State } from './state';

export interface selectExercisesDataModel {
  exercises: GetExerciseDto[];
  pageSize: number;
  pageNumber: number;
}

export const selectCanAssignExercises = createSelector(
  createFeatureSelector('exercise'),
  (state: State) => state.canAssign
);

export const selectPublicExercisesDataModel = createSelector(
  createFeatureSelector('exercise'),
  (state: State) =>
    <selectExercisesDataModel>{
      exercises: Object.values(state.publicExercises.entities),
      pageSize: state.publicExercisesPageSize,
      pageNumber: state.publicExercisesPageNumber,
    }
);

export const selectCreatedExercisesDataModel = createSelector(
  createFeatureSelector('exercise'),
  (state: State) =>
    <selectExercisesDataModel>{
      exercises: Object.values(state.createdExercises.entities),
      pageSize: state.createdExercisesPageSize,
      pageNumber: state.createdExercisesPageNumber,
    }
);
