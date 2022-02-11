import { Component, OnInit } from '@angular/core';
import { SendQueryService } from '../send-query.service';

@Component({
  selector: 'app-query-result',
  templateUrl: './query-result.component.html',
  styleUrls: ['./query-result.component.css'],
})
export class QueryResultComponent implements OnInit {
  columnNames: string[] = [];
  queryResult: string[][] = [];
  dataSource: object[] = [];
  constructor(private sendQueryService: SendQueryService) {}

  ngOnInit() {
    this.queryResult = this.sendQueryService.queryResult;
    this.assignColumnNames();
    this.dataSource = this.getData();
  }

  getData() {
    let data: object[] = [];
    for (let i = 1; i < this.queryResult.length; i++) {
      let object: any = {};
      for (let j = 0; j < this.columnNames.length; j++) {
        object[this.columnNames[j]] = this.queryResult[i][j];
      }
      data.push(object);
    }
    return data;
  }

  assignColumnNames() {
    this.columnNames = this.queryResult[0];
  }
}
