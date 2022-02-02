import { Component, Input, OnInit } from '@angular/core';
import { GetExerciseDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-exercise-card',
  templateUrl: './exercise-card.component.html',
  styleUrls: ['./exercise-card.component.css'],
})
export class ExerciseCardComponent implements OnInit {
  shortenedDescription: string = '';
  @Input() model: GetExerciseDto = {} as GetExerciseDto;
  constructor() {}

  ngOnInit(): void {
    this.shortenedDescription = this.model.description?.slice(0, 125)!;
    if (this.model.description?.length! > 85) {
      this.shortenedDescription += '...';
    }
  }
}
