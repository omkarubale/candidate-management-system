import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyManagerCompanyComponent } from './company-manager-company.component';

describe('CompanyManagerCompanyComponent', () => {
  let component: CompanyManagerCompanyComponent;
  let fixture: ComponentFixture<CompanyManagerCompanyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyManagerCompanyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyManagerCompanyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
