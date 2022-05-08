import { createFeatureSelector, createSelector } from '@ngrx/store';
import { GetExerciseDto } from 'src/app/web-api-client';
import { ExerciseState } from './state';

export interface selectExercisesDataModel {
  exercises: GetExerciseDto[];
  pageSize: number;
  pageNumber: number;
}

export const selectCreatedExercises = createSelector(
  createFeatureSelector('exercise'),
  (state: ExerciseState) => {
    return state.createdExercises;
  }
);

export const selectPublicExercises = createSelector(
  createFeatureSelector('exercise'),
  (state: ExerciseState) => {
    return state.publicExercises;
  }
);

export const selectPublicExercisesPageNumber = createSelector(
  createFeatureSelector('exercise'),
  (state: ExerciseState) => {
    return state.publicExercisesPageNumber;
  }
);

export const selectPublicExercisesPageSize = createSelector(
  createFeatureSelector('exercise'),
  (state: ExerciseState) => {
    return state.publicExercisesPageSize;
  }
);

export const selectCreatedExercisesPageNumber = createSelector(
  createFeatureSelector('exercise'),
  (state: ExerciseState) => {
    return state.publicExercisesPageNumber;
  }
);

export const selectCreatedExercisesPageSize = createSelector(
  createFeatureSelector('exercise'),
  (state: ExerciseState) => {
    return state.createdExercisesPageSize;
  }
);

export const selectCanAssignExercises = createSelector(
  createFeatureSelector('exercise'),
  (state: ExerciseState) => state.canAssign
);

export const selectPublicExercisesDataModel = createSelector(
  createFeatureSelector('exercise'),
  (state: ExerciseState) =>
    <selectExercisesDataModel>{
      exercises: state.publicExercises,
      pageSize: state.publicExercisesPageSize,
      pageNumber: state.publicExercisesPageNumber,
    }
);

export const selectCreatedExercisesDataModel = createSelector(
  createFeatureSelector('exercise'),
  (state: ExerciseState) =>
    <selectExercisesDataModel>{
      exercises: state.createdExercises,
      pageSize: state.createdExercisesPageSize,
      pageNumber: state.createdExercisesPageNumber,
    }
);
