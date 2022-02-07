import { Component, Input, OnInit } from '@angular/core';
import { GetQueryValueQuery, QueryDto } from 'src/app/web-api-client';
import { SendQueryService } from '../send-query.service';

@Component({
  selector: 'app-query-card',
  templateUrl: './query-card.component.html',
  styleUrls: ['./query-card.component.css'],
})
export class QueryCardComponent implements OnInit {
  @Input() queryModel!: QueryDto;
  constructor(private sendQueryService: SendQueryService) {}

  ngOnInit(): void {}

  executeQuery() {
    this.sendQueryService.sendQuery(<GetQueryValueQuery>{
      query: this.queryModel.queryValue,
      databaseName: this.queryModel.databaseName,
    });
  }
}
