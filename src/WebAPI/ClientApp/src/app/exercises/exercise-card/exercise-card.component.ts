import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { GetExerciseDto } from 'src/app/web-api-client';
import { SolveExerciseFormComponent } from '../solve-exercise-form/solve-exercise-form.component';

@Component({
  selector: 'app-exercise-card',
  templateUrl: './exercise-card.component.html',
  styleUrls: ['./exercise-card.component.css'],
})
export class ExerciseCardComponent implements OnInit {
  shortenedDescription: string = '';
  buttonText!: string;
  @Input() model: GetExerciseDto = {} as GetExerciseDto;
  constructor(private dialog: MatDialog) {}

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
  }
}
