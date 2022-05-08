import { state } from '@angular/animations';
import { createEntityAdapter, EntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { CreateExerciseCommand, GetExerciseDto } from 'src/app/web-api-client';
import {
  addExercise,
  addExerciseFailure,
  addExerciseSuccess,
  loadCreatedExercises as loadCreatedExercises,
  loadCreatedExercisesFailure as loadCreatedExercisesFailure,
  loadCreatedExercisesSuccess as loadCreatedExercisesSuccess,
  loadPublicExercises as loadPublicExercises,
  loadPublicExercisesFailure as loadPublicExercisesFailure,
  loadPublicExercisesSuccess as loadPublicExercisesSuccess,
  loadCanAssignExercises,
  loadCanAssignExercisesFailure,
  loadCanAssignExercisesSuccess,
} from './actions';
import { initialState } from './state';

export const exerciseReducer = createReducer(
  initialState,
  on(addExercise, (state) => ({
    ...state,
  })),
  on(addExerciseSuccess, (state) => ({
    ...state,
    error: '',
    createdExercises: [...state.createdExercises],
  })),
  on(addExerciseFailure, (state, { error }) => ({
    ...state,
    error: error,
  })),
  on(loadCreatedExercises, (state) => ({
    ...state,
  })),
  on(loadCreatedExercisesSuccess, (state, data) => ({
    ...state,
    error: '',
    createdExercises: data.exercises,
    createdExercisesPageNumber: data.pageNumber,
    createdExercisesPageSize: data.pageSize,
  })),
  on(loadCreatedExercisesFailure, (state, { error }) => ({
    ...state,
    error: error,
  })),
  on(loadPublicExercises, (state) => ({
    ...state,
  })),
  on(loadPublicExercisesSuccess, (state, data) => ({
    ...state,
    error: '',
    publicExercises: data.exercises,
    publicExercisesPageNumber: data.pageNumber,
    publicExercisesPageSize: data.pageSize,
  })),
  on(loadPublicExercisesFailure, (state, { error }) => ({
    ...state,
    error: error,
  })),
  on(loadCanAssignExercises, (state) => ({
    ...state,
  })),
  on(loadCanAssignExercisesSuccess, (state, { canAssign }) => ({
    ...state,
    error: '',
    canAssign: true,
  })),
  on(loadCanAssignExercisesFailure, (state, { error }) => ({
    ...state,
    error: error,
  }))
);
