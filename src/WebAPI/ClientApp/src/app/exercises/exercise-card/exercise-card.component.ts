import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { GetExerciseDto, GroupClient, QueryDto } from 'src/app/web-api-client';
import { ShowSolutionDialogComponent } from '../show-solution-dialog/show-solution-dialog.component';
import { SolveExerciseFormComponent } from '../solve-exercise-form/solve-exercise-form.component';

@Component({
  selector: 'app-exercise-card',
  templateUrl: './exercise-card.component.html',
  styleUrls: ['./exercise-card.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExerciseCardComponent implements OnInit {
  shortenedDescription: string = '';
  buttonText!: string;

  @Input() canAssign: boolean | undefined;
  @Input() model: GetExerciseDto = {} as GetExerciseDto;

  @Output() onSolve = new EventEmitter();
  constructor(
    public dialog: MatDialog,
    private groupClient: GroupClient,
    private snackBar: SnackbarService
  ) {}

  ngOnInit(): void {
    this.sliceDescription();
    this.buttonText = this.model.passed ? 'SOLVED' : 'SOLVE';
  }

  sliceDescription() {
    this.shortenedDescription = this.model.description?.slice(0, 200)!;
    if (this.model.description?.length! > 85) {
      this.shortenedDescription += '...';
    }
  }

  getAllAssignableGroups() {
    this.groupClient.getAllAssignableGroups().subscribe({
      next: ({ value }) => {
        console.log(value);
      },
      error: () => {
        this.snackBar.openSnackBar(
          'Unexpected error has occurred while getting groups'
        );
      },
    });
  }

  openSolveDialog() {
    let innerWidth = window.innerWidth;
    const dialogRef = this.dialog.open(SolveExerciseFormComponent, {
      width: innerWidth > 992 ? '75%' : '100%',
      data: {
        exerciseId: this.model.id,
        title: this.model.title,
        description: this.model.description,
      },
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.onSolve.emit();
      }
    });
  }

  openLastQueryDialog() {
    const dialogRef = this.dialog.open(ShowSolutionDialogComponent, {
      width: innerWidth > 992 ? '40%' : '75%',
      data: {
        exerciseId: this.model.id,
        databaseName: this.model.databaseName,
      },
    });
  }
}
