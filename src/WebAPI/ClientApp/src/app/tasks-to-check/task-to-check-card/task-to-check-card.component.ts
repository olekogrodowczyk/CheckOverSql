import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { GetSolvingDto, SolvingClient } from 'src/app/web-api-client';
import { CheckTaskDialogComponent } from '../check-task-dialog/check-task-dialog.component';

@Component({
  selector: 'app-task-to-check-card',
  templateUrl: './task-to-check-card.component.html',
  styleUrls: ['./task-to-check-card.component.css'],
})
export class TaskToCheckCardComponent implements OnInit {
  shortenedDescription!: string;
  shortenedAnswer!: string;
  @Input() model!: GetSolvingDto;
  constructor(
    private solvingClient: SolvingClient,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.sliceDescription();
    this.sliceAnswer();
  }

  sliceDescription() {
    this.shortenedDescription = this.model.exercise?.description?.slice(
      0,
      100
    )!;
    if (this.model.exercise?.description?.length! > 100) {
      this.shortenedDescription += '...';
    }
  }

  sliceAnswer() {
    this.shortenedAnswer = this.model.solution?.query?.slice(0, 100)!;
    if (this.model.solution?.query?.length! > 100) {
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

  showCheckTaskModal() {
    let innerWidth = window.innerWidth;
    const dialogRef = this.dialog.open(CheckTaskDialogComponent, {
      width: innerWidth > 992 ? '50%' : '75%%',
      data: {
        title: this.model.exercise?.title,
        description: this.model.exercise?.description,
        answer: this.model.solution?.query,
        maxPoints: this.model.maxPoints,
      },
    });
  }
}
