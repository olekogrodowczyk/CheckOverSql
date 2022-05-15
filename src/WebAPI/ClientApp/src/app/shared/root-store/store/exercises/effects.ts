import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import {
  catchError,
  concatMap,
  EMPTY,
  exhaustMap,
  filter,
  from,
  map,
  mergeMap,
  Observable,
  of,
  OperatorFunction,
  pipe,
  pluck,
  switchMap,
  tap,
  UnaryFunction,
  withLatestFrom,
} from 'rxjs';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { AppState } from 'src/app/store/app.state';
import {
  CreateExerciseCommand,
  ExerciseClient,
  SolutionClient,
} from 'src/app/web-api-client';
import {
  loadCreatedExercises,
  loadCreatedExercisesSuccess,
  loadCreatedExercisesFailure,
  addExercise,
  addExerciseSuccess,
  loadPublicExercisesSuccess,
  loadPublicExercises,
  addExerciseFailure,
  loadCanAssignExercises,
  loadCanAssignExercisesSuccess,
  solveExercise,
  loadExerciseById,
  loadExerciseByIdSuccess,
  loadExerciseByIdFailure,
  solveExerciseFailure,
  solveExerciseSuccess,
} from './actions';
import { selectCanAssignExercises } from './selectors';

function filterNullish<T>(): UnaryFunction<
  Observable<T | null | undefined>,
  Observable<T>
> {
  return pipe(
    filter((x) => x != null) as OperatorFunction<T | null | undefined, T>
  );
}

@Injectable()
export class ExerciseEffects {
  constructor(
    private actions$: Actions,
    private store: Store<AppState>,
    private exerciseClient: ExerciseClient,
    private solutionClient: SolutionClient,
    private snackBar: SnackbarService
  ) {}

  addExerciseEffect$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(addExercise),
      switchMap(({ exercise }) => {
        return this.exerciseClient.createExercise(exercise).pipe(
          pluck('value'),
          mergeMap((exerciseId) => {
            return [
              addExerciseSuccess(),
              loadExerciseById({ exerciseId: exerciseId! }),
            ];
          }),
          tap(() =>
            this.snackBar.openSnackBar(
              'The exercise has been added successfully'
            )
          ),
          catchError((error) => {
            this.snackBar.openErrorSnackBar(error);
            return of(addExerciseFailure({ error }));
          })
        );
      })
    );
  });

  getCreatedExercisesEffect$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(loadCreatedExercises),
      exhaustMap((data) =>
        this.exerciseClient
          .getAllCreated(data.pageNumber + 1, data.pageSize)
          .pipe(
            pluck('value'),
            map((paginatedList) => paginatedList?.items),
            filterNullish(),
            map((exercises) =>
              loadCreatedExercisesSuccess({
                exercises: exercises,
                pageNumber: data.pageNumber,
                pageSize: data.pageSize,
              })
            ),
            catchError((error) => {
              this.snackBar.openErrorSnackBar(error);
              return of(loadCreatedExercisesFailure(error));
            })
          )
      )
    );
  });

  getPublicExercisesEffect$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(loadPublicExercises),
      exhaustMap((data) =>
        this.exerciseClient
          .getAllPublic(data.pageNumber + 1, data.pageSize)
          .pipe(
            pluck('value'),
            map((paginatedList) => paginatedList?.items),
            filterNullish(),
            map((exercises) =>
              loadPublicExercisesSuccess({
                exercises: exercises,
                pageNumber: data.pageNumber,
                pageSize: data.pageSize,
              })
            ),
            catchError((error) => {
              this.snackBar.openErrorSnackBar(error);
              return of(loadCreatedExercisesFailure(error));
            })
          )
      )
    );
  });

  loadCanAssignExercisesEffect$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(loadCanAssignExercises),
      withLatestFrom(this.store.select(selectCanAssignExercises)),
      filter(([action, canAssign]) => canAssign === undefined),
      exhaustMap(() => {
        return this.exerciseClient.checkIfUserCanAssignExercise().pipe(
          pluck('value'),
          filterNullish(),
          mergeMap((canAssign) => [
            loadCanAssignExercisesSuccess({
              canAssign: canAssign,
            }),
          ]),
          catchError((error) => {
            this.snackBar.openErrorSnackBar(error);
            return of(loadCreatedExercisesFailure(error));
          })
        );
      })
    );
  });

  loadExerciseByIdEffect$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(loadExerciseById),
      exhaustMap(({ exerciseId }) => {
        return this.exerciseClient.getById(exerciseId).pipe(
          pluck('value'),
          filterNullish(),
          map((exercise) => loadExerciseByIdSuccess({ result: exercise })),
          catchError((error) => {
            this.snackBar.openErrorSnackBar(error);
            return of(loadExerciseByIdFailure(error));
          })
        );
      })
    );
  });

  solveExerciseEffect$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(solveExercise),
      exhaustMap(({ command }) => {
        return this.solutionClient.createSolution(command).pipe(
          pluck('value'),
          filterNullish(),
          mergeMap((response) => {
            if (response.result) {
              this.snackBar.openSnackBar("You've passed the exercise");
            } else {
              this.snackBar.openSnackBar("You haven't passed the exercise");
            }
            return [
              loadExerciseById({ exerciseId: response.exerciseId! }),
              solveExerciseSuccess({ result: response }),
            ];
          }),
          catchError((error) => {
            this.snackBar.openErrorSnackBar(error);
            return of(solveExerciseFailure(error));
          })
        );
      })
    );
  });
}
