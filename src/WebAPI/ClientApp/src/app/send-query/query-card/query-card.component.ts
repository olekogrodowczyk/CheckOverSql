import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GetQueryValueQuery, QueryDto } from 'src/app/web-api-client';
import { SendQueryService } from '../send-query.service';

@Component({
  selector: 'app-query-card',
  templateUrl: './query-card.component.html',
  styleUrls: ['./query-card.component.css'],
})
export class QueryCardComponent implements OnInit {
  @Input() queryModel!: QueryDto;
  constructor(
    private sendQueryService: SendQueryService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  executeQuery() {
    this.sendQueryService.model = <GetQueryValueQuery>{
      databaseName: this.queryModel.databaseName,
      query: this.queryModel.queryValue,
    };
    this.router.navigateByUrl('send-query/query-result');
  }
}
