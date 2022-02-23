import { Component, Input, OnInit } from '@angular/core';
import { GetGroupDto } from 'src/app/web-api-client';
import { GlobalVariables } from 'src/app/global';
import { MatDialog } from '@angular/material/dialog';
import { AssignDialogComponent } from 'src/app/exercises/assign-dialog/assign-dialog.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-group-card',
  templateUrl: './group-card.component.html',
  styleUrls: ['./group-card.component.css'],
})
export class GroupCardComponent implements OnInit {
  @Input() model!: GetGroupDto;
  @Input() toAssign!: boolean;
  exerciseId: string | null | undefined;
  imagePath!: string;
  constructor(private dialog: MatDialog, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.imagePath = `${GlobalVariables.BASE_API_URL}/${this.model.imagePath}`;
    if (this.toAssign) {
      this.getExerciseIdFromRoute();
    }
  }

  getExerciseIdFromRoute() {
    this.exerciseId = this.route.snapshot.paramMap.get('exerciseId');
  }

  openAssignDialog() {
    let innerWidth = window.innerWidth;
    const dialogRef = this.dialog.open(AssignDialogComponent, {
      data: {
        groupId: this.model.id,
        groupName: this.model.name,
        exerciseId: this.exerciseId,
      },
    });
  }
}
