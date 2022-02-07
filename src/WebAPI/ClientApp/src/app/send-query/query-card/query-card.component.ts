import { Component, Input, OnInit } from '@angular/core';
import { QueryHistoryDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-query-card',
  templateUrl: './query-card.component.html',
  styleUrls: ['./query-card.component.css'],
})
export class QueryCardComponent implements OnInit {
  @Input() query!: QueryHistoryDto;
  constructor() {}

  ngOnInit(): void {}
}
