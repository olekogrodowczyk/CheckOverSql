import { Component, OnInit } from '@angular/core';

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
  constructor() {}
  getCreatedExercisesEnum: TypeOfExercise = TypeOfExercise.Created;
  getPublicExercisesEnum: TypeOfExercise = TypeOfExercise.Public;
  ngOnInit(): void {}
}
