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
  solveExercise,
  solveExerciseSuccess,
  solveExerciseFailure,
  loadExerciseByIdSuccess,
} from './actions';
import { initialState } from './state';

export const exerciseReducer = createReducer(
  initialState,
  // on(addExercise, (state) => ({
  //   ...state,
  // })),
  // on(addExerciseSuccess, (state) => ({
  //   ...state,
  //   error: '',
  //   createdExercises: adapter.upsertOne(),
  // })),
  // on(addExerciseFailure, (state, { error }) => ({
  //   ...state,
  //   error: error,
  // })),
  on(loadCreatedExercisesSuccess, (state, data) => ({
    ...state,
    error: '',
    createdExercises: adapter.upsertMany(
      data.exercises,
      state.createdExercises
    ),
    createdExercisesPageNumber: data.pageNumber,
    createdExercisesPageSize: data.pageSize,
  })),
  on(loadCreatedExercisesFailure, (state, { error }) => ({
    ...state,
    error: error,
  })),
  on(loadPublicExercisesSuccess, (state, data) => ({
    ...state,
    error: '',
    publicExercises: adapter.setAll(data.exercises, state.publicExercises),
    publicExercisesPageNumber: data.pageNumber,
    publicExercisesPageSize: data.pageSize,
  })),
  on(loadPublicExercisesFailure, (state, { error }) => ({
    ...state,
    error: error,
  })),
  on(loadCanAssignExercisesSuccess, (state, { canAssign }) => ({
    ...state,
    error: '',
    canAssign: canAssign,
  })),
  on(loadCanAssignExercisesFailure, (state, { error }) => ({
    ...state,
    error: error,
  })),
  on(solveExerciseFailure, (state, { error }) => ({
    ...state,
    error: error,
  })),
  on(solveExerciseSuccess, (state, { result }) => {
    return {
      ...state,
      publicExercises: adapter.updateOne(
        { id: result.exerciseId!, changes: { passed: result.result } },
        state.publicExercises
      ),
    };
  }),
  on(loadExerciseByIdSuccess, (state, { result }) => {
    return {
      ...state,
      publicExercises: adapter.upsertOne(result, state.publicExercises),
    };
  })
);

export const adapter: EntityAdapter<GetExerciseDto> =
  createEntityAdapter<GetExerciseDto>();
