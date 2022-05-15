import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { GetExerciseDto } from 'src/app/web-api-client';
import { adapter } from './reducer';

export interface State {
  createdExercises: ExercisesState;
  publicExercises: ExercisesState;
  error: string;
  publicExercisesPageNumber: number;
  publicExercisesPageSize: number;
  createdExercisesPageNumber: number;
  createdExercisesPageSize: number;
  canAssign: boolean | undefined;
}

export const initialState: State = {
  createdExercises: {
    ids: [],
    entities: {},
  },
  publicExercises: {
    ids: [],
    entities: {},
  },
  error: '',
  publicExercisesPageNumber: 0,
  publicExercisesPageSize: 8,
  createdExercisesPageNumber: 0,
  createdExercisesPageSize: 8,
  canAssign: undefined,
};

export interface ExercisesState {
  ids: number[];
  entities: { [key: number]: GetExerciseDto };
}
