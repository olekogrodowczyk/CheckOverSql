import { Component, OnInit } from '@angular/core';
import { ResolveEnd, ResolveStart, Router } from '@angular/router';
import { filter, mapTo, Observable } from 'rxjs';
import { merge } from 'rxjs';
import { HeaderService } from './shared/services/header.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  headerTitle!: string;
  title = 'ClientApp';
  isLoading$!: Observable<boolean>;
  private _showLoaderEvents$!: Observable<boolean>;
  private _hideLoaderEvents$!: Observable<boolean>;
  constructor(private router: Router, public headerService: HeaderService) {}

  ngOnInit(): void {
    this._showLoaderEvents$ = this.router.events.pipe(
      filter((e) => e instanceof ResolveStart),
      mapTo(true)
    );
    this._hideLoaderEvents$ = this.router.events.pipe(
      filter((e) => e instanceof ResolveEnd),
      mapTo(false)
    );
    this.isLoading$ = merge(this._hideLoaderEvents$, this._showLoaderEvents$);
  }
}
