import { Component, Input, OnInit } from '@angular/core';
import { GetGroupDto } from 'src/app/web-api-client';
import { GlobalVariables } from 'src/app/global';

@Component({
  selector: 'app-group-card',
  templateUrl: './group-card.component.html',
  styleUrls: ['./group-card.component.css'],
})
export class GroupCardComponent implements OnInit {
  @Input() model!: GetGroupDto;
  imagePath!: string;
  constructor() {}

  ngOnInit(): void {
    this.imagePath = `${GlobalVariables.BASE_API_URL}/${this.model.ImagePath}`;
  }
}
