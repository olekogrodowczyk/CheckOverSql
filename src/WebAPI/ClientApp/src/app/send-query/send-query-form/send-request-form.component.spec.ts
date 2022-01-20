import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendRequestFormComponent } from './send-request-form.component';

describe('SendRequestFormComponent', () => {
  let component: SendRequestFormComponent;
  let fixture: ComponentFixture<SendRequestFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SendRequestFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SendRequestFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
