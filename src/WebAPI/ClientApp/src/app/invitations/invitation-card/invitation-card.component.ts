import { Component, Input, OnInit } from '@angular/core';
import { GetInvitationDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-invitation-card',
  templateUrl: './invitation-card.component.html',
  styleUrls: ['./invitation-card.component.css'],
})
export class InvitationCardComponent implements OnInit {
  @Input() model!: GetInvitationDto;
  constructor() {}

  ngOnInit(): void {}
}
