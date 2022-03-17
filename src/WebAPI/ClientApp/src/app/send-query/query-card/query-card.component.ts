import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { GetQueryValueQuery, QueryDto } from 'src/app/web-api-client';
import { SendQueryFormComponent } from '../send-query-form/send-request-form.component';
import { SendQueryService } from '../send-query.service';

@Component({
  selector: 'app-query-card',
  templateUrl: './query-card.component.html',
  styleUrls: ['./query-card.component.css'],
})
export class QueryCardComponent implements OnInit {
  @Input() queryModel!: QueryDto;
  shortenedQuery!: string;
  showQueryButton: boolean = false;

  constructor(
    private sendQueryService: SendQueryService,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.sliceQueryIfTooLong();
  }

  sliceQueryIfTooLong() {
    this.shortenedQuery = this.queryModel.queryValue?.slice(0, 200)!;
    if (this.queryModel.queryValue?.length! > 200) {
      this.shortenedQuery += '...';
      this.showQueryButton = true;
    }
  }

  executeQuery() {
    this.sendQueryService.model = <GetQueryValueQuery>{
      databaseName: this.queryModel.databaseName,
      query: this.queryModel.queryValue,
    };
    this.router.navigateByUrl('send-query/query-result');
  }

  openShowQueryDialog() {
    let innerWidth = window.innerWidth;
    const dialogRef = this.dialog.open(SendQueryFormComponent, {
      width: innerWidth > 992 ? '50%' : '75%',
      data: {
        databaseName: this.queryModel.databaseName,
        query: this.queryModel.queryValue,
      },
    });
  }
}
