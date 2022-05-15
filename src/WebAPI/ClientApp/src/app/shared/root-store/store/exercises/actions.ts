import { Update } from '@ngrx/entity';
import { createAction, props } from '@ngrx/store';
import {
  CreateExerciseCommand,
  CreateSolutionCommand,
  GetComparisonDto,
  GetExerciseDto,
} from 'src/app/web-api-client';

export const addExercise = createAction(
  '[Exercise] Add Exercise',
  props<{ exercise: CreateExerciseCommand }>()
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

export const solveExercise = createAction(
  '[Exercise] Solve Exercise',
  props<{ command: CreateSolutionCommand }>()
);
export const solveExerciseSuccess = createAction(
  '[Exercise] Solve Exercise Success',
  props<{ result: GetComparisonDto }>()
);
export const solveExerciseFailure = createAction(
  '[Exercise] Solve Exercise Failure',
  props<{ error: string }>()
);

export const loadExerciseById = createAction(
  '[Exercise] Load Exercise By Id',
  props<{ exerciseId: number }>()
);

export const loadExerciseByIdSuccess = createAction(
  '[Exercise] Load Exercise By Id Success',
  props<{ result: GetExerciseDto }>()
);

export const loadExerciseByIdFailure = createAction(
  '[Exercise] Load Exercise By Id Failure',
  props<{ error: string }>()
);
