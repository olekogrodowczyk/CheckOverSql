import { createAction, props } from '@ngrx/store';
import { CreateExerciseCommand, GetExerciseDto } from 'src/app/web-api-client';

export const addExercise = createAction(
  '[Exercise] Add Exercise',
  props<{ exercise: GetExerciseDto }>()
);

export const addExerciseSuccess = createAction(
  '[Exercise] Add Exercise Success'
);

export const addExerciseFailure = createAction(
  '[Exercise] Add Exercise Success',
  props<{ error: string }>()
);

export const loadCreatedExercises = createAction(
  '[Exercise] Load Created Exercises',
  props<{ pageNumber: number; pageSize: number }>()
);

export const loadCreatedExercisesSuccess = createAction(
  '[Exercise] Load Created Exercises Success',
  props<{ exercises: GetExerciseDto[]; pageNumber: number; pageSize: number }>()
);

export const loadCreatedExercisesFailure = createAction(
  '[Exercise] Load Created Exercises Failure',
  props<{ error: string }>()
);

export const loadPublicExercises = createAction(
  '[Exercise] Load Public Exercises',
  props<{ pageNumber: number; pageSize: number }>()
);

export const loadPublicExercisesSuccess = createAction(
  '[Exercise] Load Public Exercises Success',
  props<{ exercises: GetExerciseDto[]; pageNumber: number; pageSize: number }>()
);

export const loadPublicExercisesFailure = createAction(
  '[Exercise] Load Public Exercises Failure',
  props<{ error: string }>()
);

export const loadCanAssignExercises = createAction(
  '[Exercise] Load Can Assign Exercise'
);

export const loadCanAssignExercisesSuccess = createAction(
  '[Exercise] Load Can Assign Exercises Success',
  props<{ canAssign: boolean }>()
);

export const loadCanAssignExercisesFailure = createAction(
  '[Exercise] Load Can Assign Exercises Failure',
  props<{ error: string }>()
);
