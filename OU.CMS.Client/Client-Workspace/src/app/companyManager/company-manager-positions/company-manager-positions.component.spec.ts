import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyManagerPositionsComponent } from './company-manager-positions.component';

describe('CompanyManagerPositionsComponent', () => {
  let component: CompanyManagerPositionsComponent;
  let fixture: ComponentFixture<CompanyManagerPositionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyManagerPositionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyManagerPositionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
