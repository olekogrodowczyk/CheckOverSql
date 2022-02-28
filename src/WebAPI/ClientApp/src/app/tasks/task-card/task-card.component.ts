import { Component, Input, OnInit } from '@angular/core';
import { GetSolvingDto, SolvingClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-task-card',
  templateUrl: './task-card.component.html',
  styleUrls: ['./task-card.component.css'],
})
export class TaskCardComponent implements OnInit {
  @Input() model!: GetSolvingDto;
  constructor(private solvingClient: SolvingClient) {}

  ngOnInit(): void {}

  refresh() {
    this.solvingClient.getById(this.model.id!).subscribe({
      next: ({ value }) => {
        this.model = value!;
      },
      error: (response) => {
        console.log(
          'An unexpected error has occurred while getting the solving'
        );
      },
    });
  }
}
