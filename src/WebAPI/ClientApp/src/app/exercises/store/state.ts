import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { GetExerciseDto } from 'src/app/web-api-client';

export interface ExerciseState {
  createdExercises: GetExerciseDto[];
  publicExercises: GetExerciseDto[];
  error: string;
  publicExercisesPageNumber: number;
  publicExercisesPageSize: number;
  createdExercisesPageNumber: number;
  createdExercisesPageSize: number;
  canAssign: boolean | undefined;
}

export const initialState: ExerciseState = {
  createdExercises: [] as GetExerciseDto[],
  publicExercises: [] as GetExerciseDto[],
  error: '',
  publicExercisesPageNumber: 0,
  publicExercisesPageSize: 8,
  createdExercisesPageNumber: 0,
  createdExercisesPageSize: 8,
  canAssign: undefined,
};
