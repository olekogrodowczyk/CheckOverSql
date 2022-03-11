import { Component, Input, OnInit } from '@angular/core';
import { GetSolvingDto, SolvingClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-task-to-check-card',
  templateUrl: './task-to-check-card.component.html',
  styleUrls: ['./task-to-check-card.component.css'],
})
export class TaskToCheckCardComponent implements OnInit {
  shortenedDescription!: string;
  shortenedAnswer!: string;
  @Input() model!: GetSolvingDto;
  constructor(private solvingClient: SolvingClient) {}

  ngOnInit(): void {
    this.sliceDescription();
    this.sliceAnswer();
  }

  sliceDescription() {
    this.shortenedDescription = this.model.exercise?.description?.slice(0, 50)!;
    if (this.model.exercise?.description?.length! > 50) {
      this.shortenedDescription += '...';
    }
  }

  sliceAnswer() {
    this.shortenedAnswer = this.model.solution?.query?.slice(0, 50)!;
    if (this.model.solution?.query?.length! > 50) {
      this.shortenedAnswer += '...';
    }
  }

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
