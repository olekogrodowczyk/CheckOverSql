import { Component, OnInit } from '@angular/core';
import { HeaderService } from 'src/app/shared/header.service';

@Component({
  selector: 'app-invitations',
  templateUrl: './invitations.component.html',
  styleUrls: ['./invitations.component.css'],
})
export class InvitationsComponent implements OnInit {
  constructor(private headerService: HeaderService) {}

  ngOnInit(): void {
    this.headerService.headerTitle$.next('Invitations');
  }
}
