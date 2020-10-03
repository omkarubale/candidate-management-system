import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateMyProfileComponent } from './candidate-my-profile.component';

describe('CandidateMyProfileComponent', () => {
  let component: CandidateMyProfileComponent;
  let fixture: ComponentFixture<CandidateMyProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CandidateMyProfileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CandidateMyProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
