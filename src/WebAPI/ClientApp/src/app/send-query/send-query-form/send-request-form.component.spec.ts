import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendQueryFormComponent } from './send-request-form.component';

describe('SendRequestFormComponent', () => {
  let component: SendQueryFormComponent;
  let fixture: ComponentFixture<SendQueryFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SendQueryFormComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SendQueryFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
