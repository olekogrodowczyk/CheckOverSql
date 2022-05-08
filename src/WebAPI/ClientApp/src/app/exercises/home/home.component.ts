import { Component, OnInit } from '@angular/core';
import { GlobalVariables } from 'src/app/global';
import { HeaderService } from 'src/app/shared/services/header.service';
export enum TypeOfExercise {
  Created,
  Public,
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  constructor(private headerService: HeaderService) {}
  getCreatedExercisesEnum: TypeOfExercise = TypeOfExercise.Created;
  getPublicExercisesEnum: TypeOfExercise = TypeOfExercise.Public;
  ngOnInit(): void {
    this.headerService.headerTitle$.next('Exercises');
  }
}
