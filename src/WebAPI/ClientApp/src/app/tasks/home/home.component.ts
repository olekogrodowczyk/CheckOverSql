import { Component, OnInit } from '@angular/core';
import {
  TasksListComponent,
  TaskStatus,
} from '../tasks-list/tasks-list.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  taskStatusToDo: TaskStatus = TaskStatus.ToDo;
  taskStatusDone: TaskStatus = TaskStatus.Done;
  taskStatusOverdue: TaskStatus = TaskStatus.Overdue;
  taskStatusDoneButOverdue: TaskStatus = TaskStatus.DoneButOverdue;
  taskStatusChecked: TaskStatus = TaskStatus.Checked;
  constructor() {}

  ngOnInit(): void {}
}
