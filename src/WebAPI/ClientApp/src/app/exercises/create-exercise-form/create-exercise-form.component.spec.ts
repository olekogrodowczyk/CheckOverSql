import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateExerciseFormComponent } from './create-exercise-form.component';

describe('CreateExerciseFormComponent', () => {
  let component: CreateExerciseFormComponent;
  let fixture: ComponentFixture<CreateExerciseFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateExerciseFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateExerciseFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
