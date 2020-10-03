import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyManagerAssessmentsComponent } from './company-manager-assessments.component';

describe('CompanyManagerAssessmentsComponent', () => {
  let component: CompanyManagerAssessmentsComponent;
  let fixture: ComponentFixture<CompanyManagerAssessmentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyManagerAssessmentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyManagerAssessmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
