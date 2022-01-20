import { Component, OnInit } from '@angular/core';
import { SendQueryService } from '../send-query.service';

@Component({
  selector: 'app-query-result',
  templateUrl: './query-result.component.html',
  styleUrls: ['./query-result.component.css'],
})
export class QueryResultComponent implements OnInit {
  displayedColumns: string[] = ['1', '2', '3', '4'];
  queryResult: string[][] = [];
  constructor(private sendQueryService: SendQueryService) {}

  ngOnInit(): void {
    this.queryResult = this.sendQueryService.queryResult;
    console.log(this.queryResult);
  }
}
