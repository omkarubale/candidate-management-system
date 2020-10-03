import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyManagerDrivesComponent } from './company-manager-drives.component';

describe('CompanyManagerDrivesComponent', () => {
  let component: CompanyManagerDrivesComponent;
  let fixture: ComponentFixture<CompanyManagerDrivesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyManagerDrivesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyManagerDrivesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
