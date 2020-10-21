import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyManagerLoginComponent } from './company-manager-login.component';

describe('CompanyManagerLoginComponent', () => {
  let component: CompanyManagerLoginComponent;
  let fixture: ComponentFixture<CompanyManagerLoginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyManagerLoginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyManagerLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
